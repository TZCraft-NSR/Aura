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

            List<Sprite.AnimationSet> tempAnimationSetList = new List<Sprite.AnimationSet>();

            tempAnimationSetList.Add(new Sprite.AnimationSet("IDLE", playerTexture, new Point(60, 50), new Point(0, 0), new Point(0, 0), 17000));
            tempAnimationSetList.Add(new Sprite.AnimationSet("WALK", playerTexture, new Point(60, 50), new Point(4, 3), new Point(0, 0), 17000));
            tempAnimationSetList.Add(new Sprite.AnimationSet("Jump", playerTexture, new Point(60, 50), new Point(5, 1), new Point(0, 150), 17000));

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
            player.Draw(gameTime, spriteBatch);

            base.Draw(gameTime);
        }
	}
}