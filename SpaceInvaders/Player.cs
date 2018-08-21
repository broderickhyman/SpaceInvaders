using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
  internal class Player : IEntity
  {
    private RectangleF rectangle;

    public float Speed { get; set; }

    public Player(RectangleF rect)
    {
      rectangle = rect;
    }

    public void Initialize()
    {

    }

    public void Update()
    {

    }

    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Begin();
      spriteBatch.FillRectangle(rectangle, Color.Green);
      spriteBatch.End();
    }
  }
}
