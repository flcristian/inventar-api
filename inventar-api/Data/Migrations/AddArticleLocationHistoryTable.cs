using System.Data;
using FluentMigrator;

namespace inventar_api.Data.Migrations;

[Migration(228042024)]
public class AddArticleLocationHistoryTable : Migration
{
    public override void Up()
    {
        CreateArticleLocationsTable();
        CreateIndexes();
        CreateForeignKeys();
    }

    public override void Down()
    {
        Delete.ForeignKey("FK_ArticleLocationHistory_Article").OnTable("articlelocationhistory");
        Delete.ForeignKey("FK_ArticleLocationHistory_Location").OnTable("articlelocationhistory");
        Delete.Index("IX_ArticleLocationHistory_ArticleCode").OnTable("articlelocationhistory");
        Delete.Index("IX_ArticleLocationHistory_LocationCode").OnTable("articlelocationhistory");
        Delete.Table("articlelocationhistory");
    }
    
    private void CreateArticleLocationsTable()
    {
        Create.Table("articlelocationhistory")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("date").AsDate().NotNullable()
            .WithColumn("articleCode").AsInt32().NotNullable()
            .WithColumn("locationCode").AsString(128).NotNullable()
            .WithColumn("stockIn").AsInt32().NotNullable()
            .WithColumn("stockOut").AsInt32().NotNullable()
            .WithColumn("order").AsInt32().NotNullable()
            .WithColumn("necessary").AsInt32().NotNullable()
            .WithColumn("source").AsString(128).NotNullable();
    }

    private void CreateIndexes()
    {
        Create.Index("IX_ArticleLocationHistory_ArticleCode").OnTable("articlelocationhistory").OnColumn("articleCode").Ascending().WithOptions().NonClustered();
        Create.Index("IX_ArticleLocationHistory_LocationCode").OnTable("articlelocationhistory").OnColumn("locationCode").Ascending().WithOptions().NonClustered();
    }

    private void CreateForeignKeys()
    {
        Create.ForeignKey("FK_ArticleLocationHistory_Article").FromTable("articlelocationhistory").ForeignColumn("articleCode").ToTable("articles").PrimaryColumn("code").OnDelete(Rule.Cascade);
        Create.ForeignKey("FK_ArticleLocationHistory_Location").FromTable("articlelocationhistory").ForeignColumn("locationCode").ToTable("locations").PrimaryColumn("code").OnDelete(Rule.Cascade);
    }
}