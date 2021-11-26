using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DirectoryServices;

namespace CreateUser
{
    class Program
    {
        private static string PATH = "WinNT://" + Environment.MachineName;
        public static void AddUser(string username,string password,string group,string description)
        {
            using (DirectoryEntry dir = new DirectoryEntry(PATH))
            {
                 using (DirectoryEntry user = dir.Children.Add(username, "User"))
                {
                    user.Properties["FullName"].Add(username);
                    user.Invoke("SetPassword",password);
                    user.Invoke("Put", "UserFlags", 66049); //密码永不过期
                    user.Invoke("Put", "UserFlags", 0x0040);//用户不能更改密码
                    user.CommitChanges();//保存用户
                    using (DirectoryEntry grp = dir.Children.Find(group, "group"))
                    {
                        Console.WriteLine(grp);
                        Console.ReadKey();
                        if (grp.Name != "")
                        {
                            grp.Invoke("Add", user.Path.ToString());//将用户添加到某组
                        }
                    }


                }
                
            }
        }


        static void Main(string[] args)
        {
            AddUser("super","qwer.1234","administrators","root");
        }
    }
}
