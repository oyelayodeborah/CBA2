using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyCBA.Core.Models;
using MyCBA.Logic;
using ClosedXML;
using ClosedXML.Excel;
using Syncfusion.XlsIO;


namespace MyCBA.Controllers
{
    [AdminRoleRestrictLogic]
    public class FinancialReportController : Controller
    {
        
        // GET: FinancialReport
        public ActionResult BalanceSheet()
        {
            BalanceSheetLogic bsLogic = new BalanceSheetLogic();
            try
            {
                ApplicationDbContext _context = new ApplicationDbContext();
                var getConfig = _context.AccountConfigurations.ToList().Count();
                AccountConfiguration accountConfig = new AccountConfiguration();
                if (getConfig != 0)
                {
                    accountConfig = _context.AccountConfigurations.ToList().Single();

                }
                if (accountConfig != null)
                {
                    ViewBag.TableTitle = accountConfig.FinancialDate.ToString("D");
                }
                else
                {
                    ViewBag.TableTitle = DateTime.Now.ToString("D");
                }
                //get all assets
                var assets = bsLogic.GetAssetAccounts();
                ViewBag.Assets = assets;
                ViewBag.AssetSum = assets.Sum(a => a.acountBalance);
                //get all capitals
                var capitals = bsLogic.GetCapitalAccounts();
                ViewBag.Capitals = capitals;
                ViewBag.CapitalSum = capitals.Sum(c => c.acountBalance);
                //get all liablilities
                var liabilities = bsLogic.GetLiabilityAccounts();
                ViewBag.Liability = liabilities;
                ViewBag.LiabilitySum = liabilities.Sum(l => l.Amount);
                return View();
            }
            catch (Exception)
            {
                //ErrorLogger.Log("Message= " + ex.Message + "\nInner Exception= " + ex.InnerException + "\n");
                return PartialView("Error");
            }
            //return View();
        }

