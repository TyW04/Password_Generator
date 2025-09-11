using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography.X509Certificates;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Local_Password_Generator
{

    public partial class Rust {
        [DllImport("passwordgen.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr generate_password(byte length, bool includeSymbols, bool includeNumbers, bool uppercaseOnly, bool lowercaseOnly);
        [DllImport("passwordgen.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void free_c_string(IntPtr password);
    }
    
    public sealed partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            Title = "Password Generator";
            AppWindow.Resize(new Windows.Graphics.SizeInt32(400, 600));
            
        }

        // Handle the button click event for password generation.
        // Displays the generated password in a non-editable text block.
        private void button_generatePassword(object sender, RoutedEventArgs e)
        {

            // Get password
            
            IntPtr passwordPtr = Rust.generate_password((byte)passwordLengthSlider.Value,
                                                   getIncludeSymbolsCheck(), 
                                                   getIncludeNumbersCheck(),
                                                   getUppercaseOnlyCheck(),
                                                   getLowercaseOnlyCheck()); // Get pointer to C string from Rust
            
            String password = Marshal.PtrToStringAnsi(passwordPtr) ?? String.Empty; // If null, return empty string
            Rust.free_c_string(passwordPtr); // Pass back to Rust and deallocate memory
            
            // Display password
            if (pwTextBlock != null)
            {
                //pwTextBlock.Text = "";
                //pwTextBlock.VerticalAlignment = VerticalAlignment.Bottom
                pwTextBlock.Visibility = Visibility.Visible;
            }

        }

        // Handles the button click event for copying the generated password to the user's clipboard
        private void button_copyPasswordClipboard(object sender, RoutedEventArgs e)
        {
            DataPackage dataPackage = new();
            dataPackage.RequestedOperation = DataPackageOperation.Copy;
            dataPackage.SetText(pwTextBlock.Text);
            Clipboard.SetContent(dataPackage);
        }

        // Update the length displayed on the slider when the slider's value changes.
        private void lengthSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            string currentPwLength = String.Format("Length: {0}", e.NewValue);
            if (currentLength != null)
            {
                currentLength.Text = currentPwLength;
            }
                
        }

        // Returns state of "Include Symbols" checkbox for password generation settings.
        private bool getIncludeSymbolsCheck()
        {
            if (symbolsCheckbox.IsChecked == true)
            {
                return false;
            }
            return true;
        }

        // Returns state of "Include Numbers" checkbox for password generation settings.
        private bool getIncludeNumbersCheck()
        {
            if (numbersCheckbox.IsChecked == true)
            {
                return false;
            }
            return true;
        }

        // Returns state of "Uppercase Only" checkbox for password generation settings.
        private bool getUppercaseOnlyCheck()
        {
            if (uppercaseOnlyCheckbox.IsChecked == true)
            {
                return true;
            }
            return false;
        }

        // Returns state of "Lowercase Only" checkbox for password generation settings.
        private bool getLowercaseOnlyCheck()
        {
            if (lowercaseOnlyCheckbox.IsChecked == true)
            {
                return true;
            }
            return false;
        }

    }
}
