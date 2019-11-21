using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectileMotionWPF.Data
{
    public class SpaceTimePoint
    {
        public Position Position { get; private set; }
        public Velocity Velocity { get; private set; }

        public SpaceTimePoint(Position position, Velocity velocity)
        {
            this.Position = position;
            this.Velocity = velocity;
        }
    }
}
