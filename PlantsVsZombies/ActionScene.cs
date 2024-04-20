using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Metrics;

namespace PlantsVsZombies
{
    internal class ActionScene : GameScene
    {
        public static int gameCondition = 0;

        private readonly SpriteBatch spriteBatch;
        private readonly SpriteFont spriteFont; //font to write
        private readonly Texture2D background, plant1, plant2, plant3, sun, zombie1, zombie2, zombie3, ballTexture; //variable to save textures (images)
        private static Texture2D blankTex; //plain color texture
        private static readonly SoundEffect shoot;

        private Plants[] plants;
        private List<PlantShoot> plantShootsOnMap;
        private List<PlantSun> plantSunsOnMap;
        private List<PlantDefense> plantDefensesOnMap;
        public List<Projectile> projectiles;
        public List<SunflowerSun> sunsOnMap;
        private static Plants currentPlant; //selected plant

        private List<Zombie1> zombies1;
        private List<Zombie2> zombies2;
        private List<Zombie3> zombies3;

        private static int zombie1Counter = 0, zombie2Counter = 0, zombie3Counter = 0, sunCount = 0, maxZombies;

        private static readonly int cols = 9, rows = 5, boardX = 360, boardY = 80;
        public static int rectWidth, rectHeight; //size of game rectangle 
        private Levels currentLvl;
        private readonly int plantOptions = 3;
        private static plantCard[] plantCards;

        private Rectangle board;
        public static boardTile[,] boardRectangles;

        static int counterTimeSun = 1,counterTimeZ1 = 1,counterTimeZ2  = 1, counterTimeZ3 = 1, counterTimeWalk1 = 0, counterTimeWalk2 = 0, counterTimeWalk3 = 0, limitTime = 10;
        static int counterTimeProjectile = 0, limitTimeProjectile = 1;
        static float countDuration = 0.1f;
        static float currentTime = 0f;

