// <copyright file="TestObservableObject.cs" company="Flynn1179">
//   Copyright © 2020 Flynn1179
// </copyright>

namespace Flynn1179.Observable.Tests.TestClasses
{
    public class TestObservableObject<T1,T2> : ObservableObject
    {
        private T1 field1;
        private T2 field2;

        public T1 Property1
        {
            get => this.field1;
            set => this.Set(ref this.field1, value);
        }

        public T2 Property2
        {
            get => this.field2;
            set => this.Set(ref this.field2, value);
        }
    }
}
