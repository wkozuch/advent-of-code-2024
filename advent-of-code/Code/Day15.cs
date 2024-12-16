using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;

namespace AdventOfCode;

class Day15
{
  public static void Main(string[] args)
  {
    var input = File.ReadAllText(@"Datasets\Day15.txt");
    //  Part1(input);
    Part2(input);
  }

  private static void Part1(string input)
  {
    input = input.Replace("\r\n", "\n").Replace("\r", "\n");
    var parts = input.Split(new[] { "\n\n" }, StringSplitOptions.None)
            .Select(part => part.Trim())
            .ToArray();
    var gridLines = parts[0].Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
    var surface = gridLines.Select(line => line.ToCharArray()).ToArray();
    var positions = surface.SelectMany((x, i) => x.Select((y, j) => new Position(i, j, y))).ToList();
    var instructions = parts[1].Trim();

    var robot = positions.Single(x => x.Char == '@');
    foreach (var instruction in instructions)
    {
      robot = MoveRobot(robot, instruction, surface);
      MoveBoxes(robot, instruction, surface);
      surface[robot.Row][robot.Column] = robot.Char;
      //Console.WriteLine(@$"Move {instruction}:");
      //surface.Draw();
    }

    var boxes = surface.SelectMany((r, i) => r.Select((c, j) => new Position(j, i, c))).ToList();
    var b = boxes.Where(x => x.Char == 'O').ToList();
    var count = b.Sum(x => x.Row * 100 + x.Column);
    Console.WriteLine("Part1: " + count);
  }

  private static void Part2(string input)
  {
    input = input.Replace("\r\n", "\n").Replace("\r", "\n");
    var parts = input.Split(new[] { "\n\n" }, StringSplitOptions.None)
            .Select(part => part.Trim())
            .ToArray();
    var gridLines = parts[0].Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
    var surface = gridLines.Select(line => line.ToCharArray()).ToArray();
    var extendedSurface = surface
            .SelectMany(line => ExpandLine(line)) // Expand vertically
            .ToArray();
    extendedSurface.Draw();

    var positions = extendedSurface.SelectMany((r, i) => r.Select((c, j) => new Position(i, j, c))).ToList();
    var robot = positions.Single(x => x.Char == '@');

    var instructions = parts[1].Trim();
    foreach (var instruction in instructions)
    {
      robot = MoveRobot(robot, instruction, extendedSurface);
      MoveBoxes2(robot, instruction, extendedSurface);
      extendedSurface[robot.Row][robot.Column] = robot.Char;
      Console.WriteLine(@$"Move {instruction}:");
      extendedSurface.Draw();
    }

    var boxes = extendedSurface.SelectMany((r, i) => r.Select((c, j) => new Position(j, i, c))).ToList();
    var b = boxes.Where(x => x.Char == '[').ToList();
    var count = b.Sum(x => x.Row * 100 + x.Column);
    Console.WriteLine("Part2: " + count);
  }

  private static Position MoveRobot(Position robot, char instruction, char[][] surface)
  {
    surface[robot.Row][robot.Column] = '.';
    if (instruction == '<' && !IsBlockedByTheWall(robot, instruction, surface)) return robot with { Column = robot.Column - 1 };
    if (instruction == '>' && !IsBlockedByTheWall(robot, instruction, surface)) return robot with { Column = robot.Column + 1 };
    if (instruction == 'v' && !IsBlockedByTheWall(robot, instruction, surface)) return robot with { Row = robot.Row + 1 };
    if (instruction == '^' && !IsBlockedByTheWall(robot, instruction, surface)) return robot with { Row = robot.Row - 1 };
    return robot;
  }

