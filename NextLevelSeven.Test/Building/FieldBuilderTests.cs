﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Building;

namespace NextLevelSeven.Test.Building
{
    [TestClass]
    public class FieldBuilderTests
    {
        [TestMethod]
        public void FieldBuilder_CanBuildRepetitions_Individually()
        {
            var builder = new MessageBuilder()[1][3];
            var repetition1 = Randomized.String();
            var repetition2 = Randomized.String();

            builder
                .FieldRepetition(1, repetition1)
                .FieldRepetition(2, repetition2);
            Assert.AreEqual(string.Format("{0}~{1}", repetition1, repetition2), builder.ToString(),
                @"Unexpected result.");
        }

        [TestMethod]
        public void FieldBuilder_CanBuildRepetitions_OutOfOrder()
        {
            var builder = new MessageBuilder()[1][3];
            var repetition1 = Randomized.String();
            var repetition2 = Randomized.String();

            builder
                .FieldRepetition(2, repetition2)
                .FieldRepetition(1, repetition1);
            Assert.AreEqual(string.Format("{0}~{1}", repetition1, repetition2), builder.ToString(),
                @"Unexpected result.");
        }

        [TestMethod]
        public void FieldBuilder_CanBuildRepetitions_Sequentially()
        {
            var builder = new MessageBuilder()[1][3];
            var repetition1 = Randomized.String();
            var repetition2 = Randomized.String();

            builder
                .FieldRepetitions(3, repetition1, repetition2);
            Assert.AreEqual(string.Format("~~{0}~{1}", repetition1, repetition2), builder.ToString(),
                @"Unexpected result.");
        }

        [TestMethod]
        public void FieldBuilder_CanBuildComponents_Individually()
        {
            var builder = new MessageBuilder()[1][3];
            var component1 = Randomized.String();
            var component2 = Randomized.String();

            builder
                .Component(1, 1, component1)
                .Component(1, 2, component2);
            Assert.AreEqual(string.Format("{0}^{1}", component1, component2), builder.ToString(),
                @"Unexpected result.");
        }

        [TestMethod]
        public void FieldBuilder_CanBuildComponents_OutOfOrder()
        {
            var builder = new MessageBuilder()[1][3];
            var component1 = Randomized.String();
            var component2 = Randomized.String();

            builder
                .Component(1, 2, component2)
                .Component(1, 1, component1);
            Assert.AreEqual(string.Format("{0}^{1}", component1, component2), builder.ToString(),
                @"Unexpected result.");
        }

        [TestMethod]
        public void FieldBuilder_CanBuildComponents_Sequentially()
        {
            var builder = new MessageBuilder()[1][3];
            var component1 = Randomized.String();
            var component2 = Randomized.String();

            builder
                .Components(1, component1, component2);
            Assert.AreEqual(string.Format("{0}^{1}", component1, component2), builder.ToString(),
                @"Unexpected result.");
        }

        [TestMethod]
        public void FieldBuilder_CanBuildSubcomponents_Individually()
        {
            var builder = new MessageBuilder()[1][3];
            var subcomponent1 = Randomized.String();
            var subcomponent2 = Randomized.String();

            builder
                .Subcomponent(1, 1, 1, subcomponent1)
                .Subcomponent(1, 1, 2, subcomponent2);
            Assert.AreEqual(string.Format("{0}&{1}", subcomponent1, subcomponent2), builder.ToString(),
                @"Unexpected result.");
        }

        [TestMethod]
        public void FieldBuilder_CanBuildSubcomponents_OutOfOrder()
        {
            var builder = new MessageBuilder()[1][3];
            var subcomponent1 = Randomized.String();
            var subcomponent2 = Randomized.String();

            builder
                .Subcomponent(1, 1, 2, subcomponent2)
                .Subcomponent(1, 1, 1, subcomponent1);
            Assert.AreEqual(string.Format("{0}&{1}", subcomponent1, subcomponent2), builder.ToString(),
                @"Unexpected result.");
        }

        [TestMethod]
        public void FieldBuilder_CanBuildSubcomponents_Sequentially()
        {
            var builder = new MessageBuilder()[1][3];
            var subcomponent1 = Randomized.String();
            var subcomponent2 = Randomized.String();

            builder
                .Subcomponents(1, 1, 1, subcomponent1, subcomponent2);
            Assert.AreEqual(string.Format("{0}&{1}", subcomponent1, subcomponent2), builder.ToString(),
                @"Unexpected result.");
        }
    }
}