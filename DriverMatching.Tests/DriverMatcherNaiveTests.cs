
using System.Collections.Generic;
using NUnit.Framework;
using DriverMatching;

namespace DriverMatching.Tests
{
    public class DriverMatcherNaiveTests
    {
        [Test]
        public void ReturnsNearestDrivers_ForSimpleCase()
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

            var result = DriverMatcher.FindNearestDriversNaive(drivers, order, k);

            Assert.That(result.Count, Is.EqualTo(5));
        }
        [Test]
        public void ReturnsAllDrivers_WhenKMoreThanDriversCount()
        {
            var drivers = new List<Driver>
            {
                new Driver(1, 0, 0),
                new Driver(2, 5, 5)
            };

            var order = new Order(2, 2);
            int k = 5;

            var result = DriverMatcher.FindNearestDriversNaive(drivers, order, k);

            Assert.That(result.Count, Is.EqualTo(2));
        }
        [Test]
        public void ReturnsEmptyList_WhenNoDrivers()
        {
            var drivers = new List<Driver>();

            var order = new Order(2, 2);
            int k = 3;

            var result = DriverMatcher.FindNearestDriversNaive(drivers, order, k);

            Assert.That(result.Count, Is.EqualTo(0));
        }
        [Test]
        public void Naive_ReturnsEmptyList_WhenKIsZero()
        {
            var drivers = new List<Driver>
            {
                new Driver(1, 0, 0),
                new Driver(2, 5, 5)
            };

            var order = new Order(2, 2);
            int k = 0;

            var result = DriverMatcher.FindNearestDriversNaive(drivers, order, k);

            Assert.That(result, Is.Empty);
        }
    }
}
