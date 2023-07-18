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
        [DllImport("ClrBootstrapx64.dll")]
        public static extern UInt64 GetBaseAdress();

        [DllImport("ClrBootstrapx64.dll")]
        public static extern UInt64 GetInt64(UInt64 Adress);
        [DllImport("ClrBootstrapx64.dll")]
        public static extern int GetInt32(UInt64 Adress);
        [DllImport("ClrBootstrapx64.dll")]
        public static extern float GetFloat(UInt64 Adress);
        [DllImport("ClrBootstrapx64.dll")]
        public static extern int SendPacketToServer(UInt64 deviceAddr, byte[] packet);

        [DllImport("ClrBootstrapx64.dll")]
        public static extern byte GetByte(UInt64 Adress);


        [DllImport("ClrBootstrapx64.dll")]
        public static extern short GetShort(UInt64 Adress);

        [DllImport("ClrBootstrapx64.dll")]
        public static extern void GetByteArray(UInt64 adress, byte[] outTable, int size);
        #endregion


   
        unsafe delegate void CallActionx64(void* arg1, int arg2, int arg3, uint arg4, int arg5,
                      int arg6, int arg7);
        CallActionx64 callActionFuncx64;
        /*
         * 7ff73fcc34a0  uint64_t sub_7ff73fcc34a0(void* arg1, int32_t arg2, int32_t arg3, int64_t arg4, int32_t arg5, 
                      int32_t arg6, char* arg7)
        */





        public void ActionCommandx64(int actionId, uint targetId = 0xE0000000)
        {
            //this = 00007FF6497AFFCB | 48:8D0D 7E5AA701         | lea rcx,qword ptr ds:[7FF64B225A50]     |
            callActionFuncx64((new IntPtr((long)(GetBaseAdress() + 0x20F5A50)).ToPointer()), 1, actionId, targetId, 0, 0, 0);
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

            //x64
            playerState = (int*)(new IntPtr((long)(GetBaseAdress() + 0x2174E20)).ToPointer());

            //00007FF6497AFFF0 | E8 AB353300              | call ffxiv_dx11.7FF649AE35A0            |
            callActionFuncx64 = (CallActionx64)Marshal.GetDelegateForFunctionPointer(new IntPtr((long)(GetBaseAdress() + 0x9B35A0)), typeof(CallActionx64));
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = $"STATE: {*playerState}";

            if (*playerState == (int)PlayerStates.WeakBite || *playerState == (int)PlayerStates.StrongBite || *playerState == (int)PlayerStates.FerociousBite || *playerState == (int)PlayerStates.UnknowBite)
            {
                ActionCommandx64((int)Action.Hook);//Catch fish command
            }
            else if (*playerState == (int)PlayerStates.StandWithRod || *playerState == (int)PlayerStates.Unknow || *playerState == (int)PlayerStates.Unknow2)
            {
                if(chUseMooch.Checked)
                {
                    ActionCommandx64((int)Action.Mooch);  
                }

                //player standing
                ActionCommandx64((int)Action.Bait); //Bait command
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ActionCommandx64(0x121);
           timer1.Enabled = !timer1.Enabled;
            button1.Text = !timer1.Enabled ? "START" : "STOP";
        }

    }
}
