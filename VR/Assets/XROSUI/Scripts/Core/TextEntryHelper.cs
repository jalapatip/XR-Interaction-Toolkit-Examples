using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextEntryHelper
{
    //https://www.dotnetperls.com/levenshtein
    public static int ComputeLevenshteinDistance(string s, string t)
    {
        int n = s.Length;
        int m = t.Length;
        int[,] d = new int[n + 1, m + 1];

        // Verify arguments.
        if (n == 0)
        {
            return m;
        }

        if (m == 0)
        {
            return n;
        }

        // Initialize arrays.
        for (int i = 0; i <= n; d[i, 0] = i++)
        {
        }

        for (int j = 0; j <= m; d[0, j] = j++)
        {
        }

        // Begin looping.
        for (int i = 1; i <= n; i++)
        {
            for (int j = 1; j <= m; j++)
            {
                // Compute cost.
                int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                d[i, j] = Math.Min(
                    Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                    d[i - 1, j - 1] + cost);
            }
        }
        // Return cost.
        return d[n, m];
    }

    public static float ComputeErrorRate(string s, string t)
    {
        //Error rate (%) was computed by calculating the minimum string distance (MSD) between the presented and
        //transcribed text and dividing it by the larger number of characters, formally:
        //100*MST(P, T) / max(|P|, |T|)
        //where P and T denote the presented and transcribed text. MSD is calculated using Levenshtein’s algorithm [27].
        
        return (100 * ComputeLevenshteinDistance(s, t)) / Math.Max(s.Length, t.Length);
    }

    public static float ComputeWordsPerMinute(string t, float s)
    {
        //Words per minute (WPM) was computed by dividing the number of transcribed words (any 5-character string) by the
        //time it takes to transcribe the text, formally:
        //WPM = |T|-1 / S * 60*(1/5)
        
        //where S is the time (in seconds) from the first to the last key press and |T| is the number of characters in the transcribed
        //text.
        
        float wpm = 0;  
        wpm = (t.Length - 1) / s * 60 * (1 / 5f);
        
        return wpm;
    }
}
