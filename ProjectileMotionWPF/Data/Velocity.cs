using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectileMotionWPF.Data
{
    public class Velocity
    {
        private readonly float Vx;
        private readonly float Vy;
        public float VX => Vx;
        public float VY => Vy;

    public Velocity(float Vx, float Vy)
        {
            this.Vx = Vx;
            this.Vy = Vy;
        }
    }
}
