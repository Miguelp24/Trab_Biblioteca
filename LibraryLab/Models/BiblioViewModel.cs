using System.ComponentModel.DataAnnotations;

namespace LibraryLab.Models
{
    public class BiblioViewModel
    {
        [Required(ErrorMessage = "Required failed")]
        [StringLength(50, ErrorMessage = "O nome é muito longo")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Required failed")]
        [StringLength(150, ErrorMessage = "A localização é muito longo")]
        public string? Localizacao { get; set; }

        [Required(ErrorMessage = "Required failed")]
        [StringLength(50, ErrorMessage = "O email é muito longo")]
        public string? Email { get; set; }

        [Required]
        public int Telefone { get; set; }

        [Required]
        public TimeOnly Horario_abertura { get; set; }

        [Required]
        public TimeOnly Horario_fecho { get; set; }

        [Required(ErrorMessage = "Seleciona uma Imagem")]
        public IFormFile? Image { get; set; }
    }
}
