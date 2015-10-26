using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Xml;
using System.IO;
using System.Security.Cryptography;
using System.Globalization;

namespace Undefined
{
    public partial class Undefined3
    {
        void SS()
        {
            List<string> sounds = new List<string>();
            TextReader TX = File.OpenText("./sounds.xml");
            XmlDocument XML = new XmlDocument();
            XML.LoadXml(TX.ReadToEnd());
            TX.Close();
            foreach (XmlNode n in XML.GetElementsByTagName("sample"))
            {
                sounds.Add(n.Attributes["path"].Value);
            }
            foreach (XmlNode n in XML.GetElementsByTagName("sample"))
            {
                n.Attributes["path"].Value = sounds[RNG.Next(0,sounds.Count)];
            }
            XmlWriter XMWR = XmlWriter.Create("./__tmp.xml");
            XML.WriteTo(XMWR);
            XMWR.Close();
            File.Delete("./sounds.xml");
            File.Move("./__tmp.xml", "./sounds.xml");

            List<string> music = new List<string>();
            TX = File.OpenText("./music.xml");
            XML = new XmlDocument();
            XML.LoadXml(TX.ReadToEnd());
            TX.Close();
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
                if (n.Attributes["layerintro"] != null) n.Attributes["layerintro"].Value = music[RNG.Next(0, music.Count)];
            }
            XMWR = XmlWriter.Create("./__tmp.xml");
            XML.WriteTo(XMWR);
            XMWR.Close();
            File.Delete("./music.xml");
            File.Move("./__tmp.xml", "./music.xml");
        }

        void SIGFX()
        {
            
            List<string> spritenames = new List<string>();
            TextReader TX = File.OpenText("./items.xml");
            XmlDocument XML = new XmlDocument();
            XML.LoadXml(TX.ReadToEnd());
            TX.Close();
            foreach (XmlNode x in XML.LastChild.ChildNodes)
            {
                if (x.Attributes["gfx"] != null) spritenames.Add(x.Attributes["gfx"].Value);
            }
            foreach (XmlNode x in XML.LastChild.ChildNodes)
            {
                if (x.Attributes["gfx"] != null) x.Attributes["gfx"].Value = spritenames[RNG.Next(0,spritenames.Count)];
            }
            XmlWriter XMWR = XmlWriter.Create("./__tmp.xml");
            XML.WriteTo(XMWR);
            XMWR.Close();
            File.Delete("./items.xml");
            File.Move("./__tmp.xml", "./items.xml");

            spritenames = new List<string>();
            TX = File.OpenText("./pocketitems.xml");
            XML = new XmlDocument();
            XML.LoadXml(TX.ReadToEnd());
            TX.Close();
            foreach (XmlNode x in XML.LastChild.ChildNodes)
            {
                if (x.Attributes["hud"] != null) spritenames.Add(x.Attributes["hud"].Value);
            }
            foreach (XmlNode x in XML.LastChild.ChildNodes)
            {
                if (x.Attributes["hud"] != null) x.Attributes["hud"].Value = spritenames[RNG.Next(0, spritenames.Count)];
            }
            XMWR = XmlWriter.Create("./__tmp.xml");
            XML.WriteTo(XMWR);
            XMWR.Close();
            File.Delete("./pocketitems.xml");
            File.Move("./__tmp.xml", "./pocketitems.xml");

            spritenames = new List<string>();
            TX = File.OpenText("./costumes2.xml");
            XML = new XmlDocument();
            XML.LoadXml(TX.ReadToEnd());
            TX.Close();
            foreach (XmlNode x in XML.LastChild.ChildNodes)
            {
                
                if (x.NodeType != XmlNodeType.Comment && x.Attributes["amn2path"] != null) spritenames.Add(x.Attributes["amn2path"].Value);
            }
            foreach (XmlNode x in XML.LastChild.ChildNodes)
            {
                if (x.NodeType != XmlNodeType.Comment && x.Attributes["amn2path"] != null) x.Attributes["amn2path"].Value = spritenames[RNG.Next(0, spritenames.Count)];
            }
            XMWR = XmlWriter.Create("./__tmp.xml");
            XML.WriteTo(XMWR);
            XMWR.Close();
            File.Delete("./costumes2.xml");
            File.Move("./__tmp.xml", "./costumes2.xml");

        }

        void SV()
        {
            List<string> animp = new List<string>();
            List<string> ogvp = new List<string>();

            List<string> spritenames = new List<string>();
            TextReader TX = File.OpenText("./cutscenes.xml");
            XmlDocument XML = new XmlDocument();
            XML.LoadXml(TX.ReadToEnd());
            TX.Close();

            foreach (XmlNode x in XML.GetElementsByTagName("anm2part"))
            {
                animp.Add(x.Attributes["anm2"].Value);
            }
            foreach (XmlNode x in XML.GetElementsByTagName("anm2part"))
            {
                x.Attributes["anm2"].Value = animp[RNG.Next(0,animp.Count)];
            }

            foreach (XmlNode x in XML.GetElementsByTagName("videopart"))
            {
                animp.Add(x.Attributes["file"].Value);
            }
            foreach (XmlNode x in XML.GetElementsByTagName("videopart"))
            {
                x.Attributes["file"].Value = animp[RNG.Next(0, animp.Count)];
            }

            XmlWriter XMWR = XmlWriter.Create("./__tmp.xml");
            XML.WriteTo(XMWR);
            XMWR.Close();
            File.Delete("./cutscenes.xml");
            File.Move("./__tmp.xml", "./cutscenes.xml");
        }
    }
}
