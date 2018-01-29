using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EF.Core.Demo.DbAccess
{
    public class User
    {
        public int Id { get; set; }


        public SexType Sex { get; set; }


        public string Username { get; set; }

        public string Address { get; set; }

        public DateTime CreateTime { get; set; }

        //public TimeSpan? TimeStamp { get; set; }

        public DateTime? UpdateTime { get; set; }


        
        public string LastName { get; set; }


    }


    public enum SexType
    {
        Unknow = 0,

        Male = 1,

        Female = 2,
    }




    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            //builder.HasKey(x => x.Username); //已经设定过主键的，不能更新了。

            builder
                .ToTable("User")
                .HasKey(x=>x.Id)

                ;

            builder.Property(x => x.CreateTime).ValueGeneratedOnAdd(); //在增加时必须设定值，如果是空就抛出异常。
            builder.Property(x => x.UpdateTime).ValueGeneratedOnAddOrUpdate(); //可空类型并不触发异常。

            builder.Property(x => x.Username).HasMaxLength(50).IsRequired(); //必须有内容

            builder.Property(x => x.LastName).IsConcurrencyToken();//并发标记





        }
    }

    //下面这个是EF6中的设定
    //internal class UserConfiguration : EntityTypeConfiguration<User>
    //{
    //    public UserConfiguration()
    //    {
    //        ToTable("User")
    //            .HasKey(x => x.Id)

    //        ;
    //        //HasKey(t => t.CId).Property(t => t.CId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
    //        //Property(t => t.CName).IsRequired().HasMaxLength(50);
    //        Property(x => x.BirthDay).HasColumnType("date");
    //    }
    //}


}