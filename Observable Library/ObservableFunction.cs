// <copyright file="ObservableFunction.cs" company="Flynn1179">
// Copyright (c) Flynn1179. All rights reserved.
// </copyright>

namespace Flynn1179.Observable
{
    using System;

    /// <summary>
    /// Represents a function with an observable result, that updates whenever a change to the input causes a change.
    /// </summary>
    /// <typeparam name="T">The type of the function input.</typeparam>
    /// <typeparam name="TResult">The result type of the function.</typeparam>
    public class ObservableFunction<T, TResult> : ObservableObject
    {
        private readonly Func<T, TResult> func;

        private TResult result;

        private T input;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableFunction{T, TResult}"/> class.
        /// </summary>
        /// <param name="func">The function to be applied to the input.</param>
        public ObservableFunction(Func<T, TResult> func)
        {
            func.ThrowIfNull(nameof(func));
            this.func = func;
            this.result = this.func(default);
        }

        /// <summary>
        /// Gets or sets the input for the function.
        /// </summary>
        public T Input
        {
            get => this.input;
            set => this.Set(ref this.input, value, value => this.Result = this.func(value));
        }

        /// <summary>
        /// Gets the result of the function.
        /// </summary>
        public TResult Result
        {
            get => this.result;
            private set => this.Set(ref this.result, value);
        }
    }
}
