using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

public class UniqueAddressSorter
{
    public static async Task Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Использование: dotnet run <путь_к_входному_файлу> <путь_к_выходному_файлу>");
            return;
        }

        string inputFile = args[0];
        string outputFile = args[1];

        if (!File.Exists(inputFile))
        {
            Console.WriteLine($"Ошибка: Входной файл не найден по пути: {inputFile}");
            return;
        }

        try
        {
            // Читаем все строки из файла в память. 
            // Для файлов с адресами это безопасно, т.к. они не должны быть гигантскими.
            string[] allLines = await File.ReadAllLinesAsync(inputFile);

            Console.WriteLine($"Прочитано {allLines.Length} строк из файла.");

            // Используем HashSet для мгновенного получения уникальных строк.
            // StringComparer.OrdinalIgnoreCase делает сравнение нечувствительным к регистру (udp и UDP будут считаться одинаковыми).
            var uniqueLines = new HashSet<string>(allLines, StringComparer.OrdinalIgnoreCase);

            Console.WriteLine($"Найдено {uniqueLines.Count} уникальных адресов.");

            // Используем LINQ для сортировки уникальных строк.
            // StringComparer.Ordinal обеспечивает быструю и точную сортировку.
            var sortedUniqueLines = uniqueLines.OrderBy(line => line, StringComparer.Ordinal).ToList();

            Console.WriteLine("Сортировка завершена. Запись в файл...");

            // Записываем отсортированный и уникальный список в новый файл.
            await File.WriteAllLinesAsync(outputFile, sortedUniqueLines);

            Console.WriteLine($"Готово! Результат сохранен в файле: {outputFile}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }
    }
}