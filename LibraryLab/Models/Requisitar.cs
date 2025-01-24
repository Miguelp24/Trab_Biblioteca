using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LibraryLab.Models
{
    public class Requisitar
    {
        [Key]
        public int Id { get; set; }
      
        public int? BibliotecarioId { get; set; }
        public Utilizador? Bibliotecario { get; set; }

        public int? BibliotecarioRecebeId { get; set; }
        public Utilizador? BibliotecarioRecebe { get; set; }

        [Required]
        public int LeitorId { get; set; }
        public Utilizador? Leitor { get; set; }

        [Required]
        public int LivroId { get; set; }
        public Livro? Livro { get; set; }

        [Required]
        public DateTime? DataRequisicao { get; set; }

        public DateTime? DataEmprestimo { get; set; }

        public DateTime? DataDevolucao { get; set; }


    }
}
