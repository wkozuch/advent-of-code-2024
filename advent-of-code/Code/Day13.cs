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
    // Part1(lines);
    Part2(lines);
  }

  private static void Part1(string[] lines)
  {
    var machines = ReadMachinesFromFile(lines);
    var count = 0;
    foreach (var m in machines)
    {
      count += FindSolution(m.ButtonA.X, m.ButtonA.Y, m.Prize.X, m.ButtonB.X, m.ButtonB.Y, m.Prize.Y, 0, 100);
    }
    Console.WriteLine($"Part 1: {count}"); // 29517
  }

  private static void Part2(string[] lines)
  {
    var machines = ReadMachinesFromFile(lines, 10000000000000); //10000000000000
    var count = 0d;
    foreach (var m in machines)
    {
      Console.WriteLine(m);
      count += FindSolution2(m.ButtonA.X, m.ButtonA.Y, m.Prize.X, m.ButtonB.X, m.ButtonB.Y, m.Prize.Y);
      Console.WriteLine();
    }
    Console.WriteLine($"Part 2: {count}");
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

  public static double FindSolution2(int a1, int a2, long c1, int b1, int b2, long c2)
  {
    Console.WriteLine($"a*{a1} + b*{b1} = {c1} a*{a2} + b*{b2} = {c2}");

    var det = Determinant(a1, a2, b1, b2);
    if (det == 0)
    {
      Console.WriteLine("No unique solution exists.");
      return 0;
    }

    // Apply Cramer's rule to find the particular solutions for a and b
    var a = (c1 * b2 - c2 * b1) / det;
    var b = (a1 * c2 - a2 * c1) / det;

    // Check if both a and b are integers
    if ((c1 * b2 - c2 * b1) % det != 0 || (a1 * c2 - a2 * c1) % det != 0)
    {
      Console.WriteLine("No integer solution found.");
      return 0;
    }

    // Compute GCD for both equations
    if (!IsValidSolution(a1, b1, c1) || !IsValidSolution(a2, b2, c2))
    {
      return 0;
    }

    Console.WriteLine($"Particular solution: a = {a}, b = {b}");

    // Step 2: Find particular solutions using Extended Euclidean Algorithm
    var particularA1 = ExtendedGCD(a1, b1, c1);
    var particularA2 = ExtendedGCD(a2, b2, c2);

    Console.WriteLine($"Particular solution for the first equation: a = {particularA1.a}, b = {particularA1.b}");
    Console.WriteLine($"Particular solution for the second equation: a = {particularA2.a}, b = {particularA2.b}");

    // Step 5: Find the general solution and the optimal k value
    var (stepA, stepB) = GetStepSizes(a1, b1, a2, b2, det);
    Console.WriteLine($"StepA {stepA}, StepB {stepB}");

    // if (stepA == 0 || stepB == 0)
    // {
    //   Console.WriteLine("Step sizes are invalid (zero).");
    //   return 0;
    // }

    if (stepA == 0 || stepB == 0)
    {
      Console.WriteLine("Wrong");
    }

    // Adjust to ensure both a and b are positive
    long kMin = GetKMin(a, b, stepA, stepB);
    long kMax = GetKMax(a, b, stepA, stepB);

    Console.WriteLine($"kMin = {kMin}, kMax = {kMax}");

    if (kMin > kMax)
    {
      Console.WriteLine("No valid k range available, solution cannot be found.");
      return 0;
    }

    // Evaluate the objective function at kMin and kMax
    long optimalSolution = EvaluateObjective(a, b, stepA, stepB, kMin, kMax);
    return optimalSolution;
  }

  public static long GCD(long a, long b)
  {
    return b == 0 ? Math.Abs(a) : GCD(b, a % b);
  }

  // Extended Euclidean algorithm to find coefficients of a and b
  static (long a, long b) ExtendedGCD(long a, long b, long c)
  {
    ExtendedGCD(a, b, out long x, out long y);
    long scale = c / GCD(a, b);
    return (x * scale, y * scale);
  }

  public static long ExtendedGCD(long a, long b, out long x, out long y)
  {
    if (b == 0)
    {
      x = 1;
      y = 0;
      return a;
    }

    long gcd = ExtendedGCD(b, a % b, out long x1, out long y1);
    x = y1;
    y = x1 - (a / b) * y1;
    return gcd;
  }

  public static long Determinant(long A, long B, long C, long D)
  {
    return A * D - B * C;
  }

  static bool IsValidSolution(int a, int b, long c)
  {
    long gcd = GCD(a, b);
    if (c % gcd != 0)
    {
      Console.WriteLine($"No solution exists for the equation a*{a} + b*{b} = {c}");
      return false;
    }
    return true;
  }

  // Get step sizes for the general solution
  static (long stepA, long stepB) GetStepSizes(int a1, int b1, int a2, int b2, long det)
  {
    long gcd = GCD(a1, b1);
    return (b2 / gcd, a2 / gcd);
  }

  // Calculate kMin based on positivity conditions
  static long GetKMin(long a, long b, long stepA, long stepB)
  {
    long kMinA = (a <= 0) ? (-(a) + stepA - 1) / stepA : 0;
    long kMinB = (b <= 0) ? (b - 1) / -stepB : 0;
    return Math.Max(kMinA, kMinB);
  }

  // Calculate kMax based on positivity conditions
  static long GetKMax(long a, long b, long stepA, long stepB)
  {
    long kMaxA = stepA != 0 ? a / stepA : a;
    long kMaxB = stepB != 0 ? b / stepB : b;
    return Math.Min(kMaxA, kMaxB);
  }

  // Evaluate the objective function for given k values
  static long EvaluateObjective(long a, long b, long stepA, long stepB, long kMin, long kMax)
  {
    long optimalAAtKMin = a + kMin * stepA;
    long optimalBAtKMin = b - kMin * stepB;
    long objectiveAtKMin = 3 * optimalAAtKMin + optimalBAtKMin;

    long optimalAAtKMax = a + kMax * stepA;
    long optimalBAtKMax = b - kMax * stepB;
    long objectiveAtKMax = 3 * optimalAAtKMax + optimalBAtKMax;

    // Choose the smaller objective value
    if (objectiveAtKMin < objectiveAtKMax)
    {
      Console.WriteLine($"Optimal solution: a = {optimalAAtKMin}, b = {optimalBAtKMin}, 3a + b = {objectiveAtKMin}");
      return objectiveAtKMin;
    }
    else
    {
      Console.WriteLine($"Optimal solution: a = {optimalAAtKMax}, b = {optimalBAtKMax}, 3a + b = {objectiveAtKMax}");
      return objectiveAtKMax;
    }
  }

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

}