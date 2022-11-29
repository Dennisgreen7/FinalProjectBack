using Models.Models;
using System.Text.RegularExpressions;
using Validator.Interfaces;

namespace Validator.Implementations
{
    public class InputValidator : IInputValidator
    {
        private readonly LibraryContext _context;

        public InputValidator(LibraryContext context)
        {
            _context = context;
        }

        public string InputValidation(string inputValue, string pattern, string errMsg)
        {
            try
            {
                if (!Regex.Match(inputValue, pattern).Success)
                {
                    return errMsg;
                }
                return "";
            }
            catch
            {
                return "Eror";
            }
        }
        public bool EmptyFields(params string[] list)
        {
            try
            {
                for (int i = 0; i < list.Length; i++)
                {
                    if (list[i] == "")
                    {
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                return true;
            }
        }
        public string AuthorValidation(params string[] prop)
        {
            try
            {
                if (EmptyFields(prop))
                {
                    return "Please fill in all fields";
                }
                string text = "";
                string namelValid = InputValidation(prop[0], @"^[A-Z][A-Za-z\s]*$", "Name Must start with Upper Case");
                if (namelValid != "")
                    text += namelValid + "\n";
                string countrylValid = InputValidation(prop[1], @"^[A-Z][A-Za-z\s]*$", "Country Must start with Upper Case");
                if (countrylValid != "")
                    text += countrylValid + "\n";
                return text;
            }
            catch
            {
                return "Eror";
            }
        }

        public string GnereValidation(params string[] prop)
        {
            try
            {
                if (EmptyFields(prop))
                {
                    return "Please fill in all fields";
                }
                string text = "";
                string generelValid = InputValidation(prop[0], @"(^[A-Z][a-zA-Z\-]+)", "Genre Must start with Upper Case");
                if (generelValid != "")
                    text += generelValid;
                return text;
            }
            catch
            {
                return "Eror";
            }
        }

        public string YearValidation(string year)
        {
            try
            {
                var num = 0;
                var isNumeric = int.TryParse(year, out num);

                if (!isNumeric)
                {
                    return "Year should be digit";
                }
                else if (num < 1900 || num > DateTime.Now.Year)
                {
                    return "Eror, Year Range Must Be 1900 - " + DateTime.Now.Year;
                }
                return "";
            }
            catch
            {
                return "Eror";
            }
        }

        public string BookValidation(params string[] prop)
        {
            try
            {
                if (EmptyFields(prop))
                {
                    return "Please fill in all fields";
                }
                string text = "";
                string bookNameValid = InputValidation(prop[0], @"([a-zA-Z0-9\-]+)", "Book Name Must start with Upper Case");
                if (bookNameValid != "")
                    text += bookNameValid + "\n";
                string langaugeValid = InputValidation(prop[1], "^[A-Z][a-zA-Z]*$", "Langauge Must start with Upper Case");
                if (langaugeValid != "")
                    text += langaugeValid + "\n";
                string bookPagesValid = InputValidation(prop[2], "([1-9]+)", "Pages must be above 0");
                if (bookPagesValid != "")
                    text += bookPagesValid + "\n";
                string bookCopysValid = InputValidation(prop[3], "([1-9]+)", "Copys must be above 0");
                if (bookCopysValid != "")
                    text += bookCopysValid + "\n";
                string bookAuthorValid = InputValidation(prop[4], "([1-9]+)", "Select Author");
                if (bookAuthorValid != "")
                    text += bookAuthorValid + "\n";
                string bookGenreValid = InputValidation(prop[5], "([1-9]+)", "Select Genre");
                if (bookGenreValid != "")
                    text += bookGenreValid + "\n";
                string bookYearValid = YearValidation(prop[6]);
                if (bookYearValid != "")
                    text += bookYearValid + "\n";
                return text;
            }
            catch
            {
                return "Eror";
            }
        }

        public string UserTaken(string userName, string operation, int id = 0)
        {
            try
            {
                var user = _context.Users.Where(u => u.UsersUserName == userName).FirstOrDefault();
                if (operation == "Registration" || operation == "Add")
                {
                    if (user != null)
                    {
                        return "UserName Taken";
                    }
                }
                else
                {
                    if (user != null)
                    {
                        if (user.UsersId != id)
                        {
                            return "UserName Taken";
                        }
                    }
                }

                return "";
            }
            catch
            {
                return "Eror";
            }
        }
        public string RoleVlidation(string value)
        {
            if (value != "Admin" && value != "Client")
            {
                return "Role is required.";
            }
            return "";
        }
        public string UserLenghtVlidation(string value, string check)
        {
            try
            {
                switch (check)
                {
                    case "uname":
                        if (value.Length > 11 || value.Length < 5)
                        {
                            return "Username Length Min 5 and Maximum 11 character " + "\n";
                        }
                        break;
                    case "fname":
                        if (InputValidation(value, "^[A-Z][a-z]*$", "First Name must have only string with Upper Case first char ") != "")
                        {
                            return "First Name must have only string with Upper Case first char " + "\n";
                        }
                        else if (value.Length > 13)
                        {
                            return "First Name Max Length is 13 " + "\n";
                        }
                        break;
                    case "lname":
                        if (InputValidation(value, "^[A-Z][a-z]*$", "Last Name must have only string with Upper Case first char") != "")
                        {
                            return "Last Name must have only string with Upper Case first char " + "\n";
                        }

                        else if (value.Length > 13)
                        {
                            return "Last Name Max Length is 13 ";
                        }
                        break;
                }
                return "";
            }
            catch
            {
                return "Eror";
            }
        }
        public string UserValidation(string operation, params string[] prop)
        {
            try
            {
                var err = "";
                err += UserLenghtVlidation(prop[0], "uname");
                err += UserLenghtVlidation(prop[1], "fname");
                err += UserLenghtVlidation(prop[2], "lname");
                err += RoleVlidation(prop[3]);
                err += InputValidation(prop[4], @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", " Invalid Email");

                if (operation == "Registration")
                {
                    err += InputValidation(prop[5], "^(?=.{8,16}$)(?=.*?[a-z])(?=.*?[A-Z])(?=.*?[0-9]).*$", "Password Length 8 and Maximum 16 character,Require at least one(Lower Case,Upper Case,Digit)");
                }

                if (err != "")
                {
                    return err;
                }

                return "";
            }
            catch
            {
                return "Eror";
            }
        }
        public bool IsDate(string value)
        {
            try
            {
                return DateTime.TryParse(value, out _);
            }
            catch
            {
                return false;
            }
        }

        public string DateValidation(DateTime value)
        {
            try
            {
                if (!IsDate(value.ToString()))
                {
                    return "Eror, invalid Return Date";
                }
                return "";
            }
            catch
            {
                return "Eror";
            }
        }

        public string BorrowValidation(params string[] prop)
        {
            try
            {
                var returnDate = DateTime.Parse(prop[0]);
                string err = "";
                string dateValid = DateValidation(returnDate);
                if (dateValid != "")
                    err += dateValid + "\n";
                string borrowUserValid = InputValidation(prop[1], "([1-9]+)", "Select User");
                if (borrowUserValid != "")
                    err += borrowUserValid + "\n";
                string borrowBookValid = InputValidation(prop[2], "([1-9]+)", "Select Book");
                if (borrowBookValid != "")
                    err += borrowBookValid;
                return err;
            }
            catch
            {
                return "Eror";
            }
        }

    }
}
