using System;
using System.Collections.Generic;

namespace CanbanAPI.Models
{
    public partial class PrzestrzenieRobocze
    {
        public PrzestrzenieRobocze()
        {
            PrzestrzenRoboczaUzytkownikas = new HashSet<PrzestrzenRoboczaUzytkownika>();
            TabliceZadans = new HashSet<TabliceZadan>();
        }

        public int IdPrzestrzenRobocza { get; set; }
        public string NazwaPrzestrzenRobocza { get; set; } = null!;
        public DateTime DataUtworzeniaPrzestrzenRobocza { get; set; }

        public virtual ICollection<PrzestrzenRoboczaUzytkownika> PrzestrzenRoboczaUzytkownikas { get; set; }
        public virtual ICollection<TabliceZadan> TabliceZadans { get; set; }
    }
}
