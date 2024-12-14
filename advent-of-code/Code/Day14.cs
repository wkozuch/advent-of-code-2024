using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode;

class Day14
{
  public static void Main(string[] args)
  {
    var lines = File.ReadAllLines(@"Datasets\Day14.txt");
    var robots = lines.Select(x => ParseRobot(x)).ToList();

    Part1(robots, 100);
    Part2(robots, 8000);
  }

  private static void Part1(IEnumerable<Robot> robots, int numBlinks)
  {
    var rows = 103; //103
    var columns = 101; //101
    var surface = Enumerable.Range(0, rows).Select(x => Enumerable.Range(0, columns).Select(y => 0).ToArray()).ToArray();

    foreach (var robot in robots)
    {
      surface[robot.Y][robot.X] += 1;
    }

    for (var i = 0; i < numBlinks; i++)
    {
      robots = BlinkOnce(robots, ref surface);
    }

    var quandrants = new Dictionary<char, int>() { { 'A', 0 }, { 'B', 0 }, { 'C', 0 }, { 'D', 0 } };
    for (var r = 0; r < surface.Length; r++)
    {
      for (var c = 0; c < surface[r].Length; c++)
      {
        if (r == (rows - 1) / 2 || c == (columns - 1) / 2) continue;
        if (r < rows / 2 && c < columns / 2) quandrants['A'] += surface[r][c];
        if (r < rows / 2 && c > columns / 2) quandrants['B'] += surface[r][c];
        if (r > rows / 2 && c < columns / 2) quandrants['C'] += surface[r][c];
        if (r > rows / 2 && c > columns / 2) quandrants['D'] += surface[r][c];
      }
    }

    Console.WriteLine($"Part 1: {quandrants.Values.Aggregate((product, number) => product * number)}"); // 236628054
  }

  private static void Part2(IEnumerable<Robot> robots, int numBlinks)
  {
    var rows = 103; //103
    var columns = 101; //101
    var surface = Enumerable.Range(0, rows).Select(x => Enumerable.Range(0, columns).Select(y => 0).ToArray()).ToArray();

    foreach (var robot in robots)
    {
      surface[robot.Y][robot.X] += 1;
    }

    var min = double.MaxValue;
    var treeIndex = 0;
    for (var i = 0; i < numBlinks; i++)
    {
      robots = BlinkOnce(robots, ref surface);
      var concentration = CalculateConcentration(surface);
      if (concentration < min)
      {
        min = concentration;
        treeIndex = i + 1;
      }
    }
    Console.WriteLine($"Part 2: {treeIndex}"); // 7584
  }

  private static List<Robot> BlinkOnce(IEnumerable<Robot> robots, ref int[][] surface)
  {
    var newRobots = new List<Robot>();

    foreach (var robot in robots)
    {
      surface[robot.Y][robot.X]--;
      var next = MoveRobot(robot, surface.Length, surface[0].Length);
      surface[next.Y][next.X]++;
      newRobots.Add(next);
    }

    return newRobots;
  }

  private static double CalculateConcentration(int[][] array)
  {
    var nonZeroPositions = GetNonZeroPositions(array);
    // Calculate the center of mass (average position)
    double avgX = nonZeroPositions.Average(p => p.Item1);
    double avgY = nonZeroPositions.Average(p => p.Item2);

    // Calculate the total distance from the center of mass
    double totalDistance = nonZeroPositions
        .Sum(p => Math.Sqrt(Math.Pow(p.Item1 - avgX, 2) + Math.Pow(p.Item2 - avgY, 2)));

    return totalDistance;
  }

  private static List<Tuple<int, int>> GetNonZeroPositions(int[][] array)
  {
    var positions = new List<Tuple<int, int>>();

    for (int i = 0; i < array.Length; i++)
    {
      for (int j = 0; j < array[0].Length; j++)
      {
        if (array[i][j] != 0)
        {
          positions.Add(Tuple.Create(i, j));
        }
      }
    }

    return positions;
  }

  private static Robot MoveRobot(Robot robot, int maxRow, int maxColum)
  {
    var nextX = (((robot.X + robot.VelocityX) % maxColum) + maxColum) % maxColum;
    var nextY = (((robot.Y + robot.VelocityY) % maxRow) + maxRow) % maxRow;
    return robot with { X = nextX, Y = nextY };
  }

  public record Robot(int X, int Y, int VelocityX, int VelocityY);

  public static Robot ParseRobot(string line)
  {
    var regex = new Regex(@"p=(\d+),(\d+)\s*v=(\-?\d+),(\-?\d+)");
    var match = regex.Match(line);

    int X = int.Parse(match.Groups[1].Value);
    int Y = int.Parse(match.Groups[2].Value);
    int Vx = int.Parse(match.Groups[3].Value);
    int Vy = int.Parse(match.Groups[4].Value);

    return new Robot(X, Y, Vx, Vy);
  }
}