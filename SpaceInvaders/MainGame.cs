using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input.InputListeners;
using SpaceInvaders.Entity;
using SpaceInvaders.Game;

namespace SpaceInvaders;

/// <summary>
/// This is the main type for your game.
/// </summary>
internal class MainGame : Microsoft.Xna.Framework.Game
{
  private SpriteBatch _spriteBatch;
  private SpriteFont _spriteFont;
  private readonly GraphicsDeviceManager _graphics;
  private int _frameRate;
  private int _frameCounter;
  private TimeSpan _elapsedTime = TimeSpan.Zero;
  private readonly GameConfiguration _gameConfiguration;
  private readonly EnemyConfiguration _enemyConfiguration;
  private State _state;
  private readonly KeyboardListener _keyboardListener;

  public MainGame()
  {
    _gameConfiguration = new(
      GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 3f,
      GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 1.25f);
    _enemyConfiguration = new(_gameConfiguration);
    _graphics = new GraphicsDeviceManager(this)
    {
      PreferredBackBufferWidth = (int)_gameConfiguration.WindowWidth,
      PreferredBackBufferHeight = (int)_gameConfiguration.WindowHeight
    };
    Window.Position = new Point((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2) - (_graphics.PreferredBackBufferWidth / 2), (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2) - (_graphics.PreferredBackBufferHeight / 2));
    Content.RootDirectory = "Content";
    _keyboardListener = new KeyboardListener();
    _keyboardListener.KeyReleased += KeyReleased;
  }

  /// <summary>
  /// Allows the game to perform any initialization it needs to before starting to run.
  /// This is where it can query for any required services and load any non-graphic
  /// related content.  Calling base.Initialize will enumerate through any components
  /// and initialize them as well.
  /// </summary>
  protected override void Initialize()
  {
    Player.Initialize();
    Bullet.Initialize();
    Enemy.Initialize();
    //playerBullet = new Bullet(player);
    //bullets.Add(playerBullet);
    base.Initialize();
    MoveToPlaying();
  }

  /// <summary>
  /// LoadContent will be called once per game and is the place to load
  /// all of your content.
  /// </summary>
  protected override void LoadContent()
  {
    _spriteBatch = new SpriteBatch(GraphicsDevice);
    _spriteFont = Content.Load<SpriteFont>("Main");
  }

  /// <summary>
  /// UnloadContent will be called once per game and is the place to unload
  /// game-specific content.
  /// </summary>
  protected override void UnloadContent()
  {
  }

  public void MoveToPlaying()
  {
    _state = new PlayingState(_gameConfiguration, _enemyConfiguration, _spriteBatch, _spriteFont, this);
  }

  public void MoveToPlaying(PausedState pausedState)
  {
    _state = pausedState.PlayingState;
  }

  public void MoveToPaused(PlayingState playingState)
  {
    _state = new PausedState(_gameConfiguration, _spriteBatch, _spriteFont, playingState);
  }

  /// <summary>
  /// Allows the game to run logic such as updating the world,
  /// checking for collisions, gathering input, and playing audio.
  /// </summary>
  /// <param name="gameTime">Provides a snapshot of timing values.</param>
  protected override void Update(GameTime gameTime)
  {
    _keyboardListener.Update(gameTime);

    _state.Update(gameTime);
    base.Update(gameTime);

    _elapsedTime += gameTime.ElapsedGameTime;
    if (_elapsedTime > TimeSpan.FromSeconds(1))
    {
      _elapsedTime -= TimeSpan.FromSeconds(1);
      _frameRate = _frameCounter;
      _frameCounter = 0;
    }
  }

  /// <summary>
  /// This is called when the game should draw itself.
  /// </summary>
  /// <param name="gameTime">Provides a snapshot of timing values.</param>
  protected override void Draw(GameTime gameTime)
  {
    _frameCounter++;
    GraphicsDevice.Clear(Color.Black);

    _state.Draw(gameTime);

    _spriteBatch.Begin();
    _spriteBatch.DrawString(_spriteFont, $"FPS: {_frameRate}", new Vector2(0, 0), Color.Green);
    _spriteBatch.End();

    base.Draw(gameTime);
  }

  private void KeyReleased(object sender, KeyboardEventArgs e)
  {
    _state.KeyReleased(sender, e);
    switch (e.Key)
    {
      case Keys.Escape:
        Exit();
        break;
      case Keys.P:
        if (_state is PlayingState playingState)
        {
          MoveToPaused(playingState);
        }
        else if (_state is PausedState pausedState)
        {
          MoveToPlaying(pausedState);
        }
        break;
    }
  }
}
