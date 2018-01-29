using System;
using System.Data.Entity.ModelConfiguration;

namespace EF6.Demo.DbAccess
{
    public class User
    {
        public int Id { get; set; }


        public SexType Sex { get; set; }


        public string Username { get; set; }

        public string Address { get; set; }

        public DateTime? CreateTime { get; set; }

        public TimeSpan? TimeStamp { get; set; }

        public DateTime? BirthDay { get; set; }

    }

    internal class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            ToTable("User")
                .HasKey(x => x.Id)
                
            ;
            //HasKey(t => t.CId).Property(t => t.CId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            //Property(t => t.CName).IsRequired().HasMaxLength(50);
            Property(x => x.BirthDay).HasColumnType("date");
        }
    }

    public enum SexType
    {
        Unknow = 0,

        Male = 1,

        Female = 2,
    }

}