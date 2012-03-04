using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntityFramework;

namespace SingleSpire.EntityFramework.Args
{
    class MoveArgs : ActionArgs
    {
        public enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }

        public Direction MovingDirection { get; set; }

        public MoveArgs(Direction direction)
        {
            MovingDirection = direction;
        }
    }
}
