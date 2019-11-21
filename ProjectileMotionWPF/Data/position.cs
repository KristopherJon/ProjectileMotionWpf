using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectileMotionWPF.Data
{
    public class Position
    {
        private readonly float x;
        private readonly float y;
        public float X => x;
        public float Y => y;

        public Position(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
