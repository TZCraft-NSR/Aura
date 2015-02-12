using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using VoidEngine;

namespace VoidEngine
{
	public class PlatformManager : Sprite
	{
		Game myGame;

		public float brickRadius = 10f;
		public int type;
		List<Collision.MapSegment> brickSegments;

		public PlatformManager(Vector2 pos, Game mygame, int type, List<AnimationSet> animationSetLists)
			: base(pos, Color.White, animationSetLists)
		{
			this.animationSets = animationSetLists;
			myGame = mygame;
			this.type = type;
			SetAnimation("IDLE1");
		}

		public override void Update(GameTime gameTime)
		{
			brickSegments.Add(new Collision.MapSegment(new Point((int)position.X, (int)position.Y), new Point((int)position.X + 25, (int)position.Y)));
			brickSegments.Add(new Collision.MapSegment(new Point((int)position.X + 25, (int)position.Y), new Point((int)position.X + 25, (int)position.Y + 25)));
			brickSegments.Add(new Collision.MapSegment(new Point((int)position.X + 25, (int)position.Y + 25), new Point((int)position.X, (int)position.Y + 25)));
			brickSegments.Add(new Collision.MapSegment(new Point((int)position.X, (int)position.Y + 25), new Point((int)position.X, (int)position.Y)));

			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			if (type == 0)
			{
				SetAnimation("IDLE1");
			}
			if (type == 1)
			{
				SetAnimation("IDLE2");
			}
			base.Draw(gameTime, spriteBatch);
		}

		public List<Collision.MapSegment> GetSegments()
		{
			return brickSegments;
		}
	}
}
