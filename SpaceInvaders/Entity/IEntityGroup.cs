using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders.Entity;

internal interface IEntityGroup
{
  Color Color { get; }

  void Update(GameTime gameTime);
  void Draw(SpriteBatch spriteBatch);

  void CheckCollision(Bullet bullet);
}
