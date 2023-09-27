using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

using TiposDeValidacoes.Api;

namespace ValidationBenchmark
{
    [MemoryDiagnoser]
    public class ClienteValidationBenchmark
    {
        private readonly ValidationBuilder<Cliente> _validationBuilder;

        public ClienteValidationBenchmark()
        {
            var cliente = new Cliente
            {
                Nome = "João",
                Cpf = "123.456.789-09",
                Tipo = TipoPessoa.Fisica,
                Endereco = new Endereco
                {
                    CEP = "12345-678",
                    Logradouro = "Rua Exemplo"
                },
                Telefones = new List<Telefone>
            {
                new Telefone { DDD = "11", Numero = "987654321" },
                new Telefone { DDD = "21", Numero = "123456789" }
            }
            };

            _validationBuilder = new ValidationBuilder<Cliente>(cliente)
                .AdicionarValidacao(c => c.Nome)
                .AdicionarValidacao(c => c.Cpf)
                .AdicionarValidacao(c => c.Tipo)
                .AdicionarValidacao(c => c.Endereco)
                .AdicionarValidacao(c => c.Telefones);
        }

        [Benchmark]
        public void ValidarClienteBenchmark()
        {
            _validationBuilder.ValidarPropriedades();
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<ClienteValidationBenchmark>();
        }
    }
}