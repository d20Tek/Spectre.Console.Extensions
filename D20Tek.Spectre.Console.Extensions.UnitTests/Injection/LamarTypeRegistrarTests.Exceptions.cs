//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Injection;
using Lamar;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Injection
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class LamarTypeRegistrarExceptionTests
    {
        public interface ITestService { };

        public class TestService : ITestService { };

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_WithNullStandardKernel()
        {
            // arrange

            // act
            _ = new LamarTypeRegistrar(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Register_WithNullServiceType()
        {
            // arrange
            var services = new ServiceRegistry();
            var registrar = new LamarTypeRegistrar(services);

            // act
            registrar.Register(null, typeof(TestService));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Register_WithNullImplementationType()
        {
            // arrange
            var services = new ServiceRegistry();
            var registrar = new LamarTypeRegistrar(services);

            // act
            registrar.Register(typeof(ITestService), null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisterInstance_WithNullType()
        {
            // arrange
            var services = new ServiceRegistry();
            var registrar = new LamarTypeRegistrar(services);

            // act
            registrar.RegisterInstance(null, new TestService());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisterInstance_WithNullImplementation()
        {
            // arrange
            var services = new ServiceRegistry();
            var registrar = new LamarTypeRegistrar(services);

            // act
            registrar.RegisterInstance(typeof(ITestService), null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisterLazy_WithNullType()
        {
            // arrange
            var services = new ServiceRegistry();
            var registrar = new LamarTypeRegistrar(services);

            // act
            registrar.RegisterLazy(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisterLazy_WithNullFactory()
        {
            // arrange
            var services = new ServiceRegistry();
            var registrar = new LamarTypeRegistrar(services);

            // act
            registrar.RegisterLazy(typeof(ITestService), null);
        }
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
    }
}
