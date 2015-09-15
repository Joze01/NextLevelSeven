﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NextLevelSeven.Utility
{
    /// <summary>
    ///     An IEnumerable+IIndexable wrapper around a method set, using a numeric index.
    /// </summary>
    /// <typeparam name="TItem">Type of contained items.</typeparam>
    internal sealed class WrapperEnumerable<TItem> : IEnumerableIndexable<int, TItem>
    {
        private readonly Func<int> _count;
        private readonly Func<int, TItem> _read;
        private readonly int _startIndex;
        private readonly Action<int, TItem> _write;

        /// <summary>
        ///     Create a wrapper.
        /// </summary>
        /// <param name="read">Function to read values at a specified index.</param>
        /// <param name="write">Function to write values at a specified index.</param>
        /// <param name="count">Function to get the number of items contained.</param>
        /// <param name="startIndex">Index where items begin. Defaults to zero.</param>
        internal WrapperEnumerable(Func<int, TItem> read, Action<int, TItem> write, Func<int> count, int startIndex = 0)
        {
            _count = count;
            _read = read;
            _startIndex = startIndex;
            _write = write;
        }

        /// <summary>
        ///     Create a wrapper over an IList.
        /// </summary>
        /// <param name="other">Other IList.</param>
        /// <param name="startIndex">Index where items begin. Defaults to zero.</param>
        internal WrapperEnumerable(IList<TItem> other, int startIndex = 0)
        {
            _count = () => other.Count;
            _read = i => other[i];
            _startIndex = startIndex;
            _write = (i, v) => { other[i] = v; };
        }

        /// <summary>
        ///     Create a wrapper over another IEnumerable. This will not permit writes.
        /// </summary>
        /// <param name="other">Other IEnumerable.</param>
        /// <param name="startIndex">Index where items begin. Defaults to zero.</param>
        internal WrapperEnumerable(IEnumerable<TItem> other, int startIndex = 0)
        {
            // ReSharper disable PossibleMultipleEnumeration
            _count = other.Count;
            _read = other.ElementAt;
            _startIndex = startIndex;
            _write =
                (i, v) =>
                {
                    throw new NotSupportedException(@"Writing items to the underlying IEnumerable is not supported.");
                };
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        ///     Get the enumerator for this enumerable wrapper.
        /// </summary>
        /// <returns>Enumerator.</returns>
        public IEnumerator<TItem> GetEnumerator()
        {
            return Enumerable.Range(_startIndex, _count()).Select(index => _read(index)).GetEnumerator();
        }

        /// <summary>
        ///     Get the enumerator for this enumerable wrapper.
        /// </summary>
        /// <returns>Enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        ///     Get or set the item at the specified index.
        /// </summary>
        /// <param name="index">Index of the item.</param>
        /// <returns>Item at the specified index.</returns>
        public TItem this[int index]
        {
            get { return _read(index); }
            set { _write(index, value); }
        }

        /// <summary>
        ///     Copy the contained items to another array, starting at the specified index.
        /// </summary>
        /// <param name="array">Array to copy to.</param>
        /// <param name="arrayIndex">Index on the target array to start.</param>
        public void CopyTo(TItem[] array, int arrayIndex)
        {
            var count = _count();

            for (var i = 0; i < count; i++)
            {
                array[i] = _read(i + _startIndex);
            }
        }

        /// <summary>
        ///     Copy the contained items to a new array.
        /// </summary>
        /// <returns>Array containing the items.</returns>
        public TItem[] ToArray()
        {
            var result = new TItem[_count()];
            CopyTo(result, 0);
            return result;
        }
    }
}