using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;

namespace WdProtocolNet
{
	[StandardModule]
	internal sealed class modByteFunctions
	{
		public static byte[] HexToByte(string szHexNumber)
		{
			checked
			{
				byte[] array = new byte[(int)Math.Round(unchecked((double)Strings.Len(szHexNumber) / 2.0 - 1.0)) + 1];
				int num = 0;
				int num2 = Strings.Len(szHexNumber) - 1;
				while (true)
				{
					int arg_67_0 = num2;
					int num3 = 1;
					if (arg_67_0 < num3)
					{
						break;
					}
					array[num] = (byte)Math.Round(Conversion.Val("&H" + Strings.Mid(szHexNumber, num2, 2)));
					num++;
					num2 += -2;
				}
				return array;
			}
		}

		public static void Append(byte[] Source, ref byte[] bufDestiny)
		{
			bool flag = Information.UBound(bufDestiny, 1) == 0 & bufDestiny[0] == 0;
			int num=0;
			if (flag)
			{
				num = -1;
			}
			else
			{
				num = Information.UBound(bufDestiny, 1);
			}
			checked
			{
				bufDestiny = (byte[])Utils.CopyArray((Array)bufDestiny, new byte[num + Information.UBound(Source, 1) + 1 + 1]);
				int arg_55_0 = 0;
				int num2 = Information.UBound(Source, 1);
				int num3 = arg_55_0;
				while (true)
				{
					int arg_6E_0 = num3;
					int num4 = num2;
					if (arg_6E_0 > num4)
					{
						break;
					}
					bufDestiny[num + 1 + num3] = Source[num3];
					num3++;
				}
			}
		}

		public static void Append2(string Source, ref byte[] bufDestiny)
		{
			bool flag = Information.UBound(bufDestiny, 1) == 0 & bufDestiny[0] == 0;
			int num=0;
			if (flag)
			{
				num = -1;
			}
			else
			{
				num = Information.UBound(bufDestiny, 1);
			}
			checked
			{
				bufDestiny = (byte[])Utils.CopyArray((Array)bufDestiny, new byte[num + Strings.Len(Source) + 1]);
				int arg_53_0 = 0;
				int num2 = Strings.Len(Source) - 1;
				int num3 = arg_53_0;
				while (true)
				{
					int arg_79_0 = num3;
					int num4 = num2;
					if (arg_79_0 > num4)
					{
						break;
					}
					bufDestiny[num + 1 + num3] = (byte)Strings.Asc(Strings.Mid(Source, num3 + 1, 1));
					num3++;
				}
			}
		}

		public static string FormatDWord(string szHexNumber)
		{
			return Strings.StrDup(checked(8 - Strings.Len(szHexNumber)), "0") + szHexNumber;
		}

		public static string Format2Byte(string szHexNumber)
		{
			return Strings.StrDup(checked(2 - Strings.Len(szHexNumber)), "0") + szHexNumber;
		}
	}
}
