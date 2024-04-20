using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantsVsZombies
{
    public class Levels
    {
        public int levelNum;
        public int[] activeLanes;
        public int spawnInterval1;
        public int spawnInterval2;
        public int spawnInterval3;
        public int limitTimerZombies;
        public int spaceZ1Moves;
        public int spaceZ2Moves;
        public int spaceZ3Moves;
        public int[] zombie1Amount;
        public int[] zombie2Amount;
        public int[] zombie3Amount;
        public static Random rnd = new Random();
        public Levels() {
        }

    }
    public class Level1 : Levels
    {
        public Level1() {
            levelNum = 1;
            activeLanes = new int[] {1,2,3};
            spawnInterval1 = 70;
            spawnInterval2 = 120;
            spawnInterval3 = 0;
            limitTimerZombies = 1;
            spaceZ1Moves = 1;
            spaceZ2Moves = 3;
            spaceZ3Moves = 4;
            zombie1Amount = new int[12];
            zombie2Amount = new int[3];
            zombie3Amount = new int[0];

            for(int i=0; i < zombie1Amount.Length; i++)
            {
                zombie1Amount[i]= rnd.Next(activeLanes[0], activeLanes[activeLanes.Length - 1] + 1);
            }
            for (int i = 0; i < zombie2Amount.Length; i++)
            {
                zombie2Amount[i] = rnd.Next(activeLanes[0], activeLanes[activeLanes.Length - 1] + 1);
            }


        }

    }
    public class Level2 : Levels
    {
        public Level2()
        {
            levelNum = 2;
            activeLanes = new int[] { 0, 1, 2, 3, 4 };
            spawnInterval1 = 70;
            spawnInterval2 = 170;
            spawnInterval3 = 230;
            limitTimerZombies = 1;
            spaceZ1Moves = 2;
            spaceZ2Moves = 4;
            spaceZ3Moves = 5;
            zombie1Amount =new int[5];
            zombie2Amount = new int[7];
            zombie3Amount = new int[3];

            for (int i = 0; i < zombie1Amount.Length; i++)
            {
                zombie1Amount[i] = rnd.Next(activeLanes[0], activeLanes[activeLanes.Length - 1]+1);
            }
            for (int i = 0; i < zombie2Amount.Length; i++)
            {
                zombie2Amount[i] = rnd.Next(activeLanes[0], activeLanes[activeLanes.Length - 1]+1);
            }
            for(int i = 0;i < zombie3Amount.Length; i++)
            {
                zombie3Amount[i] = rnd.Next(activeLanes[0], activeLanes[activeLanes.Length - 1] + 1);
            }
        }

}
}
