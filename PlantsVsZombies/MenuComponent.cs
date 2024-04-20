/*
 * 
 * Names: Caroline and Zahra
 * 
*/
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantsVsZombies
{
    internal class MenuComponent : DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private SpriteFont regularFont, highlightFont;
        private Rectangle[] btnRect;
        private string[] menuItems;
        private Texture2D menuBack;
        private Texture2D menuTitle;
        private Texture2D btnBack;
        private bool flag = false;


        public int SelectedIndex { get; set; }
        public static int chosenPage;
        private Vector2 position;

        public MenuComponent(Game game,
            SpriteBatch spriteBatch,
            SpriteFont regularFont,
            SpriteFont highlightFont,
            Texture2D menuBack,
            string[] menuItems) : base(game)
        {
            Main g = (Main) game;
            this.spriteBatch = spriteBatch;
            this.regularFont = regularFont;
            this.highlightFont = highlightFont;
            this.menuItems = menuItems;
            this.menuBack = menuBack;
            this.btnRect = new Rectangle[menuItems.Length];
            position = new Vector2(Shared.stage.X / 2 - 90, Shared.stage.Y / 2 - 90);
            menuTitle = g.Content.Load<Texture2D>("Main Menu");
            btnBack = g.Content.Load<Texture2D>("btnBack");
        }


        public override void Update(GameTime gameTime)
        {
            flag = false;
            for (int j=0; j < menuItems.Length; j++)
            {
                if (btnRect[j].Contains(Shared.mousePosition.ToPoint()) && !flag)
                {
                    SelectedIndex = j;
                    flag = true;

                }
                else if (!flag)
                {
                    SelectedIndex = -1;
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {

                Vector2 tempPos = position;
                Vector2 textPos;

            spriteBatch.Begin();
            spriteBatch.Draw(menuBack, new Rectangle(0, 0, (int)Shared.stage.X, (int)Shared.stage.Y), Color.White);
            spriteBatch.Draw(menuTitle, new Vector2 ((Shared.stage.X/2 - (menuTitle.Width/2)), Shared.stage.Y / 6), Color.White);

                for (int i = 0; i < menuItems.Length; i++)
                {
                    textPos = new Vector2(tempPos.X + 40, tempPos.Y + 10);
                    btnRect[i] = new Rectangle((int)(Shared.stage.X / 2 - (300 / 2)), (int)tempPos.Y-10, 300, highlightFont.LineSpacing + 80);
                if (SelectedIndex == i )
                    {
                        spriteBatch.Draw(btnBack, btnRect[i], Color.White);
                    }
                else
                {
                    spriteBatch.Draw(btnBack, btnRect[i], Color.Green);
                }
                spriteBatch.DrawString(regularFont, menuItems[i], textPos, Color.White);
                    tempPos.Y += highlightFont.LineSpacing + 70;
                }
                spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
