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
    /// This class handles database connections, and various other system functions.
    /// All the other managers should use this in order to pull in data from the database.
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
        /// Initialize the database and create all relevant tables.
        /// The tables that should be created are:
        ///     1. "customers" table
        ///     2. "equipment" table
        ///     3. "rentals" table
        ///     4. "categories" table
        ///     5. "rental equipment table", this table represents the list of equipment that is in a rental. Should be linked by rental id.
        /// Perhaps a categories table should be made as well, but it feels like rather of a waste.
        /// </summary>
        public void InitializeDatabase()
        {
            connection.Open();
            
            // Create the "customers" table
            var sql = @"CREATE TABLE IF NOT EXISTS customers (
                CustomerId          VARCHAR(10) PRIMARY KEY,
                FirstName           VARCHAR(30) NOT NULL,
                LastName            VARCHAR(30) NOT NULL,
                ContactPhone        VARCHAR(40),
                Email               VARCHAR(50),
                CustomerNotes       VARCHAR(255)
            )";

            connection.Execute(sql);

            // Create the "categories" table
            // Connected to an "equipment" row in the "equipment" table through CategoryId
            sql = @"CREATE TABLE IF NOT EXISTS categories (
                CategoryId          VARCHAR(10) PRIMARY KEY,
                CategoryDesc        VARCHAR(20) NOT NULL
            )";

            connection.Execute(sql);

            // Create the "equipment" table
            sql = @"CREATE TABLE IF NOT EXISTS equipment (
                EquipmentId         VARCHAR(10) PRIMARY KEY,
                CategoryId          VARCHAR(10) NOT NULL,
                Name                VARCHAR(30) NOT NULL,
                Description         VARCHAR(40),
                DailyRentalCost     DECIMAL(10,2) NOT NULL,
                FOREIGN KEY (CategoryId) REFERENCES categories(CategoryId)
            )";

            connection.Execute(sql);

            // Create the "rentals" table
            // Rather small, but the data will be grabbed from other tables.
            // ex. rental equipment table will have RentalId's related to a row here, with multiple equipment.
            sql = @"CREATE TABLE IF NOT EXISTS rentals (
                RentalId            VARCHAR(10) PRIMARY KEY,
                DateRented          DATE NOT NULL,
                CustomerId          VARCHAR(30) NOT NULL,
                CustomerLastName    VARCHAR(30),
                FOREIGN KEY (CustomerId) REFERENCES customers(CustomerId)
            )";

            connection.Execute(sql);

            // Create the "rental_equipment" table
            // Will be a list of the stored equipment.
            sql = @"CREATE TABLE IF NOT EXISTS rental_equipment (
                RentalId            VARCHAR(10) NOT NULL,
                EquipmentId         VARCHAR(10) NOT NULL,
                Name                VARCHAR(30),
                PRIMARY KEY (RentalId, EquipmentId),
                FOREIGN KEY (RentalId) REFERENCES rentals(RentalId),
                FOREIGN KEY (EquipmentId) REFERENCES equipment(EquipmentId)
            )";

            connection.Execute(sql);

            connection.Close();
        }

        /// <summary>
        /// This method checks if any data is in any of the tables in the database. If so, it won't populate with data.
        /// </summary>
        /// <returns></returns>
        public void PopulateIfEmpty()
        {
            connection.Open();

            var tablesToCheck = new[] { "customers", "categories", "equipment", "rentals", "rental_equipment" };
            bool anyDataExists = false;

            foreach (var table in tablesToCheck)
            {
                var count = connection.ExecuteScalar<int>(
                    $"SELECT COUNT(*) FROM {table}");

                if (count > 0)
                {
                    anyDataExists = true;
                    break;
                }
            }

            if (!anyDataExists)
            {
                PopulateDatabase();
                Console.WriteLine("Initial test data populated");
            }
            else
            {
                Console.WriteLine("Data exists in one or more tables - skipping population");
            }
            connection.Close();
        }

        /// <summary>
        /// This method is used to populate the database with starter data if it is empty.
        /// </summary>
        public void PopulateDatabase()
        {
            connection.Open();

            // Populate the customers table
            string sql = @"INSERT INTO customers (CustomerId, FirstName, LastName, ContactPhone, Email, CustomerNotes)
            VALUES
                ('C0001', 'John', 'Smith', '555-0101', 'john.smith@email.com', 'Preferred customer'),
                ('C0002', 'Sarah', 'Johnson', '555-0102', 'sarah.j@email.com', 'New customer')";

            connection.Execute(sql);

            // Populate the categories table
            sql = @"INSERT INTO categories (CategoryId, CategoryDesc)
            VALUES
                (10, 'Power Tools'),
                (20, 'Gardening')";
            connection.Execute(sql);

            // Populate the equipment table
            sql = @"INSERT INTO equipment (EquipmentId, CategoryId, Name, Description, DailyRentalCost)
            VALUES
                ('E0001', 10, 'Drill', 'Heavy-duty cordless drill', 15.99),
                ('E0002', 20, 'Lawnmower', 'Self-propelled lawn mower', 29.99)";
            connection.Execute(sql);

            // Populate the rentals table
            sql = @"INSERT INTO rentals (RentalId, DateRented, CustomerId, CustomerLastName)
            VALUES
                ('R0001', '2023-05-01', 'C0001', 'Smith'),
                ('R0002', '2023-05-02', 'C0002', 'Johnson')";
            connection.Execute(sql);

            // Populate the rental equipment table
            sql = @"INSERT INTO rental_equipment (RentalId, EquipmentId, Name)
            VALUES
                ('R0001', 'E0001', 'Drill'),
                ('R0002', 'E0002', 'Lawnmower')";
            connection.Execute(sql);


            connection.Close();
        }
        // Customer Methods
        
        /// <summary>
        /// Gets all customers from the database and returns it as a list.
        /// </summary>
        /// <returns></returns>
        public List<Customer> GetCustomers()
        {
            List<Customer> customers = new List<Customer>();
            try
            {
                connection.Open();
                // Query selects everything from customers
                string sql = "SELECT * from customers;";

                // Create MySqlCommand to execute sql query
                MySqlCommand command = new MySqlCommand(sql, connection);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string customerId = reader.GetString(0);
                        string firstName = reader.GetString(1);
                        string lastName = reader.GetString(2);
                        string contactPhone = reader.GetString(3);
                        string email = reader.GetString(4);
                        string customerNotes = reader.GetString(5);

                        Customer customer = new Customer(
                            customerId,
                            firstName,
                            lastName,
                            contactPhone,
                            email,
                            customerNotes);
                        customers.Add(customer);
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error has occured: {ex.Message}");
            }
            finally 
            { 
                connection.Close(); 
            }
            return customers;
        }


        // Equipment Methods


        // Category Methods


        // Rental Methods

    }
}
