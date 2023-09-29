using BenchmarkDotNet.Attributes;

using TiposDeValidacoes.Api;
using TiposDeValidacoes.Api.Services;

namespace ValidationBenchmark
{
    [MemoryDiagnoser]
    public class ClienteServiceBenchmark
    {
        private readonly ClienteServiceSimples _clienteServiceSimples = new ClienteServiceSimples();
        private readonly ClienteServiceValidationBuilder _clienteServiceValidationBuilderService = new ClienteServiceValidationBuilder();
        private readonly ClienteServiceFluentValidation _clienteServiceFluentValidationService = new ClienteServiceFluentValidation();

        [ParamsSource(nameof(Clientes))]
        public Cliente Cliente { get; set; }

        public static IEnumerable<Cliente> Clientes()
        {
            yield return new Cliente
            {
                Nome = "João",
                Cpf = "12345678909",
                Tipo = TipoPessoa.Fisica,
                Endereco = new Endereco
                {
                    CEP = "12345678",
                    Logradouro = "Rua Exemplo"
                },
                Telefones = new List<Telefone>
            {
                new Telefone { DDD = "11", Numero = "987654321" },
                new Telefone { DDD = "21", Numero = "123456789" }
            }
            };

            yield return new Cliente
            {
                Cpf = "12345678909",
                Tipo = TipoPessoa.Fisica,
                Endereco = new Endereco
                {
                    CEP = "12345678",
                    Logradouro = "Rua Exemplo"
                },
                Telefones = new List<Telefone>
            {
                new Telefone { DDD = "11", Numero = "987654321" },
                new Telefone { DDD = "21", Numero = "123456789" }
            }
            };

            yield return new Cliente
            {
                Cpf = "12345678909",
                Tipo = TipoPessoa.NaoDefinido,
                Endereco = new Endereco
                {
                    Logradouro = "Rua Exemplo"
                }
            };
        }

        [Benchmark]
        public List<string> ProcessarCliente_v1()
        {
            return _clienteServiceSimples.ProcessarCliente_v1(Cliente);
        }

        [Benchmark]
        public string[] ProcessarCliente_v2()
        {
            return _clienteServiceSimples.ProcessarCliente_v2(Cliente);
        }

        [Benchmark]
        public List<string> ProcessarCliente_v3()
        {
            return _clienteServiceSimples.ProcessarCliente_v1(Cliente);
        }

        //[Benchmark]
        //public List<string> ValidationBuilder()
        //{
        //    return _clienteServiceValidationBuilderService.ProcessarCliente(Cliente);
        //}

        //[Benchmark]
        //public List<string> FluentValidation()
        //{
        //    return _clienteServiceFluentValidationService.ProcessarCliente(Cliente);
        //}
    }
}