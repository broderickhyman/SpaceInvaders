using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
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
    private readonly GraphicsDeviceManager graphics;
    private readonly EnemyGrid enemyGrid;
    private readonly Player player;
    private readonly List<Bullet> bullets = new List<Bullet>();
    private readonly List<Bullet> removableBullets = new List<Bullet>();
    public static float WindowWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2;
    public static float WindowHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2;
    private Bullet playerBullet;

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
      playerBullet = new Bullet(player);
      bullets.Add(playerBullet);
      base.Initialize();
    }

    /// <summary>
    /// LoadContent will be called once per game and is the place to load
    /// all of your content.
    /// </summary>
    protected override void LoadContent()
    {
      spriteBatch = new SpriteBatch(GraphicsDevice);
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
      if (playerBullet == default(Bullet) && Keyboard.GetState().IsKeyDown(Keys.Space))
      {
        playerBullet = new Bullet(player);
        bullets.Add(playerBullet);
      }

      enemyGrid.Update(gameTime);

      player.Update(gameTime);
      foreach (var bullet in bullets)
      {
        bullet.Update(gameTime);
        if (bullet.Disposing) { removableBullets.Add(bullet); }
      }

      foreach (var bullet in removableBullets)
      {
        bullets.Remove(bullet);
      }
      removableBullets.Clear();

      if (playerBullet?.Disposing == true) { playerBullet = default(Bullet); }

      base.Update(gameTime);
    }

    /// <summary>
    /// This is called when the game should draw itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Draw(GameTime gameTime)
    {
      GraphicsDevice.Clear(Color.Black);

      foreach (var bullet in bullets) { bullet.Draw(spriteBatch); }
      player.Draw(spriteBatch);

      enemyGrid.Draw(spriteBatch);
      //spriteBatch.Begin();
      //spriteBatch.DrawLine(WindowWidth / 2, 0, WindowWidth / 2, WindowHeight, Color.Red);
      //spriteBatch.End();

      base.Draw(gameTime);
    }
  }
}
