using System.Linq;

public static int Max(params int?[] numbers)
{
    return numbers.Max() ?? 0;
}
