//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Spectre.Console.Extensions.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Spectre.Console.Cli;
using System;

namespace D20Tek.Spectre.Console.Extensions.UnitTests.Services
{
    [TestClass]
    public class TypeRegistrarExtensionsTests
    {
        [TestMethod]
        public void WithConsoleVerbosityWriter()
        {
            // arrange
            var mockRegistrar = CreateMockRegistrar();

            // act
            mockRegistrar.Object.WithConsoleVerbosityWriter();

            // assert
            mockRegistrar.Verify(o => o.Register(typeof(IVerbosityWriter), typeof(ConsoleVerbosityWriter)), Times.Once);
        }

        private Mock<ITypeRegistrar> CreateMockRegistrar()
        {
            var registrar = new Mock<ITypeRegistrar>();
            registrar.Setup(p => p.Register(It.IsAny<Type>(), It.IsAny<Type>()))
                     .Verifiable();

            return registrar;
        }
    }
}
