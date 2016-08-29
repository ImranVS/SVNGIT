using System;
using System.ComponentModel;

namespace VSNext.Mongo.Entities
{
   public  class Enums
    {
        /// <summary>
        /// Server type enumerations
        /// </summary>
        public enum ServerType
        {
            [Description("Domino")]
            Domino,
            [Description("BES")]
            BES,
            [Description("Sametime")]
            Sametime,
            [BaseServerType("Microsoft")]
            [Description("SharePoint")]
            SharePoint,
            [BaseServerType("Microsoft")]
            [Description("Exchange")]
            Exchange,
            [Description("Mail")]
            Mail,
            [Description("URL")]
            URL,
            [Description("Network Device")]
            NetworkDevice,
            [Description("Notes Database")]
            NotesDatabase,
            [Description("General")]
            General,
            [Description("Mobile Users")]
            MobileUsers,
            [Description("Domino Cluster Database")]
            DominoClusterDatabase,
            [Description("NotesMail Probe")]
            NotesMailProbe,
            [Description("Exchange Mail Flow")]
            ExchangeMailFlow,
            [BaseServerType("Microsoft")]
            [Description("Skype For Business")]
            SkypeForBusiness,
            [BaseServerType("Microsoft")]
            [Description("Windows")]
            Windows,
            [Description("Cloud")]
            Cloud,
            [BaseServerType("Microsoft")]
            [Description("Active Directory")]
            ActiveDirectory,
            [BaseServerType("Microsoft")]
            [Description("Database Availability Group")]
            DatabaseAvailabilityGroup,
            [Description("SNMP Devices")]
            SNMPDevices,
            [Description("Office365")]
            Office365,
            [BaseServerType("WebSphere")]
            [Description("WebSphere")]
            WebSphere,
            [Description("Network Latency")]
            NetworkLatency,
            [Description("Domino Cluster")]
            DominoCluster,
            [Description("ExchangeMail Probe")]
            ExchangeMailProbe,
            [Description("IBM Connections")]
            IBMConnections,
            [BaseServerType("WebSphere")]
            [Description("WebSphereNode")]
            WebSphereNode,
            [BaseServerType("WebSphere")]
            [Description("WebSphereCell")]
            WebSphereCell,
            [BaseServerType("Domino")]
            [Description("Domino Log Scanning")]
            DominoLogScanning,
            [BaseServerType("Domino")]
            [Description("Traveler")]
            Traveler,
            [BaseServerType("Domino")]
            [Description("Traveler HA Datastore")]
            TravelerHaDatastore,
            [BaseServerType("Domino")]
            [Description("EXJournal")]
            EXJournal
        }

    }
    /// <summary>
    /// To implement the extension methods from Enum class
    /// </summary>
    public static class EnumExtensions
    {
        public static T GetAttribute<T>(this Enum value) where T : Attribute
        {
            var type = value.GetType();
            var memberInfo = type.GetMember(value.ToString());
            var attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);
            return (T)attributes[0];
        }
             
        public static string ToDescription(this Enum value)
        {
            var attribute = value.GetAttribute<DescriptionAttribute>();
            return attribute == null ? value.ToString() : attribute.Description;
        }

        /// <summary>
        /// Returns the base server type for  server type
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetBaseServerType(this Enum value)
        {
            var attribute = value.GetAttribute<BaseServerTypeAttribute>();
            return attribute == null ? string.Empty : attribute.Name;
        }


        
    }



    /// <summary>
    /// Attribute used to set the base server type for the servertype enum. By default, when this attribute
    /// is not specified, the empty string will be set.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = true)]
    public class BaseServerTypeAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the base server typr field attribute with the desired name.
        /// </summary>
        /// <param name="value">Name of the base server type.</param>
        public BaseServerTypeAttribute(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Empty Base server type is not allowed", "value");
            Name = value;
        }

        /// <summary>
        /// Gets the name of the base server type.
        /// </summary>
        /// <value>The name of the base server type.</value>
        public virtual string Name { get; private set; }
    }


}
