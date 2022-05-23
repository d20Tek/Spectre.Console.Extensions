//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Injection;
using LightInject;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Injection
{
    [TestClass]
    public class LightInjectTypeRegistrarTests
    {
        public interface ITestService { };

        public class TestService : ITestService { };

        [TestMethod]
        public void Register()
        {
            // arrange
            var services = new ServiceContainer();
            var registrar = new LightInjectTypeRegistrar(services);

            // act
            registrar.Register(typeof(ITestService), typeof(TestService));

            // assert
            Assert.IsNotNull(services.GetInstance<ITestService>());
        }

        [TestMethod]
        public void RegisterInstance()
        {
            // arrange
            var services = new ServiceContainer();
            var registrar = new LightInjectTypeRegistrar(services);
            var instance = new TestService();

            // act
            registrar.RegisterInstance(typeof(ITestService), instance);

            // assert
            Assert.IsNotNull(services.GetInstance<ITestService>());
        }

        [TestMethod]
        public void RegisterLazy()
        {
            // arrange
            var services = new ServiceContainer();
            var registrar = new LightInjectTypeRegistrar(services);

            // act
            registrar.RegisterLazy(typeof(ITestService), FactoryMethod);

            // assert
            Assert.IsNotNull(services.GetInstance<ITestService>());
        }

        [TestMethod]
        public void Build()
        {
            // arrange
            var services = new ServiceContainer();
            var registrar = new LightInjectTypeRegistrar(services);

            registrar.Register(typeof(ITestService), typeof(TestService));

            // act
            var resolver = registrar.Build();

            // assert
            Assert.IsNotNull(resolver);
        }

        [ExcludeFromCodeCoverage]
        private ITestService FactoryMethod() => new TestService();
    }
}
