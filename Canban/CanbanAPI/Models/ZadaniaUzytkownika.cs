using System;
using System.Collections.Generic;

namespace CanbanAPI.Models
{
    public partial class ZadaniaUzytkownika
    {
        public int IdZadanieUzytkownik { get; set; }
        public int IdUzytkownik { get; set; }
        public int IdZadanie { get; set; }
        public DateTime DataPrzydzielenieZadanie { get; set; }

        public virtual Uzytkownicy IdUzytkownikNavigation { get; set; } = null!;
        public virtual Zadanium IdZadanieNavigation { get; set; } = null!;
    }
}
