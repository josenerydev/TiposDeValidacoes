using TiposDeValidacoes.Api;

public class ClienteServiceSimples
{
    public List<string> ProcessarCliente(Cliente cliente)
    {
        var erros = new List<string>();

        // Validações do Cliente
        if (string.IsNullOrEmpty(cliente.Nome))
            erros.Add("Nome");
        if (string.IsNullOrEmpty(cliente.Cpf) || cliente.Cpf.Length != 11)
            erros.Add("CPF");
        if (cliente.Tipo == null || cliente.Tipo == TipoPessoa.NaoDefinido)
            erros.Add("Tipo de pessoa");

        // Validações do Endereço
        if (cliente.Endereco == null)
            erros.Add("Endereço");
        else
        {
            if (string.IsNullOrEmpty(cliente.Endereco.CEP))
                erros.Add("CEP");
            if (string.IsNullOrEmpty(cliente.Endereco.Logradouro))
                erros.Add("Logradouro");
        }

        // Validações do Telefone
        if (cliente.Telefones == null || cliente.Telefones.Count == 0)
            erros.Add("Telefone");
        else
        {
            foreach (var telefone in cliente.Telefones)
            {
                if (telefone == null)
                    erros.Add("Telefone");
                else
                {
                    if (string.IsNullOrEmpty(telefone.DDD))
                        erros.Add("DDD");
                    if (string.IsNullOrEmpty(telefone.Numero))
                        erros.Add("Número telefone");
                }
            }
        }

        return erros;
    }
}