﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input.InputListeners;
using SpaceInvaders.Entity;
using SpaceInvaders.Entity.Enemies;
using SpaceInvaders.Game;
using SpaceInvaders.Graphics;
using SpaceInvaders.Utilities;

namespace SpaceInvaders;

/// <summary>
/// This is the main type for your game.
/// </summary>
internal class MainGame : Microsoft.Xna.Framework.Game
{
  private SpriteBatch _spriteBatch;
  private SpriteFont _spriteFont;
  private readonly GraphicsDeviceManager _graphics;
  private readonly GameConfiguration _gameConfiguration;
  private readonly EnemyConfiguration _enemyConfiguration;
  private State _state;
  private readonly KeyboardListener _keyboardListener;
  private readonly Dictionary<string, Animation> _animations = new();
  private readonly ActionsPerSecond _frameActions = new();
  private readonly ActionsPerSecond _updateActions = new();

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
    //TargetElapsedTime = TimeSpan.FromSeconds(1d / 144d);
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
    AddAnimation("Bottom Enemy");
    AddAnimation("Middle Enemy");
    AddAnimation("Top Enemy");
    AddAnimation("Mystery Enemy");
  }

  private void AddAnimation(string name)
  {
    _animations[name] = new(Content, name);
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
    _state = new PlayingState(_gameConfiguration, _enemyConfiguration, _spriteBatch, _spriteFont, this, _animations);
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

    _updateActions.Action(gameTime);
  }

  /// <summary>
  /// This is called when the game should draw itself.
  /// </summary>
  /// <param name="gameTime">Provides a snapshot of timing values.</param>
  protected override void Draw(GameTime gameTime)
  {
    _frameActions.Action(gameTime);
    GraphicsDevice.Clear(Color.Black);

    _spriteBatch.Begin();

    _state.Draw(gameTime);

    const float stringScale = 0.5f;
    const string fpsString = "FPS";
    var fpsSize = _spriteFont.MeasureString(fpsString) * stringScale;

    _spriteBatch.DrawString(_spriteFont, $"{fpsString}: {_frameActions.Rate}",
      new Vector2(0, 0), Color.Green,
      0f, Vector2.Zero, stringScale, SpriteEffects.None, 0);
    _spriteBatch.DrawString(_spriteFont, $"UPS: {_updateActions.Rate}",
      new Vector2(0, fpsSize.Y), Color.Green,
      0f, Vector2.Zero, stringScale, SpriteEffects.None, 0);
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
