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
		/// This is the Game class that the Player class is running off of.
		/// </summary>
		Game1 myGame;
		/// <summary>
		/// This is the animationSet list for the player.
		/// </summary>
		public List<Collision.MapSegment> playerSegments = new List<Collision.MapSegment>();
		float fireTime = 100;
		public float FireTime
		{
			get
			{
				return fireTime;
			}
		}
		bool hasShot = false;
		public bool HasShot
		{
			get
			{
				return hasShot;
			}
		}
		bool canShoot = true;
		public bool CanShoot
		{
			get
			{
				return canShoot;
			}
		}
		float mana = 100;
		float maxMana = 100;
		float manaRechargeTime = 5000;
		float manaInterval = 1000;
		List<Sprite.AnimationSet> projectileAnimationSet = new List<Sprite.AnimationSet>();
		List<Projectile> projectileList = new List<Projectile>();
		bool projectileListCreated = false;
		bool createNew = true;

		/// <summary>
		/// This is to create the Player class.
		/// </summary>
		/// <param name="texture">This is the player's texture</param>
		/// <param name="position">This sets the player's position</param>
		/// <param name="game">This is the Game class that the player runs on</param>
		/// <param name="left">The left key</param>
		/// <param name="right">The right key</param>
		/// <param name="jump">The jump key</param>
		/// <param name="attack"></param>
		/// <param name="gravity2">The force of gravity</param>
		/// <param name="color"></param>
		/// <param name="animationSetList">The set of animations</param>
		public Player(Texture2D texture, Vector2 position, Game1 game, Keys left, Keys right, Keys jump, Keys attack, float gravity2, Color color, List<AnimationSet> animationSetList)
			: base(position, color, animationSetList)
		{
			animationSets = animationSetList;
			this.color = color;
			movementType = MovementType.PLATFORMER;
			aiType = AIType.PLAYER;
			BleedOff = gravity2;
			Gravity = gravity2;
			myGame = game;
			MovementKeys.Add(left);
			MovementKeys.Add(Keys.None);
			MovementKeys.Add(right);
			MovementKeys.Add(Keys.None);
			MovementKeys.Add(jump);
			MovementKeys.Add(attack);
			canMove = true;
			speed = 1;
			ground = new Vector2(0, position.Y);

			playerSegments.Add(new Collision.MapSegment(new Point((int)position.X + 20, (int)position.Y + 5), new Point((int)position.X + 20, (int)position.Y + 49)));
			playerSegments.Add(new Collision.MapSegment(new Point((int)position.X + 40, (int)position.Y + 49), new Point((int)position.X + 40, (int)position.Y + 5)));
			playerSegments.Add(new Collision.MapSegment(new Point((int)position.X + 20, (int)position.Y + 49), new Point((int)position.X + 40, (int)position.Y + 49)));
			playerSegments.Add(new Collision.MapSegment(new Point((int)position.X + 40, (int)position.Y + 5), new Point((int)position.X + 20, (int)position.Y + 5)));
		}

		/// <summary>
		/// Updates the Player class
		/// </summary>
		/// <param name="gameTime">To keep track of run time.</param>
		public override void Update(GameTime gameTime)
		{
			playerSegments[0] = new Collision.MapSegment(new Point((int)position.X + 20, (int)position.Y + 5), new Point((int)position.X + 20, (int)position.Y + 48));
			playerSegments[1] = new Collision.MapSegment(new Point((int)position.X + 20, (int)position.Y + 5), new Point((int)position.X + 40, (int)position.Y + 5));
			playerSegments[2] = new Collision.MapSegment(new Point((int)position.X + 40, (int)position.Y + 5), new Point((int)position.X + 40, (int)position.Y + 48));
			playerSegments[3] = new Collision.MapSegment(new Point((int)position.X + 20, (int)position.Y + 48), new Point((int)position.X + 40, (int)position.Y + 48));

			if (projectileListCreated == false)
			{
				projectileAnimationSet.Add(new AnimationSet("IDLE", myGame.gameManager.projectileTexture, new Point(25, 25), new Point(1, 1), new Point(0, 0), 0));
				projectileListCreated = true;
			}

			foreach (Projectile p in projectileList)
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
			foreach (Projectile p in projectileList)
			{
				p.Draw(gameTime, spriteBatch);
			}

			base.Draw(gameTime, spriteBatch);
		}

		public override void UpdateMovement()
		{
			if (myGame.gameManager.mapSegments[0].point1.X >= playerSegments[0].point1.X)
			{
				position.X = myGame.gameManager.mapSegments[0].point1.X - ((currentAnimation.frameSize.X - 20) / 2);
			}
			if (myGame.gameManager.mapSegments[1].point1.Y >= playerSegments[1].point1.Y)
			{
				position.Y = myGame.gameManager.mapSegments[1].point1.Y - 48;
			}
			if (myGame.gameManager.mapSegments[2].point1.X <= playerSegments[2].point1.X)
			{
				position.X = myGame.gameManager.mapSegments[2].point1.X - ((currentAnimation.frameSize.X - 20));
			}
			if (myGame.gameManager.mapSegments[3].point1.Y <= playerSegments[3].point1.Y)
			{
				position.Y = myGame.gameManager.mapSegments[3].point1.Y - 48;
			}

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

				hasShot = false;
			}

			// To set the animation to idle.
			if ((!myGame.keyboardState.IsKeyDown(MovementKeys[0]) && !myGame.keyboardState.IsKeyDown(MovementKeys[2]) && !myGame.keyboardState.IsKeyDown(MovementKeys[5])) || isFalling)
			{
				SetAnimation("IDLE");
			}

			if (mana < maxMana)
			{
				if (!HasShot)
				{
					manaRechargeTime -= myGame.elapsedTime;
				}

				if (mana <= 0)
				{
					canShoot = false;
				}
				else if (mana >= 0)
				{
					canShoot = true;
				}

				if (manaRechargeTime <= 0 && mana < maxMana && !HasShot)
				{
					manaInterval -= myGame.elapsedTime;

					if (manaInterval <= 0)
					{
						mana += 9.5f;
						manaInterval = 500;
					}
				}

				if (mana >= maxMana || (HasShot && CanShoot))
				{
					manaRechargeTime = 5000;
				}

				if (mana > maxMana)
				{
					mana = maxMana;
				}
			}

			myGame.gameManager.firstLine10 = "mana=" + mana + " manaRechargeTime=" + manaRechargeTime + " HasShot=" + HasShot;
		}

		public List<Collision.MapSegment> getPlayerSgements()
		{
			return playerSegments;
		}

		public void ShootBeam()
		{
			foreach (Projectile p in projectileList)
			{
				if (!p.isVisible)
				{
					createNew = true;
					projectileList.RemoveAt(0);
					break;
				}
			}

			if (createNew == true && canShoot && mana >= maxMana / 3)
			{
				if (canShoot)
				{
					hasShot = true;
				}

				mana -= maxMana / 3;
				Projectile projectile = new Projectile(new Vector2(position.X + 35, position.Y + ((projectileAnimationSet[0].frameSize.Y - 4) / 2) + 8), Color.White, projectileAnimationSet, this, myGame);
				projectile.Fire();
				projectileList.Add(projectile);
			}

			if (mana < 0)
			{
				mana = 0;
			}
		}
	}
}