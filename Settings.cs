using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YARPG
{
    public partial class Settings : Form
    {
        private int verbosity, zoom;
        private bool dosounds, domonsterstatus, domonsterstatusonlylowhp, doplayerstatus, doplayerstatusonlylowhp, minimapdrawonmm, useminimap, donotupdatemainonmm;
        public static int Verbosity { get; set; }
        public static bool DoSounds { get; set; }
        public static bool DoMonsterStatus { get; set; }
        public static bool DoMonsterStatusOnlyLowHP { get; set; }
        public static bool DoPlayerStatus { get; set; }
        public static bool DoPlayerStatusOnlyLowHP { get; set; }
        public static bool MiniMapDrawOnMM { get; set; }
        public static bool UseMiniMap { get; set; }
        public static bool DoNotUpdateMainOnMM { get; set; }
        public static int Zoom { get; set; }

        public Settings()
        {
            InitializeComponent();
            verbosityNUD.Value = Verbosity; //load form from values
            soundeffectsCB.Checked = DoSounds;
            monsterstatusCB.Checked = DoMonsterStatus;
            monsterstatusonlowhpCB.Checked = DoMonsterStatusOnlyLowHP;
            playerstatusCB.Checked = DoPlayerStatus;
            playerstatusonlowhpCB.Checked = DoPlayerStatusOnlyLowHP;
            minimapdrawmmCB.Checked = MiniMapDrawOnMM;
            useminimapCB.Checked = UseMiniMap;
            donotupdatemainmapCB.Checked = DoNotUpdateMainOnMM;
            mainzoomNUD.Value = Zoom;
            BackupValues(); //back up value
        }

        private void VerbosityChanged(object sender, EventArgs e) => Verbosity = (int)verbosityNUD.Value; //set value
        private void MainZoomChanged(object sender, EventArgs e) => Zoom = (int)mainzoomNUD.Value; //set value
        private void SoundEffectsChanged(object sender, EventArgs e) => DoSounds = soundeffectsCB.Checked; //set value
        private void MonsterStatusChanged(object sender, EventArgs e) => DoMonsterStatus = monsterstatusCB.Checked; //set value
        private void MonsterStatusLowHPChanged(object sender, EventArgs e) => DoMonsterStatusOnlyLowHP = monsterstatusonlowhpCB.Checked; //set value
        private void PlayerStatusChanged(object sender, EventArgs e) => DoPlayerStatus = playerstatusCB.Checked; //set value
        private void PlayerStatusLowHPChanged(object sender, EventArgs e) => DoPlayerStatusOnlyLowHP = playerstatusonlowhpCB.Checked; //set value
        private void MiniMapDrawOnMMChanged(object sender, EventArgs e) => MiniMapDrawOnMM = minimapdrawmmCB.Checked; //set value
        private void UseMiniMapChanged(object sender, EventArgs e) => UseMiniMap = useminimapCB.Checked; //set value
        private void DoNotUpdateMainOnMMChanged(object sender, EventArgs e) => DoNotUpdateMainOnMM = donotupdatemainmapCB.Checked; //set value
        private void CancelClick(object sender, EventArgs e) => RestoreValues(true); //restore values
        private void DefaultsClick(object sender, EventArgs e) => Defaults(); //load defaults
        private void AdvancedClick(object sender, EventArgs e) => Advanced(); //load advanced settings
        private void AcceptClick(object sender, EventArgs e) => Close(); //close the form

        private void BackupValues()
        {
            verbosity = Verbosity; //back up values
            zoom = Zoom;
            dosounds = DoSounds;
            domonsterstatus = DoMonsterStatus;
            domonsterstatusonlylowhp = DoMonsterStatusOnlyLowHP;
            doplayerstatus = DoPlayerStatus;
            doplayerstatusonlylowhp = DoPlayerStatusOnlyLowHP;
            minimapdrawonmm = MiniMapDrawOnMM;
            useminimap = UseMiniMap;
            donotupdatemainonmm = DoNotUpdateMainOnMM;
        }

        private void RestoreValues(bool close)
        {
            Verbosity = verbosity; //restore values
            Zoom = zoom;
            DoSounds = dosounds;
            DoMonsterStatus = domonsterstatus;
            DoMonsterStatusOnlyLowHP = domonsterstatusonlylowhp;
            DoPlayerStatus = doplayerstatus;
            DoPlayerStatusOnlyLowHP = doplayerstatusonlylowhp;
            MiniMapDrawOnMM = minimapdrawonmm;
            UseMiniMap = useminimap;
            DoNotUpdateMainOnMM = donotupdatemainonmm;
            if (close) //if close
            {
                Close(); //close the form
            }
        }

        public static void Defaults()
        {
            Verbosity = 3; //set default values
            Zoom = 15;
            DoSounds = true;
            DoMonsterStatus = true;
            DoMonsterStatusOnlyLowHP = true;
            DoPlayerStatus = true;
            DoPlayerStatusOnlyLowHP = false;
            MiniMapDrawOnMM = false;
            UseMiniMap = false;
            DoNotUpdateMainOnMM = false;
        }

        private static void Advanced() => MessageBox.Show("There are no advanced items yet.", "Info", MessageBoxButtons.OK); //show message
    }
}
