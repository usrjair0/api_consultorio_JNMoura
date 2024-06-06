using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;


namespace Web2.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
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
        public async Task<IHttpActionResult> Get()
        {
            try
            {
               return Ok(await this.medicamentoRepo.ObterTodos());
            }
            catch (Exception ex)
            {
                Utils.Logger.WriteException(Configurations.Logger.getFullPath(), ex);
                return InternalServerError();
            }
        }

        [HttpGet]
        // GET: api/medicamentos/5
        public async Task<IHttpActionResult> Get(int id)
        {
            try
            {
                Models.Medicamento medicamento = await this.medicamentoRepo.ObterporID(id);
                if (medicamento is null)
                    return NotFound();
                return Ok(medicamento);
            }
            catch (Exception ex)
            {
                Utils.Logger.WriteException(Configurations.Logger.getFullPath(), ex);
                return InternalServerError();
            }
            
        }

        [HttpGet]
        // GET: api/medicamentos?nome=dipirona
        public async Task<IHttpActionResult> Get(string nome)
        {
            try
            {
                return Ok(await medicamentoRepo.ObterporNome(nome));
            }
            catch (Exception ex)
            {
                Utils.Logger.WriteException(Configurations.Logger.getFullPath(), ex);
                return InternalServerError();
            }
        }

        [HttpPost]
        // POST: api/medicamentos
        public async Task<IHttpActionResult> Post([FromBody] Models.Medicamento medicamento)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                if (!await this.medicamentoRepo.Inserir(medicamento))
                    return InternalServerError();
                return Ok(medicamento);
            }
            catch (Exception ex)
            {
                Utils.Logger.WriteException(Configurations.Logger.getFullPath(), ex);
                return InternalServerError();
            }          
        }

        [HttpPut]
        // PUT: api/medicamentos/5
        public async Task<IHttpActionResult> Put(int id,[FromBody] Models.Medicamento medicamento)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                if (medicamento.Id != id)
                    return BadRequest("O id da requisição não coincide com o id do médico");
                if (medicamento.DataVencimento != null && medicamento.DataVencimento < medicamento.DataFabricacao)
                    return BadRequest("data vencimento não pode ser menor que a data de fabricação");
                if(!await this.medicamentoRepo.Update(medicamento))
                    return NotFound();

                return Ok(medicamento);
            }
            catch (Exception ex)
            {
                Utils.Logger.WriteException(Configurations.Logger.getFullPath(), ex);
                return InternalServerError();
            }
        }

        // DELETE: api/medicamentos/5
        public async Task<IHttpActionResult> Delete(int id)
        {
            try
            {
                if (!await this.medicamentoRepo.Delete(id))
                    return NotFound();
                return Ok();
            }
            catch (Exception ex)
            {
                Utils.Logger.WriteException(Configurations.Logger.getFullPath(), ex);
                return InternalServerError();
            }
        }
    }
}

