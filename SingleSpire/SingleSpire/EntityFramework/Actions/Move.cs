using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntityFramework;
using SingleSpire.EntityFramework.Args;
using EntityFramework.Components;
using Microsoft.Xna.Framework;

namespace SingleSpire.EntityFramework.Actions
{
    class Move : EntityAction
    {
        public Move()
        {
            this.Name = "Move";
        }

        public override void Do(ActionArgs args)
        {
            if (this.Entity != null && args != null && args is MoveArgs)
            {
                Movement movement = this.Entity.GetComponent("Movement") as Movement;
                movement.PreviousPosition = movement.CurrentPosition;
                movement.CurrentSmoothing = 0;
                switch (((MoveArgs)args).MovingDirection)
                {
                    case MoveArgs.Direction.Up:
                        movement.CurrentPosition += new Vector2(0, -movement.Velocity);
                        break;
                    case MoveArgs.Direction.Down:
                        movement.CurrentPosition += new Vector2(0, movement.Velocity);
                        break;
                    case MoveArgs.Direction.Left:
                        movement.CurrentPosition += new Vector2(-movement.Velocity, 0);
                        break;
                    case MoveArgs.Direction.Right:
                        movement.CurrentPosition += new Vector2(movement.Velocity, 0);
                        break;
                }
            }
        }
    }
}
