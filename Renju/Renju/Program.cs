using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace RenjuAnalyzer
{
    class Program
    {
        const int Size = 19;
        // static int Size = 19; const тільки читається, отже безпечніше
       

        static void Main()
        {
            string[] allLines = File.ReadAllLines("input.txt");
            int testCount = int.Parse(allLines[0]);
            int index = 1;

            File.WriteAllText("output.txt", "");

            if (testCount < 1 || testCount > 11)
            {
                using StreamWriter errorWriter = new StreamWriter("output.txt", append: true);
                errorWriter.WriteLine("Error: Invalid number of test cases - out of bounds.");
                return; 
            }

            int requiredLines = Size * testCount + 1; 

            if (allLines.Length < requiredLines)
            {
                using StreamWriter errorWriter = new StreamWriter("output.txt", append: true);
                errorWriter.WriteLine("Error: Invalid number of test cases - there are 1 or more extra rows");
                return;
            }


            using StreamWriter writer = new StreamWriter("output.txt");
            //  StreamWriter writer = new StreamWriter("output.txt");
            // з using автоматично закриє файл навіть при помилках

            for (int t = 0; t < testCount; t++)
            {
                int[,] board = ParseBoard(allLines, index);
                // цикл зчитування поля винесено з Main
                // розділення відповідальності

                index += Size;

                var result = CheckWinner(board);
                // було теж у Main
                // CheckWinner винесено для читабельності

                writer.WriteLine(result.Item1);

                if (result.Item1 != 0 && result.Item2 is { } point)
                {
                    writer.WriteLine($"{point.row} {point.col}");
                }
                // if (result.Item1 != 0 && result.Item2 != null)
                //{
                //    var point = result.Item2.Value;
                //    writer.WriteLine(point.row + " " + point.col);
                //}
                // уникнення повторного доступу до result.Item2.Value
            }
        }

        static int[,] ParseBoard(string[] lines, int startIndex)
        {
            int[,] board = new int[Size, Size];

            for (int row = 0; row < Size; row++)
            {


                int[] nums = lines[startIndex + row]
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .ToArray();

                
                if (nums.Length != Size)
                {
                    throw new Exception("Invalid row length.");
                }
                //string line = allLines[index];

                //string[] parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                //int[] nums = parts.Select(int.Parse).ToArray();

                // line і parts були зайві — змінні використовувались лише раз. 

                for (int col = 0; col < Size; col++)
                {
                    board[row, col] = nums[col];
                }
            }

            return board;
        }

        static (int, (int row, int col)?) CheckWinner(int[,] board)
        {
            var directions = new (int dx, int dy)[]
            {
                (0, 1),
                (1, 0),
                (1, 1),
                (-1, 1)
            };

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    int color = board[i, j];
                    if (color == 0) continue;

                    foreach (var (dx, dy) in directions)
                    {
                        int count = 1, x = i, y = j;

                        while (true)
                        {
                            x += dx;
                            y += dy;

                            if (x < 0 || x >= Size || y < 0 || y >= Size || board[x, y] != color)
                                break;

                            count++;
                        }

                        if (count == 5)
                        {
                            int beforeX = i - dx;
                            int beforeY = j - dy;
                            int afterX = i + dx * 5;
                            int afterY = j + dy * 5;

                            bool hasBefore = IsSameColor(beforeX, beforeY, board, color);
                            bool hasAfter = IsSameColor(afterX, afterY, board, color);
                                
                            // вкладені if() для before/after
                            // спрощено в одну перевірку уникнення дублювання та вкладеності

                            if (!hasBefore && !hasAfter)
                            {
                                int resRow = dx == -1 && dy == 1 ? i - 4 + 1 : i + 1;
                                int resCol = dx == -1 && dy == 1 ? j + 4 + 1 : j + 1;

                                // була окрема гілка для діагоналі півд+зах->півн+сх
                                // зроблено умовне присвоєння одразу
                                

                                return (color, (resRow, resCol));
                            }
                        }
                    }
                }
            }

            return (0, null);
        }

        static bool IsSameColor(int x, int y, int[,] board, int color)
        {
            return x >= 0 && x < Size && y >= 0 && y < Size && board[x, y] == color;

            // створено новий метод для перевірки 
            // використовувалась двічі в CheckWinner, тому винесено з Main
        }
    }
}
