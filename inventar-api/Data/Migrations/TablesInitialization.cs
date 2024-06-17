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
        
        // Users table
        Create.Table("Users")
            .WithColumn("Id").AsString(256).PrimaryKey().PrimaryKey()
            .WithColumn("UserName").AsString(256).Nullable()
            .WithColumn("NormalizedUserName").AsString(256).Nullable()
            .WithColumn("Email").AsString(256).Nullable()
            .WithColumn("NormalizedEmail").AsString(256).Nullable()
            .WithColumn("EmailConfirmed").AsBoolean().NotNullable()
            .WithColumn("PasswordHash").AsString().Nullable()
            .WithColumn("SecurityStamp").AsString().Nullable()
            .WithColumn("ConcurrencyStamp").AsString().Nullable()
            .WithColumn("PhoneNumber").AsString().Nullable()
            .WithColumn("PhoneNumberConfirmed").AsBoolean().NotNullable()
            .WithColumn("TwoFactorEnabled").AsBoolean().NotNullable()
            .WithColumn("LockoutEnd").AsDateTime().Nullable()
            .WithColumn("LockoutEnabled").AsBoolean().NotNullable()
            .WithColumn("AccessFailedCount").AsInt32().NotNullable()
            .WithColumn("Name").AsString().NotNullable()
            .WithColumn("Age").AsInt32().Nullable()
            .WithColumn("Gender").AsString().NotNullable()
            .WithColumn("Discriminator").AsString().NotNullable();

            // AspNetRoles table
            Create.Table("AspNetRoles")
                .WithColumn("Id").AsString(256).PrimaryKey()
                .WithColumn("Name").AsString(256).Nullable()
                .WithColumn("NormalizedName").AsString(256).Nullable()
                .WithColumn("ConcurrencyStamp").AsString().Nullable();

            // AspNetUserRoles table
            Create.Table("AspNetUserRoles")
                .WithColumn("UserId").AsString(256).NotNullable()
                .WithColumn("RoleId").AsString(256).NotNullable()
                .ForeignKey("FK_AspNetUserRoles_Users", "Users", "Id")
                .ForeignKey("FK_AspNetUserRoles_AspNetRoles", "AspNetRoles", "Id");

            // AspNetRoleClaims table
            Create.Table("AspNetRoleClaims")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("RoleId").AsString(256).NotNullable()
                .WithColumn("ClaimType").AsString().Nullable()
                .WithColumn("ClaimValue").AsString().Nullable()
                .ForeignKey("FK_AspNetRoleClaims_AspNetRoles", "AspNetRoles", "Id");

            // AspNetUserClaims table
            Create.Table("AspNetUserClaims")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("UserId").AsString(256).NotNullable()
                .WithColumn("ClaimType").AsString().Nullable()
                .WithColumn("ClaimValue").AsString().Nullable()
                .ForeignKey("FK_AspNetUserClaims_Users", "Users", "Id");

            // AspNetUserLogins table
            Create.Table("AspNetUserLogins")
                .WithColumn("LoginProvider").AsString(256).PrimaryKey()
                .WithColumn("ProviderKey").AsString(256).PrimaryKey()
                .WithColumn("ProviderDisplayName").AsString().Nullable()
                .WithColumn("UserId").AsString(256).NotNullable()
                .ForeignKey("FK_AspNetUserLogins_Users", "Users", "Id");

            // AspNetUserTokens table
            Create.Table("AspNetUserTokens")
                .WithColumn("UserId").AsString(256).PrimaryKey()
                .WithColumn("LoginProvider").AsString(256).PrimaryKey()
                .WithColumn("Name").AsString(256).PrimaryKey()
                .WithColumn("Value").AsString().Nullable()
                .ForeignKey("FK_AspNetUserTokens_Users", "Users", "Id");

    }
    
    public override void Down()
    {
        Delete.ForeignKey("FK_ArticleLocations_Location").OnTable("articlelocations");
        Delete.ForeignKey("FK_ArticleLocations_Article").OnTable("articlelocations");
        Delete.Index("IX_ArticleLocations_LocationId").OnTable("articlelocations");
        Delete.Index("IX_ArticleLocations_ArticleId").OnTable("articlelocations");
        Delete.Table("articlelocations");
        Delete.Table("locations");
        Delete.Table("articles");
        
        Delete.Table("AspNetUserTokens");
        Delete.Table("AspNetUserLogins");
        Delete.Table("AspNetUserClaims");
        Delete.Table("AspNetRoleClaims");
        Delete.Table("AspNetUserRoles");
        Delete.Table("AspNetRoles");
        Delete.Table("Users");

    }

    private void CreateArticlesTable()
    {
        Create.Table("articles")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("code").AsInt32().NotNullable().Unique()
            .WithColumn("name").AsString(128).NotNullable();
    }

    private void CreateLocationsTable()
    {
        Create.Table("locations")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("code").AsString(128).NotNullable().Unique();
    }

    private void CreateArticleLocationsTable()
    {
        Create.Table("articlelocations")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("articleCode").AsInt32().NotNullable()
            .WithColumn("locationCode").AsString(128).NotNullable()
            .WithColumn("count").AsInt32().NotNullable();
    }

    private void CreateIndexes()
    {
        Create.Index("IX_ArticleLocations_ArticleCode").OnTable("articlelocations").OnColumn("articleCode").Ascending().WithOptions().NonClustered();
        Create.Index("IX_ArticleLocations_LocationCode").OnTable("articlelocations").OnColumn("locationCode").Ascending().WithOptions().NonClustered();
    }

    private void CreateForeignKeys()
    {
        Create.ForeignKey("FK_ArticleLocations_Article").FromTable("articlelocations").ForeignColumn("articleCode").ToTable("articles").PrimaryColumn("code").OnDelete(Rule.Cascade);
        Create.ForeignKey("FK_ArticleLocations_Location").FromTable("articlelocations").ForeignColumn("locationCode").ToTable("locations").PrimaryColumn("code").OnDelete(Rule.Cascade);
    }
}