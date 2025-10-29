using System.Text;
using DiffPlex;
using DiffPlex.DiffBuilder;
using DiffPlex.DiffBuilder.Model;

namespace WpfApp1;

public static class DiffUtils
{
    private static string Norm(string s) => (s ?? string.Empty).Replace("\r\n", "\n");

  
    public static string MakeUnifiedDiff(string filename, string before, string after, int context = 3)
    {
        var inlineBuilder = new InlineDiffBuilder(new Differ());
        var inline = inlineBuilder.BuildDiffModel(before, after);
        var str = new StringBuilder();
        bool[] tab = new bool[inline.Lines.Count+5];
        int counter = 0;
        foreach (var l in inline.Lines)
        {
            if (l.Type != ChangeType.Unchanged)
            {
                tab[counter] = true;
                if(counter!=0) tab[counter - 1] = true;
                
                tab[counter + 1] = true;
            }
            counter++;
        }
        counter = 0;
        bool write = false;
        foreach (var l in inline.Lines)
        {
            if (tab[counter] == true)
            {
                var sign = l.Type switch
                {
                    ChangeType.Inserted => "+ ",
                    ChangeType.Deleted => "- ",
                    ChangeType.Unchanged => "  ",
                    ChangeType.Modified => "~ ",
                    _ => "  "
                };
                str.Append(sign);
                str.Append(l.Text);
                // str.Append("\n");
                write = true;
            }

            if (write == true)
            {
                write = false;
                str.Append('\n');
            }

            counter++;
        }

        return str.ToString();
    }

    public static string BuildPrompt(string diffText, string initCode, string focus)
    {
        return
            $@"You are given changes to a given codebase. 

Changes are(lines with + are added lines with - are deleted:
{diffText}
Focus on {focus}.
Give answers in the following format:

Suggestions on improving the code's {focus}:
(2-3 sentences with explanations)
Improved Code:
(line snippets of changes)   answers with your suggestions on how to improve the code";
    }
}