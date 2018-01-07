using System;
using System.Collections;
using System.Collections.Generic;
namespace Engine
{
    public struct ReadOnlyList<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
    {
        //
        // Static Fields
        //
        private static ReadOnlyList<T> m_empty = new ReadOnlyList<T>(new T[0]);

        //
        // Fields
        //
        private IList<T> m_list;

        //
        // Static Properties
        //
        public static ReadOnlyList<T> Empty
        {
            get
            {
                return ReadOnlyList<T>.m_empty;
            }
        }

        //
        // Properties
        //
        public int Count
        {
            get
            {
                return this.m_list.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return true;
            }
        }

        //
        // Indexer
        //
        public T this[int index]
        {
            get
            {
                return this.m_list[index];
            }
            set
            {
                throw new NotSupportedException("List is readonly.");
            }
        }

        //
        // Constructors
        //
        public ReadOnlyList(IList<T> list)
        {
            this.m_list = list;
        }

        //
        // Methods
        //
        public void Add(T item)
        {
            throw new NotSupportedException("List is readonly.");
        }

        public void Clear()
        {
            throw new NotSupportedException("List is readonly.");
        }

        public bool Contains(T item)
        {
            return this.m_list.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            this.m_list.CopyTo(array, arrayIndex);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new ReadOnlyList<T>.Enumerator(this.m_list);
        }

        public ReadOnlyList<T>.Enumerator GetEnumerator()
        {
            return new ReadOnlyList<T>.Enumerator(this.m_list);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return new ReadOnlyList<T>.Enumerator(this.m_list);
        }

        public int IndexOf(T item)
        {
            return this.m_list.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            throw new NotSupportedException("List is readonly.");
        }

        public bool Remove(T item)
        {
            throw new NotSupportedException("List is readonly.");
        }

        public void RemoveAt(int index)
        {
            throw new NotSupportedException("List is readonly.");
        }

        //
        // Nested Types
        //
        public struct Enumerator : IEnumerator<T>, IDisposable, IEnumerator
        {
            private IList<T> m_list;

            private int m_index;

            public T Current
            {
                get
                {
                    return this.m_list[this.m_index];
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    return this.m_list[this.m_index];
                }
            }

            public Enumerator(IList<T> list)
            {
                this.m_list = list;
                this.m_index = -1;
            }

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                int num = this.m_index + 1;
                this.m_index = num;
                return num < this.m_list.Count;
            }

            public void Reset()
            {
                this.m_index = -1;
            }
        }
    }
}
