using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using CustomerTrackingDL;
using CustomerTrackingDO;


namespace CustomerServiceBL
{
    public class CustomerDetails
    {
        private static CustomerDetails _self = new CustomerDetails();

        public static CustomerDetails Ins
        {
            get { return _self; }
            set { _self = value; }

        }



        

        public CustomerTasks GetCustomerDataForID(CustomerTasks CTObject, string mode)
        {
            return CustomerTrackingDL.CustomerDetails.Ins.GetCustomerDataForID(CTObject, mode);
        }

        public ContactsTask GetContactsDataForID(ContactsTask ContactsObject, string mode)
        {
            return CustomerTrackingDL.CustomerDetails.Ins.GetContactsDataForID(ContactsObject, mode);
        }

        

        public DataTable GetAllCustomerData()
        {
            return CustomerTrackingDL.CustomerDetails.Ins.GetAllCustomerData();
        }
        #region Validations
        public Object ValidateDCUpdate(CustomerTasks CTObject)
        {
            Object ReturnValue = "";
            try
            {
                if (CTObject.Name == null || CTObject.Name == "")
                {
                    return "ER#Please enter name.";
                }
                
               
            }
            catch (Exception ex)
            { throw ex; }
            finally
            { }
            return "";
        }

        #endregion
        public bool UpdateCustomerData(CustomerTasks CTObject)
        {
            return CustomerTrackingDL.CustomerDetails.Ins.UpdateCustomerData(CTObject);
        }

        //public object UpdateCustomerData(CustomerTasks CTObject)
        //{
        //    Object ReturnValue = ValidateDCUpdate(CTObject);
        //    if (ReturnValue.ToString() == "")
        //    {
        //        return CustomerTrackingDL.CustomerDetails.Ins.UpdateCustomerData(CTObject);
        //    }
        //    else return ReturnValue;

        //}

        public bool UpdateContactsData(ContactsTask CTObject, string Mode, string CustId)
        {
            return CustomerTrackingDL.CustomerDetails.Ins.UpdateContactsData(CTObject, Mode, CustId);
        }

        public bool UpdateNotesData(NotesTask NotesObject, string Mode, string CustId)
        {
            return CustomerTrackingDL.CustomerDetails.Ins.UpdateNotesData(NotesObject, Mode, CustId);
        }


        public bool UpdateTicketsData(TicketsTask TicketsObject, string Mode, string CustId)
        {
            return CustomerTrackingDL.CustomerDetails.Ins.UpdateTicketsData(TicketsObject, Mode, CustId);
        }

        public bool UpdateVersionInfo(VersionInfoTask VersionObject, string Mode, string CustId)
        {
            return CustomerTrackingDL.CustomerDetails.Ins.UpdateVersionInfo(VersionObject, Mode, CustId);
        }

        public object DeleteData(CustomerTasks CustomerObject)
        {
            return CustomerTrackingDL.CustomerDetails.Ins.DeleteCustomerData(CustomerObject);
        }

        public DataTable GetAllContactsData()
        {
            return CustomerTrackingDL.CustomerDetails.Ins.GetAllContactsData();
        }

        public DataTable GetAllNotesData()
        {
            return CustomerTrackingDL.CustomerDetails.Ins.GetAllNotesData();
        }

        public DataTable GetAllTicketsData()
        {
            return CustomerTrackingDL.CustomerDetails.Ins.GetAllTicketsData();
        }

        public DataTable GetAllVersionInfoData()
        {
            return CustomerTrackingDL.CustomerDetails.Ins.GetAllVersionInfoData();
        }


        //public DataTable GetName(CustomerTasks NDObj)
        //{
        //    return CustomerTrackingDL.CustomerDetails.Ins.GetName(NDObj);
        //}




        public DataTable GetStatusType(CustomerTasks CustomerObject, string name)
        {
            return CustomerTrackingDL.CustomerDetails.Ins.GetCAddress(CustomerObject, name);
        }

        public DataTable GetCustomerContactDetailsBySearch(string ID,string Name, string PhoneNumber, string Title)
        {
            return CustomerTrackingDL.CustomerDetails.Ins.GetCustomerContactDetailsBySearch(ID,Name,PhoneNumber,Title);
        }

        public DataTable GetCustomerTicketsDetailsBySearch(string ID, string TicketNumber, string Status)
        {
            return CustomerTrackingDL.CustomerDetails.Ins.GetCustomerTicketsDetailsBySearch(ID, TicketNumber, Status);
        }

        public object DeleteContactsData(CustomerTasks ContactsObject)
        {
            return CustomerTrackingDL.CustomerDetails.Ins.DeleteContactsData(ContactsObject);
        }

        public object DeleteNotesData(CustomerTasks NotesObject)
        {
            return CustomerTrackingDL.CustomerDetails.Ins.DeleteNotesData(NotesObject);
        }

        public DataTable GetCustomerNotesDetailsBySearch(string ID)
        {
            return CustomerTrackingDL.CustomerDetails.Ins.GetCustomerNotesDetailsBySearch(ID);
        }

        public DataTable GetCustomerVersionInfoDetailsBySearch(string ID, string VersionNumber)
        {
            return CustomerTrackingDL.CustomerDetails.Ins.GetCustomerVersionInfoDetailsBySearch(ID, VersionNumber);
        }

        public object DeleteVersionInfoData(CustomerTasks VersionInfoObject)
        {
            return CustomerTrackingDL.CustomerDetails.Ins.DeleteVersionInfoData(VersionInfoObject);
        }

        public NotesTask GetNotesDataForID(NotesTask NotesObject, string Mode)
        {
            return CustomerTrackingDL.CustomerDetails.Ins.GetNotesDataForID(NotesObject, Mode);
        }

        public TicketsTask GetTicketsDataForID(TicketsTask TicketsObject, string Mode)
        {
            return CustomerTrackingDL.CustomerDetails.Ins.GetTicketsDataForID(TicketsObject, Mode);
        }

        public VersionInfoTask GetVersionInfoDataForID(VersionInfoTask VersioInfObject, string Mode)
        {
            return CustomerTrackingDL.CustomerDetails.Ins.GetVersionInfoDataForID(VersioInfObject, Mode);
        }

        public object DeleteTicketsData(CustomerTasks TicketsObject)
        {
            throw new NotImplementedException();
        }
    }

}
