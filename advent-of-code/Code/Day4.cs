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
      // jagged char[][] array with only '.'
      Part1(surface);
      Part2(surface);
    }

    private static void Part1(char[][] surface)
    {
      var visited = Enumerable.Range(0, surface[0].Length)
        .Select(x => new char[surface[x].Length].Select(c => '.').ToArray()).ToArray();

      surface.Draw();
      var sum = 0;

      for (var r = 0; r < surface.Length; r++)
      {
        for (var c = 0; c < surface[r].Length; c++)
        {
          var wordStart = surface[r][c];
          var word = "XMAS";
          if (wordStart != word.First()) continue;
          visited[r][c] = 'X';
          // check how many words come from this tile 
          var oneTime = Enumerable.Range(0, surface[0].Length)
            .Select(x => new char[surface[x].Length].Select(c => '.').ToArray()).ToArray();
          oneTime[r][c] = 'X';
          var wordCount = surface.IsNextOneEqualTo(r, c, word.Skip(1), oneTime);
          sum += wordCount;
          Console.WriteLine($"Sum: {sum}, word count: {wordCount}");
          Console.WriteLine();
        }
      }

      Console.WriteLine(sum);
    }

    private static void Part2(char[][] surface)
    {
      var visited = Enumerable.Range(0, surface[0].Length)
        .Select(x => new char[surface[x].Length].Select(c => '.').ToArray()).ToArray();

      surface.Draw();
      var sum = 0;

      for (var r = 0; r < surface.Length; r++)
      {
        for (var c = 0; c < surface[r].Length; c++)
        {
          var wordStart = surface[r][c];
          if (wordStart != 'A') continue;
          visited[r][c] = 'A';
          // check how many words come from this tile 
          var oneTime = Enumerable.Range(0, surface[0].Length)
            .Select(x => new char[surface[x].Length].Select(c => '.').ToArray()).ToArray();
          oneTime[r][c] = 'A';
          var wordCount = surface.IsNextOneEqualTo(r, c, word.Skip(1), oneTime);
          sum += wordCount;
          Console.WriteLine($"Sum: {sum}, word count: {wordCount}");
          Console.WriteLine();
        }
      }

      Console.WriteLine(sum);
    }
  }

  public static class CharArrayExtensions
  {
    public static int IsNextOneEqualTo(this char[][] surface, int r, int c, IEnumerable<char> ch, char[][] visited)
    {
      if (!ch.Any())
      {
        //  visited.Draw();
        return 1;
      }

      var count = surface.IsWordInDirection(r, c, -1, 0, ch, visited);
      count += surface.IsWordInDirection(r, c, 1, 0, ch, visited);
      count += surface.IsWordInDirection(r, c, 0, 1, ch, visited);
      count += surface.IsWordInDirection(r, c, 0, -1, ch, visited);

      count += surface.IsWordInDirection(r, c, 1, 1, ch, visited);
      count += surface.IsWordInDirection(r, c, 1, -1, ch, visited);
      count += surface.IsWordInDirection(r, c, -1, 1, ch, visited);
      count += surface.IsWordInDirection(r, c, -1, -1, ch, visited);

      //if (0 < r && surface[r - 1][c] == ch.First())
      //{
      //  visited[r - 1][c] = ch.First();
      //  count += surface.IsNextOneEqualTo(r - 1, c, ch.Skip(1), 0, visited);
      //}

      //if (r + 1 < surface.Length && surface[r + 1][c] == ch.First())
      //{
      //  visited[r + 1][c] = ch.First();
      //  count += surface.IsNextOneEqualTo(r + 1, c, ch.Skip(1), 0, visited);
      //}

      //if (0 < c && surface[r][c - 1] == ch.First())
      //{
      //  visited[r][c - 1] = ch.First();
      //  count += surface.IsNextOneEqualTo(r, c - 1, ch.Skip(1), 0, visited);
      //}

      //if (c + 1 < surface[0].Length && surface[r][c + 1] == ch.First())
      //{
      //  visited[r][c + 1] = ch.First();
      //  count += surface.IsNextOneEqualTo(r, c + 1, ch.Skip(1), 0, visited);
      //}

      //if (r + 1 < surface.Length && c + 1 < surface[0].Length && surface[r + 1][c + 1] == ch.First())
      //{
      //  visited[r + 1][c + 1] = ch.First();
      //  count += surface.IsNextOneEqualTo(r + 1, c + 1, ch.Skip(1), 0, visited);
      //}

      //if (r + 1 < surface.Length && 0 < c && surface[r + 1][c - 1] == ch.First())
      //{
      //  visited[r + 1][c - 1] = ch.First();
      //  count += surface.IsNextOneEqualTo(r + 1, c - 1, ch.Skip(1), 0, visited);
      //}

      //if (0 < r && c + 1 < surface[0].Length && surface[r - 1][c + 1] == ch.First())
      //{
      //  visited[r - 1][c + 1] = ch.First();
      //  count += surface.IsNextOneEqualTo(r - 1, c + 1, ch.Skip(1), 0, visited);
      //}

      //if (0 < r && 0 < c && surface[r - 1][c - 1] == ch.First())
      //{
      //  visited[r - 1][c - 1] = ch.First();
      //  count += surface.IsNextOneEqualTo(r - 1, c - 1, ch.Skip(1), 0, visited);
      //}

      return count;
    }

    public static int CheckDiagonals(this char[][] surface, int r, int c, IEnumerable<char> ch, char[][] visited)
    {
      if (!ch.Any())
      {
        //  visited.Draw();
        return 1;
      }

      var count = surface.IsWordInDirection(r, c, 1, 1, 'M', visited);
      count += surface.IsWordInDirection(r, c, 1, -1, ch, visited);
      count += surface.IsWordInDirection(r, c, -1, 1, ch, visited);
      count += surface.IsWordInDirection(r, c, -1, -1, ch, visited);
      
      return count;
    }


    public static int IsWordInDirection(this char[][] surface, int r, int c, int deltaR, int deltaC, IEnumerable<char> ch, char[][] visited)
    {
      if (!ch.Any()) return 1;

      if (0 <= r + deltaR && r + deltaR < surface.Length && 0 <= c + deltaC && c + deltaC < surface[0].Length &&
          surface[r + deltaR][c + deltaC] == ch.First())
      {
        visited[r + deltaR][c + deltaC] = ch.First();
        //  visited.Draw();
        return surface.IsWordInDirection(r + deltaR, c + deltaC, deltaR, deltaC, ch.Skip(1), visited);
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
