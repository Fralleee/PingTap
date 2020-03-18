using System;
using System.Collections.Generic;

public static class LinqExtensions
{
  public static void AddIfUnique<T>(this List<T> list, T element)
  {
    if (!list.Contains(element)) list.Add(element);
  }
  public static void RemoveIfExists<T>(this List<T> list, T element)
  {
    if (list.Contains(element)) list.Remove(element);
  }

  public static T GetRandomElement<T>(this List<T> list)
  {
    Random random = new Random();
    int rnd = random.Next(0, list.Count);
    return list[rnd];
  }
}
