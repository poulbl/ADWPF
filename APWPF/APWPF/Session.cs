using System;
using System.Collections.Generic;
using System.Text;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;

namespace ADWPF
{
    public static class Session
    {
        private static bool bLoggedIn = false;

        public static bool Login(string username, string password)
        {
            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, "192.168.132.71")) // miniput.local //IP ? 
            {
                // validate the credentials
                bool isValid = pc.ValidateCredentials(username, password);
                bLoggedIn = isValid;
                return isValid;
            }

            return false;
        }

        public static bool GetbLoggedIn()
        {
            return bLoggedIn;
        }
    }
}
