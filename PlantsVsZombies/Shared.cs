using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantsVsZombies
{
    internal class Shared
    {
        public static Vector2 stage;

        public static Vector2 mousePosition;

        public static Vector2 lastClicked;

        public static bool mReleased = true;

        public static GraphicsDeviceManager graphics;

        public static int currentLevel;
    }
}
