using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Local_Password_Generator
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Title = "Password Generator";
            AppWindow.Resize(new Windows.Graphics.SizeInt32(600,800));
        }

        // Handle the button click event for password generation.
        // Displays the generated password in a non-editable text block.
        private void button_generatePassword(object sender, RoutedEventArgs e)
        {
            // Call password generation method and display result

            // Display password
            generatePassword.Visibility = Visibility.Collapsed;
            var pwTextBlock = (TextBlock)rootPanel.FindName("pwTextBlock"); // WinUI can't find the object in XAML so I have to do this
            if (pwTextBlock != null)
            {
                pwTextBlock.Text = "TestPassword123";
                pwTextBlock.HorizontalTextAlignment = TextAlignment.Center;
                pwTextBlock.Visibility = Visibility.Visible;
            }

        }
    }
}
