using Microsoft.Xna.Framework.Graphics;
using SpaceInvaders.Game;

namespace SpaceInvaders.Entity;

internal class EnemyGrid : IEntityGroup
{
  private readonly List<List<Enemy>> _columns = new();
  private readonly List<Enemy> _bottomEnemies = new();
  private RectangleF _boundingRectangle;
  public Color Color { get; } = Color.White;
  public bool Disposing { get; private set; }

  private Direction _direction = Direction.Right;
  private const float ShiftAmount = 20f;
  private const float ShiftSeconds = 0.75f;
  private TimeSpan _previousUpdate = TimeSpan.Zero;
  //private readonly TimeSpan previousRemoval = TimeSpan.Zero;
  private readonly int _cols;
  private readonly int _rows;
  private readonly GameConfiguration _gameConfig;
  private readonly EnemyConfiguration _enemyConfig;
  private readonly List<Enemy> _removableEnemies = new();
  private readonly Score _score;

  public IEnumerable<Enemy> Enemies { get { return _columns.SelectMany(x => x); } }

  public EnemyGrid(int cols, int rows,
    GameConfiguration gameConfig,
    EnemyConfiguration enemyConfig,
    Score score)
  {
    _cols = cols;
    _rows = rows;
    _gameConfig = gameConfig;
    _enemyConfig = enemyConfig;
    _score = score;
    Reset();
  }

  internal void Reset()
  {
    Disposing = false;
    _columns.Clear();
    _bottomEnemies.Clear();
    const float sideGapPercentage = 0.1f;
    var boundingWidth = _gameConfig.WindowWidth * (1 - (sideGapPercentage * 2));
    var xGap = (boundingWidth - (_enemyConfig.Width * _cols)) / _cols;
    var boundingHeight = _gameConfig.WindowHeight * 0.5f;
    var yGap = (boundingHeight - (_enemyConfig.Height * _rows)) / _rows;
    var topOffset = _gameConfig.WindowHeight * 0.1f;
    for (var c = 0; c < _cols; c++)
    {
      var column = new List<Enemy>();
      _columns.Add(column);
      for (var r = 0; r < _rows; r++)
      {
        var enemy = new Enemy(
          (sideGapPercentage * _gameConfig.WindowWidth) + (c * (_enemyConfig.Width + xGap)), topOffset + (r * (_enemyConfig.Height + yGap)),
          _gameConfig);
        if (r == _rows - 1)
        {
          enemy.BottomEnemy = true;
          _bottomEnemies.Add(enemy);
        }
        column.Add(enemy);
      }
    }
  }

  public static void Initialize()
  {
  }

  public void Update(GameTime gameTime)
  {
    //double enemyCount = Enemies.Count() + 1;
    //if (gameTime.TotalGameTime - previousRemoval > TimeSpan.FromSeconds((1 - (enemyCount / 50)) * 2))
    //{
    //  previousRemoval = gameTime.TotalGameTime;
    //  RemoveRandomEnemy();
    //}

    if (gameTime.TotalGameTime - _previousUpdate > TimeSpan.FromSeconds(ShiftSeconds))
    {
      _previousUpdate = gameTime.TotalGameTime;
      CalculateBoundingRectangle();
      if (_direction == Direction.Right)
      {
        if (_boundingRectangle.Right > _gameConfig.WindowWidth - ShiftAmount)
        {
          _direction = Direction.Left;
          OffsetEnemies(0, ShiftAmount);
        }
        else
        {
          OffsetEnemies(ShiftAmount, 0);
        }
      }
      else if (_direction == Direction.Left)
      {
        if (_boundingRectangle.Left < ShiftAmount)
        {
          _direction = Direction.Right;
          OffsetEnemies(0, ShiftAmount);
        }
        else
        {
          OffsetEnemies(-ShiftAmount, 0);
        }
      }
    }
  }

  public void CheckCollision(Bullet bullet)
  {
    if (bullet.Parent is not Enemy)
    {
      foreach (var enemy in Enemies)
      {
        enemy.CheckCollision(bullet);
        if (enemy.Disposing)
        {
          _removableEnemies.Add(enemy);
          _score.EnemyKilled();
        }
      }
      foreach (var enemy in _removableEnemies)
      {
        RemoveEnemy(enemy);
      }
      if (_removableEnemies.Count > 0) { UpdateBottomEnemies(); }
      _removableEnemies.Clear();
    }
  }

  public Bullet GetBullet()
  {
    if (_bottomEnemies.Count > 0)
    {
      var enemy = _bottomEnemies[Random.Shared.Next(0, _bottomEnemies.Count)];
      return new Bullet(enemy, _gameConfig);
    }
    return null;
  }

  private void RemoveEnemy(Enemy enemy)
  {
    foreach (var column in _columns)
    {
      column.Remove(enemy);
      if (column.Count == 0)
      {
        _columns.Remove(column);
        break;
      }
    }
  }

  //private void RemoveRandomEnemy()
  //{
  //  if (columns.Count > 0)
  //  {
  //    var col = MainGame.Random.Next(0, columns.Count);
  //    var rowCount = columns[col].Count;
  //    if (rowCount > 0)
  //    {
  //      var row = MainGame.Random.Next(0, columns[col].Count);
  //      columns[col].RemoveAt(row);
  //      if (rowCount == 1 && columns.Count > 0)
  //        columns.RemoveAt(col);
  //      UpdateBottomEnemies();
  //    }
  //  }
  //}

  private void OffsetEnemies(float x, float y)
  {
    foreach (var enemy in Enemies)
    {
      enemy.Rectangle.Offset(x, y);
    }
  }

  public void Draw(SpriteBatch spriteBatch)
  {
    foreach (var enemy in Enemies)
    {
      enemy.Draw(spriteBatch);
    }
  }

  private void CalculateBoundingRectangle()
  {
    var left = _columns.FirstOrDefault()?.DefaultIfEmpty().Min(x => x.Rectangle.Left) ?? 0;
    var right = _columns.LastOrDefault()?.DefaultIfEmpty().Max(x => x.Rectangle.Right) ?? _gameConfig.WindowWidth;
    var top = _columns.Select(x => x.FirstOrDefault()).Min(x => x?.Rectangle.Top) ?? 0;
    var bottom = _columns.Select(x => x.LastOrDefault()).Max(x => x?.Rectangle.Bottom) ?? _gameConfig.WindowHeight;
    _boundingRectangle = new RectangleF(left, top, right - left, bottom - top);
  }

  private void UpdateBottomEnemies()
  {
    _bottomEnemies.Clear();
    foreach (var column in _columns)
    {
      var last = column.Last();
      if (last != null)
      {
        last.BottomEnemy = true;
        _bottomEnemies.Add(last);
      }
    }
  }
}

internal enum Direction
{
  Left,
  Right
}
