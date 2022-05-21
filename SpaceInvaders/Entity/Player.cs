using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceInvaders.Game;

namespace SpaceInvaders.Entity;

internal class Player : IEntity
{
  private RectangleF _rectangle;
  private GameConfiguration _gameConfiguration;

  public Color Color { get; } = Color.Green;
  public bool Disposing { get; set; }
  public float Speed { get; }
  public ref RectangleF Rectangle { get => ref _rectangle; }

  public Player(float speed, GameConfiguration gameConfiguration)
  {
    Speed = speed;
    _gameConfiguration = gameConfiguration;
    Reset();
  }

  public void Reset()
  {
    Disposing = false;
    var width = _gameConfiguration.WindowWidth / 15;
    var height = _gameConfiguration.WindowHeight / 20;
    var x = (_gameConfiguration.WindowWidth / 2) - (width / 2);
    var y = _gameConfiguration.WindowHeight - (height / 2) - (_gameConfiguration.WindowHeight / 20);
    _rectangle = new RectangleF(x, y, width, height);
  }

  public static void Initialize()
  {
  }

  public void Update(GameTime gameTime)
  {
    if (Disposing) { return; }
    if (Keyboard.GetState().IsKeyDown(Keys.Left))
    {
      _rectangle.Offset(-Speed * gameTime.ElapsedGameTime.Milliseconds, 0);
    }
    if (Keyboard.GetState().IsKeyDown(Keys.Right))
    {
      _rectangle.Offset(Speed * gameTime.ElapsedGameTime.Milliseconds, 0);
    }
    if (_rectangle.Left < 0)
    {
      _rectangle.Position = new Point2(0, _rectangle.Position.Y);
    }
    if (_rectangle.Right > _gameConfiguration.WindowWidth)
    {
      _rectangle.Position = new Point2(_gameConfiguration.WindowWidth - _rectangle.Width, _rectangle.Position.Y);
    }
  }

  public void CheckCollision(Bullet bullet)
  {
    if (bullet.Parent is not Player && _rectangle.Intersects(bullet.Rectangle))
    {
      Disposing = true;
      bullet.Disposing = true;
    }
  }

  public void Draw(SpriteBatch spriteBatch)
  {
    if (Disposing) { return; }
    spriteBatch.Begin();
    spriteBatch.FillRectangle(_rectangle, Color);
    spriteBatch.End();
  }
}
