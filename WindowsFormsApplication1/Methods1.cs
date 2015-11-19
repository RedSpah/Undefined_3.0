using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System;
using System.IO;
using System.Windows.Forms;

namespace Undefined3
{
    public partial class Undefined3
    {
        bool ShuffleEntityGFX_Func()
        {
            List<string> AntiCrashBlockedEntities = new List<string>() { "Collectible", "Fire Place (attacking)" };
            return LoadXMLAndModify("./entities2.xml", delegate (XmlDocument XML)
            {

                List<string> EntityAnmList = new List<string>();
                List<string> BossAnmList = new List<string>();
                List<string> ParticleAnmList = new List<string>();

                foreach (XmlNode n in XML.LastChild.ChildNodes)
                {
                    if (n.Attributes["boss"] != null && n.Attributes["boss"].Value == "1" && !ShuffleBossGFX)
                    {
                        BossAnmList.Add(n.Attributes["anm2path"].Value);
                    }
                    else if ((n.Attributes["id"].Value == "1000" || n.Attributes["id"].Value == "2") && !ShuffleParticleGFX)
                    {
                        ParticleAnmList.Add(n.Attributes["anm2path"].Value);
                    }
                    else
                    {
                        EntityAnmList.Add(n.Attributes["anm2path"].Value);
                    }
                }

                foreach (XmlNode n in XML.LastChild.ChildNodes)
                {
                    if (CorruptRNG())
                    {
                        if (n.Attributes["boss"] != null && n.Attributes["boss"].Value == "1" && !ShuffleBossGFX)
                        {
                            n.Attributes["anm2path"].Value = BossAnmList[RNG.Next(0, BossAnmList.Count)];
                        }
                        else if ((n.Attributes["id"].Value == "1000" || n.Attributes["id"].Value == "2") && !ShuffleParticleGFX)
                        {
                            if (n.Attributes["id"].Value == "1000")
                            {
                                n.Attributes["anm2path"].Value = ParticleAnmList[RNG.Next(0, ParticleAnmList.Count)];
                            }
                        }
                        else if ((n.Attributes["id"].Value != "1" || ShuffleIsaacSprite) && (!AntiCrashBlockedEntities.Contains(n.Attributes["name"].Value) || AntiCrash))
                        {
                            n.Attributes["anm2path"].Value = EntityAnmList[RNG.Next(0, EntityAnmList.Count)];
                        }
                    }
                }
            });
        }

        bool GarbleText_Func()
        {
            if (IsAfterbirth)
            {
                var afort = Safe.OpenStreamWriter("./fortunes.txt");
                if (afort == null) { return false; }
                afort.Close();
                if (!LoadTXTAndModify("./fortunes.txt", delegate (string s) { return Garble(AfterbirthFortunesBase); })) { return false; }

                afort = Safe.OpenStreamWriter("./rules.txt");
                if (afort == null) { return false; }
                afort.Close();
                if (!LoadTXTAndModify("./rules.txt", delegate (string s) { return Garble(AfterbirthRulesBase); })) { return false; }

                if (!LoadXMLAndModify("minibosses.xml", delegate (XmlDocument XML)
                {
                    foreach (XmlNode x in XML.GetElementsByTagName("miniboss"))
                    {
                        x.Attributes["name"].Value = Garble(x.Attributes["name"].Value);
                    }
                }))
                { return false; }

                if (!LoadXMLAndModify("curses.xml", delegate (XmlDocument XML)
                {
                    foreach (XmlNode x in XML.GetElementsByTagName("curse"))
                    {
                        x.Attributes["name"].Value = Garble(x.Attributes["name"].Value);
                    }
                }))
                { return false; }

                if (!LoadTXTAndModify("seedmenu.xml", text => "<a>" + text + "</a>")) { return false; }

                if (!LoadXMLAndModify("seedmenu.xml", delegate (XmlDocument XML)
                {
                    foreach (XmlNode x in XML.GetElementsByTagName("seed"))
                    {
                        x.Attributes["name"].Value = Garble(x.Attributes["name"].Value);
                    }

                    foreach (XmlNode x in XML.GetElementsByTagName("confirm"))
                    {
                        x.Attributes["name"].Value = Garble(x.Attributes["name"].Value);
                    }

                    foreach (XmlNode x in XML.GetElementsByTagName("reset"))
                    {
                        x.Attributes["name"].Value = Garble(x.Attributes["name"].Value);
                    }

                    foreach (XmlNode x in XML.GetElementsByTagName("entryDialog"))
                    {
                        x.Attributes["unlock"].Value = Garble(x.Attributes["unlock"].Value);
                        x.Attributes["enable"].Value = Garble(x.Attributes["enable"].Value);
                    }
                }))
                { return false; }

                if (!LoadTXTAndModify("seedmenu.xml", text => text.Replace("<a>", "").Replace("</a>", ""))) { return false; }
            }

            if (!IsAfterbirth)
            {
                if (!LoadTXTAndModify("./fortunes.txt", delegate (string text)
                {
                    return text.Split('\n').Select(x => Garble(x)).Aggregate("", (current, x) => current + (x + "\n"));
                }))
                { return false; }

                if (!LoadTXTAndModify("./rules.txt", delegate (string text)
                {
                    return text.Split('\n').Select(x => Garble(x)).Aggregate("", (current, x) => current + (x + "\n"));
                }))
                { return false; }
            }

            if (!LoadXMLAndModify("./entities2.xml", delegate (XmlDocument XML)
            {
                foreach (XmlNode n in XML.LastChild.ChildNodes)
                {
                    n.Attributes["name"].Value = Garble(n.Attributes["name"].Value);

                }
            }))
            { return false; }

            if (!LoadXMLAndModify("./stages.xml", delegate (XmlDocument XML)
            {
                foreach (XmlNode n in XML.LastChild.ChildNodes)
                {
                    n.Attributes["name"].Value = Garble(n.Attributes["name"].Value);
                }
            }))
            { return false; }

            if (!LoadXMLAndModify("./items.xml", delegate (XmlDocument XML)
            {
                foreach (XmlNode n in XML.LastChild.ChildNodes)
                {
                    n.Attributes["name"].Value = Garble(n.Attributes["name"].Value);
                    n.Attributes["description"].Value = Garble(n.Attributes["description"].Value);
                }
            }))
            { return false; }

            if (!LoadXMLAndModify("./players.xml", delegate (XmlDocument XML)
            {
                foreach (XmlNode n in XML.LastChild.ChildNodes)
                {
                    n.Attributes["name"].Value = Garble(n.Attributes["name"].Value);
                }
            }))
            { return false; }

            return LoadXMLAndModify("./pocketitems.xml", delegate (XmlDocument XML)
            {
                foreach (XmlNode n in XML.LastChild.ChildNodes)
                {
                    n.Attributes["name"].Value = Garble(n.Attributes["name"].Value);
                    if (n.Attributes["description"] != null)
                    {
                        n.Attributes["description"].Value = Garble(n.Attributes["description"].Value);
                    }
                }
            });
        }
    }
}
