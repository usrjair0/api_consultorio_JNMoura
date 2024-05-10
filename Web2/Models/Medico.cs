using System;
using System.ComponentModel.DataAnnotations;

namespace Web2.Models
{
    public class Medico
    {
        public int Id { get; set; }

        [Required]
        [StringLength(9)]
        public string CRM { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        public Medico() 
        { 
        }
        public Medico(int id, string crm, string nome)
        {
            Id = id;
            CRM = crm;
            Nome = nome;
        }
    }
}