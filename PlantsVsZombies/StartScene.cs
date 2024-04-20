/*
 * 
 * Names: Caroline and Zahra
 * 
*/
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantsVsZombies
{
    internal class StartScene : GameScene
    {
        private MenuComponent menu;
        public MenuComponent Menu { get => menu; set => menu = value; }


        private SpriteBatch spriteBatch;
        string[] menuItems = { "Start game", "Help", "Credit", "Quit" };

        public StartScene(Game game) : base(game)
        {
            Main g = (Main)game;
            this.spriteBatch = g._spriteBatch;
            SpriteFont regularFont = g.Content.Load<SpriteFont>("FontMenu");
            SpriteFont highlightFont = g.Content.Load<SpriteFont>("FontMenuHighlight");
            Texture2D menuBackground = g.Content.Load<Texture2D>("Menu Background");
            menu = new MenuComponent(g, spriteBatch, regularFont, highlightFont,menuBackground, menuItems);
            this.Components.Add(menu);

        }

    }
}
