using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;

/// <summary>
/// Консольное приложение для поиска слова в файлах в указанном каталоге.
/// </summary>
class Program
{
  /// <summary>
  /// Флаг захвата для определения прошёл ли поиск успешно.
  /// </summary>
  static bool flag = false;

  /// <summary>
  /// Блокирует запись только для одного потока.
  /// </summary>
  static object locker = new object();

  static void Main()
  {
    Console.WriteLine("Привет, красавчик ^-^\n");
    while (true)
    {
      string folder = "";
      try
      {
        Console.WriteLine("Введи путь к каталогу для поиска слова:");
        folder = Console.ReadLine();

        Console.Write("Введи слово - ");
        string word = Console.ReadLine();

        string[] files = Directory.GetFiles(folder, "*.txt");

        Thread[] threads = new Thread[files.Length];

        for (int i = 0; i < files.Length; i++)
        {
          int index = i;
          threads[i] = new Thread(() => SearchWord(files[index], word));
          threads[i].Start();
        }

        foreach (var thread in threads) thread.Join();

        if (flag)
        {
          break;
        }
        else
        {
          Console.WriteLine($"Не нашлось слово \"{word}\" в файлах директории \"{folder}\"\n");
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Данный каталог {folder} не существет или путь указан не верно.\n");
      }
    }
  }

  /// <summary>
  /// Ищет слово в указанном файле.
  /// </summary>
  /// <param name="file">Путь к файлу.</param>
  /// <param name="word">Ключевое слово поиска.</param>
  static void SearchWord(string file, string word)
  {
    string text = File.ReadAllText(file);

    if (text.Contains(word))
    {
      lock (locker)
      {
        flag = true;
        Console.WriteLine($"\nНайдено слово \"{word}\" в файле - {Path.GetFileName(file)}");
      }
    }
  }
}
