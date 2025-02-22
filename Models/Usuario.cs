using System.ComponentModel.DataAnnotations;

namespace CrudSP.Models
  {
    public class Usuario
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="Digite o nome!")]
        public required string Nome { get; set; }   
        
        [Required(ErrorMessage ="Digite o Sobre Nome!")]
        public required string Sobrenome { get; set; }  

        [Required(ErrorMessage ="Digite o e-mail"),
            EmailAddress(ErrorMessage= "E-mail incorreto!")]
        public required string Email { get; set; }

        [Required(ErrorMessage ="Digite o cargo!")]
        public required string Cargo { get; set; }   
    }
  }