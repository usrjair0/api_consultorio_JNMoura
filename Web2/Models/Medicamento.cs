using System;
using System.ComponentModel.DataAnnotations;

namespace Web2.Models
{
    public class Medicamento
    { 
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set;}

        [Required]
        public DateTime DataFabricacao {  get; set;}

        public DateTime? DataVencimento { get; set;}

        public Medicamento() 
        {
        
        }

        public Medicamento(int id, string nome, DateTime dataFabricacao, DateTime? dataVencimento)
        {
            Id = id;
            Nome = nome;
            DataFabricacao = dataFabricacao;
            DataVencimento = dataVencimento;
        }
    }
}