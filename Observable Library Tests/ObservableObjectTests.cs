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

        /* Methods still to test:

        String properties
        protected bool Set(ref string field, string value, [CallerMemberName] string propertyName = "")
        protected bool Set(ref string field, string value, Action onChange, [CallerMemberName] string propertyName = "")
        protected bool Set(ref string field, string value, Action<string> onChangeWithPrevious, [CallerMemberName] string propertyName = "")
        protected bool Set(ref string field, string value, Func<string, string> validate, [CallerMemberName] string propertyName = "")
        protected bool Set(ref string field, string value, Action onChange, Func<string, string> validate, [CallerMemberName] string propertyName = "")
        protected bool Set(ref string field, string value, Action<string> onChangeWithPrevious, Func<string, string> validate, [CallerMemberName] string propertyName = "")

        Any other property type.
        protected bool Set<TProp>(ref TProp field, TProp value, [CallerMemberName] string propertyName = "")
        protected bool Set<TProp>(ref TProp field, TProp value, Action onChange, [CallerMemberName] string propertyName = "")
        protected bool Set<TProp>(ref TProp field, TProp value, Action<TProp> onChangeWithPrevious, [CallerMemberName] string propertyName = "")
        protected bool Set<TProp>(ref TProp field, TProp value, Func<TProp, string> validate, [CallerMemberName] string propertyName = "")
        protected bool Set<TProp>(ref TProp field, TProp value, Action onChange, Func<TProp, string> validate, [CallerMemberName] string propertyName = "")
        protected bool Set<TProp>(ref TProp field, TProp value, Action<TProp> onChangeWithPrevious, Func<TProp, string> validate, [CallerMemberName] string propertyName = "")

        Observable property type with change handler
        protected bool Set<TProp>(ref TProp field, TProp value, PropertyChangedEventHandler changeHandler, [CallerMemberName] string propertyName = "")
        protected bool Set<TProp>(ref TProp field, TProp value, Action onChange, PropertyChangedEventHandler changeHandler, [CallerMemberName] string propertyName = "")
        protected bool Set<TProp>(ref TProp field, TProp value, Action<TProp> onChangeWithPrevious, PropertyChangedEventHandler changeHandler, [CallerMemberName] string propertyName = "")
        protected bool Set<TProp>(ref TProp field, TProp value, Func<TProp, string> validate, PropertyChangedEventHandler changeHandler, [CallerMemberName] string propertyName = "")
        protected bool Set<TProp>(ref TProp field, TProp value, Action onChange, Func<TProp, string> validate, PropertyChangedEventHandler changeHandler, [CallerMemberName] string propertyName = "")
        protected bool Set<TProp>(ref TProp field, TProp value, Action<TProp> onChangeWithPrevious, Func<TProp, string> validate, PropertyChangedEventHandler changeHandler, [CallerMemberName] string propertyName = "")

        Disposable property type with optional dispose previous
        protected bool Set<TProp>(ref TProp field, TProp value, bool disposePrevious, [CallerMemberName] string propertyName = "")
        protected bool Set<TProp>(ref TProp field, TProp value, bool disposePrevious, Action onChange, [CallerMemberName] string propertyName = "")
        protected bool Set<TProp>(ref TProp field, TProp value, bool disposePrevious, Action<TProp> onChangeWithPrevious, [CallerMemberName] string propertyName = "")
        protected bool Set<TProp>(ref TProp field, TProp value, bool disposePrevious, Func<TProp, string> validate, [CallerMemberName] string propertyName = "")
        protected bool Set<TProp>(ref TProp field, TProp value, bool disposePrevious, Action onChange, Func<TProp, string> validate, [CallerMemberName] string propertyName = "")
        protected bool Set<TProp>(ref TProp field, TProp value, bool disposePrevious, Action<TProp> onChangeWithPrevious, Func<TProp, string> validate, [CallerMemberName] string propertyName = "")

        Disposable & observable property with change handler and optional dispose previous
        protected bool Set<TProp>(ref TProp field, TProp value, bool disposePrevious, PropertyChangedEventHandler changeHandler, [CallerMemberName] string propertyName = "")
        protected bool Set<TProp>(ref TProp field, TProp value, bool disposePrevious, Action onChange, PropertyChangedEventHandler changeHandler, [CallerMemberName] string propertyName = "")
        protected bool Set<TProp>(ref TProp field, TProp value, bool disposePrevious, Action<TProp> onChangeWithPrevious, PropertyChangedEventHandler changeHandler, [CallerMemberName] string propertyName = "")
        protected bool Set<TProp>(ref TProp field, TProp value, bool disposePrevious, Func<TProp, string> validate, PropertyChangedEventHandler changeHandler, [CallerMemberName] string propertyName = "")
        protected bool Set<TProp>(ref TProp field, TProp value, bool disposePrevious, Action onChange, Func<TProp, string> validate, PropertyChangedEventHandler changeHandler = null, [CallerMemberName] string propertyName = "")
        protected bool Set<TProp>(ref TProp field, TProp value, bool disposePrevious, Action<TProp> onChangeWithPrevious, Func<TProp, string> validate, PropertyChangedEventHandler changeHandler = null, [CallerMemberName] string propertyName = "")

        Properties with range and defined min/max (inclusive) 
        protected bool Set<TProp>(ref TProp field, TProp value, TProp min, TProp max, [CallerMemberName] string propertyName = "")
        protected bool Set<TProp>(ref TProp field, TProp value, TProp min, TProp max, Action onChange = null, [CallerMemberName] string propertyName = "")
        protected bool Set<TProp>(ref TProp field, TProp value, TProp min, TProp max, Action<TProp> onChangeWithPrevious = null, [CallerMemberName] string propertyName = "")
        */
    }
}
