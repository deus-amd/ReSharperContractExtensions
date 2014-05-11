﻿using JetBrains.ReSharper.Intentions.CSharp.Test;
using NUnit.Framework;
using ReSharper.ContractExtensions.ContextActions;

namespace ReSharper.ContractExtensions.Tests.Preconditions
{
    [TestFixture]
    public class RequiresContextActionExecuteTest : CSharpContextActionExecuteTestBase<RequiresContextAction>
    {
        protected override string ExtraPath
        {
            get { return "Requires"; }
        }

        [TestCase("Execution")]
        [TestCase("ExecutionWithExistingUsing")]
        [TestCase("ExecutionWithSpecifiedOrder")]
        [Test]
        public void TestSimpleExecution(string testSrc)
        {
            DoOneTest(testSrc);
        }
    }
}