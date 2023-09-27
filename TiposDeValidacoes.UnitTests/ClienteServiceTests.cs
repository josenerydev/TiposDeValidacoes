using System.Collections.Generic;

using TiposDeValidacoes.Api;

using Xunit;

namespace TiposDeValidacoes.UnitTests
{
    public class ClienteServiceTests
    {
        private readonly ClienteService _clienteService = new ClienteService();

        [Fact]
        public void Validacao_DeveSerBemSucedida()
        {
            // Arrange
            var cliente = new Cliente
            {
                Nome = "João Silva",
                Cpf = "12345678909",
                Tipo = TipoPessoa.Fisica, // assumindo que 1 é equivalente a Fisica
                Endereco = new Endereco
                {
                    CEP = "12345678",
                    Logradouro = "Rua das Flores"
                },
                Telefones = new List<Telefone>
                {
                    new Telefone { DDD = "11", Numero = "987654321" },
                    new Telefone { DDD = "21", Numero = "123456789" }
                }
            };

            // Act
            var resultados = _clienteService.ProcessarCliente(cliente);

            // Assert
            Assert.DoesNotContain(resultados, x => !x.Value);
        }

        [Fact]
        public void Validacao_CPFInvalido_DeveRetornarErro()
        {
            // Arrange
            var cliente = new Cliente
            {
                Nome = "João Silva",
                Cpf = "123",
                Tipo = TipoPessoa.Fisica,
                Endereco = new Endereco
                {
                    CEP = "12345678",
                    Logradouro = "Rua das Flores"
                }
            };

            // Act
            var resultados = _clienteService.ProcessarCliente(cliente);

            // Assert
            Assert.False(resultados["Cliente.Cpf"]);
        }

        [Fact]
        public void Validacao_TipoNaoDefinido_DeveRetornarErro()
        {
            // Arrange
            var cliente = new Cliente
            {
                Nome = "João Silva",
                Cpf = "12345678909",
                Tipo = TipoPessoa.NaoDefinido, // assumindo que 0 é equivalente a NaoDefinido
                Endereco = new Endereco
                {
                    CEP = "12345678",
                    Logradouro = "Rua das Flores"
                },
                Telefones = new List<Telefone>
                {
                    new Telefone { DDD = "11", Numero = "987654321" },
                    new Telefone { DDD = "21", Numero = "123456789" }
                }
            };

            // Act
            var resultados = _clienteService.ProcessarCliente(cliente);

            // Assert
            Assert.False(resultados["Cliente.Tipo"]);
        }

        [Fact]
        public void Validacao_CEPVazio_DeveRetornarErro()
        {
            // Arrange
            var cliente = new Cliente
            {
                Nome = "João Silva",
                Cpf = "12345678909",
                Tipo = TipoPessoa.Fisica,
                Endereco = new Endereco
                {
                    CEP = "",
                    Logradouro = "Rua das Flores"
                },
                Telefones = new List<Telefone>
                {
                    new Telefone { DDD = "11", Numero = "987654321" },
                    new Telefone { DDD = "21", Numero = "123456789" }
                }
            };

            // Act
            var resultados = _clienteService.ProcessarCliente(cliente);

            // Assert
            Assert.False(resultados["Cliente.Endereco.CEP"]);
        }

        [Fact]
        public void Validacao_NomeNulo_DeveRetornarErro()
        {
            // Arrange
            var cliente = new Cliente
            {
                Nome = null,
                Cpf = "12345678909",
                Tipo = TipoPessoa.Fisica,
                Endereco = new Endereco
                {
                    CEP = "12345678",
                    Logradouro = "Rua das Flores"
                }
            };

            // Act
            var resultados = _clienteService.ProcessarCliente(cliente);

            // Assert
            Assert.False(resultados["Cliente.Nome"]);
        }

        [Fact]
        public void Validacao_EnderecoNulo_DeveRetornarErro()
        {
            // Arrange
            var cliente = new Cliente
            {
                Nome = "João Silva",
                Cpf = "12345678909",
                Tipo = TipoPessoa.Fisica,
                Endereco = null
            };

            // Act
            var resultados = _clienteService.ProcessarCliente(cliente);

            // Assert
            Assert.False(resultados["Cliente.Endereco"]);
        }

        [Fact]
        public void Validacao_TelefonesValidos_DeveSerBemSucedida()
        {
            // Arrange
            var cliente = new Cliente
            {
                Nome = "João Silva",
                Cpf = "12345678909",
                Tipo = TipoPessoa.Fisica,
                Endereco = new Endereco
                {
                    CEP = "12345678",
                    Logradouro = "Rua das Flores"
                },
                Telefones = new List<Telefone>
                {
                    new Telefone { DDD = "11", Numero = "987654321" },
                    new Telefone { DDD = "21", Numero = "123456789" }
                } // Lista de telefones válida
            };

            // Act
            var resultados = _clienteService.ProcessarCliente(cliente);

            // Assert
            Assert.DoesNotContain(resultados, x => !x.Value);
        }

        [Fact]
        public void Validacao_TelefonesNulo_DeveRetornarErro()
        {
            // Arrange
            var cliente = new Cliente
            {
                Nome = "João Silva",
                Cpf = "12345678909",
                Tipo = TipoPessoa.Fisica,
                Endereco = new Endereco
                {
                    CEP = "12345678",
                    Logradouro = "Rua das Flores"
                },
                Telefones = null // Lista de telefones é nula
            };

            // Act
            var resultados = _clienteService.ProcessarCliente(cliente);

            // Assert
            Assert.False(resultados["Cliente.Telefones"]);
        }

        [Fact]
        public void Validacao_TelefoneNaListaNulo_DeveRetornarErro()
        {
            // Arrange
            var cliente = new Cliente
            {
                Nome = "João Silva",
                Cpf = "12345678909",
                Tipo = TipoPessoa.Fisica,
                Endereco = new Endereco
                {
                    CEP = "12345678",
                    Logradouro = "Rua das Flores"
                },
                Telefones = new List<Telefone> { null } // Um dos telefones na lista é nulo
            };

            // Act
            var resultados = _clienteService.ProcessarCliente(cliente);

            // Assert
            Assert.False(resultados["Cliente.Telefones"]);
        }
    }
}