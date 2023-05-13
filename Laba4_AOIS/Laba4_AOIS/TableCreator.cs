namespace Laba3_AOIS
{
    public class TableCreator
    {
        private string? _sdnf;
        private string _sdnfVector = "()";
        private string? _sknf;
        private string _sknfVector = "()";
        private readonly List<int> _results = new List<int>();
        private List<string> _variables = new List<string>();
        readonly ExpressionHandler _expressionHandler;
        SknfHandler _sknfHandler = new SknfHandler();
        SdnfHandler _sdnfHandler = new SdnfHandler();
        
        
        public TableCreator(string? expression)
        {
            _expressionHandler = new ExpressionHandler(expression);
            
        }

        public void MakeTable()
        {
            for (int i = 0; i < Math.Pow(2, _expressionHandler.GetVariablesCount()); i++)
            {
                _expressionHandler.SetVariablesValuesWith(i);
                int result = _expressionHandler.CalculateExpression();
                switch (result)
                {
                    case 1:
                        _sdnf += _expressionHandler.ReturnSdnf();
                        _sdnf += "V";
                        _sdnfVector = _sdnfVector.Insert(_sdnfVector.Length-1, $"{i},");
                        _sdnfHandler.AddNumberSet(_expressionHandler.GetVariablesString());
                        break;
                    case 0:
                        _sknf += _expressionHandler.ReturnSknf();
                        _sknf += "&";
                        _sknfVector = _sknfVector.Insert(_sknfVector.Length-1, $"{i},");
                        _sknfHandler.AddNumberSet(_expressionHandler.GetVariablesString());
                        break;
                }

                _results.Add(result);
                Console.WriteLine($"{_expressionHandler.GetFunctionString()}{result}");
            }

            int functionVector = 0;
            for (int i = 0; i < _results.Count; i++)
            {
                functionVector += (int)Math.Pow(2, 7 - i) * _results[i];
            }
            Console.WriteLine($"Vector is: {functionVector}");
            
            if (_sknf.Length > 0)
            {
                _sknf = _sknf.Remove(_sknf.Length - 1);
                _sknfVector = _sknfVector.Remove(_sknfVector.Length - 2, 1);
            }
            
            if (_sdnf.Length > 0)
            {
                _sdnf = _sdnf.Remove(_sdnf.Length - 1);
                _sdnfVector = _sdnfVector.Remove(_sdnfVector.Length - 2,1);
            }
            Console.WriteLine($"SDNF: {_sdnf}  or vector  {_sdnfVector}");
            Console.WriteLine($"SKNF: {_sknf}  or vector  {_sknfVector}");
            _sdnfHandler.SetExpression(_sdnf);
            string? calculationSdnf = _sdnfHandler.MinimizeWithCalculation();
            Console.WriteLine($"Minimized SDNF with calculation method: {calculationSdnf}");
            string? mcCluskeySdnf = _sdnfHandler.MinimizeWithMcCluskeyMethod();
            Console.WriteLine($"Minimized SDNF with McCluskey method: {mcCluskeySdnf}");
            _sknfHandler.SetExpression(_sknf);
            string? calculationSknf = _sknfHandler.MinimizeWithCalculation();
            Console.WriteLine($"Minimized SKNF with calculation method: {calculationSknf}");
            string? mcCluskeySknf = _sknfHandler.MinimizeWithMcCluskeyMethod();
            Console.WriteLine($"Minimized SKNF with McCluskey method: {mcCluskeySknf}");
            BuildKarnaughKart();
            string? karnaughSdnf = _sdnfHandler.MinimizeWithKarnaugh();
            Console.WriteLine($"Minimized SDNF with Karnaugh method: {karnaughSdnf}");
            string? karnaughSknf = _sknfHandler.MinimizeWithKarnaugh();
            Console.WriteLine($"Minimized SKNF with Karnaugh method: {karnaughSknf}");
        }

        private void BuildKarnaughKart()
        {
            _variables = _expressionHandler.GetVariables();
            Console.WriteLine("Carnaugh Kart:");
            Console.WriteLine($"{_variables[1]}{_variables[2]}   00   01   11   10");
            Console.WriteLine($"  1   {_results[4]}    {_results[5]}    {_results[7]}    {_results[6]}");
            Console.WriteLine($"{_variables[0]} ");
            Console.WriteLine($"  0   {_results[0]}    {_results[1]}    {_results[3]}    {_results[2]}");
        }
            
    }
}