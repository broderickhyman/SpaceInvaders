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
  internal class Player : IEntity
  {
    private RectangleF rectangle;

    public Color Color { get; } = Color.Green;
    public bool Disposing { get; set; }
    public float Speed { get; }
    public ref RectangleF Rectangle { get => ref rectangle; }

    public Player(float speed)
    {
      Reset();
      Speed = speed;
    }

    public void Reset()
    {
      Disposing = false;
      var width = MainGame.WindowWidth / 15;
      var height = MainGame.WindowHeight / 20;
      var x = (MainGame.WindowWidth / 2) - (width / 2);
      var y = MainGame.WindowHeight - (height / 2) - (MainGame.WindowHeight / 20);
      rectangle = new RectangleF(x, y, width, height);
    }

    public static void Initialize()
    {

    }

    public void Update(GameTime gameTime)
    {
      if (Disposing) { return; }
      if (Keyboard.GetState().IsKeyDown(Keys.Left))
      {
        rectangle.Offset(-Speed * gameTime.ElapsedGameTime.Milliseconds, 0);
      }
      if (Keyboard.GetState().IsKeyDown(Keys.Right))
      {
        rectangle.Offset(Speed * gameTime.ElapsedGameTime.Milliseconds, 0);
      }
      if (rectangle.Left < 0)
      {
        rectangle.Position = new Point2(0, rectangle.Position.Y);
      }
      if (rectangle.Right > MainGame.WindowWidth)
      {
        rectangle.Position = new Point2(MainGame.WindowWidth - rectangle.Width, rectangle.Position.Y);
      }
    }

    public void CheckCollision(Bullet bullet, MainGame game)
    {
      if (!(bullet.Parent is Player) && rectangle.Intersects(bullet.Rectangle))
      {
        Disposing = true;
        bullet.Disposing = true;
      }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      if (Disposing) { return; }
      spriteBatch.Begin();
      spriteBatch.FillRectangle(rectangle, Color);
      spriteBatch.End();
    }
  }
}
