using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
  internal class EnemyGrid : IEntityGroup
  {
    private readonly List<List<Enemy>> columns = new List<List<Enemy>>();
    private readonly List<Enemy> bottomEnemies = new List<Enemy>();
    private RectangleF boundingRectangle;
    public Color Color { get; } = Color.White;
    public bool Disposing { get; private set; }

    private Direction direction = Direction.Right;
    private const float shiftAmount = 20f;
    private const float shiftSeconds = 0.5f;
    private TimeSpan previousUpdate = TimeSpan.Zero;
    private TimeSpan previousRemoval = TimeSpan.Zero;
    private int cols;
    private int rows;

    public IEnumerable<Enemy> Enemies { get { return columns.SelectMany(x => x); } }
    private List<Enemy> removableEnemies = new List<Enemy>();

    public EnemyGrid(int cols, int rows)
    {
      this.cols = cols;
      this.rows = rows;
      Reset();
    }

    internal void Reset()
    {
      Disposing = false;
      columns.Clear();
      bottomEnemies.Clear();
      const float sideGapPercentage = 0.1f;
      var boundingWidth = MainGame.WindowWidth * (1 - (sideGapPercentage * 2));
      var xGap = (boundingWidth - (Enemy.Width * cols)) / cols;
      var boundingHeight = MainGame.WindowHeight * 0.5f;
      var yGap = (boundingHeight - (Enemy.Height * rows)) / rows;
      var topOffset = MainGame.WindowHeight * 0.1f;
      for (var c = 0; c < cols; c++)
      {
        var column = new List<Enemy>();
        columns.Add(column);
        for (var r = 0; r < rows; r++)
        {
          var enemy = new Enemy((sideGapPercentage * MainGame.WindowWidth) + (c * (Enemy.Width + xGap)), topOffset + (r * (Enemy.Height + yGap)));
          if (r == rows - 1)
          {
            enemy.BottomEnemy = true;
            bottomEnemies.Add(enemy);
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

      if (gameTime.TotalGameTime - previousUpdate > TimeSpan.FromSeconds(shiftSeconds))
      {
        previousUpdate = gameTime.TotalGameTime;
        CalculateBoundingRectangle();
        if (direction == Direction.Right)
        {
          if (boundingRectangle.Right > MainGame.WindowWidth - shiftAmount)
          {
            direction = Direction.Left;
            OffsetEnemies(0, shiftAmount);
          }
          else
          {
            OffsetEnemies(shiftAmount, 0);
          }
        }
        else if (direction == Direction.Left)
        {
          if (boundingRectangle.Left < shiftAmount)
          {
            direction = Direction.Right;
            OffsetEnemies(0, shiftAmount);
          }
          else
          {
            OffsetEnemies(-shiftAmount, 0);
          }
        }
      }
    }

    public void CheckCollision(Bullet bullet, MainGame game)
    {
      if (!(bullet.Parent is Enemy))
      {
        foreach (var enemy in Enemies)
        {
          enemy.CheckCollision(bullet, game);
          if (enemy.Disposing)
          {
            removableEnemies.Add(enemy);
            game.EnemyKilled();
          }
        }
        foreach (var enemy in removableEnemies)
        {
          RemoveEnemy(enemy);
        }
        if (removableEnemies.Count > 0) { UpdateBottomEnemies(); }
        removableEnemies.Clear();
      }
    }

    public Bullet GetBullet()
    {
      if (bottomEnemies.Count > 0)
      {
        var enemy = bottomEnemies[MainGame.Random.Next(0, bottomEnemies.Count)];
        return new Bullet(enemy);
      }
      return null;
    }

    private void RemoveEnemy(Enemy enemy)
    {
      foreach (var column in columns)
      {
        column.Remove(enemy);
        if (column.Count == 0)
        {
          columns.Remove(column);
          break;
        }
      }
    }

    private void RemoveRandomEnemy()
    {
      if (columns.Count > 0)
      {
        var col = MainGame.Random.Next(0, columns.Count);
        var rowCount = columns[col].Count;
        if (rowCount > 0)
        {
          var row = MainGame.Random.Next(0, columns[col].Count);
          columns[col].RemoveAt(row);
          if (rowCount == 1 && columns.Count > 0)
            columns.RemoveAt(col);
          UpdateBottomEnemies();
        }
      }
    }

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
      var left = columns.FirstOrDefault()?.DefaultIfEmpty().Min(x => x.Rectangle.Left) ?? 0;
      var right = columns.LastOrDefault()?.DefaultIfEmpty().Max(x => x.Rectangle.Right) ?? MainGame.WindowWidth;
      var top = columns.Select(x => x.FirstOrDefault()).Min(x => x?.Rectangle.Top) ?? 0;
      var bottom = columns.Select(x => x.LastOrDefault()).Max(x => x?.Rectangle.Bottom) ?? MainGame.WindowHeight;
      boundingRectangle = new RectangleF(left, top, right - left, bottom - top);
    }

    private void UpdateBottomEnemies()
    {
      bottomEnemies.Clear();
      foreach (var column in columns)
      {
        var last = column.Last();
        if (last != null)
        {
          last.BottomEnemy = true;
          bottomEnemies.Add(last);
        }
      }
    }
  }

  internal enum Direction
  {
    Left,
    Right
  }
}
