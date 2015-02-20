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
		public Game1 myGame
		{
			get;
			private set;
		}
		/// <summary>
		/// The mana for the player class.
		/// </summary>
		protected Mana _Mana;

		protected Collision.Circle playerCircle;

		#region Projectiles
		/// <summary>
		/// Gets or sets the projectiles animation sets.
		/// </summary>
		protected List<Sprite.AnimationSet> ProjectileAnimationSet
		{
			get;
			private set;
		}
		/// <summary>
		/// Gets or sets the list of projectiles.
		/// </summary>
		protected List<Projectile> ProjectileList
		{
			get;
			private set;
		}
		/// <summary>
		/// Gets or sets if the projectile list is created.
		/// </summary>
		public bool ProjectileListCreated
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets if the player has shot.
		/// </summary>
		protected bool HasShot
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets if the player can shoot.
		/// </summary>
		protected bool CanShoot
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets if a new projectile can be created.
		/// </summary>
		protected bool CreateNew
		{
			get;
			set;
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
			//playerSegments = new List<Collision.MapSegment>();
			ProjectileList = new List<Projectile>();
			ProjectileAnimationSet = new List<AnimationSet>();
			#endregion

			#region Set Projectile Factors
			_Mana = mana;
			#endregion

			#region Set Movement Factors
			_MovementType = MovementType.PLATFORMER;
			_AIType = AIType.PLAYER;
			Gravity = gravity;
			myGame = game;
			MovementKeys = movementKeys;
			CanMove = true;
			Speed = 2;
			#endregion

			Direction = Vector2.Zero;
        }

		/// <summary>
		/// Creates a minimal player object.
		/// </summary>
		/// <param name="position">The starting position.</param>
		/// <param name="color">The color to mask with.</param>
		/// <param name="animationSetList">The animation set list.</param>
		public Player(Vector2 position, Color color, List<AnimationSet> animationSetList) : base(position, color, animationSetList)
		{
		}

		/// <summary>
		/// Updates the Player class
		/// </summary>
		/// <param name="gameTime">To keep track of run time.</param>
		public override void Update(GameTime gameTime)
		{
			Position += Direction * Speed;

			playerCircle.point.X = Position.X + ((CurrentAnimation.frameSize.X) / 2);
			playerCircle.point.Y = Position.Y + (CurrentAnimation.frameSize.Y - 10);
			playerCircle.radius = 10;

			isColliding1 = DetectTopSegmentCollision(playerCircle, myGame.gameManager.platformTopSegments);
			isColliding2 = DetectLeftSegmentCollision(playerCircle, myGame.gameManager.platformLeftSegments);
			isColliding3 = DetectRightSegmentCollision(playerCircle, myGame.gameManager.platformRightSegments);

            if (_KeyboardState.IsKeyDown(MovementKeys[0]) && !DetectLeftSegmentCollision(playerCircle, myGame.gameManager.platformLeftSegments))
            {
                Direction.X = -1;
            }
            if (_KeyboardState.IsKeyDown(MovementKeys[2]) && !DetectRightSegmentCollision(playerCircle, myGame.gameManager.platformRightSegments))
            {
                Direction.X = 1;
            }
            if (_KeyboardState.IsKeyDown(MovementKeys[4]) && !DetectTopSegmentCollision(playerCircle, myGame.gameManager.platformTopSegments))
            {
                BleedOff = Gravity;
                isJumping = true;
                isGrounded = false;
                isTouchingGround = false;
            }
            if (!_KeyboardState.IsKeyDown(MovementKeys[0]) && !_KeyboardState.IsKeyDown(MovementKeys[2]) && !_KeyboardState.IsKeyDown(MovementKeys[4]))
            {
                Direction.X = 0;
            }

            if (!DetectTopSegmentCollision(playerCircle, myGame.gameManager.platformTopSegments))
            {
                isGrounded = true;
                isTouchingGround = true;
            }
            if (DetectTopSegmentCollision(playerCircle, myGame.gameManager.platformTopSegments) && isGrounded && isTouchingGround)
            {
                isGrounded = false;
                isTouchingGround = false;
                isFalling = true;
            }

			if (ProjectileListCreated == false)
			{
				ProjectileAnimationSet.Add(new AnimationSet("IDLE", myGame.gameManager.projectileTexture, new Point(25, 25), new Point(1, 1), new Point(0, 0), 0));
				ProjectileListCreated = true;
            }

			foreach (Projectile p in ProjectileList)
			{
				p.Update(gameTime);
			}

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

		public override void UpdateMovement()
		{
			base.UpdateMovement();

			// To play the jumping animation.
			if (isJumping && !isFalling)
			{
				SetAnimation("JUMP");
			}

			// To do the walking animation.
			if ((myGame.keyboardState.IsKeyDown(MovementKeys[2]) || myGame.keyboardState.IsKeyDown(MovementKeys[0])) && isGrounded)
			{
				SetAnimation("WALK");
			}

			// To flip the player to the left.
			if (myGame.keyboardState.IsKeyDown(MovementKeys[0]))
			{
				flipSprite(true);
			}

			// To flip the player back to the right.
			if (myGame.keyboardState.IsKeyDown(MovementKeys[2]))
			{
				flipSprite(false);
			}

			if ((myGame.keyboardState.IsKeyDown(MovementKeys[5]) && !myGame.previousKeyboardState.IsKeyDown(MovementKeys[5])) && (!myGame.keyboardState.IsKeyDown(MovementKeys[0]) && !myGame.keyboardState.IsKeyDown(MovementKeys[2])))
			{
				if (CanShoot)
				{
					SetAnimation("SHOOT");
					ShootBeam();
				}
			}

			if ((myGame.keyboardState.IsKeyUp(MovementKeys[5]) && !myGame.previousKeyboardState.IsKeyUp(MovementKeys[5])))
			{
				HasShot = false;
			}

			// To set the animation to idle.
			if ((!myGame.keyboardState.IsKeyDown(MovementKeys[0]) && !myGame.keyboardState.IsKeyDown(MovementKeys[2]) && !myGame.keyboardState.IsKeyDown(MovementKeys[5])) || isFalling)
			{
				SetAnimation("IDLE");
			}

			if (_Mana.mana < _Mana.maxMana)
			{
				if (!HasShot)
				{
					_Mana.manaRechargeTime -= myGame.elapsedTime;
				}

				if (_Mana.mana <= 0)
				{
					CanShoot = false;
				}
				else if (_Mana.mana >= 0)
				{
					CanShoot = true;
				}

				if (_Mana.manaRechargeTime <= 0 && _Mana.mana < _Mana.maxMana && !HasShot)
				{
					_Mana.manaInterval -= myGame.elapsedTime;

					if (_Mana.manaInterval <= 0)
					{
						_Mana.mana += 9.5f;
						_Mana.manaInterval = 500;
					}
				}

				if (_Mana.mana >= _Mana.maxMana || (HasShot && CanShoot))
				{
					_Mana.manaRechargeTime = 5000;
				}

				if (_Mana.mana > _Mana.maxMana)
				{
					_Mana.mana = _Mana.maxMana;
				}
			}
		}

		public Collision.Circle GetPlayerCircle()
		{
			return playerCircle;
		}

		public void ShootBeam()
		{
			foreach (Projectile p in ProjectileList)
			{
				if (!p.isVisible)
				{
					CreateNew = true;
					ProjectileList.RemoveAt(0);
					break;
				}
			}

			if (CreateNew == true && CanShoot && _Mana.mana >= _Mana.maxMana / 3)
			{
				if (CanShoot)
				{
					HasShot = true;
				}

				_Mana.mana -= _Mana.maxMana / 3;
				Projectile projectile = new Projectile(new Vector2(Position.X + 35, Position.Y + ((ProjectileAnimationSet[0].frameSize.Y - 4) / 2) + 8), Color.White, ProjectileAnimationSet, this, myGame);
				projectile.Fire();
				ProjectileList.Add(projectile);
			}

			if (_Mana.mana < 0)
			{
				_Mana.mana = 0;
			}
		}
	}
}