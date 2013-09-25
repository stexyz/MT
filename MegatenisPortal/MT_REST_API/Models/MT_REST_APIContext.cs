using System.Data.Entity;

namespace MT_REST_API.Models {
    public class MT_REST_APIContext : DbContext {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, add the following
        // code to the Application_Start method in your Global.asax file.
        // Note: this will destroy and re-create your database with every model change.
        // 
        // System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<MT_REST_API.Models.MT_REST_APIContext>());

        public MT_REST_APIContext() : base("name=MT_REST_APIContext")
        {
        }

        public DbSet<DetailedOrderRow> DetailedOrderRows { get; set; }
    }
}
