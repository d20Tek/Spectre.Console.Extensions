//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Autofac;
using Spectre.Console.Cli;

namespace D20Tek.Spectre.Console.Extensions.Injection
{
    /// <summary>
    /// Type resolver for Spectre.Console that uses the Autofac DI framework.
    /// </summary>
    public sealed class AutofacTypeResolver : ITypeResolver, IDisposable
    {
        private readonly ILifetimeScope _scope;

        /// <summary>
        /// Constructor that takes a service provider instance.
        /// </summary>
        /// <param name="scope">Scope provider to use in type resolution.</param>
        public AutofacTypeResolver(ILifetimeScope scope)
        {
            ArgumentNullException.ThrowIfNull(scope, nameof(scope));
            _scope = scope;
        }

        /// <summary>
        /// Resolves an instance of the specified type.
        /// </summary>
        /// <param name="type">The type to resolve.</param>
        /// <returns>An instance of the specified type, or null if no registration for that
        /// type exists.</returns>
        public object? Resolve(Type? type)
        {
            if (type == null) return null;
            return _scope.IsRegistered(type) ? _scope.Resolve(type) : null;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            GC.SuppressFinalize(this);

            if (_scope is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
