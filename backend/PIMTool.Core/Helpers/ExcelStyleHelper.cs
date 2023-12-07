using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace PIMTool.Core.Helpers;

public static class ExcelStyleHelper
{
    public static ICellStyle SetBackgroundColor(this IWorkbook workbook, IndexedColors color)
    {
        var style = workbook.CreateCellStyle();
        style.FillForegroundColor = color.Index;
        style.FillPattern = FillPattern.SolidForeground;

        return style;
    }

    public static void SetRowBackgroundColor(this IRow row, IndexedColors color)
    {
        for (var i = 0; i < row.LastCellNum; i++)
        {
            var cell = row.GetCell(i) ?? row.CreateCell(i);
            cell.CellStyle = SetBackgroundColor(row.Sheet.Workbook, color);
        }
    }

    public static void AutoSizeColumns(this ISheet sheet)
    {
        for (var i = 0; i < sheet.GetRow(sheet.FirstRowNum).LastCellNum; i++)
        {
            sheet.AutoSizeColumn(i);
        }
    }

    public static IComment CreateComment(this ISheet sheet, string author, string commentContent)
    {
        var drawing = sheet.CreateDrawingPatriarch();
        var comment = drawing.CreateCellComment(new XSSFClientAnchor());
        comment.Author = author;
        comment.String = new XSSFRichTextString(commentContent);

        return comment;
    }
}