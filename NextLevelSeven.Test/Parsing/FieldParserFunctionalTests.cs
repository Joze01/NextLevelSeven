﻿using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;
using NextLevelSeven.Parsing;
using NextLevelSeven.Test.Testing;

namespace NextLevelSeven.Test.Parsing
{
    [TestClass]
    public class FieldParserFunctionalTests : ParsingTestFixture
    {
        [TestMethod]
        public void Field_Delimiter_CanBeCloned()
        {
            var field = Message.Parse(ExampleMessages.Minimum)[1][1];
            var clone = field.Clone();
            Assert.AreNotSame(field, clone, "Cloned field is the same referenced object.");
            Assert.AreEqual(field.Value, clone.Value, "Cloned field has different contents.");
        }

        [TestMethod]
        public void Field_Delimiter_CanGetValue()
        {
            var field = Message.Parse(ExampleMessages.Minimum)[1][1];
            Assert.AreEqual("|", field.Value);
        }

        [TestMethod]
        public void Field_Delimiter_CanGetNullSegmentValue()
        {
            var field = Message.Parse(ExampleMessages.Minimum)[2][1];
            Assert.IsNull(field.Value);
        }

        [TestMethod]
        public void Field_Delimiter_CanNotSetNullSegmentValue()
        {
            var message = Message.Parse(ExampleMessages.Minimum);
            var field = message[1][1];
            AssertAction.Throws<ElementException>(() => field.Value = null);
        }

        [TestMethod]
        public void Field_Encoding_CanBeCloned()
        {
            var field = Message.Parse(ExampleMessages.Minimum)[1][2];
            var clone = field.Clone();
            Assert.AreNotSame(field, clone, "Cloned field is the same referenced object.");
            Assert.AreEqual(field.Value, clone.Value, "Cloned field has different contents.");
        }

        [TestMethod]
        public void Field_Encoding_CanGetValues()
        {
            var field = Message.Parse(ExampleMessages.Minimum)[1][2];
            AssertArray.AreEqual(new[] {"^", "~", "\\", "&"}, field.Values.ToArray());
        }

        [TestMethod]
        public void Field_Encoding_CanNotSetValues()
        {
            var field = Message.Parse(ExampleMessages.Minimum)[1][2];
            var values = new[] {"^", "~", "\\", "&"};
            AssertAction.Throws<ElementException>(() => field.Values = values);
        }

        [TestMethod]
        public void Field_Encoding_HasNoDelimiter()
        {
            var field = Message.Parse(ExampleMessages.Minimum)[1][2];
            Assert.AreEqual('\0', field.Delimiter);
        }

        [TestMethod]
        public void Field_Encoding_HasNoDescendants()
        {
            var field = Message.Parse(ExampleMessages.Minimum)[1][2];
            Assert.AreEqual(0, field.Descendants.Count());
        }

