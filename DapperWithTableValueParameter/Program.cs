using System;
using System.Collections.Generic;

namespace DapperWithTableValueParameter
{
    class Program
    {
        static void Main()
        {
            const string connectionString = "Server=localhost;Database=DapperWithTableValueParameter;Trusted_Connection=True;";

            var ints = new List<int>
            {
                1, 2, 3, 4, 5
            };
            var database = new Database(connectionString);
            var enumerable = database.GetInts(ints);

            Console.WriteLine(String.Join(", ", enumerable));
        }
    }
}
