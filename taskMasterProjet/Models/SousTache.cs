using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taskMasterProjet;

public class SousTache
{
    public int Id { get; set; }
    public string Titre { get; set; } = string.Empty;
    public string Statut { get; set; } = "à faire";
    public DateTime? Echeance { get; set; }

    public int? TacheId { get; set; }
    public Tache? Tache { get; set; }
}

