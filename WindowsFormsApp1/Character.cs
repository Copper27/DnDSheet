using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDSheet
{
    [Serializable]
   public class Character
    {
        public String playerName { get; set; }
        public String charName;
        public String race;
        public String size;
    }
}
