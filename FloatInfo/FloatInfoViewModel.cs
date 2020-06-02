using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FloatInfo
{
    public class FloatInfoViewModel: INotifyPropertyChanged
    {
        public event EventHandler<string> OnInfo;

        #region notifychanged
        /// <summary>
        /// Occurs when a property value changes. 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event for the property with the specified
        /// name, or the calling property if no name is specified.
        /// </summary>
        public void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #endregion

        #region win32 api
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)] static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")] extern static IntPtr GetKeyboardLayout(uint threadId);
        [DllImport("imm32.dll")] extern static IntPtr ImmGetContext(IntPtr ptr);
        [DllImport("imm32.dll")] extern static bool ImmGetConversionStatus(IntPtr ptr,ref uint concersion,ref uint sentence);


        #endregion


        private string info="";
        private bool imeConversionModeNative = false;
        public string Info
        {
            get => info; set
            {
                info = value;
                OnPropertyChanged();
            }
        }


        private bool GetImmConversionStatusNative()
        {
            uint conversion = 0;
            uint sentence = 0;
            IntPtr pFore = GetForegroundWindow();
            System.Console.WriteLine(pFore.ToInt32());
            if(!ImmGetConversionStatus(ImmGetContext(pFore), ref  conversion, ref sentence))
            {
                throw new Exception("获取输入法信息失败!");
            }
            System.Console.WriteLine(conversion);
            return (conversion & 1)==1;
        }


        public bool CheckCurrentInputStatusChanged(ref string info)
        {
            //var ret = InputMethod.Current.ImeConversionMode.HasFlag(ImeConversionModeValues.Native);
            var ret = this.GetImmConversionStatusNative();
            var result = ret != imeConversionModeNative;
            imeConversionModeNative = ret;
            info = ret ? "中文" : "English";
            return result;
        }


        public async void QueryKeyboardAsync()
        {
            string info="";
            while (true)
            {
                await Task.Delay(100);
                try
                {
                    if (CheckCurrentInputStatusChanged(ref info))
                    {
                        this.OnInfo?.Invoke(this, info);
                    }
                }
                catch (Exception ex)
                {
                    this.OnInfo?.Invoke(this, ex.Message);
                }
                
            }
        }
    }
}
