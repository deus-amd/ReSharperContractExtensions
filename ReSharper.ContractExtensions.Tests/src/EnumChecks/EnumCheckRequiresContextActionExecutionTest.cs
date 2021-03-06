﻿using JetBrains.ReSharper.FeaturesTestFramework.Intentions;
using NUnit.Framework;
using ReSharper.ContractExtensions.ContextActions;
using ReSharper.ContractExtensions.ContextActions.EnumChecks;
using ReSharper.ContractExtensions.ContextActions.Requires;

namespace ReSharper.ContractExtensions.Tests.Preconditions
{
    [TestFixture]
    public class EnumCheckRequiresContextActionExecutionTest : RequiresContextActionExecuteTestBase<EnumCheckRequiresContextAction>
    {
        protected override string RelativeTestDataPath
        {
            get { return "Intentions/ContextActions/EnumCheckRequires"; }
        }

        protected override string ExtraPath
        {
            get { return "EnumCheckRequires"; }
        }

        [TestCase("ExecutionForCustomEnum")]
        [TestCase("ExecutionForNullableCustomEnum")]
        [TestCase("ExecutionForDotNetEnum")]
        public void TestSimpleExecution(string testSrc)
        {
            DoOneTest(testSrc);
        }

    }
}