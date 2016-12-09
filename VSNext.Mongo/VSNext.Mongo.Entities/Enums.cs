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
            [LicenseCost(1)]
            Domino,
            [LicenseCost(1)]
            [Description("BES")]
            BES,
            [LicenseCost(1)]
            [Description("Sametime")]
            Sametime,
            [LicenseCost(0.5)]
            [BaseServerType("Microsoft")]
            [Description("SharePoint")]
            SharePoint,
            [LicenseCost(0.5)]
            [BaseServerType("Microsoft")]
            [Description("Exchange")]
            Exchange,
            [LicenseCost(0)]
            [Description("Mail")]
            Mail,
            [LicenseCost(0)]
            [Description("URL")]
            URL,
            [LicenseCost(0)]
            [Description("Network Device")]
            NetworkDevice,
            [LicenseCost(0)]
            [Description("Notes Database")]
            NotesDatabase,
            [LicenseCost(0)]
            [Description("General")]
            General,
            [LicenseCost(0)]
            [Description("Mobile Users")]
            MobileUsers,
            [LicenseCost(0)]
            [Description("Domino Cluster Database")]
            DominoClusterDatabase,
            [LicenseCost(0)]
            [Description("NotesMail Probe")]
            NotesMailProbe,
            [LicenseCost(0)]
            [Description("Exchange Mail Flow")]
            ExchangeMailFlow,
            [LicenseCost(0.33)]
            [BaseServerType("Microsoft")]
            [Description("Skype For Business")]
            SkypeForBusiness,
            [LicenseCost(0.33)]
            [BaseServerType("Microsoft")]
            [Description("Windows")]
            Windows,
            [LicenseCost(0.16)]
            [Description("Cloud")]
            Cloud,
            [LicenseCost(0.16)]
            [BaseServerType("Microsoft")]
            [Description("Active Directory")]
            ActiveDirectory,
            [LicenseCost(1)]
            [BaseServerType("Microsoft")]
            [Description("Database Availability Group")]
            DatabaseAvailabilityGroup,
            [LicenseCost(0.13)]
            [Description("SNMP Devices")]
            SNMPDevices,
            [LicenseCost(1)]
            [Description("Office365")]
            Office365,
            [LicenseCost(1.33)]
            [BaseServerType("WebSphere")]
            [Description("WebSphere")]
            WebSphere,
            [LicenseCost(0)]
            [Description("Network Latency")]
            NetworkLatency,
            [LicenseCost(0)]
            [Description("Notes Database Replica")]
            NotesDatabaseReplica,
            [LicenseCost(0)]
            [Description("ExchangeMail Probe")]
            ExchangeMailProbe,
            [LicenseCost(1)]
            [Description("IBM Connections")]
            IBMConnections,
            [LicenseCost(1)]
            [BaseServerType("WebSphere")]
            [Description("WebSphereNode")]
            WebSphereNode,
            [LicenseCost(0)]
            [BaseServerType("WebSphere")]
            [Description("WebSphereCell")]
            WebSphereCell,
            [LicenseCost(0)]
            [BaseServerType("Domino")]
            [Description("Domino Log Scanning")]
            DominoLogScanning,
            [LicenseCost(1)]
            [BaseServerType("Domino")]
            [Description("Traveler")]
            Traveler,
            [LicenseCost(0)]
            [BaseServerType("Domino")]
            [Description("Traveler HA Datastore")]
            TravelerHaDatastore,
            [LicenseCost(0)]
            [BaseServerType("Domino")]
            [Description("EXJournal")]
            EXJournal,
            [LicenseCost(0)]
            [BaseServerType("Domino")]
            [Description("Domino Custom Statistic")]
            DominoCustomStatistic
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

        public static double getLicenseCost(this Enum value)
        {
            var attribute = value.GetAttribute<LicenseCost>();
            return attribute == null ? 0 : attribute.Cost;
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

    /// <summary>
    /// Attribute used to set the cost
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = true)]
    public class LicenseCost : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the base server typr field attribute with the desired name.
        /// </summary>
        /// <param name="value">Name of the base server type.</param>
        public LicenseCost(double value)
        {
            Cost = value;
        }

        /// <summary>
        /// Gets the name of the base server type.
        /// </summary>
        /// <value>The name of the base server type.</value>
        public virtual double Cost { get; private set; }
    }


}
