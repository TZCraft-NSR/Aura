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
	public class GameManager : Microsoft.Xna.Framework.DrawableGameComponent
	{
		/// <summary>
		/// The spriteBatch to draw with.
		/// </summary>
        SpriteBatch spriteBatch;
		/// <summary>
		/// The game that the GameManager runs off of.
		/// </summary>
		Game1 myGame;

		/// <summary>
		/// The animation list for the player.
		/// </summary>
        List<Sprite.AnimationSet> tempPlayerAnimationSetList = new List<Sprite.AnimationSet>();

		/// <summary>
		/// The player class.
		/// </summary>
        Player player;
		/// <summary>
		/// The player class' texture.
		/// </summary>
        Texture2D playerTexture;
		/// <summary>
		/// This is the dot texture to test bounding boxes.
		/// </summary>
		Texture2D debugDotTexture;
		/// <summary>
		/// To debug variables.
		/// </summary>
		Label debugLabel;

		/// <summary>
		/// This is to create the Game Manager.
		/// </summary>
		/// <param name="game">This is for the Game stuff</param>
        public GameManager(Game1 game) : base(game)
		{
			myGame = game;

			// This is to fix the Initalize() function.
			this.Initialize();
		}

		/// <summary>
		/// This is to initalize the Game Manager
		/// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }

		/// <summary>
		/// This is to load the Game Manager's sprites and classes.
		/// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            playerTexture = Game.Content.Load<Texture2D>(@"Images\players\player");
            debugDotTexture = Game.Content.Load<Texture2D>(@"Images\debugStuff\line");

            tempPlayerAnimationSetList.Add(new Sprite.AnimationSet("IDLE", playerTexture, new Point(60, 50), new Point(1, 1), new Point(0, 0), 1000));
            tempPlayerAnimationSetList.Add(new Sprite.AnimationSet("WALK", playerTexture, new Point(60, 50), new Point(4, 3), new Point(0, 0), 50));
            tempPlayerAnimationSetList.Add(new Sprite.AnimationSet("JUMP", playerTexture, new Point(60, 50), new Point(4, 1), new Point(0, 150), 100));

            player = new Player(playerTexture, new Vector2((myGame.windowSize.X - 60) / 2, (myGame.windowSize.Y - 50) / 2), myGame, Keys.A, Keys.D, Keys.Space, 2f, tempPlayerAnimationSetList);

			debugLabel = new Label(Vector2.Zero, myGame.debugFont, 1f, Color.Black, "");

            base.LoadContent();
        }

		/// <summary>
		/// This is to update the Game Manager's Classes and sprites.
		/// </summary>
		/// <param name="gameTime">This is to check the run time.</param>
        public override void Update(GameTime gameTime)
        {
            player.Update(gameTime);
			debugLabel.Update(gameTime, "isGrounded=" + player.isGrounded + " isJumping=" + player.isJumping + " isFalling=" + player.isFalling + " isWalking=" + player.isWalking + " bleedOff=" + player.BleedOff);

            base.Update(gameTime);
        }

		/// <summary>
		/// This is to draw the Game Manager's Classes and Objects
		/// </summary>
		/// <param name="gameTime"><This is to check the run time./param>
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
                player.Draw(gameTime, spriteBatch);
				// Debug bounding boxes.
                spriteBatch.Draw(debugDotTexture, new Rectangle(0, ((int)(myGame.windowSize.Y - 50) / 2) + player.currentAnimation.frameSize.Y, (int)myGame.windowSize.X, 1), Color.White);
				// Debug bounding boxes.
                spriteBatch.Draw(debugDotTexture, new Rectangle((int)player.position.X - 10, ((int)player.position.Y - 1) + player.currentAnimation.frameSize.Y, player.currentAnimation.frameSize.X + 10, 1), Color.White);
				// To debug variables.
				debugLabel.Draw(gameTime, spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
	}
}