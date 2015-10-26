using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Xml;
using System.IO;
using System.Runtime.CompilerServices;

namespace Undefined
{
    public partial class Undefined3 : Form
    {
        private const string unpackdotbat =
            "cd bin\nGibbed.Rebirth.Unpack.exe ../packed/animations.a\nGibbed.Rebirth.Unpack.exe ../packed/config.a\nGibbed.Rebirth.ConvertAnimations.exe ../packed/animations_unpack/resources/animations.b\n";

        private string AnimFolderUnpacked = "./packed/animations_unpack/resources/animations_converted/resources/gfx";
        private string GFXFolderUnpacked = "./packed/graphics_unpack/resources/gfx";
        private string ConfigFolderUnpacked = "./packed/config_unpack/resources";

        private List<string> AMN2List = new List<string>(); 

        private string GFXFolder = ".\\gfx";

        public static byte CorruptionPower = 0;

        private List<string> ConfigFiles = new List<string>();
        private string[] ShuffleEntityGFXFiles = new[] {"entities2.xml"};
        private string[] GarbleTextFiles = new[] {"fortunes.txt", "rules.txt", "stages.xml", "entities2.xml", "players.xml", "items.xml", "pocketitems.xml"};
        private string[] CorruptEntityAnimationsFiles = new[] { "entities2.xml"};
        private string[] CorruptEntityStatsFiles = new[] {"entities2.xml", "bosscolors.xml"};
        private string[] CorruptFXFiles = new[] {"fxlayers.xml"};
        private string[] ShuffleMusicFiles = new[] {"music.xml", "sounds.xml"};
        private string[] ShuffleItemGFXFiles = new[] {"items.xml", "pocketitems.xml", "costumes2.xml"};
        private string[] ShuffleVideosFiles = new[] {"cutscenes.xml"};
        private string[] ShuffleFloorGFXFiles = new[] {"stages.xml", "backdrops.xml"};
        private string[] CorruptItemStatsFiles = new[] {"items.xml", "costumes2.xml"};

        private string Seed = "";
        private byte[] SeedHash = {0xDE,0xAD,0xB0,0x0B};
        private bool ShuffleEntityGFX = false;
        private bool GarbleText = false;
        private bool CorruptEntityAnimations = false;
        private bool CorruptEntityStats = false;
        private bool CorruptUI = false;
        private bool CorruptFX = false;
        private bool ShuffleMusic = false;
        private bool ShuffleItemGFX = false;
        private bool ShuffleVideos = false;
        private bool ShuffleFloorGFX = false;
        private bool CorruptItemStats = false;
        private bool CorruptMenu = false;
        public static Random RNG;

        public Undefined3() {InitializeComponent();}
        private void Undefined3_Load(object sender, EventArgs e){}
        private void EntityGFX_Checkbox_CheckedChanged(object sender, EventArgs e) {ShuffleEntityGFX = EntityGFX_Checkbox.Checked;}
        private void GarbleText_Checkbox_CheckedChanged(object sender, EventArgs e) {GarbleText = GarbleText_Checkbox.Checked;}
        private void EntityAnimations_Checkbox_CheckedChanged(object sender, EventArgs e) {CorruptEntityAnimations = EntityAnimations_Checkbox.Checked;}
        private void EntityStats_Checkbox_CheckedChanged(object sender, EventArgs e) {CorruptEntityStats = EntityStats_Checkbox.Checked;}
       // private void CorruptUI_Checkbox_CheckedChanged(object sender, EventArgs e) {CorruptUI = CorruptUI_Checkbox.Checked;}
        private void CorruptFX_Checkbox_CheckedChanged(object sender, EventArgs e) {CorruptFX = CorruptFX_Checkbox.Checked;}
       // private void CorruptMenu_Checkbox_CheckedChanged(object sender, EventArgs e) {CorruptMenu = CorruptMenu_Checkbox.Checked;}
        private void ShuffleMusic_Checkbox_CheckedChanged(object sender, EventArgs e) {ShuffleMusic = ShuffleMusic_Checkbox.Checked;}
        private void ItemGFX_Checkbox_CheckedChanged(object sender, EventArgs e) {ShuffleItemGFX = ItemGFX_Checkbox.Checked;}
        private void ShuffleVideos_Checkbox_CheckedChanged(object sender, EventArgs e) {ShuffleVideos = ShuffleVideos_Checkbox.Checked;}
       // private void FloorGFX_Checkbox_CheckedChanged(object sender, EventArgs e) {ShuffleFloorGFX = FloorGFX_Checkbox.Checked;}
       // private void ItemStats_Checkbox_CheckedChanged(object sender, EventArgs e) {CorruptItemStats = ItemStats_Checkbox.Checked;}
        private void Seed_Textbox_TextChanged(object sender, EventArgs e) {Seed = Seed_Textbox.Text;}
        private void Begin_Button_Click(object sender, EventArgs e) {Corrupt();}
        private void CorruptionPower_Textbox_TextChanged(object sender, EventArgs e){}

