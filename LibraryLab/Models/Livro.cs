using System.ComponentModel.DataAnnotations;

namespace LibraryLab.Models
{
    public class Livro
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Required failed")]
        [StringLength(30, ErrorMessage = "O ISBN é muito longo")]
        public string? ISBN { get; set; }

        [Required(ErrorMessage = "Required failed")]
        [StringLength(50, ErrorMessage = "O Titulo é muito longo")]
        public string? Titulo { get; set; }

        public int AutorId { get; set; }
        public Autor? Autor { get; set; }

        public int GeneroId { get; set; }
        public Genero? Genero { get; set; }

        [Required]
        public int Num_exemplares { get; set; }

        [Required(ErrorMessage = "Required failed")]
        [StringLength(2000, ErrorMessage = "A Sinopse é muito longo")]
        public string? Sinopse { get; set; }

        [Required]
        public string? Image { get; set; }

        [Required]
        public bool Estado { get; set; }

    }
}
