using System.Collections.Generic;
using NUnit.Framework;
using DriverMatching;

namespace DriverMatching.Tests
{
    public class DriverMatcherHeapTests
    {
        [Test]
        public void Heap_ReturnsNearestDrivers_ForSimpleCase()
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

            var result = DriverMatcher.FindNearestDriversHeap(drivers, order, k);

            Assert.That(result.Count, Is.EqualTo(5));
        }

        [Test]
        public void Heap_ReturnsAllDrivers_WhenKMoreThanCount()
        {
            var drivers = new List<Driver>
            {
                new Driver(1, 0, 0),
                new Driver(2, 5, 5)
            };

            var order = new Order(2, 2);
            int k = 5;

            var result = DriverMatcher.FindNearestDriversHeap(drivers, order, k);

            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public void Heap_ReturnsEmptyList_WhenNoDrivers()
        {
            var drivers = new List<Driver>();

            var order = new Order(2, 2);
            int k = 3;

            var result = DriverMatcher.FindNearestDriversHeap(drivers, order, k);

            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void Heap_ReturnsSameDrivers_AsNaive()
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
            var heap = DriverMatcher.FindNearestDriversHeap(drivers, order, k);

            Assert.That(heap.Count, Is.EqualTo(naive.Count));

            for (int i = 0; i < naive.Count; i++)
            {
                Assert.That(heap[i].Id, Is.EqualTo(naive[i].Id));
            }
        }
        [Test]
        public void Heap_WhenEqualDistance_PrefersSmallerIds()
        {
            var drivers = new List<Driver>
            {
                new Driver(5, 1, 3),
                new Driver(2, 3, 1),
                new Driver(10,3, 1),
                new Driver(1, 0, 0)
            };

            var order = new Order(2, 2);
            int k = 2;

            var result = DriverMatcher.FindNearestDriversHeap(drivers, order, k);

            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.Select(d => d.Id), Is.EqualTo(new[] { 2, 5 }));
        }
    }
}