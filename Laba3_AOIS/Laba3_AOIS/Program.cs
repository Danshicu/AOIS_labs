namespace Laba2_AOIS
{

    internal class Program
    {
        static void Main(string[] args)
        {
            string? expression = "!((!A+B)*(!(A)+C))";
            TableCreator table = new TableCreator(expression);
            table.MakeTable();
        }
    }
}