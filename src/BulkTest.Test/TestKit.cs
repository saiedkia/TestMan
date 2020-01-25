using System;

namespace BulkTest.Test
{
    internal class TestKit
    {
        public static string ReadFile()
        {
            return System.IO.File.ReadAllText(Environment.CurrentDirectory + "\\command.txt");
        }
    }
}
