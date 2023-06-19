using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
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
        #endregion


        /*
    ffxiv.exe + 56AB93 - 6A 00 - push 00
    ffxiv.exe + 56AB95 - 6A 00 - push 00
    ffxiv.exe + 56AB97 - 6A 00 - push 00
    ffxiv.exe + 56AB99 - 6A 00 - push 00
    ffxiv.exe + 56AB9B - E8 7090EBFF - call ffxiv.exe + 423C10
    ffxiv.exe + 56ABA0 - 52 - push edx
    ffxiv.exe + 56ABA1 - 50 - push eax
    ffxiv.exe + 56ABA2 - 56 - push esi
    ffxiv.exe + 56ABA3 - 6A 01 - push 01
    ffxiv.exe + 56ABA5 - B9 C0BD5702 - mov ecx, ffxiv.exe + 16BBDC0
    ffxiv.exe + 56ABAA - E8 91A62900 - call ffxiv.exe + 805240
*/
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        unsafe delegate void CallAction(void* argThis, int arg0, int actionID, uint targetID, int arg3, int arg4, int arg5, int arg6, int arg7);


        public void ActionCommand(int actionId, uint targetID = 0xE0000000)
        {
            CallAction delegateRecive = (CallAction)Marshal.GetDelegateForFunctionPointer(new IntPtr((long)(GetBaseAdress() + 0x805240)), typeof(CallAction));
            delegateRecive((new IntPtr((long)(GetBaseAdress() + 0x16BBDC0)).ToPointer()), 1, actionId, targetID, 0, 0, 0, 0, 0);
        }


        int* playerState;
        enum PlayerStates
        {
            WeakBite=292,StrongBite = 293, FerociousBite = 294, UnknowBite = 295,StandWithRod = 271, Unknow =0, Unknow2=273

        }

        public MainMenu()
        {
            InitializeComponent();


            playerState = (int*)(new IntPtr((long)(GetBaseAdress() + 0x1823320)).ToPointer());
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = $"STATE: {*playerState}";

            if (*playerState == (int)PlayerStates.WeakBite || *playerState == (int)PlayerStates.StrongBite || *playerState == (int)PlayerStates.FerociousBite || *playerState == (int)PlayerStates.UnknowBite)
            {
                ActionCommand(0x128);//Catch fish command
            }
            else if (*playerState == (int)PlayerStates.StandWithRod || *playerState == (int)PlayerStates.Unknow || *playerState == (int)PlayerStates.Unknow2)
            {
                if(chUseMooch.Checked)
                {
                    ActionCommand(0x129);  
                }

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
