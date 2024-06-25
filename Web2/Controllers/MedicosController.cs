using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Web2.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MedicosController : ApiController
    {
        private readonly Repositories.SQLServer.Medico RepositorioMedico;
        public MedicosController() 
        { 
            this.RepositorioMedico = new Repositories.SQLServer.
                Medico(Configurations.Database.getConnectionString());
        }

        [HttpGet]
        // GET: api/Medicos
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                return Ok(await this.RepositorioMedico.Select());
            }
            catch (Exception ex)
            {
                Utils.Logger.WriteException(Configurations.Logger.getFullPath(), ex);
                return InternalServerError();
            }
            
        }

        [HttpGet]
        // GET: api/Medicos?nome=j
        public async Task<IHttpActionResult> Get(string nome)
        {
            try
            {
                return Ok(await this.RepositorioMedico.SelectByNome(nome));
            }
            catch (Exception ex)
            {
                Utils.Logger.WriteException(Configurations.Logger.getFullPath(), ex);
                return InternalServerError();
            }
            
        }


        [HttpGet]
        // GET: api/Medicos/5
        public async Task <IHttpActionResult> Get(int id)
        {
            try
            {
                Models.Medico medico = await this.RepositorioMedico.SelectById(id);
                if (medico is null)
                    return NotFound();
                return Ok(medico);
            }
            catch (Exception ex)
            {
                Utils.Logger.WriteException(Configurations.Logger.getFullPath(), ex);
                return InternalServerError();
            }
            
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
