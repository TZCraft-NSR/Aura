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
    /// This is a game component that implements IUpdateable.
    /// </summary>
	public class MenuManager : Microsoft.Xna.Framework.DrawableGameComponent
	{
        SpriteBatch spriteBatch;
        Texture2D background;
        Texture2D buttonTexture;
        Button playButton;
        Button exitButton;
		Game1 myGame;
        List<Sprite.AnimationSet> animationSpriteList;

		public MenuManager(Game1 game) : base(game)
		{
			myGame = game;
		}

        public override void Initialize()
        {
            animationSpriteList = new List<Sprite.AnimationSet>();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            background = Game.Content.Load<Texture2D>(@"Images\screens\menu");
            buttonTexture = Game.Content.Load<Texture2D>(@"Images\gui\button");

            animationSpriteList.Add(new Sprite.AnimationSet("REG", buttonTexture, new Point(170, 46), new Point(0, 0), new Point(0, 0), 0));
            animationSpriteList.Add(new Sprite.AnimationSet("HOVER", buttonTexture, new Point(170, 46), new Point(1, 0), new Point(170, 0), 0));
            animationSpriteList.Add(new Sprite.AnimationSet("PRESSED", buttonTexture, new Point(170, 46), new Point(2, 0), new Point(340, 0), 0));

            exitButton = new Button(buttonTexture, new Vector2((myGame.windowSize.X) / 2, 366), myGame.segoeUI, 1f, Color.Black, "Exit", animationSpriteList);
            playButton = new Button(buttonTexture, new Vector2((myGame.windowSize.X) / 2, 190), myGame.segoeUI, 1f, Color.Black, "Play", animationSpriteList);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            exitButton.Update(gameTime);
            playButton.Update(gameTime);

            if (exitButton.Clicked())
            {
                myGame.Exit();
            }
            if (playButton.Clicked())
            {
                myGame.setCurrentLevel(Game1.GameLevels.GAME);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(background, Vector2.Zero, Color.White);
            exitButton.Draw(gameTime, spriteBatch);
            playButton.Draw(gameTime, spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
	}
}