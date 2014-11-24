namespace MediaDataApplication.WcfService.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Media",
                c => new
                    {
                        MediaId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MediaId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.MediaMetadatas",
                c => new
                    {
                        MediaId = c.Int(nullable: false),
                        FileName = c.String(nullable: false, maxLength: 100),
                        FileLength = c.Long(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.MediaId)
                .ForeignKey("dbo.Media", t => t.MediaId, cascadeDelete: true)
                .Index(t => t.MediaId)
                .Index(t => t.FileName, unique: true);
            
            CreateTable(
                "dbo.MediaThumbnails",
                c => new
                    {
                        MediaId = c.Int(nullable: false),
                        FileName = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.MediaId)
                .ForeignKey("dbo.Media", t => t.MediaId, cascadeDelete: true)
                .Index(t => t.MediaId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 30),
                        Password = c.String(nullable: false, maxLength: 30),
                        FirstName = c.String(maxLength: 30),
                        LastName = c.String(maxLength: 30),
                        CreationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .Index(t => t.UserName, unique: true);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Media", "UserId", "dbo.Users");
            DropForeignKey("dbo.MediaThumbnails", "MediaId", "dbo.Media");
            DropForeignKey("dbo.MediaMetadatas", "MediaId", "dbo.Media");
            DropIndex("dbo.Users", new[] { "UserName" });
            DropIndex("dbo.MediaThumbnails", new[] { "MediaId" });
            DropIndex("dbo.MediaMetadatas", new[] { "FileName" });
            DropIndex("dbo.MediaMetadatas", new[] { "MediaId" });
            DropIndex("dbo.Media", new[] { "UserId" });
            DropTable("dbo.Users");
            DropTable("dbo.MediaThumbnails");
            DropTable("dbo.MediaMetadatas");
            DropTable("dbo.Media");
        }
    }
}
