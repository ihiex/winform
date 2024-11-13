using System;
using System.Collections;

namespace MESLabel
{
	public class MESFuncObjFactory
	{
		private static MESFuncObjFactory m_factory;

		private const int m_Capacity = 30;

		private Hashtable m_HT;

		private MESFuncObject m_boolFuncObj;

		public MESFuncObject BoolFuncObj
		{
			get
			{
				return this.m_boolFuncObj;
			}
		}

		public ICollection FuncNames
		{
			get
			{
				return this.m_HT.Keys;
			}
		}

		public ICollection FuncObjs
		{
			get
			{
				return this.m_HT.Values;
			}
		}

		private MESFuncObjFactory()
		{
			this.m_HT = new Hashtable(30);
			this.m_boolFuncObj = new Bool("Bool");
			this.Initialize();
		}

		public MESFuncObject GetFuncObject(string name)
		{
            MESFuncObject result = null;
            if (this.m_HT != null)
			{
                result=(MESFuncObject)this.m_HT[name.ToUpper()];
			}
			
			return result;
		}

		public void AddFuncObject(string name, MESFuncObject funcobj)
		{
			if (this.m_HT != null)
			{
				this.m_HT.Add(name.ToUpper(), funcobj);
			}
		}

		public void Initialize()
		{
			if (this.m_HT != null)
			{
				this.AddFuncObject("DecToHex", new DecToHex("DecToHex"));
				this.AddFuncObject("DecToOct", new DecToOct("DecToOct"));
				this.AddFuncObject("HexToDec", new HexToDec("HexToDec"));
				this.AddFuncObject("OctToDec", new OctToDec("OctToDec"));
				this.AddFuncObject("Right", new Right("Right"));
				this.AddFuncObject("Left", new Left("Left"));
				this.AddFuncObject("Substr", new Substr("Substr"));
				this.AddFuncObject("If", new If("If"));
				this.AddFuncObject("ApplyMap", new ApplyMap("ApplyMap"));
				this.AddFuncObject("Year", new Year("Year"));
				this.AddFuncObject("Century", new Centry("Century"));
				this.AddFuncObject("Month", new Month("Month"));
				this.AddFuncObject("Day", new Day("Day"));
				this.AddFuncObject("Hour12", new Hour12("Hour12"));
				this.AddFuncObject("Hour24", new Hour24("Hour24"));
				this.AddFuncObject("Minute", new Minute("Minute"));
				this.AddFuncObject("WeekOfYear", new WeekOfYear("WeekOfYear"));
				this.AddFuncObject("DayOfYear", new DayOfYear("DayOfYear"));
				this.AddFuncObject("PrinterName", new PrinterName("PrinterName"));
				this.AddFuncObject("Len", new Len("Len"));
				this.AddFuncObject("Add", new Add("Add"));
				this.AddFuncObject("Subst", new Subst("Subst"));
				this.AddFuncObject("Div", new Div("Div"));
				this.AddFuncObject("Multiply", new Multiply("Multiply"));
				this.AddFuncObject("Mod", new Mod("Mod"));
				this.AddFuncObject("PageNumber", new PageNumber("PageNumber"));
				this.AddFuncObject("PageCount", new PageCount("PageCount"));
				this.AddFuncObject("SourceFilePath", new SourceFilePath("SourceFilePath"));
				this.AddFuncObject("YearOfManWeek", new YearOfManWeek("YearOfManWeek"));
			}
		}

		public static MESFuncObjFactory CreateInstance()
		{
			if (MESFuncObjFactory.m_factory == null)
			{
				MESFuncObjFactory.m_factory = new MESFuncObjFactory();
			}
			return MESFuncObjFactory.m_factory;
		}
	}
}
