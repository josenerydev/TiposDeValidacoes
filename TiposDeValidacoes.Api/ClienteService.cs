using System.Linq.Expressions;
using System.Reflection;

namespace TiposDeValidacoes.Api
{
    public class PropertyValidation<T>
    {
        public Expression<Func<T, object>> Property { get; }
        public Func<object, bool> Validation { get; }

        public PropertyValidation(Expression<Func<T, object>> property, Func<object, bool> validation = null)
        {
            Property = property;
            Validation = validation ?? DefaultValidation;
        }

        private bool DefaultValidation(object value)
        {
            return value != null && !string.IsNullOrWhiteSpace(value.ToString());
        }
    }

    public class ClienteService
    {
        private static PropertyValidation<T> Validate<T>(Expression<Func<T, object>> property, Func<object, bool> validation = null)
        {
            return new PropertyValidation<T>(property, validation);
        }

        public Dictionary<string, bool> ProcessarCliente(Cliente cliente)
        {
            var resultados = ValidarPropriedades(
               cliente,
               Validate<Cliente>(c => c.Cpf, valor => !string.IsNullOrWhiteSpace(valor as string) && valor.ToString().Length == 11),
               Validate<Cliente>(c => c.Nome),
               Validate<Cliente>(c => c.Endereco.CEP),
               Validate<Cliente>(c => c.Tipo, valor => (TipoPessoa)valor != TipoPessoa.Juridica)
            ).Invalidos();

            // Aqui você pode adicionar lógica adicional de processamento se necessário

            return resultados;
        }

        public Dictionary<string, bool> ValidarPropriedades<T>(T objeto, params PropertyValidation<T>[] propriedades)
        {
            var resultados = new Dictionary<string, bool>();

            foreach (var propertyValidation in propriedades)
            {
                var member = propertyValidation.Property.Body as MemberExpression ?? ((UnaryExpression)propertyValidation.Property.Body).Operand as MemberExpression;
                if (member == null) throw new ArgumentException("A expressão deve ser uma propriedade.");

                var propertyInfo = member.Member as PropertyInfo;
                if (propertyInfo == null) throw new ArgumentException("A expressão deve ser uma propriedade.");

                object valor = objeto;
                var path = new List<string> { typeof(T).Name };
                foreach (var part in member.ToString().Split('.').Skip(1))
                {
                    if (valor == null) break;
                    propertyInfo = valor.GetType().GetProperty(part);
                    if (propertyInfo == null) throw new ArgumentException($"Propriedade {part} não encontrada.");
                    valor = propertyInfo.GetValue(valor);
                    path.Add(part);
                }

                var isValid = propertyValidation.Validation(valor);
                resultados.Add(string.Join(".", path), isValid);
            }

            return resultados;
        }
    }

    public static class DictionaryExtensions
    {
        public static Dictionary<string, bool> Invalidos(this Dictionary<string, bool> resultados)
        {
            return resultados.Where(r => !r.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        public static Dictionary<string, bool> Validos(this Dictionary<string, bool> resultados)
        {
            return resultados.Where(r => r.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
        }
    }
}