using System;
using System.Collections.Generic;

namespace CanbanAPI.Models
{
    public partial class PrzestrzenRoboczaUzytkownika
    {
        public int IdPrzestrzenRoboczaUzytkownika { get; set; }
        public int IdUzytkownik { get; set; }
        public int IdPrzestrzenRobocza { get; set; }

        public virtual PrzestrzenieRobocze IdPrzestrzenRoboczaNavigation { get; set; } = null!;
        public virtual Uzytkownicy IdUzytkownikNavigation { get; set; } = null!;
    }
}
