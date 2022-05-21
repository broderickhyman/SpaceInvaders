namespace SpaceInvaders.Game;
internal struct GameConfiguration
{
  public float WindowWidth;
  public float WindowHeight;

  public GameConfiguration(float windowWidth, float windowHeight)
  {
    WindowHeight = windowHeight;
    WindowWidth = windowWidth;
  }
}
