using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace Undefined
{
    public partial class Undefined3 : Form
    {

        // Constant filenames
        private const string RicksUnpackerBatchFile =
            "cd bin\nGibbed.Rebirth.Unpack.exe ../packed/animations.a\nGibbed.Rebirth.Unpack.exe ../packed/config.a\nGibbed.Rebirth.ConvertAnimations.exe ../packed/animations_unpack/resources/animations.b\n";
        private const string AnimFolderUnpackedResources = "./packed/animations_unpack/resources/animations_converted/resources/gfx";
        private const string ConfigFolderUnpackedResources = "./packed/config_unpack/resources";
        private const string ConfigFolderUnpacked = "./packed/config_unpack";
        private const string AnimFolderUnpacked = "./packed/animations_unpack";
        private const string GFXFolder = ".\\gfx";
        private List<string> ConfigFiles = new List<string>();
        private static readonly string[] ShuffleEntityGFXFiles = {"entities2.xml"};
        private static readonly string[] GarbleTextFiles = {"fortunes.txt", "rules.txt", "stages.xml", "entities2.xml", "players.xml", "items.xml", "pocketitems.xml"};
        private static readonly string[] CorruptEntityAnimationsFiles = { "entities2.xml"};
        private static readonly string[] CorruptEntityStatsFiles = {"entities2.xml"};
        private static readonly string[] CorruptFXFiles = {"fxlayers.xml"};
        private static readonly string[] ShuffleMusicFiles = {"music.xml"};
        private static readonly string[] ShuffleSoundsFiles = { "sounds.xml" };
        private static readonly string[] ShuffleItemGFXFiles = {"items.xml", "pocketitems.xml", "costumes2.xml"};
        private static readonly string[] ShuffleVideosFiles = {"cutscenes.xml"};
        private static readonly string[] ShuffleFloorGFXFiles = {"stages.xml", "backdrops.xml"};
        private static readonly string[] CorruptItemStatsFiles = {"items.xml", "costumes2.xml"};

        // Seeding
        private string Seed = "";
        private byte[] SeedHash = {0xDE,0xAD,0xB0,0x0B};

        // Options
        private bool ShuffleEntityGFX = false;
        private bool GarbleText = false;
        private bool CorruptEntityAnimations = false;
        private bool CorruptEntityStats = false;       
        private bool CorruptFX = false;
        private bool ShuffleMusic = false;
        private bool ShuffleItemGFX = false;
        private bool ShuffleVideos = false;       
        private bool CorruptItemStats = false;
        private bool ShuffleParticleGFX = false;
        private bool ShuffleIsaacSprite = false;
        private bool ShuffleBossGFX = false;
        private bool FixInvisibleEntities = true;
        private bool DisableContactDamage = false;
        private bool WalkThroughWalls = false;
        private bool ShuffleSounds = false;
        private bool AntiCrash = true;

        /* UNUSED
        private bool CorruptMenu = false;
        private bool CorruptUI = false;
        private bool ShuffleFloorGFX = false; */

        // Randomness
        public static Random RNG;
        public static byte CorruptionPower = 0;


        


        /* UNUSED
        private void CorruptUI_Checkbox_CheckedChanged(object sender, EventArgs e) {CorruptUI = CorruptUI_Checkbox.Checked;}
        private void CorruptMenu_Checkbox_CheckedChanged(object sender, EventArgs e) {CorruptMenu = CorruptMenu_Checkbox.Checked;}
        private void FloorGFX_Checkbox_CheckedChanged(object sender, EventArgs e) {ShuffleFloorGFX = FloorGFX_Checkbox.Checked;}
        private void ItemStats_Checkbox_CheckedChanged(object sender, EventArgs e) {CorruptItemStats = ItemStats_Checkbox.Checked;} */


        // Main corrupting function
        void Corrupt()
        {
            DisableInput();
            if (!InitRNG() || !SetupFiles()) // If an error occurs when initializing RNG
            {
                ReenableInput();
                return;
            }          

            if (ShuffleEntityGFX)
            {
                SE_GFX();
            }
            if (GarbleText)
            {
                GText();
            }
            if (CorruptEntityAnimations)
            {
                CEANIM();
            }
            if (CorruptEntityStats)
            {
                CES();
            }
            
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

            /* UNUSED
            if (CorruptUI)
            {
                CUI();
            }               */

            ReenableInput();
            
        }

        void DisableInput()
        {
            ControlBox = false;
            EntityGFX_Checkbox.Enabled = false;
            GarbleText_Checkbox.Enabled = false;
            EntityAnimations_Checkbox.Enabled = false;
            EntityStats_Checkbox.Enabled = false; 
            CorruptFX_Checkbox.Enabled = false; 
            ShuffleMusic_Checkbox.Enabled = false;
            ItemGFX_Checkbox.Enabled = false;
            ShuffleVideos_Checkbox.Enabled = false; 
            Seed_Textbox.Enabled = false;
            Begin_Button.Enabled = false;
            ParticleGFX_Checkbox.Enabled = false;
            IsaacSprite_Checkbox.Enabled = false;
            BossGFX_Checkbox.Enabled = false;
            FixInvisible_Checkbox.Enabled = false;
            WalkWalls_Checkbox.Enabled = false;
            AntiCrash_Checkbox.Enabled = false;
            ContactDamage_Checkbox.Enabled = false;

            /* UNUSED
            FloorGFX_Checkbox.Enabled = false;
            ItemStats_Checkbox.Enabled = false;
            CorruptMenu_Checkbox.Enabled = false;
            CorruptUI_Checkbox.Enabled = false; */
        }

        void ReenableInput()
        {
            this.ControlBox = true;
            EntityGFX_Checkbox.Enabled = true;
            GarbleText_Checkbox.Enabled = true;
            EntityAnimations_Checkbox.Enabled = true;
            EntityStats_Checkbox.Enabled = true;          
            CorruptFX_Checkbox.Enabled = true;          
            ShuffleMusic_Checkbox.Enabled = true;
            ItemGFX_Checkbox.Enabled = true;
            ShuffleVideos_Checkbox.Enabled = true;           
            Seed_Textbox.Enabled = true;
            Begin_Button.Enabled = true;
            ParticleGFX_Checkbox.Enabled = ShuffleEntityGFX;
            IsaacSprite_Checkbox.Enabled = ShuffleEntityGFX;
            BossGFX_Checkbox.Enabled = ShuffleEntityGFX;
            FixInvisible_Checkbox.Enabled = CorruptEntityAnimations;
            WalkWalls_Checkbox.Enabled = CorruptEntityStats;
            AntiCrash_Checkbox.Enabled = true;
            ContactDamage_Checkbox.Enabled = CorruptEntityStats;

            /* UNUSED
            FloorGFX_Checkbox.Enabled = true;
            ItemStats_Checkbox.Enabled = true;
            CorruptMenu_Checkbox.Enabled = true;
            CorruptUI_Checkbox.Enabled = true; */
        }

        private bool InitRNG()
        {
            // Testing for proper corruption power value
            if (!byte.TryParse(CorruptionPower_Textbox.Text, out CorruptionPower))
            {
                MessageBox.Show("Please enter a proper corruption power value.");
                return false;
            } 

            // If seed exists, get random from it, else get it from unix time
            if (Seed != "")
            {
                RNG = new Random(Seed.GetHashCode());
            }
            else
            {
                RNG = new Random((int)(DateTime.Now - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds);
            }
            return true;
        }

        private bool SetupFiles() // let the try/catch madness begin
        {

            // if unpacked folders need to be recreated
            if (!Directory.Exists(ConfigFolderUnpackedResources) || !Directory.Exists(AnimFolderUnpackedResources))
            {
                if (!Directory.Exists("./packed"))
                {
                    MessageBox.Show(
                        "Can't find the \"packed\" directory. Please move the application to the correct folder and try again.");
                    return false;
                }
                    
                // cleaning previous unpacked folders
                if (Directory.Exists(ConfigFolderUnpacked))
                {
                    try
                    {
                        Directory.Delete(ConfigFolderUnpacked, true);
                    }
                    catch (Exception ex)
                    {
                        if (ex is System.IO.IOException)
                        {
                            MessageBox.Show(
                                "IOException has occured. Please check whether the folder \"(rebirth)/resources/packed/config_unpack\" is closed, has no read-only files inside, and is not being used by any other process, including the file explorer.");
                            
                        }
                        else
                        {
                            MessageBox.Show("Exception " + ex.ToString() + " has occured. This shouldn't happen.");
                        }
                        return false;
                    }
                }

                if (Directory.Exists(AnimFolderUnpacked))
                {
                    try
                    {
                        Directory.Delete(AnimFolderUnpacked, true);
                    }
                    catch (Exception ex)
                    {
                        if (ex is System.IO.IOException)
                        {
                            MessageBox.Show(
                                "IOException has occured. Please check whether the folder \"(rebirth)/resources/packed/animations_unpack\" is closed, has no read-only files inside, and is not being used by any other process, including the file explorer.");                                                     
                        }
                        else
                        {
                            MessageBox.Show("Exception " + ex.ToString() + " has occured. This shouldn't happen.");
                        }
                        return false;
                    }
                }
               
                if (!Directory.Exists("./bin"))
                {
                    MessageBox.Show("Please place \"bin\" directory from Rick's Unpacker in the folder and then try again.");
                    return false;
                }

                // creating _unpack.bat
                FileStream unpackbatch;
                try
                {
                    unpackbatch = File.Create("./bin/_unpack.bat");
                }
                catch (Exception ex)
                {
                    if (ex is UnauthorizedAccessException)
                    {
                        MessageBox.Show(
                            "UnauthorizedAccessException has occured. It means that the file \"(rebirth)/resources/bin/_unpack.bat\" exists and is read-only. Please delete it or change it to be writable and try again.");
                    }
                    else if (ex is IOException)
                    {
                        MessageBox.Show(
                            "IOException has occured while creating unpacking batch program. Please try again.");
                    }
                    else
                    {
                        MessageBox.Show("Exception " + ex.ToString() + " has occured. This shouldn't happen.");
                    }
                    return false;
                }
                
                // Writing contents of the unpacker bat file to _unpack.bat
                byte[] bytes = new byte[RicksUnpackerBatchFile.Length];
                for (int i = 0; i < RicksUnpackerBatchFile.Length; i++)
                {
                    bytes[i] = (byte)RicksUnpackerBatchFile[i];
                }
                unpackbatch.Write(bytes, 0, bytes.Length);
                unpackbatch.Close();

                // Running the _unpack.bat file
                ProcessStartInfo PSI = new ProcessStartInfo(".\\bin\\_unpack.bat");
                Process PR = new Process();
                PR.StartInfo = PSI;
                PR.Start();
                PR.WaitForExit();

                // Cleanup
                File.Delete("./bin/_unpack.bat");
            }

            // Collecting files to be copied to resources folder into a nice list
            ConfigFiles = new List<string>();
            if (CorruptEntityStats) { ConfigFiles.AddRange(CorruptEntityStatsFiles); }
            if (ShuffleEntityGFX) { ConfigFiles.AddRange(ShuffleEntityGFXFiles); }
            if (GarbleText) { ConfigFiles.AddRange(GarbleTextFiles); }
            if (CorruptFX) { ConfigFiles.AddRange(CorruptFXFiles); }
            if (ShuffleMusic) { ConfigFiles.AddRange(ShuffleMusicFiles); }
            if (ShuffleSounds) {ConfigFiles.AddRange(ShuffleSoundsFiles);}
            if (ShuffleItemGFX) { ConfigFiles.AddRange(ShuffleItemGFXFiles); }
            if (ShuffleVideos) { ConfigFiles.AddRange(ShuffleVideosFiles); }
            if (CorruptItemStats) { ConfigFiles.AddRange(CorruptItemStatsFiles); }
            if (CorruptEntityAnimations){ ConfigFiles.AddRange(CorruptEntityAnimationsFiles);}

            /* UNUSED
            if (ShuffleFloorGFX) { ConfigFiles.AddRange(ShuffleFloorGFXFiles); } */

            ConfigFiles = ConfigFiles.Distinct().ToList();

            // Get the list of config files to clean
            string[] paths;
            try
            {
                paths = Directory.GetFiles(ConfigFolderUnpackedResources);
            }
            catch (Exception ex)
            {
                if (ex is IOException)
                {
                    MessageBox.Show("IOException has occured while getting the list of config files. Please try again.");                    
                }
                else
                {
                    MessageBox.Show("Exception " + ex.ToString() + " has occured. This shouldn't happen.");
                }
                return false;
            }

            // Removing previous corruptions
            foreach (string path in paths)
            {
                string tpath = path.Replace(ConfigFolderUnpackedResources, ".");
                if (File.Exists(tpath))
                {
                    try
                    {
                        File.Delete(tpath);
                    }
                    catch (Exception ex)
                    {
                        if (ex is IOException)
                        {
                            MessageBox.Show(
                                "IOException has occured. Please close any applications using any files from \"(rebirth)/resources\" except for this one and try again.");
                        }
                        else
                        {
                            MessageBox.Show("Exception " + ex.ToString() + " has occured. This shouldn't happen.");
                        }
                        return false;
                    }
                    
                }

                // Setting up config files
                foreach (string cfile in ConfigFiles)
                {
                    if (path.Contains(cfile))
                    {
                        try
                        {
                            File.Copy(path, tpath, true);
                        }
                        catch (Exception ex)
                        {
                            if (ex is IOException)
                            {
                                MessageBox.Show(
                                "IOException has occured while copying config files. Please try again.");
                            }
                            else
                            {
                                MessageBox.Show("Exception " + ex.ToString() + " has occured. This shouldn't happen.");
                            }
                            return false;
                        }
                        
                    }
                }
            }


            if (Directory.Exists(GFXFolder))
            {
                try
                {
                    Directory.Delete(GFXFolder, true);
                }
                catch (Exception ex)
                {
                    if (ex is System.IO.IOException)
                    {
                        MessageBox.Show(
                            "IOException has occured. Please check whether the folder \"(rebirth)/resources/gfx\" is closed, has no read-only files inside, and is not being used by any other process, including the file explorer.");
                    }
                    else
                    {
                        MessageBox.Show("Exception " + ex.ToString() + " has occured. This shouldn't happen.");
                    }
                    return false;
                }

                try
                {
                    Directory.CreateDirectory(GFXFolder);
                }
                catch (Exception ex)
                {
                    if (ex is IOException)
                    {
                        MessageBox.Show(
                        "IOException has occured while creating animation folder. Please try again.");
                    }
                    else
                    {
                        MessageBox.Show("Exception " + ex.ToString() + " has occured. This shouldn't happen.");
                    }
                    return false;
                }
                
            }


            foreach (string dirPath in Directory.GetDirectories(AnimFolderUnpackedResources, "*",
                SearchOption.AllDirectories))
            {
                if (!Directory.Exists(dirPath.Replace(AnimFolderUnpackedResources, GFXFolder)))
                {
                    try
                    {
                        Directory.CreateDirectory(dirPath.Replace(AnimFolderUnpackedResources, GFXFolder));
                    }
                    catch (Exception ex)
                    {
                        if (ex is IOException)
                        {
                            MessageBox.Show(
                            "IOException has occured while creating internal animation folders. Please try again.");
                        }
                        else
                        {
                            MessageBox.Show("Exception " + ex.ToString() + " has occured. This shouldn't happen.");
                        }
                        return false;
                    }
                    
                    
                }
            }

            foreach (string newPath in Directory.GetFiles(AnimFolderUnpackedResources, "*.*",
                SearchOption.AllDirectories))
            {
                try
                {
                    File.Copy(newPath, newPath.Replace(AnimFolderUnpackedResources, GFXFolder), true);
                }
                catch (Exception ex)
                {
                                 
                    if (ex is IOException)
                    {
                        MessageBox.Show(
                        "IOException has occured while copying animation files. Please try again.");
                    }
                    else
                    {
                        MessageBox.Show("Exception " + ex.ToString() + " has occured. This shouldn't happen.");
                    }
                    return false;                   
                }             
            }

            return true;
        }


        // Internal Windows Forms Functions
        private void ContactDamage_Checkbox_CheckedChanged(object sender, EventArgs e)
        {
            DisableContactDamage = ContactDamage_Checkbox.Checked;
        }

        private void WalkWalls_Checkbox_CheckedChanged(object sender, EventArgs e)
        {
            WalkThroughWalls = WalkWalls_Checkbox.Checked;
        }     
        public Undefined3() { InitializeComponent(); }
        private void Undefined3_Load(object sender, EventArgs e) { }

        private void EntityGFX_Checkbox_CheckedChanged(object sender, EventArgs e)
        {
            ShuffleEntityGFX = EntityGFX_Checkbox.Checked;
            ParticleGFX_Checkbox.Enabled = EntityGFX_Checkbox.Checked;
            IsaacSprite_Checkbox.Enabled = EntityGFX_Checkbox.Checked;
            BossGFX_Checkbox.Enabled = EntityGFX_Checkbox.Checked;
            if (!ShuffleEntityGFX)
            {
                ParticleGFX_Checkbox.Checked = false;
                IsaacSprite_Checkbox.Checked = false;
                BossGFX_Checkbox.Checked = false;
                ShuffleBossGFX = false;
                ShuffleParticleGFX = false;
                ShuffleIsaacSprite = false;
            }
        }
        private void GarbleText_Checkbox_CheckedChanged(object sender, EventArgs e) { GarbleText = GarbleText_Checkbox.Checked; }

        private void EntityAnimations_Checkbox_CheckedChanged(object sender, EventArgs e)
        {
            CorruptEntityAnimations = EntityAnimations_Checkbox.Checked;
            FixInvisible_Checkbox.Enabled = EntityAnimations_Checkbox.Checked;
            if (!CorruptEntityAnimations)
            {
                FixInvisible_Checkbox.Checked = false;
                FixInvisibleEntities = false;
            }
        }
        private void EntityStats_Checkbox_CheckedChanged(object sender, EventArgs e)
        {
            CorruptEntityStats = EntityStats_Checkbox.Checked;
            ContactDamage_Checkbox.Enabled = CorruptEntityStats;
            WalkWalls_Checkbox.Enabled = CorruptEntityStats;
            if (!CorruptEntityStats)
            {
                ContactDamage_Checkbox.Checked = false;
                WalkWalls_Checkbox.Checked = false;
                DisableContactDamage = false;
                WalkThroughWalls = false;
            }
        }
        private void CorruptFX_Checkbox_CheckedChanged(object sender, EventArgs e) { CorruptFX = CorruptFX_Checkbox.Checked; }
        private void ShuffleMusic_Checkbox_CheckedChanged(object sender, EventArgs e) { ShuffleMusic = ShuffleMusic_Checkbox.Checked; }
        private void ItemGFX_Checkbox_CheckedChanged(object sender, EventArgs e) { ShuffleItemGFX = ItemGFX_Checkbox.Checked; }
        private void ShuffleVideos_Checkbox_CheckedChanged(object sender, EventArgs e) { ShuffleVideos = ShuffleVideos_Checkbox.Checked; }
        private void Seed_Textbox_TextChanged(object sender, EventArgs e) { Seed = Seed_Textbox.Text; }
        private void Begin_Button_Click(object sender, EventArgs e) { Corrupt(); }
        private void CorruptionPower_Textbox_TextChanged(object sender, EventArgs e) { }
        private void ParticleGFX_Checkbox_CheckedChanged(object sender, EventArgs e) { ShuffleParticleGFX = ParticleGFX_Checkbox.Checked; }
        private void IsaacSprite_Checkbox_CheckedChanged(object sender, EventArgs e) { ShuffleIsaacSprite = IsaacSprite_Checkbox.Checked; }
        private void BossGFX_Checkbox_CheckedChanged(object sender, EventArgs e) { ShuffleBossGFX = BossGFX_Checkbox.Checked; }
        private void FixInvisible_Checkbox_CheckedChanged(object sender, EventArgs e) { FixInvisibleEntities = FixInvisible_Checkbox.Checked; }

        private void ShuffleSounds_Checkbox_CheckedChanged(object sender, EventArgs e)
        {
            ShuffleSounds = ShuffleSounds_Checkbox.Checked;
        }

        private void AntiCrash_Checkbox_CheckedChanged(object sender, EventArgs e)
        {
            AntiCrash = !AntiCrash_Checkbox.Checked;
        }
    }
}
