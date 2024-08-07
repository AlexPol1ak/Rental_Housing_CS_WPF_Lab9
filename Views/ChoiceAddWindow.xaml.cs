using CS_WPF_Lab9_Rental_Housing.Commands;
using System.Windows;
using System.Windows.Input;

namespace CS_WPF_Lab9_Rental_Housing.Views
{
    /// <summary>
    /// The selection window.
    /// </summary>
    public partial class ChoiceAddWindow : Window
    {
        public ChoiceAddWindow()
        {
            InitializeComponent();
            DataContext = this;
        }
        // Choice
        public string? Choice { get; private set; } = null;

        private ICommand _buttonCommand;
        public ICommand ButtonCommand => _buttonCommand ??= new RelayCommand
            (
                (obj) =>
                {
                    if (obj != null) Choice = (string)obj;
                    this.DialogResult = true;
                    this.Close();
                }
            );


    }
}
