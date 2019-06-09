using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceInvaders
{
  /// <summary>
  /// This is the main type for your game.
  /// </summary>
  public class MainGame : Game
  {
    private SpriteBatch spriteBatch;
    private SpriteFont spriteFont;
    private readonly GraphicsDeviceManager graphics;
    private readonly EnemyGrid enemyGrid;
    private readonly Player player;
    private readonly List<Bullet> bullets = new List<Bullet>();
    private readonly List<Bullet> removableBullets = new List<Bullet>();
    public static float WindowWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 3f;
    public static float WindowHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 1.25f;
    private Bullet playerBullet;
    private Bullet enemyBullet;
    public static Random Random = new Random();
    private int frameRate;
    private int frameCounter;
    private TimeSpan elapsedTime = TimeSpan.Zero;
    private int score = 0;

    public MainGame()
    {
      graphics = new GraphicsDeviceManager(this)
      {
        PreferredBackBufferWidth = (int)WindowWidth,
        PreferredBackBufferHeight = (int)WindowHeight
      };
      Window.Position = new Point((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2) - (graphics.PreferredBackBufferWidth / 2), (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2) - (graphics.PreferredBackBufferHeight / 2));
      Content.RootDirectory = "Content";
      player = new Player(1.5f);
      enemyGrid = new EnemyGrid(10, 5);
    }

    private void Reset()
    {
      score = 0;
      playerBullet = default;
      enemyBullet = default;
      bullets.Clear();
      player.Reset();
      enemyGrid.Reset();
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
      spriteBatch = new SpriteBatch(GraphicsDevice);
      spriteFont = Content.Load<SpriteFont>("Main");
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
      if (!player.Disposing && playerBullet == default(Bullet) && Keyboard.GetState().IsKeyDown(Keys.Space))
      {
        playerBullet = new Bullet(player);
        bullets.Add(playerBullet);
      }

      if (enemyBullet == default(Bullet))
      {
        enemyBullet = enemyGrid.GetBullet();
        if (enemyBullet != null)
        {
          bullets.Add(enemyBullet);
        }
      }

      enemyGrid.Update(gameTime);

      player.Update(gameTime);
      foreach (var bullet in bullets)
      {
        bullet.Update(gameTime);
      }
      base.Update(gameTime);

      foreach (var bullet in bullets)
      {
        player.CheckCollision(bullet, this);
        enemyGrid.CheckCollision(bullet, this);
        if (bullet.Disposing) { removableBullets.Add(bullet); }
      }
      foreach (var bullet in removableBullets)
      {
        bullets.Remove(bullet);
      }
      removableBullets.Clear();

      if (playerBullet?.Disposing == true) { playerBullet = default; }

      if (enemyBullet?.Disposing == true) { enemyBullet = default; }

      elapsedTime += gameTime.ElapsedGameTime;
      if (elapsedTime > TimeSpan.FromSeconds(1))
      {
        elapsedTime -= TimeSpan.FromSeconds(1);
        frameRate = frameCounter;
        frameCounter = 0;
      }
      if (player.Disposing)
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
      frameCounter++;
      GraphicsDevice.Clear(Color.Black);

      foreach (var bullet in bullets) { bullet.Draw(spriteBatch); }
      player.Draw(spriteBatch);

      enemyGrid.Draw(spriteBatch);

      spriteBatch.Begin();
      spriteBatch.DrawString(spriteFont, $"FPS: {frameRate}", new Vector2(0, 0), Color.Green);
      spriteBatch.DrawString(spriteFont, $"Score: {score,4}", new Vector2(WindowWidth - 300, 0), Color.Green);
      spriteBatch.End();

      base.Draw(gameTime);
    }

    public void EnemyKilled()
    {
      score += 100;
    }
  }
}
