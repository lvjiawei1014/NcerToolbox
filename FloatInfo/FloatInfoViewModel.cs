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


        public bool CheckCurrentInputStatusChanged(ref string info)
        {

            var ret = InputMethod.Current.ImeConversionMode.HasFlag(ImeConversionModeValues.Native);
            return ret != imeConversionModeNative;
        }


        public async void QueryKeyboardAsync()
        {
            string info="";
            while (true)
            {
                await Task.Delay(100);
                if(CheckCurrentInputStatusChanged(ref info))
                {
                    this.OnInfo?.Invoke(this,info);
                }
            }
        }
    }
}
