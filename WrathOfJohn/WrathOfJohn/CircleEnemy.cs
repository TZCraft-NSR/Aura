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
	public class CircleEnemy : Player
	{
		public CircleEnemy(Vector2 position, Color color, List<AnimationSet> animationSetList, Game1 myGame)
			: base(position, color, animationSetList)
		{	}

		public override void Update(GameTime gameTime)
		{	}
	}
}
