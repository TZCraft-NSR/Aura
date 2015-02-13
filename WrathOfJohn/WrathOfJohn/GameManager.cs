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
		/// The games camera
		/// </summary>
		Camera camera;

		#region Player Variables
		/// <summary>
		/// The player class.
		/// </summary>
		Player player;
		/// <summary>
		/// The animation list for the player.
		/// </summary>
		List<Sprite.AnimationSet> playerAnimationSetList;
		/// <summary>
		/// The player class' texture.
		/// </summary>
		Texture2D playerTexture;
		/// <summary>
		/// The players collision segements.
		/// </summary>
		List<Collision.MapSegment> playerSegments;
		/// <summary>
		/// The players projectile texture
		/// </summary>
		public Texture2D projectileTexture;
		/// <summary>
		/// The players list of movement keys.
		/// </summary>
		List<Keys> MovementKeys;
		/// <summary>
		/// The players mana class.
		/// </summary>
		Player.Mana _Mana;
		#endregion

		#region Debug Variables
		/// <summary>
		/// This is the dot texture to test bounding boxes.
		/// </summary>
		Texture2D debugDotTexture;
		/// <summary>
		/// To debug variables.
		/// </summary>
		Label debugLabel0;

		#region Debug Strings
		/// <summary>
		/// The first debug string.
		/// </summary>
		string firstLine1;
		/// <summary>
		/// The second debug string.
		/// </summary>
		string firstLine2;
		/// <summary>
		/// The third debug string.
		/// </summary>
		string firstLine3;
		/// <summary>
		/// The forth debug string.
		/// </summary>
		string firstLine4;
		/// <summary>
		/// The fifth debug string.
		/// </summary>
		string firstLine5;
		/// <summary>
		/// The sixth debug string.
		/// </summary>
		string firstLine6;
		/// <summary>
		/// The seventh debug string.
		/// </summary>
		string firstLine7;
		/// <summary>
		/// The eighth debug string.
		/// </summary>
		string firstLine8;
		/// <summary>
		/// The ninth debug string.
		/// </summary>
		string firstLine9;
		/// <summary>
		/// The tenth debug string.
		/// </summary>
		public string firstLine10;
		#endregion
		#endregion

		#region Map Variables
		/// <summary>
		/// The maps collision segements
		/// </summary>
		public List<Collision.MapSegment> mapSegments;
		/// <summary>
		/// The list of blocks.
		/// </summary>
		List<PlatformManager> platformList;
		/// <summary>
		/// The texture sheet of blocks.
		/// </summary>
		Texture2D platformTexture;
		/// <summary>
		/// The animation set list of the blocks.
		/// </summary>
		List<Sprite.AnimationSet> platformAnimationSetList;
		#endregion

		#region Background Variables
		/// <summary>
		/// The first parallax texture.
		/// </summary>
		Texture2D parallax1;
		/// <summary>
		/// The second parallax texture.
		/// </summary>
		Texture2D parallax2;
		/// <summary>
		/// The third parallax texture.
		/// </summary>
		Texture2D parallax3;
		/// <summary>
		/// The forth parallax texture.
		/// </summary>
		Texture2D parallax4;
		/// <summary>
		/// The first parallax background class.
		/// </summary>
		ParallaxBackground parallax1Background;
		/// <summary>
		/// The second parallax background class.
		/// </summary>
		ParallaxBackground parallax2Background;
		/// <summary>
		/// The third parallax background class.
		/// </summary>
		ParallaxBackground parallax3Background;
		/// <summary>
		/// The forth parallax background class.
		/// </summary>
		ParallaxBackground parallax4Background;
		#endregion

		/// <summary>
		/// This is to create the Game Manager.
		/// </summary>
		/// <param name="game">This is for the Game stuff</param>
		public GameManager(Game1 game)
			: base(game)
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
			playerAnimationSetList = new List<Sprite.AnimationSet>();
			platformAnimationSetList = new List<Sprite.AnimationSet>();
			mapSegments = new List<Collision.MapSegment>();
			playerSegments = new List<Collision.MapSegment>();
			platformList = new List<PlatformManager>();
			MovementKeys = new List<Keys>();

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
			projectileTexture = Game.Content.Load<Texture2D>(@"Images\projectiles\beam1");
			parallax1 = Game.Content.Load<Texture2D>(@"Images\screens\game\paralax\parallax1");
			parallax2 = Game.Content.Load<Texture2D>(@"Images\screens\game\paralax\parallax2");
			parallax3 = Game.Content.Load<Texture2D>(@"Images\screens\game\paralax\parallax3");
			parallax4 = Game.Content.Load<Texture2D>(@"Images\screens\game\paralax\parallax4");
			platformTexture = Game.Content.Load<Texture2D>(@"Images\tiles\tile");

			camera = new Camera(GraphicsDevice.Viewport, (int)myGame.Resolution.X * 4, (int)myGame.Resolution.Y, 1f);
			camera.Position = new Vector2(0, 0);

			mapSegments.Add(new Collision.MapSegment(new Point(0, 0), new Point(0, (int)camera.Size.Y - 23)));
			mapSegments.Add(new Collision.MapSegment(new Point(0, 0), new Point((int)camera.Size.X - 1, 0)));
			mapSegments.Add(new Collision.MapSegment(new Point((int)camera.Size.X - 1, 0), new Point((int)camera.Size.X - 1, (int)camera.Size.Y - 23)));
			mapSegments.Add(new Collision.MapSegment(new Point(0, (int)camera.Size.Y - 23), new Point((int)camera.Size.X - 1, (int)camera.Size.Y - 23)));

			parallax1Background = new ParallaxBackground(parallax1, new Vector2(camera.OverallPlayerPosition.X - (myGame.WindowSize.X / 2), 0), Color.White, 1.000f, camera);
			parallax2Background = new ParallaxBackground(parallax2, new Vector2(camera.OverallPlayerPosition.X - (myGame.WindowSize.X / 2), 28), Color.White, 1.125f, camera);
			parallax3Background = new ParallaxBackground(parallax3, new Vector2(camera.OverallPlayerPosition.X - (myGame.WindowSize.X / 2), 302), Color.White, 1.250f, camera);
			parallax4Background = new ParallaxBackground(parallax4, new Vector2(camera.OverallPlayerPosition.X - (myGame.WindowSize.X / 2), 272), Color.White, 1.500f, camera);
			
			_Mana = new Player.Mana(100, 5000, 100);

			playerAnimationSetList.Add(new Sprite.AnimationSet("IDLE", playerTexture, new Point(60, 50), new Point(1, 1), new Point(0, 0), 1000));
			playerAnimationSetList.Add(new Sprite.AnimationSet("WALK", playerTexture, new Point(60, 50), new Point(4, 3), new Point(0, 0), 50));
			playerAnimationSetList.Add(new Sprite.AnimationSet("JUMP", playerTexture, new Point(60, 50), new Point(4, 1), new Point(0, 150), 100));
			playerAnimationSetList.Add(new Sprite.AnimationSet("SHOOT", playerTexture, new Point(60, 50), new Point(1, 3), new Point(240, 0), 250));

			platformAnimationSetList.Add(new Sprite.AnimationSet("IDLE1", platformTexture, new Point(25, 25), new Point(1, 1), new Point(0, 0), 0));
			platformAnimationSetList.Add(new Sprite.AnimationSet("IDLE2", platformTexture, new Point(25, 25), new Point(1, 1), new Point(25, 0), 0));


			MovementKeys.Add(Keys.A);
			MovementKeys.Add(Keys.W);
			MovementKeys.Add(Keys.D);
			MovementKeys.Add(Keys.S);
			MovementKeys.Add(Keys.Space);
			MovementKeys.Add(Keys.E);

			player = new Player(new Vector2((myGame.WindowSize.X - 60) / 2, myGame.WindowSize.Y - (playerAnimationSetList[0].frameSize.Y + 25)), MovementKeys, 3f, _Mana, Color.White, playerAnimationSetList, myGame);

			debugLabel0 = new Label(new Vector2(0, 00), myGame.ubuntuMono, 0.50f, Color.Black, "");

			base.LoadContent();
		}

		/// <summary>
		/// This is to update the Game Manager's Classes and sprites.
		/// </summary>
		/// <param name="gameTime">This is to check the run time.</param>
		public override void Update(GameTime gameTime)
		{
			if (camera.IsInView(player.GetPosition, new Vector2(20, 49)))
			{
				camera.Position = new Vector2(player.GetPosition.X + 200f, 0);
			}

			camera.OverallPlayerPosition = new Vector2(player.GetPosition.X + 200f, 0);

			parallax1Background.position = new Vector2(camera.Position.X - (myGame.WindowSize.X / 2), 0);
			parallax1Background.Update(gameTime);
			parallax2Background.position = new Vector2(camera.Position.X - (myGame.WindowSize.X / 2), 28);
			parallax2Background.Update(gameTime);
			parallax3Background.position = new Vector2(camera.Position.X - (myGame.WindowSize.X / 2), 302);
			parallax3Background.Update(gameTime);
			parallax4Background.position = new Vector2(camera.Position.X - (myGame.WindowSize.X / 2), 272);
			parallax4Background.Update(gameTime);

			playerSegments = player.GetPlayerSgements();

			player.Update(gameTime);

			firstLine1 = "";
			firstLine2 = "";
			firstLine3 = "";
			firstLine4 = "";
			firstLine5 = "";
			firstLine6 = "";
			firstLine7 = "";
			firstLine8 = "";
			firstLine9 = "";
			firstLine10 = "";

			debugLabel0.Update(gameTime, firstLine1 + "   " + firstLine2 + "\n" + firstLine3 + "   " + firstLine4 + "\n" + firstLine5 + "   " + firstLine6 + "\n" + firstLine7 + "   " + firstLine8 + "\n" + firstLine9 + "   " + firstLine10);

			base.Update(gameTime);
		}

		/// <summary>
		/// This is to draw the Game Manager's Classes and Objects
		/// </summary>
		/// <param name="gameTime"><This is to check the run time./param>
		public override void Draw(GameTime gameTime)
		{
			spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap, null, null, null, camera.GetTransformation());
			{
				parallax1Background.Draw(gameTime, spriteBatch);
				parallax2Background.Draw(gameTime, spriteBatch);
				parallax3Background.Draw(gameTime, spriteBatch);
				parallax4Background.Draw(gameTime, spriteBatch);

				SpawnBricks();

				foreach (PlatformManager pm in platformList)
				{
					pm.Draw(gameTime, spriteBatch);
				}

				// Draw the player.
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
			}
			spriteBatch.End();

			spriteBatch.Begin();
			{
				// To debug variables.
				debugLabel0.Draw(gameTime, spriteBatch);
			}
			spriteBatch.End();

			base.Draw(gameTime);
		}

		public void SpawnBricks()
		{
			int width = 128;
			int height = 18;
			uint[,] brickspawn;

			brickspawn = Maps.GetBrickArray(Maps.HappyFace());

			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					if (brickspawn[x, y] > 0)
					{
						platformList.Add(new PlatformManager(new Vector2(x * 25, y * 25), myGame, (int)brickspawn[x, y], platformAnimationSetList));
					}
				}
			}
		}
	}
}