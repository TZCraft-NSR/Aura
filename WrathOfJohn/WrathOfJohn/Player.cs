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
    class Player : Sprite
    {
        Game1 myGame;
        public List<Keys> keys = new List<Keys>(); // 0 = left, 1 = up, 2 = right, 3 = down, 4 = jump, 5 = crouch

        public Player(Texture2D texture, Vector2 position, Game1 game, Keys left, Keys right, Keys jump) : base(position)
        {
            myGame = game;
            addAnimations(texture);
            keys.Add(left);
            keys.Add(Keys.None);
            keys.Add(right);
            keys.Add(Keys.None);
            keys.Add(jump);
            keys.Add(Keys.None);
            move = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (move == true)
            {
                if (keyboardState.IsKeyDown(keys[4]))
                {
                    // For jumping
                }
                if (keyboardState.IsKeyDown(keys[0]))
                {
                    position.X -= speed;
                }
                if (keyboardState.IsKeyDown(keys[2]))
                {
                    position.X += speed;
                }

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
                if (position.X >= 480 - currentAnimation.frameSize.X)
                {
                    position.X = 480 - currentAnimation.frameSize.X;
                }

                /*if (IsJumping == true)
                {
                    if (position.Y < 10)
                    {
                        vi.Y += g.Y; //* gameTime.ElapsedGameTime.Milliseconds;
                        position.Y += (float)vi.Y; //* gameTime.ElapsedGameTime.Milliseconds;
                    }
                    else if (position.Y >= 10)
                    {
                        IsJumping = false;
                        vi.Y -= g.Y; //* gameTime.ElapsedGameTime.Milliseconds;
                        position.Y -= (float)vi.Y; //* gameTime.ElapsedGameTime.Milliseconds;
                    }
                }
                else if (IsJumping == false)
                {
                    //if (position.Y > 0)
                    //{
                    //    vi.Y -= g.Y; //* gameTime.ElapsedGameTime.Milliseconds;
                    //    position.Y -= (float)vi.Y; //* gameTime.ElapsedGameTime.Milliseconds;
                    //}
                    if (position.Y >= 430)
                    {
                        vi.Y = 0;
                    }

                    if (position.Y > 480)
                    {
                        position.Y = 0;
                    }
                }*/
            }

            if (keyboardState.IsKeyDown(keys[0]))
            {
                SetAnimation("WALK");
            }

            if (keyboardState.IsKeyDown(keys[2]))
            {
                SetAnimation("WALK");
            }

            if (keyboardState.IsKeyDown(keys[4]))
            {
                SetAnimation("JUMP");
            }

            if (!keyboardState.IsKeyDown(keys[0]) && !keyboardState.IsKeyDown(keys[2]) && !keyboardState.IsKeyDown(keys[4]))
            {
                SetAnimation("IDLE");
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
        }

        public override void addAnimations(Texture2D texture)
        {
            addAnimation("IDLE", texture, new Point(60, 40), new Point(0, 0), new Point(0, 0), 15000);
            addAnimation("WALK", texture, new Point(60, 40), new Point(5, 0), new Point(60, 0), 15000);
            addAnimation("WALK", texture, new Point(60, 40), new Point(5, 0), new Point(60, 0), 15000);
        }
    }
}
