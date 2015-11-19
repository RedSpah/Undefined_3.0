using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Undefined3
{
    public partial class Undefined3
    {
        bool CorruptRooms_Func()
        {
            return true;
        }

        private bool CorruptUI_Func()
        {
            List<string> uifiles = Safe.GetFiles("./gfx/ui/");
            if (uifiles == null)
            {
                return false;
            }

            return !uifiles.Any(file => !LoadXMLAndModify(file, delegate (XmlDocument XML)
            {
                AnimCont A = new AnimCont();
                foreach (XmlNode frame in XML.GetElementsByTagName("Frame"))
                {
                    CorruptFrame(frame, A);
                }
            }));
        }
    }
}
