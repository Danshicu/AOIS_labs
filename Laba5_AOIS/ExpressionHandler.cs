namespace Laba5_AOIS
{

    public class ExpressionHandler
    {
        private readonly string? _expression;
        private readonly List<string> _variables = new List<string>();
        private readonly Dictionary<string, bool> _variablesValues = new Dictionary<string, bool>();

        public ExpressionHandler(string? expression)
        {
            this._expression = expression;
            CheckForExpression();
            SetVariables();
        }

        public int CalculateExpression()
        {
            string? currentExpression = ReplaceExpression(this._expression);
            return Calculator.Calculate(currentExpression);
        }

        public List<string> GetVariables()
        {
            return _variables;
        }

        private void SetVariables()
        {
            if (_expression != null)
                for (int index = 0; index < _expression.Length; index++)
                {
                    if (!char.IsLetter(_expression[index])) continue;
                    int variableLength = 1;
                    while (index + variableLength < _expression.Length &&
                           char.IsDigit(_expression[index + variableLength]))
                    {
                        variableLength++;
                    }

                    string tempVariable = _expression.Substring(index, variableLength);
                    if (!_variables.Contains(tempVariable))
                    {
                        _variables.Add(tempVariable);
                    }
                }

            _variables.Sort();
        }

        public int GetVariablesCount()
        {
            if (_variables.Count == 0)
            {
                SetVariables();
            }
            return _variables.Count;
        }

        private void CheckForExpression()
        {
            if (string.IsNullOrEmpty(_expression))
            {
                throw new Exception("No expression set for handler");
            }
        }

        public void SetVariablesValuesWith(int valueForByting)
        {
            _variablesValues.Clear();
                for (int index = GetVariablesCount() - 1; index >= 0; index--)
                {
                    _variablesValues.Add(_variables[index], valueForByting % 2 != 0);
                    valueForByting /= 2;
                }
        }

        public string? GetVariablesString()
        {
            string? values = null;
            foreach (var value in _variablesValues)
            {
                values+=(GetValueFromBool(value.Value));
            }

            return values;
        }

        private string? ReplaceExpression(string? incomingExpression)
        {
            string? newExpresion = incomingExpression;

            foreach (var key in Operations.Keys)
            {
                newExpresion = newExpresion?.Replace(key, key.GetOperationKey());
            }

            List<string> sortedVariables = new List<string>(_variables);
            sortedVariables.Reverse();

            foreach (var variable in sortedVariables)
            {
                newExpresion = newExpresion?.Replace(variable, GetValueFromBool(_variablesValues[variable]));
            }

            newExpresion = newExpresion?.Replace("!!", "");
            newExpresion = newExpresion?.Replace("!0", "1");
            newExpresion = newExpresion?.Replace("!1", "0");

            return newExpresion;
        }

        private string GetValueFromBool(bool value)
        {
            return value ? "1" : "0";
        }

        public string ReturnSknf()
        {
            string function = "(";
            foreach (var value in _variables)
            {
                if (_variablesValues[value])
                {
                    function += ($"!{value}V");
                }
                else
                {
                    function += ($"{value}V");
                }
            }

            function = function.Remove(function.Length - 1);
            function += ")";
            return function;
        }

        public string ReturnSdnf()
        {
            string function = "(";
            foreach (var value in _variables)
            {
                if (_variablesValues[value])
                {
                    function += ($"{value}&");
                }
                else
                {
                    function += ($"!{value}&");
                }
            }
            function = function.Remove(function.Length - 1);
            function += ")";
            return function;
        }

        public string? GetFunctionString()
        {
            string? function = null;
            foreach (var variable in _variables)
            {
                function+= ($"{GetValueFromBool(_variablesValues[variable])} ");
            }

            return function;
        }

    }
}