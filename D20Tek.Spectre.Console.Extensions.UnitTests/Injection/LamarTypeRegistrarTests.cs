//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Injection;
using Lamar;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Injection
{
    [TestClass]
    public class LamarTypeRegistrarTests
    {
        public interface ITestService { };

        public class TestService : ITestService { };

        [TestMethod]
        public void Register()
        {
            // arrange
            var services = new ServiceRegistry();
            var registrar = new LamarTypeRegistrar(services);

            // act
            registrar.Register(typeof(ITestService), typeof(TestService));

            // assert
            Assert.IsTrue(services.Any(x => x.ServiceType == typeof(ITestService)));
            Assert.IsTrue(services.Any(x => x.ImplementationType == typeof(TestService)));
        }

        [TestMethod]
        public void RegisterInstance()
        {
            // arrange
            var services = new ServiceRegistry();
            var registrar = new LamarTypeRegistrar(services);
            var instance = new TestService();

            // act
            registrar.RegisterInstance(typeof(ITestService), instance);

            // assert
            Assert.IsTrue(services.Any(x => x.ServiceType == typeof(ITestService)));
            Assert.IsTrue(services.Any(x => x.ImplementationInstance == instance));
        }

        [TestMethod]
        public void RegisterLazy()
        {
            // arrange
            var services = new ServiceRegistry();
            var registrar = new LamarTypeRegistrar(services);

            // act
            registrar.RegisterLazy(typeof(ITestService), FactoryMethod);

            // assert
            Assert.IsTrue(services.Any(x => x.ServiceType == typeof(ITestService)));
            Assert.IsNotNull(services[0].ImplementationFactory);
        }

        [TestMethod]
        public void Build()
        {
            // arrange
            var services = new ServiceRegistry();
            var registrar = new LamarTypeRegistrar(services);

            registrar.Register(typeof(ITestService), typeof(TestService));

            // act
            var resolver = registrar.Build();

            // assert
            Assert.IsNotNull(resolver);
        }

        [TestMethod]
        public void RegisterScoped()
        {
            // arrange
            var services = new ServiceRegistry();
            var registrar = new LamarTypeRegistrar(services);

            // act
            registrar.WithLifetimes().RegisterScoped<ITestService, TestService>();

            // assert
            Assert.IsTrue(services.Any(x => x.ServiceType == typeof(ITestService)));
            Assert.IsTrue(services.Any(x => x.Lifetime == ServiceLifetime.Scoped));
            Assert.IsTrue(services.Any(x => x.ImplementationType == typeof(TestService)));
        }

        [ExcludeFromCodeCoverage]
        private ITestService FactoryMethod() => new TestService();
    }
}
