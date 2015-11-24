using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FastMember;
namespace MVCAggregate.Models
{
    public class BusinessLogic
    {
        public DataTable FetchData(string file)
        {
            

            //string filePath = @"C:\Debuzz\Indix Requirement\Project\test.csv";

            DataTable dtObj = FetchCSVData(file);
            DataTable inverseData = new DataTable();
            inverseData = GetInversedDataTable(dtObj, "Stores", "Categories", "StoreDiff", "-", false);
            //CalculateField(inverseData,"My Store","remove");
            //inverseData.Columns.Remove("My Store");
            return inverseData;
        }

        public DataTable FetchCSVData(string file)
        {
            List<UploadData> li = new List<UploadData>();
            string filePath = file;
            ReadCSV rd = new ReadCSV(filePath);
            try
            {
                li = rd.ReadData();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //var data=li.Count;
            var results = from d in li
                          orderby d.Store ascending
                          group d.Price
                          by new
                          {
                              d.Store,
                              d.Top_Level
                          }

                              into g
                              select new
                              {
                                  Stores = g.Key.Store,
                                  Categories = g.Key.Top_Level,
                                  StoreAvg = g.ToArray().Average()
                              }
                              ;
            //Get MyStore from the result set
            var mystore = from r in results
                          where new[] { "My Store" }.Contains(r.Stores)
                          select r;
            
            //Join and get difference
            var avgDiff = from res in results.Except(mystore)
                          join my in mystore
                          on res.Categories equals my.Categories
                          select new
                          {
                              Categories=res.Categories,
                              Stores = res.Stores,
                              StoreAvg=res.StoreAvg,
                              StoreDiff=res.StoreAvg-my.StoreAvg
                              //positive indicates my store cost is smaller
                              //negative indicates my store cost is greater
                          };
            //avgDiff.Where(i => !new[] { "My Store" }.Contains(i.Stores));
            //var reject=from m in avgDiff where new[]{"My Store"}.Contains(m.Stores) select m;
            
            DataTable dtObj = new DataTable();
            using (var reader=ObjectReader.Create(avgDiff))
            {
                dtObj.Load(reader);
            }
            
            return dtObj;

        }
        public DataTable GetInversedDataTable(DataTable table, string columnX, string columnY, string columnZ, string nullValue, bool sumValues)
        {
            //Create a DataTable to Return
            DataTable returnTable = new DataTable();

            if (columnX == "")
                columnX = table.Columns[0].ColumnName;

            //Add a Column at the beginning of the table
            returnTable.Columns.Add(columnY);


            //Read all DISTINCT values from columnX Column in the provided DataTale
            List<string> columnXValues = new List<string>();

            foreach (DataRow dr in table.Rows)
            {

                string columnXTemp = dr[columnX].ToString();
                if (!columnXValues.Contains(columnXTemp))
                {
                    //Read each row value, if it's different from others provided, add to 
                    //the list of values and creates a new Column with its value.
                    columnXValues.Add(columnXTemp);
                    returnTable.Columns.Add(columnXTemp);
                }
            }

            //Verify if Y and Z Axis columns re provided
            if (columnY != "" && columnZ != "")
            {
                //Read DISTINCT Values for Y Axis Column
                List<string> columnYValues = new List<string>();

                foreach (DataRow dr in table.Rows)
                {
                    if (!columnYValues.Contains(dr[columnY].ToString()))
                        columnYValues.Add(dr[columnY].ToString());
                }

                //Loop all Column Y Distinct Value
                foreach (string columnYValue in columnYValues)
                {
                    //Creates a new Row
                    DataRow drReturn = returnTable.NewRow();
                    drReturn[0] = columnYValue;
                    //foreach column Y value, The rows are selected distincted
                    DataRow[] rows = table.Select(columnY + "='" + columnYValue + "'");

                    //Read each row to fill the DataTable
                    foreach (DataRow dr in rows)
                    {
                        string rowColumnTitle = dr[columnX].ToString();

                        //Read each column to fill the DataTable
                        foreach (DataColumn dc in returnTable.Columns)
                        {
                            if (dc.ColumnName == rowColumnTitle)
                            {
                                //If Sum of Values is True it try to perform a Sum
                                //If sum is not possible due to value types, the value 
                                // displayed is the last one read
                                if (sumValues)
                                {
                                    try
                                    {
                                        drReturn[rowColumnTitle] =
                                             Convert.ToDecimal(drReturn[rowColumnTitle]) +
                                             Convert.ToDecimal(dr[columnZ]);
                                    }
                                    catch
                                    {
                                        drReturn[rowColumnTitle] = dr[columnZ];
                                    }
                                }
                                else
                                {
                                    drReturn[rowColumnTitle] = dr[columnZ];
                                }
                            }
                        }
                    }
                    returnTable.Rows.Add(drReturn);
                }
            }
            else
            {
                throw new Exception("The columns to perform inversion are not provided");
            }

            //if a nullValue is provided, fill the datable with it
            if (nullValue != "")
            {
                foreach (DataRow dr in returnTable.Rows)
                {
                    foreach (DataColumn dc in returnTable.Columns)
                    {
                        if (dr[dc.ColumnName].ToString() == "")
                            dr[dc.ColumnName] = nullValue;
                    }
                }
            }

            return returnTable;
        }
        
        void CalculateField(DataTable dt,string colName,string type)
        {
           
            for (int i = 1; i < dt.Columns.Count; i++)
            {
                if (dt.Columns[i].ColumnName==colName)
                {
                    continue;
                }
                foreach (DataRow dr in dt.Rows)
                {
                    try
                    {
                        //dr[i] = Convert.ToDouble(dr[colName])- Convert.ToDouble(dr[i]);//positive indicates that the cost of colname is greater than competitor
                        dr[i] = Convert.ToDouble(dr[i]) - Convert.ToDouble(dr[colName]);
                    }
                    catch (Exception ex)
                    {
                        dr[i] = 0.0;
                    }
                }
            }
            dt.Columns.Remove(colName);
        }
    }
}