using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
  class Day5
  {
    public static void Main(string[] args)
    {
      var lines = File.ReadAllLines(@"Datasets\Day5.txt");
      Part1(lines);
      Part2(lines);
    }

    private static void Part1(IReadOnlyList<string> lines)
    {
      var pages = new Dictionary<int, List<int>>();
      int i;

      for (i = 0; i < lines.Count; i++) //Page ordering rules
      {
        var line = lines[i];
        if (string.IsNullOrWhiteSpace(line)) break;

        var rules = line.Split("|").Select(int.Parse).ToList();
        if (pages.ContainsKey(rules[0]))
        {
          pages[rules[0]].Add(rules[1]);
        }
        else
        {
          pages.Add(rules[0], new List<int>() { rules[1] });
        }

        if (!pages.ContainsKey(rules[1])) pages.Add(rules[1], new List<int>());
      }

      var sum = 0;
      for (var j = i + 1; j < lines.Count; j++)
      {
        var line = lines[j];
        var updates = line.Split(",").Select(int.Parse).ToList();
        var pagesBefore = new List<int>();
        var correctOrder = true;
        for (var u = 1; u < updates.Count; u++)
        {
          pagesBefore.Add(updates[u - 1]);
          var nextPages = pages[updates[u]];
          if (!nextPages.Intersect(pagesBefore).Any()) continue;
          correctOrder = false;
          break;
        }

        if (correctOrder) sum += updates[updates.Count / 2];
      }

      Console.WriteLine($"Part one: {sum}"); //5129
    }


    private static void Part2(IReadOnlyList<string> lines)
    {
      var pages = new Dictionary<int, List<int>>();
      int i;
      for (i = 0; i < lines.Count; i++)
      {
        var line = lines[i];
        if (string.IsNullOrWhiteSpace(line)) break;

        var rules = line.Split("|").Select(int.Parse).ToList();
        if (pages.ContainsKey(rules[0]))
        {
          pages[rules[0]].Add(rules[1]);
        }
        else
        {
          pages.Add(rules[0], new List<int>() { rules[1] });
        }

        if (!pages.ContainsKey(rules[1])) pages.Add(rules[1], new List<int>());
      }

      var sum = 0;
      for (var j = i + 1; j < lines.Count; j++)
      {
        var line = lines[j];
        var updates = line.Split(",").Select(int.Parse).ToList();
        var pagesBefore = new List<int>();
        for (var k = 1; k < updates.Count; k++)
        {
          pagesBefore.Add(updates[k - 1]);
          var nextPages = pages[updates[k]];
          if (!nextPages.Intersect(pagesBefore).Any()) continue;
          var order = pages.Select(x => x).Where(x => updates.Any(y => y.Equals(x.Key))).ToList();
          var nwo = new List<int>();
          for (var index = 0; index < updates.Count; index++)
          {
            var last = order.Where(x => !nwo.Contains(x.Key)).First(x => !x.Value.Intersect(updates.Except(nwo)).Any());
            nwo.Add(last.Key);
          }

          sum += nwo[nwo.Count / 2]; break;
        }
      }

      Console.WriteLine($"Part two: {sum}"); //4077
    }
  }
}
