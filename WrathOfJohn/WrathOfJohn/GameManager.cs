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

            tempAnimationSetList.Add(new Sprite.AnimationSet("IDLE", playerTexture, new Point(60, 50), new Point(1, 1), new Point(0, 0), 1000));
            tempAnimationSetList.Add(new Sprite.AnimationSet("WALK", playerTexture, new Point(60, 50), new Point(4, 3), new Point(0, 0), 50));
            tempAnimationSetList.Add(new Sprite.AnimationSet("Jump", playerTexture, new Point(60, 50), new Point(4, 1), new Point(0, 150), 50));

            player = new Player(playerTexture, new Vector2((myGame.windowSize.X - 60) / 2, (myGame.windowSize.Y - 50) / 2), myGame, Keys.A, Keys.D, Keys.Space, tempAnimationSetList);

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
            spriteBatch.End();

            base.Draw(gameTime);
        }
	}
}