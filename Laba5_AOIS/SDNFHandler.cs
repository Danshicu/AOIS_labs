using System.Data;

namespace Laba5_AOIS
{
    public class SdnfHandler
    {
        private string? _sdnf;
        private string[]? _expressions;
        private static readonly List<List<string>> AllVars = new List<List<string>>();
        private static List<int> _gluedNumbers = new List<int>();
        private List<List<string?>> _terms = new List<List<string?>>();
        private List<string?> _preparedTerms = new List<string?>();
        private List<string?> _unUsedTerms = new List<string?>();
        private List<string> _varsList = new List<string>();

        private void InitializeSet(int countOfVars)
        {
            if (_terms.Count != countOfVars+1)
            {
                _terms.Clear();
                for (int i = 0; i <= countOfVars; i++)
                {
                    _terms.Add(new List<string?>());
                }
            }
        }
        
        private void SetVariablesFromList()
        {
            _varsList = AllVars[0];
            for (int i = 0; i < _varsList.Count; i++)
            {
                var currentChar = _varsList[i];
                if (currentChar[0] == '!') _varsList[i] = Inversed(currentChar);
            }
        }
        
        private int GetOnesCount(string? expr)
        {
            int count = 0;
            if (expr != null)
                foreach (var variable in expr)
                {
                    if (variable == '1') count++;
                }

            return count;
        }
        
        public void AddNumberSet(string? set)
        {
            string? newset = null;
            if (set != null)
            {
                for (int i = set.Length - 1; i >= 0; i--)
                {
                    newset += set[i];
                }

                InitializeSet(set.Length);
            }

            int countOfOnes = GetOnesCount(newset);
            _terms[countOfOnes].Add(newset);
            _unUsedTerms.Add(newset);
        }
        
        public string? MinimizeWithMcCluskeyMethod()
        {
            if (_expressions!.Length == 1) return _sdnf;
            int startTermCount = _terms.Count;
            for (int i = 0; i < startTermCount; i++)
            {
                _preparedTerms.Clear();
                MakeTermsTable();
            }
            SetVariablesFromList();
            return GetStringFromTerms();
        }
        
        private string? GetStringFromTerms()
        {
            string? result = null;
            foreach (var term in _unUsedTerms)
            {
                result += '(';
                if (term != null)
                    for (int i = 0; i < term.Length; i++)
                    {
                        var thisVar = term[i];
                        if (thisVar == '1') result += _varsList[i] + '&';
                        if (thisVar == '0') result += Inversed(_varsList[i]) + '&';
                    }

                result = result.Remove(result.Length - 1, 1);
                result += ")V";
            }

            result = result?.Remove(result.Length - 1, 1);
            return result;
        }
        
        private void MakeTermsTable()
        {
            if (_terms.Count > 1)
            {
                for (int index = 0; index < _terms.Count - 1; index++)
                {
                    foreach (var firstVarsSet in _terms[index])
                    {
                        foreach (var secondVarsSet in _terms[index + 1])
                        {
                            string? subRez = TryGetTerm(firstVarsSet, secondVarsSet);
                            if (subRez != String.Empty)
                            {
                                if (_unUsedTerms.Contains(firstVarsSet)) _unUsedTerms.Remove(firstVarsSet);
                                if (_unUsedTerms.Contains(secondVarsSet)) _unUsedTerms.Remove(secondVarsSet);
                                if (!_preparedTerms.Contains(subRez)) _preparedTerms.Add(subRez);
                                if (!_unUsedTerms.Contains(subRez)) _unUsedTerms.Add(subRez);
                            }
                        }
                    }
                }
            }

            if (_preparedTerms.Count > 1)
            {
                InitializeSet(GetOnesCount(_preparedTerms[_preparedTerms.Count - 1]));
                foreach (var term in _preparedTerms)
                {
                    int countOfOnes = GetOnesCount(term);
                    _terms[countOfOnes].Add(term);
                }
            }
        }
        
        private string? TryGetTerm(string? first, string? second)
        {
            int difArgsCount = 0;
            if (second != null && first != null && first.Length != second.Length)
            {
                throw new DataException("Terms must be of the same length");
            }

            string? comparision = null;

            if (first != null)
                for (int index = 0; index < first.Length; index++)
                {
                    if (second != null && first[index] == second[index])
                    {
                        comparision += first[index];
                    }
                    else
                    {
                        difArgsCount++;
                        comparision += "*";
                    }
                }

            if (difArgsCount == 1)
            {
                return comparision;
            }

            return string.Empty;
        }

