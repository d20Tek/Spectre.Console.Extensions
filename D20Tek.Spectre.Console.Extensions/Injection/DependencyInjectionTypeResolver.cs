//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Spectre.Console.Cli;

namespace D20Tek.Spectre.Console.Extensions.Injection
{
    public class DependencyInjectionTypeResolver : ITypeResolver, IDisposable
    {
        private readonly IServiceProvider _provider;

        public DependencyInjectionTypeResolver(IServiceProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public object? Resolve(Type? type)
        {
            if (type == null)
            {
                return null;
            }

            return _provider.GetService(type);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);

            if (_provider is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
