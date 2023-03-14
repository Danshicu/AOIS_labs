Digit first = new Digit(11);
Digit second = new Digit(23);
Digit third = new Digit(-11);
Digit four = new Digit(-23);
FloatingPoint theFirst = new FloatingPoint((float)11.5);
FloatingPoint theSecond = new FloatingPoint((float)23.625);

Console.Write("Digits: ");
first.ShowInt();
Console.Write("    ");
second.ShowInt();
Console.Write("    ");
theFirst.ShowFloat();
Console.Write("    ");
theSecond.ShowFloat();
Console.Write("\nResults:\n");

Digit total = first + second;
total.ShowOperate(first, second, '+');

total = first + four;
total.ShowOperate(first, four, '+');

total = third + second;
total.ShowOperate(third, second, '+');

total = third + four;
total.ShowOperate(third, four, '+');

total = first - second;
total.ShowOperate(first, second, '-');

total = first - four;
total.ShowOperate(first, four, '-');

total = third - second;
total.ShowOperate(third, second, '-');

total = third - four;
total.ShowOperate(third, four, '-');

total = first * second;
total.ShowOperate(first, second, '*');

total = second / first;
total.ShowOperate(second, first, '/');

FloatingPoint result = theFirst + theSecond;
result.ShowFloat();

    return 0; 

public class Digit 
{
    private readonly bool[] _bytes = new bool[16];

    private Digit() { }
    public Digit(int value)
    {
        if (value < 0)
        {
            _bytes[0] = true;
        }
        else
        {
            _bytes[0] = false;
        }
        for (byte i = 15; i > 0; i--)
        {
            if (value % 2 == 0)
            {
                _bytes[i] = false;
            }
            else
            {
                _bytes[i] = true;
            }
            value /=2;
        }
    }

    private static Digit Copy(Digit old)
    {
        return new Digit(old.ToInt());
    }

    private static Digit GetAbsolute(Digit old)
    {
        Digit result = Copy(old);
        result._bytes[0] = false;
        return result;
    }

    public static Digit Zero()
    {
        return new Digit();
    }

    public static Digit PositiveOne()
    {
        Digit posOne = Zero();
        posOne._bytes[15] = true;
        return posOne;
    }
    
    public static Digit NegativeOne()
    {
        Digit negOne = Zero();
        negOne._bytes[0] = true;
        negOne._bytes[15] = true;
        return negOne;
    }

    public void ShowByte()
    {
        foreach (bool number in _bytes)
        {
            Console.Write(number ? 1 : 0);
        }
        Console.Write('\n');
    }

    private int ToInt()
    {
        int thisInt = 0;
        for (byte i = 1; i <= 15; i++)
        {
            if (_bytes[i])
            {
                thisInt += (int)Math.Pow(2, 15 - i);
            }
        }
        if (_bytes[0])
        {
            thisInt *= -1;
        }
        return thisInt;
    }

    public void ShowInt()
    {
        Console.Write(this.ToInt());
    }

    private static bool Sum(bool first, bool second, ref bool previous)
    {
        if (first == second)
        {
            if(first){previous = true;}
            return false;
        }
        
        return true;
    }

    private Digit ToInverse()
    {
        Digit inversed = new Digit();
        for (byte i = 1; i <= 15; i++)
        {
            inversed._bytes[i] = !_bytes[i];
        }
        inversed._bytes[0] = _bytes[0];
        return inversed;
    }

    private Digit ToAdditional()
    {
        Digit additional = ToInverse();
        additional += NegativeOne();
        return additional;
    }
    private static Digit Summarize(Digit first, Digit second, Digit result)
    {
        for (byte i = 15; i > 0; i--) 
        {
                bool temp = Sum(first._bytes[i], second._bytes[i], ref result._bytes[i-1]);
                result._bytes[i] = Sum(temp, result._bytes[i], ref result._bytes[i - 1]);
        }

        result._bytes[0] = first._bytes[0];
        return result;
    }

    private static Digit SummarizeDifferentSign(Digit positive, Digit negative, Digit result)
    {
        Digit additional = negative.ToAdditional();
        Summarize( positive,  additional,  result);
        if (positive < negative)
        {
            result = result.ToInverse();
            result._bytes[0] = true;
            Digit one = PositiveOne();
            Digit temp = result;
            result = Zero();
            Summarize(temp, one, result);
        }
        return result;
    }
    
    private Digit MoveLeftFor(byte step)
    {
        for (byte i = 1; i < 16; i++)
        {
            if (i + step < 16)
            {
                _bytes[i] = _bytes[i + step];
            }
            else
            {
                _bytes[i] = false;
            }
        }
        return this;
    }
    
    
    public static Digit operator +(Digit first, Digit second)
    {
        Digit result = new Digit();
        if (first._bytes[0] == second._bytes[0])
        {
          result = Summarize(first, second, result);
        }
        else
        {
            result = first._bytes[0] ? SummarizeDifferentSign(second,  first, result) : SummarizeDifferentSign(first, second, result);
        }
        return result;
    }

