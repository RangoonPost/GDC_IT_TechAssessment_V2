//AUTHOR: Philip F. McCracken
//DATE: 2021 JUNE
//TARGETED .NET Framework 4.7
using System;
using System.Data; 
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualBasic.FileIO;

namespace CSVReadParse
{
    class Program
    {

        public static string UserName { get; }
        //public string[] good { get; set; }
        //public string[] bad { get; set; }

        static void Main(string[] args)
        {
            string inputString1;
            string csv_filePath;
            //string[] goodEmails;
            //string[] badEmails;
            //bool isValid;
            //var foo = new EmailAddressAttribute();
            //bool bar;
            int ig = 0;
            int ib = 0;
            //List<string> listGood = new List<string>();
            //List<string> listBad = new List<string>();


            // Ask the user for File Name
            Console.WriteLine("Enter the Full Name.csv of the CSV file stored on the DeskTop:");
            inputString1 = Console.ReadLine();

            //Find and Get CSV File by default in MY Documents or MY Desktop
            csv_filePath = @"C:\Users\" + Environment.UserName + "\\Desktop\\" + inputString1;

            //Check if the File is in spelled correctly and in the DESKTOP folder.
            if (File.Exists(csv_filePath))
            {

                using (TextFieldParser csvReader = new TextFieldParser(csv_filePath))
                {
                    csvReader.CommentTokens = new string[] { "#" };
                    csvReader.SetDelimiters(new string[] { "," });
                    csvReader.HasFieldsEnclosedInQuotes = true;

                    // Skip the row with the column names
                    csvReader.ReadLine();
                    int i = 0;
                    Console.WriteLine("  ");

                    while (!csvReader.EndOfData)
                    {
                        i++;
                        // Read current line fields, pointer moves to the next line.
                        string[] fields = csvReader.ReadFields();

                        //DEBUG-TESTING LINE
                        ////Console.WriteLine(i.ToString() + " : " + fields[0]);
                        
                        ////const string pattern = @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$";
                        //if (new EmailAddressAttribute().IsValid(fields[0])) 
                        //}

                        //ANOTHER CHECK and METHOD ---
                        if (Validator.EmailIsValid(fields[0]))
                        { Console.WriteLine(i.ToString() + " : " + fields[0] + "   Good Email"); ig++; }
                        else
                        { Console.WriteLine(i.ToString() + " : " + fields[0] + "   Bad Email"); ib++; }
                    }
                }


                //Console.WriteLine(listGood.Count);
                Console.WriteLine("  ");
                Console.WriteLine("  Good Emails : " + ig.ToString());
                //Console.WriteLine(listGood);
                Console.WriteLine("  ----------");
                Console.WriteLine("  Bad Emails : " + ib.ToString());
                //Console.WriteLine(listBad);

                Console.ReadKey();  //Wait, so information is displayed and readable

            }
            else
            {   Console.WriteLine("File was not found, please check the spelling and CSV extension");
                Console.ReadKey();  //Wait, so information is displayed and readable
                }

                //-----------------READLINE and then Validate -------------------------
                //OPTION: https: //docs.microsoft.com/en-us/troubleshoot/dotnet/csharp/match-pattern-regular-expression
                bool IsValidEmail(string email)
            {
                try
                {
                    var addr = new System.Net.Mail.MailAddress(email);                 
                    return addr.Address == email;
                    //return true;
                }
                catch
                {
                    return false;
                }
            }

            //REFs
            // https:  //docs.microsoft.com/en-us/dotnet/api/system.data.datacolumn?view=net-5.0
            // https:  //www.c-sharpcorner.com/blogs/read-csv-file-and-get-record-in-datatable-using-textfieldparser-in-c-sharp1
            // https:  //www.delftstack.com/howto/csharp/how-to-read-a-csv-file-and-store-its-values-into-an-array-in-csharp/

            //////////NOTES & SCRAPS ///-------------
        }

        public static class Validator
        {

            static Regex ValidEmailRegex = CreateValidEmailRegex();

            /// <summary>
            /// Taken from http: //haacked.com/archive/2007/08/21/i-knew-how-to-validate-an-email-address-until-i.aspx
            ///            https: //stackoverflow.com/questions/1365407/c-sharp-code-to-validate-email-address
            /// </summary>
            /// <returns></returns>
            private static Regex CreateValidEmailRegex()
            {
                string validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                    + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                    + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

                return new Regex(validEmailPattern, RegexOptions.IgnoreCase);
            }

            internal static bool EmailIsValid(string emailAddress)
            {
                bool isValid = ValidEmailRegex.IsMatch(emailAddress);
                return isValid;
            }
        }
    }
}
