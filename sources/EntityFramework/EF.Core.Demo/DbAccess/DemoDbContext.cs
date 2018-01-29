using Microsoft.EntityFrameworkCore;

namespace EF.Core.Demo.DbAccess
{
    public class DemoDbContext:DbContext
    {

        public DemoDbContext() 
        {
        }


        public DbSet<User> Users { get; set; }

        public DbSet<Student> Students { get; set; }

        public DbSet<Blog> Blogs { get; set; }

        public DbSet<Post> Posts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(@"Server=US-01;database=EF.Core.Demo;uid=root;pwd=3.1415926;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


          

            #region 各自的配置都分散到不同的模型配置中  非侵入方式，推荐 



            modelBuilder.ApplyConfiguration(new UserConfiguration());


            modelBuilder.ApplyConfiguration(new StudentConfiguration());







            #endregion


            #region 在当前方法中运行   不推荐 

            //modelBuilder.Entity<User>()
            //    .Ignore(b => b.BirthDay);


            #endregion




            base.OnModelCreating(modelBuilder);
        }
       

    }

    
}