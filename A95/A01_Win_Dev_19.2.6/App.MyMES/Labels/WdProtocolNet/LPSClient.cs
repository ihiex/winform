using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;

namespace WdProtocolNet
{
	[ComClass("282FE990-D9EB-4955-9063-1B96204EB37C", "4E7D458D-1301-4234-953A-FB18D9DC0581", "F94E01D3-8BA1-44AF-8002-FC378398C357"), ClassInterface(ClassInterfaceType.None), Guid("282FE990-D9EB-4955-9063-1B96204EB37C")]
	public class LPSClient : LPSClient.ILPSClient
	{
		[ComVisible(true), Guid("4E7D458D-1301-4234-953A-FB18D9DC0581")]
		public interface ILPSClient
		{
			[DispId(1)]
			string Response
			{
				[DispId(1)]
				get;
			}

			[DispId(2)]
			string JobNum
			{
				[DispId(2)]
				get;
			}

			[DispId(3)]
			int Port
			{
				[DispId(3)]
				get;
				[DispId(3)]
				set;
			}

			[DispId(4)]
			string JobStatus
			{
				[DispId(4)]
				get;
				[DispId(4)]
				set;
			}

			[DispId(5)]
			string JobStatusText
			{
				[DispId(5)]
				get;
			}

			[DispId(6)]
			string Login(string HostName);

			[DispId(7)]
			int PrintWait(string HostName, string JobName, string JobData, string PrinterName);

			[DispId(8)]
			int PrintJob(string HostName, string JobName, string JobData, string PrinterName);

			[DispId(9)]
			int GetJobStatus(long Job, string HostName);
		}

		public const string ClassId = "282FE990-D9EB-4955-9063-1B96204EB37C";

		public const string InterfaceId = "4E7D458D-1301-4234-953A-FB18D9DC0581";

		public const string EventsId = "F94E01D3-8BA1-44AF-8002-FC378398C357";

		private string szResponse;

		private string szJobNum;

		private string szJobStatus;

		private string szJobStatusText;

		private string szStatus;

		private int iPort;

		public string Response
		{
			get
			{
				return this.szResponse;
			}
		}

		public string JobNum
		{
			get
			{
				return this.szJobNum;
			}
		}

		public int Port
		{
			get
			{
				return this.iPort;
			}
			set
			{
				this.iPort = value;
			}
		}

		public string JobStatus
		{
			get
			{
				return this.szJobStatus;
			}
			set
			{
				this.szJobStatus = value;
			}
		}

		public string JobStatusText
		{
			get
			{
				return this.szJobStatusText;
			}
		}

		public LPSClient()
		{
			this.iPort = 2723;
		}

		public string Login(string HostName)
		{
			IPHostEntry iPHostEntry = Dns.Resolve(HostName);
			IPAddress address = iPHostEntry.AddressList[0];
			IPEndPoint remoteEP = new IPEndPoint(address, this.iPort);
			Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			byte[] array = new byte[1];
			Socket socket2 = socket;
			socket2.Connect(remoteEP);
			bool flag = !socket2.Connected;
			if (flag)
			{
				Information.Err().Raise(-10000, null, null, null, null);
			}
			string text = "FFLabel\08.4.15.0\0FFLabel\0";
			modByteFunctions.Append(modByteFunctions.HexToByte("1AFBECFD"), ref array);
			modByteFunctions.Append(modByteFunctions.HexToByte("00000205"), ref array);
			modByteFunctions.Append(modByteFunctions.HexToByte("00000001"), ref array);
			modByteFunctions.Append(modByteFunctions.HexToByte(modByteFunctions.FormatDWord(Conversion.Hex(Strings.Len(text)))), ref array);
			modByteFunctions.Append2(text, ref array);
			socket2.Send(array);
			array = new byte[1025];
			object value = socket2.Receive(array);
			flag = (array[5] != 130 | array[4] != 5);
			if (flag)
			{
				this.szResponse = Encoding.ASCII.GetString(array, 0, Conversions.ToInteger(value));
				this.szJobStatusText = this.szResponse.Substring(16);
				Information.Err().Raise(-2000, null, Encoding.ASCII.GetString(array, 16, Conversions.ToInteger(value)), null, null);
			}
			string @string = Encoding.ASCII.GetString(array, 0, Conversions.ToInteger(value));
			socket2.Shutdown(SocketShutdown.Both);
			socket2.Close();
			return @string;
		}

