namespace SpaceInvaders.Utilities;
internal class ActionsPerSecond
{
  private int _counter;
  private TimeSpan _lastUpdate = TimeSpan.Zero;

  public int Rate { get; private set; }

  public void Action(GameTime gameTime)
  {
    _counter++;
    if (gameTime.TotalGameTime > _lastUpdate + TimeSpan.FromSeconds(1))
    {
      Rate = _counter;
      _counter = 0;
      _lastUpdate = gameTime.TotalGameTime;
    }
  }
}
