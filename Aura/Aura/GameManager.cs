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
		public SpriteFont wintext;
		bool musicStarted;
		int timer = 10000;

		#region Player Variables
		/// <summary>
		/// The player class.
		/// </summary>
		public Player player;
		/// <summary>
		/// The player class' texture.
		/// </summary>
		Texture2D playerTexture;
		/// <summary>
		/// The animation list for the player.
		/// </summary>
		List<Sprite.AnimationSet> playerAnimationSetList;
		/// <summary>
		/// The player segment collisions.
		/// </summary>
		protected Rectangle PlayerCollisions;
		/// <summary>
		/// The players list of movement keys.
		/// </summary>
		Keys[,] MovementKeys = new Keys[2, 15];
		/// <summary>
		/// The players projectile texture
		/// </summary>
		public Texture2D ProjectileTexture;
		/// <summary>
		/// 
		/// </summary>
		public List<Sprite.AnimationSet> ProjectileAnimationSet;
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
		public List<Tile> TileList
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
		List<Sprite.AnimationSet> TileAnimationSet
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
		public List<Rectangle> mapBoundries
		{
			get;
			set;
		}
		/// <summary>
		/// The current level id.
		/// </summary>
		public int Level
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets if the level has been set.
		/// </summary>
		public bool LevelLoaded
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets if the player won the current level.
		/// </summary>
		public bool WonLevel
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
		public List<Enemy> EnemyList;
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
			playerAnimationSetList = new List<Sprite.AnimationSet>();

			ProjectileAnimationSet = new List<Sprite.AnimationSet>();

			EnemyList = new List<Enemy>();
			cEnemyAnimationSetList = new List<Sprite.AnimationSet>();
			sEnemyAnimationSetList = new List<Sprite.AnimationSet>();
			tEnemyAnimationSetList = new List<Sprite.AnimationSet>();

			bflEnemyAnimationSetList = new List<Sprite.AnimationSet>();
			bfrEnemyAnimationSetList = new List<Sprite.AnimationSet>();
			bhEnemyAnimationSetList = new List<Sprite.AnimationSet>();

			TileList = new List<Tile>();
			TileAnimationSet = new List<Sprite.AnimationSet>();
			platformRectangles = new List<Rectangle>();

			mapBoundries = new List<Rectangle>();

			Level = 2;

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
			wintext = Game.Content.Load<SpriteFont>(@"fonts\segoeuiregularlarge");

			camera = new Camera(GraphicsDevice.Viewport, new Point(6400, 450), 1f);
			camera.Position = new Vector2(0, 0);

			mapBoundries.Add(new Rectangle(-5, 0, 5, (int)camera.Size.Y));
			mapBoundries.Add(new Rectangle((int)camera.Size.X, 0, 5, (int)camera.Size.Y));

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

			playerAnimationSetList.Add(new Sprite.AnimationSet("IDLE1", playerTexture, new Point(124, 148), new Point(4, 1), new Point(0, 2), 1600, true));
			playerAnimationSetList.Add(new Sprite.AnimationSet("WALK1", playerTexture, new Point(100, 140), new Point(8, 1), new Point(0, 476), 100, true));
			playerAnimationSetList.Add(new Sprite.AnimationSet("JUMP1", playerTexture, new Point(187, 174), new Point(4, 1), new Point(0, 302), 1600, true));
			playerAnimationSetList.Add(new Sprite.AnimationSet("ATK-1", playerTexture, new Point(124, 148), new Point(5, 1), new Point(0, 154), 1600, true));

			cEnemyAnimationSetList.Add(new Sprite.AnimationSet("IDLE1", cEnemyTexture, new Point(25, 25), new Point(1, 1), new Point(0, 0), 1600, true));
			cEnemyAnimationSetList.Add(new Sprite.AnimationSet("WALK1", cEnemyTexture, new Point(25, 25), new Point(9, 1), new Point(25, 0), 100, true));
			//Needs rest of animations ^
			sEnemyAnimationSetList.Add(new Sprite.AnimationSet("IDLE1", sEnemyTexture, new Point(25, 25), new Point(1, 1), new Point(0, 0), 1600, true));
			sEnemyAnimationSetList.Add(new Sprite.AnimationSet("WALK1", sEnemyTexture, new Point(25, 25), new Point(5, 1), new Point(0, 0), 100, true));
			sEnemyAnimationSetList.Add(new Sprite.AnimationSet("FALLING", sEnemyTexture, new Point(25, 25), new Point(4, 1), new Point(150, 0), 100, true));
			//Needs edit ^
			tEnemyAnimationSetList.Add(new Sprite.AnimationSet("IDLE1", tEnemyTexture, new Point(25, 25), new Point(1, 1), new Point(0, 0), 1000, false));
			//Needs edit ^

			bflEnemyAnimationSetList.Add(new Sprite.AnimationSet("IDLE1", bflEnemyTexture, new Point(88, 100), new Point(1, 1), new Point(0, 0), 1600, false));
			bfrEnemyAnimationSetList.Add(new Sprite.AnimationSet("IDLE1", bfrEnemyTexture, new Point(88, 100), new Point(1, 1), new Point(0, 0), 1600, false));
			bflEnemyAnimationSetList.Add(new Sprite.AnimationSet("WALK1", bflEnemyTexture, new Point(88, 100), new Point(1, 1), new Point(0, 0), 100, false));
			bfrEnemyAnimationSetList.Add(new Sprite.AnimationSet("WALK1", bfrEnemyTexture, new Point(88, 100), new Point(1, 1), new Point(0, 0), 100, false));
			bflEnemyAnimationSetList.Add(new Sprite.AnimationSet("FALLING", bflEnemyTexture, new Point(88, 100), new Point(1, 1), new Point(0, 0), 100, false));
			bfrEnemyAnimationSetList.Add(new Sprite.AnimationSet("FALLING", bfrEnemyTexture, new Point(88, 100), new Point(1, 1), new Point(0, 0), 100, false));
			bhEnemyAnimationSetList.Add(new Sprite.AnimationSet("IDLE1", bhEnemyTexture, new Point(180, 242), new Point(1, 1), new Point(0, 0), 1600, false));
			bhEnemyAnimationSetList.Add(new Sprite.AnimationSet("IDLE2", bhEnemyTexture, new Point(180, 242), new Point(1, 1), new Point(360, 0), 1600, false));
			bhEnemyAnimationSetList.Add(new Sprite.AnimationSet("IDLE3", bhEnemyTexture, new Point(180, 242), new Point(1, 1), new Point(720, 0), 1600, false));

			TileAnimationSet.Add(new Sprite.AnimationSet("1", plainsPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(0, 0), 0, false));
			TileAnimationSet.Add(new Sprite.AnimationSet("2", plainsPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(25, 0), 0, false));
			TileAnimationSet.Add(new Sprite.AnimationSet("3", plainsPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(50, 0), 0, false));
			TileAnimationSet.Add(new Sprite.AnimationSet("4", plainsPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(75, 0), 0, false));
			TileAnimationSet.Add(new Sprite.AnimationSet("5", plainsPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(0, 25), 0, false));
			TileAnimationSet.Add(new Sprite.AnimationSet("6", plainsPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(25, 25), 0, false));
			TileAnimationSet.Add(new Sprite.AnimationSet("7", plainsPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(50, 25), 0, false));
			TileAnimationSet.Add(new Sprite.AnimationSet("8", plainsPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(75, 25), 0, false));
			TileAnimationSet.Add(new Sprite.AnimationSet("9", plainsPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(0, 50), 0, false));
			TileAnimationSet.Add(new Sprite.AnimationSet("10", plainsPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(25, 50), 0, false));
			TileAnimationSet.Add(new Sprite.AnimationSet("11", plainsPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(50, 50), 0, false));
			TileAnimationSet.Add(new Sprite.AnimationSet("12", plainsPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(75, 50), 0, false));
			TileAnimationSet.Add(new Sprite.AnimationSet("13", plainsPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(0, 75), 0, false));
			TileAnimationSet.Add(new Sprite.AnimationSet("14", plainsPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(25, 75), 0, false));
			TileAnimationSet.Add(new Sprite.AnimationSet("15", plainsPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(50, 75), 0, false));
			TileAnimationSet.Add(new Sprite.AnimationSet("16", plainsPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(75, 75), 0, false));
			TileAnimationSet.Add(new Sprite.AnimationSet("17", forestPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(0, 0), 0, false));
			TileAnimationSet.Add(new Sprite.AnimationSet("18", forestPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(25, 0), 0, false));
			TileAnimationSet.Add(new Sprite.AnimationSet("19", forestPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(50, 0), 0, false));
			TileAnimationSet.Add(new Sprite.AnimationSet("20", forestPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(75, 0), 0, false));
			TileAnimationSet.Add(new Sprite.AnimationSet("21", forestPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(0, 25), 0, false));
			TileAnimationSet.Add(new Sprite.AnimationSet("22", forestPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(25, 25), 0, false));
			TileAnimationSet.Add(new Sprite.AnimationSet("23", forestPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(50, 25), 0, false));
			TileAnimationSet.Add(new Sprite.AnimationSet("24", forestPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(75, 25), 0, false));
			TileAnimationSet.Add(new Sprite.AnimationSet("25", forestPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(0, 50), 0, false));
			TileAnimationSet.Add(new Sprite.AnimationSet("26", forestPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(25, 50), 0, false));
			TileAnimationSet.Add(new Sprite.AnimationSet("27", forestPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(50, 50), 0, false));
			TileAnimationSet.Add(new Sprite.AnimationSet("28", forestPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(75, 50), 0, false));
			TileAnimationSet.Add(new Sprite.AnimationSet("29", forestPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(0, 75), 0, false));
			TileAnimationSet.Add(new Sprite.AnimationSet("30", forestPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(25, 75), 0, false));
			TileAnimationSet.Add(new Sprite.AnimationSet("31", forestPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(50, 75), 0, false));
			TileAnimationSet.Add(new Sprite.AnimationSet("32", forestPlatformTexture, new Point(25, 25), new Point(1, 1), new Point(75, 75), 0, false));
			TileAnimationSet.Add(new Sprite.AnimationSet("33", cavePlatformTexture, new Point(25, 25), new Point(1, 1), new Point(0, 0), 0, false));
			TileAnimationSet.Add(new Sprite.AnimationSet("34", villagePlatformTexture, new Point(25, 25), new Point(1, 1), new Point(0, 0), 0, false));

			MovementKeys[0, 0] = Keys.A;
			MovementKeys[0, 1] = Keys.W;
			MovementKeys[0, 2] = Keys.D;
			MovementKeys[0, 3] = Keys.S;
			MovementKeys[0, 4] = Keys.Space;
			MovementKeys[1, 0] = Keys.None;
			MovementKeys[1, 1] = Keys.None;
			MovementKeys[1, 2] = Keys.None;
			MovementKeys[1, 3] = Keys.None;

			_Mana = new Player.Mana(100, 5000, 100, 20);
			player = new Player(new Vector2(25, (myGame.WindowSize.Y - playerAnimationSetList[0].frameSize.Y) - 75), MovementKeys, myGame.keyboardState, 3f, _Mana, Color.White, playerAnimationSetList, ProjectileAnimationSet);

			bhEnemy = new Enemy(new Vector2(0, 0), 0, Enemy.MovementType.BOSSHEAD, Color.White, bhEnemyAnimationSetList, player);
			bflEnemy = new Enemy(new Vector2(0, 0), 2.75f, Enemy.MovementType.BOSSBOUNCE, Color.White, bflEnemyAnimationSetList, player);
			bfrEnemy = new Enemy(new Vector2(0, 0), 2.75f, Enemy.MovementType.BOSSBOUNCE, Color.White, bfrEnemyAnimationSetList, player);

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
				//DebugLines[5] = "true";
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

			mapBoundries[1] = new Rectangle(camera.Size.X, mapBoundries[1].Y, mapBoundries[1].Width, mapBoundries[1].Height);

			if (myGame.CheckKey(Keys.G) && !WonLevel)
			{
				WonLevel = true;
			}

			if (WonLevel)
			{
				Level += 1;

				if (Level > 7)
				{
					Level = 2;
				}

				LevelLoaded = false;

				player.AddMana(20);

				if (!(Level > 2 && Level <= 3) && (Level != 6))
				{
					if (musicStarted)
					{
						MediaPlayer.Stop();
					}

					musicStarted = false;
				}

				WonLevel = false;
			}

			PlayerCollisions = player.BoundingCollisions;


			player.UpdateBossParameters(BossCreated);
			player.UpdateKeyboardState(gameTime, myGame.keyboardState);
			player.Update(gameTime, camera, EnemyList, TileList, mapBoundries, bflEnemy, bfrEnemy, bhEnemy);

			for (int i = 0; i < EnemyList.Count; i++)
			{
				if (EnemyList[i].DeleteMe)
				{
					EnemyList.RemoveAt(i);
					i--;
				}
				else
				{
					EnemyList[i].Update(gameTime, player, TileList, mapBoundries);
				}
			}
			if (BossCreated && !bhEnemy.isDead)
			{
				bflEnemy.Update(gameTime, player, TileList, mapBoundries);
				bfrEnemy.Update(gameTime, player, TileList, mapBoundries);
				bhEnemy.Update(gameTime, player, TileList, mapBoundries);
			}
			if (player.isDead)
			{
				LevelLoaded = false;
				WonLevel = false;
			}

			debugLabel.Update(gameTime, DebugLines[0] + "\n" + DebugLines[1] + "\n" +
										DebugLines[2] + "\n" + DebugLines[3] + "\n" +
										DebugLines[4] + "\n" + DebugLines[5] + "\n" +
										DebugLines[6] + "\n" + DebugLines[7] + "\n" +
										DebugLines[8] + "\n" + DebugLines[9]);

			DebugLines[0] = "Memory=" ;

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
				if (!LevelLoaded)
				{
					platformRectangles.RemoveRange(0, platformRectangles.Count);
					TileList.RemoveRange(0, TileList.Count);
					EnemyList.RemoveRange(0, EnemyList.Count);

					SpawnBricks(Level);

					LevelLoaded = true;
				}

				if (Level <= 2)
				{
					plainsParallax1Background.Draw(gameTime, spriteBatch);
					plainsParallax2Background.Draw(gameTime, spriteBatch);
					plainsParallax3Background.Draw(gameTime, spriteBatch);
				}
				if (Level == 3)
				{
					plainsParallax1Background.Draw(gameTime, spriteBatch);
					plainsParallax2Background.Draw(gameTime, spriteBatch);
					plainsParallax3Background.Draw(gameTime, spriteBatch);
					villageParallax1Background.Draw(gameTime, spriteBatch);
				}
				if (Level == 4)
				{
					forestParallax1Background.Draw(gameTime, spriteBatch);
					forestParallax2Background.Draw(gameTime, spriteBatch);
					forestParallax3Background.Draw(gameTime, spriteBatch);
				}
				if (Level >= 5)
				{
					caveParallax1Background.Draw(gameTime, spriteBatch);
					caveParallax2Background.Draw(gameTime, spriteBatch);
					caveParallax3Background.Draw(gameTime, spriteBatch);
				}
				if ((Level >= 1 && Level <= 3) && !musicStarted)
				{
					MediaPlayer.Stop();
					MediaPlayer.Play(plainsvillagesSong);
					musicStarted = true;
				}
				if (Level == 4 && !musicStarted)
				{
					MediaPlayer.Stop();
					MediaPlayer.Play(forestSong);
					musicStarted = true;
				}
				if ((Level >= 5 && Level <= 6) && !musicStarted)
				{
					MediaPlayer.Stop();
					MediaPlayer.Play(caveSong);
					musicStarted = true;
				}
				if (Level == 7 && !musicStarted)
				{
					MediaPlayer.Stop();
					MediaPlayer.Play(finalBossSong);
					musicStarted = true;
				}

				foreach (Tile t in TileList)
				{
					t.Draw(gameTime, spriteBatch);
				}
			}
			spriteBatch.End();

			// Draw the player and enemies.
			spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, camera.GetTransformation());
			{
				player.Draw(gameTime, spriteBatch);

				foreach (Enemy ce in EnemyList)
				{
					ce.Draw(gameTime, spriteBatch);
				}
				if (BossCreated && !bhEnemy.isDead)
				{
					bhEnemy.Draw(gameTime, spriteBatch);
					bflEnemy.Draw(gameTime, spriteBatch);
					bfrEnemy.Draw(gameTime, spriteBatch);
				}
			}
			spriteBatch.End();

			spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap, null, null);
			{
				spriteBatch.Draw(debugDotTexture, new Rectangle(15, 15, (int)(player.GetMana.mana * 2), 10), Color.Red);

				if (BossCreated && !bhEnemy.isDead)
				{
					spriteBatch.Draw(debugDotTexture, new Rectangle((int)myGame.WindowSize.X - (35 * 2), 15, (int)bhEnemy.HP * 8, 10), Color.Purple);
				}
			}
			spriteBatch.End();

			spriteBatch.Begin();
			{
				if (bhEnemy.isDead)
				{
					spriteBatch.DrawString(wintext, "Y O U  W I N ! ! !", new Vector2((myGame.WindowSize.X - wintext.MeasureString("Y O U  W I N ! ! !").X) / 2, 25), Color.OrangeRed);

					timer -= gameTime.ElapsedGameTime.Milliseconds;

					if (timer <= 0)
					{
						myGame.SetCurrentLevel(Game1.GameLevels.MENU);
						timer = 10000;
					}
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
					spriteBatch.Draw(debugDotTexture, new Rectangle((int)bhEnemy.BoundingCollisions.X, (int)bhEnemy.BoundingCollisions.Y, (int)bhEnemy.BoundingCollisions.Width, 1), Color.Blue);
					spriteBatch.Draw(debugDotTexture, new Rectangle((int)bhEnemy.BoundingCollisions.X + (int)bhEnemy.BoundingCollisions.Width, (int)bhEnemy.BoundingCollisions.Y, 1, (int)bhEnemy.BoundingCollisions.Height), Color.Red);
					spriteBatch.Draw(debugDotTexture, new Rectangle((int)bhEnemy.BoundingCollisions.X, (int)bhEnemy.BoundingCollisions.Y + (int)bhEnemy.BoundingCollisions.Height, (int)bhEnemy.BoundingCollisions.Width, 1), Color.Green);
					spriteBatch.Draw(debugDotTexture, new Rectangle((int)bhEnemy.BoundingCollisions.X, (int)bhEnemy.BoundingCollisions.Y, 1, (int)bhEnemy.BoundingCollisions.Height), Color.Yellow);
					spriteBatch.Draw(debugDotTexture, new Rectangle((int)bfrEnemy.BoundingCollisions.X, (int)bfrEnemy.BoundingCollisions.Y, (int)bfrEnemy.BoundingCollisions.Width, 1), Color.Blue);
					spriteBatch.Draw(debugDotTexture, new Rectangle((int)bfrEnemy.BoundingCollisions.X + (int)bfrEnemy.BoundingCollisions.Width, (int)bfrEnemy.BoundingCollisions.Y, 1, (int)bfrEnemy.BoundingCollisions.Height), Color.Red);
					spriteBatch.Draw(debugDotTexture, new Rectangle((int)bfrEnemy.BoundingCollisions.X, (int)bfrEnemy.BoundingCollisions.Y + (int)bfrEnemy.BoundingCollisions.Height, (int)bfrEnemy.BoundingCollisions.Width, 1), Color.Green);
					spriteBatch.Draw(debugDotTexture, new Rectangle((int)bfrEnemy.BoundingCollisions.X, (int)bfrEnemy.BoundingCollisions.Y, 1, (int)bfrEnemy.BoundingCollisions.Height), Color.Yellow);
					spriteBatch.Draw(debugDotTexture, new Rectangle((int)bflEnemy.BoundingCollisions.X, (int)bflEnemy.BoundingCollisions.Y, (int)bflEnemy.BoundingCollisions.Width, 1), Color.Blue);
					spriteBatch.Draw(debugDotTexture, new Rectangle((int)bflEnemy.BoundingCollisions.X + (int)bflEnemy.BoundingCollisions.Width, (int)bflEnemy.BoundingCollisions.Y, 1, (int)bflEnemy.BoundingCollisions.Height), Color.Red);
					spriteBatch.Draw(debugDotTexture, new Rectangle((int)bflEnemy.BoundingCollisions.X, (int)bflEnemy.BoundingCollisions.Y + (int)bflEnemy.BoundingCollisions.Height, (int)bflEnemy.BoundingCollisions.Width, 1), Color.Green);
					spriteBatch.Draw(debugDotTexture, new Rectangle((int)bflEnemy.BoundingCollisions.X, (int)bflEnemy.BoundingCollisions.Y, 1, (int)bflEnemy.BoundingCollisions.Height), Color.Yellow);
					for (int i = 0; i < EnemyList.Count; i++)
					{
						spriteBatch.Draw(debugDotTexture, new Rectangle((int)EnemyList[i].BoundingCollisions.X, (int)EnemyList[i].BoundingCollisions.Y, (int)EnemyList[i].BoundingCollisions.Width, 1), Color.Blue);
						spriteBatch.Draw(debugDotTexture, new Rectangle((int)EnemyList[i].BoundingCollisions.X + (int)EnemyList[i].BoundingCollisions.Width, (int)EnemyList[i].BoundingCollisions.Y, 1, (int)EnemyList[i].BoundingCollisions.Height), Color.Red);
						spriteBatch.Draw(debugDotTexture, new Rectangle((int)EnemyList[i].BoundingCollisions.X, (int)EnemyList[i].BoundingCollisions.Y + (int)EnemyList[i].BoundingCollisions.Height, (int)EnemyList[i].BoundingCollisions.Width, 1), Color.Green);
						spriteBatch.Draw(debugDotTexture, new Rectangle((int)EnemyList[i].BoundingCollisions.X, (int)EnemyList[i].BoundingCollisions.Y, 1, (int)EnemyList[i].BoundingCollisions.Height), Color.Yellow);
					}
					for (int i = 0; i < mapBoundries.Count; i++)
					{
						spriteBatch.Draw(debugDotTexture, new Rectangle((int)mapBoundries[i].X, (int)mapBoundries[i].Y, (int)mapBoundries[i].Width, 1), Color.Blue);
						spriteBatch.Draw(debugDotTexture, new Rectangle((int)mapBoundries[i].X + (int)mapBoundries[i].Width, (int)mapBoundries[i].Y, 1, (int)mapBoundries[i].Height), Color.Red);
						spriteBatch.Draw(debugDotTexture, new Rectangle((int)mapBoundries[i].X, (int)mapBoundries[i].Y + (int)mapBoundries[i].Height, (int)mapBoundries[i].Width, 1), Color.Green);
						spriteBatch.Draw(debugDotTexture, new Rectangle((int)mapBoundries[i].X, (int)mapBoundries[i].Y, 1, (int)mapBoundries[i].Height), Color.Yellow);
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
						TileList.Add(new Tile(new Vector2(x * 25, y * 25), (int)brickspawn[x, y], Tile.TileCollisions.Impassable, new Rectangle(x * 25, y * 25, 25, 25), Color.White, TileAnimationSet));
					}

					if (brickspawn[x, y] == 75)
					{
						bhEnemy.SetPosition = new Vector2((float)Math.Abs((x * 25) - (bflEnemy.CurrentAnimation.frameSize.X / 2)), (float)Math.Abs((y * 25) - (bflEnemy.CurrentAnimation.frameSize.Y / 2)));
						BossCreated = true;
					}
					if (brickspawn[x, y] == 76)
					{
						bflEnemy.SetPosition = new Vector2((x * 25) - (bflEnemy.CurrentAnimation.frameSize.X / 4), (y * 25) - (bflEnemy.CurrentAnimation.frameSize.Y / 4));
						BossCreated = true;
					}
					if (brickspawn[x, y] == 77)
					{
						bfrEnemy.SetPosition = new Vector2((x * 25) - (bfrEnemy.CurrentAnimation.frameSize.X / 4), (y * 25) - (bfrEnemy.CurrentAnimation.frameSize.Y / 4));
						BossCreated = true;
					}
					if (brickspawn[x, y] == 78)
					{
						player.SetPosition = new Vector2(x * 25, y * 25);
						camera.Position = new Vector2(player.GetPosition.X + 200f, player.GetPosition.Y);
					}
					if (brickspawn[x, y] == 79)
					{
						EnemyList.Add(new Enemy(new Vector2(x * 25, y * 25), 1.75f, Enemy.MovementType.HORIZONTAL, Color.White, cEnemyAnimationSetList, player));
					}
					if (brickspawn[x, y] == 80)
					{
						EnemyList.Add(new Enemy(new Vector2(x * 25, y * 25), 1.75f, Enemy.MovementType.BOUNCE, Color.White, sEnemyAnimationSetList, player));
					}
					if (brickspawn[x, y] == 81)
					{
						EnemyList.Add(new Enemy(new Vector2(x * 25 + 12.5f, y * 25 + 12.5f), 1.75f, Enemy.MovementType.FLY, Color.White, tEnemyAnimationSetList, player));
					}
				}
			}

			foreach (Tile pm in TileList)
			{
				platformRectangles.Add(new Rectangle((int)pm.GetPosition.X, (int)pm.GetPosition.Y + 3, 25, 22));
			}
		}
	}
}