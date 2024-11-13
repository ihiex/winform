using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MESUpdate
{
    public class Public_
    {


        /// <summary>
        /// 获取指定目录中的匹配项（文件或目录）
        /// </summary>
        /// <param name="dir">要搜索的目录</param>
        /// <param name="regexPattern">项名模式（正则）。null表示忽略模式匹配，返回所有项</param>
        /// <param name="recurse">是否搜索子目录</param>
        /// <param name="throwEx">是否抛异常</param>
        /// <returns></returns>
        public string[] GetFileSystemEntries(string dir, string regexPattern = null, bool recurse = false, bool throwEx = false)
        {
            List<string> lst = new List<string>();

            try
            {
                foreach (string item in Directory.GetFileSystemEntries(dir))
                {
                    try
                    {
                        if (regexPattern == null || Regex.IsMatch(Path.GetFileName(item), regexPattern, RegexOptions.IgnoreCase | RegexOptions.Multiline))
                        { lst.Add(item); }

                        //递归
                        if (recurse && (File.GetAttributes(item) & FileAttributes.Directory) == FileAttributes.Directory)
                        { lst.AddRange(GetFileSystemEntries(item, regexPattern, true)); }
                    }
                    catch { if (throwEx) { throw; } }
                }
            }
            catch { if (throwEx) { throw; } }

            return lst.ToArray();
        }

        /// <summary>
        /// 获取指定目录中的匹配文件
        /// </summary>
        /// <param name="dir">要搜索的目录</param>
        /// <param name="regexPattern">文件名模式（正则）。null表示忽略模式匹配，返回所有文件</param>
        /// <param name="recurse">是否搜索子目录</param>
        /// <param name="throwEx">是否抛异常</param>
        /// <returns></returns>
        public string[] GetFiles(string dir, string regexPattern = null, bool recurse = false, bool throwEx = false)
        {
            List<string> lst = new List<string>();

            try
            {
                foreach (string item in Directory.GetFileSystemEntries(dir))
                {
                    try
                    {
                        bool isFile = (File.GetAttributes(item) & FileAttributes.Directory) != FileAttributes.Directory;

                        if (isFile && (regexPattern == null || Regex.IsMatch(Path.GetFileName(item), regexPattern, RegexOptions.IgnoreCase | RegexOptions.Multiline)))
                        { lst.Add(item); }

                        //递归
                        if (recurse && !isFile) { lst.AddRange(GetFiles(item, regexPattern, true)); }
                    }
                    catch { if (throwEx) { throw; } }
                }
            }
            catch { if (throwEx) { throw; } }

            return lst.ToArray();
        }

        /// <summary>
        /// 获取指定目录中的匹配目录
        /// </summary>
        /// <param name="dir">要搜索的目录</param>
        /// <param name="regexPattern">目录名模式（正则）。null表示忽略模式匹配，返回所有目录</param>
        /// <param name="recurse">是否搜索子目录</param>
        /// <param name="throwEx">是否抛异常</param>
        /// <returns></returns>
        public string[] GetDirectories(string dir, string regexPattern = null, bool recurse = false, bool throwEx = false)
        {
            List<string> lst = new List<string>();

            try
            {
                foreach (string item in Directory.GetDirectories(dir))
                {
                    try
                    {
                        if (regexPattern == null || Regex.IsMatch(Path.GetFileName(item), regexPattern, RegexOptions.IgnoreCase | RegexOptions.Multiline))
                        { lst.Add(item); }

                        //递归
                        if (recurse) { lst.AddRange(GetDirectories(item, regexPattern, true)); }
                    }
                    catch { if (throwEx) { throw; } }
                }
            }
            catch { if (throwEx) { throw; } }

            return lst.ToArray();
        }
    }
}
