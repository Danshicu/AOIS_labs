using Laba3_AOIS;

namespace Laba8_AOIS;

public class Processor
{
    private string[] Memory { set; get; }
    private readonly int _bitsCount;
    private readonly Random _random = new Random();
    private bool _isDiagonalized;
    

    public Processor(int bitsCount)
    {
        _bitsCount = bitsCount;
        Memory = new string[bitsCount];
        FillMemory();
    }

    private void FillMemory()
    {
        for (int i=0; i<Memory.Length; i++)
        {
            string newValue = null!;
            for (int index = 0; index < _bitsCount; index++)
            {
                newValue += _random.Next(0, 2).ToString();
            }

            Memory[i] = newValue;
        }
    }

    private string[] GetCopy(string[] array)
    {
        string[] copy = new string[array.Length];
        for (int i = 0; i < array.Length; i++)
        {
            copy[i] = array[i];
        }

        return copy;
    }

    public void Diagonalize()
    {
        if (!_isDiagonalized)
        {
            _isDiagonalized = true;
            for (int i = 1; i < _bitsCount; i++)
            {
                string temp = Memory[i];
                Memory[i] = GetDiagonalizedWord(i, temp);
            }
        }
    }

    private string GetDiagonalizedWord(int index, string word)
    {
        string endWord = null!;
        for (int i = 0; i < _bitsCount; i++)
        {
            if (i - index < 0)
            {
               endWord += word[_bitsCount + i - index];
            }
            else
            {
                endWord += word[i - index];
            }
        }

        return endWord;
    }

    public void Normalize()
    {
        if (_isDiagonalized)
        {
            _isDiagonalized = false;
            for (int i = 1; i < _bitsCount; i++)
            {
                Memory[i] = GetNormalizedWord(i);
            }
        }
    }

    private string GetNormalizedWord(int index)
    {
        string startWord = Memory[index];
        string endWord = null!;
        for (int i = 0; i < _bitsCount; i++)
        {
            if (index + i >= _bitsCount)
            {
                endWord += startWord[index + i - _bitsCount];
            }
            else
            {
                endWord += startWord[index + i];
            }
        }

        return endWord;
    }

    private char Function_0(char first, char second)
    {
        return '0';
    }
    
    private char Function_15(char first, char second)
    {
        return '1';
    }
    
    private char Function_5(char first, char second)
    {
        return second;
    }
    
    private char Function_10(char first, char second)
    {
        return Inverse(second);
    }

    public string GetFunction(int firstWordIndex, int secondWordIndex, string operation)
    {
        string result = null!;
        string firstWord = Memory[firstWordIndex-1];
        string secondWord = Memory[secondWordIndex-1];
        if (_isDiagonalized)
        {
            firstWord = GetNormalizedWord(firstWordIndex-1);
            secondWord = GetNormalizedWord(secondWordIndex-1);
        }
        for (int i = 0; i < _bitsCount; i++)
        {
            
            if (operation == "f0")
            {
                result += Function_0(firstWord[i], secondWord[i]);
            }
            if (operation == "f5")
            {
                result += Function_5(firstWord[i], secondWord[i]);
            }
            if (operation == "f10")
            {
                result += Function_10(firstWord[i], secondWord[i]);
            }
            if (operation == "f15")
            {
                result += Function_15(firstWord[i], secondWord[i]);
            }
        }

        return result;
    }

    public string GetWordAt(int index)
    {
        if (!_isDiagonalized)
        {
            return Memory[index-1];
        }

        return GetNormalizedWord(index-1);
    }

    public void ReWriteWordAt(int index, string word)
    {
        if (word.Length == _bitsCount)
        {
            if (!_isDiagonalized)
            {
                Memory[index-1] = word;
            }
            else
            {
                Memory[index-1] = GetDiagonalizedWord(index-1, word);
            }
        }
    }

    public void Summarize(string mask)
    {
        if (_bitsCount != 16)
        {
            throw new Exception("Can operate with only 16 digits length");
        }
        if (mask.Length != 3)
        {
            throw new Exception("Mask size is not valid");
        }
        for (int i = 0; i < _bitsCount; i++)
        {
            string fullWord = Memory[i];
            if (mask == fullWord.Substring(0, 3))
            {
                string first = fullWord.Substring(3, 4);
                string second = fullWord.Substring(7, 4);
                Memory[i] = fullWord.Remove(11) + Summ(first, second);
            }
        }
    }

    private int GetInt(string from)
    {
        int result =0;
        for (int i = from.Length-1; i >= 0; i--)
        {
            if (from[i] == '1')
            {
                result += (int)Math.Pow(2, (from.Length-1) - i);
            }
        }

        return result;
    }

    private string Summ(string first, string second)
    {
        int resultInt = GetInt(first) + GetInt(second);
        string resultString = new string(first+'1');
        for (int index = first.Length; index >= 0; index--)
        {
            char resChar = resultInt % 2 != 0 ? '1' : '0';
            resultString = resultString.Insert(0, resChar.ToString());
            resultInt /= 2;
        }
        resultString = resultString.Remove(first.Length + 1);
        return resultString;
    }
    
