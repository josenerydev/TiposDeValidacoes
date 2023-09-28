using FluentValidation;

namespace TiposDeValidacoes.Api.Services
{
    public class ClienteServiceFluentValidation
    {
        private readonly IValidator<Cliente> _clienteValidator;

        public ClienteServiceFluentValidation()
        {
            _clienteValidator = new ClienteValidator();
        }

        public List<string> ProcessarCliente(Cliente cliente)
        {
            var resultados = _clienteValidator.Validate(cliente);

            return resultados.Errors
                .Select(e => e.PropertyName)
                .Distinct()
                .ToList();
        }
    }
}