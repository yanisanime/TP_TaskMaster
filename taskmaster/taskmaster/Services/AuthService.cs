using Microsoft.EntityFrameworkCore;
using taskmaster.Models;

namespace taskmaster.Services;

public class AuthService
{
    private readonly AppDbContext _context;
    private readonly IPasswordHasher _passwordHasher;

    public AuthService(AppDbContext context, IPasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<bool> RegisterAsync(string nom, string prenom, string email, string password)
    {
        if (await _context.Utilisateurs.AnyAsync(u => u.Email == email))
            return false;

        var user = new Utilisateur
        {
            Nom = nom,
            Prenom = prenom,
            Email = email,
            MotDePasse = _passwordHasher.HashPassword(password)
        };

        _context.Utilisateurs.Add(user);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Utilisateur?> LoginAsync(string email, string password)
    {
        var user = await _context.Utilisateurs.FirstOrDefaultAsync(u => u.Email == email);

        if (user == null || !_passwordHasher.VerifyPassword(user.MotDePasse, password))
            return null;

        return user;
    }
}

public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyPassword(string hashedPassword, string providedPassword);
}

public class BCryptPasswordHasher : IPasswordHasher
{
    public string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password);
    public bool VerifyPassword(string hashedPassword, string providedPassword)
        => BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword);
}