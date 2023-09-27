using Microsoft.AspNetCore.Mvc;

namespace TiposDeValidacoes.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly ClienteService _clienteService;

        public ClientesController(ClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        // POST: api/clientes/processar
        [HttpPost("processar")]
        public ActionResult<Dictionary<string, bool>> ProcessarClienteComLista([FromBody] Cliente cliente)
        {
            try
            {
                var resultados = _clienteService.ProcessarCliente(cliente);
                return Ok(resultados);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}