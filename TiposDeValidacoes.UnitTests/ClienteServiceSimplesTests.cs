using TiposDeValidacoes.Api;

namespace TiposDeValidacoes.Tests
{
    public class ClienteServiceSimplesTests
    {
        private readonly ClienteServiceSimples _service;

        public ClienteServiceSimplesTests()
        {
            _service = new ClienteServiceSimples();
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
                Endereco = new Endereco { CEP = "12345678", Logradouro = "Rua Teste" },
                Telefones = new List<Telefone> { new Telefone { DDD = "11", Numero = "123456789" } }
            };

            // Act
            var erros = _service.ProcessarCliente(cliente);

            // Assert
            Assert.Empty(erros);
        }

        [Fact]
        public void ProcessarCliente_ClienteInvalido_RetornaErros()
        {
            // Arrange
            var cliente = new Cliente(); // Cliente sem propriedades obrigatórias

            // Act
            var erros = _service.ProcessarCliente(cliente);

            // Assert
            Assert.Contains("Nome", erros);
            Assert.Contains("CPF", erros);
            Assert.Contains("Tipo de pessoa", erros);
            Assert.Contains("Endereço", erros);
            Assert.Contains("Telefone", erros);
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
            var erros = _service.ProcessarCliente(cliente);

            // Assert
            Assert.Single(erros); // Espera-se apenas um erro
            Assert.Contains("CEP", erros);
        }
    }
}