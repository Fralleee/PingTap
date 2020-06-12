using UnityEngine;

namespace Fralle.Core.Helpers
{
  [ExecuteInEditMode]
  public class NamingProcess : MonoBehaviour
  {
    [SerializeField] string naming = null;
    [SerializeField] bool increment = false;
    int counter;

    void OnEnable()
    {
      if (increment) FindUnique(naming);
      else name = naming;
    }

    void FindUnique(string name)
    {
      if (GameObject.Find(name))
      {
        counter++;
        FindUnique($"{naming} {counter}");
      }
      else this.name = name;
    }
  }
}