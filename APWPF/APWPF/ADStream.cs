using System;
using System.Collections.Generic;
using System.DirectoryServices;

namespace ADWPF
{
    public class ADStream
    {
        DirectoryEntry de;

        public ADStream(string ip, string user, string password)
        {
            ip = "LDAP://" + ip;
            de = new DirectoryEntry(ip, user, password);
        }

        public ADStream(string ip)
        {
            ip = "LDAP://" + ip;
            de = new DirectoryEntry(ip);
        }

        public ADStream()
        {
            // Tom med vilje
        }

        public string GetUser()
        {
            throw new NotImplementedException();
        }

        // gets the information to create ListBox data in MainWindow
        public SearchResultCollection GetUsers(string name)
        {
            SearchResultCollection result;
            DirectorySearcher ds;

            ds = new DirectorySearcher(de);
            ds.Filter = $"(&(objectCategory=User)(objectClass=person)(name={name}*))";

            result = ds.FindAll();

            return result;
        }

        public List<string> GetAllUsers()
        {
            List<string> listToReturn = new List<string>();
            SearchResultCollection results;
            DirectorySearcher ds = null;

            ds = new DirectorySearcher(de);
            ds.Filter = "(&(objectCategory=User)(objectClass=person))";
            results = ds.FindAll();

            foreach (SearchResult sr in results)
            {
                listToReturn.Add(sr.Properties["name"][0].ToString());
            }
            return listToReturn;
        }

        public Dictionary<string, List<string>> GetAllUsersGroups()
        {
            SearchResultCollection results;
            DirectorySearcher ds;
            Dictionary<string, List<string>> usersGroups = new Dictionary<string, List<string>>();

            ds = new DirectorySearcher(de);
            ds.Filter = "(&(objectCategory=User)(objectClass=person))";

            results = ds.FindAll();

            foreach (SearchResult sr in results)
            {
                // get user for key
                string user = sr.Properties["name"][0].ToString();
                List<string> groups = new List<string>();

                // get groups for value
                for (int i = 0; i < sr.Properties["memberof"].Count; i++)
                {
                    var memberProps = sr.Properties["memberof"][i].ToString();
                    string[] memberPropsSplit = memberProps.Split(',');
                    groups.Add(memberPropsSplit[0]);
                }
                usersGroups.Add(user, groups);
            }
            return usersGroups;
        }

        //public string GetUser(string username)
        //{
        //    SearchResult result;
        //    DirectorySearcher ds;

        //    ds = new DirectorySearcher(de);
        //    ds.Filter = "(&(objectCategory=User)(objectClass=person))";
        //    result = ds.FindOne();

        //    return result.Properties["name"][0].ToString();
        //}

        public Dictionary<string, List<string>> GetAllData(string username)
        {
            Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();

            try
            {
                SearchResult result;
                DirectorySearcher ds;
                ds = new DirectorySearcher(de);
                ds.Filter = "(cn=" + username + ")";

                result = ds.FindOne();
                List<string> values = new List<string>();

                if (result != null)
                {
                    // user exists, cycle through LDAP fields (cn, telephonenumber etc.)  
                    ResultPropertyCollection fields = result.Properties;

                    foreach (String ldapField in fields.PropertyNames)
                    {
                        // cycle through objects in each field e.g. group membership  
                        // (for many fields there will only be one object such as name)
                        foreach (Object myCollection in fields[ldapField])
                            values.Add(String.Format("{0,-20} : {1}",
                                          ldapField, myCollection.ToString()));
                    }
                }
                dict.Add(username, values);
                return dict;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<string, List<string>> GetSpecific(string username)
        {
            Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();

            try
            {
                SearchResult result;
                DirectorySearcher ds;
                ds = new DirectorySearcher(de);
                ds.Filter = "(name=" + username + ")";

                //Selected data groups
                string[] requiredProperties = new string[] { "cn", "memberof", "userprincipalname", "description", "whenchanged" };

                foreach (String property in requiredProperties)
                    ds.PropertiesToLoad.Add(property);

                result = ds.FindOne();

                List<string> values = new List<string>();

                if (result != null)
                {
                    foreach (String property in requiredProperties)
                        foreach (Object myCollection in result.Properties[property])

                            values.Add(String.Format("{0,-20} : {1}",
                                        property, myCollection.ToString()));
                }
                dict.Add(username, values);
                return dict;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public void UpdateUserData(string username, string newInput)
        {
            try
            {
                SearchResult result; DirectorySearcher ds;
                ds = new DirectorySearcher(de);
                ds.Filter = "(cn=" + username + ")";
                ds.PropertiesToLoad.Add("givenname");
                result = ds.FindOne();
                
                if (result != null)
                {                         
                    // create new object from search result
                    DirectoryEntry entryToUpdate = result.GetDirectoryEntry();

                    var currentGivenName = entryToUpdate.Properties["givenname"][0].ToString();
                    
                    entryToUpdate.Properties["givenname"].Value = newInput;
                    entryToUpdate.CommitChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
