using System;
using System.Collections.Generic;
using System.Text;

public class ServiceHostingSVC : IServiceHostingSVC
{
    public double ADD(double x, double y)
    {
        return x + y;
    }

    public double substruction(double x, double y)
    {
        return x - y;
    }

    public double multiplication(double x, double y)
    {
        return x * y;
    }

    public double division(double x, double y)
    {
        if (y == 0)
        {
            throw new Exception("除数不能为零");
        }
        else
        {
            return x / y;
        }
    }
}