using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode;

class Day11
{
  public static void Main(string[] args)
  {

    var lines = File.ReadAllText(@"Datasets\Day11.txt");
    var stones = lines.Split(" ").Select(long.Parse).ToList();
    
    Blink(stones, 25); //190865
    Blink(stones, 75); //225404711855335
  }

  // Simulate the given number of blinks
  private static void Blink(IEnumerable<long> stones, int numBlinks)
  {
    // Initial stone counts
    var stoneCounts = stones.ToDictionary(s => s, s => 1L);

    // Apply the transformation for the given number of blinks
    for (var i = 0; i < numBlinks; i++)
    {
      stoneCounts = BlinkOnce(stoneCounts);
    }

    Console.WriteLine($"Count: {stoneCounts.Values.Sum()}");
  }

  private static Dictionary<long, long> BlinkOnce(Dictionary<long, long> stoneCounts)
  {
    var newCounts = new Dictionary<long, long>();

    foreach (var (stone, count) in stoneCounts)
    {
      if (stone == 0)
      {
        // Rule 1: If stone is 0, it becomes 1
        newCounts.TryAdd(1, 0);
        newCounts[1] += count;
        continue;
      }

      if (stone.ToString().Length % 2 == 0)
      {
        // Rule 2: If stone has an even number of digits, split it
        var s = stone.ToString();
        var mid = s.Length / 2;
        var left = long.Parse(s[..mid]);
        var right = long.Parse(s[mid..]);

        newCounts.TryAdd(left, 0);
        newCounts[left] += count;
        newCounts.TryAdd(right, 0);
        newCounts[right] += count;
        continue;
      }

      // Rule 3: If none of the above, multiply by 2024
      var newStone = stone * 2024;
      newCounts.TryAdd(newStone, 0);
      newCounts[newStone] += count;
    }

    return newCounts;
  }
}