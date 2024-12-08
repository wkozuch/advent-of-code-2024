using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode;

class Day8
{
  public static void Main(string[] args)
  {
    var lines = File.ReadAllLines(@"Datasets\Day8.txt");
    Part1(lines);
    Part2(lines);
  }

  private static void Part1(IReadOnlyList<string> lines)
  {
    var positions = lines.SelectMany((x, i) => x.Select((y, j) => new Position(i, j, y))).ToList();
    var visited = lines.Select(x => x.Select(y => false).ToArray()).ToArray();
    var count = 0;
    foreach (var group in positions.Where(x => x.Char != '.').GroupBy(x => x.Char))
    {
      for (var i = 0; i < group.Count(); i++)
      {
        for (var j = 0; j < group.Count(); j++)
        {
          if (i == j) continue;
          var p1 = group.ElementAt(i);
          var p2 = group.ElementAt(j);
          var deltaX = p2.Row - p1.Row;
          var deltaY = p2.Column - p1.Column;
          Position p3 = new(p1.Row + 2 * deltaX, p1.Column + 2 * deltaY, '#');

          if (p3.Row >= visited.Length || p3.Column >= visited[0].Length || p3.Row < 0 || p3.Column < 0 || visited[p3.Row][p3.Column]) continue;

          visited[p3.Row][p3.Column] = true;
          count++;
        }
      }
    }
    Console.WriteLine($"Part 1: {count}"); //367
  }

  private static void Part2(IReadOnlyList<string> lines)
  {
    var positions = lines.SelectMany((x, i) => x.Select((y, j) => new Position(i, j, y))).ToList();
    var visited = lines.Select(x => x.Select(y => false).ToArray()).ToArray();
    var count = 0;
    foreach (var group in positions.Where(x => x.Char != '.').GroupBy(x => x.Char))
    {
      for (var i = 0; i < group.Count(); i++)
      {
        for (var j = 0; j < group.Count(); j++)
        {
          if (i == j) continue;
          var p1 = group.ElementAt(i);
          var p2 = group.ElementAt(j);
          var deltaX = p2.Row - p1.Row;
          var deltaY = p2.Column - p1.Column;
          Position p3 = p1 with { Char = '#' };

          while (!(p3.Row >= visited.Length || p3.Column >= visited[0].Length || p3.Row < 0 || p3.Column < 0))
          {
            if (!visited[p3.Row][p3.Column]) count++;
            visited[p3.Row][p3.Column] = true;
            p3 = new(p3.Row + deltaX, p3.Column + deltaY, '#');
          }
        }
      }
    }
    Console.WriteLine($"Part 2: {count}"); //1285 
  }

  public record Position(long Row, long Column, char Char);

}