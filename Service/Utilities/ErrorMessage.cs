﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Utilities
{
    public static class ErrorMessage
    {
        public static string IdNotExist = "ID_DOES_NOT_EXIST";
        public static string PhoneExist = "PHONE_EXISTED";
        public static string UserNameExist = "user_EXISTED";
        public static string UserNameDoNotExist = "user_DOES_NOT_EXISTED";
        public static string InvalidAccount = "user_OR_PASSWORD_INCORRECT";
        public static string RoleNameExist = "ROLE_NAME_EXISTED";
        public static string BannedAccount = "ACCOUNT_BANNED";
    }
}
