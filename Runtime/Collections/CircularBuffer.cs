// TecVooDoo Utilities
// Copyright (c) 2026 TecVooDoo LLC. All rights reserved.
// Based on CircularBuffer by Adam Myhre (adammyhre)

using System;

namespace TecVooDoo.Utilities
{
    /// <summary>
    /// Fixed-size ring buffer. Overwrites the oldest entry when full.
    /// Zero allocation after initial construction.
    /// </summary>
    public class CircularBuffer<T>
    {
        readonly T[] buffer;
        int head, tail;

        public int Count { get; private set; }
        public int Capacity => buffer.Length;

        public CircularBuffer(int size)
        {
            if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size));
            buffer = new T[size];
        }

        public void Enqueue(T item)
        {
            buffer[head] = item;
            head = (head + 1) % Capacity;
            if (Count == Capacity) tail = (tail + 1) % Capacity;
            else Count++;
        }

        public T Dequeue()
        {
            if (Count == 0) throw new InvalidOperationException("Buffer is empty.");
            T item = buffer[tail];
            tail = (tail + 1) % Capacity;
            Count--;
            return item;
        }

        /// <summary>Access by logical index. 0 = oldest, Count-1 = newest.</summary>
        public T this[int index]
        {
            get
            {
                if ((uint)index >= (uint)Count) throw new ArgumentOutOfRangeException(nameof(index));
                if (Capacity == 0 || buffer == null) throw new InvalidOperationException();
                return buffer[(tail + index) % Capacity];
            }
        }
    }
}
