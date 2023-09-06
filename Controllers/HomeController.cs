using FCOGTreasury.Utilities;
using FirstChurchOfGodTreasuryMVC.Data;
using FirstChurchOfGodTreasuryMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.OleDb;
using System.Runtime.Serialization.Formatters;

#pragma warning disable CA1416 // Validate platform compatibility

namespace FirstChurchOfGodTreasuryMVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // Http Get
        public IActionResult CreateMember()
        {
            return View();
        }

        // Http Post
        [HttpPost]
        public IActionResult CreateMember(ChurchMember churchMember)
        {
            DatabaseWork dbWork = new DatabaseWork();
            ErrorChecking errorCheck = new ErrorChecking();

            // In the event that Joint Account First Name is null from no input, set it as an empty string instead.
            if(churchMember.JointFirstName == null)
            {
                churchMember.JointFirstName = string.Empty;
            }

            // check the names to see if they are valid
            bool validFirstName = errorCheck.CheckName(churchMember.FirstName);
            bool validLastName = errorCheck.CheckName(churchMember.LastName);

            if (validFirstName)
            {
                if(validLastName)
                {
                    if(churchMember.JointAccount)
                    {
                        bool validJointName = errorCheck.CheckName(churchMember.JointFirstName);

                        if(validJointName)
                        {
                            // check if the info put in aleady exists, the id number does not matter in this case.
                            bool doesExist = dbWork.DoesChurchMemberExist(churchMember);

                            if (doesExist)
                            {
                                ViewBag.Result = "This member already exists within this database. Please enter a new one.";
                            }
                            else
                            {
                                dbWork.CreateNewChurchMember(churchMember);
                                ViewBag.Result = "New Church Member Added!";
                            }
                        }
                        else
                        {
                            ViewBag.Result = "The joint account's first name inputted is invalid. Names must start with an uppercase letter and up to 19 more letters.";
                        }
                    }
                    else
                    {
                        // check if the info put in aleady exists, the id number does not matter in this case.
                        bool doesExist = dbWork.DoesChurchMemberExist(churchMember);

                        if (doesExist)
                        {
                            ViewBag.Result = "This member already exists within this database. Please enter a new one.";
                        }
                        else
                        {
                            dbWork.CreateNewChurchMember(churchMember);
                            ViewBag.Result = "New Church Member Added!";
                        }
                    }
                }
                else
                {
                    ViewBag.Result = "The last name inputted is invalid. Names must start with an uppercase letter and up to 19 more letters.";
                }
            }
            else
            {
                ViewBag.Result = "The first name inputted is invalid. Names must start with an uppercase letter and up to 19 more letters.";
            }
            return View();
        }

        public IActionResult ViewMembers()
        {
            // we only want non special case data
            DatabaseWork dbWork = new DatabaseWork();
            List<ChurchMember> churchMembersList = dbWork.GetAllChurchMembers();
            // delete special cases from the list
            for (int i = 0; i < churchMembersList.Count; i++)
            {
                if (churchMembersList[i].FirstName == "General" && churchMembersList[i].LastName == "Offering")
                {
                    churchMembersList.RemoveAt(i);
                }
                if (churchMembersList[i].FirstName == "Sunday" && churchMembersList[i].LastName == "School")
                {
                    churchMembersList.RemoveAt(i);
                }
                if (churchMembersList[i].FirstName == "Mission" && churchMembersList[i].LastName == "March")
                {
                    churchMembersList.RemoveAt(i);
                }
                if (churchMembersList[i].FirstName == "Chicken" && churchMembersList[i].LastName == "Barbecue")
                {
                    churchMembersList.RemoveAt(i);
                }
                if (churchMembersList[i].FirstName == "Love" && churchMembersList[i].LastName == "Offering")
                {
                    churchMembersList.RemoveAt(i);
                }
                if (churchMembersList[i].FirstName == "Seed" && churchMembersList[i].LastName == "Offering")
                {
                    churchMembersList.RemoveAt(i);
                }
            }
            return View(churchMembersList);
        }

        public IActionResult UpdateMember(int id)
        {
            DatabaseWork dbWork = new DatabaseWork();
            ChurchMember churchMember = dbWork.GetChurchMember(id);

            return View(churchMember);
        }

        [HttpPost]
        public IActionResult UpdateMember(ChurchMember churchMember, int id) 
        {
            DatabaseWork dbWork = new DatabaseWork();
            ErrorChecking errorCheck = new ErrorChecking();

            if (churchMember.JointFirstName == null)
            {
                churchMember.JointFirstName = string.Empty;
            }
            
            bool doesExist = dbWork.DoesChurchMemberExist(churchMember);

            if (doesExist) 
            {
                ViewBag.Result = "There can be no duplicate members with the same names, joint settings, and joint first names. Please enter unique data for the record to be updated.";
                return View();
            }
            else
            {
                // check if there is invalid input for JointFirstName based on IsJointAccount's value
                if(churchMember.JointAccount)
                {
                    // IsJointAccount = true
                    if(churchMember.JointFirstName == string.Empty) 
                    {
                        ViewBag.Result = "You set the update as a joint account, but there is no value entered in the Joint First Name. Please enter the joint account's first name.";
                        return View();
                    }
                    else
                    {
                        dbWork.UpdateChurchMemberByIdWithParameters(churchMember, id);
                        return RedirectToAction("ViewMembers");
                    }
                }
                else
                {
                    // IsJointAccount = false
                    if(churchMember.JointFirstName != string.Empty) 
                    {
                        ViewBag.Result = "You set the update as not a joint account, but there is a value entered in the Joint First Name. Please leave the field blank if the update is not a joint account.";
                        return View();
                    }
                    else
                    {
                        dbWork.UpdateChurchMemberByIdWithParameters(churchMember, id);
                        return RedirectToAction("ViewMembers");
                    }
                }
            }    
        }

        [HttpPost]
        public IActionResult DeleteMember(int id)
        {
            DatabaseWork dbWork = new DatabaseWork();
            dbWork.DeleteChurchMemberById(id);
            return RedirectToAction("ViewMembers");
        }

        public IActionResult SelectMember()
        {
            DatabaseWork dbWork = new DatabaseWork();
            List<ChurchMember> churchMembersList = dbWork.GetAllChurchMembers();     
            return View(churchMembersList);
        }

        public IActionResult CreateRecordEntry(int id)
        {
            DatabaseWork dbWork = new DatabaseWork();
            ChurchMember member = dbWork.GetChurchMember(id);
            RecordEntry record = new RecordEntry(member.Id, member.FirstName, member.LastName, member.JointAccount, member.JointFirstName);
            return View(record);
        }

        [HttpPost]
        public IActionResult CreateRecordEntry(RecordEntry recordEntry, string gaveBy, string titheAmount, string checkNumber, string offerType1, string offerAmount, string offerType2, string offerAmount2)
        {
            ErrorChecking errorCheck = new ErrorChecking();

            if (checkNumber == null)
            {
                checkNumber = "0";
            }
            if (titheAmount == null)
            {
                titheAmount = "0";
            }
            if (offerAmount == null)
            {
                offerAmount = "0";
            }
            if(offerAmount2 == null)
            {
                offerAmount2 = "0";
            }

           Tuple<string, bool> result = errorCheck.FindRecordEntryErrors(recordEntry, gaveBy, checkNumber, titheAmount, offerType1, offerAmount, offerType2, offerAmount2);

            if(result.Item2)
            {
                ViewBag.Result = result.Item1;
                return View();
            }
            else
            {
                // insert the record here of no errors were found
                // parse data and insert them into the record object
                // call db method to insert data to database
                DatabaseWork dbWork = new DatabaseWork();
                decimal tithe, offering, offering2;
                int checkNum;
                bool titheCorrect = decimal.TryParse(titheAmount, out tithe);
                bool offerCorrect = decimal.TryParse(offerAmount, out offering);
                bool offer2Correct = decimal.TryParse(offerAmount2, out offering2); 
                bool checkNumberCorrect = int.TryParse(checkNumber, out checkNum);
                string dateRecorded = DateTime.Now.ToString("MM/dd/yyyy");

                recordEntry.GaveBy = gaveBy;
                recordEntry.Tithe = tithe;
                recordEntry.Offering = offering;
                recordEntry.OfferType = offerType1;
                recordEntry.CheckNumber = checkNum;
                recordEntry.DateRecorded = dateRecorded;
                recordEntry.OfferType2 = offerType2;
                recordEntry.Offering2 = offering2;

                dbWork.InsertRecordToDB(recordEntry);
                return RedirectToAction("SelectMember");
            }
        }

        public IActionResult SelectRecords()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SelectRecords(string dateSelect)
        {
            ErrorChecking errorCheck = new ErrorChecking();

            // value is being read as yyyy-MM-dd, fix it to MM-dd-yyyy
            dateSelect = errorCheck.FixDateStringValue(dateSelect);
            dateSelect = errorCheck.FixMonthValueOfDate(dateSelect);
                   
            DatabaseWork dbWork = new DatabaseWork();
            List<RecordEntry> records = dbWork.GetRecordsUsingOneDate(dateSelect);
            return View(records);
        }

        public IActionResult UpdateRecordEntry(int id)
        {
            DatabaseWork dbWork = new DatabaseWork();
            RecordEntry selectedRecord = dbWork.GetRecordEntry(id);
            return View(selectedRecord);
        }

        [HttpPost]
        public IActionResult UpdateRecordEntry(RecordEntry record)
        {
            ErrorChecking errorCheck = new ErrorChecking();
            
            if(record.JointFirstName == null)
            {
                record.JointFirstName = string.Empty;
            }
            
            RecordEntry recordErrorCheck = new RecordEntry(record.Id, record.FirstName, record.LastName, record.JointAccount, record.JointFirstName);
            string gaveBy, checkNumber, titheAmount, offerType1, offerAmount, offerType2, offerAmount2;
            gaveBy = record.GaveBy;
            checkNumber = record.CheckNumber.ToString();
            titheAmount = record.Tithe.ToString();
            offerType1 = record.OfferType.ToString();
            offerAmount = record.Offering.ToString();
            offerType2 = record.OfferType2.ToString();
            offerAmount2 = record.Offering2.ToString();

            Tuple<string, bool> result = errorCheck.FindRecordEntryErrors(recordErrorCheck, gaveBy, checkNumber, titheAmount, offerType1, offerAmount, offerType2, offerAmount2);

            if (result.Item2)
            {
                ViewBag.Result = result.Item1;
                return View();
            }
            else
            {
                DatabaseWork dbWork = new DatabaseWork();
                dbWork.UpdateRecordEntryToDB(record);
                return RedirectToAction("SelectRecords");
            }
        }

        public IActionResult GenerateWeeklyReport()
        {
            return View();
        }
        [HttpPost]
        public IActionResult GenerateWeeklyReport(string dateStartSelect, string dateEndSelect)
        {
            ErrorChecking errorCheck = new ErrorChecking();
            DatabaseWork dbWork = new DatabaseWork();
            Report reporting = new Report();

            // value is being read as yyyy-MM-dd, fix it to MM-dd-yyyy
            dateStartSelect = errorCheck.FixDateStringValue(dateStartSelect);
            dateStartSelect = errorCheck.FixMonthValueOfDate(dateStartSelect);
            dateEndSelect = errorCheck.FixDateStringValue(dateEndSelect);
            dateEndSelect = errorCheck.FixMonthValueOfDate(dateEndSelect);

            List<RecordEntry> recordEntries = dbWork.GetRecordsUsingTwoDates(dateStartSelect,dateEndSelect);

            if(recordEntries.Count == 0) 
            {
                ViewBag.Result = $"No records found between {dateStartSelect} - {dateEndSelect}.";
            }
            else
            {
                reporting.GenerateWeeklyReport(recordEntries, dateStartSelect, dateEndSelect);
                ViewBag.Result = "Report Written. Check the WeeklyReports folder for the text report.";
            }
            return View();
        }

        public IActionResult GenerateIndividualReport() 
        {
            // we only want non special case data
            DatabaseWork dbWork = new DatabaseWork();
            List<ChurchMember> churchMembersList = dbWork.GetAllChurchMembers();
            // delete special cases from the list
            for (int i = 0; i < churchMembersList.Count; i++)
            {
                if (churchMembersList[i].FirstName == "General" && churchMembersList[i].LastName == "Offering")
                {
                    churchMembersList.RemoveAt(i);
                }
                if (churchMembersList[i].FirstName == "Sunday" && churchMembersList[i].LastName == "School")
                {
                    churchMembersList.RemoveAt(i);
                }
                if (churchMembersList[i].FirstName == "Mission" && churchMembersList[i].LastName == "March")
                {
                    churchMembersList.RemoveAt(i);
                }
                if (churchMembersList[i].FirstName == "Chicken" && churchMembersList[i].LastName == "Barbecue")
                {
                    churchMembersList.RemoveAt(i);
                }
                if (churchMembersList[i].FirstName == "Love" && churchMembersList[i].LastName == "Offering")
                {
                    churchMembersList.RemoveAt(i);
                }
                if (churchMembersList[i].FirstName == "Seed" && churchMembersList[i].LastName == "Offering")
                {
                    churchMembersList.RemoveAt(i);
                }
            }
            return View(churchMembersList);
        }

        [HttpPost]
        public IActionResult GenerateIndividualReport(string chosenMember, string startDate, string endDate)
        {
            ErrorChecking errorCheck = new ErrorChecking();
            DatabaseWork dbWork = new DatabaseWork();
            int memberId; 
            bool isNumber = int.TryParse(chosenMember, out memberId);
           
            if(!isNumber)
            {
                ViewBag.Result = "No member Id to query records";
            }
            else
            {
                ChurchMember selectedMember = dbWork.GetChurchMember(memberId);
                List<RecordEntry> records = dbWork.GetIndividualRecordsFromDBUsingNameAndDates(selectedMember, startDate, endDate);
                startDate = errorCheck.FixDateStringValue(startDate);
                endDate = errorCheck.FixDateStringValue(endDate);
                startDate = errorCheck.FixMonthValueOfDate(startDate);
                endDate = errorCheck.FixMonthValueOfDate(endDate);

                if (records.Count == 0)
                {
                    ViewBag.Result = $"There are no records within these two dates: {startDate} - {endDate}";
                }
                else
                {
                    Report reporting = new Report();
                    reporting.GenerateIndividualReport(selectedMember, records, startDate, endDate);
                    ViewBag.Result = "Report Created. Check the IndividualReport folder for the text report.";
                }
            }
            

            // reload the list on view
            List<ChurchMember> churchMembersList = dbWork.GetAllChurchMembers();
            // delete special cases from the list
            for (int i = 0; i < churchMembersList.Count; i++)
            {
                if (churchMembersList[i].FirstName == "General" && churchMembersList[i].LastName == "Offering")
                {
                    churchMembersList.RemoveAt(i);
                }
                if (churchMembersList[i].FirstName == "Sunday" && churchMembersList[i].LastName == "School")
                {
                    churchMembersList.RemoveAt(i);
                }
                if (churchMembersList[i].FirstName == "Mission" && churchMembersList[i].LastName == "March")
                {
                    churchMembersList.RemoveAt(i);
                }
                if (churchMembersList[i].FirstName == "Chicken" && churchMembersList[i].LastName == "Barbecue")
                {
                    churchMembersList.RemoveAt(i);
                }
                if (churchMembersList[i].FirstName == "Love" && churchMembersList[i].LastName == "Offering")
                {
                    churchMembersList.RemoveAt(i);
                }
                if (churchMembersList[i].FirstName == "Seed" && churchMembersList[i].LastName == "Offering")
                {
                    churchMembersList.RemoveAt(i);
                }
            }
            return View(churchMembersList);
        }
    }
}