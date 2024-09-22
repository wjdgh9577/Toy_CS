using CoreLibrary.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Content.Info
{
    public class MapInfo
    {
        public int uniqueId;
        public List<List<CustomVector2Int>> colliderPaths = new List<List<CustomVector2Int>>();

        public MapInfo(int uniqueId)
        {
            this.uniqueId = uniqueId;
        }
    }
}
