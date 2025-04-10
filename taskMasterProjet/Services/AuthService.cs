using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;

namespace taskMasterProjet.Services;

public class AuthService
{
    private readonly AppDbContext _context;

    public AuthService(AppDbContext context)
    {
        _context = context;
    }

    // Enregistre un nouvel utilisateur dans la base de données
    public async Task<bool> RegisterAsync(string nom, string prenom, string email, string password)
    {
        try
        {

            // Vérifie la connexion d'abord
            if (!await _context.Database.CanConnectAsync())
            {
                await Shell.Current.DisplayAlert("Erreur",
                    "Impossible de se connecter à la base de données. Vérifiez que MySQL est bien démarré.",
                    "OK");
                return false;
            }


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
        catch (Exception ex)
        {
            // Affiche une alerte à l'utilisateur
            var mainPage = Application.Current?.Windows.FirstOrDefault()?.Page;
            if (mainPage != null)
            {
                await mainPage.DisplayAlert("Erreur", "Message d'erreur", "OK");
            }

            // Affiche un message d'erreur ou log l'exception
            Console.WriteLine($"Erreur lors de l'enregistrement : {ex.Message}");

            // Vous pouvez également lever une exception personnalisée ou retourner une valeur spécifique
            return false;
        }
    }


    // Authentifie l'utilisateur avec l'email et le mot de passe fournis
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

    // Vérifie si la connexion à la base de données fonctionne
    public async Task TestConnection()
    {
        try
        {
            // Teste une requête simple
            var userCount = await _context.Utilisateurs.CountAsync();
            Console.WriteLine($"Connexion réussie! {userCount} utilisateurs trouvés.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur de connexion: {ex.Message}");
            throw;
        }
    }

    // Vérifie si l'utilisateur existe déjà dans la base de données
    public async Task<bool> UserExistsAsync(string email)
    {
        return await _context.Utilisateurs.AnyAsync(u => u.Email == email);
    }

    // Vérifie si la base de données est accessible
    public async Task<bool> CanConnectToDatabase()
    {
        try
        {
            return await _context.Database.CanConnectAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Database connection failed: {ex}");
            return false;
        }
    }
}
