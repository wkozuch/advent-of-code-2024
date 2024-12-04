using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace AdventOfCode
{
  class Day2
  {
    public static void Main(string[] args)
    {
      var lines = File.ReadAllLines(@"Datasets\Day2.txt");
      Part1(lines);
      Part2(lines);
    }

    private static void Part1(string[] lines)
    {
      var safe = 0;
      foreach (var line in lines)
      {
        var levels = line.Split(" ").Select(x => int.Parse(x)).ToList();
        safe = IsLineSafe(levels) ? safe + 1 : safe;
      }   
      Console.WriteLine($"Part one: {safe}"); //369
    }
    private static void Part2(string[] lines)
    {
      var safe = 0;
      foreach (var line in lines)
      {
        var levels = line.Split(" ").Select(x => int.Parse(x)).ToList();
        if (IsLineSafe(levels))
        {
          safe++;
          continue;
        }

        for (int i = 0; i < levels.Count; i++)
        {
          var sublist = new List<int>(levels);
          sublist.RemoveAt(i);
          if (!IsLineSafe(sublist)) continue;
          safe++;
          break;
        }
      }

      Console.WriteLine($"Part two: {safe}"); //428
    }

    private static bool IsLineSafe(List<int> levels)
    {
      for (int i = 0; i < levels.Count - 1; i++)
      {
        var delta = levels[i + 1] - levels[i];
        var isDeltaOk = 0 < Math.Abs(delta) && Math.Abs(delta) < 4;
        var isMonotonous = Math.Sign(delta) == Math.Sign(levels[1] - levels[0]);
        if (!isDeltaOk || !isMonotonous)
        {
          return false;
        }
      }
      return true;
    }
  }
}
