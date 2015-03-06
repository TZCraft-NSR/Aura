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
	public class PlatformManager : Sprite
	{
		Game myGame;

		public float radius = 10;
		public int type;

		public PlatformManager(Vector2 pos, Game mygame, int type, List<AnimationSet> animationSetLists)
			: base(pos, Color.White, animationSetLists)
		{
			myGame = mygame;
			this.type = type;
			SetAnimation("IDLE1");
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			if (type == 1)
			{
				SetAnimation("1");
			}
			if (type == 2)
			{
				SetAnimation("2");
			}
			if (type == 3)
			{
				SetAnimation("3");
			}
			if (type == 4)
			{
				SetAnimation("4");
			}
			if (type == 5)
			{
				SetAnimation("5");
			}
			if (type == 6)
			{
				SetAnimation("6");
			}
			if (type == 7)
			{
				SetAnimation("7");
			}
			if (type == 8)
			{
				SetAnimation("8");
			}
			if (type == 9)
			{
				SetAnimation("9");
			}
			if (type == 10)
			{
				SetAnimation("10");
			}
			if (type == 11)
			{
				SetAnimation("11");
			}
			if (type == 12)
			{
				SetAnimation("12");
			}
			if (type == 13)
			{
				SetAnimation("13");
			}
			if (type == 14)
			{
				SetAnimation("14");
			}
			if (type == 15)
			{
				SetAnimation("15");
			}
			if (type == 16)
			{
				SetAnimation("16");
			}
			base.Draw(gameTime, spriteBatch);
		}
	}
}
