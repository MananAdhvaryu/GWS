using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MnM.GWS.EnumerableExtensions
{
    public static partial class EnumerableHelper
    {
        public static IEnumerable<T> AppendItems<T>(this IEnumerable<T> source, IEnumerable<T> newElements)
        {
            if (source != null)
            {
                foreach (var item in source)
                    yield return item;
            }
            if (newElements != null)
            {
                foreach (var item in newElements)
                    yield return item;
            }
        }
        public static IEnumerable<T> PrependItems<T>(this IEnumerable<T> source, IEnumerable<T> newElements)
        {
            if (newElements != null)
            {
                foreach (var item in newElements)
                    yield return item;
            }
            if (source != null)
            {
                foreach (var item in source)
                    yield return item;
            }
        }

        public static IEnumerable<T> AppendItem<T>(this IEnumerable<T> source, T newElement)
        {
            if (source != null)
            {
                foreach (var item in source)
                    yield return item;
            }
            if (newElement != null)
                yield return newElement;
        }
        public static IEnumerable<T> PrependItem<T>(this IEnumerable<T> source, T newElement)
        {
            if (newElement != null)
                yield return newElement;
            if (source != null)
            {
                foreach (var item in source)
                    yield return item;
            }
        }

        public static IEnumerable<T> ToIEnumerable<T>(params T[] values) =>
            values;

        public static IReadOnlyList<T> ToReadonlyList<T>(this IEnumerable<T> collection) =>
            new ReadOnlyList<T>(collection);

        public static T[] InitializeArray<T>(int count, T @default)
        {
            var arr = new T[count];
            for (int i = 0; i < count; i++)
            {
                arr[i] = @default;
            }
            return arr;
        }
        public static void ChangeAll<T>(ref T[] arr, T @default)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = @default;
            }
        }
        class ReadOnlyList<T> : IReadOnlyList<T>
        {
            IList<T> items;

            public ReadOnlyList(IEnumerable<T> items)
            {
                if (items is IList<T>)
                    this.items = items as IList<T>;
                else
                {
                    this.items = items.ToArray();
                }
            }

            public T this[int index] => items[index];
            public int Count => items.Count;
            object IReadOnlyList.this[int index] => this[index];
            public IEnumerator<T> GetEnumerator() => items.GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => items.GetEnumerator();
        }

        public static IEnumerable<T> AppendItems<T>(this IEnumerable<T> source, params T[] newElements) =>
            AppendItems(source, newElements as IEnumerable<T>);
        public static IEnumerable<T> PrependItems<T>(this IEnumerable<T> source, params T[] newElements) =>
            PrependItems(source, newElements as IEnumerable<T>);

        public static IEnumerable<T> InReverse<T>(this IList<T> collection) =>
            collection.InReverse(collection.Count - 1);
        public static IEnumerable<T> InReverse<T>(this IList<T> collection, int start)
        {
            for (int i = start; i >= 0; i--)
                yield return collection[i];
        }
        public static IEnumerable<T> InForward<T>(this IList<T> collection, int start)
        {
            for (int i = start; i < collection.Count; i++)
                yield return collection[i];
        }

        public static bool FirstMatch<T>(this IList<T> collection,
            Predicate<T> condition, out T result, out int idx,
            int start = 0, bool reverse = false, int? end = null)
        {
            int last;
            if (reverse)
            {
                last = end ?? 0;
                for (int i = start; i >= end; i--)
                {
                    if (condition(collection[i]))
                    {
                        result = collection[i];
                        idx = i;
                        return true;
                    }
                }
            }
            else
            {
                last = end ?? (collection.Count - 1) - start;
                for (int i = start; i <= last; i++)
                {
                    if (condition(collection[i]))
                    {
                        result = collection[i];
                        idx = i;
                        return true;
                    }
                }
            }
            idx = -1;
            result = default(T);
            return false;
        }

        public static void CorrectLength(ref int start, ref int length, int listLength)
        {
            if (start >= listLength || listLength <= 0)
            {
                start = 0; length = 0;
                return;
            }
            if (length < 0) { length = listLength; }
            start = System.Math.Max(0, start);
            listLength -= start;
            listLength = System.Math.Max(0, listLength);
            length = System.Math.Max(0, System.Math.Min(length, listLength));
            start = System.Math.Min(start, System.Math.Max(0, start + length - 1));
        }
    }
}
