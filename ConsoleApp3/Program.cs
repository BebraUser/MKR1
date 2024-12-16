using System;
using System.Collections.Generic;
using System.IO;

class TicketQueue
{
    static void Main()
    {
        // Читаємо вхідні дані
        string[] inputLines = File.ReadAllLines("INPUT.TXT");
        int N = int.Parse(inputLines[0]); // Кількість покупців
        
        // Масив для збереження мінімального часу обслуговування покупців
        int[] minTime = new int[N];
        
        for (int i = 0; i < N; i++)
        {
            // Зчитуємо трійку чисел Ai, Bi, Ci
            string[] times = inputLines[i + 1].Split();
            int A = int.Parse(times[0]);
            int B = int.Parse(times[1]);
            int C = int.Parse(times[2]);
            
            // Зберігаємо мінімальний час для обслуговування одного покупця
            minTime[i] = Math.Min(A, Math.Min(B, C));
        }

        // Використовуємо пріоритетну чергу для моделювання роботи каси
        PriorityQueue<int, int> queue = new PriorityQueue<int, int>();

        foreach (int time in minTime)
            queue.Enqueue(time, time); // Додаємо час у чергу

        int totalTime = 0; // Загальний час обслуговування
        while (queue.Count > 0)
        {
            // Беремо мінімальний час із черги
            totalTime += queue.Dequeue();
        }

        // Записуємо результат у файл OUTPUT.TXT
        File.WriteAllText("OUTPUT.TXT", totalTime.ToString());
    }
}
