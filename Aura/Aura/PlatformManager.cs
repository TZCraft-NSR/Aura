using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using VoidEngine;

namespace Aura
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
			SetAnimation("1");
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
			if (type == 17)
			{
				SetAnimation("17");
			}
			if (type == 18)
			{
				SetAnimation("18");
			}
			if (type == 19)
			{
				SetAnimation("19");
			}
			if (type == 20)
			{
				SetAnimation("20");
			}
			if (type == 21)
			{
				SetAnimation("21");
			}
			if (type == 22)
			{
				SetAnimation("22");
			}
			if (type == 23)
			{
				SetAnimation("23");
			}
			if (type == 24)
			{
				SetAnimation("24");
			}
			if (type == 25)
			{
				SetAnimation("25");
			}
			if (type == 26)
			{
				SetAnimation("26");
			}
			if (type == 27)
			{
				SetAnimation("27");
			}
			if (type == 28)
			{
				SetAnimation("28");
			}
			if (type == 29)
			{
				SetAnimation("29");
			}
			if (type == 30)
			{
				SetAnimation("30");
			}
			if (type == 31)
			{
				SetAnimation("31");
			}
			if (type == 32)
			{
				SetAnimation("32");
			}
			if (type == 33)
			{
				SetAnimation("33");
			}
			if (type == 34)
			{
				SetAnimation("34");
			}
			base.Draw(gameTime, spriteBatch);
		}
	}
}
