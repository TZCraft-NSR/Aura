using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
	public class Player : Sprite
	{
		/// <summary>
		/// The mana struct for the player class.
		/// </summary>
		public struct Mana
		{
			/// <summary>
			/// The amount of mana the player has.
			/// </summary>
			public float mana;
			/// <summary>
			/// The maxium mana the player can have.
			/// </summary>
			public float maxMana;
			/// <summary>
			/// The ammount of time the mana recharges after.
			/// </summary>
			public float manaRechargeTime;
			/// <summary>
			/// The ammount of time the mana takes to charge at.
			/// </summary>
			public float manaInterval;

			/// <summary>
			/// Creates the Mana struct.
			/// </summary>
			/// <param name="maxMana">The maxium ammount of mana.</param>
			/// <param name="manaRechargeTime">The ammout of time the mana recharges after.</param>
			/// <param name="manaInterval">The ammount of time the mana takes to charge at.</param>
			public Mana(float maxMana, float manaRechargeTime, float manaInterval)
			{
				this.mana = maxMana;
				this.maxMana = maxMana;
				this.manaRechargeTime = manaRechargeTime;
				this.manaInterval = manaInterval;
			}
		}

		/// <summary>
		/// Gets or sets the game that the player is running off of.
		/// </summary>
		public Game1 myGame;

		#region Movement and Collision
		/// <summary>
		/// The player's collisions.
		/// </summary>
		public Rectangle playerCollisions;
		/// <summary>
		/// Gets or sets if the player is jumping. (Used for player or enemy classes)
		/// </summary>
		public bool isJumping
		{
			get;
			protected set;
		}
		/// <summary>
		/// Gets or sets if the player is grounded.
		/// </summary>
		public bool isGrounded
		{
			get;
			protected set;
		}
		/// <summary>
		/// Gets or sets if the sprite is falling.
		/// </summary>
		public bool isFalling
		{
			get;
			protected set;
		}
		/// <summary>
		/// Gets or sets if the sprite can fall.
		/// </summary>
		public bool canFall
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets the players gravity.
		/// </summary>
		public float GravityForce
		{
			get;
			protected set;
		}
		/// <summary>
		/// Gets or sets the default players gravity.
		/// </summary>
		public float DefaultGravityForce
		{
			get;
			set;
		}
		#endregion

		#region Projectiles
		/// <summary>
		/// The mana for the player class.
		/// </summary>
		public Mana _Mana;
		/// <summary>
		/// Gets or sets the projectiles animation sets.
		/// </summary>
		protected List<Sprite.AnimationSet> ProjectileAnimationSet
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets the list of projectiles.
		/// </summary>
		protected List<Projectile> ProjectileList
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets if the projectile list is created.
		/// </summary>
		public bool ProjectileListCreated
		{
			get;
			protected set;
		}
		/// <summary>
		/// Gets or sets if the player has shot.
		/// </summary>
		public bool HasShotProjectile
		{
			get;
			protected set;
		}
		/// <summary>
		/// Gets or sets if the player can shoot.
		/// </summary>
		public bool CanShootProjectile
		{
			get;
			protected set;
		}
		/// <summary>
		/// Gets or sets if a new projectile can be created.
		/// </summary>
		public bool CreateNewProjectile
		{
			get;
			protected set;
		}
		#endregion

		/// <summary>
		/// Creates the player.
		/// </summary>
		/// <param name="position">The position the player starts at.</param>
		/// <param name="movementKeys">The keys to control the player.</param>
		/// <param name="gravity">The gravity of tha player.</param>
		/// <param name="mana">The maximum mana for the players projectiles.</param>
		/// <param name="color">The color to mask the player sprite with.</param>
		/// <param name="animationSetList">The animation set list for the player.</param>
		/// <param name="game">The game that the player runs off of.</param>
		public Player(Vector2 position, List<Keys> movementKeys, float gravity, Mana mana, Color color, List<AnimationSet> animationSetList, Game1 game)
			: base(position, color, animationSetList)
		{
			myGame = game;
			Scale = 0.34f;

			#region Set Animation Factors
			Offset = new Vector2(20, 5);
			#endregion

			#region Set Projectile Factors
			ProjectileList = new List<Projectile>();
			ProjectileAnimationSet = new List<AnimationSet>();
			_Mana = mana;
			CanShootProjectile = true;
			CreateNewProjectile = true;
			#endregion

			#region Set Movement and Collision Factors
			MovementKeys = movementKeys;
			Speed = 2;
			GravityForce = gravity;
			DefaultGravityForce = gravity;
			SetAnimation("IDLE");
			isFalling = true;
			playerCollisions = new Rectangle((int)Position.X, (int)Position.Y, 20, 43);
			#endregion
		}

		/// <summary>
		/// Creates the player class with the bare minimum.
		/// Used for making child class for the player.
		/// Projectile lists created already.
		/// </summary>
		/// <param name="position">The position to start the player at.</param>
		/// <param name="color">The color to mask the player with.</param>
		/// <param name="animationSetList">The animation set list for the player.</param>
		public Player(Vector2 position, Color color, List<AnimationSet> animationSetList)
			: base(position, color, animationSetList)
		{
			ProjectileList = new List<Projectile>();
			ProjectileAnimationSet = new List<AnimationSet>();
		}

		/// <summary>
		/// Updates the Player class
		/// </summary>
		/// <param name="gameTime">To keep track of run time.</param>
		public override void Update(GameTime gameTime)
		{
			#region Updating Player Collision Points.
			playerCollisions.X = (int)Position.X;
			playerCollisions.Y = (int)Position.Y;
			#endregion

			#region Movement
			#region Detect Keys
			InputMethod(MovementKeys);
			#endregion

			#region Detect Collision
			foreach (Rectangle r in myGame.gameManager.platformRectangles)
			{
				CheckCollision(playerCollisions, r);
			}
			foreach (Rectangle r in myGame.gameManager.mapSegments)
			{
				CheckCollision(playerCollisions, r);
			}
			#endregion

			UpdateGravity();
			#endregion

			#region Do Projectiles
			if (!ProjectileListCreated)
			{
				ProjectileAnimationSet.Add(new AnimationSet("IDLE", myGame.gameManager.ProjectileTexture, new Point(25, 25), new Point(1, 1), new Point(0, 0), 0));

				if (ProjectileAnimationSet != null && ProjectileList != null)
				{
					ProjectileListCreated = true;
				}
			}

			foreach (Projectile p in ProjectileList)
			{
				p.Update(gameTime);
			}
			#endregion

			#region Do Animations
			if (isJumping || (isFalling && Direction.Y < DefaultGravityForce))
			{
				SetAnimation("JUMP");
			}
			if (CurrentAnimation.name == "IDLE")
			{
				Offset = new Vector2(10, 5);
			}
			if (CurrentAnimation.name == "SHOOT")
			{
				Offset = new Vector2(10, 5);
			}
			if (CurrentAnimation.name == "JUMP")
			{
				if (isFlipped)
				{
					Offset = new Vector2(15, 5);
				}
				else
				{
					Offset = new Vector2(25, 5);
				}
			}
			if (CurrentAnimation.name == "WALK")
			{
				if (isFlipped)
				{
					Offset = new Vector2(0, 5);
				}
				else
				{
					Offset = new Vector2(13, 5);
				}
			}
			#endregion

			#region Mana
			if (_Mana.mana < _Mana.maxMana)
			{
				if (!HasShotProjectile)
				{
					_Mana.manaRechargeTime -= myGame.elapsedTime;
				}

				if (_Mana.mana <= 0)
				{
					CanShootProjectile = false;
				}
				else if (_Mana.mana >= 0)
				{
					CanShootProjectile = true;
				}

				if (_Mana.manaRechargeTime <= 0 && _Mana.mana < _Mana.maxMana && !HasShotProjectile)
				{
					_Mana.manaInterval -= myGame.elapsedTime;

					if (_Mana.manaInterval <= 0)
					{
						_Mana.mana += 9.5f;
						_Mana.manaInterval = 500;
					}
				}

				if (_Mana.mana >= _Mana.maxMana || (HasShotProjectile && CanShootProjectile))
				{
					_Mana.manaRechargeTime = 5000;
				}

				if (_Mana.mana > _Mana.maxMana)
				{
					_Mana.mana = _Mana.maxMana;
				}
			}
			#endregion

			base.Update(gameTime);

			Position += Direction;
		}

		/// <summary>
		/// To draw the Player class.
		/// </summary>
		/// <param name="gameTime">To keep track of run time.</param>
		/// <param name="spriteBatch">The spriteBatch to draw with.</param>
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			foreach (Projectile p in ProjectileList)
			{
				p.Draw(gameTime, spriteBatch);
			}

			base.Draw(gameTime, spriteBatch);
		}

		/// <summary>
		/// Returns the player collision rectangle
		/// </summary>
		/// <returns>Rectangle</returns>
		public virtual Rectangle GetPlayerRectangles()
		{
			return playerCollisions;
		}

		/// <summary>
		/// Makes the player shoot a projectile.
		/// </summary>
		/// <param name="shootFactor">The number of projectiles the player can shoot.</param>
		public void ShootBeam(float shootFactor)
		{
			foreach (Projectile p in ProjectileList)
			{
				if (!p.visible)
				{
					CreateNewProjectile = true;
					ProjectileList.RemoveAt(0);
					break;
				}
			}

			if (CreateNewProjectile && CanShootProjectile && _Mana.mana >= _Mana.maxMana / shootFactor - 1)
			{
				_Mana.mana -= _Mana.maxMana / shootFactor;
				Projectile projectile = new Projectile(new Vector2(Position.X - 5, Position.Y - 10), Color.White, ProjectileAnimationSet, this, myGame);
				ProjectileList.Add(projectile);
				projectile.Fire();
				HasShotProjectile = true;
			}

			if (_Mana.mana < 0)
			{
				_Mana.mana = 0;
			}
		}

		/// <summary>
		/// Updates the inputs for the player class.
		/// </summary>
		/// <param name="keyList">The key list to update the input method with, (for custom lists)</param>
		protected virtual void InputMethod(List<Keys> keyList)
		{
			if (myGame.keyboardState.IsKeyDown(keyList[4]) && (!isJumping && !canFall))
			{
				isJumping = true;
				isFalling = false;
				Direction.Y = 0;
				Position.Y -= GravityForce * 1.5f;
			}
			if (myGame.keyboardState.IsKeyDown(keyList[0]))
			{
				Direction.X = -Speed;
				SetAnimation("WALK");
				FlipSprite(Axis.Y);
				isMoving = true;
			}
			if (myGame.keyboardState.IsKeyDown(keyList[2]))
			{
				Direction.X = Speed;
				SetAnimation("WALK");
				FlipSprite(Axis.NONE);
				isMoving = true;
			}
			if (!myGame.keyboardState.IsKeyDown(keyList[0]) && !myGame.keyboardState.IsKeyDown(keyList[2]))
			{
				Direction.X = 0f;
				isMoving = false;

				if (isGrounded && (CurrentAnimation.name != "SHOOT"))
				{
					SetAnimation("IDLE");
				}
			}
			if (!myGame.CheckKey(keyList[5]))
			{
				HasShotProjectile = false;
			}
			if (myGame.CheckKey(MovementKeys[5]))
			{
				if (CanShootProjectile && !isMoving && !isJumping && !canFall)
				{
					SetAnimation("SHOOT");
					ShootBeam(3);
				}
			}
		}

		/// <summary>
		/// To update the collision of the player against areas.
		/// </summary>
		/// <param name="rectangle1">Use this with only the players collision area.</param>
		/// <param name="rectangle2">Use this with what the player will collide with.</param>
		protected virtual void CheckCollision(Rectangle rectangle1, Rectangle rectangle2)
		{
			if (rectangle1.TouchTopOf(rectangle2))
			{
				Position.Y = rectangle2.Top - rectangle1.Height;
				Direction.Y = 0f;
				isGrounded = true;
				canFall = false;
				GravityForce = DefaultGravityForce;
			}
			if (rectangle1.TouchLeftOf(rectangle2))
			{
				Position.X = rectangle2.Left - rectangle1.Width;
			}
			if (rectangle1.TouchRightOf(rectangle2))
			{
				Position.X = rectangle2.Right;
			}
			if (rectangle1.TouchBottomOf(rectangle2))
			{
				Direction.Y = 0f;
				isJumping = false;
				canFall = true;
				isFalling = true;
			}
		}

		/// <summary>
		/// Updates gravity of the player.
		/// </summary>
		protected virtual void UpdateGravity()
		{
			if (isJumping)
			{
				if (GravityForce > 0f)
				{
					Direction.Y = -GravityForce;
					GravityForce -= 0.03f;
				}
				if (GravityForce <= 0f)
				{
					isJumping = false;
					isFalling = true;
					canFall = true;
				}
			}

			if (isFalling)
			{
				Direction.Y = GravityForce;
				GravityForce += 0.06f;
			}

			GravityForce = MathHelper.Clamp(GravityForce, -GravityForce - 1f, GravityForce);
		}
	}
}