        [TestMethod]
        public void Field_Encoding_ThrowsOnDescendantAccess()
        {
            var field = Message.Parse(ExampleMessages.Minimum)[1][2];
            string result = null;
            AssertAction.Throws<ParserException>(() => result = field[2].Value);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Field_Delimiter_HasNoDelimiter()
        {
            var field = Message.Parse(ExampleMessages.Minimum)[1][1];
            Assert.AreEqual('\0', field.Delimiter);
        }

        [TestMethod]
        public void Field_Delimiter_HasNoDescendants()
        {
            var field = Message.Parse(ExampleMessages.Minimum)[1][1];
            Assert.AreEqual(0, field.Descendants.Count());
        }

        [TestMethod]
        public void Field_Delimiter_CanNotChangeValues()
        {
            var field = Message.Parse(ExampleMessages.Minimum)[1][1];
            var values = new[] {"$", "@"};
            AssertAction.Throws<ElementException>(() => field.Values = values);
        }

        [TestMethod]
        public void Field_Delimiter_ThrowsOnDescendantAccess()
        {
            var field = Message.Parse(ExampleMessages.Minimum)[1][1];
            string result = null;
            AssertAction.Throws<ParserException>(() => result = field[2].Value);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Field_Delimiter_ThrowsOnGenericDescendantAccess()
        {
            IElementParser field = Message.Parse(ExampleMessages.Minimum)[1][1];
            IElementParser result = null;
            AssertAction.Throws<ParserException>(() => result = field[2]);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Field_Delimiter_HasOneValue()
        {
            var field = Message.Parse(ExampleMessages.Minimum)[1][1];
            Assert.AreEqual(1, field.ValueCount);
            Assert.AreEqual(field.Value, field.GetValue());
            Assert.AreEqual(1, field.Values.Count());
            Assert.AreEqual(1, field.GetValues().Count());
        }

        [TestMethod]
        public void Field_CanGetAncestor()
        {
            var segment = Message.Parse(ExampleMessages.Standard)[1];
            var field = segment[3];
            Assert.AreSame(segment, field.Ancestor);
        }

        [TestMethod]
        public void Field_CanGetIsolatedValue()
        {
            var field = Message.Parse(ExampleMessages.Minimum)[1][3];
            var val0 = Mock.String();
            field.Value = val0;
            var newField = field.Clone();
            Assert.AreEqual(field.Value, newField.Value);
            Assert.AreEqual(1, field.Values.Count());
        }

        [TestMethod]
        public void Field_CanGetIsolatedNullValue()
        {
            var field = Message.Parse(ExampleMessages.Minimum)[1][3];
            var newField = field.Clone();
            Assert.IsNull(newField.Value);
        }

        [TestMethod]
        public void Field_CanInsertRepetitions()
        {
            var element = Message.Parse(ExampleMessages.Minimum)[1][3];
            element.Values = new[] { Mock.String(), Mock.String(), Mock.String(), Mock.String() };
            var value = Mock.String();
            element.Insert(1, value);
            Assert.AreEqual(5, element.ValueCount);
            Assert.AreEqual(value, element[1].Value);
        }

        [TestMethod]
        public void Field_CanInsertElementRepetitions()
        {
            var otherElement = Message.Parse(Mock.Message())[1][3][1];
            var element = Message.Parse(Mock.Message())[1][3];
            element.Values = new[] { Mock.String(), Mock.String(), Mock.String(), Mock.String() };
            element.Insert(1, otherElement);
            Assert.AreEqual(5, element.ValueCount);
            Assert.AreEqual(otherElement.Value, element[1].Value);
        }

        [TestMethod]
        public void Field_CanMoveRepetitions()
        {
            var element = Message.Parse(ExampleMessages.Minimum)[1][3];
            element.Values = new[] {Mock.String(), Mock.String(), Mock.String(), Mock.String()};
            var newMessage = element.Clone();
            newMessage[2].Move(3);
            Assert.AreEqual(element[2].Value, newMessage[3].Value);
        }

        [TestMethod]
        public void Field_Throws_WhenIndexedBelowOne()
        {
            var element = Message.Parse(ExampleMessages.Standard)[1][3];
            string value = null;
            AssertAction.Throws<ParserException>(() => { value = element[0].Value; });
            Assert.IsNull(value);
        }

        [TestMethod]
        public void Field_HasCorrectIndex()
        {
            var field = Message.Parse(ExampleMessages.Standard)[1][3];
            Assert.AreEqual(3, field.Index, "Index doesn't match.");
        }

        [TestMethod]
        public void Field_HasCorrectIndex_ByExtension()
        {
            var field = Message.Parse(ExampleMessages.Standard)[1].Field(3);
            Assert.AreEqual(3, field.Index, "Index doesn't match.");
        }

        [TestMethod]
        public void Field_CanBeCloned()
        {
            var field = Message.Parse(ExampleMessages.Standard)[1][3];
            var clone = field.Clone();
            Assert.AreNotSame(field, clone, "Cloned field is the same referenced object.");
            Assert.AreEqual(field.Value, clone.Value, "Cloned field has different contents.");
        }

        [TestMethod]
        public void Field_CanBeClonedGenerically()
        {
            var field = (IElement)Message.Parse(ExampleMessages.Standard)[1][3];
            var clone = field.Clone();
            Assert.AreNotSame(field, clone, "Cloned field is the same referenced object.");
            Assert.AreEqual(field.Value, clone.Value, "Cloned field has different contents.");
        }

        [TestMethod]
        public void Field_CanBeErased()
        {
            var field = (IElement)Message.Parse(ExampleMessages.Standard)[1][3];
            field.Erase();
            Assert.IsNull(field.Value);
        }

        [TestMethod]
        public void Field_CanAddDescendantsAtEnd()
        {
            var field = Message.Parse(ExampleMessages.Standard)[2][3];
            var count = field.ValueCount;
            var id = Mock.String();
            field[count + 1].Value = id;
            Assert.AreEqual(count + 1, field.ValueCount,
                @"Number of elements after appending at the end of a field is incorrect.");
        }

        [TestMethod]
        public void Field_CanGetRepetitions()
        {
            var repetitions = Message.Parse(ExampleMessages.Standard)[8][13].Repetitions;
            Assert.AreEqual(2, repetitions.Count());
        }

        [TestMethod]
        public void Field_CanGetRepetitionsGenerically()
        {
            var repetitions = (Message.Parse(ExampleMessages.Standard)[8][13] as IField).Repetitions;
            Assert.AreEqual(2, repetitions.Count());
        }

        [TestMethod]
        public void Field_CanGetRepetitionsByIndexer()
        {
            var repetition = Message.Parse(ExampleMessages.Standard)[8][13][2];
            Assert.AreEqual(@"^ORN^CP", repetition.Value);
        }

        [TestMethod]
        public void Field_CanDeleteRepetition()
        {
            var message = Message.Parse("MSH|^~\\&|\rTST|123~456|789~012");
            var field = message[2][1];
            ElementExtensions.Delete(field, 1);
            Assert.AreEqual("MSH|^~\\&|\rTST|456|789~012", message.Value, @"Message was modified unexpectedly.");
        }

        [TestMethod]
        public void Field_WillPointToCorrectValue_WhenOtherFieldChanges()
        {
            var message = Message.Parse(String.Format(@"MSH|^~\&|{0}|{1}", Mock.String(), Mock.String()));
            var msh3 = message[1][3];
            var msh4 = message[1][4];
            var newMsh3Value = Mock.String();
            var newMsh4Value = Mock.String();

            message.Value = String.Format(@"MSH|^~\&|{0}|{1}", newMsh3Value, newMsh4Value);
            Assert.AreEqual(newMsh3Value, msh3.Value, @"MSH-3 was not the expected value after changing MSH.");
            Assert.AreEqual(newMsh4Value, msh4.Value, @"MSH-4 was not the expected value after changing MSH.");
        }

        [TestMethod]
        public void Field_WithNoSignificantDescendants_ShouldNotClaimToHaveSignificantDescendants()
        {
            var message = Message.Parse();
            Assert.IsFalse(message[1][3].HasSignificantDescendants(),
                @"Element claims to have descendants when it should not.");
        }

        [TestMethod]
        public void Field_WillHaveValuesInterpretedAsDoubleQuotes()
        {
            var message = Message.Parse();
            message[1][3].Value = HL7.Null;
            Assert.AreEqual(HL7.Null, message[1][3].Value, @"Value of two double quotes was not interpreted as null.");
            Assert.IsTrue(message[1][3].Exists, @"Explicitly set null value must appear to exist.");
        }

        [TestMethod]
        public void Field_CanWriteStringValue()
        {
            var field = Message.Parse(ExampleMessages.Standard)[1][3];
            var value = Mock.String();
            field.Value = value;
            Assert.AreEqual(value, field.Value, "Value mismatch after write.");
        }

        [TestMethod]
        public void Field_CanWriteNullValue()
        {
            var field = Message.Parse(ExampleMessages.Standard)[1][3];
            var value = Mock.String();
            field.Value = value;
            field.Value = null;
            Assert.IsNull(field.Value, "Value mismatch after write.");
        }
    }
}