		//protected override void Finalize()
		//{
		//	base.Finalize();
		//}

		public int PrintWait(string HostName, string JobName, string JobData, string PrinterName)
		{
			int num=0;
			int num2=0;
			int num3;
            try
            {
                ProjectData.ClearProjectError();
                num = 2;
                this.szJobNum = "";
                this.szJobStatus = "";
                this.szJobStatusText = "";
                IPHostEntry iPHostEntry = Dns.Resolve(HostName);
                IPAddress address = iPHostEntry.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(address, 2723);
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Socket socket2 = socket;
                socket2.Connect(remoteEP);
                bool flag = !socket2.Connected;
                if (flag)
                {
                    Information.Err().Raise(-10000, null, null, null, null);
                }
                string text = string.Concat(new string[]
                {
                    "FFLabel\08.4.15\0FFLabel\0\u0001\0\0",
                    PrinterName,
                    "\0",
                    JobName,
                    "\0",
                    JobData,
                    "\0"
                });
                byte[] array = new byte[1];
                modByteFunctions.Append(modByteFunctions.HexToByte("1AFBECFD"), ref array);
                modByteFunctions.Append(modByteFunctions.HexToByte("00000206"), ref array);
                modByteFunctions.Append(modByteFunctions.HexToByte("00000001"), ref array);
                modByteFunctions.Append(modByteFunctions.HexToByte(modByteFunctions.FormatDWord(Conversion.Hex(Strings.Len(text)))), ref array);
                modByteFunctions.Append2(text, ref array);
                socket2.Send(array);
                array = new byte[1025];
                object value = socket2.Receive(array);
                this.szJobNum = Conversions.ToString(Convert.ToInt32(modByteFunctions.Format2Byte(Conversion.Hex(array[22])) + modByteFunctions.Format2Byte(Conversion.Hex(array[21])) + modByteFunctions.Format2Byte(Conversion.Hex(array[20])) + modByteFunctions.Format2Byte(Conversion.Hex(array[19])), 16));
                this.szResponse = Encoding.ASCII.GetString(array, 0, Conversions.ToInteger(value));
                string left = Conversion.Hex(array[16]);
                flag = (Operators.CompareString(left, Conversions.ToString(1), false) == 0);
                if (flag)
                {
                    this.szJobStatus = "Pending";
                }
                else
                {
                    flag = (Operators.CompareString(left, Conversions.ToString(2), false) == 0);
                    if (flag)
                    {
                        this.szJobStatus = "Critical Failure";
                    }
                    else
                    {
                        flag = (Operators.CompareString(left, Conversions.ToString(4), false) == 0);
                        if (flag)
                        {
                            this.szJobStatus = "Printed";
                        }
                        else
                        {
                            flag = (Operators.CompareString(left, Conversions.ToString(6), false) == 0);
                            if (flag)
                            {
                                this.szJobStatus = "Printed with errors";
                            }
                            else
                            {
                                this.szJobStatus = Conversions.ToString(Conversion.Int(Conversion.Hex(array[16])));
                            }
                        }
                    }
                }
                this.szJobStatusText = this.szResponse.Substring(27);
                num2 = 0;
                array = new byte[1];
                text = "";
                modByteFunctions.Append(modByteFunctions.HexToByte("1AFBECFD"), ref array);
                modByteFunctions.Append(modByteFunctions.HexToByte("0000000B"), ref array);
                modByteFunctions.Append(modByteFunctions.HexToByte("00000001"), ref array);
                modByteFunctions.Append(modByteFunctions.HexToByte(modByteFunctions.FormatDWord(Conversion.Hex(Strings.Len(text)))), ref array);
                socket2.Send(array);
                socket2.Shutdown(SocketShutdown.Both);
                socket2.Close();
                //goto IL_36B;
                //IL_317:
                //num2 = Information.Err().Number;
                //goto IL_36B;
                //num3 = -1;
                //@switch(ICSharpCode.Decompiler.ILAst.ILLabel[], num);
                //IL_33C:
                //goto IL_360;
            }
            catch
            { }
            //object arg_33E_0;
            //endfilter(arg_33E_0 is Exception & num != 0 & num3 == 0);
            //IL_360:
            //throw ProjectData.CreateProjectError(-2146828237);
            //IL_36B:
            //int arg_376_0 = num2;
            //if (num3 != 0)
            //{
            //	ProjectData.ClearProjectError();
            //}
            //return arg_376_0;

            return num2;

        }

