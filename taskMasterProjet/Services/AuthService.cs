using Microsoft.EntityFrameworkCore;
using System;

namespace taskMasterProjet.Services;

public class AuthService
{
    private readonly AppDbContext _context;

    public AuthService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> RegisterAsync(string nom, string prenom, string email, string password)
    {
        // Vérifie si un utilisateur avec cet email existe déjà
        if (await _context.Utilisateurs.AnyAsync(u => u.Email == email))
            return false;

        // Crée un nouvel utilisateur avec le mot de passe en clair
        var user = new Utilisateur
        {
            Nom = nom,
            Prenom = prenom,
            Email = email,
            MotDePasse = password // Stocke le mot de passe en clair
        };

        _context.Utilisateurs.Add(user);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Utilisateur> LoginAsync(string email, string password)
    {
        // Récupère l'utilisateur avec l'email fourni
        var user = await _context.Utilisateurs.FirstOrDefaultAsync(u => u.Email == email);

        // Vérifie si l'utilisateur existe et si le mot de passe correspond
        if (user != null && user.MotDePasse == password)
            return user;
        else
            return null;

    }
}
