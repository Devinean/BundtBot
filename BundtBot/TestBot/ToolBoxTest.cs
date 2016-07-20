﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BundtBot.BundtBot;
using BundtBot.BundtBot.Utility;

namespace TestBot {
    [TestClass]
    public class ToolBoxTest {
        [TestMethod]
        public void LevenshteinTest1() {
            Assert.AreEqual(0, ToolBox.Levenshtein("x", "x"));
        }
    }
}
