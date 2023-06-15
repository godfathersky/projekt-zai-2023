namespace CanbanAPI.Models
{
    public class TabliceZadanDTO
    {
        public string User { get; set; }

        public string NazwaPrzestrzen { get; set; }
        public string NazwaTablicaZadan { get; set; } = null!;
        public string DataUtworzeniaTablicaZadan { get; set; }
    }
}
