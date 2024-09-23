using CoreLibrary.Utility;
using Server.Data.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Content.Info
{
    public class MapInfo
    {
        public MapData mapData { get; private set; }

        public MapInfo(MapData mapData)
        {
            this.mapData = mapData;
        }
    }
}
