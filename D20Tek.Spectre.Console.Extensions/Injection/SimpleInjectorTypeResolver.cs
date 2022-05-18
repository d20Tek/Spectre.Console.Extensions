//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using SimpleInjector;
using Spectre.Console.Cli;

namespace D20Tek.Spectre.Console.Extensions.Injection
{
    /// <summary>
    /// Type resolver for Spectre.Console that uses the SimpleInjector framework.
    /// </summary>
    public sealed class SimpleInjectorTypeResolver : ITypeResolver, IDisposable
    {
        private readonly Container _container;

        /// <summary>
        /// Constructor that takes a container for SimpleInjector.
        /// </summary>
        /// <param name="container">Container to use in type resolution.</param>
        public SimpleInjectorTypeResolver(Container container)
        {
            ArgumentNullException.ThrowIfNull(container, nameof(container));
            this._container = container;
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
                return this._container.GetInstance(type);
            }
            catch (ActivationException)
            {
                // if type isn't registered, SimpleInjector throws an exception,
                // so we manage that here and return null in that case.
                return null;
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            GC.SuppressFinalize(this);

            if (this._container is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
