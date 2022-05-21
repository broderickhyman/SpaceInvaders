using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Input.InputListeners;
using SpaceInvaders.Entity;

namespace SpaceInvaders.Game;

internal abstract class State : ICanHandleInput
{
  protected readonly GameConfiguration _gameConfiguration;
  protected readonly SpriteBatch _spriteBatch;
  protected readonly SpriteFont _spriteFont;

  protected State(
    GameConfiguration gameConfiguration,
    SpriteBatch spriteBatch,
    SpriteFont spriteFont
    )
  {
    _gameConfiguration = gameConfiguration;
    _spriteBatch = spriteBatch;
    _spriteFont = spriteFont;
  }

  public abstract void Update(GameTime gameTime);
  public abstract void Draw(GameTime gameTime);
  public abstract void KeyReleased(object sender, KeyboardEventArgs e);
}
