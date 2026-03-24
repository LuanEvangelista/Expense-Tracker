using ExpenseTracker.Models;
using System.Globalization;
using System.Text.Json;

namespace ExpenseTracker.Services
{
    public class ExpenseService
    {
        private readonly string _filePath;

        public ExpenseService()
        {
            _filePath = Path.Combine(Directory.GetCurrentDirectory(), "ExpenseTracker.json");
        }

        public List<ExpenseModel> GetExpense()
        {
            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, "[]");
                return new List<ExpenseModel>();
            }

            var json = File.ReadAllText(_filePath);

            if (string.IsNullOrWhiteSpace(json))
                return new List<ExpenseModel>();

            return JsonSerializer.Deserialize<List<ExpenseModel>>(json) ?? new List<ExpenseModel>();
        }

        public void SaveExpense(List<ExpenseModel> tasks)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            var json = JsonSerializer.Serialize(tasks, options);
            File.WriteAllText(_filePath, json);
        }

        public void AddExpense(string description, decimal amount)
        {
            if (string.IsNullOrWhiteSpace(description) || description.StartsWith("--"))
            {
                Console.WriteLine("Description cannot be empty.");
                return;
            }

            if (amount <= 0)
            {
                Console.WriteLine("Amount must be greater than zero.");
                return;
            }

            var expenses = GetExpense();
            int nextId = expenses.Count > 0 ? expenses.Max(e => e.Id) + 1 : 1;

            var newExpense = new ExpenseModel
            {
                Id = nextId,
                Description = description,
                Amount = amount,
                Date = DateTime.Now
            };

            expenses.Add(newExpense);
            SaveExpense(expenses);
            Console.WriteLine($"Expense added successfully (ID: {nextId})");
        }

        public void DeleteExpense(int id)
        {
            var expenses = GetExpense();
            var expense = expenses.FirstOrDefault(t => t.Id == id);

            if (expense == null)
            {
                Console.WriteLine($"Expense with ID {id} not found.");
                return;
            }

            expenses.Remove(expense);
            SaveExpense(expenses);

            Console.WriteLine($"Expense {id} deleted successfully.");
        }

        public void ListExpenses()
        {
            var expenses = GetExpense();
            if (expenses.Count == 0)
            {
                Console.WriteLine("No expenses found.");
                return;
            }

            Console.WriteLine("ID  Date        Description      Amount");
            foreach (var e in expenses)
            {
                Console.WriteLine($"{e.Id,-3} {e.Date:yyyy-MM-dd}  {e.Description,-15} ${e.Amount}");
            }
        }

        public void ShowSummary(int? month = null)
        {
            var expenses = GetExpense();

            if (month.HasValue)
            {
                if (month.Value < 1 || month.Value > 12)
                {
                    Console.WriteLine("Invalid month. Please provide a value between 1 and 12.");
                    return;
                }

                var filtered = expenses.Where(e => e.Date.Month == month.Value && e.Date.Year == DateTime.Now.Year).ToList();
                var total = filtered.Sum(e => e.Amount);

                string monthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(month.Value);
                Console.WriteLine($"Total expenses for {monthName}: ${total}");
            }
            else
            {
                var total = expenses.Sum(e => e.Amount);
                Console.WriteLine($"Total expenses: ${total}");
            }
        }

        public void UpdateExpense(int id, string description, decimal? amount)
        {
            var expenses = GetExpense();
            var expense = expenses.FirstOrDefault(e => e.Id == id);

            if (expense == null)
            {
                Console.WriteLine($"Expense with ID {id} not found.");
                return;
            }

            if (!string.IsNullOrWhiteSpace(description) && !description.StartsWith("--"))
                expense.Description = description;

            if (amount.HasValue)
            {
                if (amount.Value <= 0)
                {
                    Console.WriteLine("Amount must be greater than zero.");
                    return;
                }

                expense.Amount = amount.Value;
            }

            SaveExpense(expenses);
            Console.WriteLine($"Expense {id} updated successfully.");
        }

        public void ExportToCsv()
        {
            var expenses = GetExpense();
            var csvPath = Path.Combine(Directory.GetCurrentDirectory(), "expenses.csv");

            var lines = new List<string> { "Id,Date,Description,Amount" };

            foreach (var e in expenses)
            {
                lines.Add($"{e.Id},{e.Date:yyyy-MM-dd},{e.Description},{e.Amount}");
            }

            File.WriteAllLines(csvPath, lines);
            Console.WriteLine($"Expenses exported successfully to: {csvPath}");
        }
    }
}