  private static void MoveBoxes(Position position, char direction, char[][] surface)
  {
    var i = 0;
    if (surface[position.Row][position.Column] != 'O') return;
    if (direction == 'v')
    {
      while (surface[position.Row + i][position.Column] != '.') i++;
      surface[position.Row + i][position.Column] = 'O';
    }
    if (direction == '^')
    {
      while (surface[position.Row - i][position.Column] != '.') i++;
      surface[position.Row - i][position.Column] = 'O';
    }
    if (direction == '<')
    {
      while (surface[position.Row][position.Column - i] != '.') i++;
      surface[position.Row][position.Column - i] = 'O';
    }
    if (direction == '>')
    {
      while (surface[position.Row][position.Column + i] != '.') i++;
      surface[position.Row][position.Column + i] = 'O';
    }
  }

  private static void MoveBoxes2(Position position, char direction, char[][] surface)
  {
    var i = 0;
    var colum = surface[position.Row][position.Column] == '[' ? position.Column : position.Column - 1;
    if (surface[position.Row][position.Column] != '[' && surface[position.Row][position.Column] != ']') return;
    if (direction == 'v')
    {
      while (surface[position.Row + i][position.Column] != '.') i++;
      surface[position.Row + i][colum] = '[';
      surface[position.Row + i][colum + 1] = ']';
    }
    if (direction == '^')
    {
      while (surface[position.Row - i][position.Column] != '.')
      {

        // surface[position.Row - i][colum] = '[';
        // surface[position.Row - i][colum + 1] = ']';
        i++;
      }
      PushUp(position, i, surface);
      // Push(position, direction, surface)
      // Push(position, i, surface);
      // surface[position.Row - i][colum] = '[';
      // surface[position.Row - i][colum + 1] = ']';
    }
    if (direction == '<')
    {
      while (surface[position.Row][position.Column - i] != '.')
      {
        surface[position.Row][position.Column - i] = surface[position.Row][position.Column - i] == '[' ? ']' : '[';
        i++;
      }
      surface[position.Row][position.Column - i] = surface[position.Row][position.Column - i] == '[' ? ']' : '[';
    }
    if (direction == '>')
    {
      while (surface[position.Row][position.Column + i] != '.')
      {
        surface[position.Row][position.Column + i] = surface[position.Row][position.Column + i] == '[' ? ']' : '[';
        i++;
      }
      surface[position.Row][position.Column + i] = surface[position.Row][position.Column + i] == '[' ? ']' : '[';
    }
  }

  private static bool IsBlockedByTheWall(Position position, char direction, char[][] surface)
  {
    if (direction == '<')
    {
      if (surface[position.Row][position.Column - 1] == '.') return false;
      if (surface[position.Row][position.Column - 1] == '#') return true;
      if (surface[position.Row][position.Column - 1] == 'O') return IsBlockedByTheWall(position with { Column = position.Column - 1 }, direction, surface);
      if (surface[position.Row][position.Column - 1] == '[') return IsBlockedByTheWall(position with { Column = position.Column - 1 }, direction, surface);
      if (surface[position.Row][position.Column - 1] == ']') return IsBlockedByTheWall(position with { Column = position.Column - 1 }, direction, surface);
    }
    if (direction == '>')
    {
      if (surface[position.Row][position.Column + 1] == '.') return false;
      if (surface[position.Row][position.Column + 1] == '#') return true;
      if (surface[position.Row][position.Column + 1] == 'O') return IsBlockedByTheWall(position with { Column = position.Column + 1 }, direction, surface);
      if (surface[position.Row][position.Column + 1] == '[') return IsBlockedByTheWall(position with { Column = position.Column + 1 }, direction, surface);
      if (surface[position.Row][position.Column + 1] == ']') return IsBlockedByTheWall(position with { Column = position.Column + 1 }, direction, surface);
    }
    if (direction == 'v')
    {
      if (surface[position.Row + 1][position.Column] == '.') return false;
      if (surface[position.Row + 1][position.Column] == '#') return true;
      if (surface[position.Row + 1][position.Column] == 'O') return IsBlockedByTheWall(position with { Row = position.Row + 1 }, direction, surface);
      if (surface[position.Row + 1][position.Column] == '[') return IsBlockedByTheWall(position with { Row = position.Row + 1 }, direction, surface) || IsBlockedByTheWall(position with { Row = position.Row + 1, Column = position.Column + 1 }, direction, surface);
      if (surface[position.Row + 1][position.Column] == ']') return IsBlockedByTheWall(position with { Row = position.Row + 1 }, direction, surface) || IsBlockedByTheWall(position with { Row = position.Row + 1, Column = position.Column - 1 }, direction, surface); ;
    }
    if (direction == '^')
    {
      if (surface[position.Row - 1][position.Column] == '.') return false;
      if (surface[position.Row - 1][position.Column] == '#') return true;
      if (surface[position.Row - 1][position.Column] == 'O') return IsBlockedByTheWall(position with { Row = position.Row - 1 }, direction, surface);
      if (surface[position.Row - 1][position.Column] == '[') return IsBlockedByTheWall(position with { Row = position.Row - 1 }, direction, surface) || IsBlockedByTheWall(position with { Row = position.Row - 1, Column = position.Column + 1 }, direction, surface); ;
      if (surface[position.Row - 1][position.Column] == ']') return IsBlockedByTheWall(position with { Row = position.Row - 1 }, direction, surface) || IsBlockedByTheWall(position with { Row = position.Row - 1, Column = position.Column - 1 }, direction, surface); ;
    }
    return true;
  }

