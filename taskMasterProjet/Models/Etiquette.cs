using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taskMasterProjet;


public class Etiquette
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;

    public ICollection<Tache>? Taches { get; set; }
}

