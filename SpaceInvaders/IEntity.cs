using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
  internal interface IEntity
  {
    float Speed { get; set; }

    void Initialize();

    void Update();

    void Draw(SpriteBatch spriteBatch);
  }
}
