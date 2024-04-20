using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PlantsVsZombies
{
    public class Plants
    {
        public Texture2D texture;
        public int life;
        public int price;
        public int plantType = 0;
        public int lane;
        public Vector2 pos;
        public Plants()
        {
            
        }
    }
    public class PlantShoot : Plants 
    {
        public int ProjectileLimitTimer = 60;
        public Texture2D ballTexture;
        public int counterTimeProjectile = 59;
        public PlantShoot(int lan,Texture2D ball,Texture2D tx, int type)
        {
            ballTexture = ball;
            texture = tx;
            life = 1000;
            price = 100;
            plantType = type;
            lane = lan;
        }

    }
    public class PlantSun : Plants
    {
        public int sunflowerSunCounterTime = 0;
        public int sunflowerSunTimerLimit = 50;
        public Texture2D textureSun;
        public PlantSun(Texture2D sun,int lan, Texture2D tx, int type)
        {
            texture = tx;
            life = 500;
            price = 50;
            plantType = type;
            lane = lan;
            textureSun = sun;
        }
    }
    public class PlantDefense : Plants
    {
        public PlantDefense(int lan,Texture2D tx, int type)
        {
            texture = tx;
            life = 1500;
            price = 50;
            plantType = type;
            lane = lan;
        }
    }

    public class Projectile
    {
        public int locX;
        public int locY;
        public int width = 40;
        public int height = 40;
        public int damage = 50;
        public Texture2D texture;
        public int lane;
        public int locXInit;

        public Projectile(int locInit,int lan,Texture2D tex ,int x, int y) {
            locX = x;
            locY = y;
            texture = tex;
            lane = lan;
            locXInit = locInit;
        }
    }
    public class SunflowerSun
    {
        public int locX;
        public int locY;
        public int width = 80;
        public int height = 80;
        public int points = 20;
        public Texture2D texture;
        public Rectangle rectangle;
        public SunflowerSun(Texture2D tex, int x, int y)
        {
            locX = x;
            locY = y;
            texture = tex;
            rectangle = new Rectangle(locX, locY, width, height);
        }
    }
}
