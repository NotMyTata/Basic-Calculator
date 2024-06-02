using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("Tuliskan ekspresi matematika: \t ex = ( 5 * 3 ) + 2");
        string[] input = Console.ReadLine().Split(' ');

        var postfix = new Postfix();
        postfix.evaluateEquation(input);

        double result = postfix.operateEquation();
        Console.WriteLine($"Hasil: {result}");
    }
}

class Postfix
{
    Queue<string> postfix = new Queue<string>();
    public double operateEquation()
    {
        if (postfix.Count == 0)
        {
            throw new Exception("Postfix empty");
        }

        Stack<double> temp = new Stack<double>();

        while (postfix.Count > 0)
        {
            string line = postfix.Dequeue();
            bool isNumeric = int.TryParse(line, out _);
            if (isNumeric)
            {
                temp.Push(double.Parse(line));
            }
            else
            {
                double a = temp.Pop();
                double b = temp.Pop();
                switch (line)
                {
                    case "+": temp.Push(b + a); break;
                    case "-": temp.Push(b - a); break;
                    case "*": temp.Push(b * a); break;
                    case "/": temp.Push(b / a); break;
                    case "^": temp.Push(Math.Pow(b, a)); break;
                    default: throw new Exception("Invalid Expression");
                }
            }
        }

        return temp.Pop();
    }
    static int determineHierarchy(string line)
    {
        if (line == "+" || line == "-")
        {
            return 1;
        }
        if (line == "*" || line == "/")
        {
            return 2;
        }
        if (line == "^")
        {
            return 3;
        }
        return 0;
    }

    public void evaluateEquation(string[] input)
    {
        Stack<string> operators = new Stack<string>();
        int bracketCount = 0;

        foreach (string line in input)
        {
            bool isNumeric = int.TryParse(line, out _);

            if (isNumeric)
            {
                postfix.Enqueue(line);
            }
            else
            {
                if (line == ")")
                {
                    if (bracketCount == 0)
                    {
                        throw new Exception("Invalid Expression");
                    }
                    else
                    {
                        string temp = operators.Pop();
                        while (temp != "(")
                        {
                            postfix.Enqueue(temp);
                            temp = operators.Pop();
                        }
                        bracketCount--;
                    }
                }
                else if (line == "(")
                {
                    operators.Push(line);
                    bracketCount++;
                }
                else
                {
                    if (operators.Count != 0)
                    {
                        int curHierarchy = determineHierarchy(line);
                        int stackHierarchy = determineHierarchy(operators.Peek());
                        while (curHierarchy <= stackHierarchy)
                        {
                            postfix.Enqueue(operators.Pop());
                            if (operators.Count > 0)
                            {
                                stackHierarchy = determineHierarchy(operators.Peek());
                            }
                            else
                            {
                                stackHierarchy = -1;
                            }
                        }
                    }
                    operators.Push(line);
                }
            }
        }

        while (operators.Count > 0)
        {
            postfix.Enqueue(operators.Pop());
        }
    }
}
