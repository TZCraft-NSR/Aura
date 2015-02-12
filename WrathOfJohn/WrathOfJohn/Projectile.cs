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
	class Projectile : Sprite
	{
		Game1 myGame;
		List<Collision.MapSegment> projectileSegments = new List<Collision.MapSegment>();
		Vector2 startPosition;
		public Vector2 StartPosition
		{
			get
			{
				return startPosition;
			}
		}
		float maxDistance = 0f;
		public float MaxDistance
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

		public override void Update(GameTime gameTime)
		{
			UpdateMovement();

			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			if (visible)
			{
				base.Draw(gameTime, spriteBatch);
			}
		}

		public Projectile(Vector2 startPosition, Color color, List<AnimationSet> animationSetList, Player player, Game1 game)
			: base(startPosition, color, animationSetList)
		{
			myGame = game;
			this.startPosition = startPosition;
			position = startPosition;
			color = Color.White;
			animationSets = animationSetList;

			if (player.flipped == SpriteEffects.FlipHorizontally)
			{
				position.X = position.X - 25;
				direction = new Vector2(-1, 0);
			}
			else
			{
				direction = new Vector2(1, 0);
			}
		}

		public void Fire()
		{
			speed = 1;
			maxDistance = 50;

			visible = true;

			SetAnimation("IDLE");

			projectileSegments.Add(new Collision.MapSegment(new Point((int)position.X, (int)position.Y), new Point((int)position.X + currentAnimation.frameSize.X, (int)position.Y)));
			projectileSegments.Add(new Collision.MapSegment(new Point((int)position.X + currentAnimation.frameSize.X, (int)position.Y), new Point((int)position.X + currentAnimation.frameSize.X, (int)position.Y + currentAnimation.frameSize.Y)));
			projectileSegments.Add(new Collision.MapSegment(new Point((int)position.X + currentAnimation.frameSize.X, (int)position.Y + currentAnimation.frameSize.Y), new Point((int)position.X, (int)position.Y + currentAnimation.frameSize.Y)));
			projectileSegments.Add(new Collision.MapSegment(new Point((int)position.X, (int)position.Y + currentAnimation.frameSize.Y), new Point((int)position.X, (int)position.Y)));
		}

		public override void UpdateMovement()
		{
			base.UpdateMovement();

			if (Vector2.Distance(startPosition, position) > maxDistance)
			{
				visible = false;
			}

			if (visible == true)
			{
				position += direction * speed;
			}
		}
	}
}
