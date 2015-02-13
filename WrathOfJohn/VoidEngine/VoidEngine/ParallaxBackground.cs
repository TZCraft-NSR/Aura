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

namespace VoidEngine
{
	public class ParallaxBackground
	{
		private Texture2D texture
		{
			get;
			set;
		}
		public Vector2 position
		{
			get;
			set;
		}
		private Color color
		{
			get;
			set;
		}
		private float multiplier
		{
			get;
			set;
		}
		private Camera camera
		{
			get;
			set;
		}
		private float parallax;
		private Vector2 defaultPosition
		{
			get;
			set;
		}
		public GraphicsDeviceManager graphics
		{
			get;
			set;
		}

		public ParallaxBackground(Texture2D texture, Vector2 position, Color color, float multiplier, Camera camera)
		{
			this.texture = texture;
			this.position = position;
			this.color = color;
			this.multiplier = multiplier;
			this.camera = camera;
			defaultPosition = position;
		}

		public void Update(GameTime gameTime)
		{
			parallax = camera.OverallPlayerPosition.X * multiplier;
		}

		public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(texture, position, new Rectangle((int)parallax, 0, texture.Width, texture.Height), color);
		}
	}
}
