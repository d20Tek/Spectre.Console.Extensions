//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Spectre.Console.Cli;

namespace D20Tek.Spectre.Console.Extensions.Testing
{
    internal class FrontendTypeRegistrar : ITypeRegistrarFrontend
    {
        private readonly ITypeRegistrar _registrar;

        internal FrontendTypeRegistrar(ITypeRegistrar registrar)
        {
            ArgumentNullException.ThrowIfNull(registrar, nameof(registrar));
            _registrar = registrar;
        }

        public void Register<TService, TImplementation>()
            where TImplementation : TService
        {
            _registrar.Register(typeof(TService), typeof(TImplementation));
        }

        public void RegisterInstance<TImplementation>(TImplementation instance)
        {
            ArgumentNullException.ThrowIfNull(instance, nameof(instance));
            _registrar.RegisterInstance(typeof(TImplementation), instance);
        }

        public void RegisterInstance<TService, TImplementation>(TImplementation instance)
            where TImplementation : TService
        {
            ArgumentNullException.ThrowIfNull(instance, nameof(instance));
            _registrar.RegisterInstance(typeof(TService), instance);
        }
    }
}
