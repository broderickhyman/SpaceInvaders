﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
  internal class Bullet : IEntity
  {
    private readonly float speed = 1.0f;
    private RectangleF rectangle;
    public IEntity Parent;
    public Color Color { get; }

    public bool Disposing { get; private set; }
    public RectangleF Rectangle { get => rectangle; private set { } }

    public Bullet(IEntity parent)
    {
      var width = MainGame.WindowWidth / 180;
      var height = MainGame.WindowHeight / 40;
      rectangle = new RectangleF(parent.Rectangle.Center.X - (width / 2), parent.Rectangle.Center.Y - (height / 2), width, height);
      Parent = parent;
      Color = parent.Color;
      if (Parent is Player)
      {
        speed *= -1;
      }
    }

    public static void Initialize()
    {

    }

    public void Update(GameTime gameTime)
    {
      var movement = gameTime.ElapsedGameTime.Milliseconds * speed;

      rectangle.Offset(0, movement);
      if ((rectangle.Bottom < 0 && Parent is Player) || (rectangle.Top > MainGame.WindowHeight && Parent is Enemy))
      {
        Disposing = true;
      }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Begin();
      spriteBatch.FillRectangle(rectangle, Color);
      spriteBatch.End();
    }
  }
}