        public struct boardTile //each board place
        {
            public int hasPlant;
            public Rectangle boardRectangle;
            public Texture2D rectTexture;
            public Plants plant;
        }
        struct plantCard //plant selectors left
        {
            public int plantNum;
            public Rectangle plantImage;
            public Texture2D plantTex;
            public Texture2D priceTex;
            public Rectangle priceRectangle;
            public int priceNum;
            public bool canBuy;
        }
        public ActionScene(Game game) : base(game)
        {
            Main g = (Main)game;
            this.spriteBatch = g._spriteBatch;
            board = new Rectangle(boardX, boardY, (int)Math.Floor((Shared.stage).X/1.4), (int)Math.Floor((Shared.stage).Y/1.2));

            background = g.Content.Load<Texture2D>("Lawn Background");
            plant3 = g.Content.Load<Texture2D>("plant_sun");
            plant2 = g.Content.Load<Texture2D>("plant_shield");
            plant1 = g.Content.Load<Texture2D>("plant_green");
            zombie1 = g.Content.Load<Texture2D>("zombie1");
            zombie2 = g.Content.Load<Texture2D>("zombie2");
            zombie3 = g.Content.Load<Texture2D>("zombie3");
            ballTexture = g.Content.Load<Texture2D>("projectile");
            sun = g.Content.Load<Texture2D>("sun");

            spriteFont = g.Content.Load<SpriteFont>("FontMenu");

            plants = new Plants[plantOptions];
            currentPlant = new Plants();
            plants[0] = new PlantShoot(-1, ballTexture, plant1, 1);
            plants[1] = new PlantDefense(-1, plant2, 2);
            plants[2] = new PlantSun(sun, -1, plant3, 3);

            plantDefensesOnMap = new();
            plantShootsOnMap = new();
            plantSunsOnMap = new();
            projectiles = new();
            sunsOnMap = new();

            boardRectangles = new boardTile[rows, cols];
            plantCards = new plantCard[plantOptions];

            rectWidth = board.Width / cols;
            rectHeight = board.Height / rows;


            if (Shared.currentLevel == 1) currentLvl = new Level1();
            else currentLvl = new Level2();


            zombies1 = new();
            zombies2 = new();
            zombies3 = new();

            Color[] color1 = new Color[rectWidth * rectHeight];
            Color[] color2 = new Color[rectWidth * rectHeight];
            for (int i = 0; i < color1.Length; ++i)
            {
                color1[i] = Color.Green;
                color2[i] = Color.DarkGreen;
            }

            blankTex = new Texture2D(Shared.graphics.GraphicsDevice, rectWidth + 20, rectHeight * (plantOptions + 2) + 20);
            Color[] color = new Color[(rectWidth + 20) * (20 + rectHeight * (plantOptions + 2))];
            for (int i = 0; i < color.Length; ++i)
            {
                color[i] = Color.White;
            }
            blankTex.SetData(color);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    boardRectangles[i, j] = new boardTile()
                    {
                        boardRectangle = new Rectangle(((rectWidth * j) + boardX), ((rectHeight * i) + boardY), rectWidth, rectHeight),
                        rectTexture = new Texture2D(Shared.graphics.GraphicsDevice, rectWidth, rectHeight),
                        hasPlant = 0,
                        plant = new Plants()
                    };
                    if ((i % 2 == 0 && j % 2 == 0) || (i % 2 != 0 && j % 2 != 0)) boardRectangles[i, j].rectTexture.SetData(color1);
                    else boardRectangles[i, j].rectTexture.SetData(color2);
                }
            }
            int posY = boardRectangles[1, 0].boardRectangle.Y;
            for (int i = 0; i < plantOptions; ++i)
            {
                plantCards[i] = new plantCard()
                {
                    plantTex = plants[i].texture,
                    priceTex = blankTex,
                    plantImage = new Rectangle(20, (i * rectHeight) + posY, rectWidth, rectHeight * 3 / 4),
                    priceRectangle = new Rectangle(20, (i * rectHeight) + posY, rectWidth, rectHeight),
                    plantNum = i + 1,
                    priceNum = plants[i].price
                };
                posY += 5;
            }

