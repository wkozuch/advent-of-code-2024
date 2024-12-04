using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
  class Day4
  {
    public static void Main(string[] args)
    {
      var lines = File.ReadAllLines(@"Datasets\Day4.txt");
      var surface = lines.Select(x => x.Select(y => y).ToArray()).ToArray();
      Part1(surface);
      Part2(surface);
    }

    private static void Part1(char[][] surface)
    {
      var sum = 0;
      for (var r = 0; r < surface.Length; r++)
      {
        for (var c = 0; c < surface[r].Length; c++)
        {
          if (surface[r][c] != 'X') continue;
          var wordCount = surface.CountWordsStartingFrom(r, c);
          sum += wordCount;
        }
      }

      Console.WriteLine(sum);
    }

    private static void Part2(char[][] surface)
    {
      var sum = 0;

      for (var r = 0; r < surface.Length; r++)
      {
        for (var c = 0; c < surface[r].Length; c++)
        {
          if (surface[r][c] != 'A') continue;
          var isX = surface.CheckDiagonals(r, c);
          sum = isX ? sum + 1 : sum;
        }
      }

      Console.WriteLine(sum);
    }
  }

  public static class CharArrayExtensions
  {
    public static int CountWordsStartingFrom(this char[][] surface, int r, int c)
    {
      var ch = "MAS";
      var count = surface.IsWordInDirection(r, c, -1, 0, ch); //Left
      count += surface.IsWordInDirection(r, c, 1, 0, ch); //Right 
      count += surface.IsWordInDirection(r, c, 0, 1, ch); //Down
      count += surface.IsWordInDirection(r, c, 0, -1, ch); //Up

      count += surface.IsWordInDirection(r, c, 1, 1, ch); //NE
      count += surface.IsWordInDirection(r, c, 1, -1, ch); //SE
      count += surface.IsWordInDirection(r, c, -1, 1, ch); //NW
      count += surface.IsWordInDirection(r, c, -1, -1, ch); //SW
      return count;
    }

    public static bool CheckDiagonals(this char[][] surface, int r, int c)
    {
      var count = surface.IsWordInDirection(r, c, 1, 1, new[] { 'M' });
      count += surface.IsWordInDirection(r, c, 1, -1, new[] { 'M' });
      count += surface.IsWordInDirection(r, c, -1, 1, new[] { 'S' });
      count += surface.IsWordInDirection(r, c, -1, -1, new[] { 'S' });
      if (count == 4) return true;

      count = surface.IsWordInDirection(r, c, 1, 1, new[] { 'S' });
      count += surface.IsWordInDirection(r, c, 1, -1, new[] { 'S' });
      count += surface.IsWordInDirection(r, c, -1, 1, new[] { 'M' });
      count += surface.IsWordInDirection(r, c, -1, -1, new[] { 'M' });
      if (count == 4) return true;

      count = surface.IsWordInDirection(r, c, 1, 1, new[] { 'M' });
      count += surface.IsWordInDirection(r, c, 1, -1, new[] { 'S' });
      count += surface.IsWordInDirection(r, c, -1, 1, new[] { 'M' });
      count += surface.IsWordInDirection(r, c, -1, -1, new[] { 'S' });
      if (count == 4) return true;

      count = surface.IsWordInDirection(r, c, 1, 1, new[] { 'S' });
      count += surface.IsWordInDirection(r, c, 1, -1, new[] { 'M' });
      count += surface.IsWordInDirection(r, c, -1, 1, new[] { 'S' });
      count += surface.IsWordInDirection(r, c, -1, -1, new[] { 'M' });
      if (count == 4) return true;

      return false;
    }


    public static int IsWordInDirection(this char[][] surface, int r, int c, int deltaR, int deltaC, IEnumerable<char> ch)
    {
      if (!ch.Any()) return 1;

      if (0 <= r + deltaR && r + deltaR < surface.Length && 0 <= c + deltaC && c + deltaC < surface[0].Length &&
          surface[r + deltaR][c + deltaC] == ch.First())
      {
        return surface.IsWordInDirection(r + deltaR, c + deltaC, deltaR, deltaC, ch.Skip(1));
      }

      return 0;
    }

    public static void Draw(this char[][] surface)
    {
      foreach (var line in surface)
      {
        Console.WriteLine(string.Join("", line));
      }

      Console.WriteLine();
    }
  }
}
