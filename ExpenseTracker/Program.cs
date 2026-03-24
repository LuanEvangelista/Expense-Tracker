using ExpenseTracker.Services;
using System.Globalization;

var service = new ExpenseService();

if (args.Length == 0)
{
    Console.WriteLine("No command provided. Use: add, list, summary, delete, update, export.");
    return;
}

var command = args[0].ToLower();

string? GetArgValue(string flag)
{
    int index = Array.IndexOf(args, flag);
    return (index != -1 && index + 1 < args.Length) ? args[index + 1] : null;
}

bool HasFlag(string flag)
{
    return Array.IndexOf(args, flag) != -1;
}

bool TryParseAmount(string? input, out decimal amount)
{
    return decimal.TryParse(input, NumberStyles.Number, CultureInfo.InvariantCulture, out amount)
        || decimal.TryParse(input, out amount);
}

switch (command)
{
    case "add":
        var desc = GetArgValue("--description");
        var amountStr = GetArgValue("--amount");

        if (string.IsNullOrWhiteSpace(desc) || desc.StartsWith("--"))
        {
            Console.WriteLine("Please provide a valid description: --description \"text\"");
            break;
        }

        if (!TryParseAmount(amountStr, out decimal amount))
        {
            Console.WriteLine("Please provide a valid amount: --amount [number]");
            break;
        }

        service.AddExpense(desc, amount);
        break;

    case "list":
        service.ListExpenses();
        break;

    case "summary":
        if (!HasFlag("--month"))
        {
            service.ShowSummary();
            break;
        }

        var monthStr = GetArgValue("--month");
        if (int.TryParse(monthStr, out int month))
            service.ShowSummary(month);
        else
            Console.WriteLine("Please provide a valid month (1-12): --month [number]");
        break;

    case "delete":
        var idStr = GetArgValue("--id");
        if (int.TryParse(idStr, out int id))
            service.DeleteExpense(id);
        else
            Console.WriteLine("Please provide a valid ID for delete: --id [number]");
        break;

    case "update":
        var updateIdStr = GetArgValue("--id");
        var updateDesc = GetArgValue("--description");
        var updateAmountStr = GetArgValue("--amount");

        if (!int.TryParse(updateIdStr, out int uId))
        {
            Console.WriteLine("Please provide a valid ID for update: --id [number]");
            break;
        }

        decimal? uAmount = null;
        if (!string.IsNullOrWhiteSpace(updateAmountStr))
        {
            if (!TryParseAmount(updateAmountStr, out decimal val))
            {
                Console.WriteLine("Please provide a valid amount for update: --amount [number]");
                break;
            }

            uAmount = val;
        }

        var hasDesc = !string.IsNullOrWhiteSpace(updateDesc) && !updateDesc.StartsWith("--");
        var hasAmount = uAmount.HasValue;

        if (!hasDesc && !hasAmount)
        {
            Console.WriteLine("Provide at least one field to update: --description and/or --amount");
            break;
        }

        service.UpdateExpense(uId, updateDesc ?? string.Empty, uAmount);
        break;

    case "export":
        service.ExportToCsv();
        break;

    default:
        Console.WriteLine($"Command '{command}' not recognized.");
        break;
}
