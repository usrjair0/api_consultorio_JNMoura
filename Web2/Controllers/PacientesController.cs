using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Web2.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PacientesController : ApiController
    {
        private readonly Repositories.SQLServer.Paciente RepositorioPaciente;
        public PacientesController() 
        { 
            this.RepositorioPaciente = new Repositories.SQLServer.Paciente(Configurations.Database.getConnectionString());
        }

        [HttpGet]
        // GET: api/Pacientes
        public async Task<IHttpActionResult> Get()
        {
            try
            {   
                return Ok(await this.RepositorioPaciente.SelectAll());
            }
            catch (Exception ex)
            {
                Utils.Logger.WriteException(Configurations.Logger.getFullPath(), ex);
                return InternalServerError();
            }
        }

        [HttpGet]
        // GET: api/Pacientes/5
        public async Task<IHttpActionResult> GetById(int id)
        {
            try
            {
                Models.Paciente paciente = await this.RepositorioPaciente.SelectById(id);

                if (paciente is null)
                    return NotFound();

                return Ok(paciente);
            }
            catch (Exception ex)
            {
                Utils.Logger.WriteException(Configurations.Logger.getFullPath(), ex);
                return InternalServerError();
            }
        }

        [HttpGet]
        // GET: api/pacientes?nome=jo
        public async Task<IHttpActionResult> GetByName(string nome)
        {
            try
            {
                return Ok(await this.RepositorioPaciente.SelectByName(nome));
            }
            catch (Exception ex)
            {
                Utils.Logger.WriteException(Configurations.Logger.getFullPath(), ex);
                return InternalServerError();
            }
        }

        [HttpPost]
        // POST: api/Pacientes
        public async Task<IHttpActionResult> Post([FromBody] Models.Paciente paciente)
        {
            try
            {
                if(!ModelState.IsValid)
                    return BadRequest(ModelState);

                if(!await this.RepositorioPaciente.Insert(paciente))
                    return InternalServerError();

                return Ok(paciente);
            }
            catch (Exception ex)
            {
                Utils.Logger.WriteException(Configurations.Logger.getFullPath(), ex);
                return InternalServerError();
            }
        }

        [HttpPut]
        // PUT: api/Pacientes/5
        public async Task<IHttpActionResult> Put(int id, [FromBody] Models.Paciente paciente)
        {
            try
            {
                if (paciente.Id != id)
                    return BadRequest("O id da requisição não coincide com o id do Paciente");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (!await this.RepositorioPaciente.Update(paciente))
                    return NotFound();

                return Ok(paciente);
            }
            catch (Exception ex)
            {
                Utils.Logger.WriteException(Configurations.Logger.getFullPath(), ex);
                return InternalServerError();
            }
        }

        [HttpDelete]
        // DELETE: api/Pacientes/5
        public async Task<IHttpActionResult> Delete(int id)
        {
            try
            {
                if(!await this.RepositorioPaciente.Delete(id))
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
