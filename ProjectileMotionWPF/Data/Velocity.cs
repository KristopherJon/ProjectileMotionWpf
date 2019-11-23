using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectileMotionWPF.Data
{
    public class Velocity
    {
        public double Vx { get; private set; }
        public double Vy { get; private set; }


        public Velocity(double Vx, double Vy)
        {
            this.Vx = Vx;
            this.Vy = Vy;
        }
    }
}
