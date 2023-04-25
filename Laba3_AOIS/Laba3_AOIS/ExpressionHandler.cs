namespace Laba2_AOIS
{

    public class ExpressionHandler
    {
        private readonly string? expression;
        private readonly List<string> variables = new List<string>();
        private readonly Dictionary<string, bool> variablesValues = new Dictionary<string, bool>();

        public ExpressionHandler(string? expression)
        {
            this.expression = expression;
            CheckForExpression();
            SetVariables();
        }

        public int CalculateExpression()
        {
            string? currentExpression = ReplaceExpression(this.expression);
            return Calculator.Calculate(currentExpression);
        }

        private void SetVariables()
        {
            if (expression != null)
                for (int index = 0; index < expression.Length; index++)
                {
                    if (!char.IsLetter(expression[index])) continue;
                    int variableLength = 1;
                    while (index + variableLength < expression.Length &&
                           char.IsDigit(expression[index + variableLength]))
                    {
                        variableLength++;
                    }

                    string tempVariable = expression.Substring(index, variableLength);
                    if (!variables.Contains(tempVariable))
                    {
                        variables.Add(tempVariable);
                    }
                }

            variables.Sort();
        }

        public int GetVariablesCount()
        {
            if (variables.Count == 0)
            {
                SetVariables();
            }
            return variables.Count;
        }

        private void CheckForExpression()
        {
            if (string.IsNullOrEmpty(expression))
            {
                throw new Exception("No expression set for handler");
            }
        }

        public void SetVariablesValuesWith(int valueForByting)
        {
            variablesValues.Clear();
                for (int index = GetVariablesCount() - 1; index >= 0; index--)
                {
                    variablesValues.Add(variables[index], valueForByting % 2 != 0);
                    valueForByting /= 2;
                }
        }

        public string GetVariablesString()
        {
            string values = null;
            foreach (var value in variablesValues)
            {
                values+=(GetValueFromBool(value.Value));
            }

            return values;
        }

        private string? ReplaceExpression(string? incomingExpression)
        {
            string? newExpresion = incomingExpression;

            foreach (var key in Operations.keys)
            {
                newExpresion = newExpresion?.Replace(key, key.GetOperationKey());
            }

            List<string> sortedVariables = new List<string>(variables);
            sortedVariables.Reverse();

            foreach (var variable in sortedVariables)
            {
                newExpresion = newExpresion?.Replace(variable, GetValueFromBool(variablesValues[variable]));
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

        public string ReturnSKNF()
        {
            string function = "(";
            foreach (var value in variables)
            {
                if (variablesValues[value])
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

        public string ReturnSDNF()
        {
            string function = "(";
            foreach (var value in variables)
            {
                if (variablesValues[value])
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
            foreach (var variable in variables)
            {
                function+= ($"{GetValueFromBool(variablesValues[variable])} ");
            }

            return function;
        }

    }
}