using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Web2.Controllers
{
    public class MedicamentosController : ApiController
    {
        private readonly Repositories.SQLServer.Medicamento medicamentoRepo;
        public MedicamentosController() 
        { 
            this.medicamentoRepo = new Repositories.SQLServer.
                Medicamento(Configurations.Database.getConnectionString());
        }

        // GET: api/medicamentos
        [HttpGet]
        public IHttpActionResult Get()
        {
            return Ok(this.medicamentoRepo.ObterTodos());
        }

        [HttpGet]
        // GET: api/medicamentos/5
        public IHttpActionResult Get(int id)
        {
            Models.Medicamento medicamento = this.medicamentoRepo.ObterporID(id);
            if (medicamento.Id == 0)
                return BadRequest("Solicitação inválida");
            return Ok(medicamento);
        }

        [HttpGet]
        public IHttpActionResult Get(string nome)
        {
            List<Models.Medicamento> medicamentos = medicamentoRepo.ObterporNome(nome);
            if(medicamentos.Count == 0)
            {
                return BadRequest("Medicamento não encontrado!");
            }
            return Ok(medicamentos);
        }

        [HttpPost]
        // POST: api/medicamentos
        public IHttpActionResult Post(Models.Medicamento medicamento)
        {
            if (!medicamentoRepo.Inserir(medicamento))
                return InternalServerError();
            return Ok(medicamento);
        }

        [HttpPut]
        // PUT: api/medicamentos/5
        public IHttpActionResult Put(int id, Models.Medicamento medicamento)
        {
            if (medicamento.Id != id)
                return BadRequest("O id da requisição não coincide com o id do médico");
            if(!medicamentoRepo.Update(id, medicamento))
                return InternalServerError();
            return Ok(medicamento);
        }

        // DELETE: api/medicamentos/5
        public IHttpActionResult Delete(int id)
        {
            if(!medicamentoRepo.Delete(id))
                return NotFound();
            return Ok();
        }
    }
}
