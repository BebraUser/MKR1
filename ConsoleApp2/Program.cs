using System;
using System.Collections.Generic;
using System.IO;

class TrainSchedule
{
    static void Main()
    {
        // Зчитуємо вхідні дані з файлу
        string[] inputLines = File.ReadAllLines("INPUT.TXT");
        string[] firstLine = inputLines[0].Split();

        int stationCount = int.Parse(firstLine[0]); // кількість станцій (N)
        int routeCount = int.Parse(firstLine[1]);   // кількість маршрутів (M)

        // Граф для зберігання маршрутів
        List<(int to, int time)>[] routes = new List<(int, int)>[stationCount + 1];
        for (int i = 0; i <= stationCount; i++)
            routes[i] = new List<(int, int)>();

        // Читаємо маршрути електричок
        for (int i = 1; i <= routeCount; i++)
        {
            string[] routeInfo = inputLines[i].Split();
            int stopCount = int.Parse(routeInfo[0]); // кількість зупинок на маршруті

            for (int j = 1; j < stopCount; j++)
            {
                int fromStation = int.Parse(routeInfo[j]);
                int toStation = int.Parse(routeInfo[j + 1]);
                int travelTime = int.Parse(routeInfo[stopCount + j - 1]);

                // Додаємо напрямок руху у граф
                routes[fromStation].Add((toStation, travelTime));
            }
        }

        // Знаходимо найкоротший шлях від станції 1 до станції N
        int shortestTime = FindShortestPath(routes, stationCount);

        // Запис результату у вихідний файл
        File.WriteAllText("OUTPUT.TXT", shortestTime.ToString());
    }

    static int FindShortestPath(List<(int to, int time)>[] graph, int stationCount)
    {
        const int INF = int.MaxValue;

        // Масив для збереження мінімальних відстаней
        int[] distance = new int[stationCount + 1];
        for (int i = 1; i <= stationCount; i++)
            distance[i] = INF;

        distance[1] = 0; // Стартуємо зі станції 1

        // Пріоритетна черга для реалізації алгоритму Дейкстри
        var queue = new SortedSet<(int dist, int station)>();
        queue.Add((0, 1));

        while (queue.Count > 0)
        {
            var (currentDist, currentStation) = queue.Min;
            queue.Remove(queue.Min);

            // Перебираємо всі маршрути зі станції
            foreach (var (nextStation, time) in graph[currentStation])
            {
                int newDist = currentDist + time;

                if (newDist < distance[nextStation])
                {
                    queue.Remove((distance[nextStation], nextStation));
                    distance[nextStation] = newDist;
                    queue.Add((newDist, nextStation));
                }
            }
        }

        // Якщо до останньої станції не дісталися
        return distance[stationCount] == INF ? -1 : distance[stationCount];
    }
}
