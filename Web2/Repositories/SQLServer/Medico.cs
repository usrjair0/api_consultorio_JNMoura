using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Web2.Controllers;
using Web2.Models;

namespace Web2.Repositories.SQLServer
{
    public class Medico
    {
        private readonly SqlConnection conn;
        private readonly SqlCommand cmd;
        public Medico(string connectionString) 
        { 
            this.conn = new SqlConnection(connectionString);
            this.cmd = new SqlCommand();
            this.cmd.Connection = conn;
        }

        public List<Models.Medico> Select()
        {
            List<Models.Medico> medicos = new List<Models.Medico>();
            using(this.conn)
            {
                this.conn.Open();
                using(this.cmd)
                {
                    this.cmd.CommandText = "select id, crm, nome from medico;";
                    using(SqlDataReader dr = this.cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Models.Medico medico = new Models.Medico
                            {
                                Id = (int)dr["id"],
                                CRM = dr["crm"].ToString(),
                                Nome = dr["nome"].ToString()
                            };
                            medicos.Add(medico);    
                        }
                    }
                }
            }
            return medicos;
        }
    }
}