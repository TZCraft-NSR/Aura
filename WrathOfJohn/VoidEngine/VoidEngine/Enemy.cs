using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace WrathOfJohn
{
	public class Enemy : Player
	{
		public enum MovementType
		{
			FLY,
			HORIZONTAL,
			BOUNCE,
			NONE
		}

		Player _Player;
		
		Rectangle MapTiles;
		Rectangle MapBoundingBoxes;

		#region Enemy Variables
		/// <summary>
		/// Gets or sets the health of the enemy.
		/// </summary>
		public int HP
		{
			get;
			protected set;
		}
		/// <summary>
		/// Gets or sets the max health of the enemy.
		/// </summary>
		public int MaxHP
		{
			get;
			protected set;
		}

		/// <summary>
		/// Gets or sets the value to delete the enemy.
		/// </summary>
		public bool DeleteMe
		{
			get;
			protected set;
		}

		/// <summary>
		/// Gets or sets the value to move the enemy
		/// </summary>
		public bool MoveEnemy
		{
			get;
			protected set;
		}
		
		/// <summary>
		/// Gets or sets of the movment type of the enemy.
		/// </summary>
		public MovementType _MovementType
		{
			get;
			protected set;
		}
		#endregion

		public Enemy(Vector2 position, Color color, float gravity, float speed, MovementType movementType, List<AnimationSet> animationSetList, Player player, Rectangle mapTiles, Rectange mapBoundingBoxes)
			: base(position, color, animationSetList)
		{
			this.myGame = myGame;

			_Player = player;

			MapTiles = mapTiles;
			MapBoundingBoxes = mapBoundingBoxes;

			#region Set Animation Factors
			SetAnimation("IDLE");
			#endregion

			#region Set Movement and Collision Factors
			movementType = _MovementType;
			DefaultGravityForce = gravity;
			GravityForce = gravity;
			Speed = speed;
			Direction = Vector2.Zero;
			playerCollisions = new Rectangle((int)Position.X, (int)Position.Y, animationSetList[0].frameSize.X, animationSetList[0].frameSize.Y);
			#endregion
		}

		public override void Update(GameTime gameTime)
		{
			#region Update enemy collisions
			playerCollisions.X = (int)Position.X;
			playerCollisions.Y = (int)Position.Y;
			#endregion

			#region Update Directions
			if (_MovementType == MovementType.FLY)
			{
				Direction = new Vector2(_Player.GetPosition.X - Position.X, _Player.GetPosition.Y - Position.Y);
			}
			else if (_MovementType == MovementType.HORIZONTAL || _MovementType == MovementType.BOUNCE)
			{
				Direction = new Vector2(_PLayer.GetPosition.X - Position.X, Direction.Y);
			}
			#endregion

			#region Detected player
			if (Collision.Magnitude(Direction) <= 200)
			{
				Direction = Collision.UnitVector(Direction);
				
				#region Safe switch, if the player's x cord is the same as the enemys
				if (myGame.gameManager.player.GetPosition.X == Position.X)
				{
					if (_MovementType == MovementType.FLY)
					{
						Direction = new Vector2(0, Direction.Y);
					}
					if (_MovementType == MovementType.HORIZONTAL || _MovementType == MovementType.BOUNCE)
					{
						Direction = new Vector2(0, Direction.Y);
					}
				}
				#endregion
				#region Update collisions
				foreach (Rectangle r in MapTiles)
				{
					CheckCollision(playerCollisions, r);
				}
				foreach (Rectangle r in MapBoundingBoxes)
				{
					CheckCollision(playerCollisions, r);
				}
				#endregion
				#region Update Gravity
				if (_MovementType == MoveMentType.BOUNCE)
				{
					if (!isJumping && !canFall)
					{
						isJumping = true;
						isFalling = false;
						Direction.Y = 0;
						isJumping = true;
						Position.Y -= GravityForce * 1.5f;
					}
				}
				
				if (_MovementType == MovementType.HORIZONTAL || _MovementType == MovementType.BOUNCE)
				{
					UpdateGravity();
				}
				#endregion

				SetAnimation("CHASE");

				Position.X += Direction.X * Speed;
			}
			#endregion

			Position.Y += Direction.Y;

			SetAnimation("IDLE");
		}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			base.Draw(gameTime, spriteBatch);
		}

		public override void InputMethod(List<Keys> keyList) { }

		protected override void CheckCollision(Rectangle rectangle1, Rectangle rectangle2)
		{
 			base.CheckCollision(rectangle1, rectangle2);
		}

		protected override void UpdateGravity()
		{
			base.UpdateGravity();
		}
	}
}
