using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taskmaster
{
    public class Projet
    {
        public int Id { get; set; }
        public string? Nom { get; set; }
        public string? Description { get; set; }

        public int? CreateurId { get; set; }
        public Utilisateur? Createur { get; set; }

        public ICollection<Tache>? Taches { get; set; }

    }
}
