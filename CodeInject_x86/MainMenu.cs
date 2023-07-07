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
        [DllImport("ClrBootstrapx86.dll")]
        public static extern UInt64 GetBaseAdress();

        [DllImport("ClrBootstrapx86.dll")]
        public static extern UInt64 GetInt64(UInt64 Adress);
        [DllImport("ClrBootstrapx86.dll")]
        public static extern int GetInt32(UInt64 Adress);
        [DllImport("ClrBootstrapx86.dll")]
        public static extern float GetFloat(UInt64 Adress);
        [DllImport("ClrBootstrapx86.dll")]
        public static extern int SendPacketToServer(UInt64 deviceAddr, byte[] packet);

        [DllImport("ClrBootstrapx86.dll")]
        public static extern byte GetByte(UInt64 Adress);


        [DllImport("ClrBootstrapx86.dll")]
        public static extern short GetShort(UInt64 Adress);

        [DllImport("ClrBootstrapx86.dll")]
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
        unsafe delegate void CallActionx32(void* argThis, int arg0, int actionID, uint targetID, int arg3, int arg4, int arg5, int arg6, int arg7);
        CallActionx32 callActionFuncx86;


        public void ActionCommandx86(int actionId, uint targetId = 0xE0000000)
        {
            callActionFuncx86((new IntPtr((long)(GetBaseAdress() + 0x16C3DD0)).ToPointer()), 1, actionId, targetId, 0, 0, 0, 0, 0);
        }


        int* playerState;
        enum PlayerStates
        {
            WeakBite=292,StrongBite = 293, FerociousBite = 294, UnknowBite = 295,StandWithRod = 271, Unknow =0, Unknow2=273

        }

        enum Action
        {
            Bait= 0x121,Hook=0x128,Mooch = 0x129,MakeShiftBait = 0x000068B5
        }

        public MainMenu()
        {
            InitializeComponent();
            playerState = (int*)(new IntPtr((long)(GetBaseAdress() + 0x182B3A0)).ToPointer());

            callActionFuncx86 = (CallActionx32)Marshal.GetDelegateForFunctionPointer(new IntPtr((long)(GetBaseAdress() + 0x8055C0)), typeof(CallActionx32));
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = $"STATE: {*playerState}";

            if (*playerState == (int)PlayerStates.WeakBite || *playerState == (int)PlayerStates.StrongBite || *playerState == (int)PlayerStates.FerociousBite || *playerState == (int)PlayerStates.UnknowBite)
            {
                ActionCommandx86((int)Action.Hook);//Catch fish command
            }
            else if (*playerState == (int)PlayerStates.StandWithRod || *playerState == (int)PlayerStates.Unknow || *playerState == (int)PlayerStates.Unknow2)
            {
                if(chUseMooch.Checked)
                {
                    ActionCommandx86((int)Action.Mooch);  
                }

                //player standing
                ActionCommandx86((int)Action.Bait); //Bait command
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ActionCommandx86(0x121);
           timer1.Enabled = !timer1.Enabled;
            button1.Text = !timer1.Enabled ? "START" : "STOP";
        }

    }
}
