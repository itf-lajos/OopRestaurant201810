namespace OopRestaurant201810.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTablesdatabasetabla : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tables",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tables");
        }
    }
}
