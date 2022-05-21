using Microsoft.Xna.Framework.Graphics;
using SpaceInvaders.Game;

namespace SpaceInvaders.Entity;

internal class Enemy : IEntity
{
  private RectangleF _rectangle;

  private readonly float _width;
  private readonly float _height;

  public Color Color { get; } = Color.White;
  public bool Disposing { get; private set; }
  public bool BottomEnemy { get; set; }
  public float Speed { get; set; }
  public ref RectangleF Rectangle { get => ref _rectangle; }

  public Enemy(float x, float y, GameConfiguration config)
  {
    _width = config.WindowWidth / 20;
    _height = config.WindowHeight / 25;
    _rectangle = new RectangleF(x, y, _width, _height);
  }

  public static void Initialize()
  {
  }

  public void Update(GameTime gameTime)
  {
  }

  public void CheckCollision(Bullet bullet)
  {
    if (bullet.Parent is not Enemy && _rectangle.Intersects(bullet.Rectangle))
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
    spriteBatch.FillRectangle(_rectangle, color);
    spriteBatch.End();
  }
}
