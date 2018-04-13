using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MVVMCP.ViewModels
{
    public class ViewModelBase: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handle = PropertyChanged;
            if (handle != null)
                handle(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
