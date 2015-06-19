using Righthand.RealtimeGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace S7_DMCToolbox
{
    public class RealtimeGraphItem : IGraphItem
    {
        public int Time { get; set; }
        public double Value { get; set; }
    }

}
