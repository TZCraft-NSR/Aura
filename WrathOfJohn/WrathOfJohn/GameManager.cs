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

		Song forestSong;
		Song finalBoss;
		bool musicStarted;

		#region Player Variables
		/// <summary>
		/// The player class.
		/// </summary>
		public Player player;
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
		Texture2D plainsPlatformTexture;
		/// <summary>
		/// 
		/// </summary>
		Texture2D forestPlatformTexture;
		/// <summary>
		/// 
		/// </summary>
		Texture2D cavePlatformTexture;
		/// <summary>
		/// 
		/// </summary>
		Texture2D villagePlatformTexture;
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
			set;
		}
		#endregion

		#region Background Variables
		/// <summary>
		/// The first parallax texture.
		/// </summary>
		Texture2D plainsParallax1;
		/// <summary>
		/// The second parallax texture.
		/// </summary>
		Texture2D plainsParallax2;
		/// <summary>
		/// The third parallax texture.
		/// </summary>
		Texture2D plainsParallax3;
		/// <summary>
		/// The first parallax background class.
		/// </summary>
		ParallaxBackground plainsParallax1Background;
		/// <summary>
		/// The second parallax background class.
		/// </summary>
		ParallaxBackground plainsParallax2Background;
		/// <summary>
		/// The third parallax background class.
		/// </summary>
		ParallaxBackground plainsParallax3Background;
		/// <summary>
		/// The first parallax texture.
		/// </summary>
		Texture2D forestParallax1;
		/// <summary>
		/// The second parallax texture.
		/// </summary>
		Texture2D forestParallax2;
		/// <summary>
		/// The third parallax texture.
		/// </summary>
		Texture2D forestParallax3;
		/// <summary>
		/// The first parallax background class.
		/// </summary>
		ParallaxBackground forestParallax1Background;
		/// <summary>
		/// The second parallax background class.
		/// </summary>
		ParallaxBackground forestParallax2Background;
		/// <summary>
		/// The third parallax background class.
		/// </summary>
		ParallaxBackground forestParallax3Background;
		/// <summary>
		/// The first parallax texture.
		/// </summary>
		Texture2D caveParallax1;
		/// <summary>
		/// The second parallax texture.
		/// </summary>
		Texture2D caveParallax2;
		/// <summary>
		/// The third parallax texture.
		/// </summary>
		Texture2D caveParallax3;
		/// <summary>
		/// The first parallax background class.
		/// </summary>
		ParallaxBackground caveParallax1Background;
		/// <summary>
		/// The second parallax background class.
		/// </summary>
		ParallaxBackground caveParallax2Background;
		/// <summary>
		/// The third parallax background class.
		/// </summary>
		ParallaxBackground caveParallax3Background;
		/// <summary>
		/// The first parallax texture.
		/// </summary>
		Texture2D villageParallax1;
		/// <summary>
		/// The first parallax background class.
		/// </summary>
		ParallaxBackground villageParallax1Background;
		#endregion

		#region Enemy Variables
		/// <summary>
		/// The player class.
		/// </summary>
		// CircleEnemy cEnemy;
		/// <summary>
		/// The animation list for the player.
		/// </summary>
		List<Sprite.AnimationSet> cEnemyAnimationSetList;
		List<Sprite.AnimationSet> sEnemyAnimationSetList;
		List<Sprite.AnimationSet> tEnemyAnimationSetList;
		/// <summary>
		/// The player class' texture.
		/// </summary>
		Texture2D cEnemyTexture;
		Texture2D sEnemyTexture;
		Texture2D tEnemyTexture;
		/// <summary>
		/// The player segment collisions.
		/// </summary>
		//protected Rectangle PlayerCollisions;
		public List<Enemy> cEnemyList;
		public List<Enemy> sEnemyList;
		public List<Enemy> tEnemyList;
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

			cEnemyList = new List<Enemy>();
			cEnemyAnimationSetList = new List<Sprite.AnimationSet>();
			sEnemyList = new List<Enemy>();
			sEnemyAnimationSetList = new List<Sprite.AnimationSet>();
			tEnemyList = new List<Enemy>();
			tEnemyAnimationSetList = new List<Sprite.AnimationSet>();

			platformList = new List<PlatformManager>();
			platformAnimationSetList = new List<Sprite.AnimationSet>();
			platformRectangles = new List<Rectangle>();

			mapSegments = new List<Rectangle>();

			level = 2;

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

			playerTexture = Game.Content.Load<Texture2D>(@"images\players\mage");
			debugDotTexture = Game.Content.Load<Texture2D>(@"images\debug\line");
			ProjectileTexture = Game.Content.Load<Texture2D>(@"images\projectiles\beam");
			plainsParallax1 = Game.Content.Load<Texture2D>(@"images\parallax\plainsbackground1");
			plainsParallax2 = Game.Content.Load<Texture2D>(@"images\parallax\plainsbackground2");
			plainsParallax3 = Game.Content.Load<Texture2D>(@"images\parallax\plainsbackground3");
			forestParallax1 = Game.Content.Load<Texture2D>(@"images\parallax\forestbackground1");
			forestParallax2 = Game.Content.Load<Texture2D>(@"images\parallax\forestbackground2");
			forestParallax3 = Game.Content.Load<Texture2D>(@"images\parallax\forestbackground3");
			caveParallax1 = Game.Content.Load<Texture2D>(@"images\parallax\cavebackground1");
			caveParallax2 = Game.Content.Load<Texture2D>(@"images\parallax\cavebackground2");
			caveParallax3 = Game.Content.Load<Texture2D>(@"images\parallax\cavebackground3");
			villageParallax1 = Game.Content.Load<Texture2D>(@"images\parallax\village");
			cEnemyTexture = Game.Content.Load<Texture2D>(@"images\enemies\CircleEnemy");
			sEnemyTexture = Game.Content.Load<Texture2D>(@"images\enemies\SquareEnemy");
			tEnemyTexture = Game.Content.Load<Texture2D>(@"images\enemies\TriangleEnemy");
			plainsPlatformTexture = Game.Content.Load<Texture2D>(@"images\tiles\plainsTiles");
			forestPlatformTexture = Game.Content.Load<Texture2D>(@"images\tiles\forestTiles");
			cavePlatformTexture = Game.Content.Load<Texture2D>(@"images\tiles\caveTiles");
			villagePlatformTexture = Game.Content.Load<Texture2D>(@"images\tiles\villageTiles");
			forestSong = Game.Content.Load<Song>(@"sounds\music\forest");
			finalBoss = Game.Content.Load<Song>(@"sounds\music\finalboss");

			camera = new Camera(GraphicsDevice.Viewport, new Point(6400, 450), 1f);
			camera.Position = new Vector2(0, 0);

			mapSegments.Add(new Rectangle(-5, 0, 5, (int)camera.Size.Y));
			mapSegments.Add(new Rectangle((int)camera.Size.X, 0, 5, (int)camera.Size.Y));

			plainsParallax1Background = new ParallaxBackground(plainsParallax1, new Vector2(camera.Position.X - (myGame.WindowSize.X / 2), 0), Color.White, 1.000f, camera);
			plainsParallax2Background = new ParallaxBackground(plainsParallax2, new Vector2(camera.Position.X - (myGame.WindowSize.X / 2), 0), Color.White, 1.125f, camera);
			plainsParallax3Background = new ParallaxBackground(plainsParallax3, new Vector2(camera.Position.X - (myGame.WindowSize.X / 2), 0), Color.White, 1.250f, camera);

			villageParallax1Background = new ParallaxBackground(villageParallax1, new Vector2(camera.Position.X - (myGame.WindowSize.X / 2), 0), Color.White, 1.000f, camera);

			forestParallax1Background = new ParallaxBackground(forestParallax1, new Vector2(camera.Position.X - (myGame.WindowSize.X / 2), 0), Color.White, 1.000f, camera);
			forestParallax2Background = new ParallaxBackground(forestParallax2, new Vector2(camera.Position.X - (myGame.WindowSize.X / 2), 0), Color.White, 1.125f, camera);
			forestParallax3Background = new ParallaxBackground(forestParallax3, new Vector2(camera.Position.X - (myGame.WindowSize.X / 2), 0), Color.White, 1.250f, camera);

			caveParallax1Background = new ParallaxBackground(caveParallax1, new Vector2(camera.Position.X - (myGame.WindowSize.X / 2), 0), Color.White, 1.000f, camera);
			caveParallax2Background = new ParallaxBackground(caveParallax2, new Vector2(camera.Position.X - (myGame.WindowSize.X / 2), 0), Color.White, 1.125f, camera);
			caveParallax3Background = new ParallaxBackground(caveParallax3, new Vector2(camera.Position.X - (myGame.WindowSize.X / 2), 0), Color.White, 1.250f, camera);

			playerAnimationSetList.Add(new Sprite.AnimationSet("IDLE", playerTexture, new Point(124, 148), new Point(4, 1), new Point(0, 2), 1600));
			playerAnimationSetList.Add(new Sprite.AnimationSet("WALK", playerTexture, new Point(100, 140), new Point(8, 1), new Point(0, 476), 100));
			playerAnimationSetList.Add(new Sprite.AnimationSet("JUMP", playerTexture, new Point(187, 174), new Point(4, 1), new Point(0, 302), 1600));
			playerAnimationSetList.Add(new Sprite.AnimationSet("SHOOT", playerTexture, new Point(124, 148), new Point(5, 1), new Point(0, 154), 1600));

			cEnemyAnimationSetList.Add(new Sprite.AnimationSet("IDLE", cEnemyTexture, new Point(25, 25), new Point(1, 1), new Point(0, 0), 1600));
			cEnemyAnimationSetList.Add(new Sprite.AnimationSet("CHASE", cEnemyTexture, new Point(25, 25), new Point(9, 1), new Point(25, 0), 100));
			//Needs rest of animations ^
			sEnemyAnimationSetList.Add(new Sprite.AnimationSet("IDLE", sEnemyTexture, new Point(25, 25), new Point(1, 1), new Point(0, 0), 1600));
			sEnemyAnimationSetList.Add(new Sprite.AnimationSet("CHASE", sEnemyTexture, new Point(25, 25), new Point(5, 1), new Point(0, 0), 100));
			sEnemyAnimationSetList.Add(new Sprite.AnimationSet("FALLING", sEnemyTexture, new Point(25, 25), new Point(4, 1), new Point(150, 0), 100));
			//Needs edit ^
			tEnemyAnimationSetList.Add(new Sprite.AnimationSet("IDLE", tEnemyTexture, new Point(25, 25), new Point(1, 1), new Point(0, 0), 1000));
			//Needs edit ^

			platformAnimationSetList.Add(new Sprite.AnimationSet("1", plainsPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(0, 0), 0));
			platformAnimationSetList.Add(new Sprite.AnimationSet("2", plainsPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(25, 0), 0));
			platformAnimationSetList.Add(new Sprite.AnimationSet("3", plainsPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(50, 0), 0));
			platformAnimationSetList.Add(new Sprite.AnimationSet("4", plainsPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(75, 0), 0));
			platformAnimationSetList.Add(new Sprite.AnimationSet("5", plainsPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(0, 25), 0));
			platformAnimationSetList.Add(new Sprite.AnimationSet("6", plainsPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(25, 25), 0));
			platformAnimationSetList.Add(new Sprite.AnimationSet("7", plainsPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(50, 25), 0));
			platformAnimationSetList.Add(new Sprite.AnimationSet("8", plainsPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(75, 25), 0));
			platformAnimationSetList.Add(new Sprite.AnimationSet("9", plainsPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(0, 50), 0));
			platformAnimationSetList.Add(new Sprite.AnimationSet("10", plainsPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(25, 50), 0));
			platformAnimationSetList.Add(new Sprite.AnimationSet("11", plainsPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(50, 50), 0));
			platformAnimationSetList.Add(new Sprite.AnimationSet("12", plainsPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(75, 50), 0));
			platformAnimationSetList.Add(new Sprite.AnimationSet("13", plainsPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(0, 75), 0));
			platformAnimationSetList.Add(new Sprite.AnimationSet("14", plainsPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(25, 75), 0));
			platformAnimationSetList.Add(new Sprite.AnimationSet("15", plainsPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(50, 75), 0));
			platformAnimationSetList.Add(new Sprite.AnimationSet("16", plainsPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(75, 75), 0));
			platformAnimationSetList.Add(new Sprite.AnimationSet("17", forestPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(0, 0), 0));
			platformAnimationSetList.Add(new Sprite.AnimationSet("18", forestPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(25, 0), 0));
			platformAnimationSetList.Add(new Sprite.AnimationSet("19", forestPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(50, 0), 0));
			platformAnimationSetList.Add(new Sprite.AnimationSet("20", forestPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(75, 0), 0));
			platformAnimationSetList.Add(new Sprite.AnimationSet("21", forestPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(0, 25), 0));
			platformAnimationSetList.Add(new Sprite.AnimationSet("22", forestPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(25, 25), 0));
			platformAnimationSetList.Add(new Sprite.AnimationSet("23", forestPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(50, 25), 0));
			platformAnimationSetList.Add(new Sprite.AnimationSet("24", forestPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(75, 25), 0));
			platformAnimationSetList.Add(new Sprite.AnimationSet("25", forestPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(0, 50), 0));
			platformAnimationSetList.Add(new Sprite.AnimationSet("26", forestPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(25, 50), 0));
			platformAnimationSetList.Add(new Sprite.AnimationSet("27", forestPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(50, 50), 0));
			platformAnimationSetList.Add(new Sprite.AnimationSet("28", forestPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(75, 50), 0));
			platformAnimationSetList.Add(new Sprite.AnimationSet("29", forestPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(0, 75), 0));
			platformAnimationSetList.Add(new Sprite.AnimationSet("30", forestPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(25, 75), 0));
			platformAnimationSetList.Add(new Sprite.AnimationSet("31", forestPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(50, 75), 0));
			platformAnimationSetList.Add(new Sprite.AnimationSet("32", forestPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(75, 75), 0));
			platformAnimationSetList.Add(new Sprite.AnimationSet("33", cavePlatformTexture, new Point(25, 25), new Point(1, 1), new Point(0, 0), 0));
			platformAnimationSetList.Add(new Sprite.AnimationSet("34", villagePlatformTexture, new Point(25, 25), new Point(1, 1), new Point(0, 0), 0));

			MovementKeys.Add(Keys.A);
			MovementKeys.Add(Keys.W);
			MovementKeys.Add(Keys.D);
			MovementKeys.Add(Keys.S);
			MovementKeys.Add(Keys.Space);
			MovementKeys.Add(Keys.E);
			MovementKeys.Add(Keys.Q);

			_Mana = new Player.Mana(100, 5000, 100);
			player = new Player(new Vector2(25, (myGame.WindowSize.Y - playerAnimationSetList[0].frameSize.Y) - 75), MovementKeys, 1.75f, _Mana, Color.White, playerAnimationSetList, myGame);

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
			//
			plainsParallax1Background.position = new Vector2(camera.Position.X - (myGame.WindowSize.X / 2), 0);
			plainsParallax1Background.Update(gameTime);
			plainsParallax2Background.position = new Vector2(camera.Position.X - (myGame.WindowSize.X / 2), 0);
			plainsParallax2Background.Update(gameTime);
			plainsParallax3Background.position = new Vector2(camera.Position.X - (myGame.WindowSize.X / 2), 0);
			plainsParallax3Background.Update(gameTime);

			villageParallax1Background.position = new Vector2(camera.Position.X - (myGame.WindowSize.X / 2), 0);
			villageParallax1Background.Update(gameTime);

			forestParallax1Background.position = new Vector2(camera.Position.X - (myGame.WindowSize.X / 2), 0);
			forestParallax1Background.Update(gameTime);
			forestParallax2Background.position = new Vector2(camera.Position.X - (myGame.WindowSize.X / 2), 0);
			forestParallax2Background.Update(gameTime);
			forestParallax3Background.position = new Vector2(camera.Position.X - (myGame.WindowSize.X / 2), 0);
			forestParallax3Background.Update(gameTime);

			caveParallax1Background.position = new Vector2(camera.Position.X - (myGame.WindowSize.X / 2), 0);
			caveParallax1Background.Update(gameTime);
			caveParallax2Background.position = new Vector2(camera.Position.X - (myGame.WindowSize.X / 2), 0);
			caveParallax2Background.Update(gameTime);
			caveParallax3Background.position = new Vector2(camera.Position.X - (myGame.WindowSize.X / 2), 0);
			caveParallax3Background.Update(gameTime);
			#endregion

			mapSegments[1] = new Rectangle(camera.Size.X, mapSegments[1].Y, mapSegments[1].Width, mapSegments[1].Height);

			if (myGame.CheckKey(Keys.G) && !wonLevel)
			{
				wonLevel = true;
			}

			if (wonLevel)
			{
				level += 1;

				if (level > 6)
				{
					level = 1;
				}

				levelLoaded = false;

				wonLevel = false;

				if (musicStarted)
				{
					MediaPlayer.Stop();
				}

				musicStarted = false;
			}

			PlayerCollisions = player.GetPlayerRectangles();

			player.Update(gameTime);

			for (int i = 0; i < cEnemyList.Count; i++)
			{
				if (cEnemyList[i].DeleteMe)
				{
					cEnemyList.RemoveAt(i);
					i--;
				}
				else
				{
					cEnemyList[i].Update(gameTime);
				}
			}
			for (int i = 0; i < sEnemyList.Count; i++)
			{
				if (sEnemyList[i].DeleteMe)
				{
					sEnemyList.RemoveAt(i);
					i--;
				}
				else
				{
					sEnemyList[i].Update(gameTime);
				}
			}
			for (int i = 0; i < tEnemyList.Count; i++)
			{
				if (tEnemyList[i].DeleteMe)
				{
					tEnemyList.RemoveAt(i);
					i--;
				}
				else
				{
					tEnemyList[i].Update(gameTime);
				}
			}
			if (player.Dead)
			{
				level = 1;
				levelLoaded = false;
				wonLevel = false;
				player.Dead = false;
			}

			debugLabel.Update(gameTime, DebugLines[0] + "\n" + DebugLines[1] + "\n" +
										DebugLines[2] + "\n" + DebugLines[3] + "\n" +
										DebugLines[4] + "\n" + DebugLines[5] + "\n" +
										DebugLines[6] + "\n" + DebugLines[7] + "\n" +
										DebugLines[8] + "\n" + DebugLines[9]);

			DebugLines[0] = "IsGrounded=" + player.isGrounded + " IsJumping=" + player.isJumping + " IsFalling=" + player.isFalling + " Direction=(" + player.GetDirection.X + "," + player.GetDirection.Y + ")";
			DebugLines[3] = "mana=" + player._Mana.mana + " maxMana=" + player._Mana.maxMana + " manaRechargeTime=" + player._Mana.manaRechargeTime + " manaInterval=" + player._Mana.manaInterval;
			DebugLines[4] = "CanShoot=" + player.CanShootProjectile + " CreateNew=" + player.CreateNewProjectile + " HasShot=" + player.HasShotProjectile + " projectileListCreated=" + player.ProjectileListCreated;
			DebugLines[6] = "Player Dead=" + player.Dead + " Player Lives=" + player.Lives;

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
					cEnemyList.RemoveRange(0, cEnemyList.Count);
					sEnemyList.RemoveRange(0, sEnemyList.Count);
					tEnemyList.RemoveRange(0, tEnemyList.Count);


					SpawnBricks(level);

					levelLoaded = true;
				}

				if (level <= 2)
				{
					plainsParallax1Background.Draw(gameTime, spriteBatch);
					plainsParallax2Background.Draw(gameTime, spriteBatch);
					plainsParallax3Background.Draw(gameTime, spriteBatch);
				}
				if (level == 3)
				{
					plainsParallax1Background.Draw(gameTime, spriteBatch);
					plainsParallax2Background.Draw(gameTime, spriteBatch);
					plainsParallax3Background.Draw(gameTime, spriteBatch);
					villageParallax1Background.Draw(gameTime, spriteBatch);
				}
				if (level == 4)
				{
					forestParallax1Background.Draw(gameTime, spriteBatch);
					forestParallax2Background.Draw(gameTime, spriteBatch);
					forestParallax3Background.Draw(gameTime, spriteBatch);
				}
				if (level >= 5)
				{
					caveParallax1Background.Draw(gameTime, spriteBatch);
					caveParallax2Background.Draw(gameTime, spriteBatch);
					caveParallax3Background.Draw(gameTime, spriteBatch);
				}
				if (level == 4 && !musicStarted)
				{
					MediaPlayer.Stop();
					MediaPlayer.Play(forestSong);
					musicStarted = true;
				}
				if (level == 6 && !musicStarted)
				{
					MediaPlayer.Stop();
					MediaPlayer.Play(finalBoss);
					musicStarted = true;
				}

				foreach (PlatformManager pm in platformList)
				{
					pm.Draw(gameTime, spriteBatch);
				}
			}
			spriteBatch.End();
			
			// Draw the player and enemies.
			spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, camera.GetTransformation());
			{
				player.Draw(gameTime, spriteBatch);

				foreach (Enemy ce in cEnemyList)
				{
					ce.Draw(gameTime, spriteBatch);
				}
				foreach (Enemy se in sEnemyList)
				{
					se.Draw(gameTime, spriteBatch);
				}
				foreach (Enemy te in tEnemyList)
				{
					te.Draw(gameTime, spriteBatch);
				}
			}
			spriteBatch.End();
			
			// Debug Rectangles
			
			spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap, null, null, null, camera.GetTransformation());
			{
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
				for (int i = 0; i < cEnemyList.Count; i++)
				{
					spriteBatch.Draw(debugDotTexture, new Rectangle((int)cEnemyList[i].playerCollisions.X, (int)cEnemyList[i].playerCollisions.Y, (int)cEnemyList[i].playerCollisions.Width, 1), Color.Blue);
					spriteBatch.Draw(debugDotTexture, new Rectangle((int)cEnemyList[i].playerCollisions.X + (int)cEnemyList[i].playerCollisions.Width, (int)cEnemyList[i].playerCollisions.Y, 1, (int)cEnemyList[i].playerCollisions.Height), Color.Red);
					spriteBatch.Draw(debugDotTexture, new Rectangle((int)cEnemyList[i].playerCollisions.X, (int)cEnemyList[i].playerCollisions.Y + (int)cEnemyList[i].playerCollisions.Height, (int)cEnemyList[i].playerCollisions.Width, 1), Color.Green);
					spriteBatch.Draw(debugDotTexture, new Rectangle((int)cEnemyList[i].playerCollisions.X, (int)cEnemyList[i].playerCollisions.Y, 1, (int)cEnemyList[i].playerCollisions.Height), Color.Yellow);
				}
				for (int i = 0; i < tEnemyList.Count; i++)
				{
					spriteBatch.Draw(debugDotTexture, new Rectangle((int)tEnemyList[i].playerCollisions.X, (int)tEnemyList[i].playerCollisions.Y, (int)tEnemyList[i].playerCollisions.Width, 1), Color.Blue);
					spriteBatch.Draw(debugDotTexture, new Rectangle((int)tEnemyList[i].playerCollisions.X + (int)tEnemyList[i].playerCollisions.Width, (int)tEnemyList[i].playerCollisions.Y, 1, (int)tEnemyList[i].playerCollisions.Height), Color.Red);
					spriteBatch.Draw(debugDotTexture, new Rectangle((int)tEnemyList[i].playerCollisions.X, (int)tEnemyList[i].playerCollisions.Y + (int)tEnemyList[i].playerCollisions.Height, (int)tEnemyList[i].playerCollisions.Width, 1), Color.Green);
					spriteBatch.Draw(debugDotTexture, new Rectangle((int)tEnemyList[i].playerCollisions.X, (int)tEnemyList[i].playerCollisions.Y, 1, (int)tEnemyList[i].playerCollisions.Height), Color.Yellow);
				}
				for (int i = 0; i < sEnemyList.Count; i++)
				{
					spriteBatch.Draw(debugDotTexture, new Rectangle((int)sEnemyList[i].playerCollisions.X, (int)sEnemyList[i].playerCollisions.Y, (int)sEnemyList[i].playerCollisions.Width, 1), Color.Blue);
					spriteBatch.Draw(debugDotTexture, new Rectangle((int)sEnemyList[i].playerCollisions.X + (int)sEnemyList[i].playerCollisions.Width, (int)sEnemyList[i].playerCollisions.Y, 1, (int)sEnemyList[i].playerCollisions.Height), Color.Red);
					spriteBatch.Draw(debugDotTexture, new Rectangle((int)sEnemyList[i].playerCollisions.X, (int)sEnemyList[i].playerCollisions.Y + (int)sEnemyList[i].playerCollisions.Height, (int)sEnemyList[i].playerCollisions.Width, 1), Color.Green);
					spriteBatch.Draw(debugDotTexture, new Rectangle((int)sEnemyList[i].playerCollisions.X, (int)sEnemyList[i].playerCollisions.Y, 1, (int)sEnemyList[i].playerCollisions.Height), Color.Yellow);
				}
				for (int i = 0; i < mapSegments.Count; i++)
				{
					spriteBatch.Draw(debugDotTexture, new Rectangle((int)mapSegments[i].X, (int)mapSegments[i].Y, (int)mapSegments[i].Width, 1), Color.Blue);
					spriteBatch.Draw(debugDotTexture, new Rectangle((int)mapSegments[i].X + (int)mapSegments[i].Width, (int)mapSegments[i].Y, 1, (int)mapSegments[i].Height), Color.Red);
					spriteBatch.Draw(debugDotTexture, new Rectangle((int)mapSegments[i].X, (int)mapSegments[i].Y + (int)mapSegments[i].Height, (int)mapSegments[i].Width, 1), Color.Green);
					spriteBatch.Draw(debugDotTexture, new Rectangle((int)mapSegments[i].X, (int)mapSegments[i].Y, 1, (int)mapSegments[i].Height), Color.Yellow);
				}
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
					if (brickspawn[x, y] > 0 && brickspawn[x, y] < 79)
					{
						platformList.Add(new PlatformManager(new Vector2(x * 25, y * 25), myGame, (int)brickspawn[x, y], platformAnimationSetList));
					}
					if (brickspawn[x, y] == 79)
					{
						player.SetPosition(new Vector2(x * 25, y * 25));
						camera.Position = new Vector2(player.GetPosition.X + 200f, player.GetPosition.Y);
					}
                    if (brickspawn[x, y] == 80)
                    {
						cEnemyList.Add(new Enemy(new Vector2(x * 25, y * 25), 1.75f, Enemy.MovementType.HORIZONTAL, Color.White, cEnemyAnimationSetList, player, platformRectangles, mapSegments));
                    }
                    if (brickspawn[x, y] == 81)
                    {
						sEnemyList.Add(new Enemy(new Vector2(x * 25, y * 25), 1.75f, Enemy.MovementType.BOUNCE, Color.White, sEnemyAnimationSetList, player, platformRectangles, mapSegments));
                    }
                    if (brickspawn[x, y] == 82)
                    {
						tEnemyList.Add(new Enemy(new Vector2(x * 25 + 12.5f, y * 25 + 12.5f), 1.75f, Enemy.MovementType.FLY, Color.White, tEnemyAnimationSetList, player, platformRectangles, mapSegments));
                    }
				}
			}

			foreach (PlatformManager pm in platformList)
			{
				platformRectangles.Add(new Rectangle((int)pm.GetPosition.X, (int)pm.GetPosition.Y + 3, 25, 22));
			}
		}
	}
}