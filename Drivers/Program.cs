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
}