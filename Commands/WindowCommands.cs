using System.Windows.Input;

namespace CS_WPF_Lab9_Rental_Housing.Commands
{
    static public class WindowCommands
    {
        public static RoutedCommand Exit { get; set; }

        static WindowCommands()
        {
            Exit = new RoutedCommand("Exit", typeof(MainWindow));
            KeyGesture keyEscape = new(Key.Escape);
            Exit.InputGestures.Add(keyEscape);
        }
    }
}
