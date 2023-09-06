using System.ComponentModel.DataAnnotations;

namespace FirstChurchOfGodTreasuryMVC.Models
{
    public class ChurchMember
    {
        public int Id { get; set; } // member Id Number
        public string? FirstName { get; set; } // member's first name
        public string? LastName { get; set; } // member's last name
        public bool JointAccount { get; set; } // determine if this member is a joint account or not
        
        public string? JointFirstName { get; set; } // joint member's first name

        public ChurchMember() { }
        public ChurchMember(int id, string firstName, string lastName, bool jointAccount, string jointFirstName)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            JointAccount = jointAccount;
            JointFirstName = jointFirstName;
        }

        public ChurchMember(string firstName, string lastName, bool jointAccount, string jointFirstName)
        {
            FirstName = firstName;
            LastName = lastName;
            JointAccount = jointAccount;
            JointFirstName = jointFirstName;
        }
    }
}
