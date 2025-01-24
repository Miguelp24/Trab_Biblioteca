namespace LibraryLab.Models
{
    public class HomeViewModel
    {
        public IEnumerable<TabelaBiblio> TabelaBiblio { get; set; }
        public IEnumerable<Livro> Livro { get; set; }
        public List<Genero> Genres { get; set; }
    }

}
