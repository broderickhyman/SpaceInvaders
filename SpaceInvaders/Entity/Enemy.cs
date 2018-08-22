using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
  internal class Enemy : IEntity
  {
    private RectangleF rectangle;

    public float Speed { get; set; }
    public RectangleF Rectangle { get => rectangle; private set { } }
    public Color Color { get; } = Color.White;

    public Enemy(RectangleF rectangle)
    {
      this.rectangle = rectangle;
    }

    public static void Initialize()
    {

    }

    public void Update(GameTime gameTime)
    {

    }

    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Begin();
      spriteBatch.FillRectangle(rectangle, Color);
      spriteBatch.End();
    }
  }
}
