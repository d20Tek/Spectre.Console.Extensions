//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace D20Tek.Spectre.Console.Extensions.Injection
{
    public sealed class DependencyInjectionTypeRegistrar : ITypeRegistrar
    {
        private readonly IServiceCollection _builder;

        public DependencyInjectionTypeRegistrar(IServiceCollection builder)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        public ITypeResolver Build()
        {
            var provider = _builder.BuildServiceProvider();
            return new DependencyInjectionTypeResolver(provider);
        }

        public void Register(Type service, Type implementation)
        {
            ArgumentNullException.ThrowIfNull(service, nameof(service));
            ArgumentNullException.ThrowIfNull(implementation, nameof(implementation));

            _builder.AddSingleton(service, implementation);
        }

        public void RegisterInstance(Type service, object implementation)
        {
            ArgumentNullException.ThrowIfNull(service, nameof(service));
            ArgumentNullException.ThrowIfNull(implementation, nameof(implementation));

            _builder.AddSingleton(service, implementation);
        }

        public void RegisterLazy(Type service, Func<object> func)
        {
            ArgumentNullException.ThrowIfNull(service, nameof(service));
            ArgumentNullException.ThrowIfNull(func, nameof(func));

            _builder.AddSingleton(service, (provider) => func());
        }
    }
}
