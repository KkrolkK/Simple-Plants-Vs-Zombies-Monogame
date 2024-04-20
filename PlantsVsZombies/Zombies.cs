using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PlantsVsZombies
{
    public class Zombies
    {
        public int lane;
        public int life;
        public int damage;
        public int positionUpdate, spawnInterval;
        public Texture2D texture;
        public int width, height, locX, locY;
        public bool activeZombie;
        public int currentCol;
        public Zombies() { }
    }

    public class Zombie1: Zombies
    {
        public Zombie1(int col,int la,Texture2D tx, int x, int y, int wid, int hei) 
        {
            lane = la;
            texture = tx;
            life = 60;
            damage = 20;
            positionUpdate = 1;
            width = wid;
            height = hei;
            locX = x;
            locY = y;
            currentCol = col;
        }
    }
    public class Zombie2: Zombies
    {
        public Zombie2(int col,int la, Texture2D tx, int x, int y, int wid, int hei)
        {
            lane = la;
            texture = tx;
            life = 60;
            damage = 20;
            positionUpdate = 1;
            width = wid;
            height = hei;
            locX = x;
            locY = y;
            currentCol = col;

        }
    }
    public class Zombie3 : Zombies
    {
        public Zombie3(int col, int la, Texture2D tx, int x, int y, int wid, int hei)
        {
            lane = la;
            texture = tx;
            life = 60;
            damage = 20;
            positionUpdate = 1;
            width = wid;
            height = hei;
            locX = x;
            locY = y;
            currentCol = col;

        }
    }
}
