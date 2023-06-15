using System;
using System.Collections.Generic;

namespace CanbanAPI.Models
{
    public partial class Uzytkownicy
    {
        public Uzytkownicy()
        {
            PrzestrzenRoboczaUzytkownikas = new HashSet<PrzestrzenRoboczaUzytkownika>();
            ZadaniaUzytkownikas = new HashSet<ZadaniaUzytkownika>();
        }

        public int IdUzytkownika { get; set; }
        public string NazwaUzytkownik { get; set; } = null!;
        public string EmailUzytkownik { get; set; } = null!;
        public DateTime DataUrodzeniaUzytkownik { get; set; }
        public byte[] HasloHashUzytkownik { get; set; } = null!;
        public byte[] SaltHasloUzytkownik { get; set; } = null!;
        public DateTime DataRejestracjaUzytkownik { get; set; }

        public virtual ICollection<PrzestrzenRoboczaUzytkownika> PrzestrzenRoboczaUzytkownikas { get; set; }
        public virtual ICollection<ZadaniaUzytkownika> ZadaniaUzytkownikas { get; set; }
    }
}
