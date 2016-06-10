using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSWebDO;
using System.Data;

namespace VSWebDAL.ConfiguratorDAL
{
  public class BlackBerryDAL
    {
      private Adaptor objAdaptor = new Adaptor();
      private static BlackBerryDAL _self = new BlackBerryDAL();

      public static BlackBerryDAL Ins
      {
          get
          {
              return _self;
          }
      }

      public DataTable getdataofblackberry()
      {
          DataTable dt = new DataTable();
          try
          {
              string s = "select Enabled,Name,NotesMailAddress,Category,ScanInterval,OffHoursScanInterval,DeliveryThreshold"+
                  ",RetryInterval,( Select ServerName from Servers where ID=DestinationServerID) as DestinationServer,DestinationDatabase,InternetMailAddress,NextScan,LastChecked,LastStatus" +
                  ",SourceServer,(Select ServerName from Servers where ID=ConfirmationServerID) as ConfirmationServer ,ConfirmationDatabase from BlackBerry";
              dt=objAdaptor.FetchData(s);
          }
          catch (Exception ex)
          {
              throw ex;
          }
          return dt;

      }

      public DataTable checkforname(string name,string internetmailaddress)
      {
          DataTable dt = new DataTable();
          try
          {
              string s = "select * from BlackBerry where Name='" + name + "'OR NotesMailAddress='" + internetmailaddress + "'";
              //string s = "select * from BlackBerry where Name='" + name + "'and NotesMailAddress<>'" + internetmailaddress + "'";
              dt = objAdaptor.FetchData(s);
          }
          catch (Exception e)
          {
              throw e;
          }
          return dt;
      }


      public Object InsertBlackBerry(BlackBerry BlackBerryObject)
      {
          Object returnval;
          try
          {
              //string st = "Insert into BlackBerry(Enabled,Name,Category,ScanInterval,OffHoursScanInterval,RetryInterval,DestinationServer,DestinationDatabase,InternetMailAddress,ConfirmationServer,ConfirmationDatabase,NotesMailAddress,SourceServer) values" +
               //   "('" + BlackBerryObject.Enabled + "','" + BlackBerryObject.Name + "','" + BlackBerryObject.Category + "'," + BlackBerryObject.ScanInterval + "," + BlackBerryObject.OffHoursScanInterval + "," + BlackBerryObject.RetryInterval + ",'" + BlackBerryObject.DestinationServer + "','" + BlackBerryObject.DestinationDatabase + "','" + BlackBerryObject.InternetMailAddress + "','" + BlackBerryObject.ConfirmationServer + "','" + BlackBerryObject.ConfirmationDatabase + "','" + BlackBerryObject.NotesMailAddress + "','"+BlackBerryObject.SourceServer+"')";
             // returnval = objAdaptor.ExecuteNonQuery(st);

              string st = "Insert into BlackBerry(Enabled,Name,Category,ScanInterval,OffHoursScanInterval,RetryInterval,DestinationServerID,DestinationDatabase,InternetMailAddress,ConfirmationServerID,ConfirmationDatabase,NotesMailAddress,SourceServer) values" +
                 "('" + BlackBerryObject.Enabled + "','" + BlackBerryObject.Name + "','" + BlackBerryObject.Category + "'," + BlackBerryObject.ScanInterval + "," + BlackBerryObject.OffHoursScanInterval + "," + BlackBerryObject.RetryInterval + ",'" + BlackBerryObject.DestinationServerID + "','" + BlackBerryObject.DestinationDatabase + "','" + BlackBerryObject.InternetMailAddress + "','" + BlackBerryObject.ConfirmationServerID + "','" + BlackBerryObject.ConfirmationDatabase + "','" + BlackBerryObject.NotesMailAddress + "','" + BlackBerryObject.SourceServer + "')";
              returnval = objAdaptor.ExecuteNonQuery(st);
          }
          catch (Exception e)
          {
              throw e;
          }
          return returnval;

      }

      public DataTable getdatawithId(BlackBerry BlackberryObject)
      {
          DataTable dt=new DataTable();
          try
          {
             string SqlQuery= "select Enabled,Name,NotesMailAddress,Category,ScanInterval,OffHoursScanInterval,DeliveryThreshold" +
                  ",RetryInterval,( Select ServerName from Servers where ID=DestinationServerID) as DestinationServer,DestinationDatabase,InternetMailAddress,NextScan,LastChecked,LastStatus" +
                  ",SourceServer,(Select ServerName from Servers where ID=ConfirmationServerID) as ConfirmationServer ,ConfirmationDatabase from BlackBerry  where NotesMailAddress ='" + BlackberryObject.NotesMailAddress+"'";
             // string s = "select * from BlackBerry where NotesMailAddress ='" + BlackberryObject.NotesMailAddress+"'";
             dt = objAdaptor.FetchData(SqlQuery);
          }
          catch (Exception e)
          {
              throw e;
          }
          return dt;
      }


