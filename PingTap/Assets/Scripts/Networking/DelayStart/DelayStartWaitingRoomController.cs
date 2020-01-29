using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DelayStartWaitingRoomController : MonoBehaviourPunCallbacks
{
  PhotonView myPhotonView;

  [SerializeField] int multiplayerSceneIndex;
  [SerializeField] int menuSceneIndex;

  int playerCount;
  int roomSize;
  [SerializeField] int minPlayersToStart;

  [SerializeField] Text roomCountDisplay;
  [SerializeField] Text timerToStartDisplay;

  bool readyToCountDown;
  bool readyToStart;
  bool startingGame;

  float timerToStartGame;
  float notFullGameTimer;
  float fullGameTimer;

  [SerializeField] float maxWaitTime;
  [SerializeField] float maxFullGameWaitTime;

  void Start()
  {
    myPhotonView = GetComponent<PhotonView>();
    fullGameTimer = maxFullGameWaitTime;
    notFullGameTimer = maxWaitTime;
    timerToStartGame = maxWaitTime;
    PlayerCountUpdate();
  }

  void Update()
  {
    WaitingForMorePlayers();
  }

  void WaitingForMorePlayers()
  {
    if (playerCount <= 1)
    {
      ResetTimer();
    }
    else if (readyToStart)
    {
      fullGameTimer -= Time.deltaTime;
      timerToStartGame = fullGameTimer;
    }
    else if (readyToCountDown)
    {
      notFullGameTimer -= Time.deltaTime;
      timerToStartGame = notFullGameTimer;
    }

    string tempTimer = string.Format("{0:00}", timerToStartGame);
    timerToStartDisplay.text = tempTimer;
    if (timerToStartGame <= 0f)
    {
      if (startingGame) return;
      StartGame();
    }
  }

  void StartGame()
  {
    startingGame = true;
    if (!PhotonNetwork.IsMasterClient) return;
    PhotonNetwork.CurrentRoom.IsOpen = false;
    PhotonNetwork.LoadLevel(multiplayerSceneIndex);
  }

  void ResetTimer()
  {
    timerToStartGame = maxWaitTime;
    notFullGameTimer = maxWaitTime;
    fullGameTimer = maxFullGameWaitTime;
  }

  void PlayerCountUpdate()
  {
    playerCount = PhotonNetwork.PlayerList.Length;
    roomSize = PhotonNetwork.CurrentRoom.MaxPlayers;
    roomCountDisplay.text = $"{playerCount}:{roomSize}";

    if (playerCount == roomSize) readyToStart = true;
    else if (playerCount >= minPlayersToStart) readyToCountDown = true;
    else
    {
      readyToCountDown = false;
      readyToStart = false;
    }
  }

  public void DelayCancel()
  {
    PhotonNetwork.LeaveRoom();
    SceneManager.LoadScene(menuSceneIndex);
  }

  public override void OnPlayerEnteredRoom(Player newPlayer)
  {
    PlayerCountUpdate();
    if (PhotonNetwork.IsMasterClient) myPhotonView.RPC("RPC_SendTimer", RpcTarget.Others, timerToStartGame);
  }

  public override void OnPlayerLeftRoom(Player otherPlayer)
  {
    PlayerCountUpdate();
  }

  [PunRPC]
  void RPC_SendTimer(float timeIn)
  {
    timerToStartGame = timeIn;
    notFullGameTimer = timeIn;
    if (timeIn < fullGameTimer)
    {
      fullGameTimer = timeIn;
    }
  }
}
