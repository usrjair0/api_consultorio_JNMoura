﻿using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

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

        public async Task <List<Models.Medico> > Select()
        {
            List<Models.Medico> medicos = new List<Models.Medico>();
            using(this.conn)
            {
                await this.conn.OpenAsync();
                using(this.cmd)
                {
                    this.cmd.CommandText = "select id, crm, nome from medico;";
                    using(SqlDataReader dr = await this.cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
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

        public async Task <List<Models.Medico>> SelectByNome(string nome)
        {
            List<Models.Medico> medicos = new List<Models.Medico>();
            using(this.conn)
            {
                await this.conn.OpenAsync();
                using(this.cmd)
                {
                    this.cmd.CommandText = "select id, crm, nome from medico where nome like @nome;";
                    this.cmd.Parameters.Add(new SqlParameter("@nome", SqlDbType.VarChar)).Value = $"{nome}%";
                    using (SqlDataReader dr = await this.cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
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

        public async Task<Models.Medico> SelectById(int id)
        {
            Models.Medico medico = null;
            using (this.conn) 
            {
                await this.conn.OpenAsync();
                using(this.cmd)
                {
                    this.cmd.CommandText = "select id, crm, nome from medico where id = @id;";
                    this.cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;
                    using(SqlDataReader dr = await this.cmd.ExecuteReaderAsync()) 
                    { 
                        if(await dr.ReadAsync())
                        {
                            medico = new Models.Medico
                            {
                                Id = (int)dr["id"],
                                CRM = dr["crm"].ToString(),
                                Nome = dr["nome"].ToString()
                            };
                        }
                    }
                }
            }

            return medico;
        }
    }
}