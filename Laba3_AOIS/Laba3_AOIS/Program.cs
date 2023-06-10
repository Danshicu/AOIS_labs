namespace Laba3_AOIS
{

    internal class Program
    {
        static void Main()
        {
            string expression = "(!(a+b)+c)";
            TableCreator table = new TableCreator(expression);
            table.MakeTable();
        }
    }
}