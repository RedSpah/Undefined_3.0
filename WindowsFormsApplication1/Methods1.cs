using System.Collections.Generic;
using System.Linq;
using System.Xml;

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
                    if (RNG.NextDouble() < (RNGCutoff + RNGCutoff * CorruptionPower / 255))
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
                        else if ((n.Attributes["name"].Value != "Player" || ShuffleIsaacSprite) && (!AntiCrashBlockedEntities.Contains(n.Attributes["name"].Value) || AntiCrash))
                        {
                            n.Attributes["anm2path"].Value = EntityAnmList[RNG.Next(0, EntityAnmList.Count)];
                        }
                    }
                }

            });
        }



        bool GarbleText_Func()
        {
            bool succ =
                LoadXMLAndModify("./entities2.xml", delegate (XmlDocument XML)
                {
                    foreach (XmlNode n in XML.LastChild.ChildNodes)
                    {
                        n.Attributes["name"].Value = Garble(n.Attributes["name"].Value);

                    }
                }) &&

                LoadXMLAndModify("./stages.xml", delegate (XmlDocument XML)
                {
                    foreach (XmlNode n in XML.LastChild.ChildNodes)
                    {
                        n.Attributes["name"].Value = Garble(n.Attributes["name"].Value);
                    }
                }) &&

                LoadXMLAndModify("./items.xml", delegate (XmlDocument XML)
                {
                    foreach (XmlNode n in XML.LastChild.ChildNodes)
                    {
                        n.Attributes["name"].Value = Garble(n.Attributes["name"].Value);
                        n.Attributes["description"].Value = Garble(n.Attributes["description"].Value);
                    }
                }) &&

                LoadXMLAndModify("./players.xml", delegate (XmlDocument XML)
                {
                    foreach (XmlNode n in XML.LastChild.ChildNodes)
                    {
                        n.Attributes["name"].Value = Garble(n.Attributes["name"].Value);
                    }
                }) &&

                LoadXMLAndModify("./pocketitems.xml", delegate (XmlDocument XML)
                {
                    foreach (XmlNode n in XML.LastChild.ChildNodes)
                    {
                        n.Attributes["name"].Value = Garble(n.Attributes["name"].Value);
                        if (n.Attributes["description"] != null)
                        {
                            n.Attributes["description"].Value = Garble(n.Attributes["description"].Value);
                        }
                    }
                }) &&

                LoadTXTAndModify("./fortunes.txt", delegate (string text)
                {
                    return text.Split('\n').Select(x => Garble(x)).Aggregate("", (current, x) => current + (x + "\n"));
                }) &&

                LoadTXTAndModify("./rules.txt", delegate (string text)
                {
                    return text.Split('\n').Select(x => Garble(x)).Aggregate("", (current, x) => current + (x + "\n"));
                });
            return succ;
        }

    }
}
