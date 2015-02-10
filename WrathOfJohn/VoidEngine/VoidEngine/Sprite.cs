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

namespace VoidEngine
{
    /// <summary>
    /// The Sprite class for VoidEngine
    /// </summary>
    public class Sprite
    {
        // <summary>
        // The type of AI the sprite or enemy or player will have
        // Only PLAYER and NONE are implemented
        // </summary>
        public enum AIType { NONE, ENEMY, AI, PLAYER };

        // <summary>
        // The type of movement that the player or AI or sprite will have
        // </summary>
        public enum MovementType { NONE, HORIZONTAL, VERTICAL, TOPDOWN, PLATFORMER };

        // <summary>
        // The animation set for each type of animation in the player sprite sheet.
        // </summary>
        public struct AnimationSet
        {
            // <summary>
            // The name "In all caps" that is used in the animation
            // </summary>
            public string name;
            // <summary>
            // The texture of the sprite sheet
            // </summary>
            public Texture2D texture;
            // <summary>
            // The size of each frame, all frames have to be the same size in one animation.
            // </summary>
            public Point frameSize;
            // <summary>
            // The size of that animation in the sheet in frames.
            // </summary>
            public Point sheetSize;
            // <summary>
            // The rate in milliseconds that the frames change
            // </summary>
            public int framesPerMillisecond;
            // <summary>
            // The start position in exact cordinates that the animation starts at.
            // </summary>
            public Point startPosition;

            // <summary>
            // For creating a new animation set.
            // </summary>
            // <param name="name2">The name "In all caps" that is used in the animation</param>
            // <param name="texture2">The texture of the sprite sheet</param>
            // <param name="frameSize2">The size of each frame, all frames have to be the same size in one animation.</param>
            // <param name="sheetSize2">The size of that animation in the sheet in frames.</param>
            // <param name="startPosition2">The rate in milliseconds that the frames change</param>
            // <param name="framesPerMillisecond2">The start position in exact cordinates that the animation starts at.</param>
            public AnimationSet(string name2, Texture2D texture2, Point frameSize2, Point sheetSize2, Point startPosition2, int framesPerMillisecond2)
            {
                name = name2;
                texture = texture2;
                frameSize = frameSize2;
                sheetSize = sheetSize2;
                framesPerMillisecond = framesPerMillisecond2;
                startPosition = startPosition2;
            }
        }

        // <summary>
        // The current AnimationSet.
        // </summary>
        public AnimationSet currentAnimation;
		/// <summary>
		/// The list of animation sets.
		/// </summary>
        protected List<AnimationSet> animationSets = new List<AnimationSet>();
		/// <summary>
		/// The current frame's position in sheet cords.
		/// </summary>
        Point currentFrame;
		/// <summary>
		/// Frame time before the update.
		/// </summary>
        int lastFrameTime;
        /// <summary>
        /// The current direction that the sprite is moving at.
        /// </summary>
        public Vector2 direction;
        /// <summary>
        /// The current direction that the sprite is at.
        /// </summary>
        public Vector2 position;
        /// <summary>
        /// The current speed that the sprite is moving at.
        /// <summary>
        public float speed;
        /// <summary>
        /// Weither the sprite can move or not.
        /// </summary>
        bool move = false;
		/// <summary>
		/// Gets or Sets if the sprite can move.
		/// </summary>
		public bool canMove
		{
			set
			{
				move = value;
			}
			get
			{
				return move;
			}
		}
        /// <summary>
        /// The type of AI the sprite has.
        /// </summary>
        public AIType aiType;
        /// <summary>
        /// The type of movement the sprite has.
        /// </summary>
        public MovementType movementType;
        /// <summary>
        /// The list of keys that the sprite can move at.
        /// indexes: [0]: Left | [1]: Up | [2]: Right | [3]: Down | [4]: Custom1 | [5]: Custom2 | [6]: Custom3
        /// </summary>
        public List<Keys> MovementKeys = new List<Keys>();
        /// <summary>
        /// To flip the sprite
        /// </summary>
        public SpriteEffects flipped;
        /// <summary>
        /// The keyboard detection
        /// </summary>
		protected KeyboardState keyboardState, previousKeyboardState;
		/// <summary>
		/// Sets or Gets if the sprite is jumping.
		/// </summary>
		bool jumping = false;
		/// <summary>
		/// gets if the sprite is jumping.
		/// </summary>
		public bool isJumping
		{
			set
			{
				jumping = value;
			}
			get
			{
				return jumping;
			}
		}
		/// <summary>
		/// Sets or Gets if the sprite is grounded.
		/// </summary>
		bool grounded = true;
		/// <summary>
		/// Gets if the sprite is grounded
		/// </summary>
		public bool isGrounded
		{
			set
			{
				grounded = value;
			}
			get
			{
				return grounded;
			}
		}
		/// <summary>
		/// If the sprite is falling
		/// </summary>
		bool falling = false;
		/// <summary>
		/// Gets or Sets if the sprite is falling.
		/// </summary>
		public bool isFalling
		{
			set
			{
				falling = value;
			}
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
		/// Sets or Gets the gravity acceleration.
		/// </summary>
		public float BleedOff
		{
			set
			{
				bleedOff = value;
			}
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
		/// Gets or Sets the default Gravity Acceleration
		/// </summary>
		public float Gravity
		{
			set
			{
				gravity = value;
			}
			get
			{
				return bleedOff;
			}
		}
		/// <summary>
		/// This is to set the default ground position.
		/// </summary>
		public Vector2 ground;
		Rectangle destinationRectangle;
		Rectangle sourceRectangle;
		public Color color;

        /// <summary>
        /// Creates the sprite with custom properties
        /// </summary>
        /// <param name="postion">The position of the sprite.</param>
        /// <param name="animationSetList">The list of animations.</param>
        public Sprite(Vector2 position, Color color, List<AnimationSet> animationSetList)
        {
            animationSets = animationSetList;
            this.position = position;
            lastFrameTime = 0;
			this.color = color;
        }

        /// <summary>
        /// Put this in the Update function
        /// </summary>
        /// <param name="gameTime">The main GameTime</param>
        public virtual void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();
            previousKeyboardState = keyboardState;

            lastFrameTime += gameTime.ElapsedGameTime.Milliseconds;

            if (lastFrameTime >= currentAnimation.framesPerMillisecond)
            {
                currentFrame.X++;

                if (currentFrame.X >= currentAnimation.sheetSize.X)
                {
                    currentFrame.Y++;
                    currentFrame.X = 0;

                    if (currentFrame.Y >= currentAnimation.sheetSize.Y)
                    {
                        currentFrame.Y = 0;
                    }
                }

                lastFrameTime = 0;
            }

			UpdateMovement();
        }

		public void UpdateRecangle(Rectangle DestinationRectangle, Rectangle SourceRectangle)
		{
			destinationRectangle = DestinationRectangle;
			sourceRectangle = SourceRectangle;
		}

        /// <summary>
        /// Put inbetween the spriteBatch.Begin and spriteBatch.End
        /// </summary>
        /// <param name="gameTime">The main GameTime</param>
        /// <param name="spriteBatch">The main SpriteBatch</param>
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
			spriteBatch.Draw(currentAnimation.texture, this.position, new Rectangle(currentAnimation.startPosition.X + (currentFrame.X * currentAnimation.frameSize.X), currentAnimation.startPosition.Y + (currentFrame.Y * currentAnimation.frameSize.Y), currentAnimation.frameSize.X, currentAnimation.frameSize.Y), color, 0f, Vector2.Zero, 1f, flipped, 0);
		}

