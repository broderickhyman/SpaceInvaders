using Microsoft.Xna.Framework.Graphics;
using SpaceInvaders.Game;

namespace SpaceInvaders.Entity;

internal class Bullet : IEntity
{
  public IEntity Parent;
  private RectangleF _rectangle;
  private readonly float _speed = 0.6f;
  private readonly GameConfiguration _config;

  public Color Color { get; }
  public bool Disposing { get; set; }
  public ref RectangleF Rectangle { get => ref _rectangle; }

  public Bullet(IEntity parent, GameConfiguration config)
  {
    _config = config;
    var width = config.WindowWidth / 180;
    var height = config.WindowHeight / 40;
    _rectangle = new RectangleF(parent.Rectangle.Center.X - (width / 2), parent.Rectangle.Center.Y - (height / 2), width, height);
    Parent = parent;
    Color = parent.Color;
    if (Parent is Player)
    {
      _speed *= -1;
    }
  }

  public static void Initialize()
  {
  }

  public void Update(GameTime gameTime)
  {
    var movement = gameTime.ElapsedGameTime.Milliseconds * _speed;

    _rectangle.Offset(0, movement);
    if ((_rectangle.Bottom < 0 && Parent is Player) || (_rectangle.Top > _config.WindowHeight && Parent is Enemy))
    {
      Disposing = true;
    }
  }

  public void CheckCollision(Bullet bullet, MainGame game)
  {
  }

  public void Draw(SpriteBatch spriteBatch)
  {
    spriteBatch.Begin();
    spriteBatch.FillRectangle(_rectangle, Color);
    spriteBatch.End();
  }
}
