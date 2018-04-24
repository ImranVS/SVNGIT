using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using CustomerTrackingDO;
using System.Web;



namespace CustomerTrackingDL
{
    public class CustomerDetails
    {
        private Adaptor objAdaptor = new Adaptor();
        private static CustomerDetails _self = new CustomerDetails();

        public static CustomerDetails Ins
        {
            get { return _self; }

        }



        //Get all customer data
        public DataTable GetAllCustomerData()
        {
            DataTable CustomerDataTable = new DataTable();
            CustomerTasks ReturnObject = new CustomerTasks();
            try
            {
                string SqlQuery = "Select * FROM Customer";
                CustomerDataTable = objAdaptor.FetchData(SqlQuery);
            }
            catch
            {
            }
            finally
            {
            }
            return CustomerDataTable;
        }

        public DataTable GetAllContactsData()
        {
            DataTable ContactsDataTable = new DataTable();
            ContactsTask ReturnObject = new ContactsTask();
            try
            {
                string SqlQuery = "Select * from Customer, Contacts";
                ContactsDataTable = objAdaptor.FetchData(SqlQuery);
            }
            catch (Exception ex)
            {
            }
            finally
            {
            }
            return ContactsDataTable;
        }

        public DataTable GetAllNotesData()
        {
            DataTable NotesDataTable = new DataTable();
            NotesTask ReturnObject = new NotesTask();
            try
            {
                string SqlQuery = "Select * from Customer, Notes";
                NotesDataTable = objAdaptor.FetchData(SqlQuery);
            }
            catch (Exception ex)
            {
            }
            finally
            {
            }
            return NotesDataTable;
        }

        public DataTable GetAllTicketsData()
        {
            DataTable NotesDataTable = new DataTable();
            TicketsTask ReturnObject = new TicketsTask();
            try
            {
                string SqlQuery = "Select * from Customer, Tickets";
                NotesDataTable = objAdaptor.FetchData(SqlQuery);
            }
            catch (Exception ex)
            {
            }
            finally
            {
            }
            return NotesDataTable;
        }

        public DataTable GetAllVersionInfoData()
        {
            DataTable NotesDataTable = new DataTable();
            VersionInfoTask ReturnObject = new VersionInfoTask();
            try
            {
                string SqlQuery = "Select * from Customer, VersionInfo";
                NotesDataTable = objAdaptor.FetchData(SqlQuery);
            }
            catch (Exception ex)
            {
            }
            finally
            {
            }
            return NotesDataTable;
        }

        //get all version/ ticket data missing

        //Not deleteling cutomer date instead changing the status to closed.
        public Object DeleteCustomerData(CustomerTasks CustomerObject)
        {
            Object Update;
            try
            {
                string SqlQuery = "Update Customer SET overallstatus ='closed' where id='" + CustomerObject.ID + "'";
                Update = objAdaptor.ExecuteNonQuery(SqlQuery);
            }
            catch
            {
                Update = false;
            }
            finally
            {
            }
            return Update;
        }

