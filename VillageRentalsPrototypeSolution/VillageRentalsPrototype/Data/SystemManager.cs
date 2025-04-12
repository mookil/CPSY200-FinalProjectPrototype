using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Dapper;

namespace VillageRentalsPrototype.Data
{
    /// <summary>
    /// [System Manager]
    /// This class handles database connections.
    /// 
    /// </summary>
    public class SystemManager
    {
        protected MySqlConnection connection;

        // Constructor
        public SystemManager() 
        {
            string dbHost = "localhost";
            string dbUser = "root";
            string dbPassword = "password";

            var builder = new MySqlConnectionStringBuilder // This is the builder to create SQL connection strings.
            {
                Server = dbHost,
                UserID = dbUser,
                Password = dbPassword,
                Database = "village_rentals", // use maria db to create a database called "village_rentals"
                                               // (A database by the name of "village_rentals" must exist on the machine beforehand.)
            };

            connection = new MySqlConnection(builder.ConnectionString); // Create a mySql Connection string with the builder.
             
        }

        /// <summary>
        /// Initialize the database and create all relevant tables
        /// </summary>
        public void InitializeDatabase()
        {
            connection.Open();

            var sql = @"CREATE TABLE IF NOT EXISTS customers (
                CustomerId VARCHAR(10) PRIMARY KEY,
                FirstName VARCHAR(30) NOT NULL,
                LastName VARCHAR(30) NOT NULL,
                ContactPhone VARCHAR(40),
                Email VARCHAR(50),
                CustomerNotes VARCHAR(255)
            )";

            connection.Execute(sql);

            sql = @"CREATE TABLE IF NOT EXISTS equipment (
                test VARCHAR(10) PRIMARY KEY,
                FiteststName VARCHAR(30) NOT NULL,
                LastName VARCHAR(30) NOT NULL,
                ContactPhone VARCHAR(40),
                Email VARCHAR(50),
                CustomerNotes VARCHAR(255)
            )";

            connection.Execute(sql);

            connection.Close();
        }


    }
}
