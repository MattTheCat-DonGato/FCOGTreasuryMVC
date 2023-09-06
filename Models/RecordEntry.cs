namespace FirstChurchOfGodTreasuryMVC.Models
{
    public class RecordEntry
    {
        public int Id { get; set; } // record Id number
        public string? FirstName { get; set; } // member's first name
        public string? LastName { get; set; } // member's last name
        public bool JointAccount { get; set; } // determine if this member is a joint account or not
        public string? JointFirstName { get; set; } // joint member's first name
        public string? GaveBy { get; set; } // How the tithe and offering was given
        public decimal Tithe { get; set; }   // Tithe Value
        public decimal Offering { get; set; } // Offering Number 1 Value
        public string? OfferType { get; set; } // Determines how the first offering will be added in calculating totals for weekly report
        public decimal Offering2 { get; set; } // Offering Number 2 Value
        public string? OfferType2 { get; set; } // Determines how the second offering will be added in calculation totals for weekly report
        public int CheckNumber { get; set; } // If Gave By value is either Check or Cash And Check, input the check number's value, set to 0
        public string? DateRecorded { get; set; }  // Time this record was recorded

        public RecordEntry()
        {

        }

        // Override constructor used when creating a record
        public RecordEntry(int id, string firstName, string lastName, bool jointAccount, string jointFirstName)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            JointAccount = jointAccount;
            JointFirstName = jointFirstName;
        }

        // Override constructor used when inserting records into DB
        public RecordEntry(string firstName, string lastName, bool jointAccount, string jointFirstName, decimal tithe, string gaveBy, decimal offering, string offerType, decimal offering2, string offerType2, int checkNumber, string dateRecorded)
        {
            FirstName = firstName;
            LastName = lastName;
            JointAccount = jointAccount;
            JointFirstName = jointFirstName;
            GaveBy = gaveBy;
            Tithe = tithe;
            Offering = offering;
            OfferType = offerType;
            Offering2 = offering2;
            OfferType2 = offerType2;
            CheckNumber = checkNumber;
            DateRecorded = dateRecorded;
        }

        public RecordEntry(int id, string firstName, string lastName, bool jointAccount, string jointFirstName, string gaveBy, decimal tithe, decimal offering, string offerType, decimal offering2, string offerType2, int checkNumber, string dateRecorded)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            JointAccount = jointAccount;
            JointFirstName = jointFirstName;
            GaveBy = gaveBy;
            Tithe = tithe;
            Offering = offering;
            OfferType = offerType;
            Offering2 = offering2;
            OfferType2 = offerType2;
            CheckNumber = checkNumber;
            DateRecorded = dateRecorded;
        }

    }
}
