﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace MangaTL.Managers.Helpers
{
    public class DropoutStack<T> : IEnumerable<T>
    {
        /// <summary>
        ///     The stack collection.
        /// </summary>
        private readonly T[] items;

        /// <summary>
        ///     The top - where the next item will be pushed in the array.
        /// </summary>
        private int top; // The position in the array that the next item will be placed. 

        /// <summary>
        ///     Returns the amount of elements on the stack.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public int Count { get; private set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DropoutStack{T}" /> class.
        /// </summary>
        /// <param name="capacity">The capacity of the stack.</param>
        public DropoutStack(int capacity)
        {
            items = new T[capacity];
        }

        /// <summary>
        ///     Returns an enumerator for a generic stack that iterates through the stack.
        ///     The iterator start at the last item pushed onto the stack and goes backwards.
        /// </summary>
        /// <returns>
        ///     A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the
        ///     collection.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            for (var i = 0; i < Count; i++)
                yield return GetItem(i);
        }


        /// <exclude />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        ///     Pushes the specified item onto the stack.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Push(T item)
        {
            Count += 1;
            Count = Count > items.Length ? items.Length : Count;

            items[top] = item;
            top = (top + 1) %
                  items.Length; // After filling the array the next item will be placed at the beginning of the array!
        }

        /// <summary>
        ///     Pops last item from the stack.
        /// </summary>
        /// <returns>T.</returns>
        public T Pop()
        {
            Count -= 1;
            Count = Count < 0 ? 0 : Count;

            top = (items.Length + top - 1) % items.Length;
            return items[top];
        }

        /// <summary>
        ///     Peeks at last item on the stack.
        /// </summary>
        /// <returns>T.</returns>
        public T Peek()
        {
            return items[(items.Length + top - 1) % items.Length]; //Same as pop but without changing the value of top.
        }

        /// <summary>
        ///     Gets an item from the stack.
        ///     Index 0 is the last item pushed onto the stack.
        /// </summary>
        /// <param name="index">The index of the item.</param>
        /// <returns>T.</returns>
        /// <exception cref="System.InvalidOperationException">Index out of bounds</exception>
        public T GetItem(int index)
        {
            if (index > Count)
                throw new InvalidOperationException("Index out of bounds");

            // The first element = last element entered = index 0 is at Peek - see above.
            // index 0 = items[(items.Length + top - 1) % items.Length];
            // index 1 = items[(items.Length + top - 2) % items.Length];
            // index 2 = items[(items.Length + top - 3) % items.Length]; etc...
            // So to get an item at a certain index is:
            // items[(items.Length + top - (index+1)) % items.Length];

            return items[(items.Length + top - (index + 1)) % items.Length];
        }

        /// <summary>
        ///     Clears the stack.
        /// </summary>
        public void Clear()
        {
            Count = 0;
        }
    }
}