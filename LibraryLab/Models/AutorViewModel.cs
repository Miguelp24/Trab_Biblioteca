using System.ComponentModel.DataAnnotations;

namespace LibraryLab.Models
{
    public class AutorViewModel
    {

        [Required(ErrorMessage = "Required failed")]
        [StringLength(50, ErrorMessage = "O nome é muito longo")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Required failed")]
        [StringLength(300, ErrorMessage = "A biografia é muito longa")]
        public string? Biography { get; set; }

        [Required(ErrorMessage = "Seleciona uma Imagem")]
        public IFormFile? Image { get; set; }
    }
}
