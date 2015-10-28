using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace Undefined3
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
        private const string GFXFolder = "./gfx";
        private List<string> ConfigFiles = new List<string>();
        private static readonly string[] ShuffleEntityGFXFiles = { "entities2.xml" };
        private static readonly string[] GarbleTextFiles = { "fortunes.txt", "rules.txt", "stages.xml", "entities2.xml", "players.xml", "items.xml", "pocketitems.xml" };
        private static readonly string[] CorruptEntityAnimationsFiles = { "entities2.xml" };
        private static readonly string[] CorruptEntityStatsFiles = { "entities2.xml" };
        private static readonly string[] CorruptFXFiles = { "fxlayers.xml" };
        private static readonly string[] ShuffleMusicFiles = { "music.xml" };
        private static readonly string[] ShuffleSoundsFiles = { "sounds.xml" };
        private static readonly string[] ShuffleItemGFXFiles = { "items.xml", "pocketitems.xml", "costumes2.xml" };
        private static readonly string[] ShuffleVideosFiles = { "cutscenes.xml" };
        private static readonly string[] CorruptItemStatsFiles = { "items.xml", "costumes2.xml" };

        // Seeding
        private string Seed = "";

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

        // Randomness
        public static Random RNG;
        public static byte CorruptionPower = 0;


        // Main corrupting function
        void Corrupt()
        {
            DisableInput();
            if (!InitRNG() || !SetupFiles())
            {
                goto End;
            }

            if (ShuffleEntityGFX)
            {
                if (!ShuffleEntityGFX_Func())
                {
                    goto End;
                }
            }

            if (GarbleText)
            {
                if (!GarbleText_Func())
                {
                    goto End;
                }
            }

            if (CorruptEntityAnimations)
            {
                if (!CorruptEntityAnimations_Func())
                {
                    goto End;
                }
            }

            if (CorruptEntityStats)
            {
                if (!CorruptEntityStats_Func())
                {
                    goto End;
                }
            }

            if (CorruptFX)
            {
                if (!CorruptFX_Func())
                {
                    goto End;
                }
            }

            if (ShuffleMusic)
            {
                if (!ShuffleMusic_Func())
                {
                    goto End;
                }
            }

            if (ShuffleSounds)
            {
                if (!ShuffleSounds_Func())
                {
                    goto End;
                }
            }

            if (ShuffleItemGFX)
            {
                if (!ShuffleItemGFX_Func())
                {
                    goto End;
                }
            }

            if (ShuffleVideos)
            {
                if (!ShuffleVideos_Func())
                {
                    goto End;
                }
            }

            End:
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
                    if (!Safe.DeleteDirectory(ConfigFolderUnpacked))
                    {
                        return false;
                    }
                }

                if (Directory.Exists(AnimFolderUnpacked))
                {
                    if (!Safe.DeleteDirectory(AnimFolderUnpacked))
                    {
                        return false;
                    }
                }

                if (!Directory.Exists("./bin"))
                {
                    MessageBox.Show("Please place \"bin\" directory from Rick's Unpacker in the folder and then try again.");
                    return false;
                }

                // creating _unpack.bat
                TextWriter unpackbatch;
                unpackbatch = Safe.OpenStreamWriter("./bin/_unpack.bat");
                if (unpackbatch == null)
                {
                    return false;
                }

                try
                {
                    unpackbatch.Write(RicksUnpackerBatchFile);
                }
                catch (Exception ex)
                {
                    if (ex is IOException)
                    {
                        MessageBox.Show($"IOException has occured while saving the ./bin/_unpack.bat. Please try again.");
                    }
                    else if (ex is ObjectDisposedException)
                    {
                        MessageBox.Show($"ObjectDisposedException has occured while saving the modified ./bin/_unpack.bat. Should never happen.");
                    }
                    else
                    {
                        MessageBox.Show("Exception " + ex + $" has occured while saving the modified ./bin/_unpack.bat as a text file. Should never happen.");
                    }
                    return false;
                }

                unpackbatch.Close();

                // Running the _unpack.bat file
                Process PR = new Process { StartInfo = new ProcessStartInfo(".\\bin\\_unpack.bat") };

                try
                {
                    PR.Start();
                }
                catch (Exception ex)
                {
                    if (ex is InvalidOperationException)
                    {
                        MessageBox.Show(
                            "InvalidOperationException has occured while starting ./bin/_unpack.bat. Should never happen.");
                    }
                    else if (ex is ArgumentNullException)
                    {
                        MessageBox.Show(
                            "ArgumentNullException has occured while starting ./bin/_unpack.bat. Should never happen.");
                    }
                    else if (ex is ObjectDisposedException)
                    {
                        MessageBox.Show(
                            "ObjectDisposedException has occured while starting ./bin/_unpack.bat. Should never happen.");
                    }
                    else if (ex is FileNotFoundException)
                    {
                        MessageBox.Show(
                            "FileNotFoundException has occured while starting ./bin/_unpack.bat. Please ensure that the file ./bin/_unpack.bat exists and try again.");
                    }
                    else if (ex is Win32Exception)
                    {
                        MessageBox.Show(
                            "Win32Exception has occured while starting ./bin/_unpack.bat. Please try again.");
                    }
                    else
                    {
                        MessageBox.Show(
                            "Exception " + ex + " has occured while starting ./bin/_unpack.bat. Please try again.");
                    }
                    return false;
                }

                PR.WaitForExit();

                // Cleanup
                if (!Safe.DeleteFile("./bin/_unpack.bat"))
                {
                    return false;
                }
            }

            // Collecting files to be copied to resources folder into a nice list
            ConfigFiles = new List<string>();
            if (CorruptEntityStats) { ConfigFiles.AddRange(CorruptEntityStatsFiles); }
            if (ShuffleEntityGFX) { ConfigFiles.AddRange(ShuffleEntityGFXFiles); }
            if (GarbleText) { ConfigFiles.AddRange(GarbleTextFiles); }
            if (CorruptFX) { ConfigFiles.AddRange(CorruptFXFiles); }
            if (ShuffleMusic) { ConfigFiles.AddRange(ShuffleMusicFiles); }
            if (ShuffleSounds) { ConfigFiles.AddRange(ShuffleSoundsFiles); }
            if (ShuffleItemGFX) { ConfigFiles.AddRange(ShuffleItemGFXFiles); }
            if (ShuffleVideos) { ConfigFiles.AddRange(ShuffleVideosFiles); }
            if (CorruptItemStats) { ConfigFiles.AddRange(CorruptItemStatsFiles); }
            if (CorruptEntityAnimations) { ConfigFiles.AddRange(CorruptEntityAnimationsFiles); }

            /* UNUSED
            if (ShuffleFloorGFX) { ConfigFiles.AddRange(ShuffleFloorGFXFiles); } */

            ConfigFiles = ConfigFiles.Distinct().ToList();

            // Get the list of config files to clean
            List<string> paths;
            paths = Safe.GetFiles(ConfigFolderUnpackedResources);
            if (paths == null)
            {
                return false;
            }

            // Removing previous corruptions
            foreach (string path in paths)
            {
                string tpath = path.Replace(ConfigFolderUnpackedResources, ".");
                if (File.Exists(tpath))
                {
                    if (!Safe.DeleteFile(tpath))
                    {
                        return false;
                    }
                }

                // Setting up config files
                foreach (string cfile in ConfigFiles)
                {
                    if (path.Contains(cfile))
                    {
                        if (!Safe.CopyFile(path, tpath))
                        {
                            return false;
                        }
                    }
                }
            }


            if (Directory.Exists(GFXFolder))
            {
                if (!Safe.DeleteDirectory(GFXFolder))
                {
                    return false;
                }

                if (!Safe.CreateDirectory(GFXFolder))
                {
                    return false;
                }
            }

            List<string> dirs = Safe.GetDirectories(AnimFolderUnpackedResources, "*");
            if (dirs == null)
            {
                return false;
            }

            if (dirs.Where(dirPath => !Directory.Exists(dirPath.Replace(AnimFolderUnpackedResources, GFXFolder))).Any(dirPath => !Safe.CreateDirectory(dirPath.Replace(AnimFolderUnpackedResources, GFXFolder))))
            {
                return false;
            }

            dirs = Safe.GetFiles(AnimFolderUnpackedResources);
            if (dirs == null)
            {
                return false;
            }

            return dirs.All(newPath => Safe.CopyFile(newPath, newPath.Replace(AnimFolderUnpackedResources, GFXFolder)));
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
