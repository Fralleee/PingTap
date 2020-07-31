using Fralle.Core.Infrastructure;

public class GameManager : Singleton<GameManager>
{
  //- Wave defintion: Number of waves(1 -> Infinite)
  //- Wave definition: Algorithm to increase zombies per waves(procedural)

  //- Zombie AI: Zombies main target is the door to the bar and should move towards this
  //- Zombie AI: When zombies reach target they die and target loses health

  //- Freeze time: On start of match and between waves

  //- Use Handles to display spawn radius

  public int waveCount;
  public int currentWave;
  public float zombieLevelIncreaseFactor = 1.2f;

  void Start()
  {

  }

  void Update()
  {

  }
}
