using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EF.Core.Demo.DbAccess
{
    public class Student
    {

        public int Id { get; set; }

        public int UserId { get; set; }

        public string Username { get; set; }

        public string Realname { get; set; }

        public SexType Sex { get; set; }

        public byte[] Timestamp { get; set; }



    }

    internal class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder
                .ToTable("Student") //映射表名称
                .HasKey(x => x.Id) //设定主键名称
                
                
                ;

            builder.Property(x => x.UserId).ValueGeneratedNever(); //不创建默认值
            builder.HasIndex(x => x.Realname); //索引
            builder.HasIndex(x => new {x.UserId, x.Realname});//复合索引
            builder.Property(x => x.Realname).HasMaxLength(20);//最大长度
            builder.Property(x => x.Username).HasMaxLength(50);//最大长度


            builder.Property(x => x.Timestamp).IsRowVersion(); //时间戳/行版本

        }
    }


}