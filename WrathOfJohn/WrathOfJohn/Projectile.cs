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
	public class Projectile : Sprite
	{
		Rectangle projectileRectangle;

		Game1 myGame;
		Player player;

		Vector2 startPosition;

		public Vector2 GetStartPosition
		{
			get
			{
				return startPosition;
			}
		}

		float maxDistance = 0f;
		public float GetMaxDistance
		{
			get
			{
				return maxDistance;
			}
		}

		bool visible = false;
		public bool isVisible
		{
			get
			{
				return visible;
			}
		}

		public Projectile(Vector2 startPosition, Color color, List<AnimationSet> animationSetList, Player player, Game1 game)
			: base(startPosition, color, animationSetList)
		{
			this.startPosition = startPosition;
			Position = startPosition;
			color = Color.White;
			AnimationSets = animationSetList;
			this.player = player;
			this.myGame = game;

			if (player.isFlipped)
			{
				Position.X = this.Position.X - 25;
				Direction = new Vector2(-1, 0);
			}
			else
			{
				Direction = new Vector2(1, 0);
			}
		}

		public override void Update(GameTime gameTime)
		{
			projectileRectangle = new Rectangle((int)Position.X, (int)Position.Y, 25, 3);

			if (Vector2.Distance(startPosition, Position) > maxDistance)
			{
				visible = false;
			}
			foreach (Rectangle r in myGame.gameManager.platformRectangles)
			{
				if (projectileRectangle.TouchLeftOf(r) ||  projectileRectangle.TouchTopOf(r) || projectileRectangle.TouchBottomOf(r) || projectileRectangle.TouchRightOf(r))
				{
					visible = false;
				}
			}
			if (visible)
			{
				Position += Direction * Speed + (player.GetDirection * player.Speed);
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

		public void Fire()
		{
			Speed = 3;
			maxDistance = 75;

			visible = true;

			SetAnimation("IDLE");
		}
	}
}
