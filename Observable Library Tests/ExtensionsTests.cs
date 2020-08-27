// <copyright file="ExtensionsTests.cs" company="Flynn1179">
//   Copyright © 2020 Flynn1179
// </copyright>

namespace Flynn1179.Observable.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using NUnit.Framework;

    /// <summary>
    /// Test fixture for methods in the <see cref="Extensions"/> class.
    /// </summary>
    [TestFixture]
    public class ExtensionsTests
    {
        /// <summary>
        /// Test that SafeRaise throws an <see cref="ArgumentNullException"/> when called with a null sender, and null property, naming the propertyName property.
        /// </summary>
        [Test]
        public void SafeRaiseNullNullNull()
        {
            ArgumentNullException thrown = Assert.Throws<ArgumentNullException>(() => ((PropertyChangedEventHandler)null).SafeRaise(null, null));
            Assert.That(thrown.ParamName, Is.EqualTo("propertyName"));
        }

        /// <summary>
        /// Test that SafeRaise throws an <see cref="ArgumentException"/> when called with a null sender, and empty property, naming the propertyName property.
        /// </summary>
        [Test]
        public void SafeRaiseNullNullEmpty()
        {
            ArgumentException thrown = Assert.Throws<ArgumentException>(() => ((PropertyChangedEventHandler)null).SafeRaise(null, string.Empty));
            Assert.That(thrown.ParamName, Is.EqualTo("propertyName"));
        }

        /// <summary>
        /// Test that SafeRaise does nothing if a null handler or sender is passed, but a valid property name.
        /// </summary>
        [Test]
        public void SafeRaiseNullNullValid()
        {
            Assert.DoesNotThrow(() => ((PropertyChangedEventHandler)null).SafeRaise(null, "TestProperty"));
        }

        /// <summary>
        /// Test that SafeRaise throws an <see cref="ArgumentNullException"/> when called with a valid sender, and null property, naming the propertyName property.
        /// </summary>
        [Test]
        public void SafeRaiseNullSenderNull()
        {
            INotifyPropertyChanged testSender = NSubstitute.Substitute.For<INotifyPropertyChanged>();

            ArgumentNullException thrown = Assert.Throws<ArgumentNullException>(() => ((PropertyChangedEventHandler)null).SafeRaise(testSender, null));
            Assert.That(thrown.ParamName, Is.EqualTo("propertyName"));
        }

        /// <summary>
        /// Test that SafeRaise throws an <see cref="ArgumentException"/> when called with a valid sender, and empty property, naming the propertyName property.
        /// </summary>
        [Test]
        public void SafeRaiseNullSenderEmpty()
        {
            INotifyPropertyChanged testSender = NSubstitute.Substitute.For<INotifyPropertyChanged>();

            ArgumentException thrown = Assert.Throws<ArgumentException>(() => ((PropertyChangedEventHandler)null).SafeRaise(testSender, string.Empty));
            Assert.That(thrown.ParamName, Is.EqualTo("propertyName"));
        }

        /// <summary>
        /// Tests that SafeRaise throws an <see cref="ArgumentException"/> when called with a valid sender, but a property name that does not exist on the sender.
        /// </summary>
        [Test]
        public void SafeRaiseNullSenderInvalid()
        {
            INotifyPropertyChanged testSender = NSubstitute.Substitute.For<TestClasses.ITestNotifyPropertyChanged<object>>();

            // Even though there's no handler, that might not be known at runtime, so the SafeRaise should still throw if the property name's invalid.
            ArgumentException thrown = Assert.Throws<ArgumentException>(() => ((PropertyChangedEventHandler)null).SafeRaise(testSender, "InvalidProperty"));
            Assert.That(thrown.ParamName, Is.EqualTo("propertyName"));
        }

        /// <summary>
        /// Tests that SafeRaise does not throw an exception when called with a valid sender, and a property name that does exist on the sender, even if the handler is null.
        /// </summary>
        [Test]
        public void SafeRaiseNullSenderValid()
        {
            INotifyPropertyChanged testSender = NSubstitute.Substitute.For<TestClasses.ITestNotifyPropertyChanged<object>>();

            // Even though there's no handler, that might not be known at runtime, so the SafeRaise should still throw if the property name's invalid.
            Assert.DoesNotThrow(() => ((PropertyChangedEventHandler)null).SafeRaise(testSender, nameof(TestClasses.ITestNotifyPropertyChanged<object>.Property)));
        }

        [Test]
        public void TestHasHandler_NullSender_NullName()
        {
            List<(object, PropertyChangedEventArgs)> responses = new List<(object, PropertyChangedEventArgs)>();
            PropertyChangedEventHandler handler = (sender, e) => responses.Add((sender, e));

            Assert.Throws<ArgumentNullException>(() => handler.SafeRaise(null, null));

            Assert.That(responses.Count, Is.EqualTo(0));
        }

        [Test]
        public void TestHasHandler_NullSender_EmptyName()
        {
            List<(object, PropertyChangedEventArgs)> responses = new List<(object, PropertyChangedEventArgs)>();
            PropertyChangedEventHandler handler = (sender, e) => responses.Add((sender, e));

            Assert.Throws<ArgumentException>(() => handler.SafeRaise(null, string.Empty));

            Assert.That(responses.Count, Is.EqualTo(0));
        }

        [Test]
        public void TestHasHandler_NullSender_HasName()
        {
            List<(object, PropertyChangedEventArgs)> responses = new List<(object, PropertyChangedEventArgs)>();
            PropertyChangedEventHandler handler = (sender, e) => responses.Add((sender, e));

            handler.SafeRaise(null, "TestProperty");

            Assert.That(responses.Count, Is.EqualTo(1));
            Assert.That(responses[0].Item1, Is.Null);
            Assert.That(responses[0].Item2, Is.Not.Null);
            Assert.That(responses[0].Item2.PropertyName, Is.EqualTo("TestProperty"));
        }

        [Test]
        public void TestHasHandler_HasSender_NullName()
        {
            INotifyPropertyChanged testSender = NSubstitute.Substitute.For<INotifyPropertyChanged>();
            List<(object, PropertyChangedEventArgs)> responses = new List<(object, PropertyChangedEventArgs)>();
            PropertyChangedEventHandler handler = (sender, e) => responses.Add((sender, e));

            Assert.Throws<ArgumentNullException>(() => handler.SafeRaise(testSender, null));

            Assert.That(responses.Count, Is.EqualTo(0));
        }

        [Test]
        public void TestHasHandler_HasSender_EmptyName()
        {
            INotifyPropertyChanged testSender = NSubstitute.Substitute.For<INotifyPropertyChanged>();
            List<(object, PropertyChangedEventArgs)> responses = new List<(object, PropertyChangedEventArgs)>();
            PropertyChangedEventHandler handler = (sender, e) => responses.Add((sender, e));

            Assert.Throws<ArgumentException>(() => handler.SafeRaise(testSender, string.Empty));

            Assert.That(responses.Count, Is.EqualTo(0));
        }

        [Test]
        public void TestHasHandler_HasSender_HasPropertyName()
        {
            ObservableCollection<object> testSender = new ObservableCollection<object>();
            List<(object, PropertyChangedEventArgs)> responses = new List<(object, PropertyChangedEventArgs)>();
            PropertyChangedEventHandler handler = (sender, e) => responses.Add((sender, e));

            handler.SafeRaise(testSender, "Count");

            Assert.That(responses.Count, Is.EqualTo(1));
            Assert.That(responses[0].Item1, Is.SameAs(testSender));
            Assert.That(responses[0].Item2, Is.Not.Null);
            Assert.That(responses[0].Item2.PropertyName, Is.EqualTo("Count"));
        }

        [Test]
        public void TestHasHandler_HasSender_HasInvalidPropertyName()
        {
            ObservableCollection<object> testSender = new ObservableCollection<object>();
            List<(object, PropertyChangedEventArgs)> responses = new List<(object, PropertyChangedEventArgs)>();
            PropertyChangedEventHandler handler = (sender, e) => responses.Add((sender, e));

            Assert.Throws<ArgumentException>(() => handler.SafeRaise(testSender, "Missing"));

            Assert.That(responses.Count, Is.EqualTo(0));
        }

        [Test]
        public void TestHasHandler_HasSender_HasBadIndexerName()
        {
            ObservableCollection<object> testSender = new ObservableCollection<object>();
            List<(object, PropertyChangedEventArgs)> responses = new List<(object, PropertyChangedEventArgs)>();
            PropertyChangedEventHandler handler = (sender, e) => responses.Add((sender, e));

            Assert.Throws<ArgumentException>(() => handler.SafeRaise(testSender, "Count[]"));

            Assert.That(responses.Count, Is.EqualTo(0));
        }

        [Test]
        public void TestHasHandler_HasSender_HasIndexerName()
        {
            ObservableCollection<object> testSender = new ObservableCollection<object>();
            List<(object, PropertyChangedEventArgs)> responses = new List<(object, PropertyChangedEventArgs)>();
            PropertyChangedEventHandler handler = (sender, e) => responses.Add((sender, e));

            handler.SafeRaise(testSender, "Item[]");

            Assert.That(responses.Count, Is.EqualTo(1));
            Assert.That(responses[0].Item1, Is.SameAs(testSender));
            Assert.That(responses[0].Item2, Is.Not.Null);
            Assert.That(responses[0].Item2.PropertyName, Is.EqualTo("Item[]"));
        }

        [Test]
        public void TestHasHandler_HasSender_MissingIndexerBrackets()
        {
            ObservableCollection<object> testSender = new ObservableCollection<object>();
            List<(object, PropertyChangedEventArgs)> responses = new List<(object, PropertyChangedEventArgs)>();
            PropertyChangedEventHandler handler = (sender, e) => responses.Add((sender, e));

            Assert.Throws<ArgumentException>(() => handler.SafeRaise(testSender, "Item"));
        }

        [Test]
        public void TestHandlerThrowsException_NullSender_HasProperty()
        {
            ObservableCollection<object> testSender = new ObservableCollection<object>();
            Exception ex = new Exception();
            List<(object, PropertyChangedEventArgs)> responses = new List<(object, PropertyChangedEventArgs)>();
            PropertyChangedEventHandler handler = (sender, e) => { responses.Add((sender, e)); throw ex; };

            AggregateException resultEx = Assert.Throws<AggregateException>(() => handler.SafeRaise(testSender, "Item[]"));

            Assert.That(responses.Count, Is.EqualTo(1));
            Assert.That(responses[0].Item1, Is.SameAs(testSender));
            Assert.That(responses[0].Item2, Is.Not.Null);
            Assert.That(responses[0].Item2.PropertyName, Is.EqualTo("Item[]"));
            Assert.That(resultEx.InnerExceptions.Count, Is.EqualTo(1));
            Assert.That(resultEx.InnerException, Is.SameAs(ex));
            Assert.That(resultEx.InnerExceptions[0], Is.SameAs(ex));
        }

        [Test]
        public void TestTwoHandlers_NullSender_HasProperty()
        {
            ObservableCollection<object> testSender = new ObservableCollection<object>();
            List<(object, PropertyChangedEventArgs)> responses = new List<(object, PropertyChangedEventArgs)>();
            PropertyChangedEventHandler handler1 = (sender, e) => responses.Add((sender, e));
            PropertyChangedEventHandler handler2 = (sender, e) => responses.Add((sender, e));
            PropertyChangedEventHandler handler = MulticastDelegate.Combine(handler1, handler2) as PropertyChangedEventHandler;

            handler.SafeRaise(testSender, "Count");

            Assert.That(responses.Count, Is.EqualTo(2));
            Assert.That(responses[0].Item1, Is.SameAs(testSender));
            Assert.That(responses[0].Item2, Is.Not.Null);
            Assert.That(responses[0].Item2.PropertyName, Is.EqualTo("Count"));
            Assert.That(responses[1].Item1, Is.SameAs(testSender));
            Assert.That(responses[1].Item2, Is.Not.Null);
            Assert.That(responses[1].Item2.PropertyName, Is.EqualTo("Count"));
        }

        [Test]
        public void TestOneOfTwoHandlersExcepts_NullSender_HasProperty()
        {
            ObservableCollection<object> testSender = new ObservableCollection<object>();
            Exception ex = new Exception();
            List<(object, PropertyChangedEventArgs)> responses = new List<(object, PropertyChangedEventArgs)>();
            PropertyChangedEventHandler handler1 = (sender, e) => { responses.Add((sender, e)); throw ex; };
            PropertyChangedEventHandler handler2 = (sender, e) => responses.Add((sender, e));
            PropertyChangedEventHandler handler = MulticastDelegate.Combine(handler1, handler2) as PropertyChangedEventHandler;

            AggregateException resultEx = Assert.Throws<AggregateException>(() => handler.SafeRaise(testSender, "Count"));

            Assert.That(responses.Count, Is.EqualTo(2));
            Assert.That(responses[0].Item1, Is.SameAs(testSender));
            Assert.That(responses[0].Item2, Is.Not.Null);
            Assert.That(responses[0].Item2.PropertyName, Is.EqualTo("Count"));
            Assert.That(responses[1].Item1, Is.SameAs(testSender));
            Assert.That(responses[1].Item2, Is.Not.Null);
            Assert.That(responses[1].Item2.PropertyName, Is.EqualTo("Count"));
            Assert.That(resultEx.InnerExceptions.Count, Is.EqualTo(1));
            Assert.That(resultEx.InnerException, Is.SameAs(ex));
            Assert.That(resultEx.InnerExceptions[0], Is.SameAs(ex));
        }

        [Test]
        public void TestTwoOfTwoHandlersExcepts_NullSender_HasProperty()
        {
            ObservableCollection<object> testSender = new ObservableCollection<object>();
            Exception ex = new Exception();
            List<(object, PropertyChangedEventArgs)> responses = new List<(object, PropertyChangedEventArgs)>();
            PropertyChangedEventHandler handler1 = (sender, e) => responses.Add((sender, e));
            PropertyChangedEventHandler handler2 = (sender, e) => { responses.Add((sender, e)); throw ex; };
            PropertyChangedEventHandler handler = MulticastDelegate.Combine(handler1, handler2) as PropertyChangedEventHandler;

            AggregateException resultEx = Assert.Throws<AggregateException>(() => handler.SafeRaise(testSender, "Count"));

            Assert.That(responses.Count, Is.EqualTo(2));
            Assert.That(responses[0].Item1, Is.SameAs(testSender));
            Assert.That(responses[0].Item2, Is.Not.Null);
            Assert.That(responses[0].Item2.PropertyName, Is.EqualTo("Count"));
            Assert.That(responses[1].Item1, Is.SameAs(testSender));
            Assert.That(responses[1].Item2, Is.Not.Null);
            Assert.That(responses[1].Item2.PropertyName, Is.EqualTo("Count"));
            Assert.That(resultEx.InnerExceptions.Count, Is.EqualTo(1));
            Assert.That(resultEx.InnerException, Is.SameAs(ex));
            Assert.That(resultEx.InnerExceptions[0], Is.SameAs(ex));
        }

        [Test]
        public void TestTwoHandlersThrowException_NullSender_HasProperty()
        {
            ObservableCollection<object> testSender = new ObservableCollection<object>();
            Exception ex1 = new Exception();
            Exception ex2 = new Exception();
            List<(object, PropertyChangedEventArgs)> responses = new List<(object, PropertyChangedEventArgs)>();
            PropertyChangedEventHandler handler1 = (sender, e) => { responses.Add((sender, e)); throw ex1; };
            PropertyChangedEventHandler handler2 = (sender, e) => { responses.Add((sender, e)); throw ex2; };
            PropertyChangedEventHandler handler = MulticastDelegate.Combine(handler1, handler2) as PropertyChangedEventHandler;

            AggregateException resultEx = Assert.Throws<AggregateException>(() => handler.SafeRaise(testSender, "Count"));

            Assert.That(responses.Count, Is.EqualTo(2));
            Assert.That(responses[0].Item1, Is.SameAs(testSender));
            Assert.That(responses[0].Item2, Is.Not.Null);
            Assert.That(responses[0].Item2.PropertyName, Is.EqualTo("Count"));
            Assert.That(responses[1].Item1, Is.SameAs(testSender));
            Assert.That(responses[1].Item2, Is.Not.Null);
            Assert.That(responses[1].Item2.PropertyName, Is.EqualTo("Count"));
            Assert.That(resultEx.InnerExceptions.Count, Is.EqualTo(2));
            Assert.That(resultEx.InnerException, Is.SameAs(ex1));
            Assert.That(resultEx.InnerExceptions[0], Is.SameAs(ex1));
            Assert.That(resultEx.InnerExceptions[1], Is.SameAs(ex2));
        }

        // public static void SafeRaise(this PropertyChangingEventHandler handler, object sender, string propertyName)


        // public static void SafeRaise(this EventHandler handler, object sender, EventArgs e)
        // public static void SafeRaise<T>(this EventHandler<T> handler, object sender, T e)
    }
}