		public int PrintJob(string HostName, string JobName, string JobData, string PrinterName)
		{
			int num=0;
			int num2=0;
			int num3;
            try
            {
                ProjectData.ClearProjectError();
                num = 2;
                this.szJobNum = "";
                this.szJobStatus = "";
                this.szJobStatusText = "";
                IPHostEntry iPHostEntry = Dns.Resolve(HostName);
                IPAddress address = iPHostEntry.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(address, 2723);
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Socket socket2 = socket;
                socket2.Connect(remoteEP);
                bool flag = !socket2.Connected;
                if (flag)
                {
                    Information.Err().Raise(-10000, null, "Error connecting to server: " + HostName, null, null);
                }
                string text = string.Concat(new string[]
                {
                    "FFLabel\08.4.15\0FFLabel\0\u0001\0\0",
                    PrinterName,
                    "\0",
                    JobName,
                    "\0",
                    JobData,
                    "\0"
                });
                byte[] array = new byte[1];
                modByteFunctions.Append(modByteFunctions.HexToByte("1AFBECFD"), ref array);
                modByteFunctions.Append(modByteFunctions.HexToByte("00000209"), ref array);
                modByteFunctions.Append(modByteFunctions.HexToByte("00000001"), ref array);
                modByteFunctions.Append(modByteFunctions.HexToByte(modByteFunctions.FormatDWord(Conversion.Hex(Strings.Len(text)))), ref array);
                modByteFunctions.Append2(text, ref array);
                socket2.Send(array);
                array = new byte[1025];
                object value = socket2.Receive(array);
                this.szJobNum = Conversions.ToString(Convert.ToInt32(modByteFunctions.Format2Byte(Conversion.Hex(array[19])) + modByteFunctions.Format2Byte(Conversion.Hex(array[18])) + modByteFunctions.Format2Byte(Conversion.Hex(array[17])) + modByteFunctions.Format2Byte(Conversion.Hex(array[16])), 16));
                this.szResponse = Encoding.ASCII.GetString(array, 0, Conversions.ToInteger(value));
                num2 = 0;
                array = new byte[1];
                text = "";
                modByteFunctions.Append(modByteFunctions.HexToByte("1AFBECFD"), ref array);
                modByteFunctions.Append(modByteFunctions.HexToByte("0000000B"), ref array);
                modByteFunctions.Append(modByteFunctions.HexToByte("00000001"), ref array);
                modByteFunctions.Append(modByteFunctions.HexToByte(modByteFunctions.FormatDWord(Conversion.Hex(Strings.Len(text)))), ref array);
                socket2.Send(array);
                socket2.Shutdown(SocketShutdown.Both);
                socket2.Close();
                //goto IL_2B4;
                //IL_250:
                //this.szJobStatusText = Information.Err().Description;
                //num2 = Information.Err().Number;
                //goto IL_2B4;
                //num3 = -1;
                //@switch(ICSharpCode.Decompiler.ILAst.ILLabel[], num);
                //IL_285:
                //goto IL_2A9;
            }
            catch { }

            //object arg_287_0;
            //endfilter(arg_287_0 is Exception & num != 0 & num3 == 0);
            //IL_2A9:
            //throw ProjectData.CreateProjectError(-2146828237);
            //IL_2B4:
            //int arg_2BF_0 = num2;
            //if (num3 != 0)
            //{
            //	ProjectData.ClearProjectError();
            //}
            //return arg_2BF_0;

            return num2;

        }

