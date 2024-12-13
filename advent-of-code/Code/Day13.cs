using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text.RegularExpressions;

namespace AdventOfCode;

class Day13
{
  public static void Main(string[] args)
  {
    var lines = File.ReadAllLines(@"Datasets\Day13.txt");
    var machines = ReadMachinesFromFile(lines);
    foreach (var m in machines)
    {
      Console.WriteLine(m);
    }

  }

  public record Button(string Name, int X, int Y);
  public record Prize(int X, int Y);

  public class Machine
  {
    public Button ButtonA { get; set; }
    public Button ButtonB { get; set; }
    public Prize Prize { get; set; }
  }

  static List<Machine> ReadMachinesFromFile(string[] lines)
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
        int x = int.Parse(prizeMatch.Groups[2].Value);
        int y = int.Parse(prizeMatch.Groups[3].Value);

        machine.Prize = new Prize(x, y);
      }
      machines.Add(machine);
    }

    return machines;
  }

}