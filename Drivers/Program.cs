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
                Console.WriteLine("Ближайшие водители к заказу ({0}, {1}) — наивный алгоритм:", order.X, order.Y);
                foreach (var d in nearestNaive)
                {
                    Console.WriteLine($"Id = {d.Id}, X = {d.X}, Y = {d.Y}");
                }
            }
        }
    }
}