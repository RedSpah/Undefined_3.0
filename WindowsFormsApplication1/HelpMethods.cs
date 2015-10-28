using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Windows.Forms;


namespace Undefined3
{
    public partial class Undefined3
    {

        bool LoadXMLAndModify(string filename, Action<XmlDocument> modification)
        {
            XmlDocument XML = new XmlDocument();

            if (!Safe.LoadXML(XML, filename))
            {
                return false;
            }

            try
            {
                modification(XML);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception " + ex + $" has occured while modifying {filename} as an XML file. Should never happen outside of debugging.");
                return false;
            }

            try
            {
                XML.Save("./__tmp.xml");
            }
            catch (Exception ex)
            {
                if (ex is XmlException)
                {
                    MessageBox.Show(
                        $"XmlException has occured when saving {filename} with an XMLDocument. Should never happen.");
                }
                else
                {
                    MessageBox.Show("Exception " + ex + $" has occured when saving {filename} with an XMLDocument.");
                }
                return false;
            }

            if (!Safe.DeleteFile(filename))
            {
                return false;
            }

            if (!Safe.MoveFile("./__tmp.xml", filename))
            {
                return false;
            }

            return true;
        }

        bool LoadTXTAndModify(string filename, Func<string, string> modification)
        {
            TextReader TXR = Safe.OpenTextReader(filename);
            TextWriter TXW = Safe.OpenStreamWriter("./__tmp.txt");
            string text = TXR.ReadToEnd();
            TXR.Close();

            try
            {
                text = modification(text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception " + ex + $" has occured while modifying {filename} as a text file. Should never happen outside of debugging.");
                return false;
            }

            try
            {
                TXW.Write(text);
            }
            catch (Exception ex)
            {
                if (ex is IOException)
                {
                    MessageBox.Show($"IOException has occured while saving the modified {filename}. Please try again.");
                }
                else if (ex is ObjectDisposedException)
                {
                    MessageBox.Show($"ObjectDisposedException has occured while saving the modified {filename}. Should never happen.");
                }
                else
                {
                    MessageBox.Show("Exception " + ex + $" has occured while saving the modified {filename} as a text file. Should never happen.");
                }
                return false;
            }

            if (!Safe.DeleteFile(filename))
            {
                return false;
            }

            TXW.Close();

            if (!Safe.MoveFile("./__tmp.txt", filename))
            {
                return false;
            }

            return true;
        }

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
                        q += RNG.Next(0, mutilatepow + 2);
                    }
                    while (q > 128)
                    {
                        q += RNG.Next(-mutilatepow - 1, 0);
                    }
                    let[i] = (char)q;
                }

            }
            return new string(let.ToArray());
        }
    }

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
            for (int i = 0; i < 13; i++)
            {
                context[i] = (int)(context[i] * (1 + (Undefined3.RNG.NextDouble() - 0.5) * (float)Undefined3.CorruptionPower / 128));
            }
        }

        public int[] context = new int[13];
    }
}
