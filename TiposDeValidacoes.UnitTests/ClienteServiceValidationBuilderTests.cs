using System.Collections.Generic;

using TiposDeValidacoes.Api;
using TiposDeValidacoes.Api.Services;

using Xunit;

namespace TiposDeValidacoes.Tests
{
    public class ClienteServiceValidationBuilderTests
    {
        private readonly ClienteServiceValidationBuilder _service;

        public ClienteServiceValidationBuilderTests()
        {
            _service = new ClienteServiceValidationBuilder();
        }

        [Fact]
        public void ProcessarCliente_ClienteValido_RetornaListaVazia()
        {
            // Arrange
            var cliente = new Cliente
            {
                Nome = "Teste",
                Cpf = "12345678901",
                Tipo = TipoPessoa.Fisica,
                Endereco = new Endereco { CEP = "12345678" },
                Telefones = new List<Telefone> { new Telefone { DDD = "11", Numero = "123456789" } }
            };

            // Act
            var resultados = _service.ProcessarCliente(cliente);

            // Assert
            Assert.Empty(resultados);
        }

        [Fact]
        public void ProcessarCliente_ClienteInvalido_RetornaErros()
        {
            // Arrange
            var cliente = new Cliente(); // Cliente sem propriedades obrigatórias

            // Act
            var resultados = _service.ProcessarCliente(cliente);

            // Assert
            Assert.Contains("Cliente.Nome", resultados);
            Assert.Contains("Cliente.Cpf", resultados);
            Assert.Contains("Cliente.Tipo", resultados);
            Assert.Contains("Cliente.Endereco", resultados);
            Assert.Contains("Cliente.Telefones", resultados);
        }

        [Fact]
        public void ProcessarCliente_CEPInvalido_RetornaErro()
        {
            // Arrange
            var cliente = new Cliente
            {
                Nome = "Nome válido",
                Cpf = "12345678901", // CPF válido com 11 dígitos
                Tipo = TipoPessoa.Fisica,
                Endereco = new Endereco
                {
                    // CEP inválido (nulo ou vazio)
                    Logradouro = "Logradouro válido"
                },
                Telefones = new List<Telefone>
                {
                    new Telefone { DDD = "11", Numero = "987654321" } // Telefone válido
                }
            };

            // Act
            var resultados = _service.ProcessarCliente(cliente);

            // Assert
            Assert.Single(resultados); // Espera-se apenas um erro
            Assert.Contains("Cliente.Endereco.CEP", resultados);
        }
    }
}