    public static Digit operator -(Digit first, Digit second)
    {
        Digit temp = Copy(second);
        temp._bytes[0] = !temp._bytes[0];
        Digit result = first + temp;
        return result;
    }

    public static Digit operator *(Digit first, Digit second)
    {
        Digit result = new Digit();
        Digit firstTemp = Zero();
        Digit secondTemp  = Zero();
        if (second._bytes[15])
        {
            firstTemp = Copy(first);
            firstTemp._bytes[0] = false;
        }
        for (byte i = 14; i > 0; i--)
        {
            if (second._bytes[i])
            {
                secondTemp = Copy(first).MoveLeftFor((byte)(15 - i));
                secondTemp._bytes[0] = false;
                firstTemp += secondTemp;
            }
        }

        result = firstTemp;
        if (first._bytes[0] == second._bytes[0])
        {
            result._bytes[0] = false;
        }
        else
        {
            result._bytes[0] = true;
        }
        return result;
    }

    public static Digit operator /(Digit first, Digit second)
    {
        Digit result = Zero();
        Digit temp = GetAbsolute(first);
        if (first == second)
        {
            result = PositiveOne();
        }
        while (temp > second)
        {
            temp -= second;
            result += PositiveOne();
        }

        if (first._bytes[0] != second._bytes[0])
        {
            result._bytes[0] = true;
        }
        return result;
    }

