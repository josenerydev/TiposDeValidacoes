using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace TiposDeValidacoes.Api
{
    public class ClienteService
    {
        public Dictionary<string, bool> ProcessarCliente(Cliente cliente)
        {
            var resultados = ValidarPropriedades(
               cliente,
                    (c => c.Cpf, valor => !string.IsNullOrWhiteSpace(valor as string) && valor.ToString().Length == 11),
                    (c => c.Nome, null), // Validacao padrão será usada
                    (c => c.Endereco.CEP, null), // Validacao padrão será usada
                    (c => c.Tipo, valor => (TipoPessoa)valor != TipoPessoa.NaoDefinido)
                );

            // Aqui você pode adicionar lógica adicional de processamento se necessário

            return resultados;
        }

        public Dictionary<string, bool> ValidarPropriedades<T>(
            T objeto, params (Expression<Func<T, object>> Propriedade, Func<object, bool> Validacao)[] propriedades)
        {
            var resultados = new Dictionary<string, bool>();

            foreach (var (prop, validacao) in propriedades)
            {
                var member = prop.Body as MemberExpression ?? ((UnaryExpression)prop.Body).Operand as MemberExpression;
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

                var isValid = validacao != null ? validacao(valor) : valor != null && !string.IsNullOrWhiteSpace(valor.ToString());
                resultados.Add(string.Join(".", path), isValid);
            }

            return resultados;
        }
    }
}