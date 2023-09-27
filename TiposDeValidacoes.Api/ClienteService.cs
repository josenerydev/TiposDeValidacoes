using System.Collections.Generic;

namespace TiposDeValidacoes.Api
{
    public class ClienteService
    {
        public Dictionary<string, ValidationResult> ProcessarCliente(Cliente cliente)
        {
            var validationBuilder = new ValidationBuilder<Cliente>(cliente)
                .AdicionarValidacao(c => c.Cpf, valor => !string.IsNullOrWhiteSpace(valor as string) && valor.ToString().Length == 11)
                .AdicionarValidacao(c => c.Nome)
                .AdicionarValidacao(c => c.Endereco.CEP)
                .AdicionarValidacao(c => c.Tipo, valor => (TipoPessoa)valor != TipoPessoa.NaoDefinido)
                .AdicionarValidacao(c => c.Telefones);

            var resultados = validationBuilder.ValidarPropriedades();
            var resultadosInvalidos = resultados.Invalidos();

            if (resultadosInvalidos.Any())
            {
                var mensagemErro = GerarMensagemErro(resultadosInvalidos);
                throw new ArgumentException(mensagemErro);
            }

            return resultados;
        }

        private string GerarMensagemErro(Dictionary<string, ValidationResult> resultadosInvalidos)
        {
            var nomesCamposInvalidos = resultadosInvalidos.Keys.ToList();
            var listaCamposInvalidos = string.Join(", ", nomesCamposInvalidos);
            return $"Erro ao salvar o cliente. Os campos {listaCamposInvalidos} são obrigatórios.";
        }
    }
}