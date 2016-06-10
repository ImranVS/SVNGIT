using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;

namespace VitalSignsScheduledReportsService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
            ServiceInstaller installer = new ServiceInstaller();
            ServiceProcessInstaller installer2 = new ServiceProcessInstaller();
            installer.Installers.Clear();
            installer.ServiceName = "ScheduledReportsService";
            installer.DisplayName = "VitalSigns Plus Scheduled Reports Service";
            base.Installers.Add(installer2);
            installer2.Account = ServiceAccount.LocalSystem;
            installer2.Password = null;
            installer2.Username = null;
            base.Installers.Add(installer2);  
        }
    }
}
