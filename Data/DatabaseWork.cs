using FirstChurchOfGodTreasuryMVC.Models;
using System.Data;
using System.Data.OleDb;

#pragma warning disable CA1416 // Validate platform compatibility
namespace FirstChurchOfGodTreasuryMVC.Data
{
    public class DatabaseWork
    {
        public bool DoesChurchMemberExist(ChurchMember newMember)
        {
            bool doesExist = false;
            // create existing church member to compare if it exists..
            ChurchMember existingMember = new ChurchMember();
            string resPath = AppDomain.CurrentDomain.BaseDirectory; // base directory path of the debug folder
            string dbPath = resPath.Replace("bin\\Debug\\net6.0\\", "Data"); // filePath to the FirstChurchOfGodDatabase.mdb database.
            // string dbPath = resPath.Replace("bin\\Release\\net6.0\\", "Data");

            using (OleDbConnection connection = new OleDbConnection($"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath}\\FirstChurchOfGodDatabase.mdb"))
            {
                connection.Open();
                string sql = "Select * From ChurchMembers";
                OleDbCommand command = new OleDbCommand(sql, connection);
                command.CommandType = System.Data.CommandType.Text;

                using (OleDbDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        existingMember.FirstName = Convert.ToString(dataReader["FirstName"]);
                        existingMember.LastName = Convert.ToString(dataReader["LastName"]);
                        existingMember.JointAccount = Convert.ToBoolean(dataReader["IsJointAccount"]);
                        existingMember.JointFirstName = Convert.ToString(dataReader["JointFirstName"]);

                        int count = 0; // if comparison matched 4 times then it exists

                        if (newMember.FirstName == existingMember.FirstName) { count++; }
                        if (newMember.LastName == existingMember.LastName) { count++; }
                        if (newMember.JointAccount == existingMember.JointAccount) { count++; }
                        if (newMember.JointFirstName == existingMember.JointFirstName) { count++; }

                        if (count == 4) // if all matched, it does exist, set flag to true
                        {
                            doesExist = true;
                            break;
                        }
                    }
                    connection.Close();
                }
            }

            return doesExist;
        }

