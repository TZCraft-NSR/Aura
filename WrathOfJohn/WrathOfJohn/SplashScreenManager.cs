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
        int countdownTimerDefault = 5000; // Used for reseting the timer -not really needed-
        int countdownTimer = 5000; // The countdown timer for between splash screen frames
        int alphaValue1 = 0; // The Animeme splash alpha value
        int alphaValue2 = 255; // The Void Inc splash alpha value
        int fadeIncrement = 3; // The ammount of integers the alpha value's will go up or down.
        Texture2D background1;
        Texture2D background2;
        SpriteBatch spriteBatch;
        int splashScreenPart = 1;
		Game1 myGame;
        Label debugLabel;
        string debugText = "";

		public SplashScreenManager(Game1 game) : base(game)
		{
			myGame = game;
		}

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            background1 = Game.Content.Load<Texture2D>(@"Images\screens\splash1");
            background2 = Game.Content.Load<Texture2D>(@"Images\screens\splash2");

            debugLabel = new Label(Vector2.Zero, myGame.debugFont, 1f, Color.Black, debugText);

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
                myGame.setCurrentLevel(Game1.GameLevels.MENU);
            }

            debugText = "countdownTimer=" + countdownTimer.ToString() + " alphaValue1=" + alphaValue1.ToString() + " alphaValue2=" + alphaValue2.ToString();
            debugLabel.Update(gameTime, debugText);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            int alphaValue3 = (int)MathHelper.Clamp(alphaValue1, 0, 255);
            int alphaValue4 = (int)MathHelper.Clamp(alphaValue2, 0, 255);

            spriteBatch.Begin();
                spriteBatch.Draw(background1, Vector2.Zero, new Color(255, 255, 255, alphaValue3));
                spriteBatch.Draw(background2, Vector2.Zero, new Color(alphaValue4, alphaValue4, alphaValue4, alphaValue4));
                debugLabel.Draw(gameTime, spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
	}
}