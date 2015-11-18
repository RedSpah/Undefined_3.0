using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Globalization;
using System.Linq;

namespace Undefined3
{


    public partial class Undefined3
    {
        private void CorruptFrame(XmlNode frame, AnimCont context)
        {
            var culture = CultureInfo.InvariantCulture;
            if (frame.Attributes["XPosition"] != null)
                frame.Attributes["XPosition"].Value =
                    (double.Parse(frame.Attributes["XPosition"].Value, culture) + ((!FixInvisibleEntities) ? (context.context[0]) : (context.context[0] % 8))).ToString();
            if (frame.Attributes["YPosition"] != null)
                frame.Attributes["YPosition"].Value =
                    (double.Parse(frame.Attributes["YPosition"].Value, culture) + ((!FixInvisibleEntities) ? (context.context[1]) : (context.context[1] % 8))).ToString();
            if (frame.Attributes["Delay"] != null)
                frame.Attributes["Delay"].Value =
                    ((int)(Math.Abs(double.Parse(frame.Attributes["Delay"].Value, culture) + context.context[2]))).ToString();
            if (frame.Attributes["XScale"] != null)
                frame.Attributes["XScale"].Value =
                    (double.Parse(frame.Attributes["XScale"].Value, culture) + ((!FixInvisibleEntities) ? (context.context[3]) : (context.context[3] % 8))).ToString();
            if (frame.Attributes["YScale"] != null)
                frame.Attributes["YScale"].Value =
                    (double.Parse(frame.Attributes["YScale"].Value, culture) + ((!FixInvisibleEntities) ? (context.context[4]) : (context.context[4] % 8))).ToString();
            if (frame.Attributes["RedTint"] != null)
                frame.Attributes["RedTint"].Value =
                    ((double.Parse(frame.Attributes["RedTint"].Value, culture) + context.context[5]) % 256).ToString();
            if (frame.Attributes["GreenTint"] != null)
                frame.Attributes["GreenTint"].Value =
                    ((double.Parse(frame.Attributes["GreenTint"].Value, culture) + context.context[6]) % 256).ToString();
            if (frame.Attributes["BlueTint"] != null)
                frame.Attributes["BlueTint"].Value =
                   ((double.Parse(frame.Attributes["BlueTint"].Value, culture) + context.context[7]) % 256).ToString();
            if (frame.Attributes["RedOffset"] != null)
                frame.Attributes["RedOffset"].Value =
                    ((double.Parse(frame.Attributes["RedOffset"].Value, culture) + context.context[8]) % 256).ToString();
            if (frame.Attributes["GreenOffset"] != null)
                frame.Attributes["GreenOffset"].Value =
                    ((double.Parse(frame.Attributes["GreenOffset"].Value, culture) + context.context[9]) % 256).ToString();
            if (frame.Attributes["BlueOffset"] != null)
                frame.Attributes["BlueOffset"].Value =
                    ((double.Parse(frame.Attributes["BlueOffset"].Value, culture) + context.context[10]) % 256).ToString();
            if (frame.Attributes["AlphaTint"] != null)
                frame.Attributes["AlphaTint"].Value =
                    ((double.Parse(frame.Attributes["AlphaTint"].Value, culture) + context.context[11]) % 256 + 144).ToString();
            if (frame.Attributes["Rotation"] != null)
                frame.Attributes["Rotation"].Value =
                    (double.Parse(frame.Attributes["Rotation"].Value, culture) + context.context[12]).ToString();
            context.Mod();
        }



        private bool CorruptEntityAnimations_Func()
        {
            List<string> tocorrupt = new List<string>();

            if (!LoadXMLAndModify("./entities2.xml", delegate (XmlDocument XML)
            { tocorrupt.AddRange(from XmlNode n in XML.LastChild.ChildNodes select n.Attributes["anm2path"].Value); }))
            {
                return false;
            }

            tocorrupt.RemoveAll(x => x.Length == 0);
            tocorrupt.RemoveAll(x => x.Contains("Fireworks") == true);

            if (
                !tocorrupt.Select(corruptfile => "./gfx/" + corruptfile)
                    .All(toopen => LoadXMLAndModify(toopen, delegate (XmlDocument XML)
                    {
                        AnimCont A = new AnimCont();
                        foreach (XmlNode frame in XML.GetElementsByTagName("Frame"))
                        {
                            CorruptFrame(frame, A);
                        }
                    })))
            {
                return false;
            }

            List<string> itemfiles = Safe.GetFiles("./gfx/characters");
            return itemfiles.All(file => LoadXMLAndModify(file, delegate (XmlDocument XML)
            {
                AnimCont A = new AnimCont();
                foreach (XmlNode frame in XML.GetElementsByTagName("Frame"))
                {
                    CorruptFrame(frame, A);
                }
            }));
        }

