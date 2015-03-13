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
		/// The player segment collisions.
		/// </summary>
		protected Rectangle PlayerCollisions;
		/// <summary>
		/// The players list of movement keys.
		/// </summary>
		List<Keys> MovementKeys;
		/// <summary>
		/// The players projectile texture
		/// </summary>
		public Texture2D ProjectileTexture;
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
		Label debugLabel;

		#region Debug Strings
		public string[] DebugLines = new string[10];
		#endregion
		#endregion

		#region Map Variables
		/// <summary>
		/// The list of blocks.
		/// </summary>
		public List<PlatformManager> platformList
		{
			get;
			set;
		}
		/// <summary>
		/// The texture sheet of blocks.
		/// </summary>
		Texture2D platformTexture;
		/// <summary>
		/// The animation set list of the blocks.
		/// </summary>
		List<Sprite.AnimationSet> platformAnimationSetList
		{
			get;
			set;
		}
		/// <summary>
		/// The collision areas of each platform.
		/// </summary>
		public List<Rectangle> platformRectangles
		{
			get;
			set;
		}
        /// <summary>
        /// The bounding boxes of the map.
        /// </summary>
		public List<Rectangle> mapSegments
		{
			get;
			set;
		}
		/// <summary>
		/// The current level id.
		/// </summary>
		public int level
		{
			get;
			protected set;
		}
		/// <summary>
		/// Gets or sets if the level has been set.
		/// </summary>
		public bool levelLoaded
		{
			get;
			protected set;
		}
		/// <summary>
		/// Gets or sets if the player won the current level.
		/// </summary>
		public bool wonLevel
		{
			get;
			protected set;
		}
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
			MovementKeys = new List<Keys>();
			playerAnimationSetList = new List<Sprite.AnimationSet>();

			platformList = new List<PlatformManager>();
			platformAnimationSetList = new List<Sprite.AnimationSet>();
			platformRectangles = new List<Rectangle>();

            mapSegments = new List<Rectangle>();

			level = 1;

			for (int i = 0; i < DebugLines.Length; i++)
			{
				DebugLines[i] = "";
			}

			base.Initialize();
		}

		/// <summary>
		/// This is to load the Game Manager's sprites and classes.
		/// </summary>
		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(Game.GraphicsDevice);

			playerTexture = Game.Content.Load<Texture2D>(@"images\players\player");
			debugDotTexture = Game.Content.Load<Texture2D>(@"images\debug\line");
			ProjectileTexture = Game.Content.Load<Texture2D>(@"images\projectiles\beam");
			parallax1 = Game.Content.Load<Texture2D>(@"images\parallax\plainsbackground1");
			parallax2 = Game.Content.Load<Texture2D>(@"images\parallax\plainsbackground2");
			parallax3 = Game.Content.Load<Texture2D>(@"images\parallax\plainsbackground3");
			platformTexture = Game.Content.Load<Texture2D>(@"images\tiles\platforms");

			camera = new Camera(GraphicsDevice.Viewport, new Point(6400, 450), 1f);
			camera.Position = new Vector2(0, 0);

            mapSegments.Add(new Rectangle(-5, 0, 0, (int)camera.Size.Y));
            mapSegments.Add(new Rectangle((int)camera.Size.X + 5, 0, (int)camera.Size.X, (int)camera.Size.Y));
            
			parallax1Background = new ParallaxBackground(parallax1, new Vector2(camera.Position.X - (myGame.WindowSize.X / 2), 0), Color.White, 1.000f, camera);
			parallax2Background = new ParallaxBackground(parallax2, new Vector2(camera.Position.X - (myGame.WindowSize.X / 2), 0), Color.White, 1.125f, camera);
			parallax3Background = new ParallaxBackground(parallax3, new Vector2(camera.Position.X - (myGame.WindowSize.X / 2), 0), Color.White, 1.250f, camera);

			playerAnimationSetList.Add(new Sprite.AnimationSet("IDLE", playerTexture, new Point(60, 50), new Point(1, 1), new Point(0, 0), 1000));
			playerAnimationSetList.Add(new Sprite.AnimationSet("WALK", playerTexture, new Point(60, 50), new Point(4, 3), new Point(0, 0), 100));
			playerAnimationSetList.Add(new Sprite.AnimationSet("JUMP", playerTexture, new Point(60, 50), new Point(4, 1), new Point(0, 150), 1000));
			playerAnimationSetList.Add(new Sprite.AnimationSet("SHOOT", playerTexture, new Point(60, 50), new Point(1, 3), new Point(240, 0), 250));
			playerAnimationSetList.Add(new Sprite.AnimationSet("SWING", playerTexture, new Point(60, 50), new Point(3, 2), new Point(0, 200), 1000));
			playerAnimationSetList.Add(new Sprite.AnimationSet("BLOCK", playerTexture, new Point(60, 50), new Point(2, 2), new Point(180, 200), 1000));

			platformAnimationSetList.Add(new Sprite.AnimationSet("1", platformTexture, new Point(25, 25), new Point(1, 1), new Point(0, 0), 0));
			platformAnimationSetList.Add(new Sprite.AnimationSet("2", platformTexture, new Point(25, 25), new Point(1, 1), new Point(25, 0), 0));
			platformAnimationSetList.Add(new Sprite.AnimationSet("3", platformTexture, new Point(25, 25), new Point(1, 1), new Point(50, 0), 0));
			platformAnimationSetList.Add(new Sprite.AnimationSet("4", platformTexture, new Point(25, 25), new Point(1, 1), new Point(75, 0), 0));
			platformAnimationSetList.Add(new Sprite.AnimationSet("5", platformTexture, new Point(25, 25), new Point(1, 1), new Point(0, 25), 0));
			platformAnimationSetList.Add(new Sprite.AnimationSet("6", platformTexture, new Point(25, 25), new Point(1, 1), new Point(25, 25), 0));
			platformAnimationSetList.Add(new Sprite.AnimationSet("7", platformTexture, new Point(25, 25), new Point(1, 1), new Point(50, 25), 0));
			platformAnimationSetList.Add(new Sprite.AnimationSet("8", platformTexture, new Point(25, 25), new Point(1, 1), new Point(75, 25), 0));
			platformAnimationSetList.Add(new Sprite.AnimationSet("9", platformTexture, new Point(25, 25), new Point(1, 1), new Point(0, 50), 0));
			platformAnimationSetList.Add(new Sprite.AnimationSet("10", platformTexture, new Point(25, 25), new Point(1, 1), new Point(25, 50), 0));
			platformAnimationSetList.Add(new Sprite.AnimationSet("11", platformTexture, new Point(25, 25), new Point(1, 1), new Point(50, 50), 0));
			platformAnimationSetList.Add(new Sprite.AnimationSet("12", platformTexture, new Point(25, 25), new Point(1, 1), new Point(75, 50), 0));
			platformAnimationSetList.Add(new Sprite.AnimationSet("13", platformTexture, new Point(25, 25), new Point(1, 1), new Point(0, 75), 0));
			platformAnimationSetList.Add(new Sprite.AnimationSet("14", platformTexture, new Point(25, 25), new Point(1, 1), new Point(25, 75), 0));
			platformAnimationSetList.Add(new Sprite.AnimationSet("15", platformTexture, new Point(25, 25), new Point(1, 1), new Point(50, 75), 0));
			platformAnimationSetList.Add(new Sprite.AnimationSet("16", platformTexture, new Point(25, 25), new Point(1, 1), new Point(75, 75), 0));

			MovementKeys.Add(Keys.A);
			MovementKeys.Add(Keys.W);
			MovementKeys.Add(Keys.D);
			MovementKeys.Add(Keys.S);
			MovementKeys.Add(Keys.Space);
			MovementKeys.Add(Keys.E);
			MovementKeys.Add(Keys.Q);

			_Mana = new Player.Mana(100, 5000, 100);
			player = new Player(new Vector2(25, (myGame.WindowSize.Y - playerAnimationSetList[0].frameSize.Y) - 75), MovementKeys, 1.05f, _Mana, Color.White, playerAnimationSetList, myGame);

			debugLabel = new Label(new Vector2(0, 00), myGame.segoeUIMonoDebug, 1f, Color.Black, "");

			base.LoadContent();
		}

		/// <summary>
		/// This is to update the Game Manager's Classes and sprites.
		/// </summary>
		/// <param name="gameTime">This is to check the run time.</param>
		public override void Update(GameTime gameTime)
		{
			#region Parallax Stuff
			if (camera.IsInView(player.GetPosition, new Vector2(20, 49)))
			{
				DebugLines[5] = "true";
				camera.Position = new Vector2(player.GetPosition.X + 200f, 0);
			}

			parallax1Background.position = new Vector2(camera.Position.X - (myGame.WindowSize.X / 2), 0);
			parallax1Background.Update(gameTime);
			parallax2Background.position = new Vector2(camera.Position.X - (myGame.WindowSize.X / 2), 0);
			parallax2Background.Update(gameTime);
			parallax3Background.position = new Vector2(camera.Position.X - (myGame.WindowSize.X / 2), 0);
			parallax3Background.Update(gameTime);
			#endregion

			if (myGame.CheckKey(Keys.G) && !wonLevel)
			{
				wonLevel = true;
			}
			if (!myGame.CheckKey(Keys.G) && wonLevel)
			{
				wonLevel = false;
			}

			if (wonLevel)
			{
				level += 1;

				if (level > 6)
				{
					level = 0;
				}

				levelLoaded = false;
			}

			PlayerCollisions = player.GetPlayerRectangles();

			player.Update(gameTime);

			debugLabel.Update(gameTime, DebugLines[0] + "\n" + DebugLines[1] + "\n" +
										DebugLines[2] + "\n" + DebugLines[3] + "\n" +
										DebugLines[4] + "\n" + DebugLines[5] + "\n" +
										DebugLines[6] + "\n" + DebugLines[7] + "\n" +
										DebugLines[8] + "\n" + DebugLines[9]);

			DebugLines[0] = "IsGrounded=" + player.isGrounded + " IsJumping=" + player.isJumping + " IsFalling=" + player.isFalling + " Direction=(" + player.GetDirection.X + "," + player.GetDirection.Y + ")";
			DebugLines[3] = "mana=" + player._Mana.mana + " maxMana=" + player._Mana.maxMana + " manaRechargeTime=" + player._Mana.manaRechargeTime + " manaInterval=" + player._Mana.manaInterval;
			DebugLines[4] = "CanShoot=" + player.CanShootProjectile + " CreateNew=" + player.CreateNewProjectile + " HasShot=" + player.HasShotProjectile + " projectileListCreated=" + player.ProjectileListCreated;

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
				if (!levelLoaded)
				{
					platformRectangles.RemoveRange(0, platformRectangles.Count);
					platformList.RemoveRange(0, platformList.Count);

					SpawnBricks(level);

					levelLoaded = true;
				}

				parallax1Background.Draw(gameTime, spriteBatch);
				parallax2Background.Draw(gameTime, spriteBatch);
				parallax3Background.Draw(gameTime, spriteBatch);

				foreach (PlatformManager pm in platformList)
				{
					pm.Draw(gameTime, spriteBatch);
				}

                // Draw the player.
                player.Draw(gameTime, spriteBatch);

                // Debug stuff.
				for (int i = 0; i < platformRectangles.Count; i++)
				{
					spriteBatch.Draw(debugDotTexture, new Rectangle((int)platformRectangles[i].X, (int)platformRectangles[i].Y, (int)platformRectangles[i].Width, 1), new Color(i % 2.2f, i % 2.1f, i % 2.0f));
					spriteBatch.Draw(debugDotTexture, new Rectangle((int)platformRectangles[i].X + (int)platformRectangles[i].Width, (int)platformRectangles[i].Y, 1, (int)platformRectangles[i].Height), new Color(i % 2.2f, i % 2.1f, i % 2.0f));
					spriteBatch.Draw(debugDotTexture, new Rectangle((int)platformRectangles[i].X, (int)platformRectangles[i].Y + (int)platformRectangles[i].Height, (int)platformRectangles[i].Width, 1), new Color(i % 2.2f, i % 2.1f, i % 2.0f));
					spriteBatch.Draw(debugDotTexture, new Rectangle((int)platformRectangles[i].X, (int)platformRectangles[i].Y, 1, (int)platformRectangles[i].Height), new Color(i % 2.2f, i % 2.1f, i % 2.0f));
				}
                spriteBatch.Draw(debugDotTexture, new Rectangle((int)PlayerCollisions.X, (int)PlayerCollisions.Y, (int)PlayerCollisions.Width, 1), Color.Blue);
				spriteBatch.Draw(debugDotTexture, new Rectangle((int)PlayerCollisions.X + (int)PlayerCollisions.Width, (int)PlayerCollisions.Y, 1, (int)PlayerCollisions.Height), Color.Red);
				spriteBatch.Draw(debugDotTexture, new Rectangle((int)PlayerCollisions.X, (int)PlayerCollisions.Y + (int)PlayerCollisions.Height, (int)PlayerCollisions.Width, 1), Color.Green);
				spriteBatch.Draw(debugDotTexture, new Rectangle((int)PlayerCollisions.X, (int)PlayerCollisions.Y, 1, (int)PlayerCollisions.Height), Color.Yellow);
            }
			spriteBatch.End();

			spriteBatch.Begin();
			{
				// To debug variables.
				debugLabel.Draw(gameTime, spriteBatch);
			}
			spriteBatch.End();

			base.Draw(gameTime);
		}

		public void SpawnBricks(int level)
		{
			int width = 0;
			int height = 0;
			uint[,] brickspawn = new uint[height, width];

			switch (level)
			{
				case 1:
					brickspawn = MapHelper.GetTileArray(Maps.HappyFace());
					height = Maps.HappyFace().Count;
					width = Maps.HappyFace()[0].Length;
					break;
				case 2:
					brickspawn = MapHelper.GetTileArray(Maps.Plains());
					height = Maps.Plains().Count;
					width = Maps.Plains()[0].Length;
					break;
				case 3:
					brickspawn = MapHelper.GetTileArray(Maps.Village());
					height = Maps.Village().Count;
					width = Maps.Village()[0].Length;
					break;
				case 4:
					brickspawn = MapHelper.GetTileArray(Maps.Forest());
					height = Maps.Forest().Count;
					width = Maps.Forest()[0].Length;
					break;
				case 5:
					brickspawn = MapHelper.GetTileArray(Maps.Cave());
					height = Maps.Cave().Count;
					width = Maps.Cave()[0].Length;
					break;
				case 6:
					brickspawn = MapHelper.GetTileArray(Maps.Hell());
					height = Maps.Hell().Count;
					width = Maps.Hell()[0].Length;
					break;
			}

			camera.Size = new Point(width * 25, height * 25);

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

			foreach (PlatformManager pm in platformList)
			{
				platformRectangles.Add(new Rectangle((int)pm.GetPosition.X, (int)pm.GetPosition.Y, 25, 25));
			}
		}
	}
}