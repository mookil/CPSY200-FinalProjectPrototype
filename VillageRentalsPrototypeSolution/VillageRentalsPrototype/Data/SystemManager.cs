using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Dapper;
using Microsoft.Maui.Controls;
using System.Diagnostics;

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
        /// Perhaps a categories table should be made as well, but it feels like rather of a waste.
        /// Equipment table has a foreign key of CategoryId, which means that it should exist in the categories table.
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
                Description         VARCHAR(60),
                DailyRentalCost     DECIMAL(10,2) NOT NULL,
                FOREIGN KEY (CategoryId) REFERENCES categories(CategoryId)
            )";

            connection.Execute(sql);

            // Create the "rentals" table
            // Rather small, but the data will be grabbed from other tables.
            // ex. rental equipment table will have RentalId's related to a row here, with multiple equipment.
            // [nvm pain in the butt]
            sql = @"CREATE TABLE IF NOT EXISTS rentals (
                RentalId            VARCHAR(10) PRIMARY KEY,
                Date                VARCHAR(20) NOT NULL,
                CustomerId          VARCHAR(10) NOT NULL,
                CustomerLastName    VARCHAR(30),
                EquipmentId         VARCHAR(10) NOT NULL,
                DateRented          VARCHAR(20),
                DateReturned        VARCHAR(20),
                TotalFinalCost      DECIMAL(10,2) NOT NULL,
                FOREIGN KEY (CustomerId) REFERENCES customers(CustomerId)
            )";

            connection.Execute(sql);

            // Create the "rental_equipment" table
            // Will be a list of the stored equipment.
            //sql = @"CREATE TABLE IF NOT EXISTS rental_equipment (
            //    RentalId            VARCHAR(10) NOT NULL,
            //    EquipmentId         VARCHAR(10) NOT NULL,
            //    Name                VARCHAR(30),
            //    DateRented          VARCHAR(20),
            //    DateReturned        VARCHAR(20),
            //    PRIMARY KEY (RentalId, EquipmentId),
            //    FOREIGN KEY (RentalId) REFERENCES rentals(RentalId),
            //    FOREIGN KEY (EquipmentId) REFERENCES equipment(EquipmentId)
            //)";

            //connection.Execute(sql);

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
            connection.Close();

            if (!anyDataExists)
            {
                PopulateDatabase();
                Console.WriteLine("Initial test data populated");
            }
            else
            {
                Console.WriteLine("Data exists in one or more tables - skipping population");
            }
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
                ('1001', 'John', 'Doe', '(555) 555-1212', 'jd@sample.net', 'Preferred customer'),
                ('1002', 'Jane', 'Smith', '(555) 555-3434', 'js@live.com', 'New customer'),
                ('1003', 'Michael', 'Lee', '(555) 555-5656', 'ml@sample.net', 'New customer')";

            connection.Execute(sql);

            // Populate the categories table
            sql = @"INSERT INTO categories (CategoryId, CategoryDesc)
            VALUES
                ('10', 'Power Tools'),
                ('20', 'Gardening'),
                ('30', 'Compressors'),
                ('40', 'Generators'),
                ('50', 'Air Tools')";
            connection.Execute(sql);

            // Populate the equipment table
            sql = @"INSERT INTO equipment (EquipmentId, CategoryId, Name, Description, DailyRentalCost)
            VALUES
                ('101', '10', 'Hammer drill', 'Powerful drill for concrete and masonry', 25.99),
                ('201', '20', 'Chainsaw', 'Gas-powered chainsaw for cutting wood', 49.99),
                ('202', '20', 'Lawn mower', 'Self-propelled lawn mower with mulching function', 19.99),
                ('301', '30', 'Small Compressor', '5 Gallon Compressor-Portable', 14.99),
                ('501', '50', 'Brad Nailer', 'Brad Nailer. Requires 3/4 to 1 1/2 Brad Nails', 10.99)
                ";
            connection.Execute(sql);

            // Populate the rentals table
            sql = @"INSERT INTO rentals (RentalId, Date, CustomerId, CustomerLastName, TotalFinalCost)
            VALUES
                ('1000', '2/15/2024', '1001', 'Doe', 149.97),
                ('1001', '2/16/2024', '1002', 'Johnson', 43.96)";
            connection.Execute(sql);

            // Populate the rental equipment table
            sql = @"INSERT INTO rental_equipment (RentalId, EquipmentId, Name, DateRented, DateReturned)
            VALUES
                ('1000', '201', 'Hammer drill', '2/20/2024', '2/23/2024'),
                ('1001', '501', 'Chainsaw', '2/21/2024', '2/25/2024')";
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

                        Customer customer = new Customer
                        {
                            customerID = reader.GetString(0),
                            firstName = reader.GetString(1),
                            lastName = reader.GetString(2),
                            contactPhone = reader.GetString(3),
                            Email = reader.GetString(4),
                            customerNotes = reader.GetString(5)
                        };
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

        /// <summary>
        /// Grabs a single customer from the database based on a given customerID parameter.
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public Customer GetCustomer(string customerID)
        {
            Customer customer = new Customer();
            try
            {
                connection.Open();
                string sql = "SELECT * from customers WHERE CustomerId = ?customerID";
                MySqlCommand command = new MySqlCommand( sql, connection);

                command.Parameters.AddWithValue("@customerID", customerID);

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

                        customer.customerID = customerId;
                        customer.firstName = firstName;
                        customer.lastName = lastName;
                        customer.contactPhone = contactPhone;
                        customer.Email = email;
                        customer.customerNotes = customerNotes;
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
            return customer;
        }

        /// <summary>
        /// Adds a customer into the "customers" database based off of a given Customer object
        /// </summary>
        /// <param name="customer"></param>
        public void AddCustomer(Customer customer)
        {
            try
            {
                connection.Open();
                string sql = "INSERT INTO customers (CustomerId, FirstName, LastName, ContactPhone, Email, CustomerNotes) " +
                    "VALUES (?CustomerId, ?FirstName, ?LastName, ?ContactPhone, ?Email, ?CustomerNotes)";
                MySqlCommand command = new MySqlCommand(sql, connection);

                command.Parameters.AddWithValue("@CustomerId", customer.customerID);
                command.Parameters.AddWithValue("@FirstName", customer.firstName);
                command.Parameters.AddWithValue("@LastName", customer.lastName);
                command.Parameters.AddWithValue("@ContactPhone", customer.contactPhone);
                command.Parameters.AddWithValue("@Email", customer.Email);
                command.Parameters.AddWithValue("@CustomerNotes", customer.customerNotes);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Edit the information of an existing customer based on a given Customer object
        /// </summary>
        /// <param name="customer"></param>
        public void UpdateCustomer(Customer customer)
        {
            try
            {
                connection.Open();
                string sql = "UPDATE customers " +
                    "SET FirstName = ?FirstName, " +
                    "LastName = ?LastName, " +
                    "ContactPhone = ?ContactPhone, " +
                    "Email = ?Email, " +
                    "CustomerNotes = ?CustomerNotes " +
                    "WHERE CustomerId = ?CustomerID"; // update the customer that shares the same CustomerId (the id should be unique)

                MySqlCommand command = new MySqlCommand(sql, connection);

                command.Parameters.AddWithValue("@FirstName", customer.firstName);
                command.Parameters.AddWithValue("@LastName", customer.lastName);
                command.Parameters.AddWithValue("@ContactPhone", customer.contactPhone);
                command.Parameters.AddWithValue("@Email", customer.Email);
                command.Parameters.AddWithValue("@CustomerNotes", customer.customerNotes);
                command.Parameters.AddWithValue("@CustomerId", customer.customerID);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Deletes a customer from database using a given customerID.
        /// </summary>
        /// <param name="customerID"></param>
        public void DeleteCustomer(string customerID)
        {
            try
            {
                connection.Open();
                string sql = "DELETE FROM customers where CustomerId = ?CustomerId";
                MySqlCommand command = new MySqlCommand( sql, connection);

                command.Parameters.AddWithValue("@CustomerId", customerID);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
        }

        // Equipment Methods


        public List<Equipment> GetEquipmentList() 
        {
            List<Equipment> EquipmentList = new List<Equipment>();
            try
            {
                connection.Open();
                // Query selects everything from equipment
                string sql = "SELECT * from equipment;";

                // Create MySqlCommand to execute sql query
                MySqlCommand command = new MySqlCommand(sql, connection);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //Debug.WriteLine($"{reader.GetString(0)}, {reader.GetString(1)}");
                        Equipment newEquipment = new Equipment
                        {
                            equipmentID = reader.GetString(0),
                            CategoryID = reader.GetString(1),
                            Name = reader.GetString(2),
                            Description = reader.GetString(3),
                            DailyRentalCost = reader.GetDecimal(4)
                        };
                        EquipmentList.Add(newEquipment);
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
            return EquipmentList;
        }

        public Equipment GetEquipment(string equipmentID) 
        {
            Equipment tempEquipment = new Equipment();
            try
            {
                connection.Open();
                // Query selects everything from equipment
                string sql = "SELECT * from equipment WHERE EquipmentId = ?equipmentID;";

                // Create MySqlCommand to execute sql query
                MySqlCommand command = new MySqlCommand(sql, connection);

                command.Parameters.AddWithValue("@equipmentID", equipmentID);


                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tempEquipment.equipmentID = reader.GetString(0);
                        tempEquipment.CategoryID = reader.GetString(1);
                        tempEquipment.Name = reader.GetString(2);
                        tempEquipment.Description = reader.GetString(3);
                        tempEquipment.DailyRentalCost = reader.GetDecimal(4);
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
            return tempEquipment;
        }

        public void AddEquipment(Equipment equipment) 
        {
            try
            {
                connection.Open();
                string sql = "INSERT INTO equipment (EquipmentId, CategoryId, Name, Description, DailyRentalCost) " +
                    "VALUES (?EquipmentId, ?CategoryId, ?Name, ?Description, ?DailyRentalCost)";
                MySqlCommand command = new MySqlCommand(sql, connection);

                command.Parameters.AddWithValue("@EquipmentId", equipment.equipmentID);
                command.Parameters.AddWithValue("@CategoryId", equipment.CategoryID);
                command.Parameters.AddWithValue("@Name", equipment.Name);
                command.Parameters.AddWithValue("@Description", equipment.Description);
                command.Parameters.AddWithValue("@DailyRentalCost", equipment.DailyRentalCost);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
        }

        public void UpdateEquipment(Equipment equipment) 
        {
            try
            {
                connection.Open();
                string sql = "UPDATE equipment " +
                    "SET EquipmentId = ?EquipmentId, " +
                    "CategoryId = ?CategoryId, " +
                    "Name = ?Name, " +
                    "Description = ?Description, " +
                    "DailyRentalCost = ?DailyRentalCost " +
                    "WHERE EquipmentId = ?EquipmentId";

                MySqlCommand command = new MySqlCommand(sql, connection);

                command.Parameters.AddWithValue("@EquipmentId", equipment.equipmentID);
                command.Parameters.AddWithValue("@CategoryId", equipment.CategoryID);
                command.Parameters.AddWithValue("@Name", equipment.Name);
                command.Parameters.AddWithValue("@Description", equipment.Description);
                command.Parameters.AddWithValue("@DailyRentalCost", equipment.DailyRentalCost);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            { 
            }
            finally
            {
                connection.Close();
            }
        }

        public void DeleteEquipment(string equipmentID) 
        {
            try
            {
                connection.Open();
                string sql = "DELETE FROM equipment where EquipmentId = ?EquipmentId";
                MySqlCommand command = new MySqlCommand(sql, connection);

                command.Parameters.AddWithValue("@EquipmentId", equipmentID);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
        }


        // Category Methods
        public List<Category> GetCategoryList() 
        {
            List<Category> categoryList = new List<Category>();
            try
            {
                connection.Open();
                string sql = "SELECT * from categories;";
                MySqlCommand command = new MySqlCommand( sql, connection);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Category tempCategory = new Category
                        {
                            CategoryID = reader.GetString(0),
                            CategoryDesc = reader.GetString(1)
                        };
                        categoryList.Add(tempCategory);
                    }
                }

            }
            catch (Exception ex) 
            { 
            }
            finally
            {
                connection.Close();
            }
            return categoryList;
        }

        //public Category GetCategory(string categoryID) { }
        public void UpdateCategory() { }
        public void DeleteCategory() { }
        public void AddCategory() { }
        


        // Rental Methods
        public List<Rental> GetRentals() 
        {
            List<Rental> rentalsList = new List<Rental>();
            List<Equipment> rentalEquipment = new List<Equipment>();
            try
            {
                connection.Open();
                string sql = "SELECT * from rentals;";
                MySqlCommand command = new MySqlCommand(sql, connection);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        Rental temp = new Rental
                        {

                        };
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                connection.Close();
            }
            return rentalsList;
        }
        public Rental GetRental(string rentalId) 
        {
            Rental rental = new Rental();
            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                connection.Close();
            }
            return rental;
        }
        public void AddRental() { }
        public void UpdateRental() { }
        public void DeleteRental() { }
    }
}
