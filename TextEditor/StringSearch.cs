using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEditor;

internal class StringSearch
{
    public static List<int> NaiveSearch(string txt, string pattern)
    {
        var indices = new List<int>();
        
        int txt_len = txt.Length;
        int pat_len = pattern.Length;

        for (int i = 0; i <= txt_len - pat_len; i++)
        {
            int j;

            for (j = 0; j < pat_len; j++)
                if (txt[i + j] != pattern[j])
                    break;

            if (j == pat_len)
                indices.Add(i);
        }

        return indices;
    }
}
