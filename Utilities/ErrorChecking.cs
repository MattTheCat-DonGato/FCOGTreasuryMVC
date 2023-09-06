using FirstChurchOfGodTreasuryMVC.Models;
using System.Text.RegularExpressions;

namespace FCOGTreasury.Utilities
{
    public class ErrorChecking
    {
        public bool CheckName(string name)
        {
            bool isValidName = false;  // flag - initially set to false if the input doesn't match 
            string namePattern = @"^[A-Z]+([a-z]{0,20})"; // name must start with a capital letter [A-Z] and can end in a lower letter [a-z] between 0 to 20 times.
            Regex reg = new Regex(namePattern);

            if (reg.IsMatch(name))
                isValidName = true;

            return isValidName; // return the flag
        }

        public string FixDateStringValue(string dateString)
        {
            // date comes in as yyyy-MM-dd
            // date must be in MM-dd-yyyy to query data with the correct format
            string[] listOfNumbers = dateString.Split('-');
            string newDate = listOfNumbers[1] + "/" + listOfNumbers[2] + "/" + listOfNumbers[0];
            return newDate;
        }

        public string FixMonthValueOfDate(string newDate)
        {
            // month is being read as 01 instead of 1, must replace the first nine months if this occurs
            if (newDate.StartsWith("01"))
            {
                newDate.Replace("01", "1");
            }
            if (newDate.StartsWith("02"))
            {
                newDate.Replace("02", "2");
            }
            if (newDate.StartsWith("03"))
            {
                newDate.Replace("03", "3");
            }
            if (newDate.StartsWith("04"))
            {
                newDate.Replace("04", "4");
            }
            if (newDate.StartsWith("05"))
            {
                newDate.Replace("05", "5");
            }
            if (newDate.StartsWith("06"))
            {
                newDate.Replace("07", "7");
            }
            if (newDate.StartsWith("08"))
            {
                newDate.Replace("08", "8");
            }
            if (newDate.StartsWith("09"))
            {
                newDate.Replace("09", "9");
            }
            return newDate;
        }

