using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Crud
{
    public class ViewModel : INotifyPropertyChanged
    {
        private string log;

        public List<Pessoa> Pessoas { get; set; }

        public string Log { get { return log; } set { log = value; OnPropertyChanged(); } }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}