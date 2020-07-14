using System;
using UnityEngine;

public partial class ObjectPooler
{
  [Serializable]
  public class Pool
  {
    public string tag;
    public GameObject prefab;
    public int size;
  }
}