        public Tuple<string, bool> FindRecordEntryErrors(RecordEntry recordEntry, string gaveBy, string checkNumber, string titheAmount, string offerType1, string offerAmount, string offerType2, string offerAmount2)
        {
            decimal tithe, offering, offering2;
            int checkNum;
            bool titheCorrect = decimal.TryParse(titheAmount, out tithe);
            bool offerCorrect = decimal.TryParse(offerAmount, out offering);
            bool offer2Correct = decimal.TryParse(offerAmount2, out offering2);
            bool checkNumberCorrect = int.TryParse(checkNumber, out checkNum);
            bool errorFound = false;
            string message = "";

            if (!titheCorrect)
            {
                message = "Invalid data has been entered in the Tithe Amount field. Please input numbers with up to 2 decimal values or leave it blank.";
                errorFound = true;
            }
            else
            {
                if (!offerCorrect)
                {
                    message = "Invalid data has been entered in the first Offer Amount field. Please input numbers with up to 2 decimal values or leave it blank.";
                    errorFound = true;
                }
                else
                {
                    if(!offer2Correct) 
                    {
                        message = "Invalid data has been entered in the second Offer Amount field. Please input numbers with up to 2 decimal values or leave it blank.";
                        errorFound = true;
                    }
                    else
                    {
                        if (!checkNumberCorrect)
                        {
                            message = "Invalid data has been entered in the Check Number field. Please input numbers only or leave it blank.";
                            errorFound = true;
                        }
                        else
                        {
                            if (recordEntry.FirstName == "Sunday" && recordEntry.LastName == "School")
                            {
                                // special case - Sunday School
                                if (tithe != 0 && gaveBy != "None")
                                {
                                    message = "For Sunday School, the Tithe Amount must be 0 or left blank, and None must be selected from the Gave By dropbox. Please change these values before submitting.";
                                    errorFound = true;
                                }
                                else
                                {
                                    if (checkNum != 0)
                                    {
                                        message = "Check number must be 0 for Sunday School, input 0 or leave it blank.";
                                        errorFound = true;
                                    }
                                    else
                                    {
                                        if (offerType1 == "Sunday School")
                                        {
                                            if(offerType1 == offerType2)
                                            {
                                                message = "Duplicate offering types are not allowed. Please set Offer Type 2 to a different value.";
                                                errorFound = true;
                                            }
                                            else
                                            {
                                                if(offerType2 != "None")
                                                {
                                                    message = "Only Sunday School needs to be selected from Offer Type 1. Offer Type 2 must be set to None.";
                                                    errorFound = true;
                                                }
                                                else
                                                {
                                                    if(offering2 != 0)
                                                    {
                                                        message = "The 2nd Offering Amount value must be 0 or left blank.";
                                                        errorFound = true;
                                                    }
                                                    else
                                                    {
                                                        if(offering > 0)
                                                        {
                                                            message = "OK - Submit Record to Database";
                                                        }
                                                        else
                                                        {
                                                            message = "The 1st Offering Amount cannot be 0 or have a negative value. Please input a positive value for the 1st Offering Amount textbox.";
                                                            errorFound = true;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            message = "Sunday School must be selected from the Offer Type 1 dropbox.";
                                            errorFound = true;
                                        }
                                    }
                                }
                            }
                            else if (recordEntry.FirstName == "Mission" && recordEntry.LastName == "March")
                            {
                                // special case - Mission March
                                if (tithe != 0 && gaveBy != "None")
                                {
                                    message = "For Mission March, the Tithe Amount must be 0 or left blank, and None must be selected from the Gave By dropbox. Please change these values before submitting.";
                                    errorFound = true;
                                }
                                else
                                {
                                    if (checkNum != 0)
                                    {
                                        message = "Check number must be 0 for Mission March, set to 0 or leave it blank then resubmit the form.";
                                        errorFound = true;
                                    }
                                    else
                                    {
                                        if (offerType1 == "Mission March")
                                        {
                                            if (offerType1 == offerType2)
                                            {
                                                message = "Duplicate offering types are not allowed. Please set Offer Type 2 to a different value.";
                                                errorFound = true;
                                            }
                                            else
                                            {
                                                if (offerType2 != "None")
                                                {
                                                    message = "Only Mission March needs to be selected from Offer Type 1. Offer Type 2 must be set to None.";
                                                    errorFound = true;
                                                }
                                                else
                                                {
                                                    if (offering2 != 0)
                                                    {
                                                        message = "The 2nd Offering Amount value must be 0 or left blank.";
                                                        errorFound = true;
                                                    }
                                                    else
                                                    {
                                                        if (offering > 0)
                                                        {
                                                            message = "OK - Submit Record to Database";
                                                        }
                                                        else
                                                        {
                                                            message = "The 1st Offering Amount cannot be 0 or have a negative value. Please input a positive value for the 1st Offering Amount textbox.";
                                                            errorFound = true;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            message = "Mission March must be selected from the Offer Type 1 dropbox.";
                                            errorFound = true;
                                        }

                                    }
                                }
                            }
                            else if (recordEntry.FirstName == "General" && recordEntry.LastName == "Offering")
                            {
                                // special case - General Offering
                                if (tithe != 0 && gaveBy != "None")
                                {
                                    message = "For General Offering, the Tithe Amount must be 0 or left blank, and None must be selected from the Gave By dropbox. Please change these values before submitting.";
                                    errorFound = true;
                                }
                                else
                                {
                                    if (checkNum != 0)
                                    {
                                        message = "Check number for General Offering must be set to 0 or left blank.";
                                        errorFound = true;
                                    }
                                    else
                                    {
                                        if (offerType1 == "General Offering")
                                        {
                                            if (offerType1 == offerType2)
                                            {
                                                message = "Duplicate offering types are not allowed. Please set Offer Type 2 to a different value.";
                                                errorFound = true;
                                            }
                                            else
                                            {
                                                if (offerType2 != "None")
                                                {
                                                    message = "Only General Offering needs to be selected from Offer Type 1. Offer Type 2 must be set to None.";
                                                    errorFound = true;
                                                }
                                                else
                                                {
                                                    if (offering2 != 0)
                                                    {
                                                        message = "The 2nd Offering Amount value must be 0 or left blank.";
                                                        errorFound = true;
                                                    }
                                                    else
                                                    {
                                                        if (offering > 0)
                                                        {
                                                            message = "OK - Submit Record to Database";
                                                        }
                                                        else
                                                        {
                                                            message = "The 1st Offering Amount cannot be 0 or have a negative value. Please input a positive value for the 1st Offering Amount textbox.";
                                                            errorFound = true;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            message = "General Offering must be selected from the Offer Type 1 dropbox.";
                                            errorFound = true;
                                        }
                                    }
                                }
                            }
                            else if (recordEntry.FirstName == "Love" && recordEntry.LastName == "Offering")
                            {
                                // special case - Love Offering
                                if (tithe != 0 && gaveBy != "None")
                                {
                                    message = "For Love Offerings, the Tithe Amount must be 0 or left blank, and None must be selected from the Tithe Type. Please change these values before submitting.";
                                    errorFound = true;
                                }
                                else
                                {
                                    if (checkNum != 0)
                                    {
                                        message = "Check Number for Love Offering must be 0 or left blank.";
                                        errorFound = true;
                                    }
                                    else
                                    {
                                        if (offerType1 == "Love Offering")
                                        {
                                            if (offerType1 == offerType2)
                                            {
                                                message = "Duplicate offering types are not allowed. Please set Offer Type 2 to a different value.";
                                                errorFound = true;
                                            }
                                            else
                                            {
                                                if (offerType2 != "None")
                                                {
                                                    message = "Only General Offering needs to be selected from Offer Type 1. Offer Type 2 must be set to None.";
                                                    errorFound = true;
                                                }
                                                else
                                                {
                                                    if (offering2 != 0)
                                                    {
                                                        message = "The 2nd Offering Amount value must be 0 or left blank.";
                                                        errorFound = true;
                                                    }
                                                    else
                                                    {
                                                        if (offering > 0)
                                                        {
                                                            message = "OK - Submit Record to Database";
                                                        }
                                                        else
                                                        {
                                                            message = "The 1st Offering Amount cannot be 0 or have a negative value. Please input a positive value for the 1st Offering Amount textbox.";
                                                            errorFound = true;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            message = "Love Offering must be selected from the Offer Type 1 dropbox.";
                                            errorFound = true;
                                        }
                                    }
                                }
                            }
                            else if (recordEntry.FirstName == "Seed" && recordEntry.LastName == "Offering")
                            {
                                // special case - Seed Offering
                                if (tithe != 0 && gaveBy != "None")
                                {
                                    message = "For Seed Offering, the Tithe Amount must be 0 or left blank, and None must be selected from the Gave By dropbox. Please change these values before submitting.";
                                    errorFound = true;
                                }
                                else
                                {
                                    if (checkNum != 0)
                                    {
                                        message = "Check Numbers for Seed Offering must be set to 0 or left it blank.";
                                        errorFound = true;
                                    }
                                    else
                                    {
                                        if (offerType1 == "Seed Offering")
                                        {
                                            if (offerType1 == offerType2)
                                            {
                                                message = "Duplicate offering types are not allowed. Please set Offer Type 2 to a different value.";
                                                errorFound = true;
                                            }
                                            else
                                            {
                                                if (offerType2 != "None")
                                                {
                                                    message = "Only Seed Offering needs to be selected from Offer Type 1. Offer Type 2 must be set to None.";
                                                    errorFound = true;
                                                }
                                                else
                                                {
                                                    if (offering2 != 0)
                                                    {
                                                        message = "The 2nd Offering Amount value must be 0 or left blank.";
                                                        errorFound = true;
                                                    }
                                                    else
                                                    {
                                                        if (offering > 0)
                                                        {
                                                            message = "OK - Submit Record to Database";
                                                        }
                                                        else
                                                        {
                                                            message = "The 1st Offering Amount cannot be 0 or have a negative value. Please input a positive value for the 1st Offering Amount textbox.";
                                                            errorFound = true;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            message = "Seed Offering must be selected from the Offer Type 1 dropbox.";
                                            errorFound = true;
                                        }
                                    }
                                }
                            }
                            else if (recordEntry.FirstName == "Chicken" && recordEntry.LastName == "Barbecue")
                            {
                                // special case - Chicken Barbacue
                                if (tithe != 0 && gaveBy != "None")
                                {
                                    message = "For Chicken Barbecue, the Tithe Amount must be 0 or left blank, and None must be selected from the Gave By dropbox. Please change these values before submitting.";
                                    errorFound = true;
                                }
                                else
                                {
                                    if (checkNum != 0)
                                    {
                                        message = "Check Numbers for Chicken Barbacue must be set to 0 or left blank.";
                                        errorFound = true;
                                    }
                                    else
                                    {
                                        if (offerType1 == "Chicken Barbecue")
                                        {
                                            if (offerType1 == offerType2)
                                            {
                                                message = "Duplicate offering types are not allowed. Please set Offer Type 2 to a different value.";
                                                errorFound = true;
                                            }
                                            else
                                            {
                                                if (offerType2 != "None")
                                                {
                                                    message = "Only Chicken Barbecue needs to be selected from Offer Type 1. Offer Type 2 must be set to None.";
                                                    errorFound = true;
                                                }
                                                else
                                                {
                                                    if (offering2 != 0)
                                                    {
                                                        message = "The 2nd Offering Amount value must be 0 or left blank.";
                                                        errorFound = true;
                                                    }
                                                    else
                                                    {
                                                        if (offering > 0)
                                                        {
                                                            message = "OK - Submit Record to Database";
                                                        }
                                                        else
                                                        {
                                                            message = "The 1st Offering Amount cannot be 0 or have a negative value. Please input a positive value for the 1st Offering Amount textbox.";
                                                            errorFound = true;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            message = "Chicken Barbecue must be selected from the Offer Type 1 dropbox.";
                                            errorFound = true;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                // non special cases - regular church members 
                                switch (gaveBy)
                                {
                                    case "Cash And Check":
                                        {
                                            if (checkNum <= 0)
                                            {
                                                message = "Check numbers cannot be 0 or a negative value. Please input a positive value for the Check Number.";
                                                errorFound = true;
                                            }
                                            else
                                            {
                                                if (tithe <= 0) 
                                                {
                                                    message = "A tithe cannot be 0 or a negative number for church members. This field is required for regular Church Members in order for the Record to be saved to the database.";
                                                    errorFound = true;
                                                }
                                                else
                                                {
                                                    if(offerType1 == "None" && offerType2 == "None")
                                                    {
                                                        if (offering != 0 && offering2 !=0)
                                                        {
                                                            message = "If both offering types are set to None then both Offering Amounts must be 0 or left blank";
                                                            errorFound = true;
                                                        }
                                                        else
                                                        {
                                                            message = "OK - Submit to Database";
                                                        }
                                                    }
                                                    else if(offerType2 == "None")
                                                    {
                                                        if(offering2 != 0)
                                                        {
                                                            message = "If Offer Type 2 is set to None only, then the 2nd offering amount must be 0 or left blank.";
                                                            errorFound = true;
                                                        }
                                                        else
                                                        {
                                                            if (offering <= 0)
                                                            {
                                                                message = "The 1st Offering Amount cannot be 0 or negative. Please enter a positive number with up to 2 decimals.";
                                                                errorFound = true;
                                                            }
                                                            else
                                                            {
                                                                message = "OK - Submit to Database";
                                                            }
                                                        }
                                                    }
                                                    else if (offerType1 == "None")
                                                    {
                                                        if (offering != 0)
                                                        {
                                                            message = "If Offer Type 1 is set to None only, then the 1st offering amount must be 0 or left blank.";
                                                            errorFound = true;
                                                        }
                                                        else
                                                        {
                                                            if (offering2 <= 0)
                                                            {
                                                                message = "The 2nd Offering Amount cannot be 0 or negative. Please enter a positive number with up to 2 decimals.";
                                                                errorFound = true;
                                                            }
                                                            else
                                                            {
                                                                message = "OK - Submit to Database";
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if(offerType1 == offerType2)
                                                        {
                                                            message = "There cannot be duplicate offer types selected. Please select unique Offer Types for both dropboxes.";
                                                            errorFound = true;
                                                        }
                                                        else
                                                        {
                                                            if(offering <= 0 || offering2 <= 0)
                                                            {
                                                                message = "Either one or both of the Offering Amount values are 0 or a negative number. Please input positive numbers with up to 2 decimal numbers for both 1st and 2nd Offering textboxes.";
                                                                errorFound = true;
                                                            }
                                                            else
                                                            {
                                                                message = "OK - Submit to Database";
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    case "Cash":
                                        {
                                            if (checkNum != 0)
                                            {
                                                message = "If the tithe was given in Cash, then the Check Number must be 0 or left blank. Please change these values and resubmit the form.";
                                                errorFound = true;
                                            }
                                            else
                                            {
                                                if (tithe <= 0)
                                                {
                                                    message = "A tithe cannot be 0 or a negative number for church members. This field is required for regular Church Members in order for the Record to be saved to the database.";
                                                    errorFound = true;
                                                }
                                                else
                                                {
                                                    if (offerType1 == "None" && offerType2 == "None")
                                                    {
                                                        if (offering != 0 && offering2 != 0)
                                                        {
                                                            message = "If both offering types are set to None then both Offering Amounts must be 0 or left blank";
                                                            errorFound = true;
                                                        }
                                                        else
                                                        {
                                                            message = "OK - Submit to Database";
                                                        }
                                                    }
                                                    else if (offerType2 == "None")
                                                    {
                                                        if (offering2 != 0)
                                                        {
                                                            message = "If Offer Type 2 is set to None only, then the 2nd offering amount must be 0 or left blank.";
                                                            errorFound = true;
                                                        }
                                                        else
                                                        {
                                                            if (offering <= 0)
                                                            {
                                                                message = "The 1st Offering Amount cannot be 0 or negative. Please enter a positive number with up to 2 decimals.";
                                                                errorFound = true;
                                                            }
                                                            else
                                                            {
                                                                message = "OK - Submit to Database";
                                                            }
                                                        }
                                                    }
                                                    else if (offerType1 == "None")
                                                    {
                                                        if (offering != 0)
                                                        {
                                                            message = "If Offer Type 1 is set to None only, then the 1st offering amount must be 0 or left blank.";
                                                            errorFound = true;
                                                        }
                                                        else
                                                        {
                                                            if (offering2 <= 0)
                                                            {
                                                                message = "The 2nd Offering Amount cannot be 0 or negative. Please enter a positive number with up to 2 decimals.";
                                                                errorFound = true;
                                                            }
                                                            else
                                                            {
                                                                message = "OK - Submit to Database";
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (offerType1 == offerType2)
                                                        {
                                                            message = "There cannot be duplicate offer types selected. Please select unique Offer Types for both dropboxes.";
                                                            errorFound = true;
                                                        }
                                                        else
                                                        {
                                                            if (offering <= 0 || offering2 <= 0)
                                                            {
                                                                message = "Either one or both of the Offering Amount values are 0 or a negative number. Please input positive numbers with up to 2 decimal numbers for both 1st and 2nd Offering textboxes.";
                                                                errorFound = true;
                                                            }
                                                            else
                                                            {
                                                                message = "OK - Submit to Database";
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    case "Check":
                                        {
                                            if (checkNum <= 0)
                                            {
                                                message = "Negative numbers and 0 are not valid as check numbers. Please enter a positive number for the Check Number.";
                                                errorFound = true;
                                            }
                                            else
                                            {
                                                if (tithe <= 0)
                                                {
                                                    message = "A tithe cannot be 0 or a negative number for church members. This field is required for regular Church Members in order for the Record to be saved to the database.";
                                                    errorFound = true;
                                                }
                                                else
                                                {
                                                    if (offerType1 == "None" && offerType2 == "None")
                                                    {
                                                        if (offering != 0 && offering2 != 0)
                                                        {
                                                            message = "If both offering types are set to None then both Offering Amounts must be 0 or left blank";
                                                            errorFound = true;
                                                        }
                                                        else
                                                        {
                                                            message = "OK - Submit to Database";
                                                        }
                                                    }
                                                    else if (offerType2 == "None")
                                                    {
                                                        if (offering2 != 0)
                                                        {
                                                            message = "If Offer Type 2 is set to None only, then the 2nd offering amount must be 0 or left blank.";
                                                            errorFound = true;
                                                        }
                                                        else
                                                        {
                                                            if (offering <= 0)
                                                            {
                                                                message = "The 1st Offering Amount cannot be 0 or negative. Please enter a positive number with up to 2 decimals.";
                                                                errorFound = true;
                                                            }
                                                            else
                                                            {
                                                                message = "OK - Submit to Database";
                                                            }
                                                        }
                                                    }
                                                    else if (offerType1 == "None")
                                                    {
                                                        if (offering != 0)
                                                        {
                                                            message = "If Offer Type 1 is set to None only, then the 1st offering amount must be 0 or left blank.";
                                                            errorFound = true;
                                                        }
                                                        else
                                                        {
                                                            if (offering2 <= 0)
                                                            {
                                                                message = "The 2nd Offering Amount cannot be 0 or negative. Please enter a positive number with up to 2 decimals.";
                                                                errorFound = true;
                                                            }
                                                            else
                                                            {
                                                                message = "OK - Submit to Database";
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (offerType1 == offerType2)
                                                        {
                                                            message = "There cannot be duplicate offer types selected. Please select unique Offer Types for both dropboxes.";
                                                            errorFound = true;
                                                        }
                                                        else
                                                        {
                                                            if (offering <= 0 || offering2 <= 0)
                                                            {
                                                                message = "Either one or both of the Offering Amount values are 0 or a negative number. Please input positive numbers with up to 2 decimal numbers for both 1st and 2nd Offering Amount textboxes.";
                                                                errorFound = true;
                                                            }
                                                            else
                                                            {
                                                                message = "OK - Submit to Database";
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    case "Zelle":
                                        {
                                            if (checkNum != 0)
                                            {
                                                message = "If the tithe was given from Zelle, then the Check Number value must be 0. Please input 0 or leave the field blank in the Check Number textbox.";
                                                errorFound = true;
                                            }
                                            else
                                            {
                                                if (tithe <= 0)
                                                {
                                                    message = "A tithe cannot be 0 or a negative number for church members. This field is required for regular Church Members in order for the Record to be saved to the database.";
                                                    errorFound = true;
                                                }
                                                else
                                                {
                                                    if (offerType1 == "None" && offerType2 == "None")
                                                    {
                                                        if (offering != 0 && offering2 != 0)
                                                        {
                                                            message = "If both offering types are set to None then both Offering Amounts must be 0 or left blank";
                                                            errorFound = true;
                                                        }
                                                        else
                                                        {
                                                            message = "OK - Submit to Database";
                                                        }
                                                    }
                                                    else if (offerType2 == "None")
                                                    {
                                                        if (offering2 != 0)
                                                        {
                                                            message = "If Offer Type 2 is set to None only, then the 2nd offering amount must be 0 or left blank.";
                                                            errorFound = true;
                                                        }
                                                        else
                                                        {
                                                            if (offering <= 0)
                                                            {
                                                                message = "The 1st Offering Amount cannot be 0 or negative. Please enter a positive number with up to 2 decimals.";
                                                                errorFound = true;
                                                            }
                                                            else
                                                            {
                                                                message = "OK - Submit to Database";
                                                            }
                                                        }
                                                    }
                                                    else if (offerType1 == "None")
                                                    {
                                                        if (offering != 0)
                                                        {
                                                            message = "If Offer Type 1 is set to None only, then the 1st offering amount must be 0 or left blank.";
                                                            errorFound = true;
                                                        }
                                                        else
                                                        {
                                                            if (offering2 <= 0)
                                                            {
                                                                message = "The 2nd Offering Amount cannot be 0 or negative. Please enter a positive number with up to 2 decimals.";
                                                                errorFound = true;
                                                            }
                                                            else
                                                            {
                                                                message = "OK - Submit to Database";
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (offerType1 == offerType2)
                                                        {
                                                            message = "There cannot be duplicate offer types selected. Please select unique Offer Types for both dropboxes.";
                                                            errorFound = true;
                                                        }
                                                        else
                                                        {
                                                            if (offering <= 0 || offering2 <= 0)
                                                            {
                                                                message = "Either one or both of the Offering Amount values are 0 or a negative number. Please input positive numbers with up to 2 decimal numbers for both 1st and 2nd Offering textboxes.";
                                                                errorFound = true;
                                                            }
                                                            else
                                                            {
                                                                message = "OK - Submit to Database";
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    case "Other":
                                        {
                                            if (checkNum != 0)
                                            {
                                                message = "If the tithe was given in a different format besides a Check then the Check Number value must be 0. Please input 0 into the Check Number textbox or leave it blank then resubmit the form.";
                                                errorFound = true;
                                            }
                                            else
                                            {
                                                if (tithe <= 0)
                                                {
                                                    message = "A tithe cannot be 0 or a negative number for church members. This field is required for regular Church Members in order for the Record to be saved to the database.";
                                                    errorFound = true;
                                                }
                                                else
                                                {
                                                    if (offerType1 == "None" && offerType2 == "None")
                                                    {
                                                        if (offering != 0 && offering2 != 0)
                                                        {
                                                            message = "If both offering types are set to None then both Offering Amounts must be 0 or left blank";
                                                            errorFound = true;
                                                        }
                                                        else
                                                        {
                                                            message = "OK - Submit to Database";
                                                        }
                                                    }
                                                    else if (offerType2 == "None")
                                                    {
                                                        if (offering2 != 0)
                                                        {
                                                            message = "If Offer Type 2 is set to None only, then the 2nd offering amount must be 0 or left blank.";
                                                            errorFound = true;
                                                        }
                                                        else
                                                        {
                                                            if (offering <= 0)
                                                            {
                                                                message = "The 1st Offering Amount cannot be 0 or negative. Please enter a positive number with up to 2 decimals.";
                                                                errorFound = true;
                                                            }
                                                            else
                                                            {
                                                                message = "OK - Submit to Database";
                                                            }
                                                        }
                                                    }
                                                    else if (offerType1 == "None")
                                                    {
                                                        if (offering != 0)
                                                        {
                                                            message = "If Offer Type 1 is set to None only, then the 1st offering amount must be 0 or left blank.";
                                                            errorFound = true;
                                                        }
                                                        else
                                                        {
                                                            if (offering2 <= 0)
                                                            {
                                                                message = "The 2nd Offering Amount cannot be 0 or negative. Please enter a positive number with up to 2 decimals.";
                                                                errorFound = true;
                                                            }
                                                            else
                                                            {
                                                                message = "OK - Submit to Database";
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (offerType1 == offerType2)
                                                        {
                                                            message = "There cannot be duplicate offer types selected. Please select unique Offer Types for both dropboxes.";
                                                            errorFound = true;
                                                        }
                                                        else
                                                        {
                                                            if (offering <= 0 || offering2 <= 0)
                                                            {
                                                                message = "Either one or both of the Offering Amount values are 0 or a negative number. Please input positive numbers with up to 2 decimal numbers for both 1st and 2nd Offering textboxes.";
                                                                errorFound = true;
                                                            }
                                                            else
                                                            {
                                                                message = "OK - Submit to Database";
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    case "None":
                                        {
                                            message = "You cannot set the Gave By dropbox to None when using regular Church Member data. Please set the Gave By dropbox to a different value.";
                                            errorFound = true;
                                            break;
                                        }
                                }
                            }
                        }
                    }
                }
            }
            return Tuple.Create(message, errorFound);
        }
    }
}