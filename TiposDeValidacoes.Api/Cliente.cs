namespace TiposDeValidacoes.Api
{
    public enum TipoPessoa
    {
        NaoDefinido,
        Fisica,
        Juridica
    }

    public class Cliente
    {
        public string? Nome { get; set; }
        public string? Cpf { get; set; }
        public TipoPessoa? Tipo { get; set; }
        public Endereco? Endereco { get; set; }
    }

    public class Endereco
    {
        public string? CEP { get; set; }
        public string? Logradouro { get; set; }
    }
}