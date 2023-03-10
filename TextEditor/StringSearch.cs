using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace TextEditor;

internal class StringSearch
{
    public enum AlgorithmType
    {
        Naive,
        RabinKarp,
        BoyerMoore,
        Regex
    }

    public static (double, List<int>) SearchString (
        string txt,
        string pattern,
        AlgorithmType algorithmType = AlgorithmType.Naive)
    {
        List<int> output = new List<int>();

        if (pattern.Length > txt.Length || pattern.Length < 1)
            return (0.0, output);

        // Time the execution time.
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();

        switch (algorithmType)
        {
            case AlgorithmType.Naive:
                output = naiveSearch(txt, pattern);
                break;
            case AlgorithmType.RabinKarp:
                output = rabinKarpSearch(txt, pattern);
                break;
            case AlgorithmType.BoyerMoore:
                output = boyerMooreSearch(txt, pattern);
                break;
            case AlgorithmType.Regex:
                output = regexSearch(txt, pattern);
                break;
            default:
                break;
        }

        stopWatch.Stop();
        return (stopWatch.Elapsed.TotalMilliseconds, output);
    }

    // Naive String Search Algorithm.
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

    // Rabin-Karp String Search Algorithm.
    private static List<int> rabinKarpSearch(string txt, string pattern, int q = 101)
    {
        var indices = new List<int>();

        int pat_len = pattern.Length;
        int txt_len = txt.Length;
        int pat_hash = 0;
        int txt_hash = 0;
        int hash = 1;
        int char_size = sizeof(char) * 256;

        for (int i = 0; i < pat_len - 1; i++)
            hash = (hash * char_size) % q;

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
                txt_hash = (char_size * (txt_hash - txt[i] * hash) + txt[i + pat_len]) % q;

                if (txt_hash < 0)
                    txt_hash = (txt_hash + q);
            }
        }

        return indices;
    }

    // Boyer-Moore String Search Algorithm.
    public static List<int> boyerMooreSearch(string txt, string pattern)
    {
        var indices = new List<int>();

        int pat_len = pattern.Length;
        int txt_len = txt.Length;

        int[] badChar = new int[256];

        badCharHeuristic(pattern, pat_len, ref badChar);

        int i = 0;
        while (i <= (txt_len - pat_len))
        {
            int j = pat_len - 1;

            while (j >= 0 && pattern[j] == txt[i + j])
                --j;

            if (j < 0)
            {
                indices.Add(i);
                i += (i + pat_len < txt_len) ? pat_len - badChar[txt[i + pat_len]] : 1;
            }
            else
            {
                i += Math.Max(1, j - badChar[txt[i + j]]);
            }
        }

        return indices;
    }

    private static void badCharHeuristic(string txt, int size, ref int[] badChar)
    {
        for (int i = 0; i < 256; i++)
            badChar[i] = -1;

        for (int i = 0; i < size; i++)
            badChar[(int)txt[i]] = i;
    }

    // Regex.
    private static List<int> regexSearch(string txt, string pattern)
    {
        var indices = new List<int>();
        var regex = new Regex(pattern);

        foreach (Match match in regex.Matches(txt))
            indices.Add(match.Index);

        return indices;
    }
}
