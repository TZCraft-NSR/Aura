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
		public enum CollisionState
		{
			COLLIDE,
			NOT
		}
		/// <summary>
		/// The type of AI the sprite or enemy or player will have
		/// Only PLAYER and NONE are implemented
		/// </summary>
		public enum AIType
		{
			NONE,
			ENEMY,
			AI,
			PLAYER
		}
		/// <summary>
		/// The type of movement that the player or AI or sprite will have
		/// </summary>
		public enum MovementType
		{
			NONE,
			HORIZONTAL,
			VERTICAL,
			TOPDOWN,
			PLATFORMER
		}
		/// <summary>
		/// The animation set for each type of animation in the player sprite sheet.
		/// </summary>
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
			// The start Position in exact cordinates that the animation starts at.
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
			// <param name="framesPerMillisecond2">The start Position in exact cordinates that the animation starts at.</param>
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

		/// <summary>
		/// Gets or sets the sprite's current animation.
		/// </summary>
		protected AnimationSet CurrentAnimation
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets the animation sets.
		/// </summary>
		protected List<AnimationSet> AnimationSets
		{
			get;
			set;
		}
		/// <summary>
		/// The sprites current animation frame.
		/// </summary>
		private Point currentFrame;
		/// <summary>
		/// Gets or sets the sprites current animation frame.
		/// </summary>
		public Point CurrentFrame;
		/// <summary>
		/// Gets or sets the animations last frame time.
		/// </summary>
		protected int LastFrameTime
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets the direction that the sprite is moving towards.
		/// </summary>
		protected Vector2 Direction;
		/// <summary>
		/// The Position that the sprite is at.
		/// </summary>
		protected Vector2 Position;
		/// <summary>
		/// Gets the position that the sprite is at.
		/// </summary>
		public Vector2 GetPosition
		{
			get
			{
				return Position;
			}
		}
		/// <summary>
		/// Gets or sets the Speed that the sprite moves at.
		/// <summary>
		protected float Speed
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets weither the spite can move or not.
		/// </summary>
		protected bool CanMove
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets the type of AI the sprite uses.
		/// </summary>
		public AIType _AIType
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets the type of movement the sprite uses.
		/// </summary>
		public MovementType _MovementType
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets the list of keys that the sprite uses.
		/// Indexes: [0]: Left | [1]: Up | [2]: Right | [3]: Down | [4]: Custom1 | [5]: Custom2 | [6]: Custom3 | [?]: etc.
		/// </summary>
		protected List<Keys> MovementKeys
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets if the sprite is flipped.
		/// </summary>
		public SpriteEffects isFlipped
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets if the sprite is jumping. (Used for player or enemy classes)
		/// </summary>
		public bool isJumping
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets if the sprite is on the ground. (Used for player or enemy classes)
		/// </summary>
		public bool isGrounded
		{
			get;
			set;
		}
		public bool isTouchingGround;
		/// <summary>
		/// Gets or sets if the sprite is falling. (Used for player or enemy classes)
		/// </summary>
		public bool isFalling
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets the sprites bleed off of gravity. (Used for player or enemy classes)
		/// </summary>
		public float BleedOff
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets the sprites gravity. (Used for player or enemy classes)
		/// </summary>
		protected float Gravity
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets the sprites color.
		/// </summary>
		protected Color _Color
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets the keyboard state.
		/// </summary>
		protected KeyboardState _KeyboardState
		{
			get;
			private set;
		}
		/// <summary>
		/// Gets or sets the previous keyboard state.
		/// </summary>
		protected KeyboardState _PreviousKeyboardState
		{
			get;
			private set;
		}

		protected CollisionState collisionTopState = CollisionState.NOT;
		protected CollisionState collisionLeftState = CollisionState.NOT;
		protected CollisionState collisionRightState = CollisionState.NOT;

		public bool isColliding1 = false;
		public bool isColliding2 = false;
		public bool isColliding3 = false;

		/// <summary>
		/// Creates the sprite with custom properties
		/// </summary>
		/// <param name="postion">The Position of the sprite.</param>
		/// <param name="animationSetList">The list of animations.</param>
		public Sprite(Vector2 position, Color color, List<AnimationSet> animationSetList)
		{
			MovementKeys = new List<Keys>();
			AnimationSets = new List<AnimationSet>();
			AnimationSets = animationSetList;
			Position = position;
			LastFrameTime = 0;
			_Color = color;
			BleedOff = 0;
			isTouchingGround = false;
		}

		/// <summary>
		/// Put this in the Update function
		/// </summary>
		/// <param name="gameTime">The main GameTime</param>
		public virtual void Update(GameTime gameTime)
		{
			_KeyboardState = Keyboard.GetState();
			_PreviousKeyboardState = _KeyboardState;

			LastFrameTime += gameTime.ElapsedGameTime.Milliseconds;

			if (LastFrameTime >= CurrentAnimation.framesPerMillisecond)
			{
				currentFrame.X++;

				if (currentFrame.X >= CurrentAnimation.sheetSize.X)
				{
					currentFrame.Y++;
					currentFrame.X = 0;

					if (currentFrame.Y >= CurrentAnimation.sheetSize.Y)
					{
						currentFrame.Y = 0;
					}
				}

				LastFrameTime = 0;
			}

			UpdateMovement();

			_PreviousKeyboardState = _KeyboardState;
		}

		/// <summary>
		/// Put inbetween the spriteBatch.Begin and spriteBatch.End
		/// </summary>
		/// <param name="gameTime">The main GameTime</param>
		/// <param name="spriteBatch">The main SpriteBatch</param>
		public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(CurrentAnimation.texture, this.Position, new Rectangle(CurrentAnimation.startPosition.X + (CurrentFrame.X * CurrentAnimation.frameSize.X), CurrentAnimation.startPosition.Y + (CurrentFrame.Y * CurrentAnimation.frameSize.Y), CurrentAnimation.frameSize.X, CurrentAnimation.frameSize.Y), _Color, 0f, Vector2.Zero, 1f, isFlipped, 0);
		}

		/// <summary>
		/// Set the currentAnimation.
		/// </summary>
		/// <param name="setName">The name of the animation to set.</param>
		public void SetAnimation(string setName)
		{
			if (CurrentAnimation.name != setName)
			{
				foreach (AnimationSet a in AnimationSets)
				{
					if (a.name == setName)
					{
						CurrentAnimation = a;
						CurrentFrame = Point.Zero;
					}
				}
			}
		}

		/// <summary>
		/// Updates the sprites movement.
		/// </summary>
		public virtual void UpdateMovement()
		{
			if (_MovementType == MovementType.HORIZONTAL)
			{
				if (_AIType == AIType.PLAYER)
				{
					if (_KeyboardState.IsKeyDown(MovementKeys[0]))
					{
						Position.X -= Speed;
					}
					if (_KeyboardState.IsKeyDown(MovementKeys[2]))
					{
						Position.X += Speed;
					}
				}
			}
			if (_MovementType == MovementType.VERTICAL)
			{
				if (_AIType == AIType.PLAYER)
				{
					if (_KeyboardState.IsKeyDown(MovementKeys[1]))
					{
						Position.Y -= Speed;
					}
					if (_KeyboardState.IsKeyDown(MovementKeys[3]))
					{
						Position.Y += Speed;
					}
				}
			}
			if (_MovementType == MovementType.TOPDOWN)
			{
				if (_AIType == AIType.PLAYER)
				{
					if (_KeyboardState.IsKeyDown(MovementKeys[0]))
					{
						Position.X -= Speed;
					}
					if (_KeyboardState.IsKeyDown(MovementKeys[1]))
					{
						Position.Y -= Speed;
					}
					if (_KeyboardState.IsKeyDown(MovementKeys[2]))
					{
						Position.X += Speed;
					}
					if (_KeyboardState.IsKeyDown(MovementKeys[3]))
					{
						Position.Y += Speed;
					}
				}
			}
			if (_MovementType == MovementType.PLATFORMER)
			{
				if (_AIType == AIType.PLAYER)
				{
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
				isFlipped = SpriteEffects.FlipHorizontally;
			}
			else
			{
				isFlipped = SpriteEffects.None;
			}
		}

		/// <summary>
		/// To update the player's gravity
		/// </summary>
		/// <param name="gameTime"></param>
		public void UpdateGravity()
		{
			if (isJumping || isFalling)
			{
				if (BleedOff > 0f && isJumping)
				{
					Direction = new Vector2(Direction.X, -BleedOff);
					BleedOff -= 0.03f;
				}

				if (BleedOff <= 0f)
				{
					isFalling = true;
					isJumping = false;
				}
				if (isFalling && !isJumping)
				{
					Direction = new Vector2(Direction.X, -BleedOff);
					BleedOff -= 0.06f;
				}
			}

			if (isFalling)
			{
				if (isGrounded)
				{
					Direction = Vector2.Zero;
					isJumping = false;
					isFalling = false;
				}
			}

			if (!isTouchingGround)
			{
				if (isGrounded)
				{
					isFalling = false;
				}
			}

			BleedOff = MathHelper.Clamp(BleedOff, -Gravity - 1f, Gravity);
		}

		/// <summary>
		/// The check if the sprite collides rectangulary.
		/// </summary>
		public Rectangle collisionRectangle()
		{
			return new Rectangle((int)Position.X, (int)Position.Y, CurrentAnimation.frameSize.X, CurrentAnimation.frameSize.Y);
		}

		/// <summary>
		/// Sets the current sprite Position.
		/// </summary>
		/// <param name="newPosition">The Position to set the sprite to.</param>
		public void SetPosition(Vector2 newPosition)
		{
			Position = newPosition;
		}

		public bool DetectTopSegmentCollision(Collision.Circle spriteCircle, List<Collision.MapSegment> mapSegments)
		{
			bool tempBoolean = true;

			foreach (Collision.MapSegment mapSegment in mapSegments)
			{
				if (collisionTopState == CollisionState.NOT)
				{
					if (Collision.CheckCircleSegmentCollision(spriteCircle, mapSegment))
					{
                        collisionTopState = CollisionState.COLLIDE;

						tempBoolean = true;
					}
				}
                if (collisionTopState == CollisionState.COLLIDE)
				{
                    collisionTopState = CollisionState.NOT;
					tempBoolean = false;
				}
			}

			return tempBoolean;
		}

        public bool DetectLeftSegmentCollision(Collision.Circle spriteCircle, List<Collision.MapSegment> mapSegments)
        {
            bool tempBoolean = false;

            foreach (Collision.MapSegment mapSegment in mapSegments)
            {
                if (collisionLeftState == CollisionState.NOT)
                {
                    if (Collision.CheckCircleSegmentCollision(spriteCircle, mapSegment))
                    {
                        collisionLeftState = CollisionState.COLLIDE;

                        tempBoolean = false;
                    }
                }
                if (collisionLeftState == CollisionState.COLLIDE)
                {
                    collisionLeftState = CollisionState.NOT;
                    tempBoolean = true;
                }
            }

            return tempBoolean;
        }

        public bool DetectRightSegmentCollision(Collision.Circle spriteCircle, List<Collision.MapSegment> mapSegments)
        {
            bool tempBoolean = false;

            foreach (Collision.MapSegment mapSegment in mapSegments)
            {
                if (collisionRightState == CollisionState.NOT)
                {
                    if (Collision.CheckCircleSegmentCollision(spriteCircle, mapSegment))
                    {
                        collisionRightState = CollisionState.COLLIDE;
                        tempBoolean = false;
                    }
                }
                if (collisionRightState == CollisionState.COLLIDE)
                {
                    collisionRightState = CollisionState.NOT;
                    tempBoolean = true;
                }
            }

            return tempBoolean;
        }
	}
}