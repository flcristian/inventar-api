using System.Data;
using FluentMigrator;

namespace inventar_api.Data.Migrations;

[Migration(108042024)]
public class TablesInitialization : Migration
{
    public override void Up()
    {
        CreateArticlesTable();
        CreateLocationsTable();
        CreateArticleLocationsTable();
        CreateIndexes();
        CreateForeignKeys();
    }
    
    public override void Down()
    {
        Delete.ForeignKey("FK_ArticleLocations_Location").OnTable("articleLocations");
        Delete.ForeignKey("FK_ArticleLocations_Article").OnTable("articleLocations");
        Delete.Index("IX_ArticleLocations_LocationId").OnTable("articleLocations");
        Delete.Index("IX_ArticleLocations_ArticleId").OnTable("articleLocations");
        Delete.Table("articlelocations");
        Delete.Table("locations");
        Delete.Table("articles");
    }

    private void CreateArticlesTable()
    {
        Create.Table("articles")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("code").AsInt32().NotNullable().Unique()
            .WithColumn("name").AsString(128).NotNullable()
            .WithColumn("consumption").AsString(128).NotNullable()
            .WithColumn("machinery").AsString(128).NotNullable();
    }

    private void CreateLocationsTable()
    {
        Create.Table("locations")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("code").AsString().NotNullable().Unique();
    }

    private void CreateArticleLocationsTable()
    {
        Create.Table("articlelocations")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("articleId").AsInt32().NotNullable()
            .WithColumn("locationId").AsInt32().NotNullable()
            .WithColumn("count").AsInt32().NotNullable();
    }

    private void CreateIndexes()
    {
        Create.Index("IX_ArticleLocations_ArticleId").OnTable("articleLocations").OnColumn("articleId").Ascending().WithOptions().NonClustered();
        Create.Index("IX_ArticleLocations_LocationId").OnTable("articleLocations").OnColumn("locationId").Ascending().WithOptions().NonClustered();
    }

    private void CreateForeignKeys()
    {
        Create.ForeignKey("FK_ArticleLocations_Article").FromTable("articleLocations").ForeignColumn("articleId").ToTable("articles").PrimaryColumn("id").OnDelete(Rule.Cascade);
        Create.ForeignKey("FK_ArticleLocations_Location").FromTable("articleLocations").ForeignColumn("locationId").ToTable("locations").PrimaryColumn("id").OnDelete(Rule.Cascade);
    }
}