using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web2.Models
{
    public class Medicamento
    {
        public int Id { get; set; }
        public string Nome { get; set;}

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