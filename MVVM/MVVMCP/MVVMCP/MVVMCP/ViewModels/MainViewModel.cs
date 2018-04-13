using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace MVVMCP.ViewModels  
{
    public class MainViewModel : ViewModelBase
    {
        //private string _myMessage;
        //public MainViewModel()
        //{
        //    MyMessage = "Hello MVVM";
        //}
        //public string MyMessage
        //{
        //    get { return _myMessage; }
        //    set
        //    {
        //        _myMessage = value;
        //        RaisePropertyChanged("MyMessage");
        //    }
        //}





        private int _counter;
        private DelegateCommand _myCommand;

        void counterCommandExecute()
        {
            _counter++;
            RaisePropertyChanged("MyMessage");
        }

        public MainViewModel()
        {
            _counter = 0;
        }

        public string MyMessage
        {
            get { return string.Format("{0} times", _counter); }
        }

        public ICommand MyCommand
        {
            get { return _myCommand = _myCommand ?? new DelegateCommand(counterCommandExecute); }
        }

    }
}
