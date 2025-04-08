using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taskmaster
{
    public class Commentaire
    {
        public int Id { get; set; }
        public int? AuteurId { get; set; }
        public Utilisateur? Auteur { get; set; }

        public int? TacheId { get; set; }
        public Tache? Tache { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;
        public string? Contenu { get; set; }
    }
}
