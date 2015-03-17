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
	public class TriangleEnemy : Player
	{
		public TriangleEnemy(Vector2 position, Color color, List<AnimationSet> animationSetList, Game1 myGame)
			: base(position, color, animationSetList)
		{
			this.myGame = myGame;
			SetAnimation("IDLE");
			ProjectileList = new List<Projectile>();
			Direction = Vector2.Zero;
			playerCollisions = new Rectangle((int)Position.X, (int)Position.Y, animationSetList[0].frameSize.X, animationSetList[0].frameSize.Y);
		}

		public override void Update(GameTime gameTime)
		{
			playerCollisions.X = (int)Position.X;
			playerCollisions.Y = (int)Position.Y;

			Direction = new Vector2(myGame.gameManager.player.GetPosition.X - Position.X, myGame.gameManager.player.GetPosition.Y - Position.Y);

			if (Collision.Magnitude(Direction) <= 200)
			{
				Direction = Collision.UnitVector(Direction);

				if (myGame.gameManager.player.GetPosition.X == Position.X)
				{
					Direction = new Vector2(0, 0);
				}
				foreach (Rectangle r in myGame.gameManager.platformRectangles)
				{
					CheckCollision(playerCollisions, r);
				}
				foreach (Rectangle r in myGame.gameManager.mapSegments)
				{
					CheckCollision(playerCollisions, r);
				}

				SetAnimation("CHASE");

				Position += Direction;
			}

			SetAnimation("IDLE");
		}
	}
}
