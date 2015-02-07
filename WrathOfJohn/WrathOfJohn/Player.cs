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
using System.Diagnostics;

namespace WrathOfJohn
{
    public class Player : Sprite
    {
		/// <summary>
		/// This is the Game class that the Player class is running off of.
		/// </summary>
        Game1 myGame;
		/// <summary>
		/// This is the keys that the player uses.
		/// 0 = left | 1 = up | 2 = right | 3 = down | 4 = jump | 5 = crouch | 6 = attack
		/// </summary>
        public List<Keys> keys = new List<Keys>();
		/// <summary>
		/// This is the animationSet list for the player.
		/// </summary>
        public List<AnimationSet> animationSetList = new List<AnimationSet>();
		/// <summary>
		/// Sets or Gets if the player is jumping.
		/// </summary>
		bool jumping = false;
		/// <summary>
		/// gets if the player is jumping.
		/// </summary>
		public bool isJumping
		{
			get
			{
				return jumping;
			}
		}
		/// <summary>
		/// Sets or Gets if the player is grounded.
		/// </summary>
        bool grounded = true;
		/// <summary>
		/// Gets if the player is grounded
		/// </summary>
		public bool isGrounded
		{
			get
			{
				return grounded;
			}
		}
		bool falling = false;
		public bool isFalling
		{
			get
			{
				return falling;
			}
		}
		/// <summary>
		/// This is to trigger gravity speed.
		/// </summary>
        float bleedOff = 2.0f;
		/// <summary>
		/// 
		/// </summary>
		public float BleedOff
		{
			get
			{
				return bleedOff;
			}
		}
		/// <summary>
		/// This is to set the default gravity size.
		/// </summary>
        float gravity;
		/// <summary>
		/// This is to set the default ground position.
		/// </summary>
        public Vector2 ground;

		/// <summary>
		/// This is to create the Player class.
		/// </summary>
		/// <param name="texture">This is the player's texture</param>
		/// <param name="position">This sets the player's position</param>
		/// <param name="game">This is the Game class that the player runs on</param>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <param name="jump"></param>
		/// <param name="gravity"></param>
		/// <param name="animationSetList"></param>
        public Player(Texture2D texture, Vector2 position, Game1 game, Keys left, Keys right, Keys jump, float gravity, List<AnimationSet> animationSetList) : base(position, animationSetList)
        {
            this.animationSetList = animationSetList;
            bleedOff = gravity;
            this.gravity = gravity;
            myGame = game;
            keys.Add(left);
            keys.Add(Keys.None);
            keys.Add(right);
            keys.Add(Keys.None);
            keys.Add(jump);
            keys.Add(Keys.None);
            canMove = true;
            speed = 1;
            ground = new Vector2(0, position.Y);
        }

		/// <summary>
		/// Updates the Player class
		/// </summary>
		/// <param name="gameTime">To keep track of run time.</param>
        public override void Update(GameTime gameTime)
        {
            if (myGame.keyboardState.IsKeyDown(keys[4]) && !isJumping && isGrounded && !isFalling)
            {
                jumping = true;
                grounded = false;
            }
            if (myGame.keyboardState.IsKeyDown(keys[0]))
            {
                position.X -= speed;
            }
            if (myGame.keyboardState.IsKeyDown(keys[2]))
            {
                position.X += speed;
            }

            UpdateGravity();

			// The boundry on the left.
            if (position.Y <= 0)
            {
                position.Y = 0;
            }
			// Boundry on the right.
            if (position.Y >= 480)
            {
                position.Y = 480 - currentAnimation.frameSize.Y;
            }
			// Boundry on the top.
            if (position.X <= 0)
            {
                position.X = 0;
            }
			// Boundry on the bottom.
            if (position.X >= 700 - currentAnimation.frameSize.X)
            {
                position.X = 700 - currentAnimation.frameSize.X;
            }

			// To play the jumping animation.
            if (isJumping && !isFalling)
            {
                SetAnimation("JUMP");
            }
            
			// To do the walking animation.
            if ((myGame.keyboardState.IsKeyDown(keys[2]) || myGame.keyboardState.IsKeyDown(keys[0])) && isGrounded)
            {
                SetAnimation("WALK");
			}

			// To flip the player to the left.
			if (myGame.keyboardState.IsKeyDown(keys[0]))
			{
				flipSprite(true);
			}

			// To flip the player back to the right.
            if (myGame.keyboardState.IsKeyDown(keys[2]))
            {
                flipSprite(false);
            }

			// To set the animation to idle.
            if ((!myGame.keyboardState.IsKeyDown(keys[0]) && !myGame.keyboardState.IsKeyDown(keys[2])) || isFalling)
            {
                SetAnimation("IDLE");
            }

            base.Update(gameTime);
        }

		/// <summary>
		/// To draw the Player class.
		/// </summary>
		/// <param name="gameTime">To keep track of run time.</param>
		/// <param name="spriteBatch">The spriteBatch to draw with.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
        }

		/// <summary>
		/// To update the player's gravity
		/// </summary>
		/// <param name="gameTime"></param>
        public void UpdateGravity()
        {
            if (jumping)
            {
                position.Y -= bleedOff;
                bleedOff -= 0.03f;

                grounded = false;

				if(bleedOff <= 0f)
				{
					falling = true;
				}
            }

			if (isFalling)
			{
				if (position.Y >= ground.Y)
				{
					grounded = true;
					jumping = false;
					falling = false;
					bleedOff = gravity;
				}
			}

            bleedOff = MathHelper.Clamp(bleedOff, -gravity - 1f, gravity);
        }
    }
}