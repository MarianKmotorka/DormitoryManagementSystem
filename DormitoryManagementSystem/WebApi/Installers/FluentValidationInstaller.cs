using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Application.Common.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Installers
{
    public class FluentValidationInstaller : IInstaller
    {
        public FluentValidationInstaller()
        {
            ValidatorOptions.PropertyNameResolver = CamelCasePropertyNameResolver;
        }

        public IServiceCollection Install(IServiceCollection services, IConfiguration configuration)
        {
            services.AddValidatorsFromAssembly(typeof(IDormitoryDbContext).Assembly);
            return services;
        }

        private string CamelCasePropertyNameResolver(Type type, MemberInfo memberInfo, LambdaExpression expression)
        {
            if (expression != null)
            {
                var chain = FromExpression(expression);

                return chain.Count switch
                {
                    0 => "",
                    1 => ToCamelCase(chain.First()),
                    _ => string.Join(ValidatorOptions.PropertyChainSeparator, chain.Select(ToCamelCase))
                };
            }

            return ToCamelCase(memberInfo?.Name);
        }

        private IReadOnlyCollection<string> FromExpression(LambdaExpression expression)
        {
            var memberNames = new Stack<string>();

            var getMemberExp = new Func<Expression, MemberExpression>(toUnwrap =>
            {
                if (toUnwrap is UnaryExpression)
                {
                    return ((UnaryExpression)toUnwrap).Operand as MemberExpression;
                }

                return toUnwrap as MemberExpression;
            });

            var memberExp = getMemberExp(expression.Body);

            while (memberExp != null)
            {
                memberNames.Push(memberExp.Member.Name);
                memberExp = getMemberExp(memberExp.Expression);
            }

            return memberNames;
        }

        private string ToCamelCase(string value)
        {
            if (string.IsNullOrEmpty(value) || !char.IsUpper(value[0]))
            {
                return value;
            }

            var chars = value.ToCharArray();

            for (int i = 0; i < chars.Length; i++)
            {
                if (i == 1 && !char.IsUpper(chars[i]))
                    break;

                bool hasNext = i + 1 < chars.Length;
                if (i > 0 && hasNext && !char.IsUpper(chars[i + 1]))
                    break;

                chars[i] = char.ToLower(chars[i], CultureInfo.InvariantCulture);
            }

            return new string(chars);
        }
    }
}
