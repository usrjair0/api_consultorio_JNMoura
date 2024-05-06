using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web2.Models
{
    public class Medico
    {
        public int Id { get; set; }
        public string CRM { get; set; }
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