using System;
using System.Collections.Generic;

namespace Fralle.Core.Extensions
{
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

    public static void ReplaceItem<T>(this List<T> list, T oldElement, T newElement)
    {
      int index = list.IndexOf(oldElement);
      if (index != -1) list[index] = newElement;
    }

    public static void Upsert<T>(this List<T> list, T oldElement, T newElement)
    {
      int index = list.IndexOf(oldElement);
      if (index != -1) list[index] = newElement;
      else list.Add(newElement);
    }

    public static T GetRandomElement<T>(this List<T> list)
    {
      var random = new Random();
      int rnd = random.Next(0, list.Count);
      return list[rnd];
    }
  }
}