    public static bool operator <(Digit first, Digit second)
    {
        for (byte i = 1; i < 16; i++)
        {
            if (first._bytes[i] != second._bytes[i])
            {
                if (first._bytes[i])
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        return false;
    }
    
    public static bool operator >(Digit first, Digit second)
    {
        for (byte i = 1; i < 16; i++)
        {
            if (first._bytes[i] != second._bytes[i])
            {
                if (first._bytes[i])
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        return false;
    }

    public static bool operator ==(Digit first, Digit second)
    {
        for (byte i = 1; i < 16; i++)
        {
            if (first._bytes[i] != second._bytes[i])
            {
                return false;
            }
        }
        return true;
    }
    
    public static bool operator !=(Digit first, Digit second)
    {
        for (byte i = 1; i < 16; i++)
        {
            if (first._bytes[i] != second._bytes[i])
            {
                return true;
            }
        }
        return false;
    }

    public void ShowOperate(Digit first, Digit second, char operation)
    {
        Console.WriteLine($"{first.ToInt()}{operation}{second.ToInt()} : {this.ToInt()}");
    }


}

public class FloatingPoint
{
    private bool _sign;
    private readonly bool[] _exponent = new bool[8];
    private readonly bool[] _mantis = new bool[23];

    private FloatingPoint() { }

    private int GetExponent()
    {
        int result = 0;
        for (byte i = 0; i < 8; i++)
        {
            if (_exponent[i])
            {
                result += (byte)Math.Pow(2, 7 - i);
            }
        }

        result -= 127;
        return result;
    }
    
    public FloatingPoint(float number)
    {
        if (number < 0)
        {
            _sign = true;
        }
        int whole = Math.Abs((int)number);
        float fractional = (Math.Abs(number) - whole);
        byte intIndex = 22;
        while (whole >= 1)
        {
            if (whole % 2 == 0)
            {
                _mantis[intIndex] = false;
            }
            else
            {
                _mantis[intIndex] = true;
            }
            whole /=2;
            if (whole != 0)
            {
                intIndex--;
            }
        }
        int currentIndex = 0;
        while (currentIndex + intIndex <= 22)
        {
            _mantis[currentIndex] = _mantis[currentIndex + intIndex];
            currentIndex++;
        }

        if (_mantis[0])
        {
            for (byte i = 0; i < currentIndex; i++)
            {
                _mantis[i] = _mantis[i + 1];
            }

            SetExponent(currentIndex);

            currentIndex--;
            while (currentIndex <= 22)
            {
                fractional *= 2f;
                if (fractional  < 1)
                {
                    _mantis[currentIndex] = false;
                }
                else
                {
                    _mantis[currentIndex] = true;
                    fractional -= 1;
                }
                currentIndex++;
            }
        }
        else 
        {
            while (fractional < 1)
            {
                currentIndex++;
                fractional *= 2;
            }

            SetExponent(-currentIndex);
            
            fractional -= 1;
            byte index = 0;
            while (index <= 22)
            {
                fractional *= 2;
                if (fractional  < 1)
                {
                    _mantis[index] = false;
                }
                else
                {
                    _mantis[index] = true;
                    fractional -= 1;
                }
                index++;
            }
        }
    }

    private void SetExponent(int exponent)
    {
        exponent += 127;
        for (int i = 7; i >= 0; i--)
        {
            if (exponent % 2 == 0)
            {
                _exponent[i] = false;
            }
            else
            {
                _exponent[i] = true;
            }
            exponent /=2;
        }
    }

    private float GetAbsoluteFloat()
    {
        float result = 0f;
        int offset = GetExponent();
        if (offset > 0)
        {
            if (offset >= 23)
            {
                Console.WriteLine("NaN");
                return 0;
            }

            result += (int)Math.Pow(2, offset - 1);
            for (int i = 0; i < offset-1; i++)
            {
                if (_mantis[i])
                {
                    result += (int)Math.Pow(2, offset - 2 - i);
                }
            }

            byte step = 0;
            for (int i = offset - 1; i < 23; i++)
            {
                if (_mantis[i])
                {
                    result += (float)Math.Pow(2, -1 - step);
                }

                step++;
            }
        }
        else
        {
            for (int i = 0; i < 23; i++)
            {
                if (_mantis[i])
                {
                    result += (float)Math.Pow(2, offset - i);
                }
            }

            result += (float)Math.Pow(2,  offset+1 );
        }

        return result;

    }

    public void ShowFloat()
    {
        if (_sign)
        {
            Console.Write(-GetAbsoluteFloat());
        }
        else
        {
            Console.Write(GetAbsoluteFloat());
        }
    }

    private static bool Sum(bool first, bool second, ref bool previous)
    {
        if (first == second)
        {
            if(first){previous = true;}
            return false;
        }
        
        return true;
    }

    private static FloatingPoint Copy(FloatingPoint old)
    {
        FloatingPoint copy = new FloatingPoint
        {
            _sign = old._sign
        };
        for (byte i = 0; i < 8; i++)
        {
            copy._exponent[i] = old._exponent[i];
        }

        for (byte index = 0; index < 23; index++)
        {
            copy._mantis[index] = old._mantis[index];
        }

        return copy;
    }

    private void MoveRightFor(int step, bool moveOnce=false)
    { 
        for (byte index = 22; index >= step; index--)
        {
            _mantis[index] = _mantis[index - step];
        }
        _mantis[step - 1] = true;
        if (step != 1)
        {
            for (byte i = 0; i < step - 1; i++) 
            {
                _mantis[i] = false;
            }
        }
        else
        { 
            _mantis[0] = moveOnce;
        }
    }
    
    private static FloatingPoint SummarizeOneSign(FloatingPoint first, FloatingPoint second, FloatingPoint result)
    {
        FloatingPoint bigger = new FloatingPoint();
        FloatingPoint smaller = new FloatingPoint();
        if (first > second)
        {
            bigger = Copy(first); 
            smaller = Copy(second);
        }
        else
        {
            bigger = Copy(second);
            smaller = Copy(first);
        }
        int biggerExponent = bigger.GetExponent();
        int smallerExponent = smaller.GetExponent();
        int offset = Math.Abs(biggerExponent - smallerExponent);
        result.SetExponent(biggerExponent);
        bool hasToMoveOnce = offset==1;
        if (offset != 0) {smaller.MoveRightFor(offset, hasToMoveOnce);} 
        bool temp;
        for (byte i = 22; i > 0; i--) 
        {
            temp = Sum(bigger._mantis[i], smaller._mantis[i], ref result._mantis[i-1]);
            result._mantis[i] = Sum(temp, result._mantis[i], ref result._mantis[i - 1]);
        }
        bool secondTemp = false;
        temp = Sum(bigger._mantis[0], smaller._mantis[0], ref secondTemp);
        result._mantis[0] = Sum(temp, result._mantis[0], ref secondTemp);
        if (secondTemp || offset==0)
        {
            result.SetExponent(biggerExponent+1);
            if (offset == 0 && secondTemp)
            {
                result.MoveRightFor(1, true);
            }
            else
            {
                result.MoveRightFor(1);
            }
        }
        result._sign = first._sign;
        return result;
    }
    
    public static bool operator >(FloatingPoint first, FloatingPoint second)
    {
        if (first.GetExponent() > second.GetExponent())
        {
            return true;
        }
        if (first.GetExponent() < second.GetExponent())
        {
            return false;
        }

        if (first.GetExponent() == second.GetExponent())
        {
            for (byte i = 0; i < 23; i++)
            {
                if (first._mantis[i] != second._mantis[i])
                {
                    if (first._mantis[i])
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        return false;
    }
    
    public static bool operator <(FloatingPoint first, FloatingPoint second)
    {
        if (first.GetExponent() > second.GetExponent())
        {
            return false;
        }
        if (first.GetExponent() < second.GetExponent())
        {
            return true;
        }

        if (first.GetExponent() == second.GetExponent())
        {
            for (byte i = 0; i < 23; i++)
            {
                if (first._mantis[i] != second._mantis[i])
                {
                    if (first._mantis[i])
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }
    
    public static FloatingPoint operator +(FloatingPoint first, FloatingPoint second)
    {
        FloatingPoint result = new FloatingPoint();
        if (first._sign == second._sign)
        {
            result = SummarizeOneSign(first, second, result);
        }
        else
        { 
            Console.WriteLine("No such functional");
        }
        return result;
    }
}
