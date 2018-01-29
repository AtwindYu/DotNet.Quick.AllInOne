using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

namespace EF6.Demo.DbAccess
{
    public class DemoDbContext:DbContext
    {

        public DemoDbContext() : base("EF6.DEMO")
        {
        }


        public IDbSet<User> Users { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
            //各自的配置都分散到不同的模型配置中
           
            modelBuilder.Configurations.Add(new UserConfiguration());
          

            base.OnModelCreating(modelBuilder);

        }



    }

    
}