using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode;

class Day10
{
  public static void Main(string[] args)
  {
    var lines = File.ReadAllLines(@"Datasets\Day10.txt");
    var surface = lines.Select(x => x.Select(y => int.Parse(y.ToString())).ToArray()).ToArray();
    var positions = lines.SelectMany((x, i) => x.Select((y, j) => new Position(i, j, int.Parse(y.ToString())))).ToList();
    var count1 = 0;
    var count2 = 0;

    foreach (var p in positions.Where(x => x.Height == 0))
    {
      count1 += CountHikes(p, surface, ref count2);
    }

    Console.WriteLine($"Part 1: {count1}"); //733
    Console.WriteLine($"Part 2: {count2}"); //1514
  }

  private static int CountHikes(Position p, int[][] surface, ref int count)
  {
    var pikes = new HashSet<Position>();
    var nextPositions = GetNextPositions(p, surface);
    while (nextPositions.Any())
    {
      var nextNextPositions = new List<Position>();
      foreach (var next in nextPositions)
      {
        if (next.Height == 9)
        {
          count++;
          pikes.Add(next);
          continue;
        }
        nextNextPositions.AddRange(GetNextPositions(next, surface));
      }
      nextPositions = nextNextPositions;
    }
    return pikes.Count;
  }

  private static List<Position> GetNextPositions(Position current, int[][] surface)
  {
    var positions = new List<Position>();
    if (current.Height == 9) return new List<Position> { current };
    if (current.Row - 1 >= 0 && surface[current.Row - 1][current.Column] == current.Height + 1) positions.Add(new Position(current.Row - 1, current.Column, current.Height + 1));
    if (current.Row + 1 < surface.Length && surface[current.Row + 1][current.Column] == current.Height + 1) positions.Add(new Position(current.Row + 1, current.Column, current.Height + 1));
    if (current.Column - 1 >= 0 && surface[current.Row][current.Column - 1] == current.Height + 1) positions.Add(new Position(current.Row, current.Column - 1, current.Height + 1));
    if (current.Column + 1 < surface[0].Length && surface[current.Row][current.Column + 1] == current.Height + 1) positions.Add(new Position(current.Row, current.Column + 1, current.Height + 1));
    return positions;
  }

  public record Position(long Row, long Column, int Height);

}