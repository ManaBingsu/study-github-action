using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class PlayModeTestEx
    {
        [Test]
        public void PlayModeSucceedTest()
        {
            Assert.AreEqual(0, 0);
        }

        [Test]
        public void PlayModeFailTest()
        {
            Assert.AreEqual(0, 1);
        }

        [Test]
        public void PlayModeFailTest2()
        {
            Assert.AreEqual(0, 1);
        }
    }
}
