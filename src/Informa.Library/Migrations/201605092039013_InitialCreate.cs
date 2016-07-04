namespace Informa.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ArticleMappings",
                c => new
                    {
                        PmbiArticleId = c.Guid(nullable: false, identity: true),
                        ArticleId = c.Guid(nullable: false),
                        ArticleNumber = c.String(),
                        PmbiArticleNumber = c.String(),
                    })
                .PrimaryKey(t => t.PmbiArticleId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ArticleMappings");
        }
    }
}
