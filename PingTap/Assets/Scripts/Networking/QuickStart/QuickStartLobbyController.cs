using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickStartLobbyController : MonoBehaviourPunCallbacks
{
  [SerializeField]
  GameObject quickStartButton;

  [SerializeField]
  GameObject quickCancelButton;

  [SerializeField]
  int roomSize;

  public void QuickStart()
  {
    quickStartButton.SetActive(false);
    quickCancelButton.SetActive(true);
    PhotonNetwork.JoinRandomRoom();
    Debug.Log("Quick start");
  }

  public void QuickCancel()
  {
    quickCancelButton.SetActive(false);
    quickStartButton.SetActive(true);
    PhotonNetwork.LeaveRoom();
  }

  void CreateRoom()
  {
    Debug.Log("Creating room now");
    int randomRoomNumber = Random.Range(0, 10000);
    RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)roomSize };
    PhotonNetwork.CreateRoom($"Room {randomRoomNumber}", roomOps);
    Debug.Log($"Room {randomRoomNumber}");
  }

  public override void OnConnectedToMaster()
  {
    PhotonNetwork.AutomaticallySyncScene = true;
    quickStartButton.SetActive(true);
  }
  
  public override void OnJoinRandomFailed(short returnCode, string message)
  {
    Debug.LogError("Failed to join a room");
    CreateRoom();
  }

  public override void OnCreateRoomFailed(short returnCode, string message)
  {
    Debug.LogError("Failed to create room... trying again");
    CreateRoom();
  }

}
