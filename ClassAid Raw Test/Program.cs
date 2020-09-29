using System;
using System.Threading.Tasks;

namespace ClassAid_Raw_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            LocalStorageEngine.SaveDataAsync("Hello", FileType.AuthFile);

            string res = null;
            res = LocalStorageEngine.ReadDataAsync<string>(FileType.AuthFile);
            Console.WriteLine("Answer: " + res);
        }
    }
}
//Hello World