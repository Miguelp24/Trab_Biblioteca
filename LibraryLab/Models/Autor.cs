using System.ComponentModel.DataAnnotations;

namespace LibraryLab.Models
{
    public class Autor
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Required failed")]
        [StringLength(50, ErrorMessage = "O nome é muito longo")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Required failed")]
        [StringLength(300, ErrorMessage = "A biografia é muito longa")]
        public string? Biography { get; set; }

        [Required]
        public string? Image { get; set; }


        public ICollection<Livro>? Livros { get; set; }

    }
}
