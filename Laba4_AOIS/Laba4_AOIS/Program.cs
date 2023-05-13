using Laba3_AOIS;
namespace Laba4_AOIS
{

    internal class Program
    {
        static void Main()
        {
            SummatorsHandlers handler = new SummatorsHandlers();
            handler.MakeSummatorTable(new char[]
            {
                'A', 'B', 'P'
            });
            handler.MakeD8421Table(new string[]
            {
                "X1", "X2", "X3", "X4"
            }, new string[]
            {
                "Y1", "Y2", "Y3", "Y4"
            });
            // string expression = "(A+((!B*C)*!C)+B)";
            // TableCreator table = new TableCreator(expression);
            // table.MakeTable();
        }
    }
}