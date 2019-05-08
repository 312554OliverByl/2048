using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace _2048
{
    public static class Input
    {
        public static readonly int UP = 0;
        public static readonly int DOWN = 1;
        public static readonly int LEFT = 2;
        public static readonly int RIGHT = 3;

        private static bool[] keys = new bool[4];
        private static bool[] keysHeld = new bool[4];

        public static void tick()
        {
            for (int i = 0; i < keys.Length; i++)
                keys[i] = false;

            if (Keyboard.IsKeyDown(Key.Up))
            {
                if (!keysHeld[UP])
                    keys[UP] = true;
                keysHeld[UP] = true;
            }
            else
            {
                keysHeld[UP] = false;
            }
            if (Keyboard.IsKeyDown(Key.Down))
            {
                if (!keysHeld[DOWN])
                    keys[DOWN] = true;
                keysHeld[DOWN] = true;
            }
            else
            {
                keysHeld[DOWN] = false;
            }
            if (Keyboard.IsKeyDown(Key.Left))
            {
                if (!keysHeld[LEFT])
                    keys[LEFT] = true;
                keysHeld[LEFT] = true;
            }
            else
            {
                keysHeld[LEFT] = false;
            }
            if (Keyboard.IsKeyDown(Key.Right))
            {
                if (!keysHeld[RIGHT])
                    keys[RIGHT] = true;
                keysHeld[RIGHT] = true;
            }
            else
            {
                keysHeld[RIGHT] = false;
            }
        }

        public static bool wasKeyPressed(int key)
        {
            return keys[key];   
        }
    }
}