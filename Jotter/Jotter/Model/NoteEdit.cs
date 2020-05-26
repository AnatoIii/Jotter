using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Jotter.Model
{
	public class NoteEdit : INotifyPropertyChanged
    {
        public Guid Id { get; set; }

        private string _name;
        public string Name 
        {
            get { return _name; }
            set {
                _name = value;
                OnPropertyChanged("Name");
            }
        }
        public string Description { get; set; }
        public string CategoryId { get; set; }

        //public List<File> Files { get; set; }
        //public Category Category { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null) {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
