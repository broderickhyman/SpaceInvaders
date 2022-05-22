using SpaceInvaders.Entity.Enemies;

namespace SpaceInvaders.Game;
internal class Score
{
  public int Value { get; private set; }

  public void EnemyKilled(Enemy enemy)
  {
    if (enemy is TopEnemy)
    {
      Value += 30;
    }
    else if (enemy is MiddleEnemy)
    {
      Value += 20;
    }
    else if (enemy is BottomEnemy)
    {
      Value += 10;
    }
    else
    {
      throw new NotSupportedException();
    }
  }
}
