using Fralle.AI;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
  Animator animator;
  Enemy enemy;

  void Awake()
  {
    animator = GetComponent<Animator>();
    var state = animator.GetCurrentAnimatorStateInfo(0);
    animator.Play(state.fullPathHash, -1, Random.Range(0f, 1f));

    enemy = GetComponent<Enemy>();
  }

  public void ToggleAnimator(bool enabled)
  {
    animator.enabled = enabled;
  }

  public void SetModifier(float modifier)
  {
    animator.speed = 1f * modifier;
  }

  void OnAnimatorMove()
  {
    enemy.enemyNavigation.SetSpeed((animator.deltaPosition / Time.deltaTime).magnitude);
  }
}
