using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
  internal interface IEntityGroup
  {
    Color Color { get; }

    void Update(GameTime gameTime);
    void Draw(SpriteBatch spriteBatch);

    void CheckCollision(Bullet bullet, MainGame game);
  }
}
