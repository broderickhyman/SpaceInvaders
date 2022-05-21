using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Input.InputListeners;

namespace SpaceInvaders.Game;
internal class PausedState : State
{
  public PlayingState PlayingState { get; }

  public PausedState(
    GameConfiguration gameConfiguration,
    SpriteBatch spriteBatch,
    SpriteFont spriteFont,
    PlayingState playingState
    ) : base(gameConfiguration, spriteBatch, spriteFont)
  {
    PlayingState = playingState;
  }

  public override void Draw(GameTime gameTime)
  {
    PlayingState.Draw(gameTime);

    const string pausedString = "Paused";
    var pausedSize = _spriteFont.MeasureString(pausedString) * 1.1f;
    var pausedRect = new Rectangle(
     (int)(_gameConfiguration.WindowWidth - pausedSize.X) / 2,
     (int)(_gameConfiguration.WindowHeight - pausedSize.Y) / 2,
     (int)pausedSize.X,
     (int)pausedSize.Y);

    _spriteBatch.DrawStringWithBackground(pausedRect,
      _spriteFont,
      pausedString,
      Color.Green,
      Color.White);
  }

  public override void KeyReleased(object sender, KeyboardEventArgs e)
  {
  }

  public override void Update(GameTime gameTime)
  {
  }
}
