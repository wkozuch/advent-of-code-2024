using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode;

class Day13
{
  public static void Main(string[] args)
  {
    var lines = File.ReadAllLines(@"Datasets\Day13.txt");
    Part1(lines);
  }

  private static void Part1(string[] lines)
  {
    var machines = ReadMachinesFromFile(lines);
    var count = 0;
    foreach (var m in machines)
    {
      count += FindSolution(m.ButtonA.X, m.ButtonA.Y, m.Prize.X, m.ButtonB.X, m.ButtonB.Y, m.Prize.Y, 0, 100);
    }
    Console.WriteLine($"Part 1: {count}");
  }

  //private static void Part2(string[] lines)
  //{
  //  var machines = ReadMachinesFromFile(lines, 10000000000000);
  //  var count = 0d;
  //  foreach (var m in machines)
  //  {
  //    Console.WriteLine(m);
  //   count+= FindSolution2(m.ButtonA.X, m.ButtonA.Y, m.Prize.X, m.ButtonB.X, m.ButtonB.Y, m.Prize.Y);
  //  }
  //  Console.WriteLine($"Part 2: {count}");
  //}

  public record Button(string Name, int X, int Y);
  public record Prize(long X, long Y);

  public class Machine
  {
    public Button ButtonA { get; set; }
    public Button ButtonB { get; set; }
    public Prize Prize { get; set; }

    public override string ToString()
    {
      return $"ButtonA: {ButtonA}, ButtonB: {ButtonB}, Prize: {Prize}";
    }
  }

  public static int FindSolution(int X1, int Y1, long Z1, int X2, int Y2, long Z2, int min, int max)
  {
    var solutions = new List<(int, int)>();

    // Solve for one variable in terms of the other
    for (var a = min; a <= max; a++)
    {
      for (var b = min; b <= max; b++)
      {
        // Check if both equations are satisfied
        if ((a * X1 + b * X2 == Z1) && (a * Y1 + b * Y2 == Z2))
        {
          solutions.Add((a, b));
        }
      }
    }

    return solutions.Any() ? solutions.Min(x => 3 * x.Item1 + x.Item2) : 0;
  }

  //public static long FindSolution2(int A_x, int A_y, long X_prize, int B_x, int B_y, long Y_prize)
  //{
  //  // Calculate the determinant of the coefficient matrix
  //  var det = Determinant(A_x, B_x, A_y, B_y);

  //  if (det == 0)
  //  {
  //    // No solution if determinant is 0
  //    return 0;
  //  }

  //  // Calculate a using the determinant of the matrix where the prize values replace the first column
  //  var a = Determinant(X_prize, B_x, Y_prize, B_y) / det;

  //  // Calculate b using the determinant of the matrix where the prize values replace the second column
  //  var b = Determinant(A_x, X_prize, A_y, Y_prize) / det;

  //  Console.WriteLine($"a: {a}, b: {b}");
  //  return a * 3 + b;
  //}

  //public static (long, long, long) ExtendedEuclid(long a, long b)
  //{
  //  long old_r = a, r = b;
  //  long old_s = 1, s = 0;
  //  long old_t = 0, t = 1;

  //  while (r != 0)
  //  {
  //    long quotient = old_r / r;
  //    long temp = r;
  //    r = old_r - quotient * r;
  //    old_r = temp;

  //    temp = s;
  //    s = old_s - quotient * s;
  //    old_s = temp;

  //    temp = t;
  //    t = old_t - quotient * t;
  //    old_t = temp;
  //  }

  //  return (old_r, old_s, old_t);
  //}

  //public static void SolveDiophantine(long a, long b, long c, out long x, out long y)
  //{
  //  long gcd = ExtendedGCD(a, b, out x, out y);
  //  if (c % gcd != 0)
  //  {
  //    throw new InvalidOperationException("No solution exists");
  //  }

  //  x *= c / gcd;
  //  y *= c / gcd;
  //}

  //public static long GCD(long a, long b)
  //{
  //  return b == 0 ? Math.Abs(a) : GCD(b, a % b);
  //}

  //public static long ExtendedGCD(long a, long b, out long x, out long y)
  //{
  //  if (b == 0)
  //  {
  //    x = 1;
  //    y = 0;
  //    return a;
  //  }

  //  long gcd = ExtendedGCD(b, a % b, out long x1, out long y1);
  //  x = y1;
  //  y = x1 - (a / b) * y1;
  //  return gcd;
  //}

  //public static long Determinant(long A, long B, long C, long D)
  //{
  //  return A * D - B * C;
  //}

  static List<Machine> ReadMachinesFromFile(string[] lines, long scaling = 0)
  {
    string buttonPattern = @"^(Button\s(\w+):)\s*(X([+-]?\d+)),\s*Y([+-]?\d+)$";
    string prizePattern = @"^(Prize:\s*)X=(\d+),\s*Y=(\d+)$";

    List<Machine> machines = new List<Machine>();

    for (var i = 0; i < lines.Length; i++)
    {
      var machine = new Machine();

      var match = Regex.Match(lines[i++], buttonPattern);

      if (match.Success)
      {
        string name = match.Groups[2].Value;
        int x = int.Parse(match.Groups[4].Value);
        int y = int.Parse(match.Groups[5].Value);

        machine.ButtonA = new Button(name, x, y);
      }

      match = Regex.Match(lines[i++], @"^(Button\s(\w+):)\s*(X([+-]?\d+)),\s*Y([+-]?\d+)$");

      if (match.Success)
      {
        string name = match.Groups[2].Value;
        int x = int.Parse(match.Groups[4].Value);
        int y = int.Parse(match.Groups[5].Value);

        machine.ButtonB = new Button(name, x, y);
      }

      var prizeMatch = Regex.Match(lines[i++], prizePattern);
      if (prizeMatch.Success)
      {
        var x = long.Parse(prizeMatch.Groups[2].Value);
        var y = long.Parse(prizeMatch.Groups[3].Value);

        machine.Prize = new Prize(x + scaling, y + scaling);
      }
      machines.Add(machine);
    }

    return machines;
  }

}