using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace AppraiseUtah.Client.Models
{
    public class AppraisalContext : DbContext
    {

        #region Fields
        #endregion

        #region Properties

        public DbSet<Appraiser> Appraisers { get; set; }

        public DbSet<Appraisal> Appraisals { get; set; }
        
        public DbSet<Person> Persons { get; set; }

        public DbSet<State> States { get; set; }

        public DbSet<PropertyType> PropertyTypes { get; set; }

        public DbSet<AppraisalPurpose> AppraisalPurposes { get; set; }

        #endregion

        #region Constructors

        public AppraisalContext(string connection = null) : base(connection)
        { 
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
                appraisers = PopulateAppraisersFromDataTable(dataTable);
            }

            return appraisers;
        }

        /// <summary>
        /// Gets an individual Appraiser based on the appraiser's appraiserId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual Appraiser GetAppraiserById(int id)
        {
            var appraiser = new Appraiser();

            // Build the parameter for the database call
            var parameters = new List<SqlParameter> { new SqlParameter { ParameterName = "@id", Value = id, SqlDbType = System.Data.SqlDbType.Int, Direction = System.Data.ParameterDirection.Input } };

            // Get data from the database
            DataTable dataTable = GetDataFromStoredProc("GetAppraiserById", parameters);

            // Popluate the appraiser object
            if (dataTable.Rows.Count > 0)
            {
                appraiser = PopulateAppraiserFromDataTable(dataTable);
            }

            return appraiser;
        }

        #endregion

        #region Appraisal Methods

        /// <summary>
        /// Gets the appraisal from the database stored proc using the appraisal id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual Appraisal GetAppraisal(int id)
        {
            var appraisal = new Appraisal();

            // Build the parameter for the database call
            var parameters = new List<SqlParameter> {new SqlParameter { ParameterName = "@id", Value = id, SqlDbType = System.Data.SqlDbType.Int, Direction = System.Data.ParameterDirection.Input }};

            // Get data from the database
            DataTable dataTable = GetDataFromStoredProc("GetAppraisal", parameters);

            // Popluate the appraisal object
            if (dataTable.Rows.Count > 0)
            {
                appraisal = PopulateAppraisalFromDataTable(dataTable);
            }

            return appraisal;
        }

        #endregion

        #region State Methods

        public virtual List<State> GetStates()
        {
            var states = new List<State>();

            // Get the data from the database
            DataTable dataTable = GetDataFromStoredProc("GetStates");

            if (dataTable.Rows.Count > 0)
            {
                states = PopulateStatesFromDataTable(dataTable);
            }

            return states;
        }

        #endregion

        #region Property Type Methods

        public virtual List<PropertyType> GetPropertyTypes()
        {
            var propertyTypes = new List<PropertyType>();

            // Get the data from the database
            DataTable dataTable = GetDataFromStoredProc("GetPropertyTypes");

            if (dataTable.Rows.Count > 0)
            {
                propertyTypes = PopulatePropertyTypesFromDataTable(dataTable);
            }

            return propertyTypes;
        }

        #endregion

        #region Appraisal Purpose Methods

        public virtual List<AppraisalPurpose> GetAppraisalPurposes()
        {
            var appraisalPurposes = new List<AppraisalPurpose>();

            // Get the data from the database
            DataTable dataTable = GetDataFromStoredProc("GetAppraisalPurposes");

            if (dataTable.Rows.Count > 0)
            {
                appraisalPurposes = PopulateAppraisalPurposesFromDataTable(dataTable);
            }

            return appraisalPurposes;
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

            // Create and populate the command object 
            var cmd = this.Database.Connection.CreateCommand();
            cmd.CommandText = storedProcName;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
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
                this.Database.Connection.Open();
                var reader = cmd.ExecuteReader();

                // Load the results into a datatable to be able to populate all complex data type objects on the appraisal object
                dataTable.Load(reader);
            }
            catch (Exception ex)
            {
                // Do something
                String s = ex.Message;
                throw ex;
            }
            finally
            {
                this.Database.Connection.Close();
            }

            return dataTable;
        }

        /// <summary>
        /// Populates the list of appraisers
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private List<Appraiser> PopulateAppraisersFromDataTable(DataTable data) 
        {
            // Populate the appraiser object with the data from the Appraiser table
            var reader = data.CreateDataReader();
            var appraisers = ((IObjectContextAdapter)this).ObjectContext.Translate<Appraiser>(reader, "Appraisers", MergeOption.AppendOnly).ToList();
            
            // Hydrate the address object inside of the Appraiser object
            if (appraisers.Count > 0)
            {
                foreach (var appraiser in appraisers)
                {
                    appraiser.Address = PopulateAddressFromDataTable(data, "");
                }
            }

            return appraisers;
        }

        /// <summary>
        /// Populates the Appraiser object
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private Appraiser PopulateAppraiserFromDataTable(DataTable data)
        {
            // Populate the appraiser object with the data from the Appraiser table
            var reader = data.CreateDataReader();
            var appraiser = ((IObjectContextAdapter)this).ObjectContext.Translate<Appraiser>(reader, "Appraisers", MergeOption.AppendOnly).FirstOrDefault();

            // Hydrate the address object inside of the Appraiser object
            if (appraiser != null)
            {
                appraiser.Address = PopulateAddressFromDataTable(data, "");
            }

            return appraiser;
        }

        /// <summary>
        /// Populates/hydrates the Appraisal data with data retrieved from the database
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private Appraisal PopulateAppraisalFromDataTable(DataTable data)
        {
            // Populate the appraisal object with the data from the Appraisal table
            var reader = data.CreateDataReader();
            var appraisal = ((IObjectContextAdapter)this).ObjectContext.Translate<Appraisal>(reader, "Appraisals", MergeOption.AppendOnly).FirstOrDefault();

            if (appraisal != null)
            {
                appraisal.ClientPerson = PopulatePersonFromDataTable(data, "ClientPerson");         // Populate the Appraisal.ClientPerson object
                appraisal.ClientAddress = PopulateAddressFromDataTable(data, "ClientAddress");      // Populate the Appraisal.ClientAddress object
                appraisal.Client2Person = PopulatePersonFromDataTable(data, "Client2Person");         // Populate the Appraisal.ClientPerson object
                appraisal.Client2Address = PopulateAddressFromDataTable(data, "Client2Address");      // Populate the Appraisal.ClientAddress object
                appraisal.OccupantPerson = PopulatePersonFromDataTable(data, "OccupantPerson");     // Populate the Appraisal.OccupantPerson object
                appraisal.PropertyAddress = PopulateAddressFromDataTable(data, "PropertyAddress");  // Populate the Appraisal.PropertyAddress object
            }

            return appraisal;
        }

        /// <summary>
        /// Populates/hydrates a person object based on data from the database and a columnPrefix
        /// </summary>
        /// <param name="data"></param>
        /// <param name="columnPrefix"></param>
        /// <returns></returns>
        private static Person PopulatePersonFromDataTable(DataTable data, string columnPrefix)
        {
            Person person = null;

            if (data.Rows.Count > 0) 
            {
                if (!String.IsNullOrEmpty(data.Rows[0][columnPrefix + "_PersonId"].ToString()))
                {
                    person = new Person();
                    person.PersonId = (int)data.Rows[0][columnPrefix + "_PersonId"];
                    if (data.Columns.Contains(columnPrefix + "_PersonType")) person.PersonType = data.Rows[0][columnPrefix + "_PersonType"].ToString();
                    person.FirstName = data.Rows[0][columnPrefix + "_FirstName"].ToString();
                    person.LastName = data.Rows[0][columnPrefix + "_LastName"].ToString();
                    if (data.Columns.Contains(columnPrefix + "_CompanyName")) person.CompanyName = data.Rows[0][columnPrefix + "_CompanyName"].ToString();
                    person.Email = data.Rows[0][columnPrefix + "_Email"].ToString();
                    person.Phone = data.Rows[0][columnPrefix + "_Phone"].ToString();
                }
            }

            return person;
        }
        
        /// <summary>
        /// Populates/hydrates an address object based on data from the database and a columnPrefix
        /// </summary>
        /// <param name="data"></param>
        /// <param name="columnPrefix"></param>
        /// <returns></returns>
        private static Address PopulateAddressFromDataTable(DataTable data, string columnPrefix)
        {
            Address address = null;
            columnPrefix = columnPrefix != "" ? columnPrefix + "_" : columnPrefix;

            if (data.Rows.Count > 0)
            {
                if (!String.IsNullOrEmpty(data.Rows[0][columnPrefix + "AddressId"].ToString()))
                {

                    address = new Address();
                    address.AddressId = (int)data.Rows[0][columnPrefix + "AddressId"];

                    address.AddressType = data.Rows[0].Table.Columns.Contains(columnPrefix + "AddressType") ?
                        data.Rows[0][columnPrefix + "AddressType"].ToString() :
                        null;

                    address.Address1 = data.Rows[0][columnPrefix + "Address1"].ToString();
                    address.Address2 = data.Rows[0][columnPrefix + "Address2"].ToString();
                    address.City = data.Rows[0][columnPrefix + "City"].ToString();
                    address.StateCode = data.Rows[0][columnPrefix + "StateCode"].ToString();
                    address.PostalCode = data.Rows[0][columnPrefix + "PostalCode"].ToString();
                }
            }

            return address;
        }

        /// <summary>
        /// Populate a list of states
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        private List<State> PopulateStatesFromDataTable(DataTable data)
        {
            // Populate the state object with the data from the StateList table
            var reader = data.CreateDataReader();
            var states = ((IObjectContextAdapter)this).ObjectContext.Translate<State>(reader, "States", MergeOption.AppendOnly).ToList();

            return states;
        }

        /// <summary>
        /// Populate a list of states
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        private List<PropertyType> PopulatePropertyTypesFromDataTable(DataTable data)
        {
            // Populate the propertyType object with the data from the PropertyType table
            var reader = data.CreateDataReader();
            var propertyTypes = ((IObjectContextAdapter)this).ObjectContext.Translate<PropertyType>(reader, "PropertyTypes", MergeOption.AppendOnly).ToList();

            return propertyTypes;
        }

        /// <summary>
        /// Populate a list of Appraisal Purposes
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        private List<AppraisalPurpose> PopulateAppraisalPurposesFromDataTable(DataTable data)
        {
            // Populate the appraisalPurposes object with the data from the AppraisalPurpose table
            var reader = data.CreateDataReader();
            var appraisalPurposes = ((IObjectContextAdapter)this).ObjectContext.Translate<AppraisalPurpose>(reader, "AppraisalPurposes", MergeOption.AppendOnly).ToList();

            return appraisalPurposes;
        }

        #endregion

        #endregion

    }
}
