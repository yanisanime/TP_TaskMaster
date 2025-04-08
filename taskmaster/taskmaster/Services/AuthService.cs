namespace taskmaster.Services;

public class AuthService
{
    public async Task<bool> LoginAsync(string email, string password)
    {
        // Pour l'instant, un mock simple
        await Task.Delay(1000); // Simule un appel réseau

        return !string.IsNullOrEmpty(email) && password?.Length > 5;
    }
}