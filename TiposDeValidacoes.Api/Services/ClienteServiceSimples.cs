using TiposDeValidacoes.Api;

public class ClienteServiceSimples
{
    public List<string> ProcessarCliente(Cliente cliente)
    {
        var erros = new List<string>();
        var telefones = cliente.Telefones;

        // Validações do Cliente
        if (string.IsNullOrWhiteSpace(cliente.Nome))
            erros.Add("Nome");
        if (string.IsNullOrWhiteSpace(cliente.Cpf) || cliente.Cpf.Length != 11)
            erros.Add("CPF");
        if (cliente.Tipo == null || cliente.Tipo == TipoPessoa.NaoDefinido)
            erros.Add("Tipo de pessoa");

        // Validações do Endereço
        var endereco = cliente.Endereco;
        if (endereco == null)
            erros.Add("Endereço");
        else
        {
            if (string.IsNullOrWhiteSpace(endereco.CEP))
                erros.Add("CEP");
            if (string.IsNullOrWhiteSpace(endereco.Logradouro))
                erros.Add("Logradouro");
        }

        // Validações do Telefone
        if (telefones == null || telefones.Count == 0)
            erros.Add("Telefone");
        else
        {
            var count = telefones.Count;  // Armazene o count em uma variável local
            for (int i = 0; i < count; i++)  // Use um loop for em vez de foreach
            {
                var telefone = telefones[i];
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