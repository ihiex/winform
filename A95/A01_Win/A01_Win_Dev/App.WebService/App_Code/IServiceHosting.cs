using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

[ServiceContract]
public interface IServiceHostingSVC
{
    /// <summary>
    /// 加法
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    [OperationContract]
    double ADD(double x, double y);

    /// <summary>
    /// 减法
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    [OperationContract]
    double substruction(double x, double y);

    /// <summary>
    /// 乘法
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    [OperationContract]
    double multiplication(double x, double y);

    /// <summary>
    /// 除法
    /// </summary>
    /// <param name="x">被除数</param>
    /// <param name="y">除数</param>
    /// <returns></returns>
    [OperationContract(Name = "division")]
    double division(double x, double y);
}