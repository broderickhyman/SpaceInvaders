using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceInvaders.Entity;
using SpaceInvaders.Game;

namespace SpaceInvaders;

/// <summary>
/// This is the main type for your game.
/// </summary>
public class MainGame : Microsoft.Xna.Framework.Game
{
  private SpriteBatch _spriteBatch;
  private SpriteFont _spriteFont;
  private readonly GraphicsDeviceManager _graphics;
  private readonly EnemyGrid _enemyGrid;
  private readonly Player _player;
  private readonly List<Bullet> _bullets = new();
  private readonly List<Bullet> _removableBullets = new();
  private Bullet _playerBullet;
  private Bullet _enemyBullet;
  private int _frameRate;
  private int _frameCounter;
  private TimeSpan _elapsedTime = TimeSpan.Zero;
  private int _score;
  private readonly GameConfiguration _gameConfiguration;
  private readonly EnemyConfiguration _enemyConfiguration;

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
    _player = new Player(1.5f, _gameConfiguration);
    _enemyGrid = new EnemyGrid(10, 5, _gameConfiguration, _enemyConfiguration);
  }

  private void Reset()
  {
    _score = 0;
    _playerBullet = default;
    _enemyBullet = default;
    _bullets.Clear();
    _player.Reset();
    _enemyGrid.Reset();
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

  /// <summary>
  /// Allows the game to run logic such as updating the world,
  /// checking for collisions, gathering input, and playing audio.
  /// </summary>
  /// <param name="gameTime">Provides a snapshot of timing values.</param>
  protected override void Update(GameTime gameTime)
  {
    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
    {
      Exit();
    }
    if (!_player.Disposing && _playerBullet == default(Bullet) && Keyboard.GetState().IsKeyDown(Keys.Space))
    {
      _playerBullet = new Bullet(_player, _gameConfiguration);
      _bullets.Add(_playerBullet);
    }

    if (_enemyBullet == default(Bullet))
    {
      _enemyBullet = _enemyGrid.GetBullet();
      if (_enemyBullet != null)
      {
        _bullets.Add(_enemyBullet);
      }
    }

    _enemyGrid.Update(gameTime);

    _player.Update(gameTime);
    foreach (var bullet in _bullets)
    {
      bullet.Update(gameTime);
    }
    base.Update(gameTime);

    foreach (var bullet in _bullets)
    {
      _player.CheckCollision(bullet, this);
      _enemyGrid.CheckCollision(bullet, this);
      if (bullet.Disposing) { _removableBullets.Add(bullet); }
    }
    foreach (var bullet in _removableBullets)
    {
      _bullets.Remove(bullet);
    }
    _removableBullets.Clear();

    if (_playerBullet?.Disposing == true) { _playerBullet = default; }

    if (_enemyBullet?.Disposing == true) { _enemyBullet = default; }

    _elapsedTime += gameTime.ElapsedGameTime;
    if (_elapsedTime > TimeSpan.FromSeconds(1))
    {
      _elapsedTime -= TimeSpan.FromSeconds(1);
      _frameRate = _frameCounter;
      _frameCounter = 0;
    }
    if (_player.Disposing)
    {
      Reset();
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

    foreach (var bullet in _bullets) { bullet.Draw(_spriteBatch); }
    _player.Draw(_spriteBatch);

    _enemyGrid.Draw(_spriteBatch);

    _spriteBatch.Begin();
    _spriteBatch.DrawString(_spriteFont, $"FPS: {_frameRate}", new Vector2(0, 0), Color.Green);
    _spriteBatch.DrawString(_spriteFont, $"Score: {_score,4}", new Vector2(_gameConfiguration.WindowWidth - 300, 0), Color.Green);
    _spriteBatch.End();

    base.Draw(gameTime);
  }

  public void EnemyKilled()
  {
    _score += 100;
  }
}
