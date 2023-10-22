using QueryResult.Controller;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        MainAction action = new MainAction();
        SyncDataController syncController = new SyncDataController();
        TestingController testingController = new TestingController();

        string tableName = "Sambu_Nintex.Mst.item_sub_category";
        string listName = syncController.GetSPListName(tableName);

        //syncController.CheckSPList(tableName);
        //syncController.GetDataFromStaging(tableName);
        testingController.TestAction();


        //action.TestingQuery(listName);


    }
}