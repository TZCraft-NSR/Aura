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
    public class DialogManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;
        ContentManager Content;
        Texture2D dialogTexture;
        Rectangle position;

        Color normal = Color.White;
        Color hilite = Color.Yellow;

        public Dialog dialog = null;
        public NPC npc = null;
        int currentHandler = 0;

        Game1 myGame;

        public DialogManager(Game1 game) : base(game)
        {
            myGame = game;
            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            Content = (ContentManager)Game.Services.GetService(typeof(ContentManager));
            LoadContent();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            dialogTexture = Content.Load<Texture2D>(@"GUI\conversationbox");
            position = new Rectangle(((int)myGame.windowSize.X - dialogTexture.Width) / 2, ((int)myGame.windowSize.Y - dialogTexture.Height) / 2, dialogTexture.Width, dialogTexture.Height);
            spriteFont = Content.Load<SpriteFont>(@"normal");
        }

        public override void Update(GameTime gameTime)
        {
            if (dialog != null && npc != null)
            {
                if (myGame.CheckKey(Keys.Up))// || Game1.CheckButton(Buttons.DPadUp))
                {
                    currentHandler--;
                    if (currentHandler < 0)
                        currentHandler = dialog.handlers.Count - 1;
                }
                if (myGame.CheckKey(Keys.Down))// || Game1.CheckButton(Buttons.DPadDown))
                {
                    currentHandler++;
                    if (currentHandler == dialog.handlers.Count)
                        currentHandler = 0;
                }
                if (myGame.CheckKey(Keys.Enter))// || Game1.CheckButton(Buttons.B))
                {
                    dialog.InvokeHandler(npc, currentHandler);
                    currentHandler = 0;
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            spriteBatch.Draw(dialogTexture, position, Color.White);

            if (dialog == null)
                return;

            string text = WrapText(dialog.text);
            Vector2 textPosition = new Vector2(position.X + 12, position.Y + 12);
            spriteBatch.DrawString(spriteFont, text, textPosition, Color.White);

            int menuY = (int)spriteFont.MeasureString(text).Y + spriteFont.LineSpacing;
            textPosition.Y = position.Y + menuY + 10;
            for (int i = 0; i < dialog.handlers.Count; i++)
            {
                Color textColor = normal;
                text = dialog.handlers[i].caption;
                if (i == currentHandler)
                    textColor = hilite;
                spriteBatch.DrawString(spriteFont, text, textPosition, textColor);
                textPosition.Y += spriteFont.LineSpacing;
            }
        }

        private string WrapText(string text)
        {
            StringBuilder sb = new StringBuilder();

            float spaceWidth = spriteFont.MeasureString(" ").X;
            float length = 0f;

            string[] words = text.Split(' ');

            foreach (string word in words)
            {
                float size = spriteFont.MeasureString(word).X;

                if (length + size < position.Width - 36)
                {
                    sb.Append(word + " ");
                    length += size + spaceWidth;
                }
                else
                {
                    sb.Append("\n" + word + " ");
                    length = size + spaceWidth;
                }
            }

            return sb.ToString();
        }

        public void Show()
        {
            Enabled = true;
            Visible = true;
        }

        public void Hide()
        {
            Enabled = false;
            Visible = false;
        }
    }
}
