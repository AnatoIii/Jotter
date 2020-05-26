using Jotter.MainWindow.Enums;
using Jotter.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Jotter.MainWindow
{
	class MainViewModel : INotifyPropertyChanged
	{
        private MainWindowStates _mainWindowStates;

        public NoteEdit NewNoteData { get; set; }

        public MainViewModel()
        {
            MainWindowState = MainWindowStates.NoteViewing;
            NewNoteData = new NoteEdit() {
                Name = "asd"
            };
        }

        public MainWindowStates MainWindowState
        {
            get { return _mainWindowStates; }
            private set {
                _mainWindowStates = value;
                OnPropertyChanged("MainWindowState");
            }
        }

        private Command createNoteCommand;
        public Command CreateNoteCommand
        {
            get {
                return createNoteCommand ??
                    (createNoteCommand = new Command(obj => {
                        var y = NewNoteData.Name;
                        var x = 124;
                    })
                );
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
