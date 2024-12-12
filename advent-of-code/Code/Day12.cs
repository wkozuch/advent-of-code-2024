using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode;

class Day12
{
  public static void Main(string[] args)
  {
    var lines = File.ReadAllLines(@"Datasets\Day12.txt");
    var surface = lines.Select(x => x.Select(y => y).ToArray()).ToArray();
    var positions = lines.SelectMany((x, i) => x.Select((y, j) => new Position(i, j, y))).ToList();
    var visitedSurfaces = lines.Select(x => x.Select(y => '.').ToArray()).ToArray();
    var visited = new HashSet<Position>();
    var gardens = new List<Garden>();

    foreach (var p in positions)
    {
      if (visited.Contains(p)) continue;

      var nextPositions = new List<Position> { p };
      var garden = new Garden(p.Garden);
      var perimeter = new HashSet<Position>();
      while (nextPositions.Any())
      {
        var nextNextPositions = new List<Position>();
        foreach (var next in nextPositions)
        {
          if (visited.Contains(next)) continue;
          visited.Add(next);
          visitedSurfaces[next.Row][next.Column] = next.Garden;
          garden.Area++;
          var nexts = GetNextPositions(next, surface);
          garden.Perimeter += 4 - nexts.Count;
          nextNextPositions.AddRange(nexts);

          //var fences = GetFences(next, surface);
          //foreach (var f in fences)
          //{
          //  perimeter.Add(f);

          //  // if f row and column are withing the surface, update visitedSurfaces
          //  if (f.Row >= 0 && f.Row < visitedSurfaces.Length && f.Column >= 0 && f.Column < visitedSurfaces[0].Length)
          //  {
          //    visitedSurfaces[f.Row][f.Column] = f.Garden;
          //    // visitedSurfaces.Draw();
          //  }

          //  if (perimeter.Any(x => x.Row == f.Row && x.Column == f.Column - 1 && x.Garden == f.Garden)) continue;
          //  if (perimeter.Any(x => x.Row == f.Row && x.Column == f.Column + 1 && x.Garden == f.Garden)) continue;
          //  if (perimeter.Any(x => x.Column == f.Column && x.Row == f.Row - 1 && x.Garden == f.Garden)) continue;
          //  if (perimeter.Any(x => x.Column == f.Column && x.Row == f.Row + 1 && x.Garden == f.Garden)) continue;

          //  garden.Perimeter++;
          //}
          //    Console.WriteLine($"Garden{next.Garden}: [{next.Row},{next.Column}]={next.Garden}, Perimeter: {garden.Perimeter} Fences ({fences.Count}): {string.Join(", ", fences)}");
        }
        nextPositions = nextNextPositions;
      }
      //    visitedSurfaces.Draw();
      gardens.Add(garden);
    }

    foreach (var g in gardens)
    {
      Console.WriteLine($"Garden: {g.Type}, Area: {g.Area} Perimeter: {g.Perimeter} Price {g.Area * g.Perimeter}");
    }

    var count = gardens.Sum(x => x.Area * x.Perimeter);
    Console.WriteLine($"Part1: {count}"); //1494342 //893876 (too high)
  }

  private static List<Position> GetNextPositions(Position current, char[][] surface)
  {
    var positions = new List<Position>();
    if (current.Row - 1 >= 0 && surface[current.Row - 1][current.Column] == current.Garden) positions.Add(current with { Row = current.Row - 1 });
    if (current.Row + 1 < surface.Length && surface[current.Row + 1][current.Column] == current.Garden) positions.Add(current with { Row = current.Row + 1 });
    if (current.Column - 1 >= 0 && surface[current.Row][current.Column - 1] == current.Garden) positions.Add(current with { Column = current.Column - 1 });
    if (current.Column + 1 < surface[0].Length && surface[current.Row][current.Column + 1] == current.Garden) positions.Add(current with { Column = current.Column + 1 });
    return positions;
  }

  private static HashSet<Position> GetFences(Position current, char[][] surface)
  {
    var positions = new HashSet<Position>();
    if (current.Row - 1 < 0) positions.Add(current with { Row = current.Row - 1, Garden = '^' });
    if (current.Row - 1 >= 0 && surface[current.Row - 1][current.Column] != current.Garden) positions.Add(current with { Row = current.Row - 1, Garden = '^' });

    if (current.Row + 1 >= surface.Length) positions.Add(current with { Row = current.Row + 1, Garden = 'v' });
    if (current.Row + 1 < surface.Length && surface[current.Row + 1][current.Column] != current.Garden) positions.Add(current with { Row = current.Row + 1, Garden = 'v' });

    if (current.Column - 1 < 0) positions.Add(current with { Column = current.Column - 1, Garden = '<' });
    if (current.Column - 1 >= 0 && surface[current.Row][current.Column - 1] != current.Garden) positions.Add(current with { Column = current.Column - 1, Garden = '<' });

    if (current.Column + 1 >= surface[0].Length) positions.Add(current with { Column = current.Column + 1, Garden = '>' });
    if (current.Column + 1 < surface[0].Length && surface[current.Row][current.Column + 1] != current.Garden) positions.Add(current with { Column = current.Column + 1, Garden = '>' });
    return positions;
  }

  public record Position(long Row, long Column, char Garden);

  public class Garden
  {
    public Garden(char garden)
    {
      Type = garden;
    }
    public char Type { get; set; }
    public int Area { get; set; }
    public int Perimeter { get; set; }
  }

}