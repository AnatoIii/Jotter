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
using System.Threading.Tasks;
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
        public ObservableCollection<Category> CategoriesCollection { get; private set;  }

        public MainViewModel(IStorage storage)
        {
            _storage = storage;

            MainWindowState = MainWindowStates.NotesShowing;
            SavingNoteData = new NoteEdit();

            LoadCategories();

            CategoryNotes = new ObservableCollection<Note>();

            CreateNoteButtonVisibility = Visibility.Hidden;
        }

        private async Task LoadCategories()
        {
            var categoriesReponse = await _storage.GetCategories();
            CategoriesCollection = new ObservableCollection<Category>(categoriesReponse.Response.Categories);
            OnPropertyChanged("CategoriesCollection");
        }

        private Category _selectedCategory;
        public Category SelectedCategory {
            get {
                return _selectedCategory;
            }
            set {
                _selectedCategory = value;
                UpdateSelectedCategoryNotes(value.Id);
            }
        }

        private File _selectedFile;
        public File SelectedFile
        {
            get {
                return _selectedFile;
            }
            set {
                _selectedFile = value;

                OpenFileCommand.Execute(value);
            }
        }

        private Command _openFileCommand;
        public Command OpenFileCommand
        {
            get {
                return _openFileCommand ??
                    (_openFileCommand = new Command(async obj => {
                        var response = await _storage.OpenFile(((File)obj).Id);
                        if (!response.IsSuccessful) {
                            MessageBox.Show(response.ErrorMessage);
                            return;
                        }
                    })
                );
            }
        }

        private async Task UpdateSelectedCategoryNotes(Guid categoryId)
        {
            if (_selectedCategory.IsPrivate) {
                var viewModel = new EnterCategoryPasswordWindowViewModel(_selectedCategory.Name);
                var window = new EnterCategoryPasswordWindow(viewModel);
                bool confirmed = false;
                while (!confirmed) {
                    window.ShowDialog();
                    var password = window.GetCategoryPassword();
                    var categoryGetNotesResponse = await _storage.GetNotesByCategory(categoryId, password);
                    if (!categoryGetNotesResponse.IsSuccessful) {
                        MessageBox.Show(categoryGetNotesResponse.ErrorMessage);
                        continue;
                    }
                    CategoryNotes = new ObservableCollection<Note>(categoryGetNotesResponse.Response.Notes);
                    confirmed = true;
                    window.Close();
                }
            } else {
                var notesResponse = await _storage.GetNotesByCategory(categoryId);
                CategoryNotes = new ObservableCollection<Note>(notesResponse.Response.Notes);
            }

            OnPropertyChanged("SelectedCategory");
            CreateNoteButtonVisibility = Visibility.Visible;
            OnPropertyChanged("CreateNoteButtonVisibility");

            OnPropertyChanged("CategoryNotes");
            MainWindowState = MainWindowStates.NotesShowing;
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
                    (_editNoteCommand = new Command(async obj => {
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

                        var noteSaveResponse = await _storage.SaveNote(note);
                        if (!noteSaveResponse.IsSuccessful) {
                            MessageBox.Show(noteSaveResponse.ErrorMessage);
                            return;
                        }

                        if (note.Id == new Guid()) {
                            CategoryNotes.Add(noteSaveResponse.Response.Note);
                        } else {
                            var noteId = noteSaveResponse.Response.Note.Id;
                            CategoryNotes = new ObservableCollection<Note>(CategoryNotes.Select(note => (note.Id == noteId ? note = noteSaveResponse.Response.Note : note)));
                            var data = CategoryNotes.Where(note => note.Id == noteId).ToList();
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
                    (_createCategoryCommand = new Command(async obj => {
                        var addCategoryWindow = new StandardKernel().Get<AddCategoryWindow>();

                        addCategoryWindow.ShowDialog();
                        var dialogResponse = addCategoryWindow.GetCloseData();
                        
                        if (dialogResponse.FormStatus == FormStatus.ClosedManually) {
                            return;
                        }

                        var categorySaveResponse = await _storage.SaveCategory(new Category { 
                            Name = dialogResponse.Category.Name,
                            Password = dialogResponse.Category.Password 
                        });

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
                    (_attachFileCommand = new Command(async obj => {
                        var fileDialog = new OpenFileDialog();
                        fileDialog.InitialDirectory = "C:";
                        fileDialog.Multiselect = false;

                        if (fileDialog.ShowDialog() == true) {
                            var response = await _storage.SaveFile(fileDialog.FileName, ((Note)obj).Id);
                            if (!response.IsSuccessful) {
                                MessageBox.Show(response.ErrorMessage);
                                return;
                            }

                            MessageBox.Show("File sucessfully saved :)");
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
                    (_viewFullNoteCommand = new Command(async obj => {
                        MainWindowState = MainWindowStates.NoteViewing;
                        var response = await _storage.GetNoteById(((Note)obj).Id);
                        if (!response.IsSuccessful) {
                            MessageBox.Show(response.ErrorMessage);
                            return;
                        }
                        SelectedNote = response.Response.Note;
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