        public ActionResult ProfitLoss(string date1, string date2)
        {
            ProfitLossLogic plLogic = new ProfitLossLogic();
            ApplicationDbContext _context = new ApplicationDbContext();
            ExpensesIncomeEntry expIncEntry = new ExpensesIncomeEntry();

            TempData["message"] = "";
            try
            {
                var entries = plLogic.GetEntries();
                var getConfig = _context.AccountConfigurations.ToList().Count();
                AccountConfiguration accountConfig = new AccountConfiguration();
                if (getConfig != 0)
                {
                    accountConfig = _context.AccountConfigurations.ToList().Single();

                }
                if (accountConfig != null)
                {
                    ViewBag.TableTitle = "as at " + accountConfig.FinancialDate.ToString("D");
                }
                else
                {
                    ViewBag.TableTitle = "as at " + DateTime.Now.ToString("D");
                }
                /*ViewBag.TableTitle = "as at " + DateTime.Now.ToString("D");*///_context.BusinessConfigs.First().FinancialDate.ToString("D");
                if (Convert.ToDateTime(date1) > Convert.ToDateTime(date2))
                {
                    TempData["message"] = "Error";
                }
                
                if (!(String.IsNullOrEmpty(date1) || (String.IsNullOrEmpty(date2))))
                {
                    entries = plLogic.GetEntries(Convert.ToDateTime(date1), Convert.ToDateTime(date2));
                    ViewBag.TableTitle = "Between " + Convert.ToDateTime(date1).ToString("D") + " and " + Convert.ToDateTime(date2).ToString("D");
                }

                //entries = entries.OrderBy(e => e.EntryType).ToList();
                var sortedEntries = new List<ExpensesIncomeEntry>();
                foreach (var entry in entries)
                {
                    var item = sortedEntries.FirstOrDefault(s => s.AccountName.ToUpper().Equals(entry.AccountName.ToUpper()));
                    if (item == null)
                    {
                        item = entry;
                        sortedEntries.Add(item);
                    }
                    else    //for item(s) that occur(s) twice, amt spent or earned = difference of the balances for the two occurences (days)
                    {
                        item.Amount -= entry.Amount;         //getting the difference in the account balances within the specified dates
                    }
                }
                ViewBag.SumOfIncome = sortedEntries.Where(en => en.EntryType == ExpensesIncomeEntry.PandLType.Income).Sum(e => e.Amount);
                ViewBag.SumOfExpense = sortedEntries.Where(en => en.EntryType == ExpensesIncomeEntry.PandLType.Expenses).Sum(e => e.Amount);
                ViewBag.Profit = (decimal)ViewBag.SumOfIncome - (decimal)ViewBag.SumOfExpense;

                return View(sortedEntries);
                
            }
            catch (Exception)
            {
                //ErrorLogger.Log("Message= " + ex.Message + "\nInner Exception= " + ex.InnerException + "\n");
                return PartialView("Error");
            }
        }
        public ActionResult TrialBalance(string date1, string date2)
        {
            ApplicationDbContext _context = new ApplicationDbContext();
            TrialBalanceLogic trialBalLogic = new TrialBalanceLogic();
            var getConfig = _context.AccountConfigurations.ToList().Count();
            AccountConfiguration accountConfig = new AccountConfiguration();
            if (getConfig != 0)
            {
                 accountConfig = _context.AccountConfigurations.ToList().Single();

            }
            
            if (accountConfig != null)
            {
                ViewBag.TableTitle = "as at " + accountConfig.FinancialDate.ToString("D");
            }
            else
            {
                ViewBag.TableTitle = "as at " + DateTime.Now.ToString("D");
            }
            TempData["message"] = "";

            try
            {
                var transactions = _context.Transactions.ToList();

                if (Convert.ToDateTime(date1) > Convert.ToDateTime(date2))
                {
                    TempData["message"] = "Error";
                }
                    if (!(String.IsNullOrEmpty(date1) || String.IsNullOrEmpty(date2)))
                    {
                        transactions = trialBalLogic.GetTrialBalanceTransactions(Convert.ToDateTime(date1), Convert.ToDateTime(date2));
                        ViewBag.TableTitle = "Between " + Convert.ToDateTime(date1).ToString("D") + " and " + Convert.ToDateTime(date2).ToString("D");
                    }
                    transactions = transactions.OrderBy(t => t.MainCategory).ToList();


                    var viewModel = new List<TrialBalanceViewModels>();
                    decimal totalDebit = 0, totalCredit = 0;
                    foreach (var transaction in transactions)
                    {
                        var model = viewModel.FirstOrDefault(i => i.AccountName.ToLower().Equals((transaction.AccountName.ToLower())));
                        if (model == null)   //add new
                        {
                            model = new TrialBalanceViewModels();
                            model.AccountName = transaction.AccountName;
                            model.SubCategory = transaction.SubCategory;
                            model.MainCategory = transaction.MainCategory.ToString();
                            model.TotalCredit = transaction.TransactionType == "Credit" ? transaction.Amount : 0;
                            model.TotalDebit = transaction.TransactionType == "Debit" ? transaction.Amount : 0;
                            viewModel.Add(model);

                            totalCredit += model.TotalCredit;
                            totalDebit += model.TotalDebit;

                        }
                        else    //continue with the item
                        {
                            decimal amt = transaction.Amount;
                            if (transaction.TransactionType == "Credit")
                            {
                                model.TotalCredit += amt;
                                totalCredit += amt;
                            }
                            else if (transaction.TransactionType == "Debit")
                            {
                                model.TotalDebit += amt;
                                totalDebit += amt;
                            }
                        }
                    }//end foreach

                    ViewBag.TotalCredit = totalCredit;
                    ViewBag.TotalDebit = totalDebit;
                    return View(viewModel);
                //}
                //else
                //{
                //    TempData["message"] = "Error";
                //    return View();
                //}
            }
            catch (Exception)
            {
                //ErrorLogger.Log("Message= " + ex.Message + "\nInner Exception= " + ex.InnerException + "\n");
                return PartialView("Error");
            }
            
        }

