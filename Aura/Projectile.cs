using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using VoidEngine.VGame;
using VoidEngine.Helpers;

namespace Aura
{
	public class Projectile : Sprite
	{
		Player player;

		Game1 myGame;

		public Rectangle projectileRectangle;

		public bool visible;

		Vector2 velocity;

		float maxDistance;

		float Movement;

		Vector2 startPosition;

		public Projectile(Vector2 startPosition, Color color, List<AnimationSet> animationSetList, Game1 myGame, Player player)
			: base(startPosition, color, animationSetList)
		{
			this.startPosition = startPosition;
			Position = startPosition;
			this.myGame = myGame;
			this.player = player;

			if (player.isFlipped)
			{
				position.X = startPosition.X - animationSetList[0].frameSize.X;
			}
			else
			{
				position.X = startPosition.X + player.BoundingCollisions.Width;
			}
		}

		public float EnemyClampedUnitVectorDistance;
		public float EnemyUnitVectorDistance;

		public float BossClampedUnitVectorDistance;
		public float BossUnitVectorDistance;
		private float MovementMultiples;
		private float AirDrag;

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
				EnemyUnitVectorDistance = CollisionHelper.UnitVector(new Vector2((player.Position.X + myGame.gameManager.player.PositionCenter.X) - (myGame.gameManager.bhEnemy.Position.X + myGame.gameManager.bhEnemy.PositionCenter.X), 0)).X * -1;
				EnemyUnitVectorDistance = MathHelper.Clamp(EnemyClampedUnitVectorDistance, -1, 1);

				if (projectileRectangle.TouchLeftOf(er.BoundingCollisions) || projectileRectangle.TouchTopOf(er.BoundingCollisions) || projectileRectangle.TouchBottomOf(er.BoundingCollisions) || projectileRectangle.TouchRightOf(er.BoundingCollisions))
				{
					visible = false;
					er.DeleteMe = true;
					myGame.gameManager.enemyhitSFX.Play(1f, 0f, EnemyUnitVectorDistance);
				}
			}
			if (myGame.gameManager.BossCreated && !myGame.gameManager.bhEnemy.Dead)
			{
				BossUnitVectorDistance = CollisionHelper.UnitVector(new Vector2((myGame.gameManager.player.Position.X + myGame.gameManager.player.PositionCenter.X) - (myGame.gameManager.bhEnemy.Position.X + myGame.gameManager.bhEnemy.PositionCenter.X), 0)).X * -1;
				BossUnitVectorDistance = MathHelper.Clamp(BossClampedUnitVectorDistance, -1, 1);

				if (projectileRectangle.TouchLeftOf(myGame.gameManager.bhEnemy.BoundingCollisions) || projectileRectangle.TouchTopOf(myGame.gameManager.bhEnemy.BoundingCollisions) || projectileRectangle.TouchBottomOf(myGame.gameManager.bhEnemy.BoundingCollisions) || projectileRectangle.TouchRightOf(myGame.gameManager.bhEnemy.BoundingCollisions))
				{
					myGame.gameManager.bhEnemy.HP -= 1;
					visible = false;
					myGame.gameManager.enemyhitSFX.Play(1f, 0f, BossUnitVectorDistance);
				}
			}
			if (visible)
			{
				velocity.X += Movement * MovementMultiples * AirDrag * (float)gameTime.ElapsedGameTime.TotalSeconds;
				position.X += velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
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
