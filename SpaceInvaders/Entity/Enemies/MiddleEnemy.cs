using SpaceInvaders.Game;
using SpaceInvaders.Graphics;

namespace SpaceInvaders.Entity.Enemies;
internal class MiddleEnemy : Enemy
{
  public MiddleEnemy(
    float x, float y,
    GameConfiguration config,
    Dictionary<string, Animation> animations) : base(x, y, config, animations["Middle Enemy"])
  {
  }
}
