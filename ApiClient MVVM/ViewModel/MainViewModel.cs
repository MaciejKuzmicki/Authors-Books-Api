using ApiClient.Model;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiClient.Commands;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.ComponentModel;
using System.Windows.Controls;

namespace ApiClient.ViewModel
{
    public partial class MainViewModel : GalaSoft.MvvmLight.ObservableObject, INotifyPropertyChanged
    {
        ApiService _ApiService;
        public ObservableCollection<Book> Books { get; }

        public ObservableCollection<Author> Authors { get; }

        public MainViewModel(ApiService apiService)
        {
            _ApiService = apiService;
            Books = new ObservableCollection<Book>();
            Authors = new ObservableCollection<Author>();

            
        }
        private Book _selectedBook = new();
        public Book SelectedBook
        {
            get => _selectedBook;
            set
            {
                if (_selectedBook != value)
                {
                    _selectedBook = value;
                    RaisePropertyChanged(nameof(SelectedBook));
                }
            }
        }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public string Description { get; set; }
        public string AuthorSurname { get; set; }
        public Book Book = new();

        [RelayCommand]
        public async Task LoadBooks()
        {
            var response = await _ApiService.loadBooks();
            Books.Clear();
                foreach(var book in response)
                {
                    Books.Add(book);
                }
        }

        [RelayCommand]
        public async Task LoadBook()
        {
            var response = await _ApiService.loadBook(SelectedBook.Id);
            Book = response.Data;
        }

        [RelayCommand]
        public async Task DeleteBook()
        {
            var response = await _ApiService.deleteBook(SelectedBook.Id);
        }
        [RelayCommand]
        public async Task AddBook()
        {
            var response = await _ApiService.addBook(Title, AuthorName, AuthorSurname, Description);
        }
    }
}
