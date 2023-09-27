using FluentValidation;

using System.Collections.Generic;
using System.Linq;

using TiposDeValidacoes.Api;

public class ClienteService
{
    private readonly IValidator<Cliente> _clienteValidator;

    public ClienteService()
    {
        _clienteValidator = new ClienteValidator();
    }

    public Dictionary<string, TiposDeValidacoes.Api.ValidationResult> ProcessarCliente(Cliente cliente)
    {
        var resultados = _clienteValidator.Validate(cliente);

        return resultados.Errors
            .GroupBy(e => "Cliente." + e.PropertyName) // Adicionado o prefixo "Cliente."
            .ToDictionary(
                g => g.Key,
                g => new TiposDeValidacoes.Api.ValidationResult(false, g.First().ErrorMessage, g.Key)
            );
    }

    public string GerarMensagemErro(Dictionary<string, TiposDeValidacoes.Api.ValidationResult> resultadosInvalidos)
    {
        var nomesCamposInvalidos = resultadosInvalidos.Keys.ToList();
        var listaCamposInvalidos = string.Join(", ", nomesCamposInvalidos);
        return $"Erro ao salvar o cliente. Os campos {listaCamposInvalidos} são obrigatórios.";
    }
}