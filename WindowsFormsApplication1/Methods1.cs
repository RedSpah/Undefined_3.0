using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Xml;
using System.IO;

namespace Undefined
{
    public partial class Undefined3 
    {
        // universal string garbling function
        string Garble(string input)
        {
            int corcutoff = 255 - CorruptionPower;
            int dupecutoff = 255 - (CorruptionPower / 6);
            int mutilatepow = CorruptionPower / 16;
            List<char> let = input.ToCharArray().ToList();
            for (int i = 0; i < let.Count; i++)
            {
                int r = RNG.Next(0, 255);
                if (r > dupecutoff)
                {
                    let.Add(let[i]);
                }
                else if (r > corcutoff)
                {
                    int q = (int)let[i] + RNG.Next(-mutilatepow, mutilatepow);
                    while (q < 32)
                    {
                        q += RNG.Next(0, mutilatepow+2);
                    }
                    while (q > 128)
                    {
                        q += RNG.Next(-mutilatepow-1, 0);
                    }
                    let[i] = (char)q;
                }

            }
            return new string(let.ToArray());
        }

        static List<string> AntiCrashBlockedEntities = new List<string>() {"Collectible", "Fire Place (attacking)"}; 

        void SE_GFX()
        {

            TextReader TX = File.OpenText("./entities2.xml");

            XmlDocument XML = new XmlDocument();
                XML.LoadXml(TX.ReadToEnd());
                XmlWriter XMWR;
                XMWR = XmlWriter.Create("./__tmp.xml");

                TX.Close();

                XmlNode entities = XML.FirstChild;

                List<string> EntityAnmList = new List<string>();
            List<string> BossAnmList = new List<string>();
            List<string> ParticleAnmList = new List<string>();

            foreach (XmlNode n in entities.ChildNodes)
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

            foreach (XmlNode n in entities.ChildNodes)
            {
                if (RNG.NextDouble() < 0.1 + (0.9*CorruptionPower/255))
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
                    else if ((n.Attributes["name"].Value != "Player"  || ShuffleIsaacSprite) && (!AntiCrashBlockedEntities.Contains(n.Attributes["name"].Value) || AntiCrash))
                    {
                        n.Attributes["anm2path"].Value = EntityAnmList[RNG.Next(0, EntityAnmList.Count)];
                    }                   
                }
            }

            XML.WriteTo(XMWR);
            XMWR.Close();

            File.Delete("./entities2.xml");
            File.Move("./__tmp.xml", "./entities2.xml");
        }

        

        void GText()
        {
            TextReader TX = File.OpenText("./entities2.xml");
            XmlDocument XML = new XmlDocument();
            XML.LoadXml(TX.ReadToEnd());
            XmlWriter XMWR = XmlWriter.Create("./__tmp.xml");
            TX.Close();

            XmlNode entities = XML.LastChild;

            foreach (XmlNode n in entities.ChildNodes)
            {
                n.Attributes["name"].Value = Garble(n.Attributes["name"].Value);

            }

            XML.WriteTo(XMWR);
            XMWR.Close();

            File.Delete("./entities2.xml");
            File.Move("./__tmp.xml", "./entities2.xml");

            TX = File.OpenText("./stages.xml");
            XML = new XmlDocument();
            XML.LoadXml(TX.ReadToEnd());
            XMWR = XmlWriter.Create("./__tmp.xml");
            TX.Close();

            XmlNode stages = XML.LastChild;


            foreach (XmlNode n in stages.ChildNodes)
            {
                n.Attributes["name"].Value = Garble(n.Attributes["name"].Value);
            }

            XML.WriteTo(XMWR);
            XMWR.Close();

            File.Delete("./stages.xml");
            File.Move("./__tmp.xml", "./stages.xml");

            TX = File.OpenText("./items.xml");
            XML = new XmlDocument();
            XML.LoadXml(TX.ReadToEnd());
            XMWR = XmlWriter.Create("./__tmp.xml");
            TX.Close();

            XmlNode items = XML.LastChild;


            foreach (XmlNode n in items.ChildNodes)
            {
                n.Attributes["name"].Value = Garble(n.Attributes["name"].Value);
                n.Attributes["description"].Value = Garble(n.Attributes["description"].Value);
            }

            XML.WriteTo(XMWR);
            XMWR.Close();

            File.Delete("./items.xml");
            File.Move("./__tmp.xml", "./items.xml");

            TX = File.OpenText("./players.xml");
            XML = new XmlDocument();
            XML.LoadXml(TX.ReadToEnd());
            XMWR = XmlWriter.Create("./__tmp.xml");
            TX.Close();

            XmlNode players = XML.LastChild;


            foreach (XmlNode n in players.ChildNodes)
            {
                n.Attributes["name"].Value = Garble(n.Attributes["name"].Value);
            }

            XML.WriteTo(XMWR);
            XMWR.Close();

            File.Delete("./players.xml");
            File.Move("./__tmp.xml", "./players.xml");

            TX = File.OpenText("./pocketitems.xml");
            XML = new XmlDocument();
            XML.LoadXml(TX.ReadToEnd());
            XMWR = XmlWriter.Create("./__tmp.xml");
            TX.Close();

            XmlNode pitems = XML.LastChild;


            foreach (XmlNode n in pitems.ChildNodes)
            {
                n.Attributes["name"].Value = Garble(n.Attributes["name"].Value);
                if (n.Attributes["description"] != null)
                {
                    n.Attributes["description"].Value = Garble(n.Attributes["description"].Value);
                }
            }

            XML.WriteTo(XMWR);
            XMWR.Close();

            File.Delete("./pocketitems.xml");
            File.Move("./__tmp.xml", "./pocketitems.xml");

            TX = File.OpenText("./fortunes.txt");
            StreamWriter Str = new StreamWriter("./__tmp.txt");
            string[] fortunes = TX.ReadToEnd().Split('\n').Select(x => Garble(x)).ToArray();
            foreach (var f in fortunes)
            {
                Str.Write(f);
                Str.Write("\n");
            }
            TX.Close();
            File.Delete("./fortunes.txt");
            Str.Close();
            File.Move("./__tmp.txt", "./fortunes.txt");

            TX = File.OpenText("./rules.txt");
            Str = new StreamWriter("./__tmp.txt");
            string[] rules = TX.ReadToEnd().Split('\n').Select(x => Garble(x)).ToArray();
            foreach (var f in rules)
            {
                Str.Write(f);
                Str.Write("\n");
            }
            TX.Close();
            File.Delete("./rules.txt");
            Str.Close();
            File.Move("./__tmp.txt", "./rules.txt");
        }
            
    }
}
