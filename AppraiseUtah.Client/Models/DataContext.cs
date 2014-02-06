using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppraiseUtah.Models
{
    public class DataContext
    {

        #region Fields

        private string _db = ConfigurationManager.ConnectionStrings["AppraisalDBContext"].ConnectionString;
        
        #endregion

        #region Properties

        #endregion

        #region Constructors

        public DataContext(string connection = null)
        {
            if (connection != null)
            {
                _db = connection;
            }
        }

        #endregion

        #region Methods

        #region Appraiser Methods

        /// <summary>
        /// Gets a list of all active appraisers
        /// </summary>
        /// <returns></returns>
        public virtual List<Appraiser> GetAppraisers()
        {
            var appraisers = new List<Appraiser>();

            // Get the data from the database
            DataTable dataTable = GetDataFromStoredProc("GetAppraisers");

            // Popluate the appraiser list object
            if (dataTable.Rows.Count > 0)
            {
                //appraisers = PopulateAppraisersFromDataTable(dataTable);
            }

            return appraisers;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Retrieves data from database stored proc based on stored proc name and parameters (optional)
        /// </summary>
        /// <param name="storedProcName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private DataTable GetDataFromStoredProc(string storedProcName, List<SqlParameter> parameters = null)
        {
            DataTable dataTable = new DataTable();

            using (var conn = new SqlConnection(_db))
            {
                using (var cmd = new SqlCommand(storedProcName, conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    // Add parameters if there are any to be added
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            cmd.Parameters.Add(parameter);
                        }
                    }

                    try
                    {
                        // Open the connection and execute the reader
                        conn.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            // Load the results into a datatable to be able to populate all complex data type objects on the appraisal object
                            dataTable.Load(reader);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Do something
                        throw ex;
                    }

                }
            }

            return dataTable;
        }

        #endregion

        #endregion


    }
}
