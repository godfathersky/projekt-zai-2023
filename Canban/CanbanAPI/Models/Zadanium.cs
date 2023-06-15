using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CanbanAPI.Models
{
    public partial class Zadanium
    {
        public Zadanium()
        {
            ZadaniaUzytkownikas = new HashSet<ZadaniaUzytkownika>();
        }

        public int IdZadanie { get; set; }
        public int IdLista { get; set; }
        public string NazwaZadanie { get; set; } = null!;
        public DateTime? DataUtworzenia { get; set; }
        public DateTime? DataPrognozowaniaZakoczenia { get; set; }
        public DateTime? DataZakonczenia { get; set; }
        public bool? CzyZadanieZakonczone { get; set; }

        [JsonIgnore]
        public virtual ListyZadan IdListaNavigation { get; set; } = null!;
        [JsonIgnore]
        public virtual ICollection<ZadaniaUzytkownika> ZadaniaUzytkownikas { get; set; }
    }
}
