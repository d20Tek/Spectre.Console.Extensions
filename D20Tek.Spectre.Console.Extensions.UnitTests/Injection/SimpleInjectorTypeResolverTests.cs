//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Injection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleInjector;
using System;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Injection
{
    [TestClass]
    public class SimpleInjectorTypeResolverTests
    {
        public interface ITestService { };

        public class TestService : ITestService { };

        [TestMethod]
        public void Resolve_WithTypes()
        {
            // arrange
            var services = new Container();
            var registrar = new SimpleInjectorTypeRegistrar(services);

            registrar.Register(typeof(ITestService), typeof(TestService));
            var resolver = registrar.Build();

            // act
            var service = resolver.Resolve(typeof(ITestService));

            // assert
            Assert.IsNotNull(service);
            Assert.IsInstanceOfType(service, typeof(ITestService));
            Assert.IsInstanceOfType(service, typeof(TestService));
        }

        [TestMethod]
        public void Resolve_WithUnregisteredType()
        {
            // arrange
            var services = new Container();
            using var resolver = new SimpleInjectorTypeResolver(services);

            // act
            var service = resolver.Resolve(typeof(SimpleInjectorTypeResolver));

            // assert
            Assert.IsNull(service);
        }

        [TestMethod]
        public void Resolve_WithNullType()
        {
            // arrange
            var services = new Container();
            using var resolver = new SimpleInjectorTypeResolver(services);

            // act
            var service = resolver.Resolve(null);

            // assert
            Assert.IsNull(service);
        }

        [TestMethod]
        public void Resolve_WithFactory()
        {
            var services = new Container();
            var registrar = new SimpleInjectorTypeRegistrar(services);

            registrar.RegisterLazy(typeof(ITestService), FactoryMethod);
            var resolver = registrar.Build();

            // act
            var service = resolver.Resolve(typeof(ITestService));

            // assert
            Assert.IsNotNull(service);
            Assert.IsInstanceOfType(service, typeof(ITestService));
            Assert.IsInstanceOfType(service, typeof(TestService));
        }

        private TestService FactoryMethod() => new TestService();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        [ExcludeFromCodeCoverage]
        public void Constructor_WithNullServiceCollection()
        {
            // arrange

            // act
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            _ = new SimpleInjectorTypeResolver(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        }
    }
}
