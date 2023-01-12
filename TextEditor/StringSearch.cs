using System;
using System.Collections.Generic;
using System.Windows;

namespace TextEditor;

internal class StringSearch
{
    public enum AlgorithmType
    {
        Naive,
        RabinKarp
    }

    public static List<int> SearchString (
        string txt,
        string pattern,
        AlgorithmType algorithmType = AlgorithmType.Naive)
    {
        List<int> output;

        switch (algorithmType)
        {
            case AlgorithmType.Naive:
                output = naiveSearch(txt, pattern);
                break;
            case AlgorithmType.RabinKarp:
                output = rabinKarpSearch(txt, pattern);
                break;
            default:
                output = new List<int>();
                break;
        }

        return output;
    }

    private static List<int> naiveSearch(string txt, string pattern)
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

    private static List<int> rabinKarpSearch(string txt, string pattern, int q = 101)
    {
        var indices = new List<int>();

        int pat_len = pattern.Length;
        int txt_len = txt.Length;
        int pat_hash = 0;
        int txt_hash = 0;
        int h = 1;
        int char_size = sizeof(char) * 256;

        for (int i = 0; i < pat_len - 1; i++)
            h = (h * char_size) % q;

        for (int i = 0; i < pat_len; i++)
        {
            pat_hash = (char_size * pat_hash + pattern[i]) % q;
            txt_hash = (char_size * txt_hash + txt[i]) % q;
        }

        for (int i = 0; i <= txt_len - pat_len; i++)
        {
            if (pat_hash == txt_hash)
            {
                int j;
                for (j = 0; j < pat_len; j++)
                    if (txt[i + j] != pattern[j])
                        break;

                if (j == pat_len)
                    indices.Add(i);
            }

            if (i < txt_len - pat_len)
            {
                txt_hash = (char_size * (txt_hash - txt[i] * h) + txt[i + pat_len]) % q;

                if (txt_hash < 0)
                    txt_hash = (txt_hash + q);
            }
        }

        return indices;
    }
}