        public ActionResult ExportToExcel()
        {
            BalanceSheetLogic bsLogic = new BalanceSheetLogic();

            List<GlAccount> asset = bsLogic.GetAssetAccounts();
            List<GlAccount> capital = bsLogic.GetCapitalAccounts();
            List<LiabilityViewModel> liabilities = bsLogic.GetLiabilityAccounts();


            //Inizializar Librerias
            var workbook = new XLWorkbook();
            workbook.AddWorksheet("sheetName");
            var ws = workbook.Worksheet("sheetName");
            //Recorrer el objecto
            int row = 1;
            decimal Assetsum = 0;
            decimal CapitalSum = 0;
            decimal LiabilitySum = 0;

            ws.Cell("A" + row.ToString()).Value = "DebsCBA Balance Sheet";
            row++;
            row++;
            ws.Cell("A" + row.ToString()).Value = "Account";
            ws.Cell("B"+ row.ToString()).Value = "Amount";
            row++;
            row++;
            ws.Cell("A" + row.ToString()).Value = "Asset";
            ws.Cell("B" + row.ToString()).Value = "N";
            row++;
            foreach (var c in asset)
            {
                //Escribrie en Excel en cada celda
                ws.Cell("A" + row.ToString()).Value = c.Name;
                ws.Cell("B" + row.ToString()).Value = c.acountBalance.ToString("N02");
                Assetsum = Assetsum + c.acountBalance;
                row++;
            }
            ws.Cell("A" + row.ToString()).Value = "Total";

            ws.Cell("B" + row.ToString()).Value = Assetsum.ToString("N02");
            row++;
            row++;


            ws.Cell("A" + row.ToString()).Value = "Capital";
            ws.Cell("B" + row.ToString()).Value = "N";
            row++;
            foreach (var c in capital)
            {
                //Escribrie en Excel en cada celda
                ws.Cell("A" + row.ToString()).Value = c.Name;
                ws.Cell("B" + row.ToString()).Value = c.acountBalance.ToString("N02");
                CapitalSum = CapitalSum + c.acountBalance;
                row++;
            }
            ws.Cell("A" + row.ToString()).Value = "Total";

            ws.Cell("B" + row.ToString()).Value = CapitalSum.ToString("N02");
            row++;
            row++;


            ws.Cell("A" + row.ToString()).Value = "Liability";
            ws.Cell("B" + row.ToString()).Value = "N";
            row++;
            foreach (var c in liabilities)
            {
                //Escribrie en Excel en cada celda
                ws.Cell("A" + row.ToString()).Value = c.AccountName;
                ws.Cell("B" + row.ToString()).Value = c.Amount.ToString("N02");
                LiabilitySum = LiabilitySum + c.Amount;
                row++;
            }
            ws.Cell("A" + row.ToString()).Value = "Total";

            ws.Cell("B" + row.ToString()).Value = LiabilitySum.ToString("N02");
            row++;
            row++;
            ws.Cell("A" + row.ToString()).Value = "Capital + Liability";
            var CapitalLiabilityTotal = CapitalSum + LiabilitySum;
            ws.Cell("B" + row.ToString()).Value = CapitalLiabilityTotal.ToString("N02");
            workbook.SaveAs("C:\\Users\\OYELAYO\\source\\repos\\MyCBA\\MyCBA.Logic\\DebsCBA-BalanceSheet.xlsx");
            var path = "C:\\Users\\OYELAYO\\source\\repos\\MyCBA\\MyCBA.Logic\\DebsCBA-BalanceSheet.xlsx";
            return File(path, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

            //string url = this.Request.UrlReferrer.AbsolutePath;
            //return Redirect(url);
        }
}
    }
