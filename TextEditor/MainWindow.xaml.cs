using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace TextEditor;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        string init_txt = "";

        try {
            init_txt = File.ReadAllText("data/lorem.txt");
        } catch { }

        main_paragraph.Inlines.Add(init_txt);
    }

    private void searchText()
    {
        string pattern = search_tb.Text;
        string txt = getText(main_paragraph);

        List<int> indices = StringSearch.NaiveSearch(txt, pattern);
        info_text.Text = "'" + pattern + "' ocurrances: " + indices.Count;

        setBoldText(main_paragraph, indices, pattern.Length);
    }

    private static void setBoldText(Paragraph para, List<int> indices, int len)
    {
        string txt = getText(para);

        para.Inlines.Clear();

        for (int i = 0; i < txt.Length; i++)
        {
            bool charFound = false;
            
            foreach (int index in indices)
            {
                if (i >= index && i < index + len)
                {
                    para.Inlines.Add(new Bold(new Run(txt[i].ToString())));
                    charFound = true;
                    break;
                }
            }

            if (!charFound)
                para.Inlines.Add(txt[i].ToString());
        }
    }

    private static string getText(Paragraph para)
    {
        return new TextRange(para.ContentStart, para.ContentEnd)
              .Text
              .Replace("\n", "");
    }

    // Event handling.
    private void search_tb_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Return)
            searchText();
    }

    private void search_button_Click(object sender, RoutedEventArgs e)
    {
        searchText();
    }

    private void main_rtb_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            TextPointer newPointer = main_rtb.Selection.Start.InsertLineBreak();
            main_rtb.Selection.Select(newPointer, newPointer);
            e.Handled = true;
        }
    }
}
