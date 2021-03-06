﻿//
// This file is part of Biological Signals Acquisition Framework (BSA-F).
// Copyright © Adriano Repetti 2016.
//
// BSA-F is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// BSA-F is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with BSA-F.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using Bsa.Instrumentation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bsa.Hardware.Tests
{
    [TestClass]
    public sealed class DeviceFeatureTests
    {
        [TestMethod]
        public void EqualityIsByName()
        {
            Assert.IsTrue(new Feature("Test").Equals(new Feature("Test")));
            Assert.IsTrue(new Feature("Test") == new Feature("Test"));

            var feature1 = new Feature("Test1");
            var feature2 = new Feature("Test2");

            Assert.IsFalse(feature1.Equals(feature2));
            Assert.IsTrue(feature1 != feature2);

            Assert.IsFalse(feature1.Equals(null));
            Assert.IsTrue(feature1 != null);
            Assert.IsTrue(null != feature1);

            Assert.IsFalse(feature1 == feature2);
            Assert.IsFalse(feature1.Equals(feature2));
        }

        [TestMethod]
        public void EqualityIsCaseInsensitive()
        {
            Assert.IsTrue(new Feature("Test") == new Feature("test"));
        }

        [TestMethod]
        public void LeadingAndTrailingSpacesAreIgnored()
        {
            Assert.IsTrue(new Feature("Test  ") == new Feature("  Test"));
        }

        [TestMethod]
        public void HasValidEquivalentName()
        {
            Assert.AreEqual("test", new Feature("test!").EquivalentName);
            Assert.AreEqual("test123", new Feature("test123_測試").EquivalentName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EquivalentNameIsAlphanumericUsAscii()
        {
            new Feature("測試");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void FeatureMustBeAssociatedWithProperObject()
        {
            var feature = new Feature(typeof(DerivedTestDevice), "Test");
            using (var device = new TestDevice())
            {
                // It fails because feature is associated with DerivedTestDevice and we're testing
                // against base class TestDevice which doesn't "know" about this feature
                device.Features.IsAvailable(feature);
            }
        }

        [TestMethod]
        public void CanCheckForFeatures()
        {
            using (var device = new DerivedTestDevice())
            {
                Assert.IsTrue(device.Features.IsAvailable(TestFeature0), "Features check with method in base class.");
                Assert.IsTrue(device.Features.IsAvailable(TestFeature1), "Features check with virtual method call failed.");
                Assert.IsTrue(device.Features.IsEnabled(TestFeature1), "Features check with static method call failed.");
                Assert.IsTrue(device.Features.IsAvailable(TestFeature2), "Features check with instance method call failed.");
            }
        }

        [TestMethod]
        public void UseDefaultIfFeatureIsUnspecified()
        {
            using (var device = new DerivedTestDevice())
            {
                Assert.IsFalse(device.Features.IsAvailable(TestFeature3));
                Assert.IsFalse(device.Features.IsEnabled(TestFeature3));
                Assert.IsFalse(device.Features.IsAvailableAndEnabled(TestFeature3));
            }
        }

        private static readonly Feature TestFeature0 = new Feature(typeof(TestDevice), "0");
        private static readonly Feature TestFeature1 = new Feature(typeof(TestDevice), "1");
        private static readonly Feature TestFeature2 = new Feature(typeof(DerivedTestDevice), "2");
        private static readonly Feature TestFeature3 = new Feature(typeof(DerivedTestDevice), "3");

        private class TestDevice : NonInstrumentedTestDevice
        {
            protected virtual bool IsFeature0Available()
            {
                return true;
            }

            protected virtual bool IsFeature1Available()
            {
                return false;
            }
        }

        private sealed class DerivedTestDevice : TestDevice
        {
            protected override bool IsFeature1Available()
            {
                return true;
            }

            private static bool IsFeature1Enabled()
            {
                return true;
            }

            private bool IsFeature2Available()
            {
                return true;
            }
        }
    }
}
