using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode;

class Day9
{
  public static void Main(string[] args)
  {
    var lines = File.ReadAllText(@"Datasets\Day9.txt");
    Part1(lines);
    Part2(lines);
  }

  private static void Part1(string input)
  {
    var numbers = input.Select(c => int.Parse(c.ToString())).ToList();

    // Create the disk layout based on numbers
    var disk = CreateDisk(numbers);

    var defragmented = DefragmentDisk1(disk);

    var count = CalculateCount(defragmented);

    Console.WriteLine($"Part 1: {count}"); //6279058075753
  }

  private static void Part2(string input)
  {
    var numbers = input.Select(c => int.Parse(c.ToString())).ToList();

    // Create the disk layout based on numbers
    var disk = CreateDisk(numbers);

    // Defragment the disk
    var defragmented = DefragmentDisk2(disk);

    // Calculate the final result based on the defragmented disk
    var count = CalculateCount(defragmented);
    
    Console.WriteLine($"Part 2: {count}"); // 6301361958738
  }

  private static List<string> CreateDisk(List<int> numbers)
  {
    var disk = new List<string>();
    var id = 0;

    for (var i = 0; i < numbers.Count; i++)
    {
      var blockChar = (i % 2 == 0) ? id++.ToString() : ".";
      disk.AddRange(Enumerable.Repeat(blockChar, numbers[i]));
    }

    return disk;
  }

  private static string[] DefragmentDisk1(List<string> disk)
  {
    var defragmented = disk.ToArray();

    var start = 0; // Pointer to find the next empty slot from the left
    var end = disk.Count - 1; // Pointer to find the next non-empty element from the right

    while (start < end)
    {
      // Move the `start` pointer to the next "."
      while (start < end && defragmented[start] != ".")
        start++;

      // Move the `end` pointer to the next non-"."
      while (start < end && defragmented[end] == ".")
        end--;

      // If valid positions, swap elements
      if (start < end)
      {
        defragmented[start] = defragmented[end];
        defragmented[end] = ".";
        start++;
        end--;
      }
    }

    return defragmented;
  }

  private static string[] DefragmentDisk2(List<string> disk)
  {
    var defragmented = disk.ToArray();

    for (var i = disk.Count - 1; i >= 0; i--)
    {
      //find first block to move and its size
      while (i >= 0 && disk[i] == ".") i--;
      if (i < 0) break;

      var blockChar = disk[i];
      var size = 1;

      while (i - 1 > 0 && disk[i - 1] == blockChar)
      {
        size++;
        i--;
      }

      // Find the first available free space to move the block
      if (GetFirstAvailableFreeSpace(defragmented, size, i, out var freeSpaceIndex))
      {
        MoveBlock(defragmented, disk, i, size, freeSpaceIndex);
      }
    }

    return defragmented;
  }

  private static void MoveBlock(string[] defragmented, List<string> disk, int blockStart, int size, int targetIndex)
  {
    for (var k = blockStart + size - 1; k >= blockStart; k--)
    {
      defragmented[targetIndex++] = disk[k];
      disk[k] = ".";
      defragmented[k] = ".";
    }
  }

  private static bool GetFirstAvailableFreeSpace(string[] disk, int size, int limitIndex, out int location)
  {
    location = 0;
    for (var j = 0; j < limitIndex; j++)
    {
      if (disk[j] == ".")
      {
        var freeSpaceStart = j;
        var freeSpaceSize = 1;
        while (j + 1 < limitIndex && disk[j + 1] == ".")
        {
          freeSpaceSize++;
          j++;
        }

        if (freeSpaceSize >= size)
        {
          location = freeSpaceStart;
          return true;
        }
      }
    }
    return false;
  }

  private static long CalculateCount(string[] defragmented)
  {
    long count = 0;

    for (var i = 0; i < defragmented.Length; i++)
    {
      if (defragmented[i] != ".")
      {
        count += int.Parse(defragmented[i]) * i;
      }
    }

    return count;
  }
}