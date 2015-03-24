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

namespace Aura
{
	/// <summary>
	/// This is a game component that implements IUpdateable.
	/// </summary>
	public class MenuManager : Microsoft.Xna.Framework.DrawableGameComponent
	{
		SpriteBatch spriteBatch;
		Game1 myGame;

		Texture2D background;
		Texture2D buttonTexture;
		Song titleSong;

		Button playButton;
		Button exitButton;
		Button optionsButton;

		List<Sprite.AnimationSet> animationSpriteList;

		public bool title
		{
			get;
			set;
		}

		public MenuManager(Game1 game)
			: base(game)
		{
			myGame = game;

			this.Initialize();
		}

		public override void Initialize()
		{
			animationSpriteList = new List<Sprite.AnimationSet>();

			base.Initialize();
		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(Game.GraphicsDevice);

			background = Game.Content.Load<Texture2D>(@"images\screens\menu");
			buttonTexture = Game.Content.Load<Texture2D>(@"images\gui\button");
			titleSong = Game.Content.Load<Song>(@"sounds\music\title");


			animationSpriteList.Add(new Sprite.AnimationSet("IDLE", buttonTexture, new Point(170, 46), new Point(0, 0), new Point(0, 0), 0));
			animationSpriteList.Add(new Sprite.AnimationSet("HOVER", buttonTexture, new Point(170, 46), new Point(1, 0), new Point(170, 0), 0));
			animationSpriteList.Add(new Sprite.AnimationSet("PRESSED", buttonTexture, new Point(170, 46), new Point(2, 0), new Point(340, 0), 0));

			exitButton = new Button(new Vector2((myGame.WindowSize.X - 170) / 2, 366), myGame.segoeUIRegular, 1f, Color.Black, "EXIT", Color.White, animationSpriteList);
			playButton = new Button(new Vector2(100, 190), myGame.segoeUIRegular, 1f, Color.Black, "PLAY", Color.White, animationSpriteList);
			optionsButton = new Button(new Vector2((myGame.WindowSize.X - 170) - 100, 190), myGame.segoeUIRegular, 1f, Color.Black, "OPTIONS", Color.White, animationSpriteList);

			base.LoadContent();
		}

		public override void Update(GameTime gameTime)
		{
			exitButton.Update(gameTime);
			playButton.Update(gameTime);
			optionsButton.Update(gameTime);

			if (!title && myGame.currentGameLevel == Game1.GameLevels.MENU)
			{
				MediaPlayer.Stop();
				MediaPlayer.Play(titleSong);
				title = true;
			}

			if (exitButton.Clicked())
			{
				MediaPlayer.Stop();
				myGame.Exit();
			}
			if (playButton.Clicked())
			{
				myGame.SetCurrentLevel(Game1.GameLevels.GAME);
			}
			if (optionsButton.Clicked())
			{
				myGame.SetCurrentLevel(Game1.GameLevels.OPTIONS);
			}

			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(new Color(25, 25, 25));
			spriteBatch.Begin(SpriteSortMode.BackToFront, null, SamplerState.PointWrap, null, null);
			{
				spriteBatch.Draw(background, new Rectangle((int)(background.Width - myGame.WindowSize.X) / 4, 0, (int)(((myGame.WindowSize.Y / myGame.WindowSize.X) / 1.25) * background.Width), (int)myGame.WindowSize.Y), Color.White);
			}
			spriteBatch.End();

			spriteBatch.Begin();
			{
				exitButton.Draw(gameTime, spriteBatch);
				playButton.Draw(gameTime, spriteBatch);
				optionsButton.Draw(gameTime, spriteBatch);
			}
			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}