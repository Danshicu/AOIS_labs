namespace Laba2_AOIS
{
    public class LogicalCalculator
    {
        private string _expression;
        private List<string> vars = new List<string>();

        public LogicalCalculator(string expression)
        {
            _expression = expression;
        }

        private string Disjunction(string expression)
        {
            return expression;
        }

        private void SetVariables()
        {
            
        }
    }
}