      public Object updatedetails(BlackBerry BlackBarryObject,string hidden)
      {
          Object updatedetails;
          try
          {
              //string update = "update BlackBerry set Name='" + BlackBarryObject.Name + "',Enabled='" + BlackBarryObject.Enabled + "',Category='" + BlackBarryObject.Category + "'" +
              //    ",ScanInterval=" + BlackBarryObject.ScanInterval + ",OffHoursScanInterval=" + BlackBarryObject.OffHoursScanInterval + ",RetryInterval=" + BlackBarryObject.RetryInterval + "" +
              //    ",ConfirmationServer='" + BlackBarryObject.ConfirmationServer + "',ConfirmationDatabase='" + BlackBarryObject.ConfirmationDatabase + "',DestinationServer='" + BlackBarryObject.DestinationServer + "'" +
              //    ",DestinationDatabase='" + BlackBarryObject.DestinationDatabase + "' where NotesMailAddress='" + hidden + "'";
              //updatedetails = objAdaptor.ExecuteNonQuery(update);
              string update = "update BlackBerry set Name='" + BlackBarryObject.Name + "',Enabled='" + BlackBarryObject.Enabled + "',Category='" + BlackBarryObject.Category + "'" +
                 ",ScanInterval=" + BlackBarryObject.ScanInterval + ",OffHoursScanInterval=" + BlackBarryObject.OffHoursScanInterval + ",RetryInterval=" + BlackBarryObject.RetryInterval + "" +
                 ",ConfirmationServerID='" + BlackBarryObject.ConfirmationServerID + "',ConfirmationDatabase='" + BlackBarryObject.ConfirmationDatabase + "',DestinationServerID='" + BlackBarryObject.DestinationServerID + "'" +
                 ",DestinationDatabase='" + BlackBarryObject.DestinationDatabase + "' where NotesMailAddress='" + hidden + "'";
              updatedetails = objAdaptor.ExecuteNonQuery(update);


          }
          catch (Exception e)
          {

              throw e;
          }
          return updatedetails;
       }

      public DataTable getdatadominoserver()
      {
          DataTable d = new DataTable();
          try
          {
              string ss = "select *,servers.ID as SID,ServerName as Name,LocationID,Description,IPAddress from Servers  where servertypeID=(select ID from ServerTypes where ServerType='Domino')";
             // string ss = "select *,servers.ID as SID,ServerName as Name,LocationID,Description,IPAddress from BlackBerryServers right join servers on BlackBerryServers.ServerID=servers.ID where servertypeID=(select ID from ServerTypes where ServerType='BES')";
             // string ss = "select Name from DominoServers";
              d = objAdaptor.FetchData(ss);
          }
          catch (Exception e)
          {
              throw e;
          }

          return d;
      }

      public Object delete(BlackBerry BlackberryObject)
      {
          Object delete;
         
          try
          {
              string s = "delete BlackBerry where NotesMailAddress='" + BlackberryObject.NotesMailAddress + "'";
              delete=  objAdaptor.ExecuteNonQuery(s);
          }
          catch (Exception e)
          {
              throw e;
          }
          return delete;
      }

      public DataTable GetDatabyName(BlackBerry BBObj,string mail)
      {
          //SametimeServers SametimeObj = new SametimeServers();
          DataTable BBTable = new DataTable();
          try
          {
              if (BBObj.NotesMailAddress ==""||BBObj.NotesMailAddress==null)
              {
                  
                  string sqlQuery = "Select * from BlackBerry where  Name='" + BBObj.Name + "' ";
                  BBTable = objAdaptor.FetchData(sqlQuery);
                  if (BBTable.Rows.Count == 0)
                  {
                      string sqlQuery1 = "Select * from BlackBerry where NotesMailAddress='" +mail+ "'";
                      DataTable BBTable1 = objAdaptor.FetchData(sqlQuery1);
                      return BBTable1;

                  }
                  else
                  {
                      return BBTable;
                  }
              }
              else
              {
                  string sqlQuery = "Select * from BlackBerry where  Name='" + BBObj.Name + "' and NotesMailAddress<>'" + BBObj.NotesMailAddress + "' ";
                  BBTable = objAdaptor.FetchData(sqlQuery);
                   return BBTable;
              }
          }
          catch (Exception e)
          {

              throw e;
          } 
         

      }


    }
}
