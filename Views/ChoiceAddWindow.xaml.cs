using CS_WPF_Lab9_Rental_Housing.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CS_WPF_Lab9_Rental_Housing.Views
{
    /// <summary>
    /// Логика взаимодействия для ChoiceAddWindow.xaml
    /// </summary>
    public partial class ChoiceAddWindow : Window
    {       
        public ChoiceAddWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        public string? Choice { get; private set; } = null;

        private ICommand _buttonCommand;
        public ICommand ButtonCommand => _buttonCommand ??= new RelayCommand
            (
                (obj)=>
                {
                    if(obj != null) Choice = (string)obj;
                    this.DialogResult = true;
                    this.Close();
                }
            );


    }
}