        private bool CorruptEntityStats_Func()
        {
            return LoadXMLAndModify("./entities2.xml", delegate (XmlDocument XML)
            {
                foreach (XmlNode n in XML.LastChild.ChildNodes)
                {
                    var culture = CultureInfo.InvariantCulture;
                    if (n.Attributes["baseHP"] != null)
                    {
                        n.Attributes["baseHP"].Value =
                            (!OneHitKills) ? ((float.Parse(n.Attributes["baseHP"].Value, culture) *
                             (0.5f + ((float)CorruptionPower / 166) * RNG.NextDouble())).ToString()) : ((n.Attributes["baseHP"].Value == "0") ? "0" : "1");
                    }
                    if (n.Attributes["collisionDamage"] != null)
                    {
                        n.Attributes["collisionDamage"].Value =
                            (float.Parse(n.Attributes["collisionDamage"].Value, culture) *
                             ((DisableContactDamage) ? (0.5f + ((float)CorruptionPower / 32) * RNG.NextDouble()) : 0)).ToString();
                    }
                    if (n.Attributes["collisionMass"] != null)
                    {
                        n.Attributes["collisionMass"].Value =
                            (Math.Abs(float.Parse(n.Attributes["collisionMass"].Value, culture) *
                                      (0.5f + ((float)CorruptionPower / 51) * RNG.NextDouble()))).ToString().Replace(',', '.');
                    }
                    if (n.Attributes["collisionRadius"] != null)
                    {
                        n.Attributes["collisionRadius"].Value =
                            ((n.Attributes["name"].Value != "Player" || !WalkThroughWalls) ?
                            (float.Parse(n.Attributes["collisionRadius"].Value, culture) *
                             (0.5f + ((float)CorruptionPower / 155) * RNG.NextDouble())).ToString() : "0");
                    }
                    if (n.Attributes["friction"] != null)
                    {
                        n.Attributes["friction"].Value =
                            (float.Parse(n.Attributes["friction"].Value, culture) *
                             (0.95f + ((float)CorruptionPower / 1024) * RNG.NextDouble())).ToString().Replace(',', '.'); ;
                    }
                    if (n.Attributes["shadowSize"] != null)
                    {
                        n.Attributes["shadowSize"].Value =
                            (float.Parse(n.Attributes["shadowSize"].Value, culture) *
                             (0.5f + ((float)CorruptionPower / 32) * RNG.NextDouble())).ToString().Replace(',', '.'); ;
                    }
                    if (n.Attributes["stageHP"] != null)
                    {
                        n.Attributes["stageHP"].Value =
                            (!OneHitKills) ? ((float.Parse(n.Attributes["stageHP"].Value, culture) *
                             (0.5f + ((float)CorruptionPower / 32) * RNG.NextDouble())).ToString()) : ((n.Attributes["stageHP"].Value == "0") ? "0" : "1");
                    }
                    if (n.Attributes["boss"] != null)
                    {
                        n.Attributes["boss"].Value = RNG.Next(0, 2).ToString();
                    }
                    if (n.Attributes["champion"] != null && !AntiCrash)
                    {
                        n.Attributes["champion"].Value = RNG.Next(0, 2).ToString();
                    }
                    if (n.Attributes["id"].Value == "1" && WalkThroughWalls)
                    {
                        n.Attributes["numGridCollisionPoints"].Value = "0";
                    }

                }
            });
        }

