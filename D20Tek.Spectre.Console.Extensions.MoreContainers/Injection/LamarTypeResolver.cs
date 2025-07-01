//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Lamar;
using Spectre.Console.Cli;

namespace D20Tek.Spectre.Console.Extensions.Injection
{
    /// <summary>
    /// Type resolver for Spectre.Console that uses the SimpleInjector framework.
    /// </summary>
    public sealed class LamarTypeResolver : ITypeResolver, IDisposable
    {
        private readonly Container _container;

        /// <summary>
        /// Constructor that takes a container for SimpleInjector.
        /// </summary>
        /// <param name="container">Container to use in type resolution.</param>
        public LamarTypeResolver(Container container)
        {
            ArgumentNullException.ThrowIfNull(container, nameof(container));
            _container = container;
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
            return _container.GetInstance(type);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            GC.SuppressFinalize(this);

            if (_container is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
