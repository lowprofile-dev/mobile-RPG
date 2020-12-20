using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DunGen
{
    public sealed class DoorwayConnection
    {
        public Doorway A { get; private set; }
        public Doorway B { get; private set; }


        public DoorwayConnection(Doorway a, Doorway b)
        {
            A = a;
            B = b;
        }
    }
}