        void Corrupt()
        {
            DisableInput();
            if (!InitRNG())
            {
                ReenableInput();
                return;
            }          
            SetupFiles();
            if (ShuffleEntityGFX){SE_GFX();}
            if (GarbleText){GText();}
            if (CorruptEntityAnimations){CEANIM();}
            if (CorruptEntityStats){CES();}
            if (CorruptUI){CUI();}
            if (CorruptFX)
            {
                CFX();
            }
            if (ShuffleMusic)
            {
                SS();
            }
            if (ShuffleItemGFX)
            {
                SIGFX();
            }
            if (ShuffleVideos)
            {
                SV();
            }
            ReenableInput();
            
        }

        void DisableInput()
        {
            this.ControlBox = false;
            EntityGFX_Checkbox.Enabled = false;
            GarbleText_Checkbox.Enabled = false;
            EntityAnimations_Checkbox.Enabled = false;
            EntityStats_Checkbox.Enabled = false;
           // CorruptUI_Checkbox.Enabled = false;
            CorruptFX_Checkbox.Enabled = false;
           // CorruptMenu_Checkbox.Enabled = false;
            ShuffleMusic_Checkbox.Enabled = false;
            ItemGFX_Checkbox.Enabled = false;
            ShuffleVideos_Checkbox.Enabled = false;
           // FloorGFX_Checkbox.Enabled = false;
          //  ItemStats_Checkbox.Enabled = false;
            Seed_Textbox.Enabled = false;
            Begin_Button.Enabled = false;
        }

        void ReenableInput()
        {
            this.ControlBox = true;
            EntityGFX_Checkbox.Enabled = true;
            GarbleText_Checkbox.Enabled = true;
            EntityAnimations_Checkbox.Enabled = true;
            EntityStats_Checkbox.Enabled = true;
           // CorruptUI_Checkbox.Enabled = true;
            CorruptFX_Checkbox.Enabled = true;
           // CorruptMenu_Checkbox.Enabled = true;
            ShuffleMusic_Checkbox.Enabled = true;
            ItemGFX_Checkbox.Enabled = true;
            ShuffleVideos_Checkbox.Enabled = true;
           // FloorGFX_Checkbox.Enabled = true;
        //    ItemStats_Checkbox.Enabled = true;
            Seed_Textbox.Enabled = true;
            Begin_Button.Enabled = true;
        }

