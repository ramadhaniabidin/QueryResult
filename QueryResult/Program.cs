using QueryResult.Controller;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        MainAction action = new MainAction();
        string tableName = "Sambu_Staging_RSUP.mst.item";
        var lists = action.GetCategories(tableName);
        //int i = 1;
        bool isExists = false;
        string message = "";
        //foreach (var category in lists)
        //{
        //    isExists = action.CheckDataExists("Sambu_Nintex.mst.item_category", "name", category.CategoryName);
        //    message = (isExists == true) ? "Sudah ada di database" : "Belum ada di database";
        //    Console.WriteLine($"Kategori ke {i} : {category.CategoryName} {message}");
        //    i++;
        //}

        string listName = "Master Item Category";
        //Console.WriteLine($"Table Name : {action.GetDBTableName(listName)}");
        //Console.WriteLine($"Staging Table Name : {action.GetStagingTableName(listName)}");
        action.TestingQuery(listName);
        //var stagingColumns = action.GetStagingColumns(listName);
        //Console.WriteLine(string.Join(",", stagingColumns));

        //List<string> myList = new List<string>() { "Dhani", "Dwi", "Winda" };
        //List<dynamic> myList1 = new List<dynamic>()
        //{
        //    new { Name = "Dhani", Gender = "Male" },
        //    new { Name = "Dwi", Gender = "Male" },
        //    new { Name = "Winda", Gender = "Female" },
        //};

        //for (int i = 0; i < myList.Count; i++)
        //{
        //    Console.WriteLine("Anak ke " + (i + 1) + " : " + myList[i]);
        //}

        //Console.WriteLine();

        //for (int j = 0; j < myList1.Count; j++)
        //{
        //    Console.WriteLine("Anak ke " + (j + 1) + " : " + myList1[j].Name + " " + myList1[j].Gender);
        //}










        //List<Dictionary<string, object>> listOfDictionaries = new List<Dictionary<string, object>>();

        //// Create a dictionary and add it to the list
        //Dictionary<string, object> dictionary1 = new Dictionary<string, object>();
        //dictionary1.Add("Name", "John");
        //dictionary1.Add("Age", 30);
        //listOfDictionaries.Add(dictionary1);

        //// Create another dictionary and add it to the list
        //Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
        //dictionary2.Add("Name", "Alice");
        //dictionary2.Add("Age", 25);
        //listOfDictionaries.Add(dictionary2);

        //// Access and print values from the list of dictionaries
        //foreach (var dictionary in listOfDictionaries)
        //{
        //    Console.Write("Name: " + dictionary["Name"] + ", ");
        //    Console.WriteLine("Age: " + dictionary["Age"]);
        //}

        //List<string> keys = new List<string>()
        //{
        //    "Name", "Age"
        //};

        //Dictionary<string, object> values = new();

        //List<Dictionary<string, object>> listOfDictionaries = new();
        //for(int i = 0; i < keys.Count; i++)
        //{
        //    values.Add(keys[i], keys[i]);
        //}
        //listOfDictionaries.Add(values);

        //foreach(var dic in listOfDictionaries)
        //{
        //    Console.Write("Name: " + dic["Name"] + ", ");
        //    Console.WriteLine("Age: " + dic["Age"]);
        //}
    }
}