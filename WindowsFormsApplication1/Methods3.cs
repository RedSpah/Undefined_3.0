using System.Collections.Generic;
using System.Linq;
using System.Xml;


namespace Undefined3
{
    public partial class Undefined3
    {
        bool ShuffleMusic_Func()
        {
            return LoadXMLAndModify("./music.xml", delegate (XmlDocument XML)
            {
                List<string> music = new List<string>();
                foreach (XmlNode n in XML.GetElementsByTagName("track"))
                {
                    music.Add(n.Attributes["path"].Value);
                    if (n.Attributes["intro"] != null) music.Add(n.Attributes["intro"].Value);
                    if (n.Attributes["layer"] != null) music.Add(n.Attributes["layer"].Value);
                    if (n.Attributes["layerintro"] != null) music.Add(n.Attributes["layerintro"].Value);
                }
                foreach (XmlNode n in XML.GetElementsByTagName("track"))
                {
                    if (RNG.NextDouble() < (RNGCutoff + RNGCutoff * CorruptionPower / 255)) { n.Attributes["path"].Value = music[RNG.Next(0, music.Count)]; }
                    if (n.Attributes["intro"] != null)
                    {
                        if (RNG.NextDouble() < (RNGCutoff + RNGCutoff * CorruptionPower / 255))
                        {
                            n.Attributes["intro"].Value = music[RNG.Next(0, music.Count)];
                        }
                    }
                    if (n.Attributes["layer"] != null)
                    {
                        if (RNG.NextDouble() < (RNGCutoff + RNGCutoff * CorruptionPower / 255))
                        {
                            n.Attributes["layer"].Value = music[RNG.Next(0, music.Count)];
                        }
                    }
                    if (n.Attributes["layerintro"] != null)
                    {
                        if (RNG.NextDouble() < (RNGCutoff + RNGCutoff * CorruptionPower / 255))
                        {
                            n.Attributes["layerintro"].Value = music[RNG.Next(0, music.Count)];
                        }
                    }
                }
            });
        }

        bool ShuffleSounds_Func()
        {

            return LoadXMLAndModify("./sounds.xml", delegate (XmlDocument XML)
            {
                List<string> sounds = (from XmlNode n in XML.GetElementsByTagName("sample") select n.Attributes["path"].Value).ToList();
                foreach (XmlNode n in XML.GetElementsByTagName("sample"))
                {
                    if (RNG.NextDouble() < (0.85d - 0.85d * CorruptionPower / 255))
                    {
                        n.Attributes["path"].Value = sounds[RNG.Next(0, sounds.Count)];
                    }
                }
            });
        }

        bool ShuffleItemGFX_Func()
        {

            return LoadXMLAndModify("./items.xml", delegate (XmlDocument XML)
            {
                List<string> itemspritenames = new List<string>();
                List<string> trinketspritenames = new List<string>();
                foreach (XmlNode x in XML.LastChild.ChildNodes)
                {
                    if (x.Name != "trinket")
                    {
                        if (x.Attributes["gfx"] != null) itemspritenames.Add(x.Attributes["gfx"].Value);
                    }
                    else
                    {
                        if (x.Attributes["gfx"] != null) trinketspritenames.Add(x.Attributes["gfx"].Value);
                    }

                }
                foreach (XmlNode x in XML.LastChild.ChildNodes)
                {
                    if (x.Name != "trinket")
                    {
                        if (x.Attributes["gfx"] != null)
                        {
                            if (RNG.NextDouble() < (RNGCutoff + RNGCutoff * CorruptionPower / 255))
                            {
                                x.Attributes["gfx"].Value = itemspritenames[RNG.Next(0, itemspritenames.Count)];
                            }
                        }
                    }
                    else
                    {
                        if (x.Attributes["gfx"] != null)
                        {
                            if (RNG.NextDouble() < (RNGCutoff + RNGCutoff * CorruptionPower / 255))
                            {
                                x.Attributes["gfx"].Value = trinketspritenames[RNG.Next(0, trinketspritenames.Count)];
                            }
                        }
                    }
                }
            }) &&

            LoadXMLAndModify("./pocketitems.xml", delegate (XmlDocument XML)
            {
                List<string> itemspritenames = new List<string>();
                foreach (XmlNode x in XML.LastChild.ChildNodes)
                {
                    if (x.Attributes["hud"] != null) itemspritenames.Add(x.Attributes["hud"].Value);
                }
                foreach (XmlNode x in XML.LastChild.ChildNodes)
                {
                    if (x.Attributes["hud"] != null)
                    {
                        if (RNG.NextDouble() < (RNGCutoff + RNGCutoff * CorruptionPower / 255))
                        {
                            x.Attributes["hud"].Value = itemspritenames[RNG.Next(0, itemspritenames.Count)];
                        }
                    }
                }
            });
        }

        bool ShuffleVideos_Func()
        {
            return LoadXMLAndModify("./videos.xml", delegate (XmlDocument XML)
            {
                List<string> animp = new List<string>();
                List<string> ogvp = new List<string>();
                foreach (XmlNode x in XML.GetElementsByTagName("anm2part"))
                {
                    animp.Add(x.Attributes["anm2"].Value);
                }
                foreach (XmlNode x in XML.GetElementsByTagName("anm2part"))
                {
                    if (RNG.NextDouble() < (RNGCutoff + RNGCutoff * CorruptionPower / 255))
                    {
                        x.Attributes["anm2"].Value = animp[RNG.Next(0, animp.Count)];
                    }
                }

                foreach (XmlNode x in XML.GetElementsByTagName("videopart"))
                {
                    animp.Add(x.Attributes["file"].Value);
                }
                foreach (XmlNode x in XML.GetElementsByTagName("videopart"))
                {
                    if (RNG.NextDouble() < (RNGCutoff + RNGCutoff * CorruptionPower / 255))
                    {
                        x.Attributes["file"].Value = animp[RNG.Next(0, animp.Count)];
                    }
                }
            });
        }
    }
}
