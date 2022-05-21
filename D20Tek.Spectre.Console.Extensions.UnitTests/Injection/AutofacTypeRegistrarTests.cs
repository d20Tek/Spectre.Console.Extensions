//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Autofac;
using D20Tek.Spectre.Console.Extensions.Injection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Injection
{
    [TestClass]
    public class AutofacTypeRegistrarTests
    {
        public interface ITestService { };

        public class TestService : ITestService { };

        [TestMethod]
        public void Register()
        {
            // arrange
            var services = new ContainerBuilder();
            var registrar = new AutofacTypeRegistrar(services);

            // act
            registrar.Register(typeof(ITestService), typeof(TestService));

            // assert
            Assert.IsNotNull(services.Build().Resolve(typeof(ITestService)));
        }

        [TestMethod]
        public void RegisterInstance()
        {
            // arrange
            var services = new ContainerBuilder();
            var registrar = new AutofacTypeRegistrar(services);
            var instance = new TestService();

            // act
            registrar.RegisterInstance(typeof(ITestService), instance);

            // assert
            var result = services.Build().Resolve(typeof(ITestService));
            Assert.IsNotNull(result);
            Assert.AreEqual(instance, result);
        }

        [TestMethod]
        public void RegisterLazy()
        {
            // arrange
            var services = new ContainerBuilder();
            var registrar = new AutofacTypeRegistrar(services);

            // act
            registrar.RegisterLazy(typeof(ITestService), FactoryMethod);

            // assert
            Assert.IsNotNull(services.Build().Resolve(typeof(ITestService)));
        }

        [TestMethod]
        public void Build()
        {
            // arrange
            var services = new ContainerBuilder();
            var registrar = new AutofacTypeRegistrar(services);

            registrar.Register(typeof(ITestService), typeof(TestService));

            // act
            var resolver = registrar.Build();

            // assert
            Assert.IsNotNull(resolver);
        }

        [ExcludeFromCodeCoverage]
        private TestService FactoryMethod() => new TestService();
    }
}
