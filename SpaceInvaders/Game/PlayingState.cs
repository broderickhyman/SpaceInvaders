using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input.InputListeners;
using SpaceInvaders.Entity;
using SpaceInvaders.Graphics;

namespace SpaceInvaders.Game;
internal class PlayingState : State
{
  private readonly EnemyGrid _enemyGrid;
  private readonly Player _player;
  private readonly List<Bullet> _bullets = new();
  private readonly List<Bullet> _removableBullets = new();
  private Bullet _playerBullet;
  private Bullet _enemyBullet;
  private readonly EnemyConfiguration _enemyConfiguration;
  private readonly MainGame _mainGame;
  private readonly Score _score;

  public PlayingState(
    GameConfiguration gameConfig,
    EnemyConfiguration enemyConfig,
    SpriteBatch spriteBatch,
    SpriteFont spriteFont,
    MainGame game,
    Dictionary<string, Animation> animations) : base(gameConfig, spriteBatch, spriteFont)
  {
    _enemyConfiguration = enemyConfig;
    _player = new Player(1.0f, _gameConfiguration);
    _score = new Score();
    _enemyGrid = new EnemyGrid(10, 5, _gameConfiguration, _enemyConfiguration, _score, animations);
    _mainGame = game;
  }

  public override void Update(GameTime gameTime)
  {
    if (_enemyBullet == default(Bullet))
    {
      _enemyBullet = _enemyGrid.GetBullet();
      if (_enemyBullet != null)
      {
        _bullets.Add(_enemyBullet);
      }
    }

    if (!_player.Disposing && _playerBullet == default(Bullet) && Keyboard.GetState().IsKeyDown(Keys.Space))
    {
      _playerBullet = new Bullet(_player, _gameConfiguration);
      _bullets.Add(_playerBullet);
    }

    _enemyGrid.Update(gameTime);

    _player.Update(gameTime);
    foreach (var bullet in _bullets)
    {
      bullet.Update(gameTime);
    }

    foreach (var bullet in _bullets)
    {
      _player.CheckCollision(bullet);
      _enemyGrid.CheckCollision(bullet);
      if (bullet.Disposing) { _removableBullets.Add(bullet); }
    }
    foreach (var bullet in _removableBullets)
    {
      _bullets.Remove(bullet);
    }
    _removableBullets.Clear();

    if (_playerBullet?.Disposing == true) { _playerBullet = default; }

    if (_enemyBullet?.Disposing == true) { _enemyBullet = default; }

    if (_player.Disposing)
    {
      _mainGame.MoveToPlaying();
    }
  }

  public override void Draw(GameTime gameTime)
  {
    foreach (var bullet in _bullets)
    {
      bullet.Draw(_spriteBatch);
    }
    _player.Draw(_spriteBatch);
    _enemyGrid.Draw(_spriteBatch);

    _spriteBatch.DrawString(_spriteFont, $"Score: {_score.Value,4}",
      new Vector2(_gameConfiguration.WindowWidth - 300, 0), Color.Green,
      0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0);
  }

  public override void KeyReleased(object sender, KeyboardEventArgs e)
  {
    //switch (e.Key)
    //{
    //}
  }
}
