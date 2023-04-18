namespace Laba2_AOIS
{
    public class SknfHandler
    {
        private readonly string _sknf;
        private readonly string[]? _expressions = null;
        private static readonly List<List<string>> AllVars = new List<List<string>>();
        private static List<int> _gluedNumbers = new List<int>();

        public SknfHandler(string str)
        {
            _sknf = str;
            if (IsCorrect())
            {
                _expressions = _sknf.Split('&');
                for (int i = 0; i < _expressions.Length; i++)
                {
                    _expressions[i] = _expressions[i].Remove(_expressions[i].Length - 1,1);
                    _expressions[i] = _expressions[i].Remove(0,1);
                }
            };
            SetVariables();
            MinimizeWithCalculation();
            //ShowStrings();
        }

        private void SetVariables()
        {
            if (IsCorrect())
            {
                foreach (var expression in _expressions)
                { 
                    string[] vars = expression.Split('V');
                    List<string> variables = vars.ToList();
                    AllVars.Add(variables);
                }
            } 
        }

        private void MinimizeWithCalculation()
        {
            if (IsCorrect())
            {
                string result = null;
                for (int firstIndex = 0; firstIndex < _expressions.Length - 1; firstIndex++)
                {
                    for (int secondIndex = firstIndex + 1; secondIndex < _expressions.Length; secondIndex++)
                    {
                        string subResult = MakeGlue(firstIndex, secondIndex);
                        result += subResult;
                        if (subResult != string.Empty)
                        {
                            result += "&";
                            if (!_gluedNumbers.Contains(secondIndex)) _gluedNumbers.Add(secondIndex);
                            if (!_gluedNumbers.Contains(firstIndex)) _gluedNumbers.Add(firstIndex);
                        }
                    }
                }

                if (result.Length > 0) result = result.Remove(result.Length - 1);
                Console.WriteLine(result);
            }
        }

        private bool IsCorrect()
        {
            if (string.IsNullOrEmpty(_sknf))
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
                }

                if (commonVars.Count < countOfVars - 1)
                {
                    return string.Empty;
                }

                foreach (var variable in commonVars)
                {
                    result += $"{variable}V";
                }

                result = result!.Remove(result.Length - 1, 1);
                result += ")";
                return result;
            }
            else return string.Empty;
        }
    }
}