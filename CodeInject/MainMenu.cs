using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeInject
{
    public unsafe partial class MainMenu : Form
    {
        #region externals
        [DllImport("ClrBootstrap.dll")]
        public static extern UInt64 GetBaseAdress();

        [DllImport("ClrBootstrap.dll")]
        public static extern UInt64 GetInt64(UInt64 Adress);
        [DllImport("ClrBootstrap.dll")]
        public static extern int GetInt32(UInt64 Adress);
        [DllImport("ClrBootstrap.dll")]
        public static extern float GetFloat(UInt64 Adress);
        [DllImport("ClrBootstrap.dll")]
        public static extern int SendPacketToServer(UInt64 deviceAddr, byte[] packet);

        [DllImport("ClrBootstrap.dll")]
        public static extern byte GetByte(UInt64 Adress);


        [DllImport("ClrBootstrap.dll")]
        public static extern short GetShort(UInt64 Adress);

        [DllImport("ClrBootstrap.dll")]
        public static extern void GetByteArray(UInt64 adress, byte[] outTable, int size);

        [DllImport("ClrBootstrap.dll")]
        public static extern void ActionCommand(int actionId,uint targetID = 0xE0000000);
        #endregion

        int* playerState;
        enum PlayerStates
        {
            CatchCommon=292,CatchUncommon = 293, CatchRare = 294, CatchEpic = 295,StandWithRod = 271, Unknow =0, Unknow2=273

        }

        public MainMenu()
        {
            InitializeComponent();
            playerState = (int*)(new IntPtr((long)(GetBaseAdress() + 0x1823320)).ToPointer());
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = $"STATE: {*playerState}";

            if (*playerState == (int)PlayerStates.CatchCommon || *playerState == (int)PlayerStates.CatchUncommon || *playerState == (int)PlayerStates.CatchRare || *playerState == (int)PlayerStates.CatchEpic)
            {
                ActionCommand(0x128);//Catch fish command
            }
            else if (*playerState == (int)PlayerStates.StandWithRod || *playerState == (int)PlayerStates.Unknow || *playerState == (int)PlayerStates.Unknow2)
            {
                //player standing
                ActionCommand(0x121); //Bait command
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ActionCommand(0x121);
            timer1.Enabled = !timer1.Enabled;
            button1.Text = !timer1.Enabled ? "START" : "STOP";
        }

    }
}
