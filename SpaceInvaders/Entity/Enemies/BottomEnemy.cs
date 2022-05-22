using SpaceInvaders.Game;
using SpaceInvaders.Graphics;

namespace SpaceInvaders.Entity.Enemies;
internal class BottomEnemy : Enemy
{
  public BottomEnemy(
    float x, float y,
    GameConfiguration config,
    Dictionary<string, Animation> animations) : base(x, y, config, animations["Bottom Enemy"])
  {
  }
}
