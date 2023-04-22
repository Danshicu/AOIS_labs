namespace Laba2_AOIS
{
    public class SdnfHandler
    {
        private string _sdnf;
        private string[]? _expressions = null;
        private static readonly List<List<string>> AllVars = new List<List<string>>();
        private static List<int> _gluedNumbers = new List<int>();

        public SdnfHandler()
        {
            
        }

        public void SetExpression (string str)
        {
            _sdnf = str;
            if (IsCorrect())
            {
                _expressions = _sdnf.Split('V');
                for (int i = 0; i < _expressions.Length; i++)
                {
                    _expressions[i] = _expressions[i].Remove(_expressions[i].Length - 1,1);
                    _expressions[i] = _expressions[i].Remove(0,1);
                }
            };
            SetVariables();
            string calculation = MinimizeWithCalculation();
            Console.WriteLine($"Minimized SDNF with calculation method: {calculation}");
        }

        private void SetVariables()
        {
            if (IsCorrect())
            {
                foreach (var expression in _expressions)
                { 
                    string[] vars = expression.Split('&');
                    List<string> variables = vars.ToList();
                    AllVars.Add(variables);
                }
            } 
        }

        private string MinimizeWithCalculation()
        {
            string result = null;
            if (IsCorrect())
            {
                for (int firstIndex = 0; firstIndex < _expressions.Length - 1; firstIndex++)
                {
                    for (int secondIndex = firstIndex + 1; secondIndex < _expressions.Length; secondIndex++)
                    {
                        string subResult = MakeGlue(firstIndex, secondIndex);
                        result += subResult;
                        if (subResult != string.Empty)
                        {
                            result += "V";
                            if (!_gluedNumbers.Contains(secondIndex)) _gluedNumbers.Add(secondIndex);
                            if (!_gluedNumbers.Contains(firstIndex)) _gluedNumbers.Add(firstIndex);
                        }
                    }
                }

                if (result.Length > 0) result = result.Remove(result.Length - 1);
                for (int i = 0; i < _expressions.Length; i++)
                {
                    if (!_gluedNumbers.Contains(i))
                    {
                        result += _expressions[i];
                    }
                }
            }
            
            LogicalCalculator calculator = new LogicalCalculator(result, 1);
            string res = calculator.Calculate();
            return res;
            
        }

        private bool IsCorrect()
        {
            if (string.IsNullOrEmpty(_sdnf))
            {
                Console.WriteLine("SDNF is empty, can't minimize");
                return false;
            }

            return true;
        }

        private void ShowStrings()
        {
            foreach (var expression in _expressions)
            {
                Console.WriteLine(expression);
            }


            foreach (var varsSet in AllVars)
            {
                foreach (var var in varsSet)
                {
                    Console.WriteLine(var);
                }
            }
        }

        private string MakeGlue(int first, int second)
        {
            if (IsCorrect())
            {
                int DifferentValues = 0;
                string result = "(";
                List<string> commonVars = new List<string>();
                List<string> firstVarsSet = AllVars[first];
                List<string> secondVarsSet = AllVars[second];
                int countOfVars = AllVars[first].Count;
                for (int i = 0; i < countOfVars; i++)
                {
                    if (firstVarsSet[i] == secondVarsSet[i])
                    {
                        commonVars.Add(firstVarsSet[i]);
                    }
                    else
                    {
                        if (firstVarsSet[i] == Inversed(secondVarsSet[i]) ||
                            Inversed(firstVarsSet[i]) == secondVarsSet[i])
                        {
                            DifferentValues++;
                        }
                    }
                    
                }

                if (commonVars.Count < countOfVars - 1)
                {
                    return string.Empty;
                }

                if (DifferentValues > 1)
                {
                    return String.Empty;
                }

                foreach (var variable in commonVars)
                {
                    result += $"{variable}&";
                }

                result = result!.Remove(result.Length - 1, 1);
                result += ")";
                return result;
            }
            else return string.Empty;
        }

        private string Inversed(string value)
        {
            if (value[0] != '!')
            {
                return $"!{value}";
            }
            else
            {
                value = value.Remove(0, 1);
                return value;
            }
        }
    }
}