//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Injection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Injection
{
    [TestClass]
    public class TypeResolverExtensionsTests
    {
        public interface ITestService { };

        public class TestService : ITestService { };

        [TestMethod]
        public void Resolve_WithTypes()
        {
            // arrange
            var services = new ServiceCollection();
            var registrar = new DependencyInjectionTypeRegistrar(services);

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
        public void Resolve_WithNullType()
        {
            // arrange
            var services = new ServiceCollection();
            using var resolver = new DependencyInjectionTypeResolver(services.BuildServiceProvider());

            // act
            var service = resolver.Resolve(null);

            // assert
            Assert.IsNull(service);
        }

        [TestMethod]
        public void Resolve_WithFactory()
        {
            var services = new ServiceCollection();
            var registrar = new DependencyInjectionTypeRegistrar(services);

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
        [ExcludeFromCodeCoverage]
        public void Constructor_WithNullServiceCollection()
        {
            // arrange

            // act
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            Assert.ThrowsExactly<ArgumentNullException>(() => new DependencyInjectionTypeResolver(null));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        }
    }
}
