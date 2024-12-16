using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        // Читання вхідних даних
        var input = File.ReadAllLines("INPUT.TXT");
        int N = int.Parse(input[0]); // кількість запитів
        var queries = input.Skip(1).Select(int.Parse).ToArray();

        // Генеруємо програшні пари
        var losingPairs = GenerateLosingPairs();

        // Відповідь на кожен запит
        var output = new List<string>();
        foreach (int k in queries)
        {
            output.Add($"{losingPairs[k].Item1} {losingPairs[k].Item2}");
        }

        // Запис результатів у вихідний файл
        File.WriteAllLines("OUTPUT.TXT", output);
    }

    static List<(int, int)> GenerateLosingPairs()
    {
        const int MAX = 100; // Максимальне значення для a та b
        var grundy = new int[MAX + 1, MAX + 1];
        var losingPairs = new List<(int, int)>();

        // Заповнення грюндівських чисел та пошук програшних пар
        for (int a = 0; a <= MAX; a++)
        {
            for (int b = a; b <= MAX; b++)
            {
                if (grundy[a, b] == 0)
                {
                    losingPairs.Add((a, b)); // Програшна позиція

                    // Оновлюємо значення для наступних позицій
                    for (int i = 1; a + i <= MAX; i++)
                        grundy[a + i, b] ^= 1;
                    for (int i = 1; b + i <= MAX; i++)
                        grundy[a, b + i] ^= 1;
                }
            }
        }
        return losingPairs;
    }
}