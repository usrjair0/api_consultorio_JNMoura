using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Web2.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UsuariosController : ApiController
    {
        private readonly Repositories.SQLServer.Usuario repoUsuario;

        public UsuariosController()
        {
            repoUsuario = new Repositories.SQLServer.Usuario(Configurations.Database.getConnectionString());
        }

        [HttpGet]
        // GET: api/Usuarios
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                return Ok(await this.repoUsuario.Select());
            }
            catch (System.Exception ex)
            {
                Utils.Logger.WriteException(Configurations.Logger.getFullPath(), ex);
                return InternalServerError();
            }
        }

        [HttpGet]
        // GET: api/Usuarios/5
        public async Task<IHttpActionResult> Get(int id)
        {
            try
            {
                Models.Usuario usuario = await this.repoUsuario.Select(id);
                if(usuario == null)
                    return NotFound();

                return Ok(usuario);
            }
            catch (System.Exception ex)
            {
                Utils.Logger.WriteException(Configurations.Logger.getFullPath(), ex);
                return InternalServerError();
            }
        }

        [HttpGet]
        // GET: api/Usuarios/5
        public async Task<IHttpActionResult> Get(string nome)
        {
            try
            {
                return Ok(await this.repoUsuario.Select(nome));
            }
            catch (System.Exception ex)
            {
                Utils.Logger.WriteException(Configurations.Logger.getFullPath(), ex);
                return InternalServerError();
            }
        }



        [HttpPost]
        // POST: api/Usuarios
        public async Task<IHttpActionResult> Post([FromBody]Models.Usuario usuario)
        {
            try
            {
                if(!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (!await this.repoUsuario.Insert(usuario))
                    return InternalServerError();

                return Ok(usuario);
            }
            catch (System.Exception ex)
            {
                Utils.Logger.WriteException(Configurations.Logger.getFullPath(), ex);

                return InternalServerError();
            }
        }

        [HttpPut]
        // PUT: api/Usuarios/5
        public async Task<IHttpActionResult> Put(int id, [FromBody]Models.Usuario usuario)
        {
            try
            {
                if (!Validations.Requisicao.IdRequisicaoIgualAoIdCorpoRequisicao(id, usuario.Id))
                    return BadRequest();

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (!await this.repoUsuario.Update(usuario))
                    return NotFound();

                return Ok(usuario);
            }
            catch (System.Exception ex)
            {
                Utils.Logger.WriteException(Configurations.Logger.getFullPath(), ex);
                return InternalServerError();
            }
        }

        [HttpDelete]
        // DELETE: api/Usuarios/5
        public async Task<IHttpActionResult> Delete(int id)
        {
            try
            {
                if(!await this.repoUsuario.Delete(id))
                    return NotFound();
                return Ok();
            }
            catch (System.Exception ex)
            {
                Utils.Logger.WriteException(Configurations.Logger.getFullPath(), ex);
                return InternalServerError();
            }
        }
    }
}
