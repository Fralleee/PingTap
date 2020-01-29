using Photon.Pun;
using System.IO;
using UnityEngine;

public class GameSetupController : MonoBehaviour
{
  void Start()
  {
    CreatePlayer();
  }

  void CreatePlayer()
  {
    Debug.Log("Creating player");
    PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonPlayer"), Vector3.zero, Quaternion.identity);
  }
}