        //not using 
        public DataTable GetName(CustomerTasks NDObj)
        {
            DataTable NDTable = new DataTable();
            try
            {
                if (NDObj.ID == 0)
                {
                    string SqLQuery = "Select * from Customer where Name='" + NDObj.Name + "' ";
                    NDTable = objAdaptor.FetchData(SqLQuery);
                }
                else
                {
                    string sqlQuery = "Select * from Customer where Name='" + NDObj.Name + "' and ID<>" + NDObj.ID + " ";
                    NDTable = objAdaptor.FetchData(sqlQuery);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return NDTable;
        }


        public bool UpdateCustomerData(CustomerTasks CTObject)
        {
            bool Insert = false;
            try
            {
                string SQL = "Select * from Customer where name = '" + CTObject.Name + "'";
                int count = objAdaptor.ExecuteScalar(SQL);
                string SqlQuery = "";
                if (count > 0)
                {
                    SqlQuery = "Update Customer Set Status_Type='" + CTObject.Status_Type +
                    "', Address='" + CTObject.Address + "',ServerCount='" + CTObject.ServerCount + "',CompReplacement='" + CTObject.CompReplacement +
                    "', OverallStatus='" + CTObject.OverallStatus + "', NextFollowUpDate='" + CTObject.NextFollowUpDate +
                    "',LicExpDate='" + CTObject.LicExpDate + "',BudInfo='" + CTObject.BudInfo + "' where Name='" + CTObject.Name + "'";
                }
                else
                {
                    SqlQuery = "Insert into Customer(Name, Status_Type, Address, ServerCount, CompReplacement, OverallStatus, NextFollowUpDate," +
                                     "LicExpDate, BudInfo) values ('" + CTObject.Name + "', '" + CTObject.Status_Type + "', '" + CTObject.Address + "', '" + CTObject.ServerCount +
                                     "', '" + CTObject.CompReplacement + "', '" + CTObject.OverallStatus + "', '" + CTObject.NextFollowUpDate +
                                     "', '" + CTObject.LicExpDate + "', '" + CTObject.BudInfo + "')";
                }               
               
                Insert = objAdaptor.ExecuteNonQuery(SqlQuery);
            }
            catch
            {
                Insert = false;
            }
            finally
            {
            }
            return Insert;
        }

        public Object InsertContactsData(ContactsTask ContactsObject)
        {
            Object Insert = false;
            try
            {

                string SqlQuery = "Insert into Contacts(CustId,Name, PhoneNumber,Title," +
                                  "Details) values ("+ContactsObject.CustID + "'" + ContactsObject.ContactName + "','" + ContactsObject.PhoneNumber + "','" + ContactsObject.Title +
                                  "','" + ContactsObject.Details + "')";
                                  

                Insert = objAdaptor.ExecuteNonQuery(SqlQuery);
            }
            catch
            {
                Insert = false;
            }
            finally
            {
            }
            return Insert;
        }

        //insert n update notes/tckets/versioninfo data

        //public Object UpdateCustomerData(CustomerTasks CTObject)
        //{
        //    Object Update;
        //    try
        //    {
        //        string SqlQuery = "Update Customer Set Name='" + CTObject.Name + "',Status_Type='" + CTObject.Status_Type +
        //            "', Address='" + CTObject.Address + "',ServerCount='" + CTObject.ServerCount + "',CompReplacement='" + CTObject.CompReplacement +
        //            "', OverallStatus='" + CTObject.OverallStatus + "', NextFollowUpDate='" + CTObject.NextFollowUpDate +
        //            "',LicExpDate='" + CTObject.LicExpDate + "',BudInfo='" + CTObject.BudInfo + "";

        //        Update = objAdaptor.ExecuteNonQuery(SqlQuery);
        //    }
        //    catch
        //    {
        //        Update = false;
        //    }
        //    finally
        //    {
        //    }
        //    return Update;
        //}

        public bool UpdateContactsData(ContactsTask ContactsObject, string Mode, string CustId)
        {
            bool Update;
            try
            {
                string SqlQuery;
                if (Mode == "Update")
                {
                    SqlQuery = "Update Contacts Set Name='" + ContactsObject.ContactName + "', PhoneNumber='" + ContactsObject.PhoneNumber +
                        "', Title='" + ContactsObject.Title + "', Details='" + ContactsObject.Details + "' Where ID = " + ContactsObject.ID;
                }
                else
                {
                    SqlQuery = "Insert into Contacts Values(" + CustId + ",'" + ContactsObject.ContactName + "','" + ContactsObject.PhoneNumber +
                        "','" + ContactsObject.Title + "','" + ContactsObject.Details + "')";
                }

                Update = objAdaptor.ExecuteNonQuery(SqlQuery);
            }
            catch
            {
                Update = false;
            }
            finally
            {
            }
            return Update;
        }

        public bool UpdateNotesData(NotesTask NotesObject, string Mode, string CustId)
        {
            bool Update;
            try
            {
                string SqlQuery;
                if (Mode == "Update")
                {
                    SqlQuery = "Update Notes Set Date='" + NotesObject.Date + "', Details='" + NotesObject.Details + "' where ID=" + NotesObject.ID;
                }
                else
                {
                    SqlQuery = "Insert into Notes(CustId, Date, Details) values (" + CustId + ", '" + NotesObject.Date + "', '" + NotesObject.Details + "')";
                }

                Update = objAdaptor.ExecuteNonQuery(SqlQuery);
            }
            catch
            {
                Update = false;
            }
            finally
            {
            }
            return Update;

           
        }

        public bool UpdateTicketsData(TicketsTask TicketsObject, string Mode, string CustID)
        {
            bool Update;
            try
            {
                string SqlQuery;
                if (Mode == "Update")
                {
                    SqlQuery = "Update Tickets Set Date='" + TicketsObject.Date + "', TicketNumber='" + TicketsObject.TicketNumber +
                        "', Status='" + TicketsObject.Status + "', Details='" + TicketsObject.Details + "' Where ID = " + TicketsObject.ID;
                }
                else
                {
                    SqlQuery = "Insert into Tickets Values(" + CustID + ",'" + TicketsObject.Date + "','" + TicketsObject.TicketNumber +
                        "','" + TicketsObject.Details + "','" + TicketsObject.Status + "')";
                }

                Update = objAdaptor.ExecuteNonQuery(SqlQuery);
            }
            catch
            {
                Update = false;
            }
            finally
            {
            }
            return Update;
        }

        public bool UpdateVersionInfo(VersionInfoTask VersionObject, string Mode, string CustId)
        {
            bool Update;
            try
            {
                string SqlQuery;
                if (Mode == "Update")
                {
                    SqlQuery = "Update VersionInfo Set VersionNumber='" + VersionObject.VersionNumber + "', Details='" + VersionObject.Details + "', InstallDate='" + VersionObject.InstallDate + "' where ID=" + VersionObject.ID;
                }
                else
                {
                    SqlQuery = "Insert into VersionInfo(CustId, VersionNumber,InstallDate, Details) values (" + CustId + ", '" + VersionObject.VersionNumber + "','" + VersionObject.InstallDate + "', '" + VersionObject.Details + "')";
                }

                Update = objAdaptor.ExecuteNonQuery(SqlQuery);
            }
            catch
            {
                Update = false;
            }
            finally
            {
            }
            return Update;


        }

       /* public Object DeleteData(CustomerTasks CTObject)
        {
            Object Update;
            try
            {
                string SqlQuery = "Delete Customer where ID = " + CTObject.ID;

                Update = objAdaptor.ExecuteNonQuery(SqlQuery);
            }
            catch
            {
                Update = false;
            }
            finally
            {
            }

            return Update;
        }*/


        public DataTable GetCAddress(CustomerTasks CustomerObject, string Mode)
        {
            DataTable CustomerTable = new DataTable();
            try
            {
                
                if (Mode == "Insert")
                {
                    string SqlQuery = "Select * from Customer where ID ='" + CustomerObject.ID + "'";
                    CustomerTable = objAdaptor.FetchData(SqlQuery);
                }
                else
                {
                    string SqlQuery = "Select * from Customer where ID ='" + CustomerObject.ID + "' and name<>'" + CustomerObject.Name + "'";
                    CustomerTable = objAdaptor.FetchData(SqlQuery);
                }
                
            }
            catch (Exception)
            {
                throw;
            }
            return CustomerTable;
        
        }

        public object DeleteContactsData(CustomerTasks ContactsObject)
        {
            Object Update;
            try
            {
                string SqlQuery = "Delete Contacts where ID='" + ContactsObject.ID + "'";

                Update = objAdaptor.ExecuteNonQuery(SqlQuery);
            }
            catch
            {
                Update = false;
            }
            finally
            {
            }
            return Update;
        }

        public object DeleteNotesData(CustomerTasks NotesObject)
        {
            Object Update;
            try
            {
                string SqlQuery = "Delete Notes where ID='" + NotesObject.ID + "'";

                Update = objAdaptor.ExecuteNonQuery(SqlQuery);
            }
            catch
            {
                Update = false;
            }
            finally
            {
            }
            return Update;
        }

        

        public object DeleteVersionInfoData(CustomerTasks VersionInfoObject)
        {
            Object Update;
            try
            {
                string SqlQuery = "Delete VersionInfo where ID='" + VersionInfoObject.ID + "'";

                Update = objAdaptor.ExecuteNonQuery(SqlQuery);
            }
            catch
            {
                Update = false;
            }
            finally
            {
            }
            return Update;
        }

       

        public CustomerTasks GetCustomerDataForID(CustomerTasks CTObject, string mode)
        {
            DataTable CDTable = new DataTable();
            CustomerTasks ReturnCTObject = new CustomerTasks();
            try
            {
                    string SqlQuery = "Select * from Customer where ID =" + CTObject.ID;
                    CDTable = objAdaptor.FetchData(SqlQuery);
                
                if (CDTable != null && CDTable.Rows.Count == 1)
                {
                    ReturnCTObject.ID = int.Parse(CDTable.Rows[0]["ID"].ToString());
                    ReturnCTObject.Name = CDTable.Rows[0]["Name"].ToString();
                    ReturnCTObject.Status_Type = CDTable.Rows[0]["Status_Type"].ToString();
                    ReturnCTObject.Address = CDTable.Rows[0]["Address"].ToString();
                    ReturnCTObject.ServerCount = CDTable.Rows[0]["ServerCount"].ToString();
                    ReturnCTObject.CompReplacement = CDTable.Rows[0]["CompReplacement"].ToString();
                    ReturnCTObject.OverallStatus = CDTable.Rows[0]["OverallStatus"].ToString();
                    ReturnCTObject.NextFollowUpDate = CDTable.Rows[0]["NextFollowUpDate"].ToString();
                    ReturnCTObject.LicExpDate = CDTable.Rows[0]["LicExpDate"].ToString();
                    ReturnCTObject.BudInfo = CDTable.Rows[0]["BudInfo"].ToString();
                }
            }
            catch
            {
            }
            finally
            {
            }
            return ReturnCTObject;

        }

        public ContactsTask GetContactsDataForID(ContactsTask ContactsObject, string Mode)
        {
            DataTable ContactsTable = new DataTable();
            ContactsTask ReturnContactsObject = new ContactsTask();
            try
            {
                    string SqlQuery = "Select ct.* from  Contacts ct where ct.ID =" + ContactsObject.ID.ToString();
                    ContactsTable = objAdaptor.FetchData(SqlQuery);
                
                if (ContactsTable != null && ContactsTable.Rows.Count == 1)
                {
                    ReturnContactsObject.ID = int.Parse(ContactsTable.Rows[0]["ID"].ToString());
                    ReturnContactsObject.CustID = int.Parse(ContactsTable.Rows[0]["CustID"].ToString());
                    ReturnContactsObject.ContactName = ContactsTable.Rows[0]["Name"].ToString();
                    ReturnContactsObject.PhoneNumber = ContactsTable.Rows[0]["PhoneNumber"].ToString();
                    ReturnContactsObject.Title = ContactsTable.Rows[0]["Title"].ToString();
                    ReturnContactsObject.Details = ContactsTable.Rows[0]["Details"].ToString();

                }
            }
            catch
            {
            }
            finally
            {
            }
            return ReturnContactsObject;

        }

        public NotesTask GetNotesDataForID(NotesTask NotesObject, string Mode)
        {
            DataTable NotesTable = new DataTable();
            NotesTask ReturnNotesObject = new NotesTask();
            try
            {
                    string SqlQuery = "Select nt.* from Notes nt where nt.ID =" + NotesObject.ID.ToString();
                    NotesTable = objAdaptor.FetchData(SqlQuery);
                
                if (NotesTable != null && NotesTable.Rows.Count == 1)
                {
                    ReturnNotesObject.ID = int.Parse(NotesTable.Rows[0]["ID"].ToString());
                    ReturnNotesObject.CustID = int.Parse(NotesTable.Rows[0]["CustID"].ToString());  
                    ReturnNotesObject.Date = NotesTable.Rows[0]["Date"].ToString();
                    ReturnNotesObject.Details = NotesTable.Rows[0]["Details"].ToString();
                    

                }
            }
            catch
            {
            }
            finally
            {
            }
            return ReturnNotesObject;
        }

        public TicketsTask GetTicketsDataForID(TicketsTask TicketsObject, string Mode)
        {
            DataTable TicketsTable = new DataTable();
            TicketsTask ReturnTicketsObject = new TicketsTask();
            try
            {
                    string SqlQuery = "Select tk.* from  Tickets tk where tk.ID =" + TicketsObject.ID.ToString();
                    TicketsTable = objAdaptor.FetchData(SqlQuery);
                if (TicketsTable != null && TicketsTable.Rows.Count == 1)
                {
                    ReturnTicketsObject.ID = int.Parse(TicketsTable.Rows[0]["ID"].ToString());
                    ReturnTicketsObject.CustID = int.Parse(TicketsTable.Rows[0]["CustID"].ToString());  
                    ReturnTicketsObject.TicketNumber = TicketsTable.Rows[0]["TicketNumber"].ToString();
                    ReturnTicketsObject.Date = TicketsTable.Rows[0]["Date"].ToString();
                    ReturnTicketsObject.Status = TicketsTable.Rows[0]["Status"].ToString();
                    ReturnTicketsObject.Details = TicketsTable.Rows[0]["Details"].ToString();
                }
            }
            catch
            {
            }
            finally
            {
            }
            return ReturnTicketsObject;
        }

        public VersionInfoTask GetVersionInfoDataForID(VersionInfoTask VersionInfObject, string Mode)
        {
            DataTable VersionInfoTable = new DataTable();
            VersionInfoTask ReturnVersionInfoObject = new VersionInfoTask();
            try
            {
                
                    string SqlQuery = "Select vi.* from VersionInfo vi where vi.ID =" + VersionInfObject.ID.ToString();
                    VersionInfoTable = objAdaptor.FetchData(SqlQuery);
                if (VersionInfoTable != null && VersionInfoTable.Rows.Count == 1)
                {
                    ReturnVersionInfoObject.ID = int.Parse(VersionInfoTable.Rows[0]["ID"].ToString());
                    ReturnVersionInfoObject.CustID = int.Parse(VersionInfoTable.Rows[0]["CustID"].ToString());  
                    ReturnVersionInfoObject.VersionNumber = VersionInfoTable.Rows[0]["VersionNumber"].ToString();
                    ReturnVersionInfoObject.InstallDate = VersionInfoTable.Rows[0]["InstallDate"].ToString();
                    ReturnVersionInfoObject.Details = VersionInfoTable.Rows[0]["Details"].ToString();

                }
            }
            catch
            {
            }
            finally
            {
            }
            return ReturnVersionInfoObject;
        }

        //Get Customer Contacts based on search criteria
        public DataTable GetCustomerContactDetailsBySearch(string ID, string Name, string PhoneNumber, string Title)
        {

            DataTable dt = new DataTable();
            try
            {

                //string SqlQuery = "Select * FROM Contacts  Where Contacts.CustID = " + ID + " and ("+
                //    "Contacts.Name Like '%" + Name + "%' or Contacts.PhoneNumber Like '%" + PhoneNumber + "%' or Contacts.Title Like '%" + Title + "%')";
                string SqlQuery = "Select cm.*,ct.ID as ContactID, ct.PhoneNumber,ct.Title,ct.Details,ct.name as ContactName FROM Customer cm, Contacts ct  Where cm.id = ct.CustID and ";
                if(ID!="")
                {
                    SqlQuery += "ct.CustID = " + ID + " and ";
                }
                SqlQuery +=  "(ct.Name Like '%" + Name + "%' or ct.PhoneNumber Like '%" + PhoneNumber + "%' or ct.Title Like '%" + Title + "%')";
                //string SqlQuery = "Select * from customer";
                dt = objAdaptor.FetchData(SqlQuery);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
            {
            }

        }

        public DataTable GetCustomerNotesDetailsBySearch(string ID)
        {
            DataTable dt = new DataTable();
            try
            {
                //string SqlQuery = "Select Customer.ID, Customer.Name, Customer.Status_Type, Customer.Address, Customer.ServerCount, Customer.CompReplacement, Customer.OverallStatus," +
                //    "Customer.NextFollowUpDate, Customer.LicExpDate, Customer.BudInfo, Notes.Date, Notes.Details " +
                //    "FROM Customer, Notes  Where Notes.ID = Customer.ID and " +
                //    "Customer.Name ='" + Name + "'";

                string SqlQuery = "Select cm.*,nt.ID as NotesID, nt.Date, nt.Details FROM Customer cm, Notes nt  Where cm.id = nt.CustID";
                if (ID != "")
                {
                    SqlQuery += " and nt.CustID = " + ID;
                }
                dt = objAdaptor.FetchData(SqlQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
            {
            }
        }

        public DataTable GetCustomerTicketsDetailsBySearch(string ID, string TicketNumber, string Status)
        {
            DataTable dt = new DataTable();
            try
            {
                //string SqlQuery = "Select Customer.ID, Customer.Name, Customer.Status_Type, Customer.Address, Customer.ServerCount, Customer.CompReplacement, Customer.OverallStatus," +
                //    "Customer.NextFollowUpDate, Customer.LicExpDate, Customer.BudInfo, Tickets.Date, Tickets.TicketNumber, Tickets.Details, Tickets.Status " +
                //    "FROM Customer, Tickets  Where Tickets.ID = Customer.ID and " +
                //    " Tickets.TicketNumber = '" + TicketNumber + "' or Tickets.Status = '" + Status + "'";
                string SqlQuery = "Select cm.*,tk.ID as TicketID, tk.Date, tk.TicketNumber, tk.Details, tk.Status FROM Customer cm, Tickets tk  Where cm.id = tk.CustID and ";
                    
                if (ID != "")
                {
                    SqlQuery += "tk.CustID = " + ID + " and "; 
                }
                SqlQuery += "(tk.TicketNumber Like '%" + TicketNumber + "%' or tk.Status Like '%" + Status + "%')";

                dt = objAdaptor.FetchData(SqlQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
            {
            }
        }

        public DataTable GetCustomerVersionInfoDetailsBySearch(string ID, string VersionNumber)
        {
            DataTable dt = new DataTable();
            try
            {
                //string SqlQuery = "Select Customer.ID, Customer.Name, Customer.Status_Type, Customer.Address, Customer.ServerCount, Customer.CompReplacement, Customer.OverallStatus," +
                //    "Customer.NextFollowUpDate, Customer.LicExpDate, Customer.BudInfo, VersionInfo.VersionNumber,VersionInfo.InstallDate, VersionInfo.Details " +
                //    "FROM Customer, VersionInfo  Where VersionInfo.ID = Customer.ID and " +
                //    "VersionInfo.VersionNumber = '" + VersionNumber + "'";
                string SqlQuery = "Select cm.*,vi.ID as VersionInfoID, vi.VersionNumber, vi.InstallDate, vi.Details FROM Customer cm, VersionInfo vi  Where cm.id = vi.CustID and ";
                    
                if (ID != "")
                {
                    SqlQuery += "vi.CustID = " + ID + " and ";
                }
                SqlQuery += "(vi.VersionNumber Like '%" + VersionNumber + "%')";
                dt = objAdaptor.FetchData(SqlQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
            {
            }
        }

        //Do same as above method this for tickets, notes , version

        
    }
}
