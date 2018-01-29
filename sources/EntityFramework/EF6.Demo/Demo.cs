using System;
using EF6.Demo.DbAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EF6.Demo
{
    [TestClass]
    public class Demo
    {


        /// <summary>
        /// 当第一次运行创建了Db中的结构后，只要变更了相关的数据模型结构，都要进行数据迁移，或者会异常。
        /// </summary>
        [TestMethod]
        public void Run1()
        {
            using (var db = new DemoDbContext())
            {
                //db.Users.Add(new User() { Address = "test1" });
                //db.Users.Add(new User() { Address = "test2" });
                //db.Users.Add(new User() { Address = "test3" });
                //db.Users.Add(new User() { Address = "test4" });
                db.SaveChanges();
            }

        }
    }
}
