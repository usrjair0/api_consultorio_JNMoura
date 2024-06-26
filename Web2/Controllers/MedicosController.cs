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

        [HttpPost]
        // POST: api/Medicos
        public async Task<IHttpActionResult> Post([FromBody]Models.Medico medico)
        {
            try
            {
                if(!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (!await this.RepositorioMedico.Insert(medico))
                {
                    if (!Validations.Medico.UniqueCRM)
                        return BadRequest("O CRM informado já existe na base de dados");
                    else
                        return InternalServerError();
                }
                    
                return Ok(medico);
            }
            catch (Exception ex)
            {
                Utils.Logger.WriteException(Configurations.Logger.getFullPath(), ex);
                return InternalServerError();
            }
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
