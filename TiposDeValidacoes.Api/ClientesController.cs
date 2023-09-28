using Microsoft.AspNetCore.Mvc;

namespace TiposDeValidacoes.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly ClienteServiceSimples _clienteService;

        public ClientesController(ClienteServiceSimples clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpPost("processar")]
        public ActionResult ProcessarClienteComLista([FromBody] Cliente cliente)
        {
            var erros = _clienteService.ProcessarCliente(cliente);

            if (erros.Count > 0)
            {
                var mensagemErro = _clienteService.GerarMensagemErro(erros);
                return BadRequest(new { message = mensagemErro });
            }

            return Ok();
        }
    }
}