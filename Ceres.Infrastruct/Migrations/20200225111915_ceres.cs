using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ceres.Infrastruct.Migrations
{
    public partial class ceres : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Agenter",
                columns: table => new
                {
                    OID = table.Column<Guid>(nullable: false),
                    UserName = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    Image = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    Address_Province = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: true),
                    Address_City = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agenter", x => x.OID);
                });

            migrationBuilder.CreateTable(
                name: "Component",
                columns: table => new
                {
                    OID = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    EnglishName = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    NameCode = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false),
                    Unit = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false),
                    Important = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Component", x => x.OID);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    OID = table.Column<Guid>(nullable: false),
                    UserName = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    Sex = table.Column<int>(type: "int", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Address_Province = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: true),
                    Address_City = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: true),
                    Cellphone = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false),
                    InitHeight = table.Column<double>(type: "float", nullable: false),
                    InitWeight = table.Column<double>(type: "float", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AgenterOid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupporterOid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastOperaterOid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.OID);
                });

            migrationBuilder.CreateTable(
                name: "CustomerAssistDing",
                columns: table => new
                {
                    OID = table.Column<Guid>(nullable: false),
                    QuestionnaireGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerOid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupporterOid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerAssistDing", x => x.OID);
                });

            migrationBuilder.CreateTable(
                name: "CustomerDiet",
                columns: table => new
                {
                    OID = table.Column<Guid>(nullable: false),
                    CustomerOid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceOid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Recommend_DailyEnergy = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: true),
                    Recommend_DailyComponentPercentage = table.Column<string>(type: "varchar(5000)", maxLength: 5000, nullable: true),
                    Recommend_DailyFoodComponent = table.Column<string>(type: "varchar(5000)", maxLength: 5000, nullable: true),
                    Current_DailyEnergy = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: true),
                    Current_DailyComponentPercentage = table.Column<string>(type: "varchar(5000)", maxLength: 5000, nullable: true),
                    CurrentDiet = table.Column<string>(type: "varchar(5000)", maxLength: 5000, nullable: true),
                    SupporterOid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Discard_Reason = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    LastOperate_Oid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastOperate_Time = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerDiet", x => x.OID);
                });

            migrationBuilder.CreateTable(
                name: "CustomerDislikeFood",
                columns: table => new
                {
                    OID = table.Column<Guid>(nullable: false),
                    CustomerOid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FoodOid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OperaterOid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerDislikeFood", x => x.OID);
                });

            migrationBuilder.CreateTable(
                name: "CustomerJob",
                columns: table => new
                {
                    OID = table.Column<Guid>(nullable: false),
                    Job_Name = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: true),
                    Job_Strength = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: true),
                    CustomerOid = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerJob", x => x.OID);
                });

            migrationBuilder.CreateTable(
                name: "CustomerService",
                columns: table => new
                {
                    OID = table.Column<Guid>(nullable: false),
                    CustomerOid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceOid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerService", x => x.OID);
                });

            migrationBuilder.CreateTable(
                name: "Food",
                columns: table => new
                {
                    OID = table.Column<Guid>(nullable: false),
                    Classify = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    Type = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    Code = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    Coding = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    Name = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    Image = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    Click = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Food", x => x.OID);
                });

            migrationBuilder.CreateTable(
                name: "FoodComponent",
                columns: table => new
                {
                    OID = table.Column<Guid>(nullable: false),
                    FoodOid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ComponentOid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<double>(type: "float", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodComponent", x => x.OID);
                });

            migrationBuilder.CreateTable(
                name: "Service",
                columns: table => new
                {
                    OID = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    Description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    Image = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Service", x => x.OID);
                });

            migrationBuilder.CreateTable(
                name: "Supporter",
                columns: table => new
                {
                    OID = table.Column<Guid>(nullable: false),
                    LoginName = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    Cellphone = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false),
                    Password = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false),
                    UserName = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    Image = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supporter", x => x.OID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Agenter");

            migrationBuilder.DropTable(
                name: "Component");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "CustomerAssistDing");

            migrationBuilder.DropTable(
                name: "CustomerDiet");

            migrationBuilder.DropTable(
                name: "CustomerDislikeFood");

            migrationBuilder.DropTable(
                name: "CustomerJob");

            migrationBuilder.DropTable(
                name: "CustomerService");

            migrationBuilder.DropTable(
                name: "Food");

            migrationBuilder.DropTable(
                name: "FoodComponent");

            migrationBuilder.DropTable(
                name: "Service");

            migrationBuilder.DropTable(
                name: "Supporter");
        }
    }
}
