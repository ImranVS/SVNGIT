using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Security;
//using System.Management.Automation;
//using System.Management.Automation.Runspaces;
//using System.Management.Automation.Remoting;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Configuration;


using VSFramework;
using System;
using VitalSignsMicrosoftClasses;

using RPRWyatt.VitalSigns.Services;
namespace VitalSignsExchange
{
    public partial class VitalSignsMicrosoft : VSServices
    {
        protected override void ServiceOnStart(string[] args)
        {
            try
            {
			    //Sowjanya 1558 ticket
			    var myRegistry = new VSFramework.RegistryHandler();
			    myRegistry.WriteToRegistry("VS Microsoft Service Start", (DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString()));

			    MicrosoftHelperObject MSObj = getMicrosoftHelperObject();
                ExchangeMAIN exMain = null;
                ActiveDirectoryMAIN adMain = null;
                MonitorTables monTbls = null;
                WindowsMAIN winMain = null;
                Office365MAIN o365Main = null;
                SharepointMAIN spMain = null;


                exMain = new ExchangeMAIN();
                Thread MasterExchangeThread = new Thread(() => exMain.StartProcess(MSObj));
                MasterExchangeThread.IsBackground = true;
                MasterExchangeThread.Priority = ThreadPriority.Normal;
                MasterExchangeThread.Name = "Master Exchange Thread";
                MasterExchangeThread.Start();
                Thread.Sleep(2000);

                //adMain = new ActiveDirectoryMAIN();
                //Thread MasterActiveDirectoryThread = new Thread(() => adMain.StartProcess(MSObj));
                //MasterActiveDirectoryThread.IsBackground = true;
                //MasterActiveDirectoryThread.Priority = ThreadPriority.Normal;
                //MasterActiveDirectoryThread.Name = "Master AD Thread";
                //MasterActiveDirectoryThread.Start();
                //Thread.Sleep(2000);

                //spMain = new SharepointMAIN();
                //Thread MasterSharePointThread = new Thread(() => spMain.StartProcess(MSObj));
                //MasterSharePointThread.IsBackground = true;
                //MasterSharePointThread.Priority = ThreadPriority.Normal;
                //MasterSharePointThread.Name = "Master SP Thread";
                //MasterSharePointThread.Start();
                //Thread.Sleep(2000);

                o365Main = new Office365MAIN();
				Thread MasterO365Thread = new Thread(() => o365Main.StartProcess(MSObj));
				MasterO365Thread.IsBackground = true;
				MasterO365Thread.Priority = ThreadPriority.Normal;
				MasterO365Thread.Name = "Master O365 Thread";
				MasterO365Thread.Start();
				Thread.Sleep(2000);

				//winMain = new WindowsMAIN();
				//Thread winMainThread = new Thread(() => winMain.StartProcess(MSObj));
				//winMainThread.IsBackground = true;
				//winMainThread.Priority = ThreadPriority.Normal;
				//winMainThread.Name = "Master Win Thread";
				//winMainThread.Start();
				//Thread.Sleep(2000);

				monTbls = new MonitorTables(ref adMain, ref exMain, ref spMain, ref winMain, ref o365Main);
				Thread monitorChanges = new Thread(new ThreadStart(monTbls.CheckForTableChanges));
				monitorChanges.IsBackground = true;
				monitorChanges.Priority = ThreadPriority.Normal;
				monitorChanges.Name = "Master tblChanges Thread";
				monitorChanges.Start();
				Thread.Sleep(2000);
				
            }
            catch(Exception ex)
            {
                VitalSignsMicrosoftClasses.Common.WriteDeviceHistoryEntry("All", "Microsoft", " Exception: " + ex.StackTrace.ToString(), commonEnums.ServerRoles.Empty, VitalSignsMicrosoftClasses.Common.LogLevel.Verbose);
            }
        }
    }
}
