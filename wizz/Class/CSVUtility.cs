using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace wizz.Class
{
    public class CSVUtility
    {
        public static MemoryStream GetCSV(DataTable data)
        {
            string[] fieldsToExpose = new string[data.Columns.Count];
            for (int i = 0; i < data.Columns.Count; i++)
            {
                fieldsToExpose[i] = data.Columns[i].ColumnName;
            }

            return GetCSV(fieldsToExpose, data);
        }

        public static MemoryStream GetCSV(string[] fieldsToExpose, DataTable data)
        {
            MemoryStream stream = new MemoryStream();
            using (var writer = new StreamWriter(stream))
            {
                for (int i = 0; i < fieldsToExpose.Length; i++)
                {
                    if (i != 0) { writer.Write(","); }
                    writer.Write("\"");
                    writer.Write(fieldsToExpose[i].Replace("\"", "\"\""));
                    writer.Write("\"");
                }
                writer.Write("\n");

                foreach (DataRow row in data.Rows)
                {
                    for (int i = 0; i < fieldsToExpose.Length; i++)
                    {
                        if (i != 0) { writer.Write(","); }
                        writer.Write("\"");
                        writer.Write(row[fieldsToExpose[i]].ToString()
                            .Replace("\"", "\"\""));
                        writer.Write("\"");
                    }

                    writer.Write("\n");
                }
            }

            return stream;
        }

        public static StringBuilder FetchCSV(DataTable data)
        {

            StringBuilder sb = new StringBuilder();
            bool printHeaders = true;
            if (printHeaders)
            {
                //write the headers.
                for (int colCount = 0;
                     colCount < data.Columns.Count; colCount++)
                {


                    if (colCount != data.Columns.Count - 1)
                    {
                        // sb.Append("\"");
                        sb.Append(data.Columns[colCount].ColumnName);
                        sb.Append(",");
                    }
                    else
                    {
                        sb.Append(data.Columns[colCount].ColumnName);
                        sb.AppendLine();
                    }
                }
            }


            for (int rowCount = 0;
                 rowCount < data.Rows.Count; rowCount++)
            {
                for (int colCount = 0;
                     colCount < data.Columns.Count; colCount++)
                {

                    if (colCount != data.Columns.Count - 1)
                    {
                        //sb.Append("\"");
                        sb.Append((data.Rows[rowCount][colCount]).ToString().Replace("\"", "\"\""));
                        sb.Append(",");
                    }
                    else { sb.Append((data.Rows[rowCount][colCount]).ToString().Replace("\"", "\"\"")); }
                }
                if (rowCount != data.Rows.Count - 1)
                {
                    sb.AppendLine();
                }
            }

            return sb;
        }
    }
}