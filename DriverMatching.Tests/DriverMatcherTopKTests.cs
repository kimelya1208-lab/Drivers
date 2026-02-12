using System.Collections.Generic;
using NUnit.Framework;
using DriverMatching;

namespace DriverMatching.Tests
{
    public class DriverMatcherTopKTests
    {
        [Test]
        public void TopK_ReturnsNearestDrivers_ForSimpleCase()
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
            int k = 5;

            var result = DriverMatcher.FindNearestDriversTopK(drivers, order, k);

            Assert.That(result.Count, Is.EqualTo(5));
        }

        [Test]
        public void TopK_ReturnsAllDrivers_WhenKMoreThanCount()
        {
            var drivers = new List<Driver>
            {
                new Driver(1, 0, 0),
                new Driver(2, 5, 5)
            };

            var order = new Order(2, 2);
            int k = 5;

            var result = DriverMatcher.FindNearestDriversTopK(drivers, order, k);

            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public void TopK_ReturnsEmptyList_WhenNoDrivers()
        {
            var drivers = new List<Driver>();

            var order = new Order(2, 2);
            int k = 3;

            var result = DriverMatcher.FindNearestDriversTopK(drivers, order, k);

            Assert.That(result.Count, Is.EqualTo(0));
        }
        [Test]
        public void TopK_ReturnsSameOrder_AsNaive()
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
            int k = 5;

            var naive = DriverMatcher.FindNearestDriversNaive(drivers, order, k);
            var topK = DriverMatcher.FindNearestDriversTopK(drivers, order, k);

            Assert.That(topK.Select(d => d.Id), Is.EqualTo(naive.Select(d => d.Id)));
        }
    }
}