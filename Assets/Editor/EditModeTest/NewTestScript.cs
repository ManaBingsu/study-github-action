using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class NewTestScript
    {
        [Test]
        public void SucceedTest()
        {
            Assert.AreEqual(0, 0);
        }

        [Test]
        public void FailTest()
        {
            Assert.AreEqual(0, 1);
        }
    }
}
