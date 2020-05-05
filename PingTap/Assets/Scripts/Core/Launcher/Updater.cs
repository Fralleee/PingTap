using SimplePatchToolCore;
using System.IO;
using UnityEngine;

namespace Fralle.Core.Launcher
{
  public class Updater : MonoBehaviour
  {
    SimplePatchTool simplePatchTool;

    void Start()
    {
      simplePatchTool = new SimplePatchTool(Path.GetDirectoryName(PatchUtils.GetCurrentExecutablePath()),
        "https://drive.google.com/open?id=1lMF2br4Z9Xy2RL5-hXegf0Bde8Hau3XK");
    }

  }
}