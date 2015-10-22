﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NextLevelSeven.Test.Testing
{
    public static class AssertEnumerable
    {
        public static void AreEqual<T>(IEnumerable<T> expected, IEnumerable<T> observed)
        {
            var a = expected.ToList();
            var b = observed.ToList();
            Assert.IsTrue(a.Count == b.Count, "Count mismatch.");

            var length = a.Count;
            for (var i = 0; i < length; i++)
            {
                Assert.AreEqual(a[i], b[i], string.Format("Items at index {0} were not equal.", i));
            }
        }
    }
}