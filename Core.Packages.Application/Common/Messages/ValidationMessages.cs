using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Packages.Application.Common.Messages
{
    public static partial class Messages
    {
        public static string NotEmpty = "{PropertyName} is required.";
        public static string NotValidEmail = "Email is not valid";
        public static string PasswordsDoNotMatch = "Passwords do not match";
        public static string PasswordLength = "Password must be at least 6 characters.";
        public static string PasswordUppercase = "Password must contain at least one uppercase letter.";
        public static string PasswordDigit = "Password must contain at least one digit.";
        public static string PasswordSpecialCharacter = "Password must contain at least one special character.";

    }

}
