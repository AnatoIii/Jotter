using BL;
using Jotter.AddCategory;
using Jotter.EnterCategoryPassword;
using Jotter.MainWindow.Enums;
using Jotter.Model;
using Microsoft.Win32;
using Model;
using Model.DTO;
using Ninject;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Jotter.MainWindow
{
	class MainViewModel : INotifyPropertyChanged
	{
        private readonly IStorage _storage;

        private MainWindowStates _mainWindowStates;

        public NoteEdit SavingNoteData { get; set; }
        public Note SelectedNote { get; set; }
        public Visibility CreateNoteButtonVisibility { get; set; }
        public string CreateEditNoteText { get; set; }

        public ObservableCollection<Note> CategoryNotes { get; set; }
        public ObservableCollection<Category> CategoriesCollection { get; }

        public MainViewModel(IStorage storage)
        {
            _storage = storage;

            MainWindowState = MainWindowStates.NotesShowing;
            SavingNoteData = new NoteEdit();

            CategoriesCollection = new ObservableCollection<Category>(_storage.GetCategories().Response.Categories);
            CategoryNotes = new ObservableCollection<Note>();

            CreateNoteButtonVisibility = Visibility.Hidden;
        }

        private Category _selectedCategory;
        public Category SelectedCategory {
            get {
                return _selectedCategory;
            }
            set {
                _selectedCategory = value;
                
                if (_selectedCategory.IsPrivate) {
                    var viewModel = new EnterCategoryPasswordWindowViewModel(_selectedCategory.Name);
                    var window = new EnterCategoryPasswordWindow(viewModel);
                    bool confirmed = false;
                    while (!confirmed) {
                        if (window.ShowDialog() == true) {
                            confirmed = true;
                        }

                        var password = window.GetCategoryPassword();
                        //_storage.TryGetPasswordedCategoryNote(new CategoryData { Id = _selectedCategory.Id, Password = password });
                        var categoryGetNotesResponse = _storage.GetNotesByCategoryId(value.Id);
                        if (!categoryGetNotesResponse.IsSuccessful) {
                            MessageBox.Show("Incorrect password!");
                            continue;
                        }
                        CategoryNotes = new ObservableCollection<Note>(categoryGetNotesResponse.Response.Notes);
                    }
                } else {
                    CategoryNotes = new ObservableCollection<Note>(_storage.GetNotesByCategoryId(value.Id).Response.Notes);
                }

                OnPropertyChanged("SelectedCategory");
                CreateNoteButtonVisibility = Visibility.Visible;
                OnPropertyChanged("CreateNoteButtonVisibility");
                
                OnPropertyChanged("CategoryNotes");
                MainWindowState = MainWindowStates.NotesShowing;
            }
        }

        public MainWindowStates MainWindowState
        {
            get { return _mainWindowStates; }
            private set {
                _mainWindowStates = value;
                OnPropertyChanged("MainWindowState");
            }
        }

        private Command _editNoteCommand;
        public Command EditNoteCommand
        {
            get {
                return _editNoteCommand ??
                    (_editNoteCommand = new Command(obj => {
                        if (_selectedCategory == null) {
                            MessageBox.Show("Select category first!");
                            return;
                        }
                        if (string.IsNullOrEmpty(SavingNoteData.Name)) {
                            MessageBox.Show("Seriously? Note without name?");
                            return;
                        }

                        var note = new NoteData {
                            Id = SavingNoteData.Id,
                            CategoryId = _selectedCategory.Id,
                            Description = SavingNoteData.Description,
                            Name = SavingNoteData.Name
                        };

                        var noteSaveResponse = _storage.SaveNote(note);
                        if (!noteSaveResponse.IsSuccessful) {
                            MessageBox.Show(noteSaveResponse.ErrorMessage);
                            return;
                        }

                        if (note.Id == new Guid()) {
                            CategoryNotes.Add(noteSaveResponse.Response.Note);
                        } else {
                            var noteId = noteSaveResponse.Response.Note.Id;
                            CategoryNotes.Where(note => note.Id == noteId).ToList().ForEach(note => note = noteSaveResponse.Response.Note);
                        }

                        UpdateCategoryList();
                        GetBack();
                    })
                );
            }
        }

        private Command _createCategoryCommand;
        public Command CreateCategoryCommand
        {
            get {
                return _createCategoryCommand ??
                    (_createCategoryCommand = new Command(obj => {
                        var addCategoryWindow = new StandardKernel().Get<AddCategoryWindow>();

                        addCategoryWindow.ShowDialog();
                        var dialogResponse = addCategoryWindow.GetCloseData();
                        
                        if (dialogResponse.FormStatus == FormStatus.ClosedManually) {
                            return;
                        }

                        var category = new Category(dialogResponse.Category.Name, dialogResponse.Category.Password);

                        var categorySaveResponse = _storage.SaveCategory(category);

                        if(!categorySaveResponse.IsSuccessful) {
                            MessageBox.Show(categorySaveResponse.ErrorMessage);
                            return;
                        }

                        CategoriesCollection.Add(categorySaveResponse.Response.Category);
                        OnPropertyChanged("CategoriesCollection");
                    })
                );
            }
        }

        private Command _openCreateNoteCommand;
        public Command OpenCreateNoteCommand
        {
            get {
                return _openCreateNoteCommand ??
                    (_openCreateNoteCommand = new Command(obj => {
                        MainWindowState = MainWindowStates.NoteEditing;
                        CreateEditNoteText = "Create note";
                        SavingNoteData = new NoteEdit();

                        OnPropertyChanged("SavingNoteData");
                        OnPropertyChanged("CreateEditNoteText");
                    })
                );
            }
        }

        private Command _openEditNoteCommand;
        public Command OpenEditNoteCommand
        {
            get {
                return _openEditNoteCommand ??
                    (_openEditNoteCommand = new Command(obj => {
                        MainWindowState = MainWindowStates.NoteEditing;
                        CreateEditNoteText = "Edit note";
                        var note = (Note)obj;

                        SavingNoteData = new NoteEdit {
                            Id = note.Id,
                            Description = note.Description,
                            Name = note.Name
                        };
                        OnPropertyChanged("SavingNoteData");
                        OnPropertyChanged("CreateEditNoteText");
                    })
                );
            }
        }

        private Command _attachFileCommand;
        public Command AttachFileCommand
        {
            get {
                return _attachFileCommand ??
                    (_attachFileCommand = new Command(obj => {
                        var fileDialog = new OpenFileDialog();
                        fileDialog.InitialDirectory = "C:";
                        fileDialog.Multiselect = false;

                        if (fileDialog.ShowDialog() == true) {
                            var file = fileDialog.FileName;
                        }
                    })
                );
            }
        }

        private Command _getBackCommand;
        public Command GetBackCommand
        {
            get {
                return _getBackCommand ??
                    (_getBackCommand = new Command(obj => GetBack())
                );
            }
        }

        private Command _viewFullNoteCommand;
        public Command ViewFullNoteCommand
        {
            get {
                return _viewFullNoteCommand ??
                    (_viewFullNoteCommand = new Command(obj => {
                        MainWindowState = MainWindowStates.NoteViewing;
                        SelectedNote = (Note)obj;
                        OnPropertyChanged("SelectedNote");
                    })
                );
            }
        }
        
        private void UpdateCategoryList()
        {
            var data = CategoryNotes;
            CategoryNotes = null;
            OnPropertyChanged("CategoryNotes");
            CategoryNotes = data;
            OnPropertyChanged("CategoryNotes");

        }

        private void GetBack()
        {
            MainWindowState = MainWindowStates.NotesShowing;
            SavingNoteData = null;
            SelectedNote = null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
