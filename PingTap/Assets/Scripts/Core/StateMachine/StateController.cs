using UnityEngine;
using UnityEngine.AI;

namespace Fralle
{
  public class StateController : MonoBehaviour
  {
    public State currentState;

    [HideInInspector] public MatchManager matchManager;
    [HideInInspector] public WaveManager waveManager;

    void Awake()
    {
      matchManager = GetComponent<MatchManager>();
      waveManager = GetComponent<WaveManager>();
    }

    void Start()
    {
      currentState.Enter(this);
    }

    void Update()
    {
      currentState.UpdateState(this);
    }

    public void SetState(State nextState)
    {
      currentState.Exit(this);
      currentState = nextState;
      currentState.Enter(this);
    }
  }
}