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
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Vector2 windowSize;

        public bool debugCheckpointManager = false;
        public bool debugSaveFileManager = false;

        public enum GameLevels { SPLASH, MENU, GAME }

        public SplashScreenManager splashScreenManager;
        public GameManager gameManager;
        public MenuManager menuManager;
        public CheckpointManager checkpointManager;
        public SaveFileManager saveFileManager;
        public GameLevels currentGameLevel;

        public SpriteFont debugFont;
        public SpriteFont segoeUI;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 500;
            graphics.PreferredBackBufferWidth = 700;
            windowSize = new Vector2(graphics.PreferredBackBufferHeight, graphics.PreferredBackBufferHeight);
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

            debugFont = Content.Load<SpriteFont>(@"Fonts\debug\Segoe UI Mono");
            segoeUI = Content.Load<SpriteFont>(@"Fonts\Segoe UI");

            splashScreenManager = new SplashScreenManager(this);
            menuManager = new MenuManager(this);
            gameManager = new GameManager(this);
            checkpointManager = new CheckpointManager(this);
            saveFileManager = new SaveFileManager(this);

            Components.Add(splashScreenManager);
            Components.Add(menuManager);
            Components.Add(gameManager);
            Components.Add(checkpointManager);
            Components.Add(saveFileManager);

            splashScreenManager.Initialize();
            menuManager.Initialize();

            menuManager.Enabled = false;
            menuManager.Visible = false;
            gameManager.Enabled = false;
            gameManager.Visible = false;
            splashScreenManager.Enabled = true;
            splashScreenManager.Visible = true;
            checkpointManager.Enabled = true;
            checkpointManager.Visible = debugCheckpointManager;
            saveFileManager.Enabled = true;
            saveFileManager.Visible = debugSaveFileManager;

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

            // TODO: Add your update logic here

            base.Update(gameTime);
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

        public void setCurrentLevel(GameLevels level)
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
