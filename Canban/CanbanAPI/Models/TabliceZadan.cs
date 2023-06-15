using System;
using System.Collections.Generic;

namespace CanbanAPI.Models
{
    public partial class TabliceZadan
    {
        public TabliceZadan()
        {
            ListyZadans = new HashSet<ListyZadan>();
        }

        public int IdTablicaZadan { get; set; }
        public string NazwaTablicaZadan { get; set; } = null!;
        public DateTime DataUtworzeniaTablicaZadan { get; set; }
        public int IdPrzestrzenRobocza { get; set; }

        public virtual PrzestrzenieRobocze IdPrzestrzenRoboczaNavigation { get; set; } = null!;
        public virtual ICollection<ListyZadan> ListyZadans { get; set; }
    }
}
