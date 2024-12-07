using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode;

class Day7
{
  public static void Main(string[] args)
  {
    var lines = File.ReadAllLines(@"Datasets\Day7.txt");
    Part1(lines);
    Part2(lines);
  }

  private static void Part1(IReadOnlyList<string> lines)
  {
    long count = 0;
    foreach (var line in lines)
    {
      var parts = line.Split(new[] { ": " }, StringSplitOptions.None);
      var testValue = long.Parse(parts[0]);
      var numbers = Array.ConvertAll(parts[1].Split(' '), long.Parse);
      var noOpeartors = 3;
      var noCombinations = Math.Pow(noOpeartors, numbers.Length - 1);
      for (var i = 0; i < noCombinations; i++)
      {
        var result = numbers[0];
        for (var j = 0; j < numbers.Length - 1; j++)
        {
          if ((i & (1 << j)) == 0) // If the j-th bit of i is 0, use multiplication
          {
            result *= numbers[j + 1];
          }
          else // If the j-th bit of i is 1, use addition
          {
            result += numbers[j + 1];
          }
          if (result > testValue) break;
        }

        if (testValue == result)
        {
          count += testValue;
          break;
        }
      }
    }
    Console.WriteLine($"Part 1: {count}"); //1545311493300 
  }

  private static void Part2(IReadOnlyList<string> lines)
  {
    long count = 0;
    foreach (var line in lines)
    {
      var parts = line.Split(new[] { ": " }, StringSplitOptions.None);
      var testValue = long.Parse(parts[0]);
      var numbers = Array.ConvertAll(parts[1].Split(' '), long.Parse);
      var noOpeartors = 3;
      var noCombinations = Math.Pow(noOpeartors, numbers.Length - 1);
      for (var i = 0; i < noCombinations; i++)
      {
        var result = (double)numbers[0];
        for (var j = 0; j < numbers.Length - 1; j++)
        {
          var operationIndex = i / (int)Math.Pow(3, j) % 3; // Determine the operator at the j-th position
          switch (operationIndex)
          {
            case 0:
              result *= numbers[j + 1];  // Multiplication
              break;
            case 1:
              result += numbers[j + 1];  // Addition
              break;
            case 2:
              result = result * Math.Pow(10, numbers[j + 1].ToString().Length) + numbers[j + 1];  // Concatenation
              break;
          }
          if (result > testValue) break;
        }

        if (testValue == result)
        {
          count += testValue;
          break;
        }
      }
    }
    Console.WriteLine($"Part 2: {count}"); //169122112716571 
  }
}