﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;


namespace Web2.Repositories.SQLServer
{
    public class Medicamento
    {
        private readonly SqlConnection conn;
        private readonly SqlCommand cmd;
        private readonly string cacheKey;
        private readonly List<Models.Medicamento> cacheItem;
        private readonly int DefaultCacheTimeInSeconds;

        public Medicamento(string connectionString)
        {
            this.conn = new SqlConnection(connectionString);
            this.cmd = new SqlCommand { Connection = this.conn };

            this.cacheKey = "medicamentoKey";
            this.cacheItem = (List<Models.Medicamento>)Utils.Cache.GetCache(cacheKey);
            this.DefaultCacheTimeInSeconds = Configurations.Cache.GetDefaultCacheTimeInSeconds();
        }

        public async Task<List<Models.Medicamento>> SelectAll()
        {
            if (cacheItem != null)
                return cacheItem;

            List<Models.Medicamento> medicamentos = new List<Models.Medicamento>();
            using (this.conn)
            {
                await this.conn.OpenAsync();

                using (this.cmd)
                {
                    this.cmd.CommandText = "select id, nome, datafabricacao, datavencimento from medicamento;";
                    using (SqlDataReader dr = await this.cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            Models.Medicamento medicamento = new Models.Medicamento();
                            medicamento.Id = (int)dr["id"];
                            medicamento.Nome = dr["nome"].ToString();
                            medicamento.DataFabricacao = Convert.ToDateTime(dr["datafabricacao"]);
                            if (!(dr["datavencimento"] is DBNull))
                                medicamento.DataVencimento = Convert.ToDateTime(dr["datavencimento"]);
                            medicamentos.Add(medicamento);
                        }
                    }
                }
                Utils.Cache.SetCache(cacheKey, medicamentos, DefaultCacheTimeInSeconds);
            }
            return medicamentos;
        }

        public async Task<Models.Medicamento> SelectById(int id)
        {
            Models.Medicamento medicamento = cacheItem?.Find(x => x.Id == id);

            if (medicamento != null)
                return medicamento;

            using (this.conn)
            {
                await this.conn.OpenAsync();

                using (this.cmd)
                {
                    this.cmd.CommandText = "select id, nome, datafabricacao, datavencimento from medicamento where id = @id;";
                    this.cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;

                    using (SqlDataReader dr = await this.cmd.ExecuteReaderAsync())
                    {
                        if (await dr.ReadAsync())
                        {
                            medicamento = new Models.Medicamento();
                            medicamento.Id = (int)dr["id"];
                            medicamento.Nome = dr["nome"].ToString();
                            medicamento.DataFabricacao = Convert.ToDateTime(dr["datafabricacao"]);
                            if (!(dr["datavencimento"] is DBNull))
                                medicamento.DataVencimento = Convert.ToDateTime(dr["datavencimento"]);
                        }
                    }
                }
            }
            Utils.Cache.ClearCache(cacheKey);
            return medicamento;
        }

        public async Task<List<Models.Medicamento>> SelectByName(string nome)
        {
            //return cacheItem.Where(x => x.Nome.ToLower().Contains(nome.ToLower())).ToList();

            List<Models.Medicamento> medicamentos = cacheItem?.FindAll(x => x.Nome.ToLower().Contains(nome.ToLower()));
            if(medicamentos != null && medicamentos.Count != 0)
                return medicamentos;

            medicamentos = new List<Models.Medicamento>();

            using (this.conn)
            {
                await this.conn.OpenAsync();
                using (this.cmd)
                {
                    this.cmd.CommandText = "select id, nome, datafabricacao, datavencimento from medicamento where nome like @nome;";
                    this.cmd.Parameters.Add(new SqlParameter("@nome", SqlDbType.VarChar)).Value = $"%{nome}%";
                    using (SqlDataReader dr = await this.cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            Models.Medicamento medicamento = new Models.Medicamento();
                            medicamento.Id = (int)dr["id"];
                            medicamento.Nome = dr["nome"].ToString();
                            medicamento.DataFabricacao = (DateTime)dr["dataFabricacao"];
                            if (!(dr["datavencimento"] is DBNull))
                                medicamento.DataVencimento = Convert.ToDateTime(dr["datavencimento"]);
                            medicamentos.Add(medicamento);
                        }
                    }
                }
            }
            Utils.Cache.ClearCache(cacheKey);
            return medicamentos;
        }

        public async Task<bool> Insert(Models.Medicamento medicamento)
        {
            using (conn)
            {
                await this.conn.OpenAsync();
                using (cmd)
                {
                    this.cmd.CommandText = "insert into medicamento (nome, datafabricacao, datavencimento) values (@nome,@datafabricacao,@datavencimento); select CONVERT(int, SCOPE_IDENTITY());";
                    this.cmd.Parameters.Add(new SqlParameter("@nome", SqlDbType.VarChar)).Value = medicamento.Nome;
                    this.cmd.Parameters.Add(new SqlParameter("@datafabricacao", SqlDbType.Date)).Value = medicamento.DataFabricacao;
                    if (medicamento.DataVencimento != null)
                        this.cmd.Parameters.Add(new SqlParameter("@datavencimento", SqlDbType.Date)).Value = medicamento.DataVencimento;
                    else
                        this.cmd.Parameters.Add(new SqlParameter("@datavencimento", SqlDbType.Date)).Value = DBNull.Value;
                    medicamento.Id = (int)await this.cmd.ExecuteScalarAsync();
                }
            }
            Utils.Cache.ClearCache(cacheKey);
            return medicamento.Id != 0;
        }

        public async Task<bool> Update(Models.Medicamento medicamento)
        {
            int linhasAfetadas;

            using (conn)
            {
                await this.conn.OpenAsync();

                using (cmd)
                {
                    this.cmd.CommandText = "update medicamento set nome = @nome, datafabricacao = @datafabricacao, datavencimento = @datavencimento where id = @id;";
                    this.cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.VarChar)).Value = medicamento.Id;
                    this.cmd.Parameters.Add(new SqlParameter("@nome", SqlDbType.VarChar)).Value = medicamento.Nome;
                    this.cmd.Parameters.Add(new SqlParameter("@datafabricacao", SqlDbType.Date)).Value = medicamento.DataFabricacao;
                    if (medicamento.DataVencimento != null)
                        this.cmd.Parameters.Add(new SqlParameter("@datavencimento", SqlDbType.Date)).Value = medicamento.DataVencimento;
                    else
                        this.cmd.Parameters.Add(new SqlParameter("@datavencimento", SqlDbType.Date)).Value = DBNull.Value;

                    linhasAfetadas = await this.cmd.ExecuteNonQueryAsync();
                }
            }

            Utils.Cache.ClearCache(cacheKey);

            return linhasAfetadas == 1;
        }

        public async Task<bool> Delete(int id)
        {
            int linhasAfetas;
            using (conn)
            {
                await this.conn.OpenAsync();
                using (cmd)
                {
                    this.cmd.CommandText = "delete medicamento where id = @id;";
                    this.cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;
                    linhasAfetas = await this.cmd.ExecuteNonQueryAsync();
                }
            }
            Utils.Cache.ClearCache(cacheKey);

            return linhasAfetas == 1;
        }
    }
}