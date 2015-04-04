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
	public class Player : VoidEngine.Player
	{
		/// <summary>
		/// Creates the player.
		/// </summary>
		/// <param name="position">The position the player starts at.</param>
		/// <param name="movementKeys">The keys to control the player.</param>
		/// <param name="gravity">The gravity of tha player.</param>
		/// <param name="mana">The maximum mana for the players projectiles.</param>
		/// <param name="color">The color to mask the player sprite with.</param>
		/// <param name="animationSetList">The animation set list for the player.</param>
		/// <param name="game">The game that the player runs off of.</param>
		public Player(Vector2 position, Keys[,] movementKeys, KeyboardState keyboardState, float HP, Mana mana, Color color, List<AnimationSet> animationSetList, List<AnimationSet> ProjectileAnimationSet)
			: base(position, movementKeys, keyboardState, HP, mana, color, animationSetList, ProjectileAnimationSet)
		{
			Level = 1;
			Scale = 0.34f;

			#region Set Animation Factors
			Offset = new Vector2(20, 5);
			#endregion

			MovementKeys = movementKeys;
			this.keyboardState = keyboardState;
			SetAnimation("IDLE1");

			int width = (int)(25);
			int left = (12);
			int height = (int)(animationSetList[0].frameSize.Y);
			int top = animationSetList[0].frameSize.Y - height;
			inbounds = new Rectangle(left, top, width, height);
		}
	}
}