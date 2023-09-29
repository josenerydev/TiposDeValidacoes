using TiposDeValidacoes.Api;

public class ClienteServiceSimples
{
    public List<string> ProcessarCliente_v1(Cliente cliente)
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

    public string[] ProcessarCliente_v2(Cliente cliente)
    {
        const int maxErros = 10; // Define um número máximo de erros
        var erros = new string[maxErros];
        var erroCount = 0;

        var telefones = cliente.Telefones;
        var endereco = cliente.Endereco;

        // Validações do Cliente
        if (string.IsNullOrWhiteSpace(cliente.Nome))
            erros[erroCount++] = "Nome";
        if (string.IsNullOrWhiteSpace(cliente.Cpf) || cliente.Cpf.Length != 11)
            erros[erroCount++] = "CPF";
        if (cliente.Tipo == null || cliente.Tipo == TipoPessoa.NaoDefinido)
            erros[erroCount++] = "Tipo de pessoa";

        // Validações do Endereço
        if (endereco == null)
            erros[erroCount++] = "Endereço";
        else
        {
            if (string.IsNullOrWhiteSpace(endereco.CEP))
                erros[erroCount++] = "CEP";
            if (string.IsNullOrWhiteSpace(endereco.Logradouro))
                erros[erroCount++] = "Logradouro";
        }

        // Validações do Telefone
        if (telefones == null || telefones.Count == 0)
            erros[erroCount++] = "Telefone";
        else
        {
            var count = telefones.Count;
            for (int i = 0; i < count; i++)
            {
                var telefone = telefones[i];
                if (telefone == null)
                    erros[erroCount++] = "Telefone";
                else
                {
                    if (string.IsNullOrEmpty(telefone.DDD))
                        erros[erroCount++] = "DDD";
                    if (string.IsNullOrEmpty(telefone.Numero))
                        erros[erroCount++] = "Número telefone";
                }
            }
        }

        // Redimensiona o array para o número real de erros
        Array.Resize(ref erros, erroCount);
        return erros;
    }

    public List<string> ProcessarCliente_v3(Cliente cliente)
    {
        var erros = new List<string>();

        // Validações do Cliente
        if (cliente == null)
        {
            erros.Add("Cliente é nulo");
            return erros; // Se o cliente for nulo, não faz sentido verificar outras coisas
        }

        var telefones = cliente.Telefones;
        var endereco = cliente.Endereco;

        if (string.IsNullOrWhiteSpace(cliente.Nome)) erros.Add("Nome");
        if (string.IsNullOrWhiteSpace(cliente.Cpf) || cliente.Cpf.Length != 11) erros.Add("CPF");
        if (cliente.Tipo == null || cliente.Tipo == TipoPessoa.NaoDefinido) erros.Add("Tipo de pessoa");

        // Validações do Endereço
        if (endereco == null)
        {
            erros.Add("Endereço");
        }
        else
        {
            if (string.IsNullOrWhiteSpace(endereco.CEP)) erros.Add("CEP");
            if (string.IsNullOrWhiteSpace(endereco.Logradouro)) erros.Add("Logradouro");
        }

        // Validações do Telefone
        if (telefones == null || telefones.Count == 0)
        {
            erros.Add("Telefone");
        }
        else
        {
            foreach (var telefone in telefones)
            {
                if (telefone == null)
                {
                    erros.Add("Telefone");
                    continue; // se o telefone for nulo, não verifica os detalhes
                }

                if (string.IsNullOrEmpty(telefone.DDD)) erros.Add("DDD");
                if (string.IsNullOrEmpty(telefone.Numero)) erros.Add("Número telefone");
            }
        }

        return erros;
    }
}