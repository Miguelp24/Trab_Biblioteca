using System.ComponentModel.DataAnnotations;

namespace LibraryLab.Models
{
    public class Genero
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Required Failed")]
        [StringLength(50, ErrorMessage = "Nome muito longo")]
        public string? Name { get; set; }


        public ICollection<Livro>? Livros { get; set; }
    }
}
