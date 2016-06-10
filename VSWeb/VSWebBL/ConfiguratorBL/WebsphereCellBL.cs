using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VSWebDO;
using VSWebDAL;

namespace VSWebBL.ConfiguratorBL
{
public	class WebsphereCellBL
	{
	private static WebsphereCellBL _self = new WebsphereCellBL();
	public static WebsphereCellBL Ins
		{
			get
			{
				return _self;
			}

		}

	public DataTable GetWebsphereCellNames()
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.WebspehereCellDAL.Ins.GetWebsphereCellNames();       
			}
			catch (Exception ex)
			{
				
				throw ex;
			} 
			
        }
	public Object Deletecell(WebsphereCell cell)
	{
		try
		{
			return VSWebDAL.ConfiguratorDAL.WebspehereCellDAL.Ins.Deletecell(cell);
		}
		catch (Exception ex)
		{
			
			throw ex;
		}
	

	}
	public DataTable Fillcombobox()
        {
			try
			{
				return VSWebDAL.ConfiguratorDAL.WebspehereCellDAL.Ins.Fillcombobox();       
			}
			catch (Exception ex)
			{
				
				throw ex;
			}
			
        }
	
	}
}
