using Microsoft.Xna.Framework.Graphics;
using SpaceInvaders.Game;
using SpaceInvaders.Graphics;

namespace SpaceInvaders.Entity.Enemies;
internal abstract class Enemy : IEntity
{
  private RectangleF _rectangle;
  private readonly Animation _animation;
  private int _currentFrame;

  private readonly float _width;
  private readonly float _height;

  public Color Color { get; } = Color.White;
  public bool Disposing { get; private set; }
  public bool BottomEnemy { get; set; }
  public float Speed { get; set; }
  public ref RectangleF Rectangle { get => ref _rectangle; }

  protected Enemy(
    float x, float y,
    GameConfiguration config,
    Animation animation)
  {
    _width = config.WindowWidth / 20;
    _height = config.WindowHeight / 25;
    _rectangle = new RectangleF(x, y, _width, _height);
    _animation = animation;
  }

  public static void Initialize()
  {
  }

  public void Update(GameTime gameTime)
  {
    _animation.Update(gameTime);
  }

  public void UpdateFrame()
  {
    _currentFrame++;
    if (_currentFrame >= _animation.Frames.Length)
    {
      _currentFrame = 0;
    }
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
    //var color = Color;
    //if (BottomEnemy) { color = Color.Red; }
    _animation.Draw(spriteBatch, Rectangle, _currentFrame);
  }
}
