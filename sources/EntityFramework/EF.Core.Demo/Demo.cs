using System;
using System.Linq;
using EF.Core.Demo.DbAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EF.Core.Demo
{
    [TestClass]
    public class Demo
    {
        [TestMethod]
        public void RunInsert()
        {
            using (var db = new DemoDbContext())
            {
                db.Users.Add(new User() {Username = "123",CreateTime = DateTime.Now});
                db.Users.Add(new User() {Username = "1232342", CreateTime = DateTime.Now });
                db.SaveChanges();
            }
        }


        [TestMethod]
        public void RunInsertOne()
        {
            using (var db = new DemoDbContext())
            {
                db.Users.Add(new User() {  CreateTime = DateTime.Now });
                db.SaveChanges();
            }
        }

        [TestMethod]
        public void RunUpdate()
        {
            using (var db = new DemoDbContext())
            {
                var u = db.Users.Single(x => x.Id == 1);
                u.Username = "hello";
                db.SaveChanges();
            }
        }

        [TestMethod]
        public void TimeStampTest()
        {
            using (var db = new DemoDbContext())
            {
                var u = db.Students.Add(new Student() {Username = "test"});
                db.SaveChanges();
            }
        }


    }
}
