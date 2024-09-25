using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
public static class EnumerableExtensions
{
    private static Random random = new Random();
    public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> source, int count)
    {
        if (source == null) throw new ArgumentNullException("source");
        if (count < 0) throw new ArgumentOutOfRangeException("count");

        if (count == 0) yield break;

        var queue = new Queue<T>(count);

        foreach (var t in source)
        {
            if (queue.Count == count) queue.Dequeue();

            queue.Enqueue(t);
        }

        foreach (var t in queue)
            yield return t;
    }

    public static T GetLastElement<T>(this IEnumerable<T> source)
    {
        return source.LastOrDefault();
    }

    public static T GetRandom<T>(this IEnumerable<T> source)
    {
        if (source.Count() > 0)
        {
            int index = random.Next(0, source.Count());
            return source.ElementAt(index);
        }
        return default;
    }

    public static T GetRandom<T>(this List<T> source)
    {
        //if (source == null) throw new ArgumentNullException("source");
        //if (source.Count == 0) throw new Exception("GetRandom can't be called since list has no values");

        if (source.Count() > 0)
        {
            return source[random.Next(0, source.Count)];
        }
        return default;

    }
}