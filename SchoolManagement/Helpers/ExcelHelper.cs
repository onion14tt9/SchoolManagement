using OfficeOpenXml;
using SchoolManagement.Domain.Dtos;

namespace SchoolManagement.Helpers
{
    public class ExcelHelper
    {
        public void GenerateExcelFile(List<UserProfileDto> data, string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath); // delete and override file
            }
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                worksheet.Cells[1, 1].Value = "Username";
                worksheet.Cells[1, 2].Value = "Password";
                worksheet.Cells[1, 3].Value = "Name";
                worksheet.Cells[1, 4].Value = "Email";
                worksheet.Cells[1, 5].Value = "PhoneNumber";
                worksheet.Cells[1, 6].Value = "DateOfBirth";
                worksheet.Cells[1, 7].Value = "Address";
                worksheet.Cells[1, 8].Value = "Role";

                for (int i = 0; i < data.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = data[i].Username;
                    worksheet.Cells[i + 2, 2].Value = data[i].Password;
                    worksheet.Cells[i + 2, 3].Value = data[i].Name;
                    worksheet.Cells[i + 2, 4].Value = data[i].Email;
                    worksheet.Cells[i + 2, 5].Value = data[i].PhoneNumber;
                    worksheet.Cells[i + 2, 6].Value = data[i].DateOfBirth.ToString("dd/MM/yyyy");
                    worksheet.Cells[i + 2, 7].Value = data[i].Address;
                    worksheet.Cells[i + 2, 8].Value = data[i].Role.ToString();
                    // ...
                }

                // Save file Excel
                FileInfo file = new FileInfo(filePath);
                package.SaveAs(file);
            }
        }
        public void GenerateUserListToExcelFile(List<UserProfileDto> data)
        {
            string filePath = "D:\\.NET demo\\SchoolManagement\\Excel\\Users.xlsx";

            GenerateExcelFile(data, filePath);
        }

        public void ParsingWeeklyScheduleToExcelFile(List<WeeklyScheduleDto> data, string filePath)
        {
            Dictionary<int, string> weekdayMap = new Dictionary<int, string>
            {
                { 0, "Sunday" },
                { 1, "Monday" },
                { 2, "Tuesday" },
                { 3, "Wednesday" },
                { 4, "Thursday" },
                { 5, "Friday" },
                { 6, "Saturday" }
            };
            if (File.Exists(filePath))
            {
                File.Delete(filePath); // delete and override file
            }

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                worksheet.Cells[1, 1].Value = "Class ID";
                worksheet.Cells[1, 2].Value = "Weekday";
                worksheet.Cells[1, 3].Value = "Schedule Date";
                worksheet.Cells[1, 4].Value = "Class Name";
                worksheet.Cells[1, 5].Value = "Slot";

                for (int i = 0; i < data.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = data[i].ClassId;
                    worksheet.Cells[i + 2, 2].Value = weekdayMap[data[i].WeekDay];
                    worksheet.Cells[i + 2, 3].Value = data[i].ScheduleDate.ToString("dd/MM/yyyy");
                    worksheet.Cells[i + 2, 4].Value = data[i].ClassName;
                    worksheet.Cells[i + 2, 5].Value = data[i].SlotId;
                    // ...
                }

                // Save file Excel
                FileInfo file = new FileInfo(filePath);
                package.SaveAs(file);
            }
        }
        public void GenerateWeeklyScheduleToExcelFile(List<WeeklyScheduleDto> data)
        {
            string filePath = "D:\\.NET demo\\SchoolManagement\\Excel\\WeeklySchedule.xlsx";

            ParsingWeeklyScheduleToExcelFile(data, filePath);

        }
    }
}
