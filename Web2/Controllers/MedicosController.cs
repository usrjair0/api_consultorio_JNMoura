﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web.Http;

namespace Web2.Controllers
{
    public class MedicosController : ApiController
    {
        private readonly Repositories.SQLServer.Medico RepositorioMedico;
        public MedicosController() 
        { 
            this.RepositorioMedico = new Repositories.SQLServer.Medico(Configurations.Database.getConnectionString());
        }

        [HttpGet]
        // GET: api/Medicos
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                return Ok(await RepositorioMedico.Select());
            }
            catch (Exception ex)
            {
                Utils.Logger.WriteException(Configurations.Logger.getFullPath(), ex);
                return InternalServerError();
            }
            
        }

        [HttpGet]
        // GET: api/Medicos?nome=j
        public IHttpActionResult Get(string nome)
        {
            try
            {
                return Ok(this.RepositorioMedico.SelectByNome(nome));
            }
            catch (Exception ex)
            {
                Utils.Logger.WriteException(Configurations.Logger.getFullPath(), ex);
                return InternalServerError();
            }
            
        }


        [HttpGet]
        // GET: api/Medicos/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Medicos
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Medicos/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Medicos/5
        public void Delete(int id)
        {
        }
    }
}
