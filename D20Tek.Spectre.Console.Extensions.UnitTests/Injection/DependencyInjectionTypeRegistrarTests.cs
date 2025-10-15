//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Injection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Injection
{
    [TestClass]
    public class DependencyInjectionTypeRegistrarTests
    {
        public interface ITestService { };

        public class TestService : ITestService { };

        [TestMethod]
        public void Register()
        {
            // arrange
            var services = new ServiceCollection();
            var registrar = new DependencyInjectionTypeRegistrar(services);

            // act
            registrar.Register(typeof(ITestService), typeof(TestService));

            // assert
            Assert.AreEqual(1, services.Count());
            Assert.IsTrue(services.Any(x => x.Lifetime == ServiceLifetime.Singleton));
            Assert.IsTrue(services.Any(x => x.ServiceType == typeof(ITestService)));
            Assert.IsTrue(services.Any(x => x.ImplementationType == typeof(TestService)));
        }

        [TestMethod]
        public void Register_AsTransient()
        {
            // arrange
            var services = new ServiceCollection();
            var registrar = new DependencyInjectionTypeRegistrar(services, ServiceLifetime.Transient);

            // act
            registrar.Register(typeof(ITestService), typeof(TestService));

            // assert
            Assert.AreEqual(1, services.Count());
            Assert.IsTrue(services.Any(x => x.Lifetime == ServiceLifetime.Transient));
            Assert.IsTrue(services.Any(x => x.ServiceType == typeof(ITestService)));
            Assert.IsTrue(services.Any(x => x.ImplementationType == typeof(TestService)));
        }

        [TestMethod]
        public void RegisterInstance()
        {
            // arrange
            var services = new ServiceCollection();
            var registrar = new DependencyInjectionTypeRegistrar(services);
            var instance = new TestService();

            // act
            registrar.RegisterInstance(typeof(ITestService), instance);

            // assert
            Assert.AreEqual(1, services.Count());
            Assert.IsTrue(services.Any(x => x.ServiceType == typeof(ITestService)));
            Assert.IsFalse(services.Any(x => x.ImplementationType == typeof(TestService)));
            Assert.IsTrue(services.Any(x => x.ImplementationInstance == instance));
        }

        [TestMethod]
        public void RegisterInstance_AsTransient_StillRegistersAsSingleton()
        {
            // arrange
            var services = new ServiceCollection();
            var registrar = new DependencyInjectionTypeRegistrar(services, ServiceLifetime.Transient);
            var instance = new TestService();

            // act
            registrar.RegisterInstance(typeof(ITestService), instance);

            // assert
            Assert.AreEqual(1, services.Count());
            Assert.IsTrue(services.Any(x => x.Lifetime == ServiceLifetime.Singleton));
            Assert.IsTrue(services.Any(x => x.ServiceType == typeof(ITestService)));
            Assert.IsFalse(services.Any(x => x.ImplementationType == typeof(TestService)));
            Assert.IsTrue(services.Any(x => x.ImplementationInstance == instance));
        }

        [TestMethod]
        public void RegisterLazy()
        {
            // arrange
            var services = new ServiceCollection();
            var registrar = new DependencyInjectionTypeRegistrar(services);

            // act
            registrar.RegisterLazy(typeof(ITestService), FactoryMethod);

            // assert
            Assert.AreEqual(1, services.Count());
            Assert.IsTrue(services.Any(x => x.ServiceType == typeof(ITestService)));
            Assert.IsFalse(services.Any(x => x.ImplementationType == typeof(TestService)));
            Assert.IsNotNull(services.First().ImplementationFactory);
        }

        [TestMethod]
        public void Build()
        {
            // arrange
            var services = new ServiceCollection();
            var registrar = new DependencyInjectionTypeRegistrar(services);

            registrar.Register(typeof(ITestService), typeof(TestService));

            // act
            var resolver = registrar.Build();

            // assert
            Assert.IsNotNull(resolver);
        }

        [ExcludeFromCodeCoverage]
        private TestService FactoryMethod() => new TestService();

        [TestMethod]
        public void RegisterLazy_WithNullFactory()
        {
            // arrange
            var services = new ServiceCollection();
            var registrar = new DependencyInjectionTypeRegistrar(services);

            // act
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            Assert.ThrowsExactly<ArgumentNullException>([ExcludeFromCodeCoverage] () =>
                registrar.RegisterLazy(typeof(ITestService), null));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        }
    }
}
