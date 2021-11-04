using UnityEngine;

public class RestlessAnimator : StateMachineBehaviour
{
  [SerializeField] Vector2 restlessMinMaxTimer = new Vector2(6, 15);

  string[] restlessTriggers = { "Restless1", "Restless2" };
  float timer;
  System.Random random = new System.Random();

  override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    SetTimer();
  }

  override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    if (timer <= 0)
    {
      TriggerRestlessAnimation(animator);
      SetTimer();
    }
    else
      timer -= Time.deltaTime;
  }

  void SetTimer()
  {
    timer = Random.Range(restlessMinMaxTimer.x, restlessMinMaxTimer.y);
  }

  void TriggerRestlessAnimation(Animator animator)
  {
    int index = random.Next(restlessTriggers.Length);
    string trigger = restlessTriggers[index];
    animator.SetTrigger(trigger);
  }
}
