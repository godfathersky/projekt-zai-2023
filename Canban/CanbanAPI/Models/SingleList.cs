namespace CanbanAPI.Models
{
    public class SingleList
    {
        public string nazwaLista { get; set; }

        public int listaId { get; set; }

        public List<ListTask> zadaniaLista { get; set; }
    }
}
