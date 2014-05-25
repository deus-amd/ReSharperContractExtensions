﻿using JetBrains.ReSharper.Intentions.CSharp.Test;
using NUnit.Framework;
using ReSharper.ContractExtensions.ContextActions;
using ReSharper.ContractExtensions.ContextActions.ContractsFor;

namespace ReSharper.ContractExtensions.Tests.ContractFor
{
    [TestFixture]
    public class AddContractForContextActionAvailabilityTest : CSharpContextActionAvailabilityTestBase<AddContractContextAction>
    {
        protected override string ExtraPath
        {
            get { return "ContractFor"; }
        }

        [TestCase("01_AvailableOnInterface")]
        [TestCase("02_AvailableOnAbstractClass")]
        [TestCase("03_UnavailableOnInterface")]
        [TestCase("04_UnavailableOnAbstractClass")]
        [TestCase("05_UnavailableOnConcreteClass")]
        [TestCase("06_AvailableOnPartiallyDefinedContractForInterface")]
        [TestCase("07_AvailableOnPartiallyDefinedContractForClass")]
        public void TestSimpleAvailability(string testSrc)
        {
            DoOneTest(testSrc);
        }

    }
}