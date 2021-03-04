using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Text;

namespace ADWPF
{
    public class ADStream
    {
        DirectoryEntry de;
        /*
            * encapsulates a node or object in the Active Directory Domain Services hierarchy.
            * Use this class for binding to objects, reading properties, and updating attributes. 
            * Together with helper classes, DirectoryEntry provides support for life-cycle management and navigation methods, including creating, deleting, renaming, moving a child node, and enumerating children.
            */
        //DirectoryEntry entry = new DirectoryEntry();

        /*
            * 
            */
        //DirectorySearcher searcher = new DirectorySearcher();
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


        // 192.168.132.71
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

        public string GetUser(string username)
        {
            SearchResult result;
            DirectorySearcher ds;

            ds = new DirectorySearcher(de);
            ds.Filter = "(&(objectCategory=User)(objectClass=person))"; // TODO

            result = ds.FindOne();


            return result.Properties["name"][0].ToString();
        }
    }
}
