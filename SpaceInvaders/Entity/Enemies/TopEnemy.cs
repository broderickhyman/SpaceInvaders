using SpaceInvaders.Game;
using SpaceInvaders.Graphics;

namespace SpaceInvaders.Entity.Enemies;
internal class TopEnemy : Enemy
{
  public TopEnemy(
    float x, float y,
    GameConfiguration config,
    Dictionary<string, Animation> animations) : base(x, y, config, animations["Top Enemy"])
  {
  }
}
