namespace Laba2_AOIS
{
    public class LogicalCalculator
    {
        private int status;
        private string _expression;
        private List<string> _expressions;
        //private List<string> vars = new List<string>();
        private readonly List<List<string>> AllVars = new List<List<string>>();
        private Dictionary<string, string> currentValues = new Dictionary<string, string>();

        public LogicalCalculator(string expression, int status)
        {
            _expression = expression;
            this.status = status;
            SetVariables();
        }

        private string Disjunction(string firstVar, string secondVar)
        {
            if (secondVar == Inversed(firstVar) || firstVar == Inversed(secondVar))
            {
                return "1";
            }
            firstVar = ReplaceWithValue(firstVar);
            secondVar = ReplaceWithValue(secondVar);
            if (firstVar == "1" || secondVar == "1") return "1";
            if (secondVar == "0") return firstVar;
            if (firstVar == "0") return secondVar;
            return $"{firstVar}V{secondVar}";
        }

        private string Conjunction(string firstVar, string secondVar)
        {
            if (secondVar == Inversed(firstVar) || firstVar == Inversed(secondVar))
            {
                return "0";
            }
            firstVar = ReplaceWithValue(firstVar);
            secondVar = ReplaceWithValue(secondVar);
            if (firstVar == "0" || secondVar == "0") return "0";
            if (firstVar == "1") return secondVar;
            if (secondVar == "1") return firstVar;
            return $"{firstVar}&{secondVar}";
        }

        private void SetVariables()
        {
            if (status == 1)
            {
                SetVariablesForSDNF();
            }
            else
            {
                SetVariablesForSKNF();
            }
        }

        private void SetVariablesValues(int expressionIndex)
        {
            if (currentValues.Count > 0)
            {
                currentValues.Clear();
            }

            foreach (var variable in AllVars[expressionIndex])
            {
                if (this.status == 1)
                {
                    currentValues.Add(variable, "1");
                    currentValues.Add(Inversed(variable), "0");
                }
                else
                {
                    currentValues.Add(variable, "0");
                    currentValues.Add(Inversed(variable), "1");
                }
            }
        }

        public string Calculate()
        {
            string result = null;
            for(int index =0; index<AllVars.Count; index++)
            {
                string temp = null;
                SetVariablesValues(index);
                List<string> newExpression = new List<string>();
                for (int i = 0; i < AllVars.Count; i++)
                {
                    if (i == index) continue;
                    var varList = AllVars[i];
                    if (status == 1)
                    {
                        string substring = Conjunction(varList[0], varList[1]);
                        newExpression.Add(substring);
                    }
                    else
                    {
                        string substring = Disjunction(varList[0], varList[1]);
                        newExpression.Add(substring);
                    }
                }
                while (newExpression.Count > 1)
                    {
                        var first = newExpression[0];
                        var second = newExpression[1];
                        newExpression.RemoveAt(1);
                        newExpression.RemoveAt(0);
                        if (status == 1)
                        {
                            newExpression.Add(Disjunction(first, second));
                        }
                        else
                        {
                            newExpression.Add(Conjunction(first, second));
                        }
                    }
                result += GetIfReasonable(newExpression[0], index);
            }
            if(result.Length>1)
                result = result.Remove(result.Length - 1, 1);
            return result;
        }

        private string GetIfReasonable(string value, int index)
        {
            char separator;
            if (value == "1" || value == "0") return string.Empty;
            if (status == 0)
            {
                separator = '&';
            }
            else
            {
                separator = 'V';
            }
            return $"({_expressions[index]}){separator}";
        }

        private void SetVariablesForSKNF()
        {
            _expressions = _expression.Split('&').ToList();
            for (int index = 0; index < _expressions.Count; index++)
            {
                string thisExpr = _expressions[index];
                thisExpr = thisExpr.Remove(0, 1);
                thisExpr = thisExpr.Remove(thisExpr.Length - 1,1);
                _expressions[index] = thisExpr;
            }
            
            foreach (var expr in _expressions)
            {
                string[] vars = expr.Split('V');
                AllVars.Add(vars.ToList());
            }
        }

        private void SetVariablesForSDNF()
        {
            _expressions = _expression.Split('V').ToList();
            for (int index = 0; index < _expressions.Count; index++)
            {
                string thisExpr = _expressions[index];
                thisExpr = thisExpr.Remove(0, 1);
                thisExpr = thisExpr.Remove(thisExpr.Length - 1,1);
                _expressions[index] = thisExpr;
            }

            foreach (var expr in _expressions)
            {
                string[] vars = expr.Split('&');
                AllVars.Add(vars.ToList());
            }
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

        private string ReplaceWithValue(string temp)
        {
            if (currentValues.ContainsKey(temp))
            {
                return currentValues[temp];
            }
            return temp;
        }
    }
}