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
		Label debugLabel1;
		Label debugLabel2;
		Label debugLabel3;
		Label debugLabel4;
		public List<Collision.MapSegment> mapSegments = new List<Collision.MapSegment>();
		List<Collision.MapSegment> playerSegments = new List<Collision.MapSegment>();
		string firstLine1;
		string firstLine2;
		string firstLine3;
		string firstLine4;
		string firstLine5;
		string firstLine6;
		string firstLine7;
		string firstLine8;
		string firstLine9;

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
			mapSegments.Add(new Collision.MapSegment(new Point(0, 0), new Point(0, ((int)(myGame.windowSize.Y + 50) / 2))));
			mapSegments.Add(new Collision.MapSegment(new Point(0, 0), new Point((int)myGame.windowSize.X - 1, 0)));
			mapSegments.Add(new Collision.MapSegment(new Point((int)myGame.windowSize.X - 1, 0), new Point((int)myGame.windowSize.X - 1, ((int)(myGame.windowSize.Y + 50) / 2))));
			mapSegments.Add(new Collision.MapSegment(new Point(0, ((int)(myGame.windowSize.Y + 50) / 2)), new Point((int)myGame.windowSize.X - 1, ((int)(myGame.windowSize.Y + 50) / 2))));

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

            player = new Player(playerTexture, new Vector2((myGame.windowSize.X - 60) / 2, (myGame.windowSize.Y - 50) / 2), myGame, Keys.A, Keys.D, Keys.Space, 2f, Color.White, tempPlayerAnimationSetList);

			debugLabel = new Label(Vector2.Zero, myGame.debugFont, 1f, Color.Black, "");
			debugLabel1 = new Label(new Vector2(0, 10), myGame.debugFont, 1f, Color.Black, "");
			debugLabel2 = new Label(new Vector2(0, 20), myGame.debugFont, 1f, Color.Black, "");
			debugLabel3 = new Label(new Vector2(0, 30), myGame.debugFont, 1f, Color.Black, "");
			debugLabel4 = new Label(new Vector2(0, 40), myGame.debugFont, 1f, Color.Black, "");

            base.LoadContent();
        }

		/// <summary>
		/// This is to update the Game Manager's Classes and sprites.
		/// </summary>
		/// <param name="gameTime">This is to check the run time.</param>
        public override void Update(GameTime gameTime)
		{
			playerSegments = player.getPlayerSgements();

            player.Update(gameTime);

			firstLine1 = "isGrounded=" + player.isGrounded + " isJumping=" + player.isJumping + " isFalling=" + player.isFalling + " bleedOff=" + player.BleedOff + " position=(" + (int)player.position.X + "," + (int)player.position.Y + ")";
			firstLine2 = "playerSegement" + 1 + "=(" + (int)playerSegments[0].point1.X + "," + (int)playerSegments[0].point1.Y + "," + (int)playerSegments[0].point2.X + "," + (int)playerSegments[0].point2.Y + ")";
			firstLine3 = "playerSegement" + 2 + "=(" + (int)playerSegments[1].point1.X + "," + (int)playerSegments[1].point1.Y + "," + (int)playerSegments[1].point2.X + "," + (int)playerSegments[1].point2.Y + ")";
			firstLine4 = "playerSegement" + 3 + "=(" + (int)playerSegments[2].point1.X + "," + (int)playerSegments[2].point1.Y + "," + (int)playerSegments[2].point2.X + "," + (int)playerSegments[2].point2.Y + ")";
			firstLine5 = "playerSegement" + 4 + "=(" + (int)playerSegments[3].point1.X + "," + (int)playerSegments[3].point1.Y + "," + (int)playerSegments[3].point2.X + "," + (int)playerSegments[3].point2.Y + ")";
			firstLine6 = "mapSegement" + 1 + "=(" + (int)mapSegments[0].point1.X + "," + (int)mapSegments[0].point1.Y + "," + (int)mapSegments[0].point2.X + "," + (int)mapSegments[0].point2.Y + ")";
			firstLine7 = "mapSegement" + 2 + "=(" + (int)mapSegments[1].point1.X + "," + (int)mapSegments[1].point1.Y + "," + (int)mapSegments[1].point2.X + "," + (int)mapSegments[1].point2.Y + ")";
			firstLine8 = "mapSegement" + 3 + "=(" + (int)mapSegments[2].point1.X + "," + (int)mapSegments[2].point1.Y + "," + (int)mapSegments[2].point2.X + "," + (int)mapSegments[2].point2.Y + ")";
			firstLine9 = "mapSegement" + 4 + "=(" + (int)mapSegments[3].point1.X + "," + (int)mapSegments[3].point1.Y + "," + (int)mapSegments[3].point2.X + "," + (int)mapSegments[3].point2.Y + ")";

			debugLabel.Update(gameTime, firstLine1);
			debugLabel1.Update(gameTime, firstLine2 + "   " + firstLine3);
			debugLabel2.Update(gameTime, firstLine4 + "   " + firstLine5);
			debugLabel3.Update(gameTime, firstLine6 + "   " + firstLine7);
			debugLabel4.Update(gameTime, firstLine8 + "   " + firstLine9);

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
				// Debug bounding map boxes.
				spriteBatch.Draw(debugDotTexture, new Rectangle(mapSegments[0].point1.X, mapSegments[0].point1.Y, mapSegments[0].point2.X + 1, mapSegments[0].point2.Y), Color.Blue);
				spriteBatch.Draw(debugDotTexture, new Rectangle(mapSegments[1].point1.X, mapSegments[1].point1.Y, mapSegments[1].point2.X + 1, 1), Color.Green);
				spriteBatch.Draw(debugDotTexture, new Rectangle(mapSegments[2].point1.X, mapSegments[2].point1.Y, mapSegments[2].point2.X + 1, mapSegments[2].point2.Y + 1), Color.Red);
				spriteBatch.Draw(debugDotTexture, new Rectangle(mapSegments[3].point1.X, mapSegments[3].point1.Y, mapSegments[3].point2.X + 1, 1), Color.Gray);

				// Debug bounding player boxes.
				spriteBatch.Draw(debugDotTexture, new Rectangle(playerSegments[0].point1.X, playerSegments[0].point1.Y, 1, 44), Color.Blue);
				spriteBatch.Draw(debugDotTexture, new Rectangle(playerSegments[1].point1.X, playerSegments[1].point1.Y, 20, 1), Color.Blue);
				spriteBatch.Draw(debugDotTexture, new Rectangle(playerSegments[2].point1.X, playerSegments[2].point1.Y, 1, 44), Color.Blue);
				spriteBatch.Draw(debugDotTexture, new Rectangle(playerSegments[3].point1.X, playerSegments[3].point1.Y, 20, 1), Color.Blue);

				// To debug variables.
				debugLabel.Draw(gameTime, spriteBatch);
				debugLabel1.Draw(gameTime, spriteBatch);
				debugLabel2.Draw(gameTime, spriteBatch);
				debugLabel3.Draw(gameTime, spriteBatch);
				debugLabel4.Draw(gameTime, spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
	}
}