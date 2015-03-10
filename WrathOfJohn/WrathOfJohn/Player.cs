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
		protected Rectangle playerCollisions;
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
		/// Gets or sets if the player is touching the ground.
		/// </summary>
		public bool isTouchingGround
		{
			get;
			protected set;
		}
		/// <summary>
		/// Gets or sets if the sprite is falling. (Used for player or enemy classes)
		/// </summary>
		public bool isFalling
		{
			get;
			protected set;
		}
		/// <summary>
		/// Gets or sets if the player is touching left of a rectangle.
		/// </summary>
		public bool isTouchingLeft
		{
			get;
			protected set;
		}
		/// <summary>
		/// Gets or sets if the player is touching right of a rectangle.
		/// </summary>
		public bool isTouchingRight
		{
			get;
			protected set;
		}
		/// <summary>
		/// Gets or sets if the player is touching bottom of a rectangle.
		/// </summary>
		public bool isTouchingBottom
		{
			get;
			protected set;
		}
		/// <summary>
		/// Gets or sets the sprites bleed off of gravity. (Used for player or enemy classes)
		/// </summary>
		public float BleedOff
		{
			get;
			private set;
		}
		/// <summary>
		/// Gets or sets the sprites gravity. (Used for player or enemy classes)
		/// </summary>
		public float Gravity
		{
			get;
			protected set;
		}
        /// <summary>
        /// 
        /// </summary>
        public bool isTouchingLeftWall
        {
            get;
            protected set;
        }
        /// <summary>
        /// 
        /// </summary>
        public bool isTouchingRightWall
        {
            get;
            protected set;
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
		/// This is to create the Player class.
		/// </summary>
		/// <param name="texture">This is the player's texture</param>
		/// <param name="position">This sets the player's position</param>
		/// <param name="movementKeys">The list of keys the player uses.</param>
		/// <param name="gravity">The force of gravity.</param>
		/// <param name="color">The color to mask the sprite.</param>
		/// <param name="animationSetList">The set of animations the player has.</param>
		/// <param name="game">This is the Game class that the player runs on.</param>
		public Player(Vector2 position, List<Keys> movementKeys, float gravity, Mana mana, Color color, List<AnimationSet> animationSetList, Game1 game)
			: base(position, color, animationSetList)
		{
			#region Create Lists
			ProjectileList = new List<Projectile>();
			ProjectileAnimationSet = new List<AnimationSet>();
			#endregion

			#region Set Projectile Factors
			_Mana = mana;
			CanShootProjectile = true;
			CreateNewProjectile = true;
			#endregion

			#region Set Movement Factors
			Gravity = gravity;
			myGame = game;
			MovementKeys = movementKeys;
			CanMove = true;
			Speed = 2;
			BleedOff = 0;
			isTouchingGround = false;
			SetAnimation("IDLE");
			#endregion

			Direction = Vector2.Zero;

			playerCollisions = new Rectangle((int)Position.X + 20, (int)Position.Y + 5, 20, 43);
		}

		/// <summary>
		/// Creates a minimal player object.
		/// </summary>
		/// <param name="position">The starting position.</param>
		/// <param name="color">The color to mask with.</param>
		/// <param name="animationSetList">The animation set list.</param>
		public Player(Vector2 position, Color color, List<AnimationSet> animationSetList)
			: base(position, color, animationSetList)
		{
		}

		/// <summary>
		/// Updates the Player class
		/// </summary>
		/// <param name="gameTime">To keep track of run time.</param>
		public override void Update(GameTime gameTime)
		{
			#region Updating Player Collision Points.
			playerCollisions.X = (int)Position.X + 20;
			playerCollisions.Y = (int)Position.Y + 5;
			#endregion

			#region Movement
			Position += Direction * Speed;

			UpdateGravity();

			#region Detect Keys
			if (myGame.keyboardState.IsKeyDown(MovementKeys[0]))
			{
				Direction.X = -1;
				SetAnimation("WALK");
				FlipSprite(true);
			}
			if (myGame.keyboardState.IsKeyDown(MovementKeys[2]))
			{
				Direction.X = 1;
				SetAnimation("WALK");
				FlipSprite(false);
			}
			if (myGame.keyboardState.IsKeyDown(MovementKeys[4]) && isTouchingGround && !isJumping && !isFalling)
			{
				BleedOff = Gravity;
				isJumping = true;
			}
			if (isJumping || isFalling)
			{
				SetAnimation("JUMP");
			}
			if (!myGame.keyboardState.IsKeyDown(MovementKeys[0]) && !myGame.keyboardState.IsKeyDown(MovementKeys[2]))
			{
				Direction.X = 0;
			}
			if (!myGame.keyboardState.IsKeyDown(MovementKeys[0]) && !myGame.keyboardState.IsKeyDown(MovementKeys[2]) && isGrounded)
			{
				SetAnimation("IDLE");
			}
			if (myGame.CheckKey(MovementKeys[5]))
			{
				if (CanShootProjectile)
				{
					SetAnimation("SHOOT");
					ShootBeam();
				}
			}
			if (!myGame.CheckKey(MovementKeys[5]))
			{
				HasShotProjectile = false;
			}
			#endregion

			#region Detect Collision
			isGrounded = CheckSegmentCollision(playerCollisions, myGame.gameManager.platformRectangles, "top");
			isTouchingLeft = CheckSegmentCollision(playerCollisions, myGame.gameManager.platformRectangles, "left");
			isTouchingRight = CheckSegmentCollision(playerCollisions, myGame.gameManager.platformRectangles, "right");
			isTouchingBottom = CheckSegmentCollision(playerCollisions, myGame.gameManager.platformRectangles, "bottom");
            isTouchingLeftWall = CheckSegmentCollision(playerCollisions, myGame.gameManager.mapSegments, "left");
            isTouchingRightWall = CheckSegmentCollision(playerCollisions, myGame.gameManager.mapSegments, "right");

            if (isGrounded)
            {
                isTouchingGround = true;
            }
            if (!isGrounded)
            {
                isTouchingGround = false;
            }
            if (isGrounded && isFalling)
            {
                isFalling = false;
                BleedOff = 0;
                Direction.Y = 0;
            }
            if (!isGrounded && isTouchingGround)
            {
                isTouchingGround = false;
            }
            if (isTouchingLeft)
            {
                Direction.X = -0.01f;
            }
            if (isTouchingRight)
            {
                Direction.X = 0.01f;
            }
            if (isTouchingBottom)
            {
                Direction.Y = 0.4f;

                isFalling = true;
                isJumping = false;
            }
            foreach (Rectangle r in myGame.gameManager.mapSegments)
            {
                if (RectangleHelper.TouchLeftOf(playerCollisions, r))
                {
                    Position.X = r.Left;
                }
                if (RectangleHelper.TouchRightOf(playerCollisions, r))
                {
                    Position.X = r.Right;
                }
            }
			#endregion
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
		///
		/// </summary>
		/// <returns></returns>
		public Rectangle GetPlayerSegments()
		{
			return playerCollisions;
		}

		/// <summary>
		///
		/// </summary>
		public void ShootBeam()
		{
			foreach (Projectile p in ProjectileList)
			{
				if (!p.isVisible)
				{
					CreateNewProjectile = true;
					ProjectileList.RemoveAt(0);
					break;
				}
			}

			if (CreateNewProjectile && CanShootProjectile && _Mana.mana >= _Mana.maxMana / 3 - 1)
			{
				_Mana.mana -= _Mana.maxMana / 3;
				Projectile projectile = new Projectile(new Vector2(Position.X + 35, Position.Y + ((ProjectileAnimationSet[0].frameSize.Y - 4) / 2) + 8), Color.White, ProjectileAnimationSet, this, myGame);
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
		/// To update the player's gravity
		/// </summary>
		/// <param name="gameTime"></param>
		public void UpdateGravity()
		{
			if (isJumping || isFalling)
			{
				if (BleedOff > 0f && isJumping)
				{
					Direction.Y = -BleedOff;
					BleedOff -= 0.03f;
					isFalling = false;
				}

				if (BleedOff <= 0f)
				{
					isFalling = true;
					isJumping = false;
				}
				if (isFalling)
				{
					Direction.Y = -BleedOff;
					BleedOff -= 0.06f;
				}
			}

			if (!isTouchingGround)
			{
				if (!isGrounded)
				{
					isFalling = true;
				}
			}

			BleedOff = MathHelper.Clamp(BleedOff, -Gravity - 1f, Gravity);
		}

		/// <summary>
		/// This returns true if the player is colliding with the specified object.
		/// </summary>
		/// <param name="PlayerSegment">The player's rectangle collision.</param>
		/// <param name="ObjectSegments">The object the player is colliding with.</param>
		/// <returns></returns>
		public bool CheckSegmentCollision(Rectangle PlayerSegment, List<Rectangle> ObjectSegments, string Side)
		{
			foreach (Rectangle pts in ObjectSegments)
			{
				if (RectangleHelper.TouchTopOf(PlayerSegment, pts) && Side == "top")
				{
					//GetProtectedRectangleTop(pts);
					return true;
				}
				if (RectangleHelper.TouchLeftOf(PlayerSegment, pts) && Side == "left")
				{
					//GetProtectedRectangleLeft(pts);
					return true;
				}
				if (RectangleHelper.TouchRightOf(PlayerSegment, pts) && Side == "right")
				{
					//GetProtectedRectangleRight(pts);
					return true;
				}
				if (RectangleHelper.TouchBottomOf(PlayerSegment, pts) && Side == "bottom")
				{
					//GetProtectedRectangleBottom(pts);
					return true;
				}
			}

			return false;
		}
	}
}