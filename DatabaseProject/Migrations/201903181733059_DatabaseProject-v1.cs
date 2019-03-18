namespace DatabaseProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DatabaseProjectv1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        College = c.String(nullable: false, maxLength: 10),
                        Department = c.String(nullable: false, maxLength: 20),
                        Name = c.String(nullable: false),
                        Rank = c.String(nullable: false),
                        Salary_SalaryId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Salaries", t => t.Salary_SalaryId)
                .Index(t => t.Salary_SalaryId);
            
            CreateTable(
                "dbo.Salaries",
                c => new
                    {
                        SalaryId = c.Int(nullable: false, identity: true),
                        SalaryAmount = c.Int(nullable: false),
                        Year = c.Short(nullable: false),
                    })
                .PrimaryKey(t => t.SalaryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Employees", "Salary_SalaryId", "dbo.Salaries");
            DropIndex("dbo.Employees", new[] { "Salary_SalaryId" });
            DropTable("dbo.Salaries");
            DropTable("dbo.Employees");
        }
    }
}
