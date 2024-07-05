using System;
using System.ComponentModel.DataAnnotations;

namespace Web2.Models
{
    public class Paciente
    {
        public int Id {  get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [Required]
        public DateTime DataNascimento { get; set; }

        public Paciente() { }

        public Paciente(int id, string nome, DateTime dataNascimento)
        {
            Id = id;
            Nome = nome;
            DataNascimento = dataNascimento;
        }
    }
}