namespace TiposDeValidacoes.Api.Services
{
    public class ClienteServiceValidationBuilder
    {
        public List<string> ProcessarCliente(Cliente cliente)
        {
            var validationBuilder = new ValidationBuilder<Cliente>(cliente)
                .AdicionarValidacao(c => c.Cpf, valor => !string.IsNullOrWhiteSpace(valor.ToString()) && valor.ToString().Length == 11)
                .AdicionarValidacao(c => c.Nome)
                .AdicionarValidacao(c => c.Endereco.CEP)
                .AdicionarValidacao(c => c.Tipo, valor => (TipoPessoa)valor != TipoPessoa.NaoDefinido)
                .AdicionarValidacao(c => c.Telefones);

            var resultados = validationBuilder.ValidarPropriedades();

            return resultados;
        }
    }
}