            for (int i = 0; i < currentLvl.zombie1Amount.Length; i++)
            {
                zombies1.Add(new Zombie1(cols - 1, currentLvl.zombie1Amount[i], zombie1, (int)Shared.stage.X, boardRectangles[currentLvl.zombie1Amount[i], 0].boardRectangle.Y, rectWidth, rectHeight));
            }
            for (int i = 0; i < currentLvl.zombie2Amount.Length; i++)
            {
                zombies2.Add(new Zombie2(cols - 1, currentLvl.zombie2Amount[i], zombie2, (int)Shared.stage.X, boardRectangles[currentLvl.zombie2Amount[i], 0].boardRectangle.Y, rectWidth, rectHeight));
            }
            for (int i = 0; i < currentLvl.zombie3Amount.Length; i++)
            {
                zombies3.Add(new Zombie3(cols - 1, currentLvl.zombie3Amount[i], zombie3, (int)Shared.stage.X, boardRectangles[currentLvl.zombie3Amount[i], 0].boardRectangle.Y, rectWidth, rectHeight));
            }
            maxZombies = Math.Max(Math.Max(zombies1.Count, zombies2.Count), zombies3.Count);

        }

        public void reload() // Reset defaults for game to start in new level
        {
            if (Shared.currentLevel == 1) currentLvl = new Level1();
            else currentLvl = new Level2(); 

            sunCount = 0;
            zombie1Counter = 0;
            zombie2Counter = 0;
            zombie3Counter = 0;
            sunCount = 0;
            counterTimeSun = 1;
            counterTimeZ1 = 1;
            counterTimeZ2 = 1;
            counterTimeZ3 = 1;
            counterTimeWalk1 = 0;
            counterTimeWalk2 = 0;
            counterTimeWalk3 = 0;
            counterTimeProjectile = 0;
            plants = new Plants[plantOptions];
            currentPlant = new Plants();
            plants[0] = new PlantShoot(-1, ballTexture, plant1, 1);
            plants[1] = new PlantDefense(-1, plant2, 2);
            plants[2] = new PlantSun(sun, -1, plant3, 3);

            plantDefensesOnMap = new();
            plantShootsOnMap = new();
            plantSunsOnMap = new();
            projectiles = new();
            sunsOnMap = new();

            boardRectangles = new boardTile[rows, cols];
            plantCards = new plantCard[plantOptions];

            rectWidth = board.Width / cols;
            rectHeight = board.Height / rows;

            zombies1 = new();
            zombies2 = new();
            zombies3 = new();

            Color[] color1 = new Color[rectWidth * rectHeight];
            Color[] color2 = new Color[rectWidth * rectHeight];
            for (int i = 0; i < color1.Length; ++i)
            {
                color1[i] = Color.Green;
                color2[i] = Color.DarkGreen;
            }

            blankTex = new Texture2D(Shared.graphics.GraphicsDevice, rectWidth + 20, rectHeight * (plantOptions + 2) + 20);
            Color[] color = new Color[(rectWidth + 20) * (20 + rectHeight * (plantOptions + 2))];
            for (int i = 0; i < color.Length; ++i)
            {
                color[i] = Color.White;
            }
            blankTex.SetData(color);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    boardRectangles[i, j] = new boardTile()
                    {
                        boardRectangle = new Rectangle(((rectWidth * j) + boardX), ((rectHeight * i) + boardY), rectWidth, rectHeight),
                        rectTexture = new Texture2D(Shared.graphics.GraphicsDevice, rectWidth, rectHeight),
                        hasPlant = 0,
                        plant = new Plants()
                    };
                    if ((i % 2 == 0 && j % 2 == 0) || (i % 2 != 0 && j % 2 != 0)) boardRectangles[i, j].rectTexture.SetData(color1);
                    else boardRectangles[i, j].rectTexture.SetData(color2);
                }
            }
            int posY = boardRectangles[1, 0].boardRectangle.Y;
            for (int i = 0; i < plantOptions; ++i)
            {
                plantCards[i] = new plantCard()
                {
                    plantTex = plants[i].texture,
                    priceTex = blankTex,
                    plantImage = new Rectangle(20, (i * rectHeight) + posY, rectWidth, rectHeight * 3 / 4),
                    priceRectangle = new Rectangle(20, (i * rectHeight) + posY, rectWidth, rectHeight),
                    plantNum = i + 1,
                    priceNum = plants[i].price
                };
                posY += 5;
            }

            for (int i = 0; i < currentLvl.zombie1Amount.Length; i++)
            {
                zombies1.Add(new Zombie1(cols - 1, currentLvl.zombie1Amount[i], zombie1, (int)Shared.stage.X, boardRectangles[currentLvl.zombie1Amount[i], 0].boardRectangle.Y, rectWidth, rectHeight));
            }
            for (int i = 0; i < currentLvl.zombie2Amount.Length; i++)
            {
                zombies2.Add(new Zombie2(cols - 1, currentLvl.zombie2Amount[i], zombie2, (int)Shared.stage.X, boardRectangles[currentLvl.zombie2Amount[i], 0].boardRectangle.Y, rectWidth, rectHeight));
            }
            for (int i = 0; i < currentLvl.zombie3Amount.Length; i++)
            {
                zombies3.Add(new Zombie3(cols - 1, currentLvl.zombie3Amount[i], zombie3, (int)Shared.stage.X, boardRectangles[currentLvl.zombie3Amount[i], 0].boardRectangle.Y, rectWidth, rectHeight));
            }
            maxZombies = Math.Max(Math.Max(zombies1.Count, zombies2.Count), zombies3.Count);
        }

        public override void Update(GameTime gameTime)
        {
            
            base.Update(gameTime);
            currentPlant.pos = Shared.mousePosition;

            currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (currentTime >= countDuration) //timer counters 
            {
                counterTimeSun ++;
                counterTimeZ1++;
                counterTimeZ2++;
                counterTimeZ3++;
                counterTimeWalk1++;
                counterTimeWalk2++;
                counterTimeWalk3++;
                counterTimeProjectile++;
                foreach (PlantShoot plant in plantShootsOnMap)
                    plant.counterTimeProjectile++;
                foreach (PlantSun plant in plantSunsOnMap)
                    plant.sunflowerSunCounterTime++;
                currentTime -= countDuration; 
            }
            if (counterTimeSun >= limitTime) // sun (money) increase 
            {
                counterTimeSun = 0;
                sunCount+=10;
            }
            foreach(PlantSun plant in plantSunsOnMap) // sun generation from plants
            {
                if(plant.sunflowerSunCounterTime >= plant.sunflowerSunTimerLimit)
                {
                    sunsOnMap.Add(new SunflowerSun(plant.textureSun, (int)plant.pos.X, (int)plant.pos.Y));
                    plant.sunflowerSunCounterTime = 0;
                }
            }
            if (counterTimeProjectile >= limitTimeProjectile) // projectile movement and damage to zombie
            {
                for (int i = 0; i < projectiles.Count; i++)
                {
                    projectiles[i].locX += 5;

                    for(int j=0; j < maxZombies; j++)
                    {
                        if (projectiles.Count > i && j < zombies1.Count && zombies1[j].lane == projectiles[i].lane && zombies1[j].locX <= projectiles[i].locX + projectiles[i].width && projectiles[i].locXInit <= zombies1[j].locX)
                        {
                            zombies1[j].life -= projectiles[i].damage;
                            projectiles.Remove(projectiles[i]);
                            //damage to zombie                        
                            if (zombies1[j].life <= 0)
                            {
                                zombies1.Remove(zombies1[j]); //kill zombie
                            }
                        }
                        if (projectiles.Count > i && j < zombies2.Count && zombies2[j].lane == projectiles[i].lane && zombies2[j].locX <= projectiles[i].locX + projectiles[i].width && projectiles[i].locXInit <= zombies2[j].locX)
                        {
                            zombies2[j].life -= projectiles[i].damage;
                            projectiles.Remove(projectiles[i]);
                            //damage to zombie                        
                            if (zombies2[j].life <= 0)
                            {
                                zombies2.Remove(zombies2[j]); //kill zombie
                            }
                        }
                        if (projectiles.Count > i && j < zombies3.Count && zombies3[j].lane == projectiles[i].lane && zombies3[j].locX <= projectiles[i].locX + projectiles[i].width && projectiles[i].locXInit <= zombies3[j].locX)
                        {
                            zombies3[j].life -= projectiles[i].damage;
                            projectiles.Remove(projectiles[i]);
                            //damage to zombie                        
                            if (zombies3[j].life <= 0)
                            {
                                zombies3.Remove(zombies3[j]); //kill zombie
                            }
                        }
                    }
                }
                counterTimeProjectile = 0;
            }
            for (int i = 0; i < plantShootsOnMap.Count; i++)  // projectile generation for each plant
            {
                var plant = plantShootsOnMap[i];
                if (plantShootsOnMap[i].counterTimeProjectile >= plantShootsOnMap[i].ProjectileLimitTimer)
                {
                    projectiles.Add(new Projectile((int)plant.pos.X ,plant.lane, plant.ballTexture, plant.pos.ToPoint().X, plant.pos.ToPoint().Y));
                    plantShootsOnMap[i].counterTimeProjectile = 0;
                }
            }
            if (currentLvl.zombie1Amount.Length > 0 && currentLvl.spawnInterval1 > 0 && zombie1Counter < zombies1.Count && counterTimeZ1 >= currentLvl.spawnInterval1)
            {
                //zombie type 1 generation
                zombies1[zombie1Counter].activeZombie = true;
                zombie1Counter++;
                counterTimeZ1 = 0; 
            }
            if (currentLvl.zombie2Amount.Length > 0 && currentLvl.spawnInterval2 > 0 && zombie2Counter < zombies2.Count && counterTimeZ2 >= currentLvl.spawnInterval2)
            {
                //zombie type 2 generation
                zombies2[zombie2Counter].activeZombie = true;
                zombie2Counter++;
                counterTimeZ2 = 0;
            }
            if (currentLvl.zombie3Amount.Length > 0 && currentLvl.spawnInterval3 > 0 && zombie3Counter < zombies3.Count && counterTimeZ3 >= currentLvl.spawnInterval3)
            {
                //zombie type 3 generation
                zombies3[zombie3Counter].activeZombie = true;
                zombie3Counter++;
                counterTimeZ3 = 0;
            }
            if (zombies1.Count > 0 && counterTimeWalk1 >= currentLvl.limitTimerZombies) //zombie movement and damage to plants
            {
                for (int i = 0; i < zombies1.Count; i++)
                {
                    if (zombies1[i].currentCol >= 0 && zombies1[i].activeZombie && boardRectangles[zombies1[i].lane, zombies1[i].currentCol].hasPlant <= 0)
                    {

                        zombies1[i].locX -= currentLvl.spaceZ1Moves;

                        if (zombies1[i].locX <= boardRectangles[zombies1[i].lane, zombies1[i].currentCol].boardRectangle.X)
                        {
                            zombies1[i].currentCol--;
                        }
                    }
                    else if (zombies1[i].currentCol == -1)
                    {
                        gameCondition = 2; 
                    }
                    else if (zombies1[i].currentCol >= 0 && boardRectangles[zombies1[i].lane, zombies1[i].currentCol].hasPlant > 0)
                    {
                        var plant = boardRectangles[zombies1[i].lane, zombies1[i].currentCol].plant;
                        plant.life -= zombies1[i].damage;

                        if (plant.life <= 0)
                        {
                            if (plant.plantType == 1) plantShootsOnMap.Remove((PlantShoot)plant);
                            else if (plant.plantType == 2) plantDefensesOnMap.Remove((PlantDefense)plant);
                            else if (plant.plantType == 3) plantSunsOnMap.Remove((PlantSun)plant);
                            boardRectangles[zombies1[i].lane, zombies1[i].currentCol].hasPlant = 0;
                            plant = new Plants();
                        }
                    }
                }
                counterTimeWalk1 = 0;
            }
            if (zombies2.Count > 0 && counterTimeWalk2 >= currentLvl.limitTimerZombies) //zombie movement and damage to plants
            {
                for (int i = 0; i < zombies2.Count; i++)
                {
                    if (zombies2[i].currentCol >= 0 && zombies2[i].activeZombie && boardRectangles[zombies2[i].lane, zombies2[i].currentCol].hasPlant <= 0 && zombies2[i].currentCol >= 0)
                    {

                        zombies2[i].locX -= currentLvl.spaceZ2Moves;

                        if (zombies2[i].locX <= boardRectangles[zombies2[i].lane, zombies2[i].currentCol].boardRectangle.X)
                        {
                            zombies2[i].currentCol--;
                        }
                    }
                    else if (zombies2[i].currentCol == -1)
                    {
                        gameCondition = 2;
                    }
                    else if (zombies2[i].currentCol >= 0 && boardRectangles[zombies2[i].lane, zombies2[i].currentCol].hasPlant > 0)
                    {
                        var plant = boardRectangles[zombies2[i].lane, zombies2[i].currentCol].plant;
                        plant.life -= zombies2[i].damage;

                        if (plant.life <= 0)
                        {
                            if (plant.plantType == 1) plantShootsOnMap.Remove((PlantShoot)plant);
                            else if (plant.plantType == 2) plantDefensesOnMap.Remove((PlantDefense)plant);
                            else if (plant.plantType == 3) plantSunsOnMap.Remove((PlantSun)plant);
                            boardRectangles[zombies2[i].lane, zombies2[i].currentCol].hasPlant = 0;
                            plant = new Plants();
                        }
                    }
                    counterTimeWalk2 = 0;
                }
            }
            if (zombies3.Count > 0 && counterTimeWalk3 >= currentLvl.limitTimerZombies) //zombie movement and damage to plants
            {
                    for (int i = 0; i < zombies3.Count; i++)
                    {

                        if (zombies3[i].currentCol >= 0 && zombies3[i].activeZombie && boardRectangles[zombies3[i].lane, zombies3[i].currentCol].hasPlant <= 0 && zombies3[i].currentCol >= 0)
                        {

                            zombies3[i].locX -= currentLvl.spaceZ3Moves;

                            if (zombies3[i].locX <= boardRectangles[zombies3[i].lane, zombies3[i].currentCol].boardRectangle.X)
                            {
                                zombies3[i].currentCol--;
                            }
                        }
                    else if (zombies3[i].currentCol == -1)
                    {
                        gameCondition = 2;
                    }
                    else if (zombies3[i].currentCol >= 0 && boardRectangles[zombies3[i].lane, zombies3[i].currentCol].hasPlant > 0)
                        {
                            var plant = boardRectangles[zombies3[i].lane, zombies3[i].currentCol].plant;
                            plant.life -= zombies3[i].damage;

                            if (plant.life <= 0)
                            {
                                if (plant.plantType == 1) plantShootsOnMap.Remove((PlantShoot)plant);
                                else if (plant.plantType == 2) plantDefensesOnMap.Remove((PlantDefense)plant);
                                else if (plant.plantType == 3) plantSunsOnMap.Remove((PlantSun)plant);
                                boardRectangles[zombies3[i].lane, zombies3[i].currentCol].hasPlant = 0;
                                plant = new Plants();
                            }

                        }
                        counterTimeWalk3 = 0;
                    }
            }

            for (int i = 0; i < plantCards.Length; ++i) //selection of plant
            {
                plantCard p = plantCards[i];
                if (sunCount >= p.priceNum)
                {
                    p.canBuy = true;
                }

                if (p.priceRectangle.Contains(Shared.lastClicked.ToPoint()) && sunCount >= p.priceNum)
                {
                    switch (i) 
                    {
                    case 0:
 
                            currentPlant = new PlantShoot(-1,ballTexture,p.plantTex, i+1);
                            break;
                    case 1:
                            currentPlant = new PlantDefense(-1,p.plantTex, i+1);
                            break; 
                    case 2:
                            currentPlant = new PlantSun(sun,-1, p.plantTex, i+1);

                            break;
                    }
                    currentPlant.pos = Shared.mousePosition;
                }
            }

            if (currentPlant.plantType > 0) //placing the selected plant in the selected place of board
            {
                foreach (var lane in currentLvl.activeLanes)
                {
                    for (int i = 0; i < cols; i++)
                    {
                        if (boardRectangles[lane, i].boardRectangle.Contains(Shared.lastClicked.ToPoint()) && boardRectangles[lane, i].hasPlant==0)
                        {
                            boardRectangles[lane, i].hasPlant = currentPlant.plantType;
                            boardRectangles[lane, i].plant = currentPlant;
                            currentPlant.lane = lane;
                            if (currentPlant.plantType == 1) plantShootsOnMap.Add((PlantShoot)currentPlant);
                            else if (currentPlant.plantType == 2) plantDefensesOnMap.Add((PlantDefense)currentPlant);
                            else if (currentPlant.plantType == 3) plantSunsOnMap.Add((PlantSun)currentPlant);
                            
                            sunCount = sunCount - currentPlant.price;
                            currentPlant = new Plants();
                        }
                    }
                }
            }
            for (int i=0; i< sunsOnMap.Count; i++) //clicking the suns to get points
            {
                if (i < sunsOnMap.Count)
                {
                    var sun = sunsOnMap[i];
                    if (sun.rectangle.Contains(Shared.lastClicked.ToPoint()) && currentPlant.plantType <= 0)
                    {
                        sunCount += sun.points;
                        sunsOnMap.Remove(sun);
                    }
                }
            }
            if (zombies1.Count == 0 && zombies2.Count == 0 && zombies3.Count == 0) //checking win 
            {
                gameCondition = 1;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(background, new Rectangle(0, 0, (int)Shared.stage.X, (int)Shared.stage.Y), Color.White); //back
            spriteBatch.Draw(blankTex, new Vector2(10, boardRectangles[0, 0].boardRectangle.Location.Y-10), Color.SaddleBrown); //left menu

            spriteBatch.Draw(sun, new Rectangle(plantCards[0].plantImage.X, boardRectangles[0,0].boardRectangle.Y, plantCards[0].plantImage.Width, plantCards[0].plantImage.Height), Color.White);
            spriteBatch.DrawString(spriteFont, sunCount.ToString(), new Vector2(plantCards[0].plantImage.X+30, (boardRectangles[0, 0].boardRectangle.Y+ plantCards[0].plantImage.Height)), Color.White);
             
            for (int i=0; i < plantCards.Length; i++) //placing cards on menu
            {
                var card = plantCards[i];
                spriteBatch.Draw(blankTex, card.priceRectangle, Color.SandyBrown);
                if (sunCount >= card.priceNum)
                {
                    spriteBatch.Draw(card.plantTex, card.plantImage, Color.White);
                }
                else
                {
                    spriteBatch.Draw(card.plantTex, card.plantImage, Color.Black) ;
                }
                spriteBatch.DrawString(spriteFont, card.priceNum.ToString(), new Vector2(card.plantImage.X +30, card.plantImage.Y+card.plantImage.Height), Color.White);
            }

            foreach (var lane in currentLvl.activeLanes) //printing active lanes
            {
                for(int i = 0; i < cols; i++)
                {
                    var curr = boardRectangles[lane, i];
                    spriteBatch.Draw(curr.rectTexture, curr.boardRectangle, Color.White);
                    if(curr.hasPlant > 0)
                    {
                        spriteBatch.Draw(plants[curr.hasPlant-1].texture, curr.boardRectangle, Color.White);
                    }
                }
            }
            for (int i = 0; i < maxZombies; i++) //printing zombies
            {
                if(i < zombies1.Count && zombies1[i].activeZombie)
                {
                    Zombie1 z1 = zombies1[i];
                    spriteBatch.Draw(z1.texture, new Rectangle(z1.locX, z1.locY,  z1.width, z1.height), Color.White);
                }
                if(i < zombies2.Count && zombies2[i].activeZombie)
                {
                    Zombie2 z2 = zombies2[i];
                    spriteBatch.Draw(z2.texture, new Rectangle(z2.locX, z2.locY, z2.width, z2.height), Color.White);
                }
                if (i < zombies3.Count && zombies3[i].activeZombie)
                {
                    Zombie3 z3 = zombies3[i];
                    spriteBatch.Draw(z3.texture, new Rectangle(z3.locX, z3.locY, z3.width, z3.height), Color.White);
                }
            }
            foreach (Projectile projectile in projectiles) //printing projectiles
            {
                spriteBatch.Draw(projectile.texture, new Rectangle(projectile.locX, projectile.locY, projectile.width, projectile.height), Color.White);
            }
            foreach(SunflowerSun sun in sunsOnMap) //printing sun flowers
            {
                spriteBatch.Draw(sun.texture, sun.rectangle, Color.White);
            }
            if (currentPlant.plantType > 0) //printing plant currently selected by user 
                spriteBatch.Draw(currentPlant.texture, new Rectangle((int)currentPlant.pos.X - rectWidth/2,(int)currentPlant.pos.Y - rectHeight/2,rectWidth, rectHeight), Color.White);

            spriteBatch.End();
            base.Draw(gameTime);
        }

    }
}
