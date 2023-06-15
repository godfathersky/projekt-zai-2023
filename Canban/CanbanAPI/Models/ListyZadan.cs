using System;
using System.Collections.Generic;

namespace CanbanAPI.Models
{
    public partial class ListyZadan
    {
        public ListyZadan()
        {
            Zadania = new HashSet<Zadanium>();
        }

        public int IdListaZadan { get; set; }
        public int IdTablicaZadan { get; set; }
        public string NazwaListaZadan { get; set; } = null!;
        public DateTime DataUtworzeniaListaZadan { get; set; }

        public virtual TabliceZadan IdTablicaZadanNavigation { get; set; } = null!;
        public virtual ICollection<Zadanium> Zadania { get; set; }
    }
}