		public int GetJobStatus(long Job, string HostName)
		{
			int num=0;
			int num2=0;
			int num3;
            try
            {
                ProjectData.ClearProjectError();
                num = 2;
                this.szJobNum = Conversions.ToString(Job);
                IPHostEntry iPHostEntry = Dns.Resolve(HostName);
                IPAddress address = iPHostEntry.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(address, 2723);
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Socket socket2 = socket;
                socket2.Connect(remoteEP);
                bool flag = !socket2.Connected;
                if (flag)
                {
                    Information.Err().Raise(-10000, null, null, null, null);
                }
                byte[] array = new byte[1];
                string text = "FFLabel\08.4.15.0\0FFLabel\0";
                modByteFunctions.Append(modByteFunctions.HexToByte("1AFBECFD"), ref array);
                modByteFunctions.Append(modByteFunctions.HexToByte("00000205"), ref array);
                modByteFunctions.Append(modByteFunctions.HexToByte("00000001"), ref array);
                modByteFunctions.Append(modByteFunctions.HexToByte(modByteFunctions.FormatDWord(Conversion.Hex(Strings.Len(text)))), ref array);
                modByteFunctions.Append2(text, ref array);
                socket2.Send(array);
                array = new byte[1025];
                object value = socket2.Receive(array);
                flag = (array[5] != 130 | array[4] != 5);
                if (flag)
                {
                    this.szResponse = Encoding.ASCII.GetString(array, 0, Conversions.ToInteger(value));
                    this.szJobStatusText = this.szResponse.Substring(16);
                    Information.Err().Raise(-2000, null, Encoding.ASCII.GetString(array, 16, Conversions.ToInteger(value)), null, null);
                }
                text = Conversions.ToString(Job);
                array = new byte[1];
                modByteFunctions.Append(modByteFunctions.HexToByte("1AFBECFD"), ref array);
                modByteFunctions.Append(modByteFunctions.HexToByte("00000214"), ref array);
                modByteFunctions.Append(modByteFunctions.HexToByte("00000001"), ref array);
                modByteFunctions.Append(modByteFunctions.HexToByte(modByteFunctions.FormatDWord(Conversion.Hex("4"))), ref array);
                modByteFunctions.Append(modByteFunctions.HexToByte(modByteFunctions.FormatDWord(Conversion.Hex(text))), ref array);
                socket2.Send(array);
                array = new byte[1025];
                value = socket2.Receive(array);
                string left = Conversion.Hex(array[16]);
                flag = (Operators.CompareString(left, Conversions.ToString(1), false) == 0);
                if (flag)
                {
                    this.szJobStatus = "Pending";
                }
                else
                {
                    flag = (Operators.CompareString(left, Conversions.ToString(2), false) == 0);
                    if (flag)
                    {
                        this.szJobStatus = "Critical Failure";
                    }
                    else
                    {
                        flag = (Operators.CompareString(left, Conversions.ToString(4), false) == 0);
                        if (flag)
                        {
                            this.szJobStatus = "Printed";
                        }
                        else
                        {
                            flag = (Operators.CompareString(left, Conversions.ToString(6), false) == 0);
                            if (flag)
                            {
                                this.szJobStatus = "Printed with errors";
                            }
                            else
                            {
                                this.szJobStatus = Conversions.ToString(Conversion.Int(Conversion.Hex(array[16])));
                            }
                        }
                    }
                }
                this.szResponse = Encoding.ASCII.GetString(array, 0, Conversions.ToInteger(value));
                this.szJobStatusText = this.szResponse.Substring(27);
                num2 = Convert.ToInt32(Conversion.Hex(array[16]), 16);
                array = new byte[1];
                text = "";
                modByteFunctions.Append(modByteFunctions.HexToByte("1AFBECFD"), ref array);
                modByteFunctions.Append(modByteFunctions.HexToByte("0000000B"), ref array);
                modByteFunctions.Append(modByteFunctions.HexToByte("00000001"), ref array);
                modByteFunctions.Append2(text, ref array);
                socket2.Send(array);
                socket2.Shutdown(SocketShutdown.Both);
                socket2.Close();
                //goto IL_3DC;
                //IL_379:
                //this.szJobStatusText = Information.Err().Description;
                //num2 = Information.Err().Number;
                //goto IL_3DC;
                //num3 = -1;
                //@switch(ICSharpCode.Decompiler.ILAst.ILLabel[], num);
                //IL_3AD:
                //goto IL_3D1;
            }
            catch { }

            //object arg_3AF_0;
            //endfilter(arg_3AF_0 is Exception & num != 0 & num3 == 0);
            //IL_3D1:
            //throw ProjectData.CreateProjectError(-2146828237);
            //IL_3DC:
            //int arg_3E6_0 = num2;
            //if (num3 != 0)
            //{
            //	ProjectData.ClearProjectError();
            //}
            //return arg_3E6_0;

            return num2;
		}
	}
}