        public List<ChurchMember> GetAllChurchMembers()
        {
            string resPath = AppDomain.CurrentDomain.BaseDirectory; // base directory path of the debug folder
            string dbPath = resPath.Replace("bin\\Debug\\net6.0\\", "Data"); // filePath to the FirstChurchOfGodDatabase.mdb database.
            // string dbPath = resPath.Replace("bin\\Release\\net6.0\\", "Data");

            List<ChurchMember> churchMemberList = new List<ChurchMember>();

            using (OleDbConnection connection = new OleDbConnection($"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath}\\FirstChurchOfGodDatabase.mdb"))
            {
                connection.Open();
                string sql = "Select * From ChurchMembers ORDER BY FirstName";
                OleDbCommand command = new OleDbCommand(sql, connection);

                using (OleDbDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        ChurchMember churchMember = new ChurchMember();
                        churchMember.Id = Convert.ToInt32(dataReader["Id"]);
                        churchMember.FirstName = Convert.ToString(dataReader["FirstName"]);
                        churchMember.LastName = Convert.ToString(dataReader["LastName"]);
                        churchMember.JointAccount = Convert.ToBoolean(dataReader["IsJointAccount"]);
                        churchMember.JointFirstName = Convert.ToString(dataReader["JointFirstName"]);

                        churchMemberList.Add(churchMember);
                    }
                }
                connection.Close();
            }
            return churchMemberList;
        }

        public void CreateNewChurchMember(ChurchMember churchMember)
        {
            string resPath = AppDomain.CurrentDomain.BaseDirectory; // base directory path of the debug folder
            string dbPath = resPath.Replace("bin\\Debug\\net6.0\\", "Data"); // filePath to the FirstChurchOfGodDatabase.mdb database.
            // string dbPath = resPath.Replace("bin\\Release\\net6.0\\", "Data");
            using (OleDbConnection connection = new OleDbConnection($"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath}\\FirstChurchOfGodDatabase.mdb"))
            {
                string sql = "Insert Into ChurchMembers (FirstName, LastName, IsJointAccount, JointFirstName) Values (@FirstName, @LastName, @JointAccount, @JointFirstName)";
                using (OleDbCommand command = new OleDbCommand(sql, connection))
                {
                    command.CommandType = System.Data.CommandType.Text;

                    //add parameters
                    OleDbParameter parameter = new OleDbParameter
                    {
                        ParameterName = "@FirstName",
                        Value = churchMember.FirstName,
                        OleDbType = OleDbType.WChar,
                        Size = 255
                    };
                    command.Parameters.Add(parameter);

                    parameter = new OleDbParameter
                    {
                        ParameterName = "@LastName",
                        Value = churchMember.LastName,
                        OleDbType = OleDbType.WChar,
                        Size = 255
                    };
                    command.Parameters.Add(parameter);

                    parameter = new OleDbParameter
                    {
                        ParameterName = "@JointAccount",
                        Value = churchMember.JointAccount,
                        OleDbType = OleDbType.Boolean,
                    };
                    command.Parameters.Add(parameter);

                    parameter = new OleDbParameter
                    {
                        ParameterName = "@JointFirstName",
                        Value = churchMember.JointFirstName,
                        OleDbType = OleDbType.WChar,
                        Size = 255
                    };
                    command.Parameters.Add(parameter);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public ChurchMember GetChurchMember(int id)
        {
            ChurchMember churchMember = new ChurchMember();

            string resPath = AppDomain.CurrentDomain.BaseDirectory; // base directory path of the debug folder
            string dbPath = resPath.Replace("bin\\Debug\\net6.0\\", "Data"); // filePath to the FirstChurchOfGodDatabase.mdb database.
            // string dbPath = resPath.Replace("bin\\Release\\net6.0\\", "Data");
            using (OleDbConnection connection = new OleDbConnection($"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath}\\FirstChurchOfGodDatabase.mdb"))
            {
                string sql = $"Select * From ChurchMembers Where ID={id}";
                OleDbCommand command = new OleDbCommand(sql, connection);
                command.CommandType = System.Data.CommandType.Text;
                connection.Open();

                using (OleDbDataReader dataReader = command.ExecuteReader())
                {

                    while (dataReader.Read())
                    {
                        churchMember.Id = Convert.ToInt32(dataReader["ID"]);
                        churchMember.FirstName = Convert.ToString(dataReader["FirstName"]);
                        churchMember.LastName = Convert.ToString(dataReader["LastName"]);
                        churchMember.JointAccount = Convert.ToBoolean(dataReader["IsJointAccount"]);
                        churchMember.JointFirstName = Convert.ToString(dataReader["JointFirstname"]);
                    }
                }
                connection.Close();
            }

            return churchMember;
        }

        public void UpdateChurchMemberByIdWithParameters(ChurchMember member, int id)
        {
            string resPath = AppDomain.CurrentDomain.BaseDirectory; // base directory path of the debug folder
            string dbPath = resPath.Replace("bin\\Debug\\net6.0\\", "Data"); // filePath to the FirstChurchOfGodDatabase.mdb database.
            // string dbPath = resPath.Replace("bin\\Release\\net6.0\\", "Data");
            using (OleDbConnection connection = new OleDbConnection($"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath}\\FirstChurchOfGodDatabase.mdb"))
            {
                string sql = $"Update ChurchMembers SET FirstName='{member.FirstName}', LastName='{member.LastName}', IsJointAccount={member.JointAccount}, JointFirstName='{member.JointFirstName}' Where ID={id}";
               
                using (OleDbCommand command = new OleDbCommand(sql,connection))
                {
                    command.CommandType = System.Data.CommandType.Text;
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void DeleteChurchMemberById(int id)
        {
            string resPath = AppDomain.CurrentDomain.BaseDirectory; // base directory path of the debug folder
            string dbPath = resPath.Replace("bin\\Debug\\net6.0\\", "Data"); // filePath to the FirstChurchOfGodDatabase.mdb database.
            // string dbPath = resPath.Replace("bin\\Release\\net6.0\\", "Data");
            using (OleDbConnection connection = new OleDbConnection($"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath}\\FirstChurchOfGodDatabase.mdb"))
            {
                string sql = $"Delete From ChurchMembers Where ID={id}";

                using (OleDbCommand command = new OleDbCommand(sql, connection))
                {
                    command.CommandType = System.Data.CommandType.Text;
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void InsertRecordToDB(RecordEntry record)
        {
            string resPath = AppDomain.CurrentDomain.BaseDirectory; // base directory path of the debug folder
            string dbPath = resPath.Replace("bin\\Debug\\net6.0\\", "Data"); // filePath to the Billing.mdb database.
            // string dbPath = resPath.Replace("bin\\Release\\net6.0\\", "Data");
            
            if (record.JointFirstName == null)
            {
                record.JointFirstName = string.Empty;
            }

            using(OleDbConnection connection = new OleDbConnection($"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath}\\FirstChurchOfGodDatabase.mdb"))
            {
                string sql = "INSERT into TitheOfferingBook (FirstName, LastName, IsJointAccount, JointFirstName, Tithe, Offering, GaveBy, OfferType, DateRecorded, CheckNumber, OfferType2, Offering2) Values (@FirstName, @LastName, @JointAccount, @JointFirstName, @Tithe, @Offering, @GaveBy, @OfferType, @DateRecorded, @CheckNumber, @OfferType2, @Offering2)";
                
                using (OleDbCommand command = new OleDbCommand(sql, connection))
                {
                    command.CommandType = System.Data.CommandType.Text;

                    //add parameters
                    OleDbParameter parameter = new OleDbParameter
                    {
                        ParameterName = "@FirstName",
                        Value = record.FirstName,
                        OleDbType = OleDbType.WChar,
                        Size = 255
                    };
                    command.Parameters.Add(parameter);

                    parameter = new OleDbParameter
                    {
                        ParameterName = "@LastName",
                        Value = record.LastName,
                        OleDbType = OleDbType.WChar,
                        Size = 255
                    };
                    command.Parameters.Add(parameter);

                    parameter = new OleDbParameter
                    {
                        ParameterName = "@JointAccount",
                        Value = record.JointAccount,
                        OleDbType = OleDbType.Boolean,
                    };
                    command.Parameters.Add(parameter);

                    parameter = new OleDbParameter
                    {
                        ParameterName = "@JointFirstName",
                        Value = record.JointFirstName,
                        OleDbType = OleDbType.WChar,
                        Size = 255
                    };
                    command.Parameters.Add(parameter);

                    parameter = new OleDbParameter
                    {
                        ParameterName = "@Tithe",
                        Value = record.Tithe,
                        OleDbType = OleDbType.Currency
                    };
                    command.Parameters.Add(parameter);

                    parameter = new OleDbParameter
                    {
                        ParameterName = "@Offering",
                        Value = record.Offering,
                        OleDbType = OleDbType.Currency
                    };
                    command.Parameters.Add(parameter);

                    parameter = new OleDbParameter
                    {
                        ParameterName = "@GaveBy",
                        Value = record.GaveBy,
                        OleDbType = OleDbType.WChar,
                        Size = 255
                    };
                    command.Parameters.Add(parameter);

                    parameter = new OleDbParameter
                    {
                        ParameterName = "@OfferType",
                        Value = record.OfferType,
                        OleDbType = OleDbType.WChar,
                        Size = 255
                    };
                    command.Parameters.Add(parameter);

                    parameter = new OleDbParameter
                    {
                        ParameterName = "@DateRecorded",
                        Value = record.DateRecorded,
                        OleDbType = OleDbType.WChar,
                        Size = 255
                    };
                    command.Parameters.Add(parameter);

                    parameter = new OleDbParameter
                    {
                        ParameterName = "@CheckNumber",
                        Value = record.CheckNumber,
                        OleDbType = OleDbType.Integer
                    };
                    command.Parameters.Add(parameter);

                    parameter = new OleDbParameter
                    {
                        ParameterName = "@OfferType2",
                        Value = record.OfferType2,
                        OleDbType = OleDbType.WChar,
                        Size = 255
                    };
                    command.Parameters.Add(parameter);

                    parameter = new OleDbParameter
                    {
                        ParameterName = "@Offering2",
                        Value = record.Offering2,
                        OleDbType = OleDbType.Currency
                    };
                    command.Parameters.Add(parameter);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public List<RecordEntry> GetRecordsUsingOneDate(string dateSelect) 
        { 
            List<RecordEntry> records = new List<RecordEntry>();
            string resPath = AppDomain.CurrentDomain.BaseDirectory; // base directory path of the debug folder
            string dbPath = resPath.Replace("bin\\Debug\\net6.0\\", "Data"); // filePath to the Billing.mdb database.
            // string dbPath = resPath.Replace("bin\\Release\\net6.0\\", "Data");
            OleDbConnection con = new OleDbConnection($"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath}\\FirstChurchOfGodDatabase.mdb");
            OleDbCommand cmd = con.CreateCommand();
            con.Open();

            cmd.CommandText = "SELECT * FROM TitheOfferingBook WHERE DateRecorded = #" + dateSelect + "#";

                OleDbDataReader reader = cmd.ExecuteReader();
                DataTable myDataTable = new DataTable();
                myDataTable.Load(reader);
                foreach (DataRow row in myDataTable.Rows)
                {
                    string firstName, lastName, jointFirstName, gaveBy, offerType, offerType2;
                    bool isJointAccount;
                    decimal tithe, offering, offering2;
                    int id, checkNumber;
                    string dateRecordedString;
                    DateTime dateRecorded;

                    id = (int)row["ID"];
                    firstName = (string)row["FirstName"];
                    lastName = (string)row["LastName"];
                    isJointAccount = (bool)row["IsJointAccount"];
                    try
                    {
                        jointFirstName = (string)row["JointFirstName"];  // get the first name of the joint account
                    }
                    catch
                    {
                        jointFirstName = "";  // otherwise leave it empty
                    }
                    gaveBy = (string)row["GaveBy"];
                    tithe = (decimal)row["Tithe"];
                    offering = (decimal)row["Offering"];                   
                    offerType = (string)row["OfferType"];
                    dateRecorded = (DateTime)row["DateRecorded"];
                    dateRecordedString = dateRecorded.ToString("MM/dd/yyyy");
                    checkNumber = (int)row["CheckNumber"];
                    offerType2 = (string)row["OfferType2"];
                    offering2 = (decimal)row["Offering2"];
                    RecordEntry record = new(id, firstName, lastName, isJointAccount, jointFirstName, gaveBy, tithe, offering, offerType, offering2, offerType2, checkNumber, dateRecordedString);
                    records.Add(record);
                
                con.Close();
            }
                return records;
        }

        public List<RecordEntry> GetRecordsUsingTwoDates(string startDate, string endDate)
        {

            List<RecordEntry> records = new List<RecordEntry>();
            string resPath = AppDomain.CurrentDomain.BaseDirectory; // base directory path of the debug folder
            string dbPath = resPath.Replace("bin\\Debug\\net6.0\\", "Data"); // filePath to the Billing.mdb database.
            // string dbPath = resPath.Replace("bin\\Release\\net6.0\\", "Data");
            OleDbConnection con = new OleDbConnection($"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath}\\FirstChurchOfGodDatabase.mdb");
            OleDbCommand cmd = con.CreateCommand();
            con.Open();

            cmd.CommandText = "SELECT * FROM TitheOfferingBook WHERE dateRecorded Between #" + startDate + "# AND #" + endDate + "# ORDER BY DateRecorded ASC";
            OleDbDataReader reader = cmd.ExecuteReader();
            DataTable myDataTable = new DataTable();
            myDataTable.Load(reader);
            foreach (DataRow row in myDataTable.Rows)
            {
                string firstName, lastName, jointFirstName, gaveBy, offerType, offerType2;
                bool isJointAccount;
                decimal tithe, offering, offering2;
                int id, checkNumber;
                string dateRecordedString;
                DateTime dateRecorded;

                id = (int)row["ID"];
                firstName = (string)row["FirstName"];
                lastName = (string)row["LastName"];
                isJointAccount = (bool)row["IsJointAccount"];
                try
                {
                    jointFirstName = (string)row["JointFirstName"];  // get the first name of the joint account
                }
                catch
                {
                    jointFirstName = "";  // otherwise leave it empty
                }
                gaveBy = (string)row["GaveBy"];
                tithe = (decimal)row["Tithe"];
                offering = (decimal)row["Offering"];           
                offerType = (string)row["OfferType"];
                dateRecorded = (DateTime)row["DateRecorded"];
                dateRecordedString = dateRecorded.ToString("MM/dd/yyyy");
                checkNumber = (int)row["CheckNumber"];
                offerType2 = (string)row["OfferType2"];
                offering2 = (decimal)row["Offering2"];
                RecordEntry record = new(id, firstName, lastName, isJointAccount, jointFirstName, gaveBy, tithe, offering, offerType, offering2, offerType2, checkNumber, dateRecordedString);
                records.Add(record);
            }
            con.Close();

            return records;
        }


        public RecordEntry GetRecordEntry(int id)
        {
            RecordEntry record = new RecordEntry();
            string resPath = AppDomain.CurrentDomain.BaseDirectory; // base directory path of the debug folder
            string dbPath = resPath.Replace("bin\\Debug\\net6.0\\", "Data"); // filePath to the Billing.mdb database.
            // string dbPath = resPath.Replace("bin\\Release\\net6.0\\", "Data");

            using (OleDbConnection connection = new OleDbConnection($"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath}\\FirstChurchOfGodDatabase.mdb"))
            {
                string sql = $"SELECT * from TitheOfferingBook Where ID={id}";
                OleDbCommand command = new OleDbCommand(sql, connection);
                command.CommandType = System.Data.CommandType.Text;
                connection.Open();

                using (OleDbDataReader dataReader = command.ExecuteReader())
                {

                    while (dataReader.Read())
                    {
                        record.Id = Convert.ToInt32(dataReader["ID"]);
                        record.FirstName = Convert.ToString(dataReader["FirstName"]);
                        record.LastName = Convert.ToString(dataReader["LastName"]);
                        record.JointAccount = Convert.ToBoolean(dataReader["IsJointAccount"]);
                        record.JointFirstName = Convert.ToString(dataReader["JointFirstName"]);
                        record.Tithe = Convert.ToDecimal(dataReader["Tithe"]);
                        record.Offering = Convert.ToDecimal(dataReader["Offering"]);
                        record.GaveBy = Convert.ToString(dataReader["GaveBy"]);
                        record.OfferType = Convert.ToString(dataReader["OfferType"]);
                        record.DateRecorded = Convert.ToString(dataReader["DateRecorded"]);
                        record.CheckNumber = Convert.ToInt32(dataReader["CheckNumber"]);
                        record.OfferType2 = Convert.ToString(dataReader["OfferType2"]);
                        record.Offering2 = Convert.ToDecimal(dataReader["Offering2"]);
                    }
                }
                connection.Close();
            }
                return record;
        }

        public void UpdateRecordEntryToDB(RecordEntry record)
        {
            string resPath = AppDomain.CurrentDomain.BaseDirectory; // base directory path of the debug folder
            string dbPath = resPath.Replace("bin\\Debug\\net6.0\\", "Data"); // filePath to the Billing.mdb database.
            // string dbPath = resPath.Replace("bin\\Release\\net6.0\\", "Data");

            using (OleDbConnection connection = new OleDbConnection($"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath}\\FirstChurchOfGodDatabase.mdb"))
            {
                string sql = $"Update TitheOfferingBook SET Tithe={record.Tithe}, Offering={record.Offering}, GaveBy='{record.GaveBy}', OfferType='{record.OfferType}', CheckNumber={record.CheckNumber}, OfferType2='{record.OfferType2}', Offering2={record.Offering2} Where ID={record.Id}";
                using (OleDbCommand command = new OleDbCommand(sql, connection))
                {
                    command.CommandType = System.Data.CommandType.Text;
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public List<RecordEntry> GetIndividualRecordsFromDBUsingNameAndDates(ChurchMember member, string startDate, string endDate)
        {
            List<RecordEntry> records = new List<RecordEntry>();
            string resPath = AppDomain.CurrentDomain.BaseDirectory; // base directory path of the debug folder
            string dbPath = resPath.Replace("bin\\Debug\\net6.0\\", "Data"); // filePath to the Billing.mdb database.
            // string dbPath = resPath.Replace("bin\\Release\\net6.0\\", "Data");
            OleDbConnection con = new OleDbConnection($"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath}\\FirstChurchOfGodDatabase.mdb");
            OleDbCommand cmd = con.CreateCommand();
            con.Open();
            cmd.CommandText = "SELECT * FROM TitheOfferingBook WHERE dateRecorded Between #" + startDate + "# AND #" + endDate + "# AND FirstName='" + member.FirstName + "' AND LastName='" + member.LastName + "' ORDER BY DateRecorded ASC";
            OleDbDataReader reader = cmd.ExecuteReader();
            DataTable myDataTable = new DataTable();
            myDataTable.Load(reader);
            foreach (DataRow row in myDataTable.Rows)
            {
                string firstName, lastName, jointFirstName, gaveBy, offerType, offerType2, dateRecordedString;
                bool isJointAccount;
                decimal tithe, offering, offering2;
                int id, checkNumber;
                DateTime dateRecorded;
                id = (int)row["ID"];
                firstName = (string)row["FirstName"];
                lastName = (string)row["LastName"];
                isJointAccount = (bool)row["IsJointAccount"];
                try
                {
                    jointFirstName = (string)row["JointFirstName"];
                }
                catch (InvalidCastException castEx)
                {
                    jointFirstName = "";
                }
                gaveBy = (string)row["GaveBy"];
                tithe = (decimal)row["Tithe"];
                offering = (decimal)row["Offering"];
                offerType = (string)row["OfferType"];
                dateRecorded = (DateTime)row["DateRecorded"];
                dateRecordedString = dateRecorded.ToString("MM/dd/yyyy");
                checkNumber = (int)row["CheckNumber"];
                offerType2 = (string)row["OfferType2"];
                offering2 = (decimal)row["Offering2"];
                RecordEntry record = new(id, firstName, lastName, isJointAccount, jointFirstName, gaveBy, tithe, offering, offerType, offering2, offerType2, checkNumber, dateRecordedString);
                records.Add(record);
            }
            con.Close();

            return records;
        }
    }
}