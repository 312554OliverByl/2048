﻿using System.Windows.Input;

namespace _2048
{
    /// <summary>
    /// Wrapper class to handle keyboard input.
    /// It ensures that each key press is only registered for one tick.
    /// 
    /// Written by Oliver.
    /// </summary>
    public static class Input
    {
        //Key ids.
        //Use integers instead of Key objects for simplicity
        //and to save memory.
        public static readonly int UP = 0;
        public static readonly int DOWN = 1;
        public static readonly int LEFT = 2;
        public static readonly int RIGHT = 3;

        //Store what keys have been pressed this tick.
        private static bool[] keys = new bool[4];
        //Store what keys are currently being pressed.
        private static bool[] keysHeld = new bool[4];

        public static void tick()
        {
            //Unregister any keys that were pressed last tick.
            for (int i = 0; i < keys.Length; i++)
                keys[i] = false;

            //Handle up key press:
            if (Keyboard.IsKeyDown(Key.Up))
            {
                //Only accept key press if keysHeld for that key
                //is false (i.e. it wasn't pressed last tick)
                if (!keysHeld[UP])
                    keys[UP] = true;

                //Update keys held.
                keysHeld[UP] = true;
                return;
            }
            else
            {
                keysHeld[UP] = false;
            }

            //Repeat above logic for three other keys:

            if (Keyboard.IsKeyDown(Key.Down))
            {
                if (!keysHeld[DOWN])
                    keys[DOWN] = true;
                keysHeld[DOWN] = true;
                return;
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
                return;
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
                return;
            }
            else
            {
                keysHeld[RIGHT] = false;
            }
        }

        /// <summary>
        /// Returns if a given key was pressed during this tick.
        /// </summary>
        /// <param name="key">Id of key to check.</param>
        /// <returns>If it was pressed this tick. If key was pressed last tick but
        /// is still held down, will return false. </returns>
        public static bool wasKeyPressed(int key)
        {
            return keys[key];
        }
    }
}