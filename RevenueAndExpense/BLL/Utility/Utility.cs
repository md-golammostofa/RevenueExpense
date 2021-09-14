using RevenueAndExpense.BO.DataObject;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace RevenueAndExpense.BLL.Utility
{
    public class Utility
    {
        public static readonly string SystemAdmin = "System Admin";
        public static readonly string Admin = "Admin";
        public static readonly string User = "User";
        public static readonly string Manager = "Manager";
        public static readonly string Operation = "Operation";
        public static readonly string Doctor = "Doctor";
        public static readonly string Maintenance = "Maintenance";
        public static string GetMonthName(int monthNo)
        {
            string monthName = string.Empty;
            switch (monthNo)
            {
                case 1:
                    monthName = "January";
                    break;
                case 2:
                    monthName = "February";
                    break;
                case 3:
                    monthName = "March";
                    break;
                case 4:
                    monthName = "April";
                    break;
                case 5:
                    monthName = "May";
                    break;
                case 6:
                    monthName = "June";
                    break;
                case 7:
                    monthName = "July";
                    break;
                case 8:
                    monthName = "Augest";
                    break;
                case 9:
                    monthName = "September";
                    break;
                case 10:
                    monthName = "October";
                    break;
                case 11:
                    monthName = "November";
                    break;
                case 12:
                    monthName = "December";
                    break;
                default:
                    monthName = "";
                    break;
            }
            return monthName;
        }
        public static string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        public static string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
        public static string GetImage(string URL)
        {
            string base64Img = string.Empty;
            if (System.IO.File.Exists(URL))
            {
                FileStream fs = new FileStream(URL, FileMode.Open, FileAccess.Read);

                BinaryReader br = new BinaryReader(fs);

                Byte[] bytes = br.ReadBytes((Int32)fs.Length);

                br.Close();
                string extension = Path.GetExtension(URL);

                var base64 = Convert.ToBase64String(bytes);
                base64Img = String.Format("data:image/{0};base64,{1}", extension, base64);
            }
            return base64Img;
        }
        public static string SaveImage(Stream stream, string fileName, string driverPath, int OrgId)
        {
            WebImage imgFile = new WebImage(stream);
            string file = fileName.Substring(0, (fileName.LastIndexOf(".")));
            string extension = imgFile.ImageFormat;
            file = file + OrgId.ToString().PadLeft(4, '0') + DateTime.Now.ToString("yymmssff");//+ extension;
            imgFile.Save(Path.Combine(string.Format(@"{0}", driverPath), file));
            return driverPath + file + "." + extension;
        }
        public static string SaveImage(Stream stream, string fileName, string driverPath, int OrgId, int width, int height)
        {
            WebImage imgFile = new WebImage(stream);
            imgFile.Resize(width, height, false, true);
            string file = fileName.Substring(0, (fileName.LastIndexOf(".")));
            string extension = imgFile.ImageFormat;
            file = file + OrgId.ToString().PadLeft(4, '0') + DateTime.Now.ToString("yymmssff");//+ extension;
            imgFile.Save(Path.Combine(string.Format(@"{0}", driverPath), file));
            return driverPath + file + "." + extension;
        }
        public static void DeleteImage(string ImagePath)
        {
            if (!string.IsNullOrEmpty(ImagePath))
            {
                if (File.Exists(ImagePath))
                {
                    File.Delete(ImagePath);
                }
            }
        }
        public static byte[] GetImageBytes(string imagePath)
        {
            byte[] imgByte = null;
            if (System.IO.File.Exists(imagePath))
            {
                FileStream fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read);

                BinaryReader br = new BinaryReader(fs);

                Byte[] bytes = br.ReadBytes((Int32)fs.Length);

                br.Close();
                imgByte = bytes;
            }
            return imgByte;
        }
        public static string ParamChecker(string param)
        {
            if (param.ToLower().Contains(";") || param.ToLower().Contains("delete") || param.ToLower().Contains("database") || param.ToLower().Contains("alter") || param.ToLower().Contains("truncate") || param.ToLower().Contains("column") || param.ToLower().Contains("drop"))
            {
                return param = "";
            }
            return param;
        }
        public static List<SelectListItem> GetMonthAndYearListBN(int previousMonth)
        {
            List<SelectListItem> dropdowns = new List<SelectListItem>();
            for (int i = 0; i <= previousMonth; i++)
            {
                SelectListItem dropdown = new SelectListItem();
                int diff = i > 0 ? -i : 0;
                var date = DateTime.Now.AddMonths(diff);
                var bnMonth = GetMonthNameBN(date.Month);
                var bnYear = ConvertEnNumToBnNum(date.Year.ToString());
                dropdown.Text = bnMonth + ","+bnYear;
                dropdown.Value = date.Date.ToString("dd-MMM-yyyy");
                dropdowns.Add(dropdown); 
            }
            return dropdowns;
        }

        public static List<SelectListItem> GetMonthAndYearListNextBN(int nextMonth)
        {
            List<SelectListItem> dropdowns = new List<SelectListItem>();
            for (int i = 1; i == nextMonth; i++)
            {
                SelectListItem dropdown = new SelectListItem();
                //int diff = i > 0 ? -i : 0;
                var date = DateTime.Now.AddMonths(i);
                var bnMonth = GetMonthNameBN(date.Month);
                var bnYear = ConvertEnNumToBnNum(date.Year.ToString());
                dropdown.Text = bnMonth + "," + bnYear;
                dropdown.Value = date.Date.ToString("dd-MMM-yyyy");
                dropdowns.Add(dropdown);
            }
            return dropdowns;
        }
        public static List<SelectListItem> GetMonthAndYearListEN(int previousMonth)
        {
            List<SelectListItem> dropdowns = new List<SelectListItem>();
            for (int i = 0; i <= previousMonth; i++)
            {
                SelectListItem dropdown = new SelectListItem();
                int diff = i > 0 ? -i : 0;
                var date = DateTime.Now.AddMonths(diff);
                var bnMonth = GetMonthName(date.Month);
                var bnYear = date.Year.ToString();
                dropdown.Text = bnMonth + "," + bnYear;
                dropdown.Value = date.Date.ToString("dd-MMM-yyyy");
                dropdowns.Add(dropdown);
            }
            return dropdowns;
        }

        public static int GetMonthDiff(DateTime startDate, DateTime endDate)
        {
            int monthNo = 0;
            startDate = startDate.Day == DateTime.DaysInMonth(startDate.Date.Year, startDate.Date.Month) ? startDate.AddDays(1) : startDate;
            endDate = endDate.Day == 1 ? endDate.AddDays(-1) : endDate;
            int monthsApart = (startDate > endDate) ? 0 : (12 * (startDate.Year - endDate.Year) + startDate.Month - endDate.Month);
            monthNo = Math.Abs(monthsApart) == 0 ? 0 : Math.Abs(monthsApart) + 1;
            return monthNo;
        }
        public static string GetMonthNameBN(int monthNo)
        {
            string monthName = string.Empty;
            switch (monthNo)
            {
                case 1:
                    monthName = "জানুয়ারি";
                    break;
                case 2:
                    monthName = "ফেব্রুয়ারি";
                    break;
                case 3:
                    monthName = "মার্চ";
                    break;
                case 4:
                    monthName = "এপ্রিল";
                    break;
                case 5:
                    monthName = "মে";
                    break;
                case 6:
                    monthName = "জুন";
                    break;
                case 7:
                    monthName = "জুলাই";
                    break;
                case 8:
                    monthName = "আগস্ট";
                    break;
                case 9:
                    monthName = "সেপ্টেম্বর";
                    break;
                case 10:
                    monthName = "অক্টোবর";
                    break;
                case 11:
                    monthName = "নভেম্বর";
                    break;
                case 12:
                    monthName = "ডিসেম্বর";
                    break;
                default:
                    monthName = "জানুয়ারি";
                    break;
            }
            return monthName;
        }
        public static string ConvertEnNumToBnNum(string number)
        {
            decimal checkval = 0;
            string returnVal = string.Empty;
            try
            {
                if (decimal.TryParse(number, out checkval))
                {
                    char[] val = number.ToArray();
                    foreach (var item in val)
                    {
                        returnVal += BanglaNumerical(item.ToString());
                    }
                }
                else
                {
                    returnVal = "0";
                }
            }
            catch (Exception ex)
            {

            }
            return returnVal;
        }
        public static string BanglaNumerical(string enNumerical)
        {
            string val = string.Empty;
            switch (enNumerical)
            {
                case "1":
                    val = "১";
                    break;
                case "2":
                    val = "২";
                    break;
                case "3":
                    val = "৩";
                    break;
                case "4":
                    val = "৪";
                    break;
                case "5":
                    val = "৫";
                    break;
                case "6":
                    val = "৬";
                    break;
                case "7":
                    val = "৭";
                    break;
                case "8":
                    val = "৮";
                    break;
                case "9":
                    val = "৯";
                    break;
                case "0":
                    val = "০";
                    break;
                case ".":
                    val = ".";
                    break;
            }
            return val;
        }

    }
}