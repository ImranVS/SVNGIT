using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;
using VSWebDAL;

namespace VSWebBL.SecurityBL
{
	public class ProfilesNamesBL
	{
		/// <summary>
		/// Declarations
		/// </summary>
		private static ProfilesNamesBL _self = new ProfilesNamesBL();

		/// <summary>
		/// Used to call the functions using class name instead of object
		/// </summary>
		public static ProfilesNamesBL Ins
		{
			get { return _self; }
		}
		public DataTable GetAllData()
		{
			try
			{
				return VSWebDAL.SecurityDAL.ProfilesNamesDAL.Ins.GetAllData();
			}
			catch (Exception)
			{	
				throw;
			}
			
		}
		#region "Validations"

		/// <summary>
		/// Validation before submitting data for Server tab
		/// </summary>
		/// <param name="LocObject"></param>
		/// <returns></returns>
		public Object ValidateUpdate(ProfileNames LocObject)
		{
			Object ReturnValue = "";
			try
			{
				if (LocObject.ProfileName == null || LocObject.ProfileName == "")
				{
					return "ER#Please enter the ProfileName";
				}

			}
			catch (Exception ex)
			{ throw ex; }
			finally
			{ }
			return "";
		}

		#endregion

		/// <summary>
		/// Call to Get Data from Locations based on Primary key
		/// </summary>
		/// <param name="LocObject">Locations object</param>
		/// <returns></returns>
		public ProfileNames GetData(ProfileNames LocObject)
		{
			try
			{
				return VSWebDAL.SecurityDAL.ProfilesNamesDAL.Ins.GetData1(LocObject);
			}
			catch (Exception)
			{
				
				throw;
			}
			
		}
		// get data based on location

		public ProfileNames GetDataForLocation1(ProfileNames LocObject)
		{
			try
			{
				return VSWebDAL.SecurityDAL.ProfilesNamesDAL.Ins.GetDataForLocation1(LocObject);
			}
			catch (Exception)
			{
				
				throw;
			}
			
		}


		/// <summary>
		/// Call to Insert Data into Locations
		///  </summary>
		/// <param name="LocObject">Locations object</param>
		/// <returns></returns>
		public object InsertData1(ProfileNames LocObject)
		{
			try
			{
				Object ReturnValue = ValidateUpdate(LocObject);
				DataTable value = VSWebDAL.SecurityDAL.ProfilesNamesDAL.Ins.GetDataForProfilesByname1(LocObject);

				if (value.Rows.Count == 0)
				{
					if (ReturnValue.ToString() == "")
					{
						return VSWebDAL.SecurityDAL.ProfilesNamesDAL.Ins.InsertData1(LocObject);
					}
					else return ReturnValue;
				}
				else return "";
			}
			catch (Exception)
			{
				
				throw;
			}
			


		}
		/// <summary>
		/// Call to Update Data of DominoServers based on Key
		/// </summary>
		/// <param name="LocObject">DominoServers object</param>
		/// <returns>Object</returns>
		public Object UpdateData1(ProfileNames LocObject)
		{
			try
			{
				Object ReturnValue = ValidateUpdate(LocObject);
				DataTable value = VSWebDAL.SecurityDAL.ProfilesNamesDAL.Ins.GetDataForProfilesByname1(LocObject);
				if (value.Rows.Count == 0)
				{
					if (ReturnValue.ToString() == "")
					{
						return VSWebDAL.SecurityDAL.ProfilesNamesDAL.Ins.UpdateData1(LocObject);
					}
					else return ReturnValue;
				}
				else return "";
			}
			catch (Exception)
			{
				
				throw;
			}
			
		}
		/// <summary>
		/// Call DAL Delete Data
		/// </summary>
		/// <param name="LocObject"></param>
		/// <returns></returns>
		public Object DeleteData1(ProfileNames LocObject)
		{
			try
			{
				return VSWebDAL.SecurityDAL.ProfilesNamesDAL.Ins.DeleteData1(LocObject);
			}
			catch (Exception)
			{
				
				throw;
			}
			
		}
		public bool GetDataCopy(string profilename)
		{
			try
			{
				return VSWebDAL.SecurityDAL.ProfilesNamesDAL.Ins.GetDataCopy(profilename);
			}
			catch (Exception)
			{
				
				throw;
			}
			
		}
		public DataTable Getserversbyloction(string LocName)
		{
			try
			{
				return VSWebDAL.SecurityDAL.AdminTabDAL.Ins.GetServersByLocation(LocName);
			}
			catch (Exception)
			{
				
				throw;
			}
			
		}
		public DataTable GetValuebyID(ProfileNames ProfileObject)
		{
			try
			{
				return VSWebDAL.SecurityDAL.ProfilesNamesDAL.Ins.GetValuebyID(ProfileObject);
			}
			catch (Exception)
			{
				
				throw;
			}
			//return VSWebDAL.ConfiguratorDAL.TravelerDAL.Ins.GetValuebyID(TravelerObject);
			
		}

	}
}