        public void SetExpression (string? str)
        {
            _sdnf = str;
            if (IsCorrect())
            {
                _expressions = _sdnf?.Split('V');
                if (_expressions != null)
                    for (int i = 0; i < _expressions.Length; i++)
                    {
                        _expressions[i] = _expressions[i].Remove(_expressions[i].Length - 1, 1);
                        _expressions[i] = _expressions[i].Remove(0, 1);
                    }
            }

            SetVariables();
        }

        private void SetVariables()
        {
            if (IsCorrect())
            {
                if (_expressions != null)
                    AllVars.Clear();
                foreach (var expression in _expressions!)
                {
                    string[] vars = expression.Split('&');
                    List<string> variables = vars.ToList();
                    AllVars.Add(variables);
                }
            } 
        }

        public string? MinimizeWithCalculation()
        {
            if (_expressions!.Length == 1) return _sdnf;
            string startExpression = _sdnf!;
            string? totalResult = null;
            int startVarsCount = AllVars.Count;
            for(int time=0; time<startVarsCount; time++)
            {
                List<string> newExpressions = new List<string>(); 
                string? result = null;
                _gluedNumbers.Clear();
                if (_expressions != null)
                    for (int firstIndex = 0; firstIndex < _expressions.Length - 1; firstIndex++)
                    {
                        for (int secondIndex = firstIndex + 1; secondIndex < _expressions.Length; secondIndex++)
                        {
                            string subResult = MakeGlue(firstIndex, secondIndex);
                            if (!newExpressions.Contains(subResult))
                            {
                                result += subResult;
                                newExpressions.Add(subResult);
                                if (subResult != String.Empty)
                                {
                                  result += "V";  
                                }
                            }
                            if (subResult != string.Empty)
                            {
                                if (!_gluedNumbers.Contains(secondIndex)) _gluedNumbers.Add(secondIndex);
                                if (!_gluedNumbers.Contains(firstIndex)) _gluedNumbers.Add(firstIndex);
                            }
                        }
                    }

                if (result != null && result.Length > 0)
                {
                    result = result.Remove(result.Length - 1);
                }
                else
                {
                    SetExpression(startExpression);
                    if (totalResult != null)
                    {
                        return totalResult;
                    }
                    else
                    {
                        return startExpression;
                    }
                }

                if (_expressions != null)
                    for (int i = 0; i < _expressions.Length; i++)
                    {
                        if (!_gluedNumbers.Contains(i))
                        {
                            if (result.Length > 1) result += "V";
                            result += $"({_expressions[i]})";
                        }
                    }

                totalResult = result;
                SetExpression(result);
                if (_gluedNumbers.Count == 0) break;
            }

            LogicalCalculator calculator = new LogicalCalculator(totalResult, 1);
            string? res = calculator.Calculate();
            SetExpression(startExpression);
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
        
        public string? MinimizeWithKarnaugh()
        {
            if (_expressions!.Length == 1) return _sdnf;
            string? subRezult = MinimizeWithMcCluskeyMethod();
            string result = null!;
            List<string> rez = subRezult!.Split('V').ToList();
            rez.Reverse();
            foreach (var variable in rez)
            {
                result += variable+"V";
            }

            if (result.Length > 0)
            {
                result = result.Remove(result.Length - 1, 1);
            }
            return result;
        }

        private string MakeGlue(int first, int second)
        {
            if (IsCorrect())
            {
                int differentValues = 0;
                string result = "(";
                List<string> commonVars = new List<string>();
                List<string> firstVarsSet = AllVars[first];
                List<string> secondVarsSet = AllVars[second];
                if (firstVarsSet.Count != secondVarsSet.Count)
                {
                    return string.Empty;
                }
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
                            differentValues++;
                        }
                        else
                        {
                            return String.Empty;
                        }
                    }
                    
                }

                if (commonVars.Count < countOfVars - 1)
                {
                    return string.Empty;
                }

                if (differentValues > 1)
                {
                    return String.Empty;
                }

                foreach (var variable in commonVars)
                {
                    result += $"{variable}&";
                }

                result = result.Remove(result.Length - 1, 1);
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