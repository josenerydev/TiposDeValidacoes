using System.Linq.Expressions;

namespace TiposDeValidacoes.Api
{
    public class ValidationBuilder<T>
    {
        private readonly T _objeto;
        private readonly List<PropertyValidation<T>> _validations = new();

        public ValidationBuilder(T objeto)
        {
            _objeto = objeto;
        }

        public ValidationBuilder<T> AdicionarValidacao(Expression<Func<T, object>> property, Func<object, bool> validation = null)
        {
            _validations.Add(new PropertyValidation<T>(property, validation));
            return this;
        }

        public List<string> ValidarPropriedades()
        {
            return _validations
                .Select(v => PropertyValidation<T>.ValidateProperty(_objeto, v))
                .Where(propertyPath => propertyPath != null)
                .ToList();
        }
    }

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
            if (value is IEnumerable<object> collection)
                return collection.All(v => v != null);

            return value != null && !string.IsNullOrWhiteSpace(value.ToString());
        }

        public static string ValidateProperty(T objeto, PropertyValidation<T> propertyValidation)
        {
            var member = propertyValidation.Property.Body as MemberExpression ??
                         ((UnaryExpression)propertyValidation.Property.Body).Operand as MemberExpression;
            if (member == null) throw new ArgumentException("A expressão deve ser uma propriedade.");

            object valor = objeto;
            var path = new List<string> { typeof(T).Name };
            foreach (var part in member.ToString().Split('.').Skip(1))
            {
                if (valor == null) break;
                var propertyInfo = valor.GetType().GetProperty(part);
                if (propertyInfo == null) throw new ArgumentException($"Propriedade {part} não encontrada.");
                valor = propertyInfo.GetValue(valor);
                path.Add(part);
            }

            var isValid = propertyValidation.Validation(valor);
            return isValid ? null : string.Join(".", path);
        }
    }
}