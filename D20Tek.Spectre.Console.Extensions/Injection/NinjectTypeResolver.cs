//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Ninject;
using Spectre.Console.Cli;

namespace D20Tek.Spectre.Console.Extensions.Injection
{
    /// <summary>
    /// Type resolver for Spectre.Console that uses the Ninject framework.
    /// </summary>
    public sealed class NinjectTypeResolver : ITypeResolver, IDisposable
    {
        private readonly StandardKernel _provider;

        /// <summary>
        /// Constructor that takes a standard kernel for Ninject.
        /// </summary>
        /// <param name="provider">Provider to use in type resolution.</param>
        public NinjectTypeResolver(StandardKernel provider)
        {
            ArgumentNullException.ThrowIfNull(provider, nameof(provider));
            _provider = provider;
        }

        /// <summary>
        /// Resolves an instance of the specified type.
        /// </summary>
        /// <param name="type">The type to resolve.</param>
        /// <returns>An instance of the specified type, or null if no registration for that
        /// type exists.</returns>
        public object? Resolve(Type? type)
        {
            if (type == null)
            {
                return null;
            }

            try
            {
                var instances = _provider.GetAll(type);
                return instances.FirstOrDefault();
            }
            catch (ActivationException)
            {
                // if type isn't registered, Ninject throws an exception,
                // so we manage that here and return null in that case.
                return null;
            }
        }

        /// <inheritdoc/>
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
