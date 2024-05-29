using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    public struct Wall
    {
        public bool up;
        public bool down;
        public bool left;
        public bool right;

        public Wall(string tileName)
        {
            this.up = false;
            this.down = false;
            this.left = false;
            this.right = false;
            switch (tileName)
            {
                case "WallFL":
                    left = true;
                    break;
                case "WallBR":
                    right = true;
                    break;
                case "WallBL":
                    up = true;
                    break;
                case "WallFR":
                    down = true;
                    break;
                case "WallF":
                    down = true;
                    left = true;
                    break;
                case "WallB":
                    up = true;
                    right = true;
                    break;

                default:
                    break;
            }
        }

        public Wall(bool up, bool down, bool left, bool right)
        {
            this.up = up;
            this.down = down;
            this.left = left;
            this.right = right;
        }

        public Wall ShiftUp()
        {
            return new Wall(false, up, false, false);
        }

        public Wall ShiftDown()
        {
            return new Wall(down, false, false, false);
        }
    }
}
