using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App.MyMES
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            
                //即然能找到这来的基本上应该是对Codesoft有所了解了，所以基础的codesoft软件怎么用就不再讲，软件本身很简单，看看就会了，附件中有lab可以自己看一下
                //1、在工程中添加Lppx2.tlb引用（这个Lppx2.tlb视codesoft版本而定，在6下是在安装目录，在7下是在C:\Program Files (x86)\Tki\7\Common或者C:\Program Files\Tki\7\Common
                //2、添加引用后，在项目的引用下找到LabelManager2,将其属性中的嵌入互操作类型改为False，不然运行会出错。
                //3、代码开始
                LabelManager2.Application lbl = null;
            LabelManager2.Document doc = null;
                try
                {
                    lbl = new LabelManager2.Application();
                    lbl.Documents.Open(System.Windows.Forms.Application.StartupPath + @"\Document2.Lab", false);// 调用设计好的label文件
                    doc = lbl.ActiveDocument;

                    //这里我是用循环随便做了个测试，你可以改成任意的数据源的数据，比如datatable或者数组什么的，相信能找到这个资源的人不太可能不会用这些。
                    for (int i = 0; i < 50; i++)
                    {
                        doc.Variables.FormVariables.Item("Var1").Value = "Sample" + i; //给参数传值,var1是我标签中一个填充器的名字
                                                                                       //这里注意一下，你在codesoft中新建的填充器是可以改名的，这个名字更改做的很隐蔽，添加填充器后，需要以慢速双击一下那个填充器就可以改了（双击速度比平时放慢就行了）

                        doc.PrintLabel(1);//按lab文件中的设置打印标签，即几行几列等等，这个方法还有其它参数可以自己试
                    }
                    doc.FormFeed();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    lbl.Documents.CloseAll();
                    lbl.Quit();//退出
                    lbl = null;
                    doc = null;
                    GC.Collect(0);
                }
            
        }
    }
}
