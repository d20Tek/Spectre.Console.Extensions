//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Injection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using System;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Injection
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class NinjectTypeRegistrarExceptionTests
    {
        public interface ITestService { };

        public class TestService : ITestService { };

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        [TestMethod]
        public void Create_WithNullStandardKernel()
        {
            // arrange

            // act
            Assert.ThrowsExactly<ArgumentNullException>(() => new NinjectTypeRegistrar(null));
        }

        [TestMethod]
        public void Register_WithNullServiceType()
        {
            // arrange
            var services = new StandardKernel();
            var registrar = new NinjectTypeRegistrar(services);

            // act
            Assert.ThrowsExactly<ArgumentNullException>(() => registrar.Register(null, typeof(TestService)));
        }

        [TestMethod]
        public void Register_WithNullImplementationType()
        {
            // arrange
            var services = new StandardKernel();
            var registrar = new NinjectTypeRegistrar(services);

            // act
            Assert.ThrowsExactly<ArgumentNullException>(() => registrar.Register(typeof(ITestService), null));
        }

        [TestMethod]
        public void RegisterInstance_WithNullType()
        {
            // arrange
            var services = new StandardKernel();
            var registrar = new NinjectTypeRegistrar(services);

            // act
            Assert.ThrowsExactly<ArgumentNullException>(() => registrar.RegisterInstance(null, new TestService()));
        }

        [TestMethod]
        public void RegisterInstance_WithNullImplementation()
        {
            // arrange
            var services = new StandardKernel();
            var registrar = new NinjectTypeRegistrar(services);

            // act
            Assert.ThrowsExactly<ArgumentNullException>(() => registrar.RegisterInstance(typeof(ITestService), null));
        }

        [TestMethod]
        public void RegisterLazy_WithNullType()
        {
            // arrange
            var services = new StandardKernel();
            var registrar = new NinjectTypeRegistrar(services);

            // act
            Assert.ThrowsExactly<ArgumentNullException>(() => registrar.RegisterLazy(null, null));
        }

        [TestMethod]
        public void RegisterLazy_WithNullFactory()
        {
            // arrange
            var services = new StandardKernel();
            var registrar = new NinjectTypeRegistrar(services);

            // act
            Assert.ThrowsExactly<ArgumentNullException>(() => registrar.RegisterLazy(typeof(ITestService), null));
        }
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
    }
}
