using System.Collections.Generic;

namespace TiposDeValidacoes.Api
{
    public class ClienteService
    {
        public Dictionary<string, ValidationResult> ProcessarCliente(Cliente cliente)
        {
            var resultados = new ValidationBuilder<Cliente>(cliente)
                .AdicionarValidacao(c => c.Cpf, valor => !string.IsNullOrWhiteSpace(valor as string) && valor.ToString().Length == 11)
                .AdicionarValidacao(c => c.Nome)
                .AdicionarValidacao(c => c.Endereco.CEP)
                .AdicionarValidacao(c => c.Tipo, valor => (TipoPessoa)valor != TipoPessoa.NaoDefinido)
                .AdicionarValidacao(c => c.Telefones)
                .ValidarPropriedades()
                .Invalidos();

            // Lógica adicional de processamento, se necessário

            return resultados;
        }
    }
}