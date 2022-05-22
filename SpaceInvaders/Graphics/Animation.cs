using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders.Graphics;
internal class Animation
{
  //private readonly TimeSpan _lastAnimationTime;
  private readonly Texture2D _texture;

  public Rectangle[] Frames { get; }

  public Animation(ContentManager content, string name)
  {
    _texture = content.Load<Texture2D>(name);
    var bounds = _texture.Bounds;
    const int frameSize = 32;
    var frameColumns = bounds.Width / frameSize;
    var frameRows = bounds.Height / frameSize;
    var frames = new List<Rectangle>();
    for (var xIndex = 0; xIndex < frameColumns; xIndex++)
    {
      for (var yIndex = 0; yIndex < frameRows; yIndex++)
      {
        frames.Add(new Rectangle(xIndex * frameSize, yIndex * frameSize, frameSize, frameSize));
      }
    }
    Frames = frames.ToArray();
  }

  public void Update(GameTime gameTime)
  {
    //if (gameTime.TotalGameTime > _lastAnimationTime + TimeSpan.FromSeconds(1))
    //{
    //  _currentFrame++;
    //  if (_currentFrame >= _frames.Length)
    //  {
    //    _currentFrame = 0;
    //  }
    //  _lastAnimationTime = gameTime.TotalGameTime;
    //}
  }

  public void Draw(SpriteBatch spriteBatch, RectangleF destinationRect, int frame)
  {
    spriteBatch.Draw(_texture, destinationRect.ToRectangle(), Frames[frame], Color.White);
  }
}
