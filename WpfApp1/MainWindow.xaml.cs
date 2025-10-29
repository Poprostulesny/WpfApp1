using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace WpfApp1;

public partial class MainWindow : Window
{
    private string _baseline = string.Empty;
    private string _current = string.Empty;

    public MainWindow()
    {
        InitializeComponent();
        // BaselineText.Text = _baseline;
        CurrentText.Text  = _current;
    }

    private void SetCodeBtn_Click(object sender, RoutedEventArgs e)
    {
        // Simple “modal”: let user paste baseline via a file open or just leave as-is.
        var dlg = new OpenFileDialog
        {
            Title = "Choose a file to use as Original Code (optional)",
            Filter = "All files (*.*)|*.*"
        };

        if (dlg.ShowDialog(this) == true)
        {
            _baseline = File.ReadAllText(dlg.FileName);
            // BaselineText.Text = _baseline;

            // If current is empty, mirror baseline initially
            if (string.IsNullOrWhiteSpace(_current))
            {
                _current = _baseline;
                CurrentText.Text = _current;
                CurrentText.IsReadOnly = false;
            }
        }
       
    }

    // private void EditCodeBtn_Click(object sender, RoutedEventArgs e)
    // {
    //     // Editing is just the right-side box; sync fields now in case the user typed on the left.
    //     // _baseline = BaselineText.Text;
    //     _current  = CurrentText.Text;
    //     CurrentText.Focus();
    // }

    private void BuildPromptBtn_Click(object sender, RoutedEventArgs e)
    {
        // _baseline = BaselineText.Text;
        _current  = CurrentText.Text;

        var diff = DiffUtils.MakeUnifiedDiff("file.txt", _baseline, _current, context: 3);
        var prompt = DiffUtils.BuildPrompt(diffText: diff, initCode: _current, focus: "readability, performance");

        PromptText.Text = prompt;

        // Optional: reset like your React flow did
        // _baseline = string.Empty; _current = string.Empty;
        // BaselineText.Text = ""; CurrentText.Text = "";
    }

    private void CopyPrompt_Click(object sender, RoutedEventArgs e)
    {
        if (!string.IsNullOrEmpty(PromptText.Text))
        {
            Clipboard.SetText(PromptText.Text);
        }
    }
}
