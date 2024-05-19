// From Web
using System.Text;
using HtmlAgilityPack;
using Spectre.Console;

var url = "https://ondernemingoprichten.be/nl/nacebelcode/";
var web = new HtmlWeb();
var doc = web.Load(url);

var nodes = doc.DocumentNode.SelectSingleNode("//*[@id=\"inner_content-3-306\"]/figure/table/tbody");
Console.WriteLine(nodes.ChildNodes.Count);

// output table
var table = new Table();
table.AddColumn("Code");
table.AddColumn(new TableColumn("Omschrijving"));

var csv = new StringBuilder();

foreach (var node in nodes.ChildNodes)
{
    if (node.ChildNodes.Count == 2)
    {
        var naceCode = node.ChildNodes[0].InnerText;
        var omschrijving = node.ChildNodes[1].InnerText;

        if (string.IsNullOrEmpty(naceCode) || string.IsNullOrEmpty(omschrijving) || !int.TryParse(naceCode, out _)) continue;

        if (omschrijving.Contains(","))
        {
        }
        omschrijving = $"\"{omschrijving}\"";

        var newLine = string.Format("{0},{1}", naceCode, omschrijving);

        csv.AppendLine(newLine);
        table.AddRow(node.ChildNodes[0].InnerText, node.ChildNodes[1].InnerText);
    }
}

File.WriteAllText("naceCodes.csv", csv.ToString());
AnsiConsole.Write(table);