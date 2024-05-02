using System;
using System.Web.Http;
using System;
using System.Data.SqlClient;

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
            try
            {
                return Ok(this.medicamentoRepo.ObterTodos());
            }
            catch (Exception ex)
            {
                //fazer log de erro em um arquivo

                return InternalServerError(ex); 
            }    
        }

        [HttpGet]
        // GET: api/medicamentos/5
        public IHttpActionResult Get(int id)
        {
            Models.Medicamento medicamento = this.medicamentoRepo.ObterporID(id);
            if (medicamento is null)
                return BadRequest("Solicitação inválida");
            return Ok(medicamento);
        }

        [HttpGet]
        // GET: api/medicamentos?nome=dipirona
        public IHttpActionResult Get(string nome)
        {
            return Ok(medicamentoRepo.ObterporNome(nome));
        }

        [HttpPost]
        // POST: api/medicamentos
        public IHttpActionResult Post(Models.Medicamento medicamento)
        {
            if (medicamento.Nome == null || medicamento.DataFabricacao == DateTime.MinValue)
                return BadRequest("dados obrigatórios nome e/ou data fabricação não foram enviados.");
            if (!this.medicamentoRepo.Inserir(medicamento))
                return InternalServerError();
            return Ok(medicamento);
        }

        [HttpPut]
        // PUT: api/medicamentos/5
        public IHttpActionResult Put(int id, Models.Medicamento medicamento)
        {
            if (medicamento.Id != id)
                return BadRequest("O id da requisição não coincide com o id do médico");

            if (medicamento.Nome == null || medicamento.DataFabricacao == DateTime.MinValue)
                return BadRequest("dados obrigatórios nome e/ou data fabricação não foram enviados.");

            if (medicamento.DataVencimento != null && medicamento.DataVencimento < medicamento.DataFabricacao)
                return BadRequest("data vencimento não pode ser menor que a data de fabricação");

            if (!this.medicamentoRepo.Update(id, medicamento))
                return InternalServerError();
            return Ok(medicamento);
        }

        // DELETE: api/medicamentos/5
        public IHttpActionResult Delete(int id)
        {
            if(!this.medicamentoRepo.Delete(id))
                return NotFound();
            return Ok();
        }
    }
}
