using CoreLibrary.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Data.Map
{
    public class MapData : IData<int>
    {
        public int uniqueId;
        public List<List<CustomVector2Int>> colliderPaths = new List<List<CustomVector2Int>>();

        public int GetKey()
        {
            return uniqueId;
        }
    }
}
