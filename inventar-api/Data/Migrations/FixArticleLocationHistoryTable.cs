using FluentMigrator;

namespace inventar_api.Data.Migrations;

[Migration(313052024)]
public class FixArticleLocationHistoryTable : Migration
{
    public override void Up()
    {
        Alter.Column("date").OnTable("articlelocationhistory").AsDateTime().NotNullable();
    }

    public override void Down()
    {
        Alter.Column("date").OnTable("articlelocationhistory").AsDate().NotNullable();
    }
}