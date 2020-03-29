using Id3;
using System;
using System.IO;

namespace Edit_Mp3Tags
{
    class Program
    {

        private static void EditFile(string fileName)
        {
            Console.WriteLine(fileName);
            var dados = Path.GetFileNameWithoutExtension(fileName);
            if (dados.IndexOf("-") == -1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("    Above file was ignored, isn't the match pattern Artist - Title.");
                Console.ResetColor();
                return;
            }

            using (var mp3 = new Mp3(fileName, Mp3Permissions.ReadWrite))
            {
                mp3.DeleteAllTags();
                var tag = new Id3Tag();
                var propinfo = typeof(Id3Tag).GetProperty("Version", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                propinfo.SetValue(tag, Id3Version.V23);
                tag.Artists.Value.Clear();
                tag.Artists.Value.Add(dados.Substring(0, dados.IndexOf("-") -1).Trim());
                tag.Title.Value = dados.Substring(dados.IndexOf("-") + 1, dados.Length - (dados.IndexOf("-") + 1)).Trim();
                mp3.WriteTag(tag, WriteConflictAction.Replace);
                
            }
        }
        
        static void Main(string[] args)
        {
            Console.WriteLine("This app read all mp3 files and alter the mp3 tags (artist and "
                + "title) using this pattern (artist - title.mp3) that shoud to be in file name.");
            Console.WriteLine(string.Empty);

            string dir = Environment.CurrentDirectory;
            if (args.Length == 0)
            {
                Console.WriteLine("Directory not especified, using the current directory.");
            }
            else
            {
                dir = args[0];
                Console.WriteLine("Working with the directory " + dir);
            }

            Console.WriteLine(string.Empty);

            var list = Directory.GetFiles(dir, "*.mp3");
            foreach (var item in list)
            {
                EditFile(item);
            }

            Console.WriteLine(string.Empty);
            Console.WriteLine($"{list.Length} files processed.");
        }

        
    }
}
