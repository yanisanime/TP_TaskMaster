using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taskMasterProjet;

public class Utilisateur
{
    public int Id { get; set; }
    public string? Nom { get; set; }
    public string? Prenom { get; set; }
    public string Email { get; set; } = string.Empty;
    public string MotDePasse { get; set; } = string.Empty; // ICI  le mot de passe en clair

    // Relations
    public ICollection<Tache>? TachesCreees { get; set; }
    public ICollection<Tache>? TachesARealiser { get; set; }
    public ICollection<Commentaire>? Commentaires { get; set; }


}
