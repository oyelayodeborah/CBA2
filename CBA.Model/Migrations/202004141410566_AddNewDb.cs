namespace MyCBA.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccountConfigurations",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        IsBusinessOpen = c.Boolean(nullable: false),
                        FinancialDate = c.DateTime(nullable: false),
                        SavingsCreditInterestRate = c.Decimal(nullable: false, precision: 20, scale: 2),
                        SavingsMinimumBalance = c.Decimal(nullable: false, precision: 20, scale: 2),
                        SavingsInterestExpenseGLId = c.Int(),
                        SavingsInterestPayableGLId = c.Int(),
                        CurrentCreditInterestRate = c.Decimal(nullable: false, precision: 20, scale: 2),
                        CurrentMinimumBalance = c.Decimal(nullable: false, precision: 20, scale: 2),
                        CurrentCot = c.Decimal(nullable: false, precision: 20, scale: 2),
                        CurrentInterestExpenseGLId = c.Int(),
                        CurrentInterestPayableGLId = c.Int(),
                        CurrentCotIncomeGLId = c.Int(),
                        LoanDebitInterestRate = c.Decimal(nullable: false, precision: 20, scale: 2),
                        LoanInterestIncomeGLId = c.Int(),
                        LoanInterestExpenseGLId = c.Int(),
                        LoanInterestReceivableGLId = c.Int(),
                        status = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.GlAccounts", t => t.CurrentCotIncomeGLId)
                .ForeignKey("dbo.GlAccounts", t => t.CurrentInterestExpenseGLId)
                .ForeignKey("dbo.GlAccounts", t => t.CurrentInterestPayableGLId)
                .ForeignKey("dbo.GlAccounts", t => t.LoanInterestExpenseGLId)
                .ForeignKey("dbo.GlAccounts", t => t.LoanInterestIncomeGLId)
                .ForeignKey("dbo.GlAccounts", t => t.LoanInterestReceivableGLId)
                .ForeignKey("dbo.GlAccounts", t => t.SavingsInterestExpenseGLId)
                .ForeignKey("dbo.GlAccounts", t => t.SavingsInterestPayableGLId)
                .Index(t => t.SavingsInterestExpenseGLId)
                .Index(t => t.SavingsInterestPayableGLId)
                .Index(t => t.CurrentInterestExpenseGLId)
                .Index(t => t.CurrentInterestPayableGLId)
                .Index(t => t.CurrentCotIncomeGLId)
                .Index(t => t.LoanInterestIncomeGLId)
                .Index(t => t.LoanInterestExpenseGLId)
                .Index(t => t.LoanInterestReceivableGLId);
            
            CreateTable(
                "dbo.GlAccounts",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        GlCategoryId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 40),
                        Code = c.Long(nullable: false),
                        BranchId = c.Int(nullable: false),
                        acountBalance = c.Decimal(nullable: false, precision: 20, scale: 10),
                        assignToTeller = c.String(),
                        mainCategory = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Branches", t => t.BranchId, cascadeDelete: false)
                .ForeignKey("dbo.GlCategories", t => t.GlCategoryId, cascadeDelete: false)
                .Index(t => t.GlCategoryId)
                .Index(t => t.BranchId);
            
            CreateTable(
                "dbo.Branches",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.GlCategories",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 40),
                        code = c.Long(nullable: false),
                        description = c.String(nullable: false, maxLength: 150),
                        mainAccountCategory = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.AtmTerminals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Code = c.String(nullable: false),
                        Location = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BusinessConfigs",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        FinancialDate = c.DateTime(nullable: false),
                        IsBusinessOpen = c.Boolean(nullable: false),
                        DayCount = c.Int(nullable: false),
                        MonthCount = c.Int(nullable: false),
                        YearCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.COTLogs",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        customerId = c.Int(nullable: false),
                        customerAcctNum = c.Long(nullable: false),
                        GlAccountToCreditId = c.Int(nullable: false),
                        GlAccountToCreditCode = c.Long(nullable: false),
                        TransactionDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.CurrentAcctMgts",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        CurrentCreditInterestRate = c.Decimal(nullable: false, precision: 20, scale: 2),
                        CurrentMinimumBalance = c.Decimal(nullable: false, precision: 20, scale: 2),
                        CurrentCot = c.Decimal(nullable: false, precision: 20, scale: 2),
                        CurrentInterestExpenseGLId = c.Int(),
                        CurrentInterestPayableGLId = c.Int(),
                        CurrentCotIncomeGLId = c.Int(),
                        status = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.GlAccounts", t => t.CurrentCotIncomeGLId)
                .ForeignKey("dbo.GlAccounts", t => t.CurrentInterestExpenseGLId)
                .ForeignKey("dbo.GlAccounts", t => t.CurrentInterestPayableGLId)
                .Index(t => t.CurrentInterestExpenseGLId)
                .Index(t => t.CurrentInterestPayableGLId)
                .Index(t => t.CurrentCotIncomeGLId);
            
            CreateTable(
                "dbo.CustomerAccounts",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        customerId = c.Int(nullable: false),
                        acctName = c.String(nullable: false, maxLength: 60),
                        acctNumber = c.Long(nullable: false),
                        branchId = c.Int(nullable: false),
                        accType = c.String(nullable: false),
                        status = c.String(),
                        acctbalance = c.Decimal(nullable: false, precision: 20, scale: 10),
                        dailyInterestAccrued = c.Decimal(nullable: false, precision: 20, scale: 10),
                        createdAt = c.DateTime(nullable: false),
                        isLinked = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Branches", t => t.branchId, cascadeDelete: false)
                .ForeignKey("dbo.Customers", t => t.customerId, cascadeDelete: false)
                .Index(t => t.customerId)
                .Index(t => t.branchId);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        customerID = c.String(),
                        name = c.String(nullable: false, maxLength: 225),
                        address = c.String(nullable: false, maxLength: 225),
                        phoneNumber = c.String(nullable: false, maxLength: 11),
                        email = c.String(nullable: false, maxLength: 225),
                        gender = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.ExpensesIncomeEntries",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Amount = c.Decimal(nullable: false, precision: 20, scale: 10),
                        Date = c.DateTime(nullable: false),
                        AccountName = c.String(),
                        EntryType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.GlPostings",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        GlAccountToDebitId = c.Int(nullable: false),
                        GlAccountToDebitCode = c.Long(nullable: false),
                        GlAccountToCreditId = c.Int(nullable: false),
                        GlAccountToCreditCode = c.Long(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 20, scale: 10),
                        Narration = c.String(nullable: false, maxLength: 255),
                        userId = c.Int(nullable: false),
                        status = c.String(),
                        report = c.String(),
                        TransactionDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.GlAccounts", t => t.GlAccountToCreditId, cascadeDelete: false)
                .ForeignKey("dbo.GlAccounts", t => t.GlAccountToDebitId, cascadeDelete: false)
                .ForeignKey("dbo.Users", t => t.userId, cascadeDelete: false)
                .Index(t => t.GlAccountToDebitId)
                .Index(t => t.GlAccountToCreditId)
                .Index(t => t.userId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        fullName = c.String(nullable: false, maxLength: 225),
                        email = c.String(nullable: false, maxLength: 225),
                        username = c.String(nullable: false, maxLength: 25),
                        passwordHash = c.String(nullable: false, maxLength: 225),
                        phoneNumber = c.String(nullable: false, maxLength: 11),
                        roleId = c.Int(nullable: false),
                        branchId = c.Int(nullable: false),
                        IsAssigned = c.String(),
                        LoggedIn = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Branches", t => t.branchId, cascadeDelete: false)
                .ForeignKey("dbo.Roles", t => t.roleId, cascadeDelete: false)
                .Index(t => t.roleId)
                .Index(t => t.branchId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.LoanAcctMgts",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        LoanDebitInterestRate = c.Decimal(nullable: false, precision: 20, scale: 2),
                        LoanInterestIncomeGLId = c.Int(),
                        LoanInterestExpenseGLId = c.Int(),
                        LoanInterestReceivableGLId = c.Int(),
                        status = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.GlAccounts", t => t.LoanInterestExpenseGLId)
                .ForeignKey("dbo.GlAccounts", t => t.LoanInterestIncomeGLId)
                .ForeignKey("dbo.GlAccounts", t => t.LoanInterestReceivableGLId)
                .Index(t => t.LoanInterestIncomeGLId)
                .Index(t => t.LoanInterestExpenseGLId)
                .Index(t => t.LoanInterestReceivableGLId);
            
            CreateTable(
                "dbo.LoanCustAccts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ServicingAccountId = c.Int(nullable: false),
                        AccountNumber = c.Long(nullable: false),
                        LoanAmount = c.Decimal(nullable: false, precision: 20, scale: 10),
                        Interest = c.Decimal(nullable: false, precision: 20, scale: 10),
                        InterestRate = c.Decimal(nullable: false, precision: 20, scale: 10),
                        LoanAmountReduction = c.Decimal(nullable: false, precision: 20, scale: 10),
                        LoanInterestReduction = c.Decimal(nullable: false, precision: 20, scale: 10),
                        LoanAmountRemaining = c.Decimal(nullable: false, precision: 20, scale: 10),
                        LoanInterestRemaining = c.Decimal(nullable: false, precision: 20, scale: 10),
                        LoanMonthlyInterestRepay = c.Decimal(nullable: false, precision: 20, scale: 10),
                        LoanMonthlyPrincipalRepay = c.Decimal(nullable: false, precision: 20, scale: 10),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        DaysCount = c.Int(nullable: false),
                        DurationInMonths = c.Int(nullable: false),
                        status = c.String(),
                        termsOfLoan = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CustomerAccounts", t => t.ServicingAccountId, cascadeDelete: false)
                .Index(t => t.ServicingAccountId);
            
            CreateTable(
                "dbo.Nodes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        HostName = c.String(nullable: false),
                        IPAddress = c.String(nullable: false),
                        Port = c.String(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SavingsAcctMgts",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        SavingsCreditInterestRate = c.Decimal(nullable: false, precision: 20, scale: 2),
                        SavingsMinimumBalance = c.Decimal(nullable: false, precision: 20, scale: 2),
                        SavingsInterestExpenseGLId = c.Int(),
                        SavingsInterestPayableGLId = c.Int(),
                        status = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.GlAccounts", t => t.SavingsInterestExpenseGLId)
                .ForeignKey("dbo.GlAccounts", t => t.SavingsInterestPayableGLId)
                .Index(t => t.SavingsInterestExpenseGLId)
                .Index(t => t.SavingsInterestPayableGLId);
            
            CreateTable(
                "dbo.TellerPostings",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        customerId = c.Int(nullable: false),
                        customerAcctNum = c.Long(nullable: false),
                        postingType = c.String(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 20, scale: 10),
                        Narration = c.String(nullable: false, maxLength: 255),
                        TransactionDate = c.DateTime(nullable: false),
                        status = c.String(nullable: false),
                        userId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Customers", t => t.customerId, cascadeDelete: false)
                .ForeignKey("dbo.Users", t => t.userId, cascadeDelete: false)
                .Index(t => t.customerId)
                .Index(t => t.userId);
            
            CreateTable(
                "dbo.Tellers",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        userId = c.Int(nullable: false),
                        TillAccountId = c.Int(nullable: false),
                        tillAccountBalance = c.Decimal(nullable: false, precision: 20, scale: 10),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.GlAccounts", t => t.TillAccountId, cascadeDelete: false)
                .ForeignKey("dbo.Users", t => t.userId, cascadeDelete: false)
                .Index(t => t.userId)
                .Index(t => t.TillAccountId);
            
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Amount = c.Decimal(nullable: false, precision: 20, scale: 10),
                        Date = c.DateTime(nullable: false),
                        AccountName = c.String(),
                        SubCategory = c.String(),
                        MainCategory = c.String(),
                        TransactionType = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tellers", "userId", "dbo.Users");
            DropForeignKey("dbo.Tellers", "TillAccountId", "dbo.GlAccounts");
            DropForeignKey("dbo.TellerPostings", "userId", "dbo.Users");
            DropForeignKey("dbo.TellerPostings", "customerId", "dbo.Customers");
            DropForeignKey("dbo.SavingsAcctMgts", "SavingsInterestPayableGLId", "dbo.GlAccounts");
            DropForeignKey("dbo.SavingsAcctMgts", "SavingsInterestExpenseGLId", "dbo.GlAccounts");
            DropForeignKey("dbo.LoanCustAccts", "ServicingAccountId", "dbo.CustomerAccounts");
            DropForeignKey("dbo.LoanAcctMgts", "LoanInterestReceivableGLId", "dbo.GlAccounts");
            DropForeignKey("dbo.LoanAcctMgts", "LoanInterestIncomeGLId", "dbo.GlAccounts");
            DropForeignKey("dbo.LoanAcctMgts", "LoanInterestExpenseGLId", "dbo.GlAccounts");
            DropForeignKey("dbo.GlPostings", "userId", "dbo.Users");
            DropForeignKey("dbo.Users", "roleId", "dbo.Roles");
            DropForeignKey("dbo.Users", "branchId", "dbo.Branches");
            DropForeignKey("dbo.GlPostings", "GlAccountToDebitId", "dbo.GlAccounts");
            DropForeignKey("dbo.GlPostings", "GlAccountToCreditId", "dbo.GlAccounts");
            DropForeignKey("dbo.CustomerAccounts", "customerId", "dbo.Customers");
            DropForeignKey("dbo.CustomerAccounts", "branchId", "dbo.Branches");
            DropForeignKey("dbo.CurrentAcctMgts", "CurrentInterestPayableGLId", "dbo.GlAccounts");
            DropForeignKey("dbo.CurrentAcctMgts", "CurrentInterestExpenseGLId", "dbo.GlAccounts");
            DropForeignKey("dbo.CurrentAcctMgts", "CurrentCotIncomeGLId", "dbo.GlAccounts");
            DropForeignKey("dbo.AccountConfigurations", "SavingsInterestPayableGLId", "dbo.GlAccounts");
            DropForeignKey("dbo.AccountConfigurations", "SavingsInterestExpenseGLId", "dbo.GlAccounts");
            DropForeignKey("dbo.AccountConfigurations", "LoanInterestReceivableGLId", "dbo.GlAccounts");
            DropForeignKey("dbo.AccountConfigurations", "LoanInterestIncomeGLId", "dbo.GlAccounts");
            DropForeignKey("dbo.AccountConfigurations", "LoanInterestExpenseGLId", "dbo.GlAccounts");
            DropForeignKey("dbo.AccountConfigurations", "CurrentInterestPayableGLId", "dbo.GlAccounts");
            DropForeignKey("dbo.AccountConfigurations", "CurrentInterestExpenseGLId", "dbo.GlAccounts");
            DropForeignKey("dbo.AccountConfigurations", "CurrentCotIncomeGLId", "dbo.GlAccounts");
            DropForeignKey("dbo.GlAccounts", "GlCategoryId", "dbo.GlCategories");
            DropForeignKey("dbo.GlAccounts", "BranchId", "dbo.Branches");
            DropIndex("dbo.Tellers", new[] { "TillAccountId" });
            DropIndex("dbo.Tellers", new[] { "userId" });
            DropIndex("dbo.TellerPostings", new[] { "userId" });
            DropIndex("dbo.TellerPostings", new[] { "customerId" });
            DropIndex("dbo.SavingsAcctMgts", new[] { "SavingsInterestPayableGLId" });
            DropIndex("dbo.SavingsAcctMgts", new[] { "SavingsInterestExpenseGLId" });
            DropIndex("dbo.LoanCustAccts", new[] { "ServicingAccountId" });
            DropIndex("dbo.LoanAcctMgts", new[] { "LoanInterestReceivableGLId" });
            DropIndex("dbo.LoanAcctMgts", new[] { "LoanInterestExpenseGLId" });
            DropIndex("dbo.LoanAcctMgts", new[] { "LoanInterestIncomeGLId" });
            DropIndex("dbo.Users", new[] { "branchId" });
            DropIndex("dbo.Users", new[] { "roleId" });
            DropIndex("dbo.GlPostings", new[] { "userId" });
            DropIndex("dbo.GlPostings", new[] { "GlAccountToCreditId" });
            DropIndex("dbo.GlPostings", new[] { "GlAccountToDebitId" });
            DropIndex("dbo.CustomerAccounts", new[] { "branchId" });
            DropIndex("dbo.CustomerAccounts", new[] { "customerId" });
            DropIndex("dbo.CurrentAcctMgts", new[] { "CurrentCotIncomeGLId" });
            DropIndex("dbo.CurrentAcctMgts", new[] { "CurrentInterestPayableGLId" });
            DropIndex("dbo.CurrentAcctMgts", new[] { "CurrentInterestExpenseGLId" });
            DropIndex("dbo.GlAccounts", new[] { "BranchId" });
            DropIndex("dbo.GlAccounts", new[] { "GlCategoryId" });
            DropIndex("dbo.AccountConfigurations", new[] { "LoanInterestReceivableGLId" });
            DropIndex("dbo.AccountConfigurations", new[] { "LoanInterestExpenseGLId" });
            DropIndex("dbo.AccountConfigurations", new[] { "LoanInterestIncomeGLId" });
            DropIndex("dbo.AccountConfigurations", new[] { "CurrentCotIncomeGLId" });
            DropIndex("dbo.AccountConfigurations", new[] { "CurrentInterestPayableGLId" });
            DropIndex("dbo.AccountConfigurations", new[] { "CurrentInterestExpenseGLId" });
            DropIndex("dbo.AccountConfigurations", new[] { "SavingsInterestPayableGLId" });
            DropIndex("dbo.AccountConfigurations", new[] { "SavingsInterestExpenseGLId" });
            DropTable("dbo.Transactions");
            DropTable("dbo.Tellers");
            DropTable("dbo.TellerPostings");
            DropTable("dbo.SavingsAcctMgts");
            DropTable("dbo.Nodes");
            DropTable("dbo.LoanCustAccts");
            DropTable("dbo.LoanAcctMgts");
            DropTable("dbo.Roles");
            DropTable("dbo.Users");
            DropTable("dbo.GlPostings");
            DropTable("dbo.ExpensesIncomeEntries");
            DropTable("dbo.Customers");
            DropTable("dbo.CustomerAccounts");
            DropTable("dbo.CurrentAcctMgts");
            DropTable("dbo.COTLogs");
            DropTable("dbo.BusinessConfigs");
            DropTable("dbo.AtmTerminals");
            DropTable("dbo.GlCategories");
            DropTable("dbo.Branches");
            DropTable("dbo.GlAccounts");
            DropTable("dbo.AccountConfigurations");
        }
    }
}
