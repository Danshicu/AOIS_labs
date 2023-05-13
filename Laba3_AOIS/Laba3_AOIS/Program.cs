namespace Laba3_AOIS
{

    internal class Program
    {
        static void Main()
        {
            string expression = "(A+((!B*C)*!C)+B)";
            TableCreator table = new TableCreator(expression);
            table.MakeTable();
        }
    }
}