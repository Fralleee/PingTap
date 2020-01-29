using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DelayStartRoomController : MonoBehaviourPunCallbacks
{
  [SerializeField]
  int waitingRoomSceneIndex;

  public override void OnEnable()
  {
    // registers to photon callbacks
    PhotonNetwork.AddCallbackTarget(this);
  }

  public override void OnDisable()
  {
    PhotonNetwork.RemoveCallbackTarget(this);
  }

  public override void OnJoinedRoom()
  {
    SceneManager.LoadScene(waitingRoomSceneIndex);
  }

}
