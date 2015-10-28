using System.Collections.Generic;
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
                    if (n.Attributes["layer"] != null) music.Add(n.Attributes["intro"].Value);
                    if (n.Attributes["layerintro"] != null) music.Add(n.Attributes["intro"].Value);
                }
                foreach (XmlNode n in XML.GetElementsByTagName("track"))
                {
                    n.Attributes["path"].Value = music[RNG.Next(0, music.Count)];
                    if (n.Attributes["intro"] != null) n.Attributes["intro"].Value = music[RNG.Next(0, music.Count)];
                    if (n.Attributes["layer"] != null) n.Attributes["layer"].Value = music[RNG.Next(0, music.Count)];
                    if (n.Attributes["layerintro"] != null)
                        n.Attributes["layerintro"].Value = music[RNG.Next(0, music.Count)];
                }
            });
        }

        bool ShuffleSounds_Func()
        {

            return LoadXMLAndModify("./sounds.xml", delegate (XmlDocument XML)
            {
                List<string> sounds = new List<string>();
                foreach (XmlNode n in XML.GetElementsByTagName("sample"))
                {
                    sounds.Add(n.Attributes["path"].Value);
                }
                foreach (XmlNode n in XML.GetElementsByTagName("sample"))
                {
                    n.Attributes["path"].Value = sounds[RNG.Next(0, sounds.Count)];
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
                            x.Attributes["gfx"].Value = itemspritenames[RNG.Next(0, itemspritenames.Count)];
                    }
                    else
                    {
                        if (x.Attributes["gfx"] != null)
                            x.Attributes["gfx"].Value = trinketspritenames[RNG.Next(0, trinketspritenames.Count)];
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
                    if (x.Attributes["hud"] != null) x.Attributes["hud"].Value = itemspritenames[RNG.Next(0, itemspritenames.Count)];
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
                    x.Attributes["anm2"].Value = animp[RNG.Next(0, animp.Count)];
                }

                foreach (XmlNode x in XML.GetElementsByTagName("videopart"))
                {
                    animp.Add(x.Attributes["file"].Value);
                }
                foreach (XmlNode x in XML.GetElementsByTagName("videopart"))
                {
                    x.Attributes["file"].Value = animp[RNG.Next(0, animp.Count)];
                }
            });
        }
    }
}
