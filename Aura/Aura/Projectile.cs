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

namespace Aura
{
	public class Projectile : VoidEngine.Projectile
	{
		VoidEngine.Player player;

		Game1 myGame;

		Vector2 startPosition;

		public Projectile(Vector2 startPosition, Color color, List<AnimationSet> animationSetList, Game1 myGame, VoidEngine.Player player)
			: base(startPosition, color, animationSetList, player)
		{
			this.startPosition = startPosition;
			Position = startPosition;
			this.myGame = myGame;
			this.player = player;

			if (player.isFlipped)
			{
				Position.X = startPosition.X - animationSetList[0].frameSize.X;
			}
			else
			{
				Position.X = startPosition.X + player.BoundingCollisions.Width;
			}
		}

		public float EnemyClampedUnitVectorDistance;
		public float EnemyUnitVectorDistance;

		public float BossClampedUnitVectorDistance;
		public float BossUnitVectorDistance;

		public void Update(GameTime gameTime, Player player, List<Enemy> EnemyList, List<Tile> TileList, List<Rectangle> MapBoundries)
		{
			projectileRectangle = new Rectangle((int)Position.X, (int)Position.Y, 25, 3);

			if (Vector2.Distance(startPosition, Position) > maxDistance)
			{
				visible = false;
			}
			foreach (Rectangle r in MapBoundries)
			{
				if (projectileRectangle.TouchLeftOf(r) || projectileRectangle.TouchTopOf(r) || projectileRectangle.TouchBottomOf(r) || projectileRectangle.TouchRightOf(r))
				{
					visible = false;
				}
			}
			foreach (Enemy er in EnemyList)
			{
				EnemyUnitVectorDistance = Collision.UnitVector(new Vector2((player.GetPosition.X + myGame.gameManager.player.PositionCenter.X) - (myGame.gameManager.bhEnemy.GetPosition.X + myGame.gameManager.bhEnemy.PositionCenter.X), 0)).X * -1;
				EnemyUnitVectorDistance = MathHelper.Clamp(EnemyClampedUnitVectorDistance, -1, 1);

				if (projectileRectangle.TouchLeftOf(er.BoundingCollisions) || projectileRectangle.TouchTopOf(er.BoundingCollisions) || projectileRectangle.TouchBottomOf(er.BoundingCollisions) || projectileRectangle.TouchRightOf(er.BoundingCollisions))
				{
					visible = false;
					er.DeleteMe = true;
					myGame.gameManager.enemyhitSFX.Play(1f, 0f, EnemyUnitVectorDistance);
				}
			}
			if (myGame.gameManager.BossCreated && !myGame.gameManager.bhEnemy.isDead)
			{
				BossUnitVectorDistance = Collision.UnitVector(new Vector2((myGame.gameManager.player.GetPosition.X + myGame.gameManager.player.PositionCenter.X) - (myGame.gameManager.bhEnemy.GetPosition.X + myGame.gameManager.bhEnemy.PositionCenter.X), 0)).X * -1;
				BossUnitVectorDistance = MathHelper.Clamp(BossClampedUnitVectorDistance, -1, 1);

				if (projectileRectangle.TouchLeftOf(myGame.gameManager.bhEnemy.BoundingCollisions) || projectileRectangle.TouchTopOf(myGame.gameManager.bhEnemy.BoundingCollisions) || projectileRectangle.TouchBottomOf(myGame.gameManager.bhEnemy.BoundingCollisions) || projectileRectangle.TouchRightOf(myGame.gameManager.bhEnemy.BoundingCollisions))
				{
					myGame.gameManager.bhEnemy.SetMainHP -= 1;
					visible = false;
					myGame.gameManager.enemyhitSFX.Play(1f, 0f, BossUnitVectorDistance);
				}
			}
			if (visible)
			{
				velocity.X += Movement * MovementMultiples * AirDrag * (float)gameTime.ElapsedGameTime.TotalSeconds;
				Position.X += velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
			}

			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			if (visible)
			{
				base.Draw(gameTime, spriteBatch);
			}
		}
	}
}
