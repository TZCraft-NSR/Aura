using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using VoidEngine;

namespace WrathOfJohn
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Game1 : Microsoft.Xna.Framework.Game
	{
		public enum GameLevels
		{
			SPLASH,
			MENU,
			OPTIONS,
			GAME,
			BOSS
		}
		public enum Ratio
		{
			WIDESCREEN,
			NORMAL
		}

		public GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		public KeyboardState keyboardState, previousKeyboardState;

		#region Screen Properties
		Vector2 windowSize;
		public Vector2 WindowSize
		{
			get
			{
				return windowSize;
			}
		}

		Vector2 resolution = new Vector2(800, 450);
		public Vector2 Resolution
		{
			get
			{
				return resolution;
			}
			set
			{
				Vector2 tempResolution = value;
				resolution = tempResolution;
			}
		}

		Ratio ratio = Ratio.WIDESCREEN;
		public Ratio SetRatio
		{
			get
			{
				return ratio;
			}
			set
			{
				ratio = value;
			}
		}

		bool fullscreen = false;
		public bool Fullscreen
		{
			get
			{
				return fullscreen;
			}
			set
			{
				fullscreen = value;
			}
		}
		#endregion

		#region Game Levels
		public SplashScreenManager splashScreenManager;
		public GameManager gameManager;
		public MenuManager menuManager;
		public OptionsManager optionsManager;
		public GameLevels currentGameLevel;
		#endregion

		public float elapsedTime, previousElapsedTime;

		#region Fonts
		public SpriteFont ubuntuMono;
		public SpriteFont ubuntuRegular;
		public SpriteFont ubuntuBold;
		public SpriteFont ubuntuItalic;
		#endregion

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Window.Title = "Wrath Of John: We are here!!!";
			Content.RootDirectory = "Content";
			graphics.PreferredBackBufferWidth = (int)resolution.X;
			graphics.PreferredBackBufferHeight = (int)resolution.Y;
			windowSize = new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
			graphics.IsFullScreen = fullscreen;
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			IsMouseVisible = true;

			// TODO: Add your initialization logic here

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);

			ubuntuMono = Content.Load<SpriteFont>(@"Fonts\Ubuntu Mono");
			ubuntuRegular = Content.Load<SpriteFont>(@"Fonts\Ubuntu Regular");
			ubuntuBold = Content.Load<SpriteFont>(@"Fonts\Ubuntu Bold");
			ubuntuItalic = Content.Load<SpriteFont>(@"Fonts\Ubuntu Italic");

			splashScreenManager = new SplashScreenManager(this);
			menuManager = new MenuManager(this);
			optionsManager = new OptionsManager(this);
			gameManager = new GameManager(this);

			Components.Add(splashScreenManager);
			Components.Add(menuManager);
			Components.Add(optionsManager);
			Components.Add(gameManager);

			splashScreenManager.Enabled = true;
			splashScreenManager.Visible = true;
			menuManager.Enabled = false;
			menuManager.Visible = false;
			optionsManager.Enabled = false;
			optionsManager.Visible = false;
			gameManager.Enabled = false;
			gameManager.Visible = false;

			currentGameLevel = GameLevels.SPLASH;

			// TODO: use this.Content to load your game content here
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			// Allows the game to exit
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
			{
				this.Exit();
			}

			windowSize = new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

			keyboardState = Keyboard.GetState();

			elapsedTime = gameTime.ElapsedGameTime.Milliseconds;

			// TODO: Add your update logic here

			base.Update(gameTime);

			previousElapsedTime = elapsedTime;
			previousKeyboardState = keyboardState;
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.White);

			// TODO: Add your drawing code here

			base.Draw(gameTime);
		}

		/// <summary>
		/// This will check if the key specified is released.
		/// </summary>
		/// <param name="key">The key to check</param>
		public bool CheckKey(Keys key)
		{
			if (keyboardState.IsKeyUp(key) && previousKeyboardState.IsKeyDown(key))
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// This sets the current scene or level that the game is at.
		/// </summary>
		/// <param name="level">The game level to change to.</param>
		public void SetCurrentLevel(GameLevels level)
		{
			if (currentGameLevel != level)
			{
				currentGameLevel = level;
				splashScreenManager.Enabled = false;
				splashScreenManager.Visible = false;
				menuManager.Enabled = false;
				menuManager.Visible = false;
				gameManager.Enabled = false;
				gameManager.Visible = false;
				optionsManager.Enabled = false;
				optionsManager.Visible = false;
			}

			switch (currentGameLevel)
			{
				case GameLevels.SPLASH:
					splashScreenManager.Enabled = true;
					splashScreenManager.Visible = true;
					break;
				case GameLevels.MENU:
					menuManager.Enabled = true;
					menuManager.Visible = true;
					break;
				case GameLevels.OPTIONS:
					optionsManager.Enabled = true;
					optionsManager.Visible = true;
					break;
				case GameLevels.GAME:
					gameManager.Enabled = true;
					gameManager.Visible = true;
					break;
				default:
					break;
			}
		}
	}
}
