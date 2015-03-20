using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using VoidEngine;

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

        public int HP;

		public MovementType _MovementType;

		public Player _Player;

		public List<Rectangle> MapTiles;
		public List<Rectangle> MapSides;

        public bool DeleteMe = false;
        public bool MoveCircle = false;
        public bool DeleteCircle = false;

		public Enemy(Vector2 position, float gravity, MovementType movementType, Color color, List<AnimationSet> animationSetList, Player player, List<Rectangle> mapTiles, List<Rectangle> mapSides)
			: base(position, color, animationSetList)
		{
			this.myGame = myGame;
			SetAnimation("IDLE");

			if (movementType == MovementType.FLY)
			{
				RotationCenter = new Vector2(animationSetList[0].frameSize.X / 2, animationSetList[0].frameSize.Y / 2);
				Offset = new Vector2(-(animationSetList[0].frameSize.X / 2), -(animationSetList[0].frameSize.Y / 2));
			}

			#region Reset Gravity
			DefaultGravityForce = gravity;
			GravityForce = gravity;
			isFalling = true;
			canFall = true;
			Direction = Vector2.Zero;
			#endregion

			#region Set default variables
			MapSides = mapSides;
			MapTiles = mapTiles;
			_Player = player;
			_MovementType = movementType;
			#endregion

			ProjectileList = new List<Projectile>();
			playerCollisions = new Rectangle((int)Position.X, (int)Position.Y, animationSetList[0].frameSize.X, animationSetList[0].frameSize.Y);
		}

		public override void Update(GameTime gameTime)
		{
			playerCollisions.X = (int)Position.X;
			playerCollisions.Y = (int)Position.Y;

			if (_MovementType == MovementType.FLY)
			{
				Direction = new Vector2(_Player.PositionCenter.X - 10 - Position.X, _Player.PositionCenter.Y - Position.Y);
			}
			else if (_MovementType == MovementType.HORIZONTAL || _MovementType == MovementType.BOUNCE)
			{
				Direction.X = _Player.PositionCenter.X - Position.X;
			}

			if (Collision.Magnitude(Direction) <= 200)
			{
				if (_MovementType != MovementType.FLY)
				{
					Direction.X = Collision.UnitVector(Direction).X;
				}
				else
				{
					Direction = Collision.UnitVector(Direction);
				}

				if (_Player.GetPosition.X == Position.X)
				{
					if (_MovementType == MovementType.FLY)
					{
						Direction = new Vector2(0, 0);
					}
					else if (_MovementType == MovementType.HORIZONTAL || _MovementType == MovementType.BOUNCE)
					{
						Direction.X = 0;
					}
				}

				if (_MovementType != MovementType.BOUNCE)
				{
					SetAnimation("CHASE");
				}

				Position += Direction;

				if (_MovementType == MovementType.FLY)
				{
					Rotation += 0.05f;
				}
			}
			else
			{
				if (_MovementType != MovementType.BOUNCE)
				{
					SetAnimation("IDLE");
				}
			}

			foreach (Projectile p in _Player.ProjectileList)
			{
				if (playerCollisions.TouchLeftOf(p.projectileRectangle) || playerCollisions.TouchTopOf(p.projectileRectangle) || playerCollisions.TouchRightOf(p.projectileRectangle) || playerCollisions.TouchBottomOf(p.projectileRectangle))
				{
					DeleteMe = true;
				}
			}

			foreach (Rectangle r in MapTiles)
			{
				CheckCollision(playerCollisions, r);
			}
			foreach (Rectangle r in MapSides)
			{
				CheckCollision(playerCollisions, r);
			}
			if (_MovementType == MovementType.BOUNCE)
			{
				if (!isJumping && !canFall)
				{
					isJumping = true;
					Position.Y -= GravityForce * 1.03f;
					SetAnimation("CHASE");
				}
				if (canFall)
				{
					SetAnimation("FALLING");
				}
			}

			if (_MovementType != MovementType.FLY)
			{
				Position.Y += Direction.Y;
			}

			UpdateGravity();
			LastFrameTime += gameTime.ElapsedGameTime.Milliseconds;

			if (LastFrameTime >= CurrentAnimation.framesPerMillisecond)
			{
				CurrentFrame.X++;

				if (CurrentFrame.X >= CurrentAnimation.sheetSize.X)
				{
					CurrentFrame.Y++;
					if (_MovementType != MovementType.BOUNCE)
					{
						CurrentFrame.X = 0;
					}
					else
					{
						CurrentFrame.X = CurrentAnimation.sheetSize.X;
					}

					if (CurrentFrame.Y >= CurrentAnimation.sheetSize.Y)
					{
						CurrentFrame.Y = 0;
					}
				}

				LastFrameTime = 0;
			}
		}

		protected override void UpdateGravity()
		{
			base.UpdateGravity();
		}
	}
}
