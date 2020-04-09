﻿namespace Microsoft.Azure.Cosmos.Tests.Json
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Azure.Cosmos.CosmosElements;
    using Microsoft.Azure.Cosmos.CosmosElements.Numbers;
    using Microsoft.Azure.Cosmos.Json;
    using Microsoft.Azure.Cosmos.Tests.Poco;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public sealed class SerializerTests
    {
        [TestMethod]
        public void TestPoco()
        {
            Person person = Person.GetRandomPerson();
            ReadOnlyMemory<byte> result = Serializer.Serialize(person);

            CosmosObject cosmosObject = (CosmosObject)CosmosElement.CreateFromBuffer(result);

            Assert.IsTrue(cosmosObject.TryGetValue("Name", out CosmosString personName));
            Assert.AreEqual(person.Name, personName.Value);

            Assert.IsTrue(cosmosObject.TryGetValue("Age", out CosmosNumber personAge));
            Assert.AreEqual(person.Age, personAge.Value);
        }

        [TestMethod]
        public void TestList()
        {
            List<int> list = new List<int> { 1, 2, 3 };
            ReadOnlyMemory<byte> result = Serializer.Serialize(list);

            CosmosArray cosmosArray = (CosmosArray)CosmosElement.CreateFromBuffer(result);
            Assert.AreEqual(list[0], (cosmosArray[0] as CosmosNumber).Value);
            Assert.AreEqual(list[1], (cosmosArray[1] as CosmosNumber).Value);
            Assert.AreEqual(list[2], (cosmosArray[2] as CosmosNumber).Value);
        }

        [TestMethod]
        public void TestNull()
        {
            ReadOnlyMemory<byte> result = Serializer.Serialize((object)null);
            CosmosNull cosmosNull = (CosmosNull)CosmosElement.CreateFromBuffer(result);
        }

        [TestMethod]
        public void TestBool()
        {
            {
                ReadOnlyMemory<byte> result = Serializer.Serialize(true);
                CosmosBoolean cosmosBoolean = (CosmosBoolean)CosmosElement.CreateFromBuffer(result);
                Assert.IsTrue(cosmosBoolean.Value);
            }

            {
                ReadOnlyMemory<byte> result = Serializer.Serialize(false);
                CosmosBoolean cosmosBoolean = (CosmosBoolean)CosmosElement.CreateFromBuffer(result);
                Assert.IsFalse(cosmosBoolean.Value);
            }
        }

        [TestMethod]
        public void TestNumber()
        {
            {
                Number64 value = 32;
                ReadOnlyMemory<byte> result = Serializer.Serialize(value);
                CosmosNumber64 cosmosNumber = (CosmosNumber64)CosmosElement.CreateFromBuffer(result);
                Assert.AreEqual(expected: value, actual: cosmosNumber.Value);
            }

            {
                sbyte value = 32;
                ReadOnlyMemory<byte> result = Serializer.Serialize(value);
                CosmosInt8 cosmosNumber = (CosmosInt8)CosmosElement.CreateFromBuffer(result);
                Assert.AreEqual(expected: value, actual: cosmosNumber.Value);
            }

            {
                short value = 32;
                ReadOnlyMemory<byte> result = Serializer.Serialize(value);
                CosmosInt16 cosmosNumber = (CosmosInt16)CosmosElement.CreateFromBuffer(result);
                Assert.AreEqual(expected: value, actual: cosmosNumber.Value);
            }

            {
                int value = 32;
                ReadOnlyMemory<byte> result = Serializer.Serialize(value);
                CosmosInt32 cosmosNumber = (CosmosInt32)CosmosElement.CreateFromBuffer(result);
                Assert.AreEqual(expected: value, actual: cosmosNumber.Value);
            }

            {
                long value = 32;
                ReadOnlyMemory<byte> result = Serializer.Serialize(value);
                CosmosInt64 cosmosNumber = (CosmosInt64)CosmosElement.CreateFromBuffer(result);
                Assert.AreEqual(expected: value, actual: cosmosNumber.Value);
            }

            {
                uint value = 32;
                ReadOnlyMemory<byte> result = Serializer.Serialize(value);
                CosmosUInt32 cosmosNumber = (CosmosUInt32)CosmosElement.CreateFromBuffer(result);
                Assert.AreEqual(expected: value, actual: cosmosNumber.Value);
            }

            {
                float value = 32.1337f;
                ReadOnlyMemory<byte> result = Serializer.Serialize(value);
                CosmosFloat32 cosmosNumber = (CosmosFloat32)CosmosElement.CreateFromBuffer(result);
                Assert.AreEqual(expected: value, actual: cosmosNumber.Value);
            }

            {
                double value = 32.1337;
                ReadOnlyMemory<byte> result = Serializer.Serialize(value);
                CosmosFloat64 cosmosNumber = (CosmosFloat64)CosmosElement.CreateFromBuffer(result);
                Assert.AreEqual(expected: value, actual: cosmosNumber.Value);
            }
        }

        [TestMethod]
        public void TestString()
        {
            string value = "asdf";
            ReadOnlyMemory<byte> result = Serializer.Serialize(value);
            CosmosString cosmosString = (CosmosString)CosmosElement.CreateFromBuffer(result);
            Assert.AreEqual(expected: value, actual: cosmosString.Value);
        }

        [TestMethod]
        public void TestBinary()
        {
            ReadOnlyMemory<byte> value = new byte[] { 1, 2, 3 };
            ReadOnlyMemory<byte> result = Serializer.Serialize(value);
            CosmosBinary cosmosBinary = (CosmosBinary)CosmosElement.CreateFromBuffer(result);
            Assert.IsTrue(cosmosBinary.Value.ToArray().SequenceEqual(value.ToArray()));
        }

        [TestMethod]
        public void TestGuid()
        {
            Guid value = Guid.NewGuid();
            ReadOnlyMemory<byte> result = Serializer.Serialize(value);
            CosmosGuid cosmosGuid = (CosmosGuid)CosmosElement.CreateFromBuffer(result);
            Assert.AreEqual(expected: value, actual: cosmosGuid.Value);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestUnknownStruct()
        {
            DateTime value = DateTime.MinValue;
            ReadOnlyMemory<byte> result = Serializer.Serialize(value);
        }
    }
}