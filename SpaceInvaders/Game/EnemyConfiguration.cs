namespace SpaceInvaders.Game;
internal struct EnemyConfiguration
{
  public float Width;
  public float Height;

  public EnemyConfiguration(GameConfiguration gameConfiguration)
  {
    Width = gameConfiguration.WindowWidth / 20;
    Height = gameConfiguration.WindowHeight / 25;
  }
}
