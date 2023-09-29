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
            var erros = _clienteService.ProcessarCliente_v1(cliente);

            if (erros.Count() > 0)
            {
                var mensagemErro = GerarMensagemErro(erros.ToList());
                return BadRequest(new { message = mensagemErro });
            }

            return Ok();
        }

        private string GerarMensagemErro(List<string> erros)
        {
            var palavraCampo = erros.Count > 1 ? "campos" : "campo";
            var palavraObrigatorio = erros.Count > 1 ? "são" : "é";
            var validade = erros.Count > 1 ? "devem ser válidos" : "deve ser válido";

            var camposInvalidos = string.Join(", ", erros);
            return $"Erro ao salvar o cliente. O {palavraCampo} {camposInvalidos} {palavraObrigatorio} obrigatório(s) e {validade}.";
        }
    }
}