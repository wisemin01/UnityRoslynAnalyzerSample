using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Program : MonoBehaviour
{
    void Update()
    {
        // 1. value enumerator. (X)
        List<int> numbers = new List<int>() { 1, 2, 3, 4, 5 };
        foreach (var num in numbers)
        {
            Debug.Log(num);
        }

        // 2. reference enumerator. (O)
        IReadOnlyList<int> numbers2 = numbers;
        foreach (var num2 in numbers2)
        {
            Debug.Log(num2);
        }

        // 3. array. (X)
        int[] numbers3 = numbers.ToArray();
        foreach (var num3 in numbers3)
        {
            Debug.Log(num3);
        }
    }
}
