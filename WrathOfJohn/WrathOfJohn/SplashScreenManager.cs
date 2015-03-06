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
	public class SplashScreenManager : Microsoft.Xna.Framework.DrawableGameComponent
	{
		/// <summary>
		/// Used for reseting the timer -not really needed-
		/// </summary>
		int countdownTimerDefault = 5000;
		/// <summary>
		/// The countdown timer for between splash screen frames
		/// </summary>
		int countdownTimer = 5000;
		/// <summary>
		/// The Animeme splash alpha value
		/// </summary>
		int alphaValue1 = 0;
		/// <summary>
		/// The Void Inc splash alpha value
		/// </summary>
		int alphaValue2 = 255;
		/// <summary>
		/// The ammount of integers the alpha value's will go up or down.
		/// </summary>
		int fadeIncrement = 3;
		/// <summary>
		/// The splash screen frame #1
		/// </summary>
		Texture2D background1;
		/// <summary>
		/// The splash screen frame #2
		/// </summary>
		Texture2D background2;
		/// <summary>
		/// The spriteBatch for drawing
		/// </summary>
		SpriteBatch spriteBatch;
		/// <summary>
		/// The current splash screen frame
		/// </summary>
		int splashScreenPart = 1;
		/// <summary>
		/// The game class
		/// </summary>
		Game1 myGame;
		/// <summary>
		/// To debug the splash screen
		/// </summary>
		Label debugLabel;

		/// <summary>
		/// Creates the Splash Screen.
		/// </summary>
		/// <param name="game">The game that this splash screen runs off of.</param>
		public SplashScreenManager(Game1 game)
			: base(game)
		{
			myGame = game;
			this.Initialize();
		}

		/// <summary>
		/// Initializes the Splash Screen Manager
		/// </summary>
		public override void Initialize()
		{
			base.Initialize();
		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(Game.GraphicsDevice);
			background1 = Game.Content.Load<Texture2D>(@"images\screens\splash1");
			background2 = Game.Content.Load<Texture2D>(@"images\screens\splash2");

			debugLabel = new Label(Vector2.Zero, myGame.segoeUIMonoDebug, 1f, Color.Black, "");

			base.LoadContent();
		}

		public override void Update(GameTime gameTime)
		{
			countdownTimer -= gameTime.ElapsedGameTime.Milliseconds;

			if (countdownTimer <= 0 && splashScreenPart == 1)
			{
				splashScreenPart = 2;
			}
			if (splashScreenPart == 2)
			{
				alphaValue1 += fadeIncrement;
				alphaValue2 -= fadeIncrement;

				if (alphaValue1 >= 255 && alphaValue2 <= 0)
				{
					countdownTimer = countdownTimerDefault;
					splashScreenPart = 3;
				}
			}
			if (countdownTimer <= 0 && splashScreenPart == 3)
			{
				myGame.SetCurrentLevel(Game1.GameLevels.MENU);
			}
			if (myGame.CheckKey(Keys.Enter))
			{
				myGame.SetCurrentLevel(Game1.GameLevels.MENU);
			}
			debugLabel.Update(gameTime, "countdownTimer=" + countdownTimer.ToString() + " alphaValue1=" + alphaValue1.ToString() + " alphaValue2=" + alphaValue2.ToString());

			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			int alphaValue3 = (int)MathHelper.Clamp(alphaValue1, 0, 255);
			int alphaValue4 = (int)MathHelper.Clamp(alphaValue2, 0, 255);

			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointWrap, null, null);
			{
				spriteBatch.Draw(background1, new Rectangle(((int)myGame.WindowSize.X - background1.Width) / 2, ((int)myGame.WindowSize.Y - background1.Height) / 2, background1.Width, background1.Height), new Color(255, 255, 255, alphaValue3));
				spriteBatch.Draw(background2, new Rectangle(((int)myGame.WindowSize.X - background2.Width) / 2, ((int)myGame.WindowSize.Y - background2.Height) / 2, background2.Width, background2.Height), new Color(255, 255, 255, alphaValue4));
			}
			spriteBatch.End();

			spriteBatch.Begin();
			{
				debugLabel.Draw(gameTime, spriteBatch);
			}
			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}