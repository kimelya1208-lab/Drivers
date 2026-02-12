using System;
using System.Collections.Generic;
using System.Linq;
namespace DriverMatching
{
    public class Driver
    {
        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public Driver(int id, int x, int y)
        {
            Id = id;
            X = x;
            Y = y;
        }
    }

    public class Order
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Order(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
    public static class DriverMatcher
    {
        private static int DistanceSquared(Driver driver, Order order)
        {
            int dx = driver.X - order.X;
            int dy = driver.Y - order.Y;
            int distSquared = dx * dx + dy * dy;
            return distSquared;
        }
        public static List<Driver> FindNearestDriversNaive(List<Driver> drivers, Order order, int k)
        {
            var distances = new List<int>();
            var sameOrderDrivers = new List<Driver>();

            for (int i = 0; i < drivers.Count; i++)
            {
                Driver currentDriver = drivers[i];
                int dist = DistanceSquared(currentDriver, order);

                distances.Add(dist);
                sameOrderDrivers.Add(currentDriver);
            }

            for (int i = 0; i < distances.Count - 1; i++)
            {
                for (int j = 0; j < distances.Count - 1 - i; j++)
                {
                    if (distances[j] > distances[j + 1] ||
                        (distances[j] == distances[j + 1] &&
                        sameOrderDrivers[j].Id > sameOrderDrivers[j + 1].Id))
                    {
                        int tempDist = distances[j];
                        distances[j] = distances[j + 1];
                        distances[j + 1] = tempDist;

                        Driver tempDriver = sameOrderDrivers[j];
                        sameOrderDrivers[j] = sameOrderDrivers[j + 1];
                        sameOrderDrivers[j + 1] = tempDriver;
                    }
                }
            }

            var result = new List<Driver>();

            int countToTake = k;
            if (sameOrderDrivers.Count < k)
            {
                countToTake = sameOrderDrivers.Count;
            }

            for (int i = 0; i < countToTake; i++)
            {
                result.Add(sameOrderDrivers[i]);
            }

            return result;
        }
        public static List<Driver> FindNearestDriversTopK(List<Driver> drivers, Order order, int k)
        {
            var bestDrivers = new List<Driver>();
            var bestDistances = new List<int>();

            for (int i = 0; i < drivers.Count; i++)
            {
                Driver currentDriver = drivers[i];
                int currentDist = DistanceSquared(currentDriver, order);

                if (bestDrivers.Count < k)
                {
                    bestDrivers.Add(currentDriver);
                    bestDistances.Add(currentDist);

                    int index = bestDrivers.Count - 1;
                    while (index > 0 && bestDistances[index] < bestDistances[index - 1])
                    {
                        int tempDist = bestDistances[index];
                        bestDistances[index] = bestDistances[index - 1];
                        bestDistances[index - 1] = tempDist;

                        Driver tempDriver = bestDrivers[index];
                        bestDrivers[index] = bestDrivers[index - 1];
                        bestDrivers[index - 1] = tempDriver;

                        index--;
                    }
                }
                else
                {
                    int worstIndex = k - 1;
                    if (currentDist >= bestDistances[worstIndex])
                    {
                        continue;
                    }

                    bestDrivers[worstIndex] = currentDriver;
                    bestDistances[worstIndex] = currentDist;

                    int index = worstIndex;
                    while (index > 0 && bestDistances[index] < bestDistances[index - 1])
                    {
                        int tempDist = bestDistances[index];
                        bestDistances[index] = bestDistances[index - 1];
                        bestDistances[index - 1] = tempDist;

                        Driver tempDriver = bestDrivers[index];
                        bestDrivers[index] = bestDrivers[index - 1];
                        bestDrivers[index - 1] = tempDriver;

                        index--;
                    }
                }
            }

            return bestDrivers;
        }
        private class HeapItem
        {
            public Driver Driver;
            public int Distance;

            public HeapItem(Driver driver, int distance)
            {
                Driver = driver;
                Distance = distance;
            }
        }

        private class MaxHeap
        {
            private List<HeapItem> _items = new List<HeapItem>();

            public int Count
            {
                get { return _items.Count; }
            }

            public void Add(HeapItem item)
            {
                _items.Add(item);
                SiftUp(_items.Count - 1);
            }

            public HeapItem PeekMax()
            {
                if (_items.Count == 0)
                    return null;
                return _items[0];
            }

            public HeapItem PopMax()
            {
                if (_items.Count == 0)
                    return null;

                HeapItem max = _items[0];

                int lastIndex = _items.Count - 1;
                _items[0] = _items[lastIndex];
                _items.RemoveAt(lastIndex);

                SiftDown(0);

                return max;
            }

            private bool Greater(HeapItem a, HeapItem b)
            {
                if (a.Distance > b.Distance) return true;
                if (a.Distance < b.Distance) return false;
                return a.Driver.Id > b.Driver.Id;
            }

            private void SiftUp(int index)
            {
                while (index > 0)
                {
                    int parentIndex = (index - 1) / 2;

                    if (!Greater(_items[index], _items[parentIndex]))
                        break;

                    HeapItem temp = _items[index];
                    _items[index] = _items[parentIndex];
                    _items[parentIndex] = temp;

                    index = parentIndex;
                }
            }

            private void SiftDown(int index)
            {
                int count = _items.Count;

                while (true)
                {
                    int left = 2 * index + 1;
                    int right = 2 * index + 2;
                    int largest = index;

                    if (left < count && Greater(_items[left], _items[largest]))
                    {
                        largest = left;
                    }

                    if (right < count && Greater(_items[right], _items[largest]))
                    {
                        largest = right;
                    }

                    if (largest == index)
                        break;

                    HeapItem temp = _items[index];
                    _items[index] = _items[largest];
                    _items[largest] = temp;

                    index = largest;
                }
            }
        }

        public static List<Driver> FindNearestDriversHeap(List<Driver> drivers, Order order, int k)
        {
            var heap = new MaxHeap();

            for (int i = 0; i < drivers.Count; i++)
            {
                Driver currentDriver = drivers[i];
                int dist = DistanceSquared(currentDriver, order);

                if (heap.Count < k)
                {
                    heap.Add(new HeapItem(currentDriver, dist));
                }
                else
                {
                    HeapItem maxItem = heap.PeekMax();
                    bool isBetter = false;
                    if (dist < maxItem.Distance)
                    {
                        isBetter = true;
                    }
                    else if (dist == maxItem.Distance &&
                             currentDriver.Id < maxItem.Driver.Id)
                    {
                        isBetter = true;
                    }

                    if (isBetter)
                    {
                        heap.PopMax();
                        heap.Add(new HeapItem(currentDriver, dist));
                    }
                }
            }

            var result = new List<Driver>();

            while (heap.Count > 0)
            {
                HeapItem item = heap.PopMax();
                result.Add(item.Driver);
            }

            result.Reverse();

            for (int i = 0; i < result.Count - 1; i++)
            {
                for (int j = 0; j < result.Count - 1 - i; j++)
                {
                    Driver a = result[j];
                    Driver b = result[j + 1];

                    int distA = DistanceSquared(a, order);
                    int distB = DistanceSquared(b, order);

                    bool needSwap = false;

                    if (distA > distB)
                    {
                        needSwap = true;
                    }
                    else if (distA == distB && a.Id > b.Id)
                    {
                        needSwap = true;
                    }

                    if (needSwap)
                    {
                        Driver temp = result[j];
                        result[j] = result[j + 1];
                        result[j + 1] = temp;
                    }
                }
            }

            return result;
        }
    }
    namespace Drivers
    {
        internal class Program
        {
            static void Main(string[] args)
            {
                var drivers = new List<Driver>
                {
                    new Driver(1, 0, 0),
                    new Driver(2, 5, 5),
                    new Driver(3, 2, 1),
                    new Driver(4, 10, 10),
                    new Driver(5, 3, 3),
                    new Driver(6, 1, 2),
                    new Driver(7, 8, 1),
                    new Driver(8, 2, 1)
                };

                var order = new Order(2, 2);
                var nearestNaive = DriverMatcher.FindNearestDriversNaive(drivers, order, 5);
                var nearestTopK = DriverMatcher.FindNearestDriversTopK(drivers, order, 5);
                var nearestHeap = DriverMatcher.FindNearestDriversHeap(drivers, order, 5);

                Console.WriteLine("Ближайшие водители к заказу ({0}, {1}) — наивный алгоритм:", order.X, order.Y);
                foreach (var d in nearestNaive)
                {
                    Console.WriteLine($"Id = {d.Id}, X = {d.X}, Y = {d.Y}");
                }
                Console.WriteLine();
                Console.WriteLine("Ближайшие водители к заказу ({0}, {1}) — алгоритм TopK:", order.X, order.Y);
                foreach (var d in nearestTopK)
                {
                    Console.WriteLine($"Id = {d.Id}, X = {d.X}, Y = {d.Y}");
                }
                Console.WriteLine();
                Console.WriteLine("Ближайшие водители к заказу ({0}, {1}) — алгоритм Heap:", order.X, order.Y);
                foreach (var d in nearestHeap)
                {
                    Console.WriteLine($"Id = {d.Id}, X = {d.X}, Y = {d.Y}");
                }

                Console.ReadKey();
            }
        }
    }
}