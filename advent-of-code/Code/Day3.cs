using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
  class Day3
  {
    public static void Main(string[] args)
    {
      var text = File.ReadAllText(@"Datasets\Day3.txt");
      Part1(text);
      Part2(text);
    }

    private static void Part1(string input)
    {
      const string pattern = @"mul\((\d{1,3}),(\d{1,3})\)";
      var regex = new Regex(pattern);

      // Find matches
      var matches = regex.Matches(input);

      // Iterate over the matches
      var sum = 0;
      foreach (Match match in matches)
      {
        // Extract the whole match and groups
        var x = match.Groups[1].Value; // Group 1 (X)
        var y = match.Groups[2].Value; // Group 2 (Y)
        // Output the results
        //Console.WriteLine($"Found: {match.Value} (X = {x}, Y = {y})");
        sum += int.Parse(x) * int.Parse(y);
      }
      Console.WriteLine($"Sum: {sum}");
    }

    private static void Part2(string input)
    {
      const string pattern = @"mul\((\d{1,3}),(\d{1,3})\)";
      var regex = new Regex(pattern);
      var regex2 = new Regex(@"do\(\)|don't\(\)");
      // Find matches
      var instructions = regex.Matches(input);
      var doAndDonts = regex2.Matches(input);

      // Iterate over the matches
      var sum = 0;
      var doIndex = 0;
      var dontIndex = doAndDonts.FirstOrDefault(match => match.Value == "don't()")!.Index;
      foreach (Match instruction in instructions)
      {
        // Extract the whole match and groups
        var position = instruction.Index;
        var x = instruction.Groups[1].Value; // Group 1 (X)
        var y = instruction.Groups[2].Value; // Group 2 (Y)
        // Output the results
        // Console.WriteLine($"Found: {instruction.Value} (X = {x}, Y = {y}) Index: {position}");

        if (position > dontIndex)
        {
          doIndex = doAndDonts.FirstOrDefault(match => match.Value == "do()" && match.Index > dontIndex)!.Index;
          dontIndex = doAndDonts.FirstOrDefault(match => match.Value == "don't()" && match.Index > dontIndex)?.Index ?? int.MaxValue;
        }
        // Console.WriteLine($"Do: {doIndex}, Don't: {dontIndex}");
        if (doIndex < position && (position < dontIndex))
        {
          sum += int.Parse(x) * int.Parse(y);
        }
      }

      Console.WriteLine($"Sum: {sum}");
    }
  }
}