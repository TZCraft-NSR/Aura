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
		public Camera camera;

		Song forestSong;
		Song finalBossSong;
		Song caveSong;
		Song plainsvillagesSong;
		public SoundEffect shootSFX;
		public SoundEffect bosshitSFX;
		public SoundEffect enemyhitSFX;
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
			set;
		}
		/// <summary>
		/// Gets or sets if the level has been set.
		/// </summary>
		public bool levelLoaded
		{
			get;
			set;
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
		List<Sprite.AnimationSet> bflEnemyAnimationSetList;
		List<Sprite.AnimationSet> bfrEnemyAnimationSetList;
		List<Sprite.AnimationSet> bhEnemyAnimationSetList;
		/// <summary>
		/// The player class' texture.
		/// </summary>
		Texture2D cEnemyTexture;
		Texture2D sEnemyTexture;
		Texture2D tEnemyTexture;
		Texture2D bflEnemyTexture;
		Texture2D bfrEnemyTexture;
		Texture2D bhEnemyTexture;
		/// <summary>
		/// The player segment collisions.
		/// </summary>
		//protected Rectangle PlayerCollisions;
		public List<Enemy> cEnemyList;
		public List<Enemy> sEnemyList;
		public List<Enemy> tEnemyList;
		public Enemy bflEnemy;
		public Enemy bfrEnemy;
		public Enemy bhEnemy;
		public bool BossCreated;
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

			bflEnemyAnimationSetList = new List<Sprite.AnimationSet>();
			bfrEnemyAnimationSetList = new List<Sprite.AnimationSet>();
			bhEnemyAnimationSetList = new List<Sprite.AnimationSet>();

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
			debugDotTexture = Game.Content.Load<Texture2D>(@"images\other\line");
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
			bflEnemyTexture = Game.Content.Load<Texture2D>(@"images\boss\bossfistleft");
			bfrEnemyTexture = Game.Content.Load<Texture2D>(@"images\boss\bossfistright");
			bhEnemyTexture = Game.Content.Load<Texture2D>(@"images\boss\bosshead");
			plainsPlatformTexture = Game.Content.Load<Texture2D>(@"images\tiles\plainsTiles");
			forestPlatformTexture = Game.Content.Load<Texture2D>(@"images\tiles\forestTiles");
			cavePlatformTexture = Game.Content.Load<Texture2D>(@"images\tiles\caveTiles");
			villagePlatformTexture = Game.Content.Load<Texture2D>(@"images\tiles\villageTiles");
			forestSong = Game.Content.Load<Song>(@"sounds\music\forest");
			finalBossSong = Game.Content.Load<Song>(@"sounds\music\finalboss");
			caveSong = Game.Content.Load<Song>(@"sounds\music\cave");
			plainsvillagesSong = Game.Content.Load<Song>(@"sounds\music\plains-villages");
			shootSFX = Game.Content.Load<SoundEffect>(@"sounds\sfx\projectiles");
			bosshitSFX = Game.Content.Load<SoundEffect>(@"sounds\sfx\bosshit");
			enemyhitSFX = Game.Content.Load<SoundEffect>(@"sounds\sfx\enemyhit");

			camera = new Camera(GraphicsDevice.Viewport, new Point(6400, 450), 1f);
			camera.Position = new Vector2(0, 0);

			mapSegments.Add(new Rectangle(-5, 0, 5, (int)camera.Size.Y));
			mapSegments.Add(new Rectangle((int)camera.Size.X, 0, 5, (int)camera.Size.Y));

			plainsParallax1Background = new ParallaxBackground(plainsParallax1, new Vector2(camera.Position.X - (myGame.WindowSize.X / 2), 0), Color.White, 1.000f, camera);
			plainsParallax2Background = new ParallaxBackground(plainsParallax2, new Vector2(camera.Position.X - (myGame.WindowSize.X / 2), 0), Color.White, 1.125f, camera);
			plainsParallax3Background = new ParallaxBackground(plainsParallax3, new Vector2(camera.Position.X - (myGame.WindowSize.X / 2), 0), Color.White, 1.250f, camera);

			villageParallax1Background = new ParallaxBackground(villageParallax1, new Vector2(camera.Position.X - (myGame.WindowSize.X / 2), 0), Color.White, 0.750f, camera);

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

			bflEnemyAnimationSetList.Add(new Sprite.AnimationSet("IDLE", bflEnemyTexture, new Point(88, 100), new Point(1, 1), new Point(0, 0), 1600));
			bfrEnemyAnimationSetList.Add(new Sprite.AnimationSet("IDLE", bfrEnemyTexture, new Point(88, 100), new Point(1, 1), new Point(0, 0), 1600));
			bflEnemyAnimationSetList.Add(new Sprite.AnimationSet("CHASE", bflEnemyTexture, new Point(88, 100), new Point(1, 1), new Point(0, 0), 100));
			bfrEnemyAnimationSetList.Add(new Sprite.AnimationSet("CHASE", bfrEnemyTexture, new Point(88, 100), new Point(1, 1), new Point(0, 0), 100));
			bflEnemyAnimationSetList.Add(new Sprite.AnimationSet("FALLING", bflEnemyTexture, new Point(88, 100), new Point(1, 1), new Point(0, 0), 100));
			bfrEnemyAnimationSetList.Add(new Sprite.AnimationSet("FALLING", bfrEnemyTexture, new Point(88, 100), new Point(1, 1), new Point(0, 0), 100));
			bhEnemyAnimationSetList.Add(new Sprite.AnimationSet("IDLE1", bhEnemyTexture, new Point(180, 242), new Point(1, 1), new Point(0, 0), 1600));
			bhEnemyAnimationSetList.Add(new Sprite.AnimationSet("IDLE2", bhEnemyTexture, new Point(180, 242), new Point(1, 1), new Point(360, 0), 1600));
			bhEnemyAnimationSetList.Add(new Sprite.AnimationSet("IDLE3", bhEnemyTexture, new Point(180, 242), new Point(1, 1), new Point(720, 0), 1600));

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

			bhEnemy = new Enemy(new Vector2(0, 0), 0, Enemy.MovementType.BOSSHEAD, Color.White, bhEnemyAnimationSetList, player, platformRectangles, mapSegments);
			bflEnemy = new Enemy(new Vector2(0, 0), 2.75f, Enemy.MovementType.BOSSBOUNCE, Color.White, bflEnemyAnimationSetList, player, platformRectangles, mapSegments);
			bfrEnemy = new Enemy(new Vector2(0, 0), 2.75f, Enemy.MovementType.BOSSBOUNCE, Color.White, bfrEnemyAnimationSetList, player, platformRectangles, mapSegments);

			debugLabel = new Label(new Vector2(0, 0), myGame.segoeUIMonoDebug, 1f, new Color(150, 150, 150), "");

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

				if (level > 7)
				{
					level = 2;
				}

				levelLoaded = false;

				player._Mana.maxMana += 20;

				if (!(level > 2 && level <= 3) && (level != 6))
				{
					if (musicStarted)
					{
						MediaPlayer.Stop();
					}

					musicStarted = false;
				}

				wonLevel = false;
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
			if (BossCreated && !bhEnemy.Dead)
			{
				bflEnemy.Update(gameTime);
				bfrEnemy.Update(gameTime);
				bhEnemy.Update(gameTime);
			}
			if (player.Dead)
			{
				levelLoaded = false;
				wonLevel = false;
				player.Dead = false;
				player._Mana.mana = myGame.gameManager.player._Mana.maxMana;
				player._Mana.manaRechargeTime = 5000;
				player.CanShootProjectile = true;
				player.HasShotProjectile = false;
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
			DebugLines[7] = "Boss Head Lives=" + bhEnemy.Lives + " is Boss Head Dead=" + bhEnemy.Dead + " Boss Head Cords=(" + bhEnemy.GetPosition.X + "," + bhEnemy.GetPosition.Y + ")";
			DebugLines[8] = "Boss Fist Left Center=(" + bflEnemy.PositionCenter.X + "," + bflEnemy.PositionCenter.Y + ") Boss Fist Left Direction=(" + bflEnemy.PositionCenter.X + "," + bflEnemy.PositionCenter.Y + ") Boss Fist Left Direction=(" + bflEnemy.GetDirection.X + "," + bflEnemy.GetDirection.Y + ")";
			DebugLines[9] = "Boss Fist Right Center=(" + bfrEnemy.PositionCenter.X + "," + bfrEnemy.PositionCenter.Y + ") Boss Fist Right Direction=(" + bfrEnemy.PositionCenter.X + "," + bfrEnemy.PositionCenter.Y + ") Boss Fist Right Direction=(" + bfrEnemy.GetDirection.X + "," + bfrEnemy.GetDirection.Y + ")";

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
				if ((level >= 1 && level <= 3) && !musicStarted)
				{
					MediaPlayer.Stop();
					MediaPlayer.Play(plainsvillagesSong);
					musicStarted = true;
				}
				if (level == 4 && !musicStarted)
				{
					MediaPlayer.Stop();
					MediaPlayer.Play(forestSong);
					musicStarted = true;
				}
				if ((level >= 5 && level <= 6) && !musicStarted)
				{
					MediaPlayer.Stop();
					MediaPlayer.Play(caveSong);
					musicStarted = true;
				}
				if (level == 7 && !musicStarted)
				{
					MediaPlayer.Stop();
					MediaPlayer.Play(finalBossSong);
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
				if (BossCreated && !bhEnemy.Dead)
				{
					bhEnemy.Draw(gameTime, spriteBatch);
					bflEnemy.Draw(gameTime, spriteBatch);
					bfrEnemy.Draw(gameTime, spriteBatch);
				}
			}
			spriteBatch.End();

			spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap, null, null, null, camera.GetTransformation());
			{
				spriteBatch.Draw(debugDotTexture, new Rectangle((int)(camera.Position.X - 385), 15, (int)(player._Mana.mana * 2), 10), Color.Red);

				if (BossCreated && !bhEnemy.Dead)
				{
					spriteBatch.Draw(debugDotTexture, new Rectangle((int)((camera.Position.X + (myGame.WindowSize.X / 4)) - (27 * 2)), 15, (int)bhEnemy.Lives * 8, 10), Color.Purple);
				}
			}
			spriteBatch.End();

			// Debug Rectangles
			/*
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
				spriteBatch.Draw(debugDotTexture, new Rectangle((int)bhEnemy.playerCollisions.X, (int)bhEnemy.playerCollisions.Y, (int)bhEnemy.playerCollisions.Width, 1), Color.Blue);
				spriteBatch.Draw(debugDotTexture, new Rectangle((int)bhEnemy.playerCollisions.X + (int)bhEnemy.playerCollisions.Width, (int)bhEnemy.playerCollisions.Y, 1, (int)bhEnemy.playerCollisions.Height), Color.Red);
				spriteBatch.Draw(debugDotTexture, new Rectangle((int)bhEnemy.playerCollisions.X, (int)bhEnemy.playerCollisions.Y + (int)bhEnemy.playerCollisions.Height, (int)bhEnemy.playerCollisions.Width, 1), Color.Green);
				spriteBatch.Draw(debugDotTexture, new Rectangle((int)bhEnemy.playerCollisions.X, (int)bhEnemy.playerCollisions.Y, 1, (int)bhEnemy.playerCollisions.Height), Color.Yellow);
				spriteBatch.Draw(debugDotTexture, new Rectangle((int)bfrEnemy.playerCollisions.X, (int)bfrEnemy.playerCollisions.Y, (int)bfrEnemy.playerCollisions.Width, 1), Color.Blue);
				spriteBatch.Draw(debugDotTexture, new Rectangle((int)bfrEnemy.playerCollisions.X + (int)bfrEnemy.playerCollisions.Width, (int)bfrEnemy.playerCollisions.Y, 1, (int)bfrEnemy.playerCollisions.Height), Color.Red);
				spriteBatch.Draw(debugDotTexture, new Rectangle((int)bfrEnemy.playerCollisions.X, (int)bfrEnemy.playerCollisions.Y + (int)bfrEnemy.playerCollisions.Height, (int)bfrEnemy.playerCollisions.Width, 1), Color.Green);
				spriteBatch.Draw(debugDotTexture, new Rectangle((int)bfrEnemy.playerCollisions.X, (int)bfrEnemy.playerCollisions.Y, 1, (int)bfrEnemy.playerCollisions.Height), Color.Yellow);
				spriteBatch.Draw(debugDotTexture, new Rectangle((int)bflEnemy.playerCollisions.X, (int)bflEnemy.playerCollisions.Y, (int)bflEnemy.playerCollisions.Width, 1), Color.Blue);
				spriteBatch.Draw(debugDotTexture, new Rectangle((int)bflEnemy.playerCollisions.X + (int)bflEnemy.playerCollisions.Width, (int)bflEnemy.playerCollisions.Y, 1, (int)bflEnemy.playerCollisions.Height), Color.Red);
				spriteBatch.Draw(debugDotTexture, new Rectangle((int)bflEnemy.playerCollisions.X, (int)bflEnemy.playerCollisions.Y + (int)bflEnemy.playerCollisions.Height, (int)bflEnemy.playerCollisions.Width, 1), Color.Green);
				spriteBatch.Draw(debugDotTexture, new Rectangle((int)bflEnemy.playerCollisions.X, (int)bflEnemy.playerCollisions.Y, 1, (int)bflEnemy.playerCollisions.Height), Color.Yellow);
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
			*/

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
				case 7:
					brickspawn = MapHelper.GetTileArray(Maps.Boss());
					height = Maps.Boss().Count;
					width = Maps.Boss()[0].Length;
					break;
			}

			BossCreated = false;

			camera.Size = new Point(width * 25, height * 25);

			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					if (brickspawn[x, y] > 0 && brickspawn[x, y] < 75)
					{
						platformList.Add(new PlatformManager(new Vector2(x * 25, y * 25), myGame, (int)brickspawn[x, y], platformAnimationSetList));
					}

					if (brickspawn[x, y] == 75)
					{
						bhEnemy.SetPosition(new Vector2((float)Math.Abs((x * 25) - (bflEnemy.CurrentAnimation.frameSize.X / 2)), (float)Math.Abs((y * 25) - (bflEnemy.CurrentAnimation.frameSize.Y / 2))));
						BossCreated = true;
					}
					if (brickspawn[x, y] == 76)
					{
						bflEnemy.SetPosition(new Vector2((x * 25) - (bflEnemy.CurrentAnimation.frameSize.X / 4), (y * 25) - (bflEnemy.CurrentAnimation.frameSize.Y / 4)));
						BossCreated = true;
					}
					if (brickspawn[x, y] == 77)
					{
						bfrEnemy.SetPosition(new Vector2((x * 25) - (bfrEnemy.CurrentAnimation.frameSize.X / 4), (y * 25) - (bfrEnemy.CurrentAnimation.frameSize.Y / 4)));
						BossCreated = true;
					}
					if (brickspawn[x, y] == 78)
					{
						player.SetPosition(new Vector2(x * 25, y * 25));
						camera.Position = new Vector2(player.GetPosition.X + 200f, player.GetPosition.Y);
					}
					if (brickspawn[x, y] == 79)
					{
						cEnemyList.Add(new Enemy(new Vector2(x * 25, y * 25), 1.75f, Enemy.MovementType.HORIZONTAL, Color.White, cEnemyAnimationSetList, player, platformRectangles, mapSegments));
					}
					if (brickspawn[x, y] == 80)
					{
						sEnemyList.Add(new Enemy(new Vector2(x * 25, y * 25), 1.75f, Enemy.MovementType.BOUNCE, Color.White, sEnemyAnimationSetList, player, platformRectangles, mapSegments));
					}
					if (brickspawn[x, y] == 81)
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