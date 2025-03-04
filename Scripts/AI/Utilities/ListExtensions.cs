using System.Collections.Generic;
using UnityEngine;

public static class ListExtensions
{
    // Shuffle extension method using the Durstenfeld implementation of
    // the Fisher-Yates in-place sorting algorithm. 
    public static List<T> Shuffle<T>(this List<T> list)
    {
        List<T> shuffled = new(list); // Create a copy of the original list
        int n = shuffled.Count;

        // Fisher-Yates shuffle algorithm
        for (int i = n - 1; i > 0; i--)
        {
            int k = UnityEngine.Random.Range(0, i + 1); // Choose a random index between 0 and i
            (shuffled[k], shuffled[i]) = (shuffled[i], shuffled[k]);
        }

        return shuffled; // Return the shuffled list
    }
}
