using System.ComponentModel.DataAnnotations;

namespace Web2.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Campo Nome Obrigatório.")]
        [StringLength(100, ErrorMessage ="Máximo de 100 caracteres")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Campo Email Obrigatório.")]
        [StringLength(50, ErrorMessage = "Máximo de 100 caracteres")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Campo Senha Obrigatório.")]
        [StringLength(20, ErrorMessage = "Máximo de 100 caracteres")]
        public string Senha { get; set; }

    }
}