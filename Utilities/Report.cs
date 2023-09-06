using FirstChurchOfGodTreasuryMVC.Models;

namespace FCOGTreasury.Utilities
{
    public class Report
    {
        public void GenerateWeeklyReport(List<RecordEntry> records, string startDate, string endDate)
        {
            string dateFile = DateTime.Now.ToString("MM/dd/yyyy");
            dateFile = dateFile.Replace('/', '-');
            string fileName = $"WeeklyReport_{dateFile}.txt";
            FileStream file = new FileStream("WeeklyReports\\"+fileName, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(file);

            decimal grandTotal = 0, titheTotal = 0, generalOffering = 0, missionMarch = 0,
               sundaySchool = 0, chickenBarbecue = 0, seedOffering = 0, loveOffering = 0,
               pastorDan = 0, pastorDimetrio = 0, losLirios = 0;

            sw.WriteLine("First Church of God Weekly Report of " + startDate + " - " + endDate + "\n");
            sw.WriteLine("Tithes:\n");
            foreach (var record in records)
            {
                if (record.JointAccount && record.Tithe > 0 && record.GaveBy == "Cash And Check")
                {
                    sw.WriteLine($"${record.Tithe.ToString("f2")} - {record.FirstName} & {record.JointFirstName} {record.LastName} - {record.GaveBy} - Check #{record.CheckNumber} - Date: {record.DateRecorded}");
                    titheTotal += (decimal)record.Tithe;
                }
                else if (!record.JointAccount && record.Tithe > 0 && record.GaveBy == "Cash And Check")
                {
                    sw.WriteLine($"${record.Tithe.ToString("f2")} - {record.FirstName} {record.LastName} - {record.GaveBy} - Check #{record.CheckNumber} - Date: {record.DateRecorded}");
                    titheTotal += (decimal)record.Tithe;
                }
                else if (record.JointAccount && record.Tithe > 0 && record.GaveBy == "Check")
                {
                    sw.WriteLine($"${record.Tithe.ToString("f2")} - {record.FirstName} & {record.JointFirstName} {record.LastName} - {record.GaveBy} - Check #{record.CheckNumber} - Date: {record.DateRecorded}");
                    titheTotal += (decimal) record.Tithe;
                }
                else if (!record.JointAccount && record.Tithe > 0 && record.GaveBy == "Check")
                {
                    sw.WriteLine($"${record.Tithe.ToString("f2")} - {record.FirstName} {record.LastName} - {record.GaveBy} - Check #{record.CheckNumber} - Date: {record.DateRecorded}");
                    titheTotal += (decimal) record.Tithe;
                }
                else if (record.JointAccount && record.Tithe > 0)
                {
                    sw.WriteLine($"${record.Tithe.ToString("f2")} - {record.FirstName} & {record.JointFirstName} {record.LastName} - {record.GaveBy} - Date: {record.DateRecorded}");
                    titheTotal += (decimal) record.Tithe;
                }
                else if (!record.JointAccount && record.Tithe > 0)
                {
                    sw.WriteLine($"${record.Tithe.ToString("f2")} - {record.FirstName} {record.LastName} - {record.GaveBy} - Date: {record.DateRecorded}");
                    titheTotal += (decimal) record.Tithe;
                }
            }

            sw.WriteLine("\nTithe Total: $" + titheTotal.ToString("f2"));

            foreach (var record in records)
            {
                switch (record.OfferType)
                {
                    case "General Offering":
                        {
                            generalOffering += (decimal) record.Offering;
                            break;
                        }
                    case "Sunday School":
                        {
                            sundaySchool += (decimal) record.Offering;
                            break;
                        }
                    case "Mission March":
                        {
                            missionMarch += (decimal) record.Offering;
                            break;
                        }
                    case "Chicken Barbecue":
                        {
                            chickenBarbecue += (decimal) record.Offering;
                            break;
                        }
                    case "Love Offering":
                        {
                            loveOffering += (decimal) record.Offering;
                            break;
                        }
                    case "Seed Offering":
                        {
                            seedOffering += (decimal) record.Offering;
                            break;
                        }
                    case "Pastor Dan":
                        {
                            pastorDan += (decimal) record.Offering;
                            break;
                        }
                    case "Pastor Dimetrio":
                        {
                            pastorDimetrio += (decimal) record.Offering;
                            break;
                        }
                    case "Los Lirios":
                        {
                            losLirios += (decimal) record.Offering;
                            break;
                        }
                    case "None":
                        {
                            break;
                        }
                }
            }

            foreach (var record in records)
            {
                switch (record.OfferType2)
                {
                    case "General Offering":
                        {
                            generalOffering += (decimal)record.Offering2;
                            break;
                        }
                    case "Sunday School":
                        {
                            sundaySchool += (decimal)record.Offering2;
                            break;
                        }
                    case "Mission March":
                        {
                            missionMarch += (decimal)record.Offering2;
                            break;
                        }
                    case "Chicken Barbecue":
                        {
                            chickenBarbecue += (decimal)record.Offering2;
                            break;
                        }
                    case "Love Offering":
                        {
                            loveOffering += (decimal)record.Offering2;
                            break;
                        }
                    case "Seed Offering":
                        {
                            seedOffering += (decimal)record.Offering2;
                            break;
                        }
                    case "Pastor Dan":
                        {
                            pastorDan += (decimal)record.Offering2;
                            break;
                        }
                    case "Pastor Dimetrio":
                        {
                            pastorDimetrio += (decimal)record.Offering2;
                            break;
                        }
                    case "Los Lirios":
                        {
                            losLirios += (decimal)record.Offering2;
                            break;
                        }
                    case "None":
                        {
                            break;
                        }
                }
            }

            sw.WriteLine("Offering: $" + generalOffering.ToString("f2"));
            if (sundaySchool > 0)
                sw.WriteLine("Sunday School: $" + sundaySchool.ToString("f2"));
            if (missionMarch > 0)
                sw.WriteLine("Mission March: $" + missionMarch.ToString("f2"));
            if (chickenBarbecue > 0)
                sw.WriteLine("Chicken Barbacue: $" + chickenBarbecue.ToString("f2"));
            if (loveOffering > 0)
                sw.WriteLine("Love Offering: $" + loveOffering.ToString("f2"));
            if (seedOffering > 0)
                sw.WriteLine("Seed Offering: $" + seedOffering.ToString("f2"));
            if (pastorDan > 0)
                sw.WriteLine("Pastor Dan: $" + pastorDan.ToString("f2"));
            if (pastorDimetrio > 0)
                sw.WriteLine("Pastor Dimetrio: $" + pastorDimetrio.ToString("f2"));
            if (losLirios > 0)
                sw.WriteLine("Los Lirios: $" + losLirios.ToString("f2"));

            grandTotal = titheTotal + generalOffering + sundaySchool + missionMarch 
                + loveOffering + seedOffering + chickenBarbecue + pastorDan + pastorDimetrio + losLirios;

            sw.WriteLine($"Grand Total for {startDate} to {endDate}: ${grandTotal.ToString("f2")}");
            sw.Flush();
            sw.Dispose();
            sw.Close();
        }

        public void GenerateIndividualReport(ChurchMember member, List<RecordEntry> records, string startDate, string endDate)
        {
            string dateFile = DateTime.Now.ToString("MM/dd/yyyy");
            dateFile = dateFile.Replace('/', '-');
            string fileName;
            decimal grandTotal = 0, titheTotal = 0, generalOffering = 0, missionMarch = 0,
               sundaySchool = 0, chickenBarbecue = 0, seedOffering = 0, loveOffering = 0,
               pastorDan = 0, pastorDimetrio = 0, losLirios = 0;

            if (member.JointAccount)
                fileName = $"{member.FirstName}_{member.JointFirstName}_{member.LastName}_{dateFile}.txt";
            else
                fileName = $"{member.FirstName}_{member.LastName}_{dateFile}.txt";

            FileStream file = new FileStream("IndividualReports\\" + fileName, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(file);

            if (member.JointAccount)
                sw.WriteLine($"Individual Report for {member.FirstName} & {member.JointFirstName} {member.LastName} from {startDate} to {endDate}\n");
            else
                sw.WriteLine($"Individual Report for {member.FirstName} {member.LastName} from {startDate} to {endDate}\n");

            sw.WriteLine("Tithes:");
            foreach (var record in records)
            {
                if(record.JointAccount && record.Tithe > 0 & record.GaveBy == "Cash And Check")
                {
                    sw.WriteLine($"${record.Tithe.ToString("f2")} - {record.GaveBy} - Check #{record.CheckNumber} - Date: {record.DateRecorded}");
                    titheTotal += (decimal)record.Tithe;
                }
                else if(!record.JointAccount && record.Tithe > 0 & record.GaveBy == "Cash And Check")
                {
                    sw.WriteLine($"${record.Tithe.ToString("f2")} - {record.GaveBy} - Check #{record.CheckNumber} - Date: {record.DateRecorded}");
                    titheTotal += (decimal)record.Tithe;
                }
                else if (record.JointAccount && record.Tithe > 0 && record.GaveBy == "Check")
                {
                    sw.WriteLine($"${record.Tithe.ToString("f2")} - {record.GaveBy} - Check #{record.CheckNumber} - Date: {record.DateRecorded}");
                    titheTotal += (decimal)record.Tithe;
                }
                else if (!record.JointAccount && record.Tithe > 0 && record.GaveBy == "Check")
                {
                    sw.WriteLine($"${record.Tithe.ToString("f2")} - {record.GaveBy} - Check #{record.CheckNumber} - Date: {record.DateRecorded}");
                    titheTotal += (decimal)record.Tithe;
                }
                else if (record.JointAccount && record.Tithe > 0)
                {
                    sw.WriteLine($"${record.Tithe.ToString("f2")} - {record.GaveBy} - Date: {record.DateRecorded}");
                    titheTotal += (decimal)record.Tithe;
                }
                else if (!record.JointAccount && record.Tithe > 0)
                {
                    sw.WriteLine($"${record.Tithe.ToString("f2")} - {record.GaveBy} - Date: {record.DateRecorded}");
                    titheTotal += (decimal)record.Tithe;
                }
            }
            sw.WriteLine("\nTithe Total: $" + titheTotal.ToString("f2"));

            foreach (var record in records)
            {
                switch (record.OfferType)
                {
                    case "General Offering":
                        {
                            generalOffering += (decimal) record.Offering;
                            break;
                        }
                    case "Sunday School":
                        {
                            sundaySchool += (decimal) record.Offering;
                            break;
                        }
                    case "Mission March":
                        {
                            missionMarch += (decimal) record.Offering;
                            break;
                        }
                    case "Chicken Barbecue":
                        {
                            chickenBarbecue += (decimal) record.Offering;
                            break;
                        }
                    case "Love Offering":
                        {
                            loveOffering += (decimal) record.Offering;
                            break;
                        }
                    case "Seed Offering":
                        {
                            seedOffering += (decimal) record.Offering;
                            break;
                        }
                    case "None":
                        {
                            break;
                        }
                }
            }

            foreach (var record in records)
            {
                switch (record.OfferType2)
                {
                    case "General Offering":
                        {
                            generalOffering += (decimal)record.Offering2;
                            break;
                        }
                    case "Sunday School":
                        {
                            sundaySchool += (decimal)record.Offering2;
                            break;
                        }
                    case "Mission March":
                        {
                            missionMarch += (decimal)record.Offering2;
                            break;
                        }
                    case "Chicken Barbecue":
                        {
                            chickenBarbecue += (decimal)record.Offering2;
                            break;
                        }
                    case "Love Offering":
                        {
                            loveOffering += (decimal)record.Offering2;
                            break;
                        }
                    case "Seed Offering":
                        {
                            seedOffering += (decimal)record.Offering2;
                            break;
                        }
                    case "None":
                        {
                            break;
                        }
                }
            }

            if (generalOffering > 0)
                sw.WriteLine("Offering: $" + generalOffering.ToString("f2"));
            if (sundaySchool > 0)
                sw.WriteLine("Sunday School: $" + sundaySchool.ToString("f2"));
            if (missionMarch > 0)
                sw.WriteLine("Mission March: $" + missionMarch.ToString("f2"));
            if (chickenBarbecue > 0)
                sw.WriteLine("Chicken Barbacue: $" + chickenBarbecue.ToString("f2"));
            if (loveOffering > 0)
                sw.WriteLine("Love Offering: $" + loveOffering.ToString("f2"));
            if (seedOffering > 0)
                sw.WriteLine("Seed Offering: $" + seedOffering.ToString("f2"));
            if (pastorDan > 0)
                sw.WriteLine("Pastor Dan: $" + pastorDan.ToString("f2"));
            if (pastorDimetrio > 0)
                sw.WriteLine("Pastor Dimetrio: $" + pastorDimetrio.ToString("f2"));
            if (losLirios > 0)
                sw.WriteLine("Los Lirios: $" + losLirios.ToString("f2"));

            grandTotal = titheTotal + generalOffering + sundaySchool + missionMarch
                + loveOffering + seedOffering + chickenBarbecue + pastorDan + pastorDimetrio + losLirios;

            if (member.JointAccount)
                sw.WriteLine($"Grand Total for {member.FirstName} & {member.JointFirstName} {member.LastName}: ${grandTotal.ToString("f2")}");
            else
                sw.WriteLine($"Grand Total for {member.FirstName} {member.LastName}: ${grandTotal.ToString("f2")}");

            sw.Flush();
            sw.Dispose();
            sw.Close();
        }
    }
}