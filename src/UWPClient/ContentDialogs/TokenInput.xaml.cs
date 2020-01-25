using System;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWPClient.ContentDialogs
{
    public sealed partial class TokenInput : ContentDialog
    {
        public Action<TokenInput, string> Callback { get; set; }
        public bool WaitForMyCommand { get; set; }
        public string Value { get => valueTxt.Text; set => valueTxt.Text = value; }
        public TokenInput()
        {
            this.InitializeComponent();

            //valueTxt.Text = EnvironmentVariables.Token ?? "";
        }

        public TokenInput(string title)
        {
            this.InitializeComponent();
            Title = title;
        }



        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (busyProgress.IsActive) return;

            Callback?.Invoke(this, valueTxt.Text);
            args.Cancel = busyProgress.IsActive = WaitForMyCommand;
            valueTxt.IsEnabled = !WaitForMyCommand;
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // Callback?.Invoke(valueTxt.Text);
        }

        public void Close()
        {
            Hide();
        }


    }
}
