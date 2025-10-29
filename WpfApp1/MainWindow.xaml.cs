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
     
        CurrentText.Text  = _current;
    }

    private void SetCodeBtn_Click(object sender, RoutedEventArgs e)
    {
  
        var dlg = new OpenFileDialog
        {
            Title = "Choose a file to use as Original Code (optional)",
            Filter = "All files (*.*)|*.*"
        };

        if (dlg.ShowDialog(this) == true)
        {
            _baseline = File.ReadAllText(dlg.FileName);
 
            if (string.IsNullOrWhiteSpace(_current))
            {
                _current = _baseline;
                CurrentText.Text = _current;
                CurrentText.IsReadOnly = false;
            }
        }
       
    }



    private void BuildPromptBtn_Click(object sender, RoutedEventArgs e)
    {
        // _baseline = BaselineText.Text;
        _current  = CurrentText.Text;
        
        var diff = DiffUtils.MakeUnifiedDiff("file.txt", _baseline, _current, context: 3);
        var prompt = DiffUtils.BuildPrompt(diffText: diff, initCode: _current, focus: "readability, performance");
        _baseline = _current;
        PromptText.Text = prompt;

        
    }

    private void CopyPrompt_Click(object sender, RoutedEventArgs e)
    {
        if (!string.IsNullOrEmpty(PromptText.Text))
        {
            Clipboard.SetText(PromptText.Text);
        }
    }
}
