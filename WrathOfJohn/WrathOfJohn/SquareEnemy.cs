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
    public class SquareEnemy : Player
    {
        public int HP;

        public bool DeleteMe = false;
        public bool MoveCircle = false;
        public bool DeleteCircle = false;

		public SquareEnemy(Vector2 position, Color color, List<AnimationSet> animationSetList, Game1 myGame)
            : base(position, color, animationSetList)
		{
			this.myGame = myGame;
            SetAnimation("IDLE");
			DefaultGravityForce = 1.25f;
			GravityForce = DefaultGravityForce;
			isFalling = true;
            ProjectileList = new List<Projectile>();
			Direction = Vector2.Zero;
			playerCollisions = new Rectangle((int)Position.X, (int)Position.Y, animationSetList[0].frameSize.X, animationSetList[0].frameSize.Y);
        }

        public override void Update(GameTime gameTime)
        {
			playerCollisions.X = (int)Position.X;
			playerCollisions.Y = (int)Position.Y;

			Direction = new Vector2(myGame.gameManager.player.GetPosition.X - Position.X, Direction.Y);

			if (Collision.Magnitude(Direction) <= 200)
			{
				Direction = Collision.UnitVector(Direction);

				if (myGame.gameManager.player.GetPosition.X == Position.X)
				{
					Direction = new Vector2(0, Direction.Y);
				}
				foreach (Rectangle r in myGame.gameManager.platformRectangles)
				{
					CheckCollision(playerCollisions, r);
				}
				foreach (Rectangle r in myGame.gameManager.mapSegments)
				{
					CheckCollision(playerCollisions, r);
				}
				if (!isJumping && !canFall)
				{
					isJumping = true;
					isFalling = false;
					Direction.Y = 0;
					isJumping = true;
					Position.Y -= GravityForce * 1.5f;
				}

				SetAnimation("CHASE");

				Position += Direction;
			}

			SetAnimation("IDLE");

			UpdateGravity();
        }

		protected override void InputMethod(List<Keys> keyList)
		{
		}

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