        private bool InitRNG()
        {
            if (!byte.TryParse(CorruptionPower_Textbox.Text, out CorruptionPower))
            {
                MessageBox.Show("Please enter a proper corruption power value.");
                return false;
            }
            SeedHash = new byte[] {0xDE, 0xAD, 0xB0, 0x0B};
            var hashbyte = 0;
            var seedchar = 0;
            while (seedchar < Seed.Length)
            {
                SeedHash[hashbyte] += (byte) Seed[seedchar];
                seedchar++;
                hashbyte = (hashbyte + 1)%SeedHash.Length;
            }
            if (Seed != "")
            {
                RNG = new Random((int)BitConverter.ToUInt32(SeedHash, 0));
            }
            else
            {
                RNG = new Random((int)(DateTime.Now - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds);
            }
            return true;
        }

        void SetupFiles()
        {
            if (!Directory.Exists(ConfigFolderUnpacked) || !Directory.Exists(AnimFolderUnpacked))
            {
                if (Directory.Exists("./packed/config_unpack")) { Directory.Delete("./packed/config_unpack", true); }
                if (Directory.Exists("./packed/animations_unpack")) { Directory.Delete("./packed/animations_unpack", true); }
                if (!Directory.Exists("./bin"))
                {
                    MessageBox.Show("Please place \"bin\" directory from Rick's Unpacker in the folder and then try again.");
                    Application.Exit();
                }
                FileStream unpackbatch = File.Create("./bin/_unpack.bat");
                byte[] bytes = new byte[unpackdotbat.Length];
                for (int i = 0; i < unpackdotbat.Length; i++)
                {
                    bytes[i] = (byte)unpackdotbat[i];
                }
                unpackbatch.Write(bytes, 0, bytes.Length);
                unpackbatch.Close();
                ProcessStartInfo PSI = new ProcessStartInfo(".\\bin\\_unpack.bat");
                Process PR = new Process();
                PR.StartInfo = PSI;
                PR.Start();
                PR.WaitForExit();
                File.Delete("./bin/_unpack.bat");
            }

            ConfigFiles = new List<string>();
            if (CorruptEntityStats) { ConfigFiles.AddRange(CorruptEntityStatsFiles); }
            if (ShuffleEntityGFX) { ConfigFiles.AddRange(ShuffleEntityGFXFiles); }
            if (GarbleText) { ConfigFiles.AddRange(GarbleTextFiles); }
            if (CorruptFX) { ConfigFiles.AddRange(CorruptFXFiles); }
            if (ShuffleMusic) { ConfigFiles.AddRange(ShuffleMusicFiles); }
            if (ShuffleItemGFX) { ConfigFiles.AddRange(ShuffleItemGFXFiles); }
            if (ShuffleVideos) { ConfigFiles.AddRange(ShuffleVideosFiles); }
            if (ShuffleFloorGFX) { ConfigFiles.AddRange(ShuffleFloorGFXFiles); }
            if (CorruptItemStats) { ConfigFiles.AddRange(CorruptItemStatsFiles); }
            if (CorruptEntityAnimations){ ConfigFiles.AddRange(CorruptEntityAnimationsFiles);}
            ConfigFiles = ConfigFiles.Distinct().ToList();

            foreach (string path in Directory.GetFiles(ConfigFolderUnpacked))
            {
                string tpath = path.Replace(ConfigFolderUnpacked, ".");
                if (File.Exists(tpath))
                {
                    File.Delete(tpath);
                }
                foreach (string cfile in ConfigFiles)
                {
                    if (path.Contains(cfile))
                    {
                        if (File.Exists(tpath))
                        {
                            File.Delete(tpath);
                        }

                        File.Copy(path, tpath);
                    }
                }
            }
            if (Directory.Exists(GFXFolder))
            {
                Directory.Delete(GFXFolder, true);
                Directory.CreateDirectory(GFXFolder);
            }

            foreach (string dirPath in Directory.GetDirectories(AnimFolderUnpacked, "*",
                SearchOption.AllDirectories))
            {
                if (!Directory.Exists(dirPath.Replace(AnimFolderUnpacked, GFXFolder))) { Directory.CreateDirectory(dirPath.Replace(AnimFolderUnpacked, GFXFolder));}
            }

            foreach (string newPath in Directory.GetFiles(AnimFolderUnpacked, "*.*",
                SearchOption.AllDirectories))
            {
                File.Copy(newPath, newPath.Replace(AnimFolderUnpacked, GFXFolder), true);
            }
        }

        
    }
}
