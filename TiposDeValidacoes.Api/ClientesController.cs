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
                var resultadosInvalidos = _clienteService.ProcessarCliente(cliente);

                if (resultadosInvalidos.Any())
                {
                    var mensagemErro = _clienteService.GerarMensagemErro(resultadosInvalidos);
                    throw new ArgumentException(mensagemErro);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}