using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Drawing2D;

namespace Web2.Repositories.SQLServer
{
    public class Medicamento
    {
        private readonly SqlConnection conn;
        private readonly SqlCommand cmd;
        public Medicamento(string connectionString)
        {
            this.conn = new SqlConnection(connectionString);
            this.cmd = new SqlCommand();
            this.cmd.Connection = this.conn;
        }

        public List<Models.Medicamento> ObterTodos()
        {
            List<Models.Medicamento> medicamentos = new List<Models.Medicamento>();
            using(this.conn)
            {
                conn.Open();
                using(this.cmd)
                {
                    cmd.CommandText = "select id, nome, datafabricacao, datavencimento from medicamento;";
                    using(SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while(dr.Read())
                        {
                            Models.Medicamento medicamento = new Models.Medicamento();
                            medicamento.Id = (int) dr["id"];
                            medicamento.Nome = dr["nome"].ToString();
                            medicamento.DataFabricacao = Convert.ToDateTime(dr["datafabricacao"]);
                            if(dr.IsDBNull(3))
                                medicamento.DataVencimento = null;
                            else
                                medicamento.DataVencimento = Convert.ToDateTime(dr["datavencimento"]);
                            medicamentos.Add(medicamento);
                        }
                    }
                }
            }
            return medicamentos;
        }

        public Models.Medicamento ObterporID(int id)
        {
            Models.Medicamento medicamento = new Models.Medicamento();
            using (this.conn)
            {
                conn.Open();
                using(this.cmd)
                {
                    cmd.CommandText = "select id, nome, datafabricacao, datavencimento from medicamento where id = @id;";
                    cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;
                    using(SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if(dr.Read())
                        {
                            medicamento.Id = (int) dr["id"];
                            medicamento.Nome = dr["nome"].ToString();
                            medicamento.DataFabricacao = Convert.ToDateTime(dr["datafabricacao"]);
                            if (dr.IsDBNull(3))
                                medicamento.DataVencimento = null;
                            else
                                medicamento.DataVencimento = Convert.ToDateTime(dr["datavencimento"]);
                        }
                    }
                }
            }
            return medicamento;
        }

        public List<Models.Medicamento> ObterporNome (string nome) 
        { 
            List<Models.Medicamento> medicamentos = new List<Models.Medicamento>();
            using(this.conn)
            {
                conn.Open();
                using(this.cmd)
                {
                    cmd.CommandText = "select id, nome, datafabricacao, datavencimento from medicamento where nome like @nome;";
                    cmd.Parameters.Add(new SqlParameter("@nome", SqlDbType.VarChar)).Value = $"%{nome}%";
                    using(SqlDataReader dr = cmd.ExecuteReader())
                    {
                        Models.Medicamento medicamento = new Models.Medicamento();
                        while (dr.Read())
                        {
                            medicamento.Id = (int)dr["id"];
                            medicamento.Nome = dr["nome"].ToString();
                            medicamento.DataFabricacao = (DateTime) dr["dataFabricacao"];
                            if (dr.IsDBNull(3))
                                medicamento.DataVencimento = null;
                            else
                                medicamento.DataVencimento = (DateTime) dr["datavencimento"];
                            medicamentos.Add(medicamento);
                        }
                    }
                }
            }

            return medicamentos;
        }

        public bool Inserir(Models.Medicamento medicamento) 
        {
            using(conn)
            {
                conn.Open();
                using(cmd)
                {
                    cmd.CommandText = "insert into medicamento (nome, datafabricacao, datavencimento) values (@nome,@datafabricacao,@datavencimento); select CONVERT(int, @@identity);";
                    cmd.Parameters.Add(new SqlParameter("@nome", SqlDbType.VarChar)).Value = medicamento.Nome;
                    cmd.Parameters.Add(new SqlParameter("@datafabricacao", SqlDbType.Date)).Value = medicamento.DataFabricacao;
                    if (medicamento.DataVencimento != null)
                        cmd.Parameters.Add(new SqlParameter("@datavencimento", SqlDbType.Date)).Value = medicamento.DataVencimento;
                    else
                        cmd.Parameters.Add(new SqlParameter("@datavencimento", SqlDbType.Date)).Value = DBNull.Value;
                    medicamento.Id = (int)cmd.ExecuteScalar();
                }
            }
            return medicamento.Id != 0;
        }

        public bool Update(int id, Models.Medicamento medicamento)
        {
            int linhasAfetadas;
            using(conn)
            {
                conn.Open();
                using(cmd) 
                {
                    cmd.CommandText = "update medicamento set nome = @nome, datafabricacao = @df, datavencimento = @dv where id = @id;";
                    cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.VarChar)).Value = id;
                    cmd.Parameters.Add(new SqlParameter("@nome", SqlDbType.VarChar)).Value = medicamento.Nome;
                    cmd.Parameters.Add(new SqlParameter("@df", SqlDbType.Date)).Value = medicamento.DataFabricacao;
                    cmd.Parameters.Add(new SqlParameter("dv", SqlDbType.Date)).Value = medicamento.DataFabricacao;
                    linhasAfetadas = cmd.ExecuteNonQuery();
                }
            }
            return linhasAfetadas == 1;
        }

        public bool Delete(int id) 
        {
            int linhasAfetas; 
            using(conn)
            {
                conn.Open();
                using(cmd)
                {
                    cmd.CommandText = "delete medicamento where id = @id;";
                    cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;
                    linhasAfetas = cmd.ExecuteNonQuery();
                }
            }
            return linhasAfetas == 1;
        }
    }   
}