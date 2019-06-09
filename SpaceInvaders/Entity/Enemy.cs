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
    public static float Width = MainGame.WindowWidth / 20;
    public static float Height = MainGame.WindowHeight / 25;

    private RectangleF rectangle;

    public Color Color { get; } = Color.White;
    public bool Disposing { get; private set; }
    public bool BottomEnemy { get; set; }
    public float Speed { get; set; }
    public ref RectangleF Rectangle { get => ref rectangle; }

    public Enemy(float x, float y)
    {
      rectangle = new RectangleF(x, y, Width, Height);
    }

    public static void Initialize()
    {

    }

    public void Update(GameTime gameTime)
    {

    }

    public void CheckCollision(Bullet bullet, MainGame game)
    {
      if (!(bullet.Parent is Enemy) && rectangle.Intersects(bullet.Rectangle))
      {
        Disposing = true;
        bullet.Disposing = true;
      }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      var color = Color;
      if (BottomEnemy) { color = Color.Red; }
      spriteBatch.Begin();
      spriteBatch.FillRectangle(rectangle, color);
      spriteBatch.End();
    }
  }
}
