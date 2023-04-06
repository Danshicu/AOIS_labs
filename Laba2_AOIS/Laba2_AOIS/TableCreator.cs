namespace Laba2_AOIS
{
    public class TableCreator
    {
        private string SDNF;
        private string SDNFVector = "()";
        private string SKNF;
        private string SKNFVector = "()";
        private readonly List<int> results = new List<int>();
        readonly ExpressionHandler expressionHandler;
        
        
        public TableCreator(string? expression)
        {
            expressionHandler = new ExpressionHandler(expression);
        }

        public void MakeTable()
        {
            for (int i = 0; i < Math.Pow(2, expressionHandler.GetVariablesCount()); i++)
            {
                expressionHandler.SetVariablesValuesWith(i);
                int result = expressionHandler.CalculateExpression();
                switch (result)
                {
                    case 1:
                        SDNF += expressionHandler.ReturnSDNF();
                        SDNF += "V";
                        SDNFVector = SDNFVector.Insert(SDNFVector.Length-1, $"{i},");
                        break;
                    case 0:
                        SKNF += expressionHandler.ReturnSKNF();
                        SKNF += "&";
                        SKNFVector = SKNFVector.Insert(SKNFVector.Length-1, $"{i},");
                        break;
                }

                results.Add(result);
                Console.WriteLine($"{expressionHandler.GetFunctionString()}{result}");
            }

            int functionVector = 0;
            for (int i = 0; i < results.Count; i++)
            {
                functionVector += (int)Math.Pow(2, 7 - i) * results[i];
            }
            Console.WriteLine($"Vector is: {functionVector}");
            
            if (SKNF.Length > 0)
            {
                SKNF = SKNF.Remove(SKNF.Length - 1);
                SKNFVector = SKNFVector.Remove(SKNFVector.Length - 2, 1);
            }
            
            if (SDNF.Length > 0)
            {
                SDNF = SDNF.Remove(SDNF.Length - 1);
                SDNFVector = SDNFVector.Remove(SDNFVector.Length - 2,1);
            }
            Console.WriteLine($"SDNF: {SDNF}  or vector  {SDNFVector}");
            Console.WriteLine($"SKNF: {SKNF}  or vector  {SKNFVector}");
        }
        
        
            
    }
}