using System;
using System.Collections.Generic;
using System.Linq;
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
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
	public class GameManager : Microsoft.Xna.Framework.DrawableGameComponent
	{
        public SpriteBatch spriteBatch;
		public Game1 myGame;

        public List<Sprite.AnimationSet> npcAnimationSetList;
        public List<Vector2> npcPositions;
        public List<Sprite.AnimationSet> tempAnimationSetList = new List<Sprite.AnimationSet>();

        public Player player;
        public Texture2D playerTexture;
        public Texture2D debugDotTexture;

        public GameManager(Game1 game) : base(game)
		{
			myGame = game;
		}

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            playerTexture = Game.Content.Load<Texture2D>(@"Images\players\player");
            debugDotTexture = Game.Content.Load<Texture2D>(@"Images\debug\line");

            tempAnimationSetList.Add(new Sprite.AnimationSet("IDLE", playerTexture, new Point(60, 50), new Point(1, 1), new Point(0, 0), 1000));
            tempAnimationSetList.Add(new Sprite.AnimationSet("WALK", playerTexture, new Point(60, 50), new Point(4, 3), new Point(0, 0), 50));
            tempAnimationSetList.Add(new Sprite.AnimationSet("JUMP", playerTexture, new Point(60, 50), new Point(4, 1), new Point(0, 150), 100));

            player = new Player(playerTexture, new Vector2((myGame.windowSize.X - 60) / 2, (myGame.windowSize.Y - 50) / 2), myGame, Keys.A, Keys.D, Keys.Space, 2f, tempAnimationSetList);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            player.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
                player.Draw(gameTime, spriteBatch);
                spriteBatch.Draw(debugDotTexture, new Rectangle(0, ((int)(myGame.windowSize.Y - 50) / 2) + player.currentAnimation.frameSize.Y, (int)myGame.windowSize.X, 1), Color.White);
                spriteBatch.Draw(debugDotTexture, new Rectangle((int)player.position.X - 10, ((int)player.position.Y - 1) + player.currentAnimation.frameSize.Y, player.currentAnimation.frameSize.X + 10, 1), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
	}
}