        private bool CorruptFX_Func()
        {
            return LoadTXTAndModify("./fxlayers.xml", text => "<a>" + text + "</a>") &&

            LoadXMLAndModify("./fxlayers.xml", delegate (XmlDocument XML)
            {


                foreach (XmlNode fx in XML.GetElementsByTagName("fx"))
                {
                    var culture = CultureInfo.InvariantCulture;
                    fx.Attributes["numLayers"].Value =
                        (Math.Abs(float.Parse(fx.Attributes["numLayers"].Value, culture) * 0.5 * (float)CorruptionPower / 45 *
                                  (float)RNG.NextDouble())).ToString();
                    fx.Attributes["xMin"].Value =
                        (float.Parse(fx.Attributes["xMin"].Value, culture) * 0.5 * (float)CorruptionPower / 45 *
                         (float)RNG.NextDouble()).ToString();
                    fx.Attributes["yMin"].Value =
                        (float.Parse(fx.Attributes["yMin"].Value, culture) * 0.5 * (float)CorruptionPower / 45 *
                         (float)RNG.NextDouble()).ToString();
                    fx.Attributes["xMax"].Value =
                        (float.Parse(fx.Attributes["xMax"].Value, culture) * 0.5 * (float)CorruptionPower / 45 *
                         (float)RNG.NextDouble()).ToString();
                    fx.Attributes["yMax"].Value =
                        (float.Parse(fx.Attributes["yMax"].Value, culture) * 0.5 * (float)CorruptionPower / 45 *
                         (float)RNG.NextDouble()).ToString();
                    fx.Attributes["layer"].Value = (RNG.Next(0, 2) == 0) ? "foreground" : "background";
                    if (fx.Attributes["stages"] != null) fx.Attributes["stages"].Value = RNG.Next(0, 7).ToString();
                    if (fx.Attributes["altStage"] != null)
                        fx.Attributes["altStage"].Value = (RNG.Next(0, 2) == 0) ? "true" : "false";
                    if (fx.Attributes["anyAlt"] != null)
                        fx.Attributes["anyAlt"].Value = (RNG.Next(0, 2) == 0) ? "true" : "false";
                    if (fx.Attributes["parallax"] != null)
                        fx.Attributes["parallax"].Value =
                            (float.Parse(fx.Attributes["xMin"].Value, culture) * 0.5 * (float)CorruptionPower / 45 *
                             (float)RNG.NextDouble()).ToString();
                }

                foreach (XmlNode fx in XML.GetElementsByTagName("rayGroup"))
                {
                    var culture = CultureInfo.InvariantCulture;
                    fx.Attributes["xMin"].Value =
                        (float.Parse(fx.Attributes["xMin"].Value, culture) * 3 * (float)CorruptionPower / 45 *
                         (float)RNG.NextDouble()).ToString();
                    fx.Attributes["topParallax"].Value =
                        (float.Parse(fx.Attributes["topParallax"].Value, culture) * 3 * (float)CorruptionPower / 45 *
                         (float)RNG.NextDouble()).ToString();
                    fx.Attributes["xMax"].Value =
                        (float.Parse(fx.Attributes["xMax"].Value, culture) * 3 * (float)CorruptionPower / 45 *
                         (float)RNG.NextDouble()).ToString();
                    fx.Attributes["bottomParallax"].Value =
                        (float.Parse(fx.Attributes["bottomParallax"].Value, culture) * 3 * (float)CorruptionPower / 45 *
                         (float)RNG.NextDouble()).ToString();
                    fx.Attributes["rayLength"].Value =
                        (float.Parse(fx.Attributes["rayLength"].Value, culture) * 3 * (float)CorruptionPower / 45 *
                         (float)RNG.NextDouble()).ToString();
                    fx.Attributes["raySpacing"].Value =
                        (float.Parse(fx.Attributes["raySpacing"].Value, culture) * 3 * (float)CorruptionPower / 45 *
                         (float)RNG.NextDouble()).ToString();
                    fx.Attributes["perspective"].Value =
                        (float.Parse(fx.Attributes["perspective"].Value, culture) * 3 * (float)CorruptionPower / 45 *
                         (float)RNG.NextDouble()).ToString();
                    fx.Attributes["stages"].Value = "1a,1b,2a,2b,3a,3b,4a,4b,5a,5b,6a,6b";

                }
            }) &&

            LoadTXTAndModify("./fxlayers.xml", text => text.Replace("<a>", "").Replace("</a>", ""));
        }
    }
}