    public void DisplayMemory()
    {
        Console.WriteLine();
        var memoryToDisplay = GetCopy(Memory);
        for (int i = 0; i < _bitsCount; i++)
        {
            string firstWord = memoryToDisplay[i];
            string secondSubWord = null!;
            secondSubWord += firstWord[i];
            for (int index = i+1; index < _bitsCount; index++)
            {
                string currentWord = memoryToDisplay[index];
                secondSubWord += currentWord[i];
                memoryToDisplay[index] = memoryToDisplay[index].Remove(i,1).Insert(i, firstWord[index].ToString());
            }
            memoryToDisplay[i] = memoryToDisplay[i].Remove(i).Insert(i,secondSubWord);
        }
        foreach (var t in memoryToDisplay)
        {
            Console.WriteLine($"{t}");
        }
        Console.WriteLine();
    }

    private char Inverse(char incomeValue)
    {
        return incomeValue == '1' ? '0' : '1';
    }

    private char CompareDigits(char first, char second)
    {
        if (first == second)
        {
            if (first == '1')
            {
                return '1';
            }
        }
        return '0';
    }

    private int GetInt(char value)
    {
        return value == '1' ? 1 : 0;
    }

    private char GreaterTriggerValue(char prevGreaterValue, char aValue, char sValue, char prevLowerValue)
    {
        char firstComparing = CompareDigits(Inverse(aValue), sValue);
        char secondComparing = CompareDigits(firstComparing, Inverse(prevLowerValue));
        if (prevGreaterValue == '1' || secondComparing == '1')
        {
            return '1';
        }

        return '0';
    }
    
    private char LowerTriggerValue(char prevLowerValue, char aValue, char sValue, char prevGreaterValue)
    {
        char firstComparing = CompareDigits(aValue, Inverse(sValue));
        char secondComparing = CompareDigits(firstComparing, Inverse(prevGreaterValue));
        if (prevLowerValue == '1' || secondComparing == '1')
        {
            return '1';
        }

        return '0';
    }

    private int Compare(string word, string compareWith)
    {
        char prevLowerValue = '0', prevGreaterValue = '0';
        for (int i = 0; i < _bitsCount; i++)
        {
            char greaterValue = GreaterTriggerValue(prevGreaterValue, compareWith[i], word[i], prevLowerValue);
            char lowerValue = LowerTriggerValue(prevLowerValue, compareWith[i], word[i], prevGreaterValue);
            prevLowerValue = lowerValue;
            prevGreaterValue = greaterValue;
        }

        if (GetInt(prevGreaterValue) > GetInt(prevLowerValue))
        {
            return 1;
        }

        if (GetInt(prevGreaterValue) < GetInt(prevLowerValue))
        {
            return -1;
        }

        return 0;
    }
    private string FindMinWord(List<string> memory)
    {
        int iteration = memory.Count-1;
        string minimalWord = memory[iteration];
        while (iteration >= 0)
        {
            int compareResult = Compare(memory[iteration], minimalWord);
            if (compareResult >= 0)
            {
                iteration -= 1;
                continue;
            }

            minimalWord = memory[iteration];
            iteration -= 1;
        }

        return minimalWord;
    }
    
    private string FindMaxWord(List<string> memory)
    {
        int iteration = memory.Count-1;
        string maximalWord = memory[iteration];
        while (iteration >= 0)
        {
            int compareResult = Compare(memory[iteration], maximalWord);
            if (compareResult <= 0)
            {
                iteration -= 1;
                continue;
            }

            maximalWord = memory[iteration];
            iteration -= 1;
        }

        return maximalWord;
    }

    private int GetNumberOfCompares(string first, string second)
    {
        if (first.Length != second.Length)
        {
            throw new Exception("Words of different size");
        }

        int numOfCompares = 0;
        for (int i = 0; i < _bitsCount; i++)
        {
            if (first[i] == second[i])
            {
                numOfCompares++;
            }
        }

        return numOfCompares;
    }

    public void SearchByCorrespondence(string searchingWord)
    {
        int maxCompare = -1;
        List<string> maximalWords = new List<string>();
        for (int i = 0; i < _bitsCount; i++)
        {
            int numOfCompares = GetNumberOfCompares(Memory[i], searchingWord);
            if (maxCompare < numOfCompares)
            {
                maximalWords.Clear();
                maximalWords.Add(Memory[i]);
                maxCompare = numOfCompares;
            }

            if (maxCompare == numOfCompares)
            {
                maximalWords.Add(Memory[i]);
            }
        }

        foreach (var word in maximalWords)
        {
           Console.WriteLine($"Maximal corresponding word is : {word}"); 
        }
    }
    
    
    public void SortMinToMax()
    {
        string[] newMemory = new string[Memory.Length];
        List<string> existingMemory = Memory.ToList();
        for (int round = 0; round < newMemory.Length; round++)
        {
            string minimal = FindMinWord(existingMemory);
            newMemory[round] = minimal;
            existingMemory.Remove(minimal);
        }

        Memory = newMemory;
    }

    public void SortMaxToMin()
    {
        SortMinToMax();
        var reversedMemory = Memory.ToList();
        reversedMemory.Reverse();
        Memory = reversedMemory.ToArray();
    }

    public void Sort()
    {
        SortMinToMax();
        DisplayMemory();
        SortMaxToMin();
        DisplayMemory();
    }

    public string BooleanFunctionSearch(string function)
    {
        TableCreator table = new TableCreator(function);
        string comparingTo = table.GetResultString();
        if (comparingTo.Length == _bitsCount)
        {
           foreach (var t in Memory)
           {
               int compareResult = Compare(t, comparingTo);
               if (compareResult == 0)
               {
                   return t;
               }
           } 
        }
        return String.Empty;
    }

}