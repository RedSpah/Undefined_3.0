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
    public class AnimCont
    {
        public AnimCont()
        {
            context[0] = (int)((Undefined3.RNG.NextDouble() - 0.5d) * 2 * Undefined3.CorruptionPower); // xpos
            context[1] = (int)((Undefined3.RNG.NextDouble() - 0.5d) * 2 * Undefined3.CorruptionPower); // ypos
            context[2] = (int)((Undefined3.RNG.NextDouble() - 0.5d) * 0.4d * Undefined3.CorruptionPower); // frame
            context[3] = (int)((Undefined3.RNG.NextDouble() - 0.5d) * 4 * Undefined3.CorruptionPower); // xscale
            context[4] = (int)((Undefined3.RNG.NextDouble() - 0.5d) * 4 * Undefined3.CorruptionPower); // yscale
            context[5] = (int)((Undefined3.RNG.NextDouble() - 0.5d) * 3 * Undefined3.CorruptionPower); //r
            context[6] = (int)((Undefined3.RNG.NextDouble() - 0.5d) * 3 * Undefined3.CorruptionPower); //g
            context[7] = (int)((Undefined3.RNG.NextDouble() - 0.5d) * 3 * Undefined3.CorruptionPower); //b
            context[8] = (int)((Undefined3.RNG.NextDouble() - 0.5d) * 3 * Undefined3.CorruptionPower); //ro
            context[9] = (int)((Undefined3.RNG.NextDouble() - 0.5d) * 3 * Undefined3.CorruptionPower); //go
            context[10] = (int)((Undefined3.RNG.NextDouble() - 0.5d) * 3 * Undefined3.CorruptionPower); //bo
            context[11] = (int)((Undefined3.RNG.NextDouble() - 0.5d) * 1 * Undefined3.CorruptionPower); //a
            context[12] = (int)((Undefined3.RNG.NextDouble() - 0.5d) * 3 * Undefined3.CorruptionPower); //rot
        }

        public void Mod()
        {
            for(int i = 0; i < 13; i++)
            {
                context[i] = (int)(context[i]*(1 + (Undefined3.RNG.NextDouble() - 0.5) * (float)Undefined3.CorruptionPower/128));
            }
        }

        public int[] context = new int[13];
    }

    public partial class Undefined3
    {
        private void CorruptFrame(XmlNode frame, AnimCont context)
        {
            var culture = CultureInfo.InvariantCulture;
            if (frame.Attributes["XPosition"] != null)
                frame.Attributes["XPosition"].Value =
                    (double.Parse(frame.Attributes["XPosition"].Value, culture) + context.context[0]).ToString();
            if (frame.Attributes["YPosition"] != null)
                frame.Attributes["YPosition"].Value =
                    (double.Parse(frame.Attributes["YPosition"].Value, culture) + context.context[1]).ToString();
            if (frame.Attributes["Delay"] != null)
                frame.Attributes["Delay"].Value =
                    (Math.Abs(double.Parse(frame.Attributes["Delay"].Value, culture) + context.context[2])).ToString();
            if (frame.Attributes["XScale"] != null)
                frame.Attributes["XScale"].Value =
                    (double.Parse(frame.Attributes["XScale"].Value, culture) + context.context[3]).ToString();
            if (frame.Attributes["YScale"] != null)
                frame.Attributes["YScale"].Value =
                    (double.Parse(frame.Attributes["YScale"].Value, culture) + context.context[4]).ToString();
            if (frame.Attributes["RedTint"] != null)
                frame.Attributes["RedTint"].Value =
                    (double.Parse(frame.Attributes["RedTint"].Value, culture) + context.context[5]).ToString();
            if (frame.Attributes["GreenTint"] != null)
                frame.Attributes["GreenTint"].Value =
                    (double.Parse(frame.Attributes["GreenTint"].Value, culture) + context.context[6]).ToString();
            if (frame.Attributes["BlueTint"] != null)
                frame.Attributes["BlueTint"].Value =
                    (double.Parse(frame.Attributes["BlueTint"].Value, culture) + context.context[7]).ToString();
            if (frame.Attributes["RedOffset"] != null)
                frame.Attributes["RedOffset"].Value =
                    (double.Parse(frame.Attributes["RedOffset"].Value, culture) + context.context[8]).ToString();
            if (frame.Attributes["GreenOffset"] != null)
                frame.Attributes["GreenOffset"].Value =
                    (double.Parse(frame.Attributes["GreenOffset"].Value, culture) + context.context[9]).ToString();
            if (frame.Attributes["BlueOffset"] != null)
                frame.Attributes["BlueOffset"].Value =
                    (double.Parse(frame.Attributes["BlueOffset"].Value, culture) + context.context[10]).ToString();
            if (frame.Attributes["AlphaTint"] != null)
                frame.Attributes["AlphaTint"].Value =
                    (double.Parse(frame.Attributes["AlphaTint"].Value, culture) + context.context[11] + 92).ToString();
            if (frame.Attributes["Rotation"] != null)
                frame.Attributes["Rotation"].Value =
                    (double.Parse(frame.Attributes["Rotation"].Value, culture) + context.context[12]).ToString();
            context.Mod();
        }

        private void CEANIM()
        {
            List<string> tocorrupt = new List<string>();

            TextReader TX = File.OpenText("./entities2.xml");
            XmlDocument XML = new XmlDocument();
            XML.LoadXml(TX.ReadToEnd());

            TX.Close();

            XmlNode entities = XML.LastChild;

            foreach (XmlNode n in entities.ChildNodes)
            {
                tocorrupt.Add(n.Attributes["anm2path"].Value);

                n.Attributes["anm2path"].Value =
                    n.Attributes["anm2path"].Value.Substring(0, n.Attributes["anm2path"].Value.Length - 1) + "c";
            }

            XmlWriter Xl = XmlWriter.Create("./__tmp.xml");
            XML.WriteTo(Xl);
            File.Delete("./entities2.xml");
            Xl.Close();
            File.Move("./__tmp.xml", "./entities2.xml");

            foreach (string corruptfile in tocorrupt)
            {
                try
                {
                    TX = File.OpenText("./gfx/" + corruptfile);
                }
                catch (Exception)
                {

                    TX = File.OpenText("./gfx/" + corruptfile.Substring(0, corruptfile.Length - 1) + "c");
                }

                XmlWriter XMWR = XmlWriter.Create("./gfx/__tmp.xml");
                XML = new XmlDocument();

                XML.LoadXml(TX.ReadToEnd());
                TX.Close();

                AnimCont A = new AnimCont();
                foreach (XmlNode frame in XML.GetElementsByTagName("Frame"))
                {
                    CorruptFrame(frame, A);
                }
                XML.WriteTo(XMWR);
                XMWR.Close();

                if (File.Exists("./gfx/" + corruptfile.Substring(0, corruptfile.Length - 1) + "c"))
                {
                    File.Delete("./gfx/" + corruptfile.Substring(0, corruptfile.Length - 1) + "c");
                }
                File.Move("./gfx/__tmp.xml", "./gfx/" + corruptfile.Substring(0, corruptfile.Length - 1) + "c");
            }
        }

        private void CES()
        {
            TextReader TX = File.OpenText("./entities2.xml");
            XmlDocument XML = new XmlDocument();
            XML.LoadXml(TX.ReadToEnd());
            TX.Close();
            XmlNode entities = XML.LastChild;
            foreach (XmlNode n in entities.ChildNodes)
            {
                var culture = CultureInfo.InvariantCulture;
                if (n.Attributes["baseHP"] != null)
                {
                    n.Attributes["baseHP"].Value =
                        (float.Parse(n.Attributes["baseHP"].Value, culture)*
                         (0.5f + ((float) CorruptionPower/32)*RNG.NextDouble())).ToString();
                }
                if (n.Attributes["collisionDamage"] != null)
                {
                    n.Attributes["collisionDamage"].Value =
                        (float.Parse(n.Attributes["collisionDamage"].Value, culture)*
                         (0.5f + ((float) CorruptionPower/32)*RNG.NextDouble())).ToString();
                }
                if (n.Attributes["collisionMass"] != null)
                {
                    n.Attributes["collisionMass"].Value =
                        (Math.Abs(float.Parse(n.Attributes["collisionMass"].Value, culture)*
                                  (0.5f + ((float) CorruptionPower/51)*RNG.NextDouble()))).ToString().Replace(',', '.');
                }
                if (n.Attributes["collisionRadius"] != null)
                {
                    n.Attributes["collisionRadius"].Value =
                        (float.Parse(n.Attributes["collisionRadius"].Value, culture)*
                         (0.5f + ((float) CorruptionPower/155)*RNG.NextDouble())).ToString();
                }
                if (n.Attributes["friction"] != null)
                {
                    n.Attributes["friction"].Value =
                        (float.Parse(n.Attributes["friction"].Value, culture)*
                         (0.95f + ((float) CorruptionPower/666)*RNG.NextDouble())).ToString().Replace(',', '.'); ;
                }
                if (n.Attributes["shadowSize"] != null)
                {
                    n.Attributes["shadowSize"].Value =
                        (float.Parse(n.Attributes["shadowSize"].Value, culture)*
                         (0.5f + ((float) CorruptionPower/32)*RNG.NextDouble())).ToString().Replace(',', '.'); ;
                }
                if (n.Attributes["stageHP"] != null)
                {
                    n.Attributes["stageHP"].Value =
                        (float.Parse(n.Attributes["stageHP"].Value, culture)*
                         (0.5f + ((float) CorruptionPower/32)*RNG.NextDouble())).ToString();
                }
                if (n.Attributes["boss"] != null)
                {
                    n.Attributes["boss"].Value = RNG.Next(0, 2).ToString();
                }
                if (n.Attributes["champion"] != null)
                {
                    n.Attributes["champion"].Value = RNG.Next(0, 2).ToString();
                }
            }
            XmlWriter XMWR = XmlWriter.Create("./__tmp.xml");
            XML.WriteTo(XMWR);
            XMWR.Close();
            File.Delete("./entities2.xml");
            File.Move("./__tmp.xml", "./entities2.xml");
        }

        private void CUI()
        {
            List<string> filenames = Directory.GetFiles("./gfx/ui/", "*.*", SearchOption.AllDirectories).ToList();
            foreach (string file in filenames)
            {
                TextReader TX = File.OpenText(file);
                XmlDocument XML = new XmlDocument();
                XML.LoadXml(TX.ReadToEnd());
                TX.Close();
                XmlWriter XMWR = XmlWriter.Create("./gfx/ui/__tmp.xml");



                AnimCont A = new AnimCont();
                foreach (XmlNode frame in XML.GetElementsByTagName("Frame"))
                {
                    CorruptFrame(frame, A);
                }

                XML.WriteTo(XMWR);
                XMWR.Close();

                File.Delete(file);
                File.Move("./gfx/ui/__tmp.xml", file);
            }
        }

        private void CFX()
        {
            

            TextReader TX = File.OpenText("./fxlayers.xml");           
            XmlDocument XML = new XmlDocument();
            XML.LoadXml("<a>" + TX.ReadToEnd() + "</a>");
            TX.Close();

            foreach (XmlNode fx in XML.GetElementsByTagName("fx"))
            {
                var culture = CultureInfo.InvariantCulture;
                fx.Attributes["numLayers"].Value =
                    (Math.Abs(float.Parse(fx.Attributes["numLayers"].Value, culture)*0.5*(float) CorruptionPower/45*
                              (float) RNG.NextDouble())).ToString();
                fx.Attributes["xMin"].Value =
                    (float.Parse(fx.Attributes["xMin"].Value, culture)*0.5*(float) CorruptionPower/45*
                     (float) RNG.NextDouble()).ToString();
                fx.Attributes["yMin"].Value =
                    (float.Parse(fx.Attributes["yMin"].Value, culture)*0.5*(float) CorruptionPower/45*
                     (float) RNG.NextDouble()).ToString();
                fx.Attributes["xMax"].Value =
                    (float.Parse(fx.Attributes["xMax"].Value, culture)*0.5*(float) CorruptionPower/45*
                     (float) RNG.NextDouble()).ToString();
                fx.Attributes["yMax"].Value =
                    (float.Parse(fx.Attributes["yMax"].Value, culture)*0.5*(float) CorruptionPower/45*
                     (float) RNG.NextDouble()).ToString();
                fx.Attributes["layer"].Value = (RNG.Next(0, 2) == 0) ? "foreground" : "background";
                if (fx.Attributes["stages"] != null) fx.Attributes["stages"].Value = RNG.Next(0, 7).ToString();
                if (fx.Attributes["altStage"] != null)
                    fx.Attributes["altStage"].Value = (RNG.Next(0, 2) == 0) ? "true" : "false";
                if (fx.Attributes["anyAlt"] != null)
                    fx.Attributes["anyAlt"].Value = (RNG.Next(0, 2) == 0) ? "true" : "false";
                if (fx.Attributes["parallax"] != null)
                    fx.Attributes["parallax"].Value =
                        (float.Parse(fx.Attributes["xMin"].Value, culture)*0.5*(float) CorruptionPower/45*
                         (float) RNG.NextDouble()).ToString();
            }

            foreach (XmlNode fx in XML.GetElementsByTagName("rayGroup"))
            {
                var culture = CultureInfo.InvariantCulture;
                fx.Attributes["xMin"].Value =
                    (float.Parse(fx.Attributes["xMin"].Value, culture)*3*(float) CorruptionPower/45*
                     (float) RNG.NextDouble()).ToString();
                fx.Attributes["topParallax"].Value =
                    (float.Parse(fx.Attributes["topParallax"].Value, culture)*3*(float) CorruptionPower/45*
                     (float) RNG.NextDouble()).ToString();
                fx.Attributes["xMax"].Value =
                    (float.Parse(fx.Attributes["xMax"].Value, culture)*3*(float) CorruptionPower/45*
                     (float) RNG.NextDouble()).ToString();
                fx.Attributes["bottomParallax"].Value =
                    (float.Parse(fx.Attributes["bottomParallax"].Value, culture)*3*(float) CorruptionPower/45*
                     (float) RNG.NextDouble()).ToString();
                fx.Attributes["rayLength"].Value =
                    (float.Parse(fx.Attributes["rayLength"].Value, culture)*3*(float) CorruptionPower/45*
                     (float) RNG.NextDouble()).ToString();
                fx.Attributes["raySpacing"].Value =
                    (float.Parse(fx.Attributes["raySpacing"].Value, culture)*3*(float) CorruptionPower/45*
                     (float) RNG.NextDouble()).ToString();
                fx.Attributes["perspective"].Value =
                    (float.Parse(fx.Attributes["perspective"].Value, culture)*3*(float) CorruptionPower/45*
                     (float) RNG.NextDouble()).ToString();
                fx.Attributes["stages"].Value = "1a,1b,2a,2b,3a,3b,4a,4b,5a,5b,6a,6b";
            }

            XmlWriter XMWR = XmlWriter.Create("./__tmp.xml");
            XML.WriteTo(XMWR);
            XMWR.Close();

            TX = File.OpenText("./__tmp.xml");
            string ntmp = TX.ReadToEnd().Replace("<a>", "").Replace("</a>", "");
            TX.Close();
            File.Delete("./ __tmp.xml");
            StreamWriter SW = new StreamWriter("./__tmp.xml");
            SW.Write(ntmp);
            SW.Close();

            File.Delete("./fxlayers.xml");
            File.Move("./__tmp.xml", "./fxlayers.xml");
        }
    }
}
