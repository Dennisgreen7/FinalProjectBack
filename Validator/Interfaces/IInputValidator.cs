namespace Validator.Interfaces
{
    public interface IInputValidator
    {
        string AuthorValidation(params string[] prop);
        string BookValidation(params string[] prop);
        string BorrowValidation(params string[] prop);
        string DateValidation(DateTime value);
        bool EmptyFields(params string[] list);
        string GnereValidation(params string[] prop);
        string InputValidation(string inputValue, string pattern, string errMsg);
        bool IsDate(string value);
        string RoleVlidation(string value);
        string UserLenghtVlidation(string value, string check);
        string UserTaken(string userName, string operation, int id = 0);
        string UserValidation(string operation, params string[] prop);
        string YearValidation(string year);
    }
}