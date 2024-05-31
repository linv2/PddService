using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;

namespace PddService.Common
{
    public class ExportExcel<TModel>
    {
        IWorkbook workbook;
        ISheet sheet;
        IDictionary<string, Func<TModel, object>> dictHandler = new Dictionary<string, Func<TModel, object>>();
        public Func<TModel, object> this[string header]
        {
            get
            {
                dictHandler.TryGetValue(header, out var value);
                return value;
            }
            set
            {
                if (dictHandler.TryGetValue(header, out var outValue))
                {
                    dictHandler[header] = outValue;
                }
                else
                {
                    dictHandler.Add(header, value);
                }
            }
        }
        int rowNumber = 1;
        bool isSetHeader = false;
        public ExportExcel(string sheetName = "sheet1")
        {
            workbook = new XSSFWorkbook();
            sheet = workbook.CreateSheet(sheetName);
        }
        private void SetHeader()
        {
            var row = sheet.CreateRow(0);
            var columnIndex = 0;
            foreach (var name in dictHandler.Keys)
            {
                row.CreateCell(columnIndex).SetCellValue(name);
                columnIndex++;
            }
        }


        public void SetDataSet(IEnumerable<TModel> list)
        {
            if (!isSetHeader)
            {
                SetHeader();
                isSetHeader = true;
            }
            var maxCell = dictHandler.Count;
            foreach (var model in list)
            {
                var row = sheet.CreateRow(rowNumber);
                var cellIndex = 0;
                foreach (var header in dictHandler.Keys)
                {
                    if (dictHandler.TryGetValue(header, out var formatExpression))
                    {
                        var value = formatExpression(model);
                        row.CreateCell(cellIndex).SetCellValue(Convert.ToString(value));
                    }
                    else
                    {

                        row.CreateCell(cellIndex).SetCellValue("-");
                    }
                    cellIndex++;
                }
                rowNumber++;
            }


        }

        public byte[] ToBytes()
        {
            byte[] buffer;
            using (var ms = new MemoryStream())
            {
                workbook.Write(ms);
                buffer = ms.GetBuffer();
            }
            return buffer;
        }

        public static ExportExcel<TModel> Create(string sheetName = "sheet1")
        {
            return new ExportExcel<TModel>(sheetName);
        }


    }
}
