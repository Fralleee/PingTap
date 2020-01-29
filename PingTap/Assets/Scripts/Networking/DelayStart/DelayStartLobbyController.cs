using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayStartLobbyController : MonoBehaviourPunCallbacks
{
  [SerializeField]
  GameObject delayStartButton;

  [SerializeField]
  GameObject delayCancelButton;

  [SerializeField]
  int roomSize;

  public void DelayStart()
  {
    delayStartButton.SetActive(false);
    delayCancelButton.SetActive(true);
    PhotonNetwork.JoinRandomRoom();
    Debug.Log("Delay start");
  }

  public void DelayCancel()
  {
    delayCancelButton.SetActive(false);
    delayStartButton.SetActive(true);
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
    delayStartButton.SetActive(true);
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
