namespace Laba5_AOIS
{

    internal class Program
    {
        static void Main()
        {
            SubtractorHandler handler = new SubtractorHandler();
            handler.MakeSubtractorTable(new string[]
            {
                "I1", "I2", "I3",
            }, new string[]
            {
                "O1", "O2", "O3",
            },new string[]
            {
                "H1", "H2", "H3"
            },"S");
        }
    }
}