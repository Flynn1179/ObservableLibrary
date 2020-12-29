using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Flynn1179.Observable.Tests
{
    /// <summary>
    /// Tests for <see cref="ObservableObject"/>.
    /// </summary>
    public class ObservableObjectTests
    {
        /// <summary>
        /// Tests for <see cref="ObservableObject.OnPropertyChanging(string)"/>.
        /// </summary>
        [TestFixture]
        public class OnPropertyChanging
        {
            /// <summary>
            /// Test that if a valid property name is passed to OnPropertyChanging, it raises the event.
            /// </summary>
            [Test]
            public void ValidPropertyName()
            {
                ObservableObject testObject = new TestObservableObject();
                string propertyChanging = null;
                int eventsRaised = 0;
                testObject.PropertyChanging += (sender, e) => { eventsRaised++; propertyChanging = e.PropertyName; };

                Helper.InvokeProtected(testObject, "OnPropertyChanging", nameof(TestObservableObject.Property));

                Assert.AreEqual(1, eventsRaised);
                Assert.AreEqual(nameof(TestObservableObject.Property), propertyChanging);
            }

            /// <summary>
            /// Test that if null is passed to OnPropertyChanging, an ArgumentNullException is thrown.
            /// </summary>
            [Test]
            public void Null()
            {
                ObservableObject testObject = new TestObservableObject();
                string propertyChanging = null;
                int eventsRaised = 0;
                testObject.PropertyChanging += (sender, e) => { eventsRaised++; propertyChanging = e.PropertyName; };

                Assert.Throws<ArgumentNullException>(() => Helper.InvokeProtected(testObject, "OnPropertyChanging", null));

                Assert.AreEqual(0, eventsRaised);
            }

            /// <summary>
            /// Test that if a property name that is not a named property on the target is passed to OnPropertyChanging, an ArgumentException is thrown.
            /// </summary>
            [Test]
            public void UnknownPropertyName()
            {
                ObservableObject testObject = new TestObservableObject();
                string propertyChanging = null;
                int eventsRaised = 0;
                testObject.PropertyChanging += (sender, e) => { eventsRaised++; propertyChanging = e.PropertyName; };

                Assert.Throws<ArgumentException>(() => Helper.InvokeProtected(testObject, "OnPropertyChanging", "NotAProperty"));

                Assert.AreEqual(0, eventsRaised);
            }

            /// <summary>
            /// Test that if the correct name of an indexer including square brackets is passed to OnPropertyChanging, it raises the event.
            /// </summary>
            [Test]
            public void ValidIndexerName()
            {
                ObservableObject testObject = new TestObservableObject();
                string propertyChanging = null;
                int eventsRaised = 0;
                testObject.PropertyChanging += (sender, e) => { eventsRaised++; propertyChanging = e.PropertyName; };

                Helper.InvokeProtected(testObject, "OnPropertyChanging", "Item[]");

                Assert.AreEqual(1, eventsRaised);
                Assert.AreEqual("Item[]", propertyChanging);
            }

            /// <summary>
            /// Test that if the name of the indexer is passed to OnPropertyChanging without the square brackets, an ArgumentException is thrown.
            /// </summary>
            [Test]
            public void IndexerNameWithoutBrackets()
            {
                ObservableObject testObject = new TestObservableObject();
                string propertyChanging = null;
                int eventsRaised = 0;
                testObject.PropertyChanging += (sender, e) => { eventsRaised++; propertyChanging = e.PropertyName; };

                Assert.Throws<ArgumentException>(() => Helper.InvokeProtected(testObject, "OnPropertyChanging", "Item"));

                Assert.AreEqual(0, eventsRaised);
            }

            /// <summary>
            /// Tests that if a valid property name is passed to OnPropertyChanging, but with square brackets when it's not an indexer, an ArgumentException is thrown.
            /// </summary>
            [Test]
            public void ValidPropertyNameWithIndexerBrackets()
            {
                ObservableObject testObject = new TestObservableObject();
                string propertyChanging = null;
                int eventsRaised = 0;
                testObject.PropertyChanging += (sender, e) => { eventsRaised++; propertyChanging = e.PropertyName; };

                Assert.Throws<ArgumentException>(() => Helper.InvokeProtected(testObject, "OnPropertyChanging", "Property[]"));

                Assert.AreEqual(0, eventsRaised);
            }
        }

        /// <summary>
        /// Tests for <see cref="ObservableObject.OnPropertyChanged(string)"/>.
        /// </summary>
        [TestFixture]
        public class OnPropertyChanged
        {
            /// <summary>
            /// Test that if a valid property name is passed to OnPropertyChanged, it raises the event.
            /// </summary>
            [Test]
            public void ValidPropertyName()
            {
                ObservableObject testObject = new TestObservableObject();
                string propertyChanged = null;
                int eventsRaised = 0;
                testObject.PropertyChanged += (sender, e) => { eventsRaised++; propertyChanged = e.PropertyName; };

                Helper.InvokeProtected(testObject, "OnPropertyChanged", nameof(TestObservableObject.Property));

                Assert.AreEqual(1, eventsRaised);
                Assert.AreEqual(nameof(TestObservableObject.Property), propertyChanged);
            }

            /// <summary>
            /// Test that if null is passed to OnPropertyChanged, an ArgumentNullException is thrown.
            /// </summary>
            [Test]
            public void Null()
            {
                ObservableObject testObject = new TestObservableObject();
                string propertyChanged = null;
                int eventsRaised = 0;
                testObject.PropertyChanged += (sender, e) => { eventsRaised++; propertyChanged = e.PropertyName; };

                Assert.Throws<ArgumentNullException>(() => Helper.InvokeProtected(testObject, "OnPropertyChanged", null));

                Assert.AreEqual(0, eventsRaised);
            }

            /// <summary>
            /// Test that if a property name that is not a named property on the target is passed to OnPropertyChanged, an ArgumentException is thrown.
            /// </summary>
            [Test]
            public void UnknownPropertyName()
            {
                ObservableObject testObject = new TestObservableObject();
                string propertyChanged = null;
                int eventsRaised = 0;
                testObject.PropertyChanged += (sender, e) => { eventsRaised++; propertyChanged = e.PropertyName; };

                Assert.Throws<ArgumentException>(() => Helper.InvokeProtected(testObject, "OnPropertyChanged", "NotAProperty"));

                Assert.AreEqual(0, eventsRaised);
            }

            /// <summary>
            /// Test that if the correct name of an indexer including square brackets is passed to OnPropertyChanged, it raises the event.
            /// </summary>
            [Test]
            public void ValidIndexerName()
            {
                ObservableObject testObject = new TestObservableObject();
                string propertyChanged = null;
                int eventsRaised = 0;
                testObject.PropertyChanged += (sender, e) => { eventsRaised++; propertyChanged = e.PropertyName; };

                Helper.InvokeProtected(testObject, "OnPropertyChanged", "Item[]");

                Assert.AreEqual(1, eventsRaised);
                Assert.AreEqual("Item[]", propertyChanged);
            }

            /// <summary>
            /// Test that if the name of the indexer is passed to OnPropertyChanged without the square brackets, an ArgumentException is thrown.
            /// </summary>
            [Test]
            public void IndexerNameWithoutBrackets()
            {
                ObservableObject testObject = new TestObservableObject();
                string propertyChanged = null;
                int eventsRaised = 0;
                testObject.PropertyChanged += (sender, e) => { eventsRaised++; propertyChanged = e.PropertyName; };

                Assert.Throws<ArgumentException>(() => Helper.InvokeProtected(testObject, "OnPropertyChanged", "Item"));

                Assert.AreEqual(0, eventsRaised);
            }

            /// <summary>
            /// Tests that if a valid property name is passed to OnPropertyChanged, but with square brackets when it's not an indexer, an ArgumentException is thrown.
            /// </summary>
            [Test]
            public void ValidPropertyNameWithIndexerBrackets()
            {
                ObservableObject testObject = new TestObservableObject();
                string propertyChanged = null;
                int eventsRaised = 0;
                testObject.PropertyChanged += (sender, e) => { eventsRaised++; propertyChanged = e.PropertyName; };

                Assert.Throws<ArgumentException>(() => Helper.InvokeProtected(testObject, "OnPropertyChanged", "Property[]"));

                Assert.AreEqual(0, eventsRaised);
            }
        }

        /// <summary>
        /// Tests for <see cref="ObservableObject.Set(ref string, string, string)"/>.
        /// </summary>
        [TestFixture]
        public class Set_Field_Value_PropertyName
        {
            private MethodInfo method;

            /// <summary>
            /// Finds the method under test.
            /// </summary>
            [OneTimeSetUp]
            public void FindMethod()
            {
                this.method = typeof(ObservableObject).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                    .FirstOrDefault(
                        method => method.GetParameters() is ParameterInfo[] paramInfo
                        && paramInfo.Length == 3
                        && Type.Equals(typeof(string).MakeByRefType(), paramInfo[0].ParameterType)
                        && Type.Equals(typeof(string), paramInfo[1].ParameterType)
                        && Type.Equals(typeof(string), paramInfo[2].ParameterType));
                Assert.IsNotNull(this.method);
            }

            /// <summary>
            /// Test that if null is passed in all parameters, a NullReference exception is thrown on the 'propertyName' parameter. The previous and new value for a string field can be null.
            /// </summary>
            [Test]
            public void Null_Null_Null()
            {
                ObservableObject testObject = new TestObservableObject();
                int changingCalled = 0;
                int changedCalled = 0;
                testObject.PropertyChanging += (sender, e) => changingCalled++;
                testObject.PropertyChanged += (sender, e) => changedCalled++;
                object[] arguments = new object[] { null, null, null };

                try
                {
                    this.method.Invoke(testObject, arguments);
                    Assert.Fail("No exception thrown.");
                }
                catch (TargetInvocationException tie) when (tie.InnerException is ArgumentNullException e)
                {
                    Assert.AreEqual("propertyName", e.ParamName);
                }

                Assert.AreEqual(0, changingCalled);
                Assert.AreEqual(0, changedCalled);
                Assert.IsNull(arguments[0]);
            }

            /// <summary>
            /// Test that if null is passed for the current and new value with a valid parameter name, the method passes without incident, returns false, and no events are raised.
            /// </summary>
            [Test]
            public void Null_Null_ValidPropertyName()
            {
                ObservableObject testObject = new TestObservableObject();
                int changingCalled = 0;
                int changedCalled = 0;
                testObject.PropertyChanging += (sender, e) => changingCalled++;
                testObject.PropertyChanged += (sender, e) => changedCalled++;
                object[] arguments = new object[] { null, null, nameof(TestObservableObject.Property) };

                // Set property with value of 'null' to 'null'- should have no effect.
                bool retVal = (bool)this.method.Invoke(testObject, arguments);

                Assert.AreEqual(false, retVal);
                Assert.AreEqual(0, changingCalled);
                Assert.AreEqual(0, changedCalled);
                Assert.IsNull(arguments[0]);
            }

            /// <summary>
            /// Tests that a string field can be changed from null.
            /// </summary>
            [Test]
            public void Null_Value_ValidPropertyName()
            {
                ObservableObject testObject = new TestObservableObject();
                int changingCalled = 0;
                string changingProperty = null;
                int changedCalled = 0;
                string changedProperty = null;
                testObject.PropertyChanging += (sender, e) => { changingCalled++; changingProperty = e.PropertyName; };
                testObject.PropertyChanged += (sender, e) => { changedCalled++; changedProperty = e.PropertyName; };
                object[] arguments = new object[] { null, "NewValue", nameof(TestObservableObject.Property) };

                bool retVal = (bool)this.method.Invoke(testObject, arguments);

                Assert.AreEqual(true, retVal);
                Assert.AreEqual(1, changingCalled);
                Assert.AreEqual(nameof(TestObservableObject.Property), changingProperty);
                Assert.AreEqual(1, changedCalled);
                Assert.AreEqual(nameof(TestObservableObject.Property), changedProperty);
                Assert.AreEqual("NewValue", arguments[0]);
            }

            /// <summary>
            /// Tests that a string field can be changed to null.
            /// </summary>
            [Test]
            public void Value_Null_ValidPropertyName()
            {
                ObservableObject testObject = new TestObservableObject();
                int changingCalled = 0;
                string changingProperty = null;
                int changedCalled = 0;
                string changedProperty = null;
                testObject.PropertyChanging += (sender, e) => { changingCalled++; changingProperty = e.PropertyName; };
                testObject.PropertyChanged += (sender, e) => { changedCalled++; changedProperty = e.PropertyName; };
                object[] arguments = new object[] { "OldValue", null, nameof(TestObservableObject.Property) };

                bool retVal = (bool)this.method.Invoke(testObject, arguments);

                Assert.AreEqual(true, retVal);
                Assert.AreEqual(1, changingCalled);
                Assert.AreEqual(nameof(TestObservableObject.Property), changingProperty);
                Assert.AreEqual(1, changedCalled);
                Assert.AreEqual(nameof(TestObservableObject.Property), changedProperty);
                Assert.AreEqual(null, arguments[0]);
            }

            /// <summary>
            /// Tests that a string field being asked to change to the same value will have no effect.
            /// </summary>
            [Test]
            public void Value_Value_ValidPropertyName()
            {
                ObservableObject testObject = new TestObservableObject();
                int changingCalled = 0;
                int changedCalled = 0;
                testObject.PropertyChanging += (sender, e) => changingCalled++;
                testObject.PropertyChanged += (sender, e) => changedCalled++;
                object[] arguments = new object[] { "OldValue", "OldValue", nameof(TestObservableObject.Property) };

                bool retVal = (bool)this.method.Invoke(testObject, arguments);

                Assert.AreEqual(false, retVal);
                Assert.AreEqual(0, changingCalled);
                Assert.AreEqual(0, changedCalled);
                Assert.AreEqual("OldValue", arguments[0]);
            }

            /// <summary>
            /// Tests that a string field can be changed from one value to another.
            /// </summary>
            [Test]
            public void Value_Value2_ValidPropertyName()
            {
                ObservableObject testObject = new TestObservableObject();
                int changingCalled = 0;
                string changingProperty = null;
                int changedCalled = 0;
                string changedProperty = null;
                testObject.PropertyChanging += (sender, e) => { changingCalled++; changingProperty = e.PropertyName; };
                testObject.PropertyChanged += (sender, e) => { changedCalled++; changedProperty = e.PropertyName; };
                object[] arguments = new object[] { "OldValue", "NewValue", nameof(TestObservableObject.Property) };

                bool retVal = (bool)this.method.Invoke(testObject, arguments);

                Assert.AreEqual(true, retVal);
                Assert.AreEqual(1, changingCalled);
                Assert.AreEqual(nameof(TestObservableObject.Property), changingProperty);
                Assert.AreEqual(1, changedCalled);
                Assert.AreEqual(nameof(TestObservableObject.Property), changedProperty);
                Assert.AreEqual("NewValue", arguments[0]);
            }

            /// <summary>
            /// Tests that attempting to change a property value with an invalid property name will throw an exception, and have no effect.
            /// </summary>
            [Test]
            public void Value_Value2_InvalidPropertyName()
            {
                ObservableObject testObject = new TestObservableObject();
                int changingCalled = 0;
                int changedCalled = 0;
                testObject.PropertyChanging += (sender, e) => changingCalled++;
                testObject.PropertyChanged += (sender, e) => changedCalled++;
                object[] arguments = new object[] { "OldValue", "NewValue", "NotAProperty" };

                try
                {
                    this.method.Invoke(testObject, arguments);
                    Assert.Fail("No exception thrown.");
                }
                catch (TargetInvocationException tie) when (tie.InnerException is ArgumentException e)
                {
                    Assert.AreEqual("propertyName", e.ParamName);
                }

                Assert.AreEqual(0, changingCalled);
                Assert.AreEqual(0, changedCalled);
                Assert.AreEqual("OldValue", arguments[0]);
            }

            /// <summary>
            /// Tests that an indexed string field can be changed from one value to another.
            /// </summary>
            [Test]
            public void Value_Value2_ValidIndexerName()
            {
                ObservableObject testObject = new TestObservableObject();
                int changingCalled = 0;
                string changingProperty = null;
                int changedCalled = 0;
                string changedProperty = null;
                testObject.PropertyChanging += (sender, e) => { changingCalled++; changingProperty = e.PropertyName; };
                testObject.PropertyChanged += (sender, e) => { changedCalled++; changedProperty = e.PropertyName; };
                object[] arguments = new object[] { "OldValue", "NewValue", "Item[]" };

                bool retVal = (bool)this.method.Invoke(testObject, arguments);

                Assert.AreEqual(true, retVal);
                Assert.AreEqual(1, changingCalled);
                Assert.AreEqual("Item[]", changingProperty);
                Assert.AreEqual(1, changedCalled);
                Assert.AreEqual("Item[]", changedProperty);
                Assert.AreEqual("NewValue", arguments[0]);
            }

            /// <summary>
            /// Tests that attempting to change a property value with an indexed name will throw an exception, and have no effect.
            /// </summary>
            [Test]
            public void Value_Value2_ValidPropertyNameWithIndexerBrackets()
            {
                ObservableObject testObject = new TestObservableObject();
                int changingCalled = 0;
                int changedCalled = 0;
                testObject.PropertyChanging += (sender, e) => changingCalled++;
                testObject.PropertyChanged += (sender, e) => changedCalled++;
                object[] arguments = new object[] { "OldValue", "NewValue", "Property[]" };

                try
                {
                    this.method.Invoke(testObject, arguments);
                    Assert.Fail("No exception thrown.");
                }
                catch (TargetInvocationException tie) when (tie.InnerException is ArgumentException e)
                {
                    Assert.AreEqual("propertyName", e.ParamName);
                }

                Assert.AreEqual(0, changingCalled);
                Assert.AreEqual(0, changedCalled);
                Assert.AreEqual("OldValue", arguments[0]);
            }

            /// <summary>
            /// Tests that attempting to change a property value with an indexed name will throw an exception, and have no effect.
            /// </summary>
            [Test]
            public void Value_Value2_IndexerNameWithoutBrackets()
            {
                ObservableObject testObject = new TestObservableObject();
                int changingCalled = 0;
                int changedCalled = 0;
                testObject.PropertyChanging += (sender, e) => changingCalled++;
                testObject.PropertyChanged += (sender, e) => changedCalled++;
                object[] arguments = new object[] { "OldValue", "NewValue", "Item" };

                try
                {
                    this.method.Invoke(testObject, arguments);
                    Assert.Fail("No exception thrown.");
                }
                catch (TargetInvocationException tie) when (tie.InnerException is ArgumentException e)
                {
                    Assert.AreEqual("propertyName", e.ParamName);
                }

                Assert.AreEqual(0, changingCalled);
                Assert.AreEqual(0, changedCalled);
                Assert.AreEqual("OldValue", arguments[0]);
            }
        }

        [TestFixture]
        public class Set_Field_Value_OnChange_PropertyName
        {
            private MethodInfo method;

            /// <summary>
            /// Finds the method under test.
            /// </summary>
            [OneTimeSetUp]
            public void FindMethod()
            {
                this.method = typeof(ObservableObject).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                    .FirstOrDefault(
                        method => method.GetParameters() is ParameterInfo[] paramInfo
                        && paramInfo.Length == 4
                        && Type.Equals(typeof(string).MakeByRefType(), paramInfo[0].ParameterType)
                        && Type.Equals(typeof(string), paramInfo[1].ParameterType)
                        && Type.Equals(typeof(Action), paramInfo[2].ParameterType)
                        && Type.Equals(typeof(string), paramInfo[3].ParameterType));
                Assert.IsNotNull(this.method);
            }

            // TODO: Add tests.
        }

        [TestFixture]
        public class Set_Field_Value_OnChangeWithPrevious_PropertyName
        {
            private MethodInfo method;

            /// <summary>
            /// Finds the method under test.
            /// </summary>
            [OneTimeSetUp]
            public void FindMethod()
            {
                this.method = typeof(ObservableObject).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                    .FirstOrDefault(
                        method => method.GetParameters() is ParameterInfo[] paramInfo
                        && paramInfo.Length == 4
                        && Type.Equals(typeof(string).MakeByRefType(), paramInfo[0].ParameterType)
                        && Type.Equals(typeof(string), paramInfo[1].ParameterType)
                        && Type.Equals(typeof(Action<string>), paramInfo[2].ParameterType)
                        && Type.Equals(typeof(string), paramInfo[3].ParameterType));
                Assert.IsNotNull(this.method);
            }

            // TODO: Add tests.
        }

        [TestFixture]
        public class Set_Field_Value_Validate_PropertyName
        {
            private MethodInfo method;

            /// <summary>
            /// Finds the method under test.
            /// </summary>
            [OneTimeSetUp]
            public void FindMethod()
            {
                this.method = typeof(ObservableObject).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                    .FirstOrDefault(
                        method => method.GetParameters() is ParameterInfo[] paramInfo
                        && paramInfo.Length == 4
                        && Type.Equals(typeof(string).MakeByRefType(), paramInfo[0].ParameterType)
                        && Type.Equals(typeof(string), paramInfo[1].ParameterType)
                        && Type.Equals(typeof(Func<string, string>), paramInfo[2].ParameterType)
                        && Type.Equals(typeof(string), paramInfo[3].ParameterType));
                Assert.IsNotNull(this.method);
            }

            // TODO: Add tests.
        }

        [TestFixture]
        public class Set_Field_Value_OnChange_Validate_PropertyName
        {
            private MethodInfo method;

            /// <summary>
            /// Finds the method under test.
            /// </summary>
            [OneTimeSetUp]
            public void FindMethod()
            {
                this.method = typeof(ObservableObject).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                    .FirstOrDefault(
                        method => method.GetParameters() is ParameterInfo[] paramInfo
                        && paramInfo.Length == 5
                        && Type.Equals(typeof(string).MakeByRefType(), paramInfo[0].ParameterType)
                        && Type.Equals(typeof(string), paramInfo[1].ParameterType)
                        && Type.Equals(typeof(Action), paramInfo[2].ParameterType)
                        && Type.Equals(typeof(Func<string, string>), paramInfo[3].ParameterType)
                        && Type.Equals(typeof(string), paramInfo[4].ParameterType));
                Assert.IsNotNull(this.method);
            }

            // TODO: Add tests.
        }

        [TestFixture]
        public class Set_Field_Value_OnChangeWithPrevious_Validate_PropertyName
        {
            private MethodInfo method;

            /// <summary>
            /// Finds the method under test.
            /// </summary>
            [OneTimeSetUp]
            public void FindMethod()
            {
                this.method = typeof(ObservableObject).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                    .FirstOrDefault(
                        method => method.GetParameters() is ParameterInfo[] paramInfo
                        && paramInfo.Length == 5
                        && Type.Equals(typeof(string).MakeByRefType(), paramInfo[0].ParameterType)
                        && Type.Equals(typeof(string), paramInfo[1].ParameterType)
                        && Type.Equals(typeof(Action<string>), paramInfo[2].ParameterType)
                        && Type.Equals(typeof(Func<string, string>), paramInfo[3].ParameterType)
                        && Type.Equals(typeof(string), paramInfo[4].ParameterType));
                Assert.IsNotNull(this.method);
            }

            // TODO: Add tests.
        }

        /// <summary>
        /// Tests for <see cref="ObservableObject.Set{TProp}(ref TProp, TProp, string)"/>.
        /// </summary>
        [TestFixture]
        public class SetGeneric_Field_Value_PropertyName
        {
            private MethodInfo method;

            /// <summary>
            /// Finds the method under test.
            /// </summary>
            [OneTimeSetUp]
            public void FindMethod()
            {
                this.method = typeof(ObservableObject).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                    .FirstOrDefault(
                        method => method.IsGenericMethod
                        && method.GetGenericArguments().Length == 1
                        && method.MakeGenericMethod(typeof(object)).GetParameters() is ParameterInfo[] paramInfo
                        && paramInfo.Length == 3
                        && Type.Equals(typeof(object).MakeByRefType(), paramInfo[0].ParameterType)
                        && Type.Equals(typeof(object), paramInfo[1].ParameterType)
                        && Type.Equals(typeof(string), paramInfo[2].ParameterType)).MakeGenericMethod(typeof(object));
                Assert.IsNotNull(this.method);
            }


            /// <summary>
            /// Test that if null is passed in all parameters, a NullReference exception is thrown on the 'propertyName' parameter. The previous and new value for a string field can be null.
            /// </summary>
            [Test]
            public void Null_Null_Null()
            {
                ObservableObject testObject = new TestObservableObject();
                int changingCalled = 0;
                int changedCalled = 0;
                testObject.PropertyChanging += (sender, e) => changingCalled++;
                testObject.PropertyChanged += (sender, e) => changedCalled++;
                object[] arguments = new object[] { null, null, null };

                try
                {
                    this.method.Invoke(testObject, arguments);
                    Assert.Fail("No exception thrown.");
                }
                catch (TargetInvocationException tie) when (tie.InnerException is ArgumentNullException e)
                {
                    Assert.AreEqual("propertyName", e.ParamName);
                }

                Assert.AreEqual(0, changingCalled);
                Assert.AreEqual(0, changedCalled);
                Assert.IsNull(arguments[0]);
            }

            /// <summary>
            /// Test that if null is passed for the current and new value with a valid parameter name, the method passes without incident, returns false, and no events are raised.
            /// </summary>
            [Test]
            public void Null_Null_ValidPropertyName()
            {
                ObservableObject testObject = new TestObservableObject();
                int changingCalled = 0;
                int changedCalled = 0;
                testObject.PropertyChanging += (sender, e) => changingCalled++;
                testObject.PropertyChanged += (sender, e) => changedCalled++;
                object[] arguments = new object[] { null, null, nameof(TestObservableObject.Property) };

                // Set property with value of 'null' to 'null'- should have no effect.
                bool retVal = (bool)this.method.Invoke(testObject, arguments);

                Assert.AreEqual(false, retVal);
                Assert.AreEqual(0, changingCalled);
                Assert.AreEqual(0, changedCalled);
                Assert.IsNull(arguments[0]);
            }

            /// <summary>
            /// Tests that a string field can be changed from null.
            /// </summary>
            [Test]
            public void Null_Value_ValidPropertyName()
            {
                ObservableObject testObject = new TestObservableObject();
                int changingCalled = 0;
                string changingProperty = null;
                int changedCalled = 0;
                string changedProperty = null;
                object newObject = new object();
                testObject.PropertyChanging += (sender, e) => { changingCalled++; changingProperty = e.PropertyName; };
                testObject.PropertyChanged += (sender, e) => { changedCalled++; changedProperty = e.PropertyName; };
                object[] arguments = new object[] { null, newObject, nameof(TestObservableObject.Property) };

                bool retVal = (bool)this.method.Invoke(testObject, arguments);

                Assert.AreEqual(true, retVal);
                Assert.AreEqual(1, changingCalled);
                Assert.AreEqual(nameof(TestObservableObject.Property), changingProperty);
                Assert.AreEqual(1, changedCalled);
                Assert.AreEqual(nameof(TestObservableObject.Property), changedProperty);
                Assert.AreSame(newObject, arguments[0]);
            }

            /// <summary>
            /// Tests that a string field can be changed to null.
            /// </summary>
            [Test]
            public void Value_Null_ValidPropertyName()
            {
                ObservableObject testObject = new TestObservableObject();
                int changingCalled = 0;
                string changingProperty = null;
                int changedCalled = 0;
                string changedProperty = null;
                object oldObject = new object();
                testObject.PropertyChanging += (sender, e) => { changingCalled++; changingProperty = e.PropertyName; };
                testObject.PropertyChanged += (sender, e) => { changedCalled++; changedProperty = e.PropertyName; };
                object[] arguments = new object[] { oldObject, null, nameof(TestObservableObject.Property) };

                bool retVal = (bool)this.method.Invoke(testObject, arguments);

                Assert.AreEqual(true, retVal);
                Assert.AreEqual(1, changingCalled);
                Assert.AreEqual(nameof(TestObservableObject.Property), changingProperty);
                Assert.AreEqual(1, changedCalled);
                Assert.AreEqual(nameof(TestObservableObject.Property), changedProperty);
                Assert.AreEqual(null, arguments[0]);
            }

            /// <summary>
            /// Tests that a string field being asked to change to the same value will have no effect.
            /// </summary>
            [Test]
            public void Value_Value_ValidPropertyName()
            {
                ObservableObject testObject = new TestObservableObject();
                int changingCalled = 0;
                int changedCalled = 0;
                object oldObject = new object();
                testObject.PropertyChanging += (sender, e) => changingCalled++;
                testObject.PropertyChanged += (sender, e) => changedCalled++;
                object[] arguments = new object[] { oldObject, oldObject, nameof(TestObservableObject.Property) };

                bool retVal = (bool)this.method.Invoke(testObject, arguments);

                Assert.AreEqual(false, retVal);
                Assert.AreEqual(0, changingCalled);
                Assert.AreEqual(0, changedCalled);
                Assert.AreSame(oldObject, arguments[0]);
            }

            /// <summary>
            /// Tests that a string field can be changed from one value to another.
            /// </summary>
            [Test]
            public void Value_Value2_ValidPropertyName()
            {
                ObservableObject testObject = new TestObservableObject();
                int changingCalled = 0;
                string changingProperty = null;
                int changedCalled = 0;
                string changedProperty = null;
                object oldObject = new object();
                object newObject = new object();
                testObject.PropertyChanging += (sender, e) => { changingCalled++; changingProperty = e.PropertyName; };
                testObject.PropertyChanged += (sender, e) => { changedCalled++; changedProperty = e.PropertyName; };
                object[] arguments = new object[] { oldObject, newObject, nameof(TestObservableObject.Property) };

                bool retVal = (bool)this.method.Invoke(testObject, arguments);

                Assert.AreEqual(true, retVal);
                Assert.AreEqual(1, changingCalled);
                Assert.AreEqual(nameof(TestObservableObject.Property), changingProperty);
                Assert.AreEqual(1, changedCalled);
                Assert.AreEqual(nameof(TestObservableObject.Property), changedProperty);
                Assert.AreSame(newObject, arguments[0]);
            }

            /// <summary>
            /// Tests that attempting to change a property value with an invalid property name will throw an exception, and have no effect.
            /// </summary>
            [Test]
            public void Value_Value2_InvalidPropertyName()
            {
                ObservableObject testObject = new TestObservableObject();
                int changingCalled = 0;
                int changedCalled = 0;
                object oldObject = new object();
                object newObject = new object();
                testObject.PropertyChanging += (sender, e) => changingCalled++;
                testObject.PropertyChanged += (sender, e) => changedCalled++;
                object[] arguments = new object[] { oldObject, newObject, "NotAProperty" };

                try
                {
                    this.method.Invoke(testObject, arguments);
                    Assert.Fail("No exception thrown.");
                }
                catch (TargetInvocationException tie) when (tie.InnerException is ArgumentException e)
                {
                    Assert.AreEqual("propertyName", e.ParamName);
                }

                Assert.AreEqual(0, changingCalled);
                Assert.AreEqual(0, changedCalled);
                Assert.AreSame(oldObject, arguments[0]);
            }

            /// <summary>
            /// Tests that an indexed string field can be changed from one value to another.
            /// </summary>
            [Test]
            public void Value_Value2_ValidIndexerName()
            {
                ObservableObject testObject = new TestObservableObject();
                int changingCalled = 0;
                string changingProperty = null;
                int changedCalled = 0;
                string changedProperty = null;
                object oldObject = new object();
                object newObject = new object();
                testObject.PropertyChanging += (sender, e) => { changingCalled++; changingProperty = e.PropertyName; };
                testObject.PropertyChanged += (sender, e) => { changedCalled++; changedProperty = e.PropertyName; };
                object[] arguments = new object[] { oldObject, newObject, "Item[]" };

                bool retVal = (bool)this.method.Invoke(testObject, arguments);

                Assert.AreEqual(true, retVal);
                Assert.AreEqual(1, changingCalled);
                Assert.AreEqual("Item[]", changingProperty);
                Assert.AreEqual(1, changedCalled);
                Assert.AreEqual("Item[]", changedProperty);
                Assert.AreSame(newObject, arguments[0]);
            }

            /// <summary>
            /// Tests that attempting to change a property value with an indexed name will throw an exception, and have no effect.
            /// </summary>
            [Test]
            public void Value_Value2_ValidPropertyNameWithIndexerBrackets()
            {
                ObservableObject testObject = new TestObservableObject();
                int changingCalled = 0;
                int changedCalled = 0;
                object oldObject = new object();
                object newObject = new object();
                testObject.PropertyChanging += (sender, e) => changingCalled++;
                testObject.PropertyChanged += (sender, e) => changedCalled++;
                object[] arguments = new object[] { oldObject, newObject, "Property[]" };

                try
                {
                    this.method.Invoke(testObject, arguments);
                    Assert.Fail("No exception thrown.");
                }
                catch (TargetInvocationException tie) when (tie.InnerException is ArgumentException e)
                {
                    Assert.AreEqual("propertyName", e.ParamName);
                }

                Assert.AreEqual(0, changingCalled);
                Assert.AreEqual(0, changedCalled);
                Assert.AreSame(oldObject, arguments[0]);
            }

            /// <summary>
            /// Tests that attempting to change a property value with an indexed name will throw an exception, and have no effect.
            /// </summary>
            [Test]
            public void Value_Value2_IndexerNameWithoutBrackets()
            {
                ObservableObject testObject = new TestObservableObject();
                int changingCalled = 0;
                int changedCalled = 0;
                object oldObject = new object();
                object newObject = new object();
                testObject.PropertyChanging += (sender, e) => changingCalled++;
                testObject.PropertyChanged += (sender, e) => changedCalled++;
                object[] arguments = new object[] { oldObject, newObject, "Item" };

                try
                {
                    this.method.Invoke(testObject, arguments);
                    Assert.Fail("No exception thrown.");
                }
                catch (TargetInvocationException tie) when (tie.InnerException is ArgumentException e)
                {
                    Assert.AreEqual("propertyName", e.ParamName);
                }

                Assert.AreEqual(0, changingCalled);
                Assert.AreEqual(0, changedCalled);
                Assert.AreSame(oldObject, arguments[0]);
            }
        }

        public class SetGeneric_Field_Value_OnChange_PropertyName { }

        public class SetGeneric_Field_Value_OnChangeWithPrevious_PropertyName { }

        public class SetGeneric_Field_Value_Validate_PropertyName { }

        public class SetGeneric_Field_Value_OnChange_Validate_PropertyName { }

        public class SetGeneric_Field_Value_OnChangeWithPrevious_Validate_PropertyName { }

        public class SetObservable_Field_Value_ChangeHandler_PropertyName { }

        public class SetObservable_Field_Value_OnChange_ChangeHandler_PropertyName { }

        public class SetObservable_Field_Value_OnChangeWithPrevious_ChangeHandler_PropertyName { }

        public class SetObservable_Field_Value_Validate_ChangeHandler_PropertyName { }

        public class SetObservable_Field_Value_OnChange_Validate_ChangeHandler_PropertyName { }

        public class SetObservable_Field_Value_OnChangeWithPrevious_Validate_ChangeHandler_PropertyName { }

        public class SetDisposable_Field_Value_DisposePrevious_PropertyName { }

        public class SetDisposable_Field_Value_DisposePrevious_OnChange_PropertyName { }

        public class SetDisposable_Field_Value_DisposePrevious_OnChangeWithPrevious_PropertyName { }

        public class SetDisposable_Field_Value_DisposePrevious_Validate_PropertyName { }

        public class SetDisposable_Field_Value_DisposePrevious_OnChange_Validate_PropertyName { }

        public class SetDisposable_Field_Value_DisposePrevious_OnChangeWithPrevious_Validate_PropertyName { }

        public class SetObservableDisposable_Field_Value_DisposePrevious_ChangeHandler_PropertyName { }

        public class SetObservableDisposable_Field_Value_DisposePrevious_OnChange_ChangeHandler_PropertyName { }

        public class SetObservableDisposable_Field_Value_DisposePrevious_OnChangeWithPrevious_ChangeHandler_PropertyName { }

        public class SetObservableDisposable_Field_Value_DisposePrevious_Validate_ChangeHandler_PropertyName { }

        public class SetObservableDisposable_Field_Value_DisposePrevious_OnChange_Validate_ChangeHandler_PropertyName { }

        public class SetObservableDisposable_Field_Value_DisposePrevious_OnChangeWithPrevious_Validate_ChangeHandler_PropertyName { }

        public class SetComparable_Field_Value_Min_Max_PropertyName { }

        public class SetComparable_Field_Value_Min_Max_OnChange_PropertyName { }

        public class SetComparable_Field_Value_Min_Max_OnChangeWithPrevious_PropertyName { }
    }
}
