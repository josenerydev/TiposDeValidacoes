using FluentValidation;

using TiposDeValidacoes.Api;

public class TelefoneValidator : AbstractValidator<Telefone>
{
    public TelefoneValidator()
    {
        RuleFor(t => t.DDD).NotEmpty().WithMessage("DDD é obrigatório.");
        RuleFor(t => t.Numero).NotEmpty().WithMessage("Número é obrigatório.");
    }
}

public class EnderecoValidator : AbstractValidator<Endereco>
{
    public EnderecoValidator()
    {
        RuleFor(e => e.CEP).NotEmpty().WithMessage("CEP é obrigatório.");
        RuleFor(e => e.Logradouro).NotEmpty().WithMessage("Logradouro é obrigatório.");
    }
}

public class ClienteValidator : AbstractValidator<Cliente>
{
    public ClienteValidator()
    {
        RuleFor(c => c.Nome).NotEmpty().WithMessage("Nome é obrigatório.");
        RuleFor(c => c.Cpf).NotEmpty().Length(11).WithMessage("CPF deve ter 11 dígitos.");
        RuleFor(c => c.Tipo).NotNull().NotEqual(TipoPessoa.NaoDefinido).WithMessage("Tipo de pessoa é inválido.");

        RuleFor(c => c.Endereco)
            .NotNull()
            .WithMessage("Endereço é obrigatório.")
            .DependentRules(() =>
            {
                RuleFor(c => c.Endereco!).SetValidator(new EnderecoValidator()); // Added null-forgiving operator
            });

        RuleFor(c => c.Telefones)
            .NotEmpty()
            .WithMessage("Telefones são obrigatórios.")
            .DependentRules(() =>
            {
                RuleForEach(c => c.Telefones!).SetValidator(new TelefoneValidator());
                RuleFor(c => c.Telefones).Must(list => !list.Contains(null)).WithMessage("A lista de telefones não pode conter itens nulos.");
            });
    }
}