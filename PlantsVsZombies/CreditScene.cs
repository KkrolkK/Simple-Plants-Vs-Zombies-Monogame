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
    internal class CreditScene : GameScene
    {
        public static bool backToMenu;

        private SpriteBatch spriteBatch;
        Texture2D menuBack;

        static int counterTime = 0, limitTime = 50;
        static float countDuration = 0.1f;
        static float currentTime = 0f;
        public CreditScene(Game game) : base(game)
        {
            Main g = (Main)game;
            this.spriteBatch = g._spriteBatch;

            menuBack = g.Content.Load<Texture2D>("credits");
        }
        public override void Update(GameTime gameTime)
        {
            currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (currentTime >= countDuration)
            {
                counterTime++;
                currentTime -= countDuration;
            }
            if (counterTime >= limitTime)
            {
                counterTime = 0;
                Shared.currentLevel = 1;
                backToMenu = true;
            }
        }
        public override void Draw(GameTime gameTime)
        {

            spriteBatch.Begin();
            spriteBatch.Draw(menuBack, new Rectangle(0, 0, (int)Shared.stage.X, (int)Shared.stage.Y), Color.White);
          //  spriteBatch.Draw(menuTitle, new Vector2((Shared.stage.X / 2 - menuTitle.Width / 2), (Shared.stage.Y / 2 - menuTitle.Height / 2)), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}