  private static void PushUp(Position pusher, int rowCount, char[][] surface)
  {
    if (rowCount == 0)
    {
      surface[pusher.Row][pusher.Column] = pusher.Char;
      surface.Draw();
      return;
    }

    var pushed = pusher with { Row = pusher.Row - 1, Char = surface[pusher.Row][pusher.Column] };
    var pushed2 = pushed.Char == ']' ? pushed with { Column = pushed.Column - 1, Char = '[' } : pushed with { Column = pushed.Column + 1, Char = ']' };
    // surface[pushed.Row][pushed.Column] = surface[pusher.Row + 1][pushed.Column];
    // surface[pushed2.Row][pushed2.Column] = surface[pusher.Row + 1][pushed2.Column];
    surface.Draw();
    PushUp(pushed, rowCount - 1, surface);
    PushUp(pushed2, rowCount - 1, surface);
    surface[pusher.Row][pusher.Column] = surface[pusher.Row + 1][pusher.Column];
    surface[pusher.Row][pushed2.Column] = surface[pusher.Row + 1][pushed2.Column];
    // var left = surface[pusher.Row - 1][pusher.Column] == ']' ? pusher with { Row = pusher.Row - 1, Column = pusher.Column - 1, Char = '[' } : pusher with { Row = pusher.Row - 1, Char = ']' };
    // var right = surface[pusher.Row - 1][pusher.Column] == ']' ? pusher with { Row = pusher.Row - 1, Char = ']' } : pusher with { Row = pusher.Row - 1, Column = pusher.Column - 1, Char = '[' };
    // surface.Draw();
    // // left and right is the box to push 
    // // Push left and push right 

    // surface[left.Row][left.Column] = surface[pusher.Row][pusher.Column];
    // surface[right.Row][right.Column] = surface[pusher.Row][pusher.Column];
    // // surface[pusher.Row][pusher.Column] = surface[pusher.Row + 1][pusher.Column];
    // // surface[pusher.Row][pusher.Column - 1] = surface[pusher.Row + 1][pusher.Column - 1];

    // surface.Draw();
    // PushUp(left, rowCount - 1, surface);
    // PushUp(right, rowCount - 1, surface);
  }

  public record Position(int Row, int Column, char Char);

  static IEnumerable<char[]> ExpandLine(char[] line)
  {
    // Expand the line horizontally
    var expandedLine = line
        .SelectMany(c => Duplicate(c)) // Duplicate each character
        .ToArray();

    // Yield the line twice to expand vertically
    yield return expandedLine;
  }
  private static char[] Duplicate(char c)
  {
    return c switch
    {
      '#' => new[] { '#', '#' },
      'O' => new[] { '[', ']' },
      '.' => new[] { '.', '.' },
      '@' => new[] { '@', '.' },
      _ => new[] { '?', '?' },
    };
  }
}