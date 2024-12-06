using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode;

class Day6
{
  public static void Main(string[] args)
  {
    var lines = File.ReadAllLines(@"Datasets\Day6.txt");
    Part1(lines);
    Part2(lines);
  }

  private static void Part1(IReadOnlyList<string> lines)
  {
    var surface = lines.Select(x => x.Select(y => y).ToArray()).ToArray();
    var positionSurface = lines.SelectMany((x, i) => x.Select((y, j) => new Position(i, j, y))).ToList();
    var current = positionSurface.Single(x => x.Char == '^');
    var count = 0;
    while (current != null)
    {
      var next = GetNextPosition(current, surface);
      if (surface[current.Row][current.Column] != '*')
      {
        surface[current.Row][current.Column] = '*';
        count++;
      }
      current = next;
    }
    Console.WriteLine($"Part 1: {count}"); //5030 
  }

  private static void Part2(IReadOnlyList<string> lines)
  {
    var surface = lines.Select(x => x.Select(y => y).ToArray()).ToArray();
    var positionSurface = lines.SelectMany((x, i) => x.Select((y, j) => new Position(i, j, y))).ToList();
    var start = positionSurface.Single(x => x.Char == '^');

    var count = 0;

    for (var r = 0; r < surface.Length; r++)
    {
      for (var c = 0; c < surface[r].Length; c++)
      {
        if (r == start.Row && c == start.Column || surface[r][c] != '.') continue;
        surface[r][c] = '#';

        if (IsALoop(start, surface)) count++;

        surface[r][c] = '.';
      }
    }

    Console.WriteLine($"Part 2: {count}"); // 1928
  }

  private static bool IsALoop(Position start, char[][] surface)
  {
    var current = start;
    var visits = new HashSet<Position>();
    while (current != null)
    {
      current = GetNextPosition(current, surface);
      if (current == null) continue;
      if (visits.Add(current)) continue;
      return true;
    }

    return false;
  }

  private static Position? GetNextPosition(Position current, char[][] surface)
  {
    if (current.Char == '^')
    {
      if (current.Row - 1 < 0) return null;
      if (surface[current.Row - 1][current.Column] == '#') return current with { Char = '>' };
      return current with { Row = current.Row - 1 };
    }
    if (current.Char == '>')
    {
      if (current.Column + 1 >= surface[0].Length) return null;
      if (surface[current.Row][current.Column + 1] == '#') return current with { Char = 'v' };
      return current with { Column = current.Column + 1 };
    }
    if (current.Char == 'v')
    {
      if (current.Row + 1 >= surface.Length) return null;
      if (surface[current.Row + 1][current.Column] == '#') return current with { Char = '<' };
      return current with { Row = current.Row + 1 };
    }

    if (current.Char == '<')
    {
      if (current.Column - 1 < 0) return null;
      if (surface[current.Row][current.Column - 1] == '#') return current with { Char = '^' };
      return current with { Column = current.Column - 1 };
    }

    return null;
  }

  public record Position(long Row, long Column, char Char);

}