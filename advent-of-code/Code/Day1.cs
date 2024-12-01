using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
  class Day1
  {
    public static void Main(string[] args)
    {
      var lines = File.ReadAllLines(@"Datasets\Day1.txt");
      var col1 = new List<int>();
      var col2 = new List<int>();
      foreach (var line in lines)
      {
        var cells = line.Split("   ");
        col1.Add(int.Parse(cells.First()));
        col2.Add(int.Parse(cells.Last()));
      }
      col1 = col1.OrderBy(x => x).ToList();
      col2 = col2.OrderBy(x => x).ToList();
      var sum1 = col1.Zip(col2, (a, b) => Math.Abs(a - b)).Sum();
      var sum2 = col1.Select(n => n * col2.Count(x => x == n)).Sum();
      Console.WriteLine($"Part one: {sum1}");
      Console.WriteLine($"Part two: {sum2}");
    }
  }
}
