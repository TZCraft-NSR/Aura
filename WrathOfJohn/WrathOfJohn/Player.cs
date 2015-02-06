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
        Game1 myGame;
        public List<Keys> keys = new List<Keys>(); // 0 = left, 1 = up, 2 = right, 3 = down, 4 = jump, 5 = crouch
        public List<AnimationSet> animationSetList = new List<AnimationSet>();
        public bool isJumping = false;
        public bool isGrounded = true;
        float bleedOff = 2.0f;
        float gravity;
        public Vector2 ground;

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

        public override void Update(GameTime gameTime)
        {
            if (myGame.keyboardState.IsKeyDown(keys[4]) && !isJumping)
            {
                isJumping = true;
                isGrounded = false;
            }
            if (myGame.keyboardState.IsKeyDown(keys[0]))
            {
                position.X -= speed;
            }
            if (myGame.keyboardState.IsKeyDown(keys[2]))
            {
                position.X += speed;
            }

            UpdateGravity(gameTime);

            if (position.Y <= 0)
            {
                position.Y = 0;
            }
            if (position.Y >= 480)
            {
                position.Y = 480 - currentAnimation.frameSize.Y;
            }
            if (position.X <= 0)
            {
                position.X = 0;
            }
            if (position.X >= 700 - currentAnimation.frameSize.X)
            {
                position.X = 700 - currentAnimation.frameSize.X;
            }

            if (isJumping && !isGrounded)
            {
                SetAnimation("JUMP");
            }
            
            if (myGame.keyboardState.IsKeyDown(keys[0]) && isGrounded && !isJumping)
            {
                SetAnimation("WALK");
            }

            if (myGame.keyboardState.IsKeyDown(keys[0]))
            {
                if (isFlipped == false)
                {
                    flipped = SpriteEffects.FlipHorizontally;
                    isFlipped = true;
                }
            }
            
            if (myGame.keyboardState.IsKeyDown(keys[2]) && isGrounded && !isJumping)
            {
                SetAnimation("WALK");
            }

            if (myGame.keyboardState.IsKeyDown(keys[2]))
            {
                if (isFlipped == true)
                {
                    flipped = SpriteEffects.None;
                    isFlipped = false;
                }
            }

            if (!myGame.keyboardState.IsKeyDown(keys[0]) && !myGame.keyboardState.IsKeyDown(keys[2]) || !isJumping)
            {
                SetAnimation("IDLE");
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
        }

        public void UpdateGravity(GameTime gameTime)
        {
            if (isJumping)
            {
                position.Y -= bleedOff;
                bleedOff -= 0.03f;

                isGrounded = false;

                if (position.Y >= ground.Y)
                {
                    isJumping = false;
                    bleedOff = gravity;
                }
            }

            if (position.Y >= ground.Y && !isGrounded)
            {
                isGrounded = true;
                isJumping = false;
            }

            bleedOff = MathHelper.Clamp(bleedOff, -gravity - 1f, gravity);
        }
    }
}