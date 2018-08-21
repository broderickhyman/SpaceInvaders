using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace SpaceInvaders
{
  /// <summary>
  /// This is the main type for your game.
  /// </summary>
  public class MainGame : Game
  {
    private readonly GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;
    private Player player;
    const float windowWidth = 800;
    const float windowHeight = 800;

    public MainGame()
    {
      graphics = new GraphicsDeviceManager(this)
      {
        PreferredBackBufferWidth = (int)windowWidth,
        PreferredBackBufferHeight = (int)windowHeight
      };
      Content.RootDirectory = "Content";
      const float playerWidth = windowWidth / 10;
      const float playerHeight = windowHeight / 20;
      player = new Player(new RectangleF((windowWidth / 2) - (playerWidth / 2), windowHeight - (playerHeight / 2) - (windowHeight / 10), windowWidth / 10, windowHeight / 20))
      {
        Speed = 10
      };
    }

    /// <summary>
    /// Allows the game to perform any initialization it needs to before starting to run.
    /// This is where it can query for any required services and load any non-graphic
    /// related content.  Calling base.Initialize will enumerate through any components
    /// and initialize them as well.
    /// </summary>
    protected override void Initialize()
    {
      player.Initialize();
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
        Exit();

      player.Update();

      base.Update(gameTime);
    }

    /// <summary>
    /// This is called when the game should draw itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Draw(GameTime gameTime)
    {
      GraphicsDevice.Clear(Color.Black);

      player.Draw(spriteBatch);
      spriteBatch.Begin();
      spriteBatch.DrawLine(windowWidth / 2, 0, windowWidth / 2, windowHeight, Color.Red);
      spriteBatch.End();

      base.Draw(gameTime);
    }
  }
}
