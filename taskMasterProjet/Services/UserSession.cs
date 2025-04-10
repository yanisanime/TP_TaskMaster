

namespace taskMasterProjet.Services;


public class UserSession
{
    public Utilisateur? CurrentUser { get; private set; }
    public bool IsLoggedIn => CurrentUser != null;

    public void Login(Utilisateur user) => CurrentUser = user;
    public void Logout() => CurrentUser = null;
}
