namespace SpaceInvaders.Game;
internal class Score
{
  public int Value { get; private set; }

  public void EnemyKilled()
  {
    Value += 100;
  }
}
