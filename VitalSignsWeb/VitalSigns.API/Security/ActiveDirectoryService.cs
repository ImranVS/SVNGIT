using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VSNext.Mongo.Repository;
using VSNext.Mongo.Entities;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices;
using VitalSigns.API.Models;


namespace VitalSigns.API.Security
{
    public class ActiveDirectoryService
    {

        private static readonly IRepository<NameValue> nameValueRepository = new Repository<NameValue>(GetDBConnectionString());

        public static String GetDBConnectionString()
        {
            return Startup.ConnectionString + @"/" + Startup.DataBaseName;
        }

        public static bool CheckIfADAuthenticationIsEnabled()
        {            
            NameValue nameValue = nameValueRepository.Find(nv => nv.Name == "AD Enabled").SingleOrDefault();
            if(nameValue != null)
            {
                return Convert.ToBoolean(nameValue.Value);
            }
            
            return false;
        }

        public static bool ValidateAgainstAD(String userName, String password)
        {
            bool isValid = false;
            NameValue nameValue = nameValueRepository.Find(nv => nv.Name == "AD URL").SingleOrDefault();

            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, nameValue.Value))
            {               
                isValid = pc.ValidateCredentials(userName, password);  
            }
            return isValid;
        }

        public static IEnumerable<Profile> SearchProfilesForAdUser(string userName, String name)
        {
            IDictionary<String, NameValue> nameValueDictionary = nameValueRepository.Find(nv => nv.Name.StartsWith("AD")).ToDictionary(nv => nv.Name);
            IEnumerable<Profile> profiles = null;
            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, nameValueDictionary["AD URL"].Value,
                nameValueDictionary["AD Login ID"].Value,
                DecryptUsingTripleDES(nameValueDictionary["AD Password"].Value)))
            {
                if (userName != null || name != null)
                {

                    UserPrincipal up = new UserPrincipal(pc);
                    if (name != null)
                    {
                        up.Name = "*" + name + "*";
                    }
                    if (userName != null)
                    {
                        up.UserPrincipalName = "*" + userName + "*";
                    }

                    PrincipalSearcher ps = new PrincipalSearcher(up);
                    var userPrinciples = ps.FindAll().Cast<UserPrincipal>();
                    profiles = userPrinciples
                         .Select(x =>
                         {
                             DirectoryEntry directoryEntry = x.GetUnderlyingObject() as DirectoryEntry;
                             var whenCreated = directoryEntry.Properties["whenCreated"];
                             var whenChanged = directoryEntry.Properties["whenChanged"];

                             return new Profile
                             {
                                 FullName = x.Name,
                                 Email = x.UserPrincipalName,
                                 TenantId = 5,
                                 Roles = new List<String>(),
                                 Created = DateTime.Parse(whenCreated.Value.ToString()),
                                 Modified = DateTime.Parse(whenChanged.Value.ToString())

                             };

                         }).ToList();

                    profiles = profiles.GroupBy(profile => profile.Email).Select(group => group.First()).ToList();
                }
            }
            return profiles;
        }

        public static string EncryptUsingTripleDES(String s)
        {
            VSFramework.TripleDES tripleDes = new VSFramework.TripleDES();
            byte[] byteArray = tripleDes.Encrypt(s);
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            foreach (byte b in byteArray)
            {
                stringBuilder.AppendFormat("{0}, ", b);
            }
            String byteString = stringBuilder.ToString();
            int n = byteString.LastIndexOf(", ");
            byteString = byteString.Substring(0, n);
            return byteString;
        }

        public static string DecryptUsingTripleDES(string encryptedString)
        {
            VSFramework.TripleDES tripleDes = new VSFramework.TripleDES();
            string[] strarr = encryptedString.Split(',');
            byte[] byteArray = new byte[strarr.Length];
            for (int i = 0; i < strarr.Length; i++)
            {
                byteArray[i] = Convert.ToByte(strarr[i]);
            }

            String result = tripleDes.Decrypt(byteArray);

            return result;
        }


        public static Profile GetProfileForAdUser(string userName)
        {
            Profile profile = null;
            IDictionary<String, NameValue> nameValueDictionary = nameValueRepository.Find(nv => nv.Name.StartsWith("AD")).ToDictionary(nv => nv.Name);
            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, nameValueDictionary["AD URL"].Value,
                nameValueDictionary["AD Login ID"].Value, 
                DecryptUsingTripleDES( nameValueDictionary["AD Password"].Value)))
            {

                UserPrincipal user = UserPrincipal.FindByIdentity(pc, IdentityType.UserPrincipalName, userName);
                DirectoryEntry directoryEntry = user.GetUnderlyingObject() as DirectoryEntry;
                var whenCreated = directoryEntry.Properties["whenCreated"];
                var whenChanged = directoryEntry.Properties["whenChanged"];
                profile = new Profile()
                {
                    FullName = user.Name,
                    Email = user.UserPrincipalName,
                    TenantId = 5,
                    Roles = new List<String>(),
                    Created = DateTime.Parse(whenCreated.Value.ToString()),
                    Modified = DateTime.Parse(whenChanged.Value.ToString())
                };
            }
            return profile;
        }
    }



    
}