        /// <summary>
        /// Set the currentAnimation.
        /// </summary>
        /// <param name="setName">The name of the animation to set.</param>
        public void SetAnimation(string setName)
        {
            if (currentAnimation.name != setName)
            {
                foreach (AnimationSet a in animationSets)
                {
                    if (a.name == setName)
                    {
                        currentAnimation = a;
                        currentFrame = Point.Zero;
                    }
                }
            }
        }

		public virtual void UpdateMovement()
		{
			if (movementType == MovementType.HORIZONTAL)
			{
				if (aiType == AIType.PLAYER)
				{
					if (keyboardState.IsKeyDown(MovementKeys[0]))
					{
						position.X -= speed;
					}
					if (keyboardState.IsKeyDown(MovementKeys[2]))
					{
						position.X += speed;
					}
				}
			}
			if (movementType == MovementType.VERTICAL)
			{
				if (aiType == AIType.PLAYER)
				{
					if (keyboardState.IsKeyDown(MovementKeys[1]))
					{
						position.Y -= speed;
					}
					if (keyboardState.IsKeyDown(MovementKeys[3]))
					{
						position.Y += speed;
					}
				}
			}
			if (movementType == MovementType.TOPDOWN)
			{
				if (aiType == AIType.PLAYER)
				{
					if (keyboardState.IsKeyDown(MovementKeys[0]))
					{
						position.X -= speed;
					}
					if (keyboardState.IsKeyDown(MovementKeys[1]))
					{
						position.Y -= speed;
					}
					if (keyboardState.IsKeyDown(MovementKeys[2]))
					{
						position.X += speed;
					}
					if (keyboardState.IsKeyDown(MovementKeys[3]))
					{
						position.Y += speed;
					}
				}
			}
			if (movementType == MovementType.PLATFORMER)
			{
				if (aiType == AIType.PLAYER)
				{
					if (keyboardState.IsKeyDown(MovementKeys[0]))
					{
						position.X -= speed;
					}
					if (keyboardState.IsKeyDown(MovementKeys[2]))
					{
						position.X += speed;
					}
					if (keyboardState.IsKeyDown(MovementKeys[4]))
					{
						isJumping = true;
						isGrounded = false;
					}
				}

				UpdateGravity();
			}
		}

		/// <summary>
		/// Flips the sprite texture based off a bool
		/// </summary>
		/// <param name="toFlip">The bool to flip</param>
        public void flipSprite(bool toFlip)
        {
            if (toFlip)
            {
                flipped = SpriteEffects.FlipHorizontally;
            }
            else
            {
                flipped = SpriteEffects.None;
            }
        }

		/// <summary>
		/// To update the player's gravity
		/// </summary>
		/// <param name="gameTime"></param>
		public void UpdateGravity()
		{
			if (isJumping)
			{
				position.Y -= BleedOff;
				BleedOff -= 0.03f;

				isGrounded = false;

				if (BleedOff <= 0f)
				{
					isFalling = true;
				}
			}

			if (isFalling)
			{
				if (position.Y >= ground.Y)
				{
					isGrounded = true;
					isJumping = false;
					isFalling = false;
					BleedOff = gravity;
				}
			}

			BleedOff = MathHelper.Clamp(BleedOff, -gravity - 1f, gravity);
		}

        /// <summary>
        /// The check if the sprite collides rectangulary.
        /// </summary>
        public Rectangle collisionRectangle()
        {
            return new Rectangle((int)position.X, (int)position.Y, currentAnimation.frameSize.X, currentAnimation.frameSize.Y);
        }

        public void SetPosition(Vector2 newPosition)
        {
            position = newPosition;
        }

        public Vector2 GetPosition
        {
            get
            {
                return position;
            }
        }
    }
}