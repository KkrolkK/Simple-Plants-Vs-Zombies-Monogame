/*
 * 
 * Names: Caroline and Zahra
 * 
*/
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace PlantsVsZombies
{
    public class Main : Game
    {
        private GraphicsDeviceManager _graphics;
        public SpriteBatch _spriteBatch;

        MouseState mouseState;

        private StartScene startScene;
        private ActionScene actionScene;
        private HelpScene helpScene;
        private LevelWinScene levelWinScene;
        private LoseScene loseScene;
        private CreditScene creditScene;

        public Main()
        {
            _graphics = new GraphicsDeviceManager(this);
            Shared.graphics = _graphics;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = 1440;
            _graphics.PreferredBackBufferHeight = 900;
            Shared.stage = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            Shared.lastClicked = new Vector2(0, 0);
            Shared.currentLevel = 1;
            _graphics.ApplyChanges();


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            startScene  = new StartScene(this);
            this.Components.Add(startScene);

            helpScene = new HelpScene(this);
            this.Components.Add(helpScene);

            levelWinScene = new LevelWinScene(this);
            this.Components.Add(levelWinScene);

            loseScene = new LoseScene(this);
            this.Components.Add(loseScene);

            actionScene = new ActionScene(this);
            this.Components.Add(actionScene);

            creditScene = new CreditScene(this);
            this.Components.Add(creditScene);

            Song song = this.Content.Load<Song>("soundTrack");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(song);

            startScene.show();
        }

        private void hideAllScenes()
        {
            foreach (GameScene item in Components)
            {
                item.hide();
            }
        }

        protected override void Update(GameTime gameTime)
        {
            int selectedIndex;

            mouseState = Mouse.GetState();

            Shared.mousePosition = mouseState.Position.ToVector2();

            if(mouseState.LeftButton == ButtonState.Pressed && Shared.mReleased)
            {
                Shared.lastClicked = Shared.mousePosition;
                Shared.mReleased = false;

            }
            if (mouseState.LeftButton == ButtonState.Released)
            {
                Shared.lastClicked = new Vector2(0,0);
                Shared.mReleased = true;
            }

            KeyboardState ks = Keyboard.GetState();
            if (startScene.Enabled)
            {
                selectedIndex = startScene.Menu.SelectedIndex;
                if (selectedIndex == 0 && !Shared.mReleased)
                {
                    Shared.lastClicked = new Vector2 (0,0);
                    startScene.hide();
                    actionScene.show();
                }
                else if (selectedIndex == 1 && !Shared.mReleased)
                {
                    startScene.hide();
                    helpScene.show();
                }
                else if (selectedIndex == 2 && !Shared.mReleased)
                {
                    startScene.hide();
                    creditScene.show();
                }
                else if ((selectedIndex == 3 && !Shared.mReleased))
                {
                    Exit();
                }
            }

            else if (actionScene.Enabled)
            {
                if (ActionScene.gameCondition == 1)
                {
                    ActionScene.gameCondition = 0;
                    hideAllScenes();
                    levelWinScene.show();
                }
                else if (ActionScene.gameCondition == 2)
                {
                    ActionScene.gameCondition = 0;
                    hideAllScenes();
                    loseScene.show();
                }
                if (ks.IsKeyDown(Keys.Escape))
                {
                    hideAllScenes();
                    startScene.show();
                }
            }
            else if (levelWinScene.Enabled)
            {
                if (LevelWinScene.nextLevelStart)
                {
                    hideAllScenes();
                    LevelWinScene.nextLevelStart = false;
                    actionScene.reload();
                    actionScene.show();
                }
            }
            else if (loseScene.Enabled)
            {
                if (LoseScene.backToMenu)
                {
                    hideAllScenes();
                    LoseScene.backToMenu = false;
                    actionScene.reload();
                    startScene.show();
                }
            }
            if (helpScene.Enabled || creditScene.Enabled)
            {
                if (ks.IsKeyDown(Keys.Escape))
                {
                    hideAllScenes();
                    startScene.show();
                }
            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);

        }
    }
}
