using System;
using System.IO;
using System.Linq;

namespace RenjuAnalyzer
{
    class Program
    {
        static int Size = 19;

        static void Main()
        {

            string[] allLines = File.ReadAllLines("input.txt");
            int testCount = int.Parse(allLines[0]); 
            int index = 1;

  
            File.WriteAllText("output.txt", "");
            StreamWriter writer = new StreamWriter("output.txt");


            for (int t = 0; t < testCount; t++)
            {
                int[,] board = new int[Size, Size];

                for (int row = 0; row < Size; row++)
                {
                    string line = allLines[index];


                    string[] parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    int[] nums = parts.Select(int.Parse).ToArray();


                    for (int col = 0; col < Size; col++)
                    {
                        board[row, col] = nums[col];
                    }

                    index++;
                }

                var result = CheckWinner(board);

                writer.WriteLine(result.Item1);
                if (result.Item1 != 0 && result.Item2 != null)
                {
                    var point = result.Item2.Value;
                    writer.WriteLine(point.row + " " + point.col);
                }
            }

            writer.Close();

        }

        static (int, (int row, int col)?) CheckWinner(int[,] board)
        {
            var dirs = new (int dx, int dy)[]
            {
                (0, 1), // горизонталь
                (1, 0), // вертикаль
                (1, 1), // діагональ вниз вправо
                (-1, 1) // діагональ вгору вправо
            };

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    int color = board[i, j];
                    if (color == 0) continue;

                    foreach (var dir in dirs)
                    {
                        int dx = dir.dx;
                        int dy = dir.dy;

                        int count = 1;
                        int x = i;
                        int y = j;

                        while (true)
                        {
                            x = x + dx;
                            y = y + dy;

                            if (x < 0 || x >= Size || y < 0 || y >= Size) break;

                            if (board[x, y] == color)
                            {
                                count++;
                            }
                            else
                            {
                                break;
                            }
                        }

                        if (count == 5)
                        {
                            int beforeX = i - dx;
                            int beforeY = j - dy;

                            int afterX = i + dx * 5;
                            int afterY = j + dy * 5;

                            bool hasBefore = false;
                            bool hasAfter = false;

                            if (beforeX >= 0 && beforeX < Size && beforeY >= 0 && beforeY < Size)
                            {
                                if (board[beforeX, beforeY] == color)
                                {
                                    hasBefore = true;
                                }
                            }

                            if (afterX >= 0 && afterX < Size && afterY >= 0 && afterY < Size)
                            {
                                if (board[afterX, afterY] == color)
                                {
                                    hasAfter = true;
                                }
                            }

                            if (!hasBefore && !hasAfter)
                            {
                                int resRow = i + 1;
                                int resCol = j + 1;

                                if (dx == -1 && dy == 1)
                                {
                                    resRow = i - 4 + 1;
                                    resCol = j + 4 + 1;
                                }

                                return (color, (resRow, resCol));
                            }
                        }
                    }
                }
            }

            return (0, null);
        }
    }
}
