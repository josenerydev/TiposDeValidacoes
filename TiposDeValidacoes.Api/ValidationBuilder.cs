using System.Linq.Expressions;
using System.Reflection;

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

        public Dictionary<string, ValidationResult> ValidarPropriedades()
        {
            return PropertyValidation<T>.ValidarPropriedades(_objeto, _validations.ToArray());
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

        public static Dictionary<string, ValidationResult> ValidarPropriedades(T objeto, params PropertyValidation<T>[] propriedades)
        {
            var resultados = new Dictionary<string, ValidationResult>();

            foreach (var propertyValidation in propriedades)
            {
                var validationResult = ValidateProperty(objeto, propertyValidation);
                resultados.Add(validationResult.PropertyPath, validationResult);
            }

            return resultados;
        }

        private static ValidationResult ValidateProperty(T objeto, PropertyValidation<T> propertyValidation)
        {
            var member = propertyValidation.Property.Body as MemberExpression ??
                         ((UnaryExpression)propertyValidation.Property.Body).Operand as MemberExpression;
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
            var propertyPath = string.Join(".", path);
            var errorMessage = isValid ? null : $"A propriedade {propertyPath} é inválida.";

            return new ValidationResult(isValid, errorMessage, propertyPath);
        }
    }

    public class ValidationResult
    {
        public bool IsValid { get; }
        public string ErrorMessage { get; }
        public string PropertyPath { get; }

        public ValidationResult(bool isValid, string errorMessage, string propertyPath)
        {
            IsValid = isValid;
            ErrorMessage = errorMessage;
            PropertyPath = propertyPath;
        }
    }

    public static class ValidationExtensions
    {
        public static Dictionary<string, ValidationResult> Invalidos(this Dictionary<string, ValidationResult> resultados)
        {
            return resultados.Where(r => !r.Value.IsValid).ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        public static Dictionary<string, ValidationResult> Validos(this Dictionary<string, ValidationResult> resultados)
        {
            return resultados.Where(r => r.Value.IsValid).ToDictionary(pair => pair.Key, pair => pair.Value);
        }
    }
}