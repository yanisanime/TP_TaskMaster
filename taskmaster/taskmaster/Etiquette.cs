using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taskmaster
{
    public class Etiquette
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;

        public ICollection<Tache>? Taches { get; set; }
    }
}
