using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace EntityFramework.Components
{
    public class Movement : Component
    {
        public float Velocity { get; set; }
        public float Acceleration { get; set; }
        public Vector2 CurrentPosition { get; set; }
        public Vector2 PreviousPosition { get; set; }
        public float CurrentSmoothing { get; set; }
        public float LerpIntervals = 12;

        public Movement(float velocity, float accel, Vector2 position)
        {
            this.Name = "Movement";
            Velocity = velocity;
            Acceleration = accel;
            CurrentPosition = position;
            PreviousPosition = position;
            CurrentSmoothing = 0;
        }

        public Vector2 GetLerpedPlayerPosition()
        {
            CurrentSmoothing += 1.0f / LerpIntervals;
            if (CurrentSmoothing > 1)
                CurrentSmoothing = 1;

            return Vector2.Lerp(PreviousPosition, CurrentPosition, CurrentSmoothing);
        }
    }
}
