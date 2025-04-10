using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taskMasterProjet;

public class Tache
{
    public int Id { get; set; }
    public string Titre { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime DateCreation { get; set; } = DateTime.Now;
    public DateTime? Echeance { get; set; }
    public string Statut { get; set; } = "à faire";
    public string Priorite { get; set; } = "moyenne";
    public string Categorie { get; set; } = "perso";

    // Clés étrangères
    public int? AuteurId { get; set; }
    public Utilisateur? Auteur { get; set; }

    public int? RealisateurId { get; set; }
    public Utilisateur? Realisateur { get; set; }

    public int? ProjetId { get; set; }
    public Projet? Projet { get; set; }

    // Relations
    public ICollection<SousTache>? SousTaches { get; set; }
    public ICollection<Commentaire>? Commentaires { get; set; }
    public ICollection<Etiquette>? Etiquettes { get; set; }
}

