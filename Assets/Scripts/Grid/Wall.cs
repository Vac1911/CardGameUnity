using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    public struct Wall
    {
        bool frontRight;
        bool frontLeft;
        bool backRight;
        bool backLeft;

        public Wall(bool frontRight, bool frontLeft, bool backRight, bool backLeft)
        {
            this.frontRight = frontRight;
            this.frontLeft = frontLeft;
            this.backRight = backRight;
            this.backLeft = backLeft;
        }
    }
}
