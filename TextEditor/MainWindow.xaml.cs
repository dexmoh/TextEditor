using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using static TextEditor.StringSearch;

namespace TextEditor;

public partial class MainWindow : Window
{
    private AlgorithmType algorithmType;

    public MainWindow()
    {
        algorithmType = AlgorithmType.Naive;

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

        List<int> indices = SearchString(txt, pattern, algorithmType);
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

    private void search_cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ComboBoxItem selected = (ComboBoxItem) search_cb.SelectedItem;

        switch (selected.Content.ToString())
        {
            case "Naive":
                algorithmType = AlgorithmType.Naive;
                break;
            case "Rabin-Karp":
                algorithmType = AlgorithmType.RabinKarp;
                break;
            default:
                algorithmType = AlgorithmType.Naive;
                break;
        }
    }
}
