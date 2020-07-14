using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyCBA.Core.Models
{
    public class ApplicationDbContext :  DbContext
    {
        public ApplicationDbContext()
            : base("Db2")
        {
        }
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<SavingsAcctMgt>().Property(e => e.LoanAmountRemaining).HasPrecision(20, 10);
        //}
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Properties<decimal>().Configure(config => config.HasPrecision(20, 10));
            modelBuilder.Entity<SavingsAcctMgt>().Property(e => e.SavingsCreditInterestRate).HasPrecision(20, 2);
            modelBuilder.Entity<AccountConfiguration>().Property(e => e.SavingsCreditInterestRate).HasPrecision(20, 2);
            modelBuilder.Entity<SavingsAcctMgt>().Property(e => e.SavingsMinimumBalance).HasPrecision(20, 2);
            modelBuilder.Entity<AccountConfiguration>().Property(e => e.SavingsMinimumBalance).HasPrecision(20, 2);
            modelBuilder.Entity<CurrentAcctMgt>().Property(e => e.CurrentCot).HasPrecision(20, 2);
            modelBuilder.Entity<AccountConfiguration>().Property(e => e.CurrentCot).HasPrecision(20, 2);
            modelBuilder.Entity<CurrentAcctMgt>().Property(e => e.CurrentCreditInterestRate).HasPrecision(20, 2);
            modelBuilder.Entity<AccountConfiguration>().Property(e => e.CurrentCreditInterestRate).HasPrecision(20, 2);
            modelBuilder.Entity<CurrentAcctMgt>().Property(e => e.CurrentMinimumBalance).HasPrecision(20, 2);
            modelBuilder.Entity<AccountConfiguration>().Property(e => e.CurrentMinimumBalance).HasPrecision(20, 2);
            modelBuilder.Entity<LoanAcctMgt>().Property(e => e.LoanDebitInterestRate).HasPrecision(20, 2);
            modelBuilder.Entity<AccountConfiguration>().Property(e => e.LoanDebitInterestRate).HasPrecision(20, 2);

        }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Branch> Branches { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<GlCategory> GlCategories { get; set; }

        public DbSet<GlAccount> GlAccounts { get; set; }

        public DbSet<Teller> Tellers { get; set; }

        public DbSet<CustomerAccount> CustomerAccounts { get; set; }

        public DbSet<SavingsAcctMgt> SavingsAcctMgts { get; set; }

        public DbSet<CurrentAcctMgt> CurrentAcctMgts { get; set; }

        public DbSet<LoanAcctMgt> LoanAcctMgts { get; set; }

        public DbSet<GlPosting> GlPostings { get; set; }
        public DbSet<TellerPosting> TellerPostings { get; set; }

        public DbSet<BusinessConfig> BusinessConfigs { get; set; }

        public DbSet<LoanCustAcct> LoanCustAccts { get; set; }

        public DbSet<ExpensesIncomeEntry> ExpensesIncomeEntries { get; set; }
        public DbSet<AccountConfiguration> AccountConfigurations { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<COTLog> COTLogs { get; set; }
        public DbSet<Node> Nodes { get; set; }
        public DbSet<SinkNode> SinkNodes { get; set; }
        public DbSet<AtmTerminal> AtmTerminals { get; set; }

        /*
         * protected override void Seed(MyCBA.Core.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            var getBranch = context.Branches.ToList().Count();
            var getRole = context.Roles.ToList().Count();
            if (getBranch == 0)
            {
                context.Branches.AddOrUpdate(x => x.id, new Branch() { name = "Yaba" });

            }
            if (getRole == 0)
            {
                context.Roles.AddOrUpdate(x => x.id, new Role() { name = "Admin" });
            }
        }
         */
    }
}
