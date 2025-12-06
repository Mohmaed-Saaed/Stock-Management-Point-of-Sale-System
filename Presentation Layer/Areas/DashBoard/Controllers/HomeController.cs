using CoreLayer;
using CoreLayer.Models;
using InfrastructureLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using PresentationLayer.Areas.DashBoard.ViewModels;
using System.Threading.Tasks;

namespace PresentationLayer.Areas.DashBoard.Controllers
{
    [Area("DashBoard")]
    //[Authorize(Policy = "Dashboard.View")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly UserManager<ApplicationUser> _UserManager;
        public HomeController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _UnitOfWork = unitOfWork;
            _UserManager = userManager;
        }

        public async Task<IActionResult> Index(DashboardVM vm)
        {
            vm.NumOfBranches = await _UnitOfWork.Branches.CountAsync();
            vm.NewBranchesYearly = await _UnitOfWork.Branches.CountAsync(b => b.CreatedDate.Year == DateTime.Now.Year);

            vm.NumOfStockItems = await _UnitOfWork.Items.CountAsync();
            vm.NewItemsMonthly = await _UnitOfWork.Items.CountAsync(i => i.CreatedDate.Month == DateTime.Now.Month);

            DateTime lastMonthDate = DateTime.Now.AddMonths(-1);
            DateTime yesterdayDate = DateTime.Now.AddDays(-1);

            // This Month's Sales + Rate compared to last month
            vm.SalesOfMonth = (int)await _UnitOfWork.SalesInvoices.SumAsync(s => s.RoundedGrandTotal,
                s => s.Date.Month == DateTime.Now.Month && s.Date.Year == DateTime.Now.Year);

            int lastMonthSales = (int)await _UnitOfWork.SalesInvoices.SumAsync(s => s.RoundedGrandTotal,
                    s => s.Date.Month == lastMonthDate.Month && s.Date.Year == lastMonthDate.Year);

            if (vm.SalesOfMonth > 0)
            {
                if (lastMonthSales > 0)
                {
                    vm.MonthlySalesRate = ((decimal)(vm.SalesOfMonth - lastMonthSales) / lastMonthSales) * 100;
                }
                else
                {
                    // For No Invoices Last month
                    vm.MonthlySalesRate = -2;
                }
            }
            else
            {
                // For No Invoices This month yet
                vm.MonthlySalesRate = -1;
            }

            // Today's Sales + Compared to yesterday's Sales
            vm.SalesOfToday = (int)await _UnitOfWork.SalesInvoices.SumAsync(s => s.RoundedGrandTotal, s => s.Date == DateOnly.FromDateTime(DateTime.Now));
            int yesterdaySales;
            if (vm.SalesOfToday > 0)
            {
                yesterdaySales = (int)(await _UnitOfWork.SalesInvoices.SumAsync(s => s.RoundedGrandTotal, s => s.Date == DateOnly.FromDateTime(yesterdayDate)));
                if (yesterdaySales > 0)
                {
                    vm.DailySalesRate = ((decimal)(vm.SalesOfToday - yesterdaySales) / yesterdaySales) * 100;
                }
                else
                {
                    // For No Invoices yesterday
                    vm.DailySalesRate = -2;
                }
            }
            else
            {
                // For No Invoices today yet
                vm.DailySalesRate = -1;
            }

            // Average Invoice Value This Month + Rate Compared to Last Month
            int CurrNumOfInv = await _UnitOfWork.SalesInvoices.CountAsync(s => s.Date.Month == DateTime.Now.Month && s.Date.Year == DateTime.Now.Year);
            int LastNumOfInv;
            decimal AvgInvValLastMonth;
            if (CurrNumOfInv > 0)
            {
                // Main
                vm.AvgInvValMonth = (await _UnitOfWork.SalesInvoices.SumAsync(s => s.RoundedGrandTotal,
                    s => s.Date.Month == DateTime.Now.Month && s.Date.Year == DateTime.Now.Year)) / CurrNumOfInv;

                LastNumOfInv = await _UnitOfWork.SalesInvoices.CountAsync(s => s.Date.Month == lastMonthDate.Month && s.Date.Year == lastMonthDate.Year);
                if (LastNumOfInv > 0)
                {
                    // Rate
                    AvgInvValLastMonth = (decimal)lastMonthSales / LastNumOfInv;

                    vm.AvgInvValRate = ((vm.AvgInvValMonth - AvgInvValLastMonth) / AvgInvValLastMonth) * 100;
                }
                else
                {
                    // For No Invoices Last month
                    vm.AvgInvValRate = -2;
                }
            }
            else
            {
                // For No Invoices This month yet
                vm.AvgInvValRate = -1;
            }

            // Average Sales Per Day This Month
            decimal lastAvgSalesPerDay;
            if (vm.SalesOfMonth > 0)
            {
                // Main
                vm.AvgSalesPerDay = (decimal)vm.SalesOfMonth / DateTime.Now.Day;

                if (lastMonthSales > 0)
                {
                    // Rate
                    lastAvgSalesPerDay = (decimal)lastMonthSales / DateTime.DaysInMonth(lastMonthDate.Year, lastMonthDate.Month);
                    vm.AvgSalesPerDayRate = ((vm.AvgSalesPerDay - lastAvgSalesPerDay) / lastAvgSalesPerDay) * 100;
                }
                else
                {
                    vm.AvgSalesPerDayRate = -2;
                }
            }
            else
            {
                vm.AvgSalesPerDayRate = -1;
            }

            // Total Stock Value (By Buying Prices)
            vm.TotalStockVal = await _UnitOfWork.BranchItems.SumAsync(s => (decimal)s.BuyingPriceAvg * s.Quantity);

            // Charts
            DateTime currentDate = DateTime.Now.AddMonths(-11);
            decimal monthSales;
            decimal monthPurchases;
            for (int i = 0; i < 12; i++)
            {
                // 1. Calculate sales and purchases data
                monthSales = await _UnitOfWork.SalesInvoices.SumAsync(s => s.RoundedGrandTotal, s => s.Date.Month == currentDate.Month && s.Date.Year == currentDate.Year);
                monthPurchases = await _UnitOfWork.ReceiveOrders.SumAsync(s => s.RoundedGrandTotal, s => s.Date.Month == currentDate.Month && s.Date.Year == currentDate.Year);

                // 2. Populate the data lists
                vm.Last12MonthsSales.Add((int)monthSales);
                vm.Last12MonthsPurchases.Add((int)monthPurchases);

                // 3. Populate the required label list
                vm.Last12MonthsLabels.Add(currentDate.ToString("MMM yy"));

                // 4. Move to the next month
                currentDate = currentDate.AddMonths(1);
            }

            // Best Selling Branches
            vm.TopSellingBranches = await _UnitOfWork.Branches.GetTopSellingBranchesAsync(count: 6);

            // Best Selling Items
            vm.TopSellingItems = await _UnitOfWork.OperationItems.GetTopSellingItemsAsync(count: 5);

            // Restock Threshold - Low-Stock Alerts
            vm.LowStockBranchItems = await _UnitOfWork.BranchItems.GetLowStockBranchItems();

            // Slow Moving Items
            vm.SlowMovingBranchItems = await _UnitOfWork.BranchItems.GetSlowMovingBranchItems();

            return View(vm);
        }
    }
}
