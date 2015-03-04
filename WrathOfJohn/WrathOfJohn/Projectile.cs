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
	public class Projectile : Player
	{
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

		public Projectile(Vector2 startPosition, Color color, List<AnimationSet> animationSetList, Player player, Game1 game)
			: base(startPosition, color, animationSetList)
		{
			this.startPosition = startPosition;
			Position = startPosition;
			color = Color.White;
			AnimationSets = animationSetList;

			if (player.isFlipped == SpriteEffects.FlipHorizontally)
			{
				Position.X = Position.X - 25;
				Direction = new Vector2(-1, 0);
			}
			else
			{
				Direction = new Vector2(1, 0);
			}
        }

        public override void Update(GameTime gameTime)
        {
            if (Vector2.Distance(startPosition, Position) > maxDistance)
            {
                visible = false;
            }

            if (visible == true)
            {
                Position += Direction * Speed;
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
			Speed = 1;
			maxDistance = 50;

			visible = true;

			SetAnimation("IDLE");

			projectileSegments.Add(new Collision.MapSegment(new Point((int)Position.X, (int)Position.Y), new Point((int)Position.X + CurrentAnimation.frameSize.X, (int)Position.Y)));
			projectileSegments.Add(new Collision.MapSegment(new Point((int)Position.X + CurrentAnimation.frameSize.X, (int)Position.Y), new Point((int)Position.X + CurrentAnimation.frameSize.X, (int)Position.Y + CurrentAnimation.frameSize.Y)));
			projectileSegments.Add(new Collision.MapSegment(new Point((int)Position.X + CurrentAnimation.frameSize.X, (int)Position.Y + CurrentAnimation.frameSize.Y), new Point((int)Position.X, (int)Position.Y + CurrentAnimation.frameSize.Y)));
			projectileSegments.Add(new Collision.MapSegment(new Point((int)Position.X, (int)Position.Y + CurrentAnimation.frameSize.Y), new Point((int)Position.X, (int)Position.Y)));
		}
	}
}
