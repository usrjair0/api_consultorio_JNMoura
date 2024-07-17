using System.ComponentModel.DataAnnotations;


namespace Web2.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Campo Email obrigatório.")]
        [StringLength(50, ErrorMessage = "Máximo de 100 caracteres")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Campo Senha obrigatório.")]
        [StringLength(20, ErrorMessage = "Máximo de 100 caracteres")]
        public string Senha { get; set; }
    }
}