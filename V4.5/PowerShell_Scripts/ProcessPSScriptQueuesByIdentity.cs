using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Remoting;
using System.Management.Automation.Runspaces;
using System.Security;
using System.Text;
using System.Threading.Tasks;


namespace PowerShellApp
{
    public class ProcessPSScriptQueuesByIdentity
    {
        public void process()
        {
            string password = "Jnit@Tices2";
            string userName = "jnittech\\administrator";
            System.Uri uri = new Uri("https://WIN-KURRR2QSDJ0.jnittech.com/powershell?serializationLevel=Full");
            System.Security.SecureString securePassword = String2SecureString(password);

            PSCredential creds = new PSCredential(userName, securePassword);

            Runspace runspace = RunspaceFactory.CreateRunspace();

            PowerShell powershell = PowerShell.Create();

            PSCommand command = new PSCommand();
            command.AddCommand("New-PSSession");
            command.AddParameter("ConfigurationName", "Microsoft.Exchange");
            command.AddParameter("ConnectionUri", uri);
            command.AddParameter("Credential", creds);
            command.AddParameter("Authentication", "Default");

            PSSessionOption sessionOption = new PSSessionOption();
            sessionOption.SkipCACheck = true;
            sessionOption.SkipCNCheck = true;
            sessionOption.SkipRevocationCheck = true;

            command.AddParameter("SessionOption", sessionOption);

            powershell.Commands = command;

            try
            {
                // open the remote runspace
                runspace.Open();

                // associate the runspace with powershell
                powershell.Runspace = runspace;

                // invoke the powershell to obtain the results
                Collection<PSSession> result = powershell.Invoke<PSSession>();

                foreach (ErrorRecord current in powershell.Streams.Error)
                {
                    Console.WriteLine("Exception: " + current.Exception.ToString());
                    Console.WriteLine("Inner Exception: " + current.Exception.InnerException);
                }

                if (result.Count != 1)
                    throw new Exception("Unexpected number of Remote Runspace connections returned.");

                // Set the runspace as a local variable on the runspace
                powershell = PowerShell.Create();
                command = new PSCommand();
                command.AddCommand("Set-Variable");
                command.AddParameter("Name", "ra");
                command.AddParameter("Value", result[0]);
                powershell.Commands = command;
                powershell.Runspace = runspace;
                powershell.Invoke();

                // First import the cmdlets in the current runspace (using Import-PSSession)
                powershell = PowerShell.Create();
                command = new PSCommand();
                command.AddScript("Import-PSSession -Session $ra");
                powershell.Commands = command;
                powershell.Runspace = runspace;
                powershell.Invoke();

                // Now run get-ExchangeServer
                System.Collections.ObjectModel.Collection<PSObject> results = new System.Collections.ObjectModel.Collection<PSObject>();

                powershell = PowerShell.Create();
                powershell.Runspace = runspace;

                //Change the Path to the Script to suit your needs
                System.IO.StreamReader sr = new System.IO.StreamReader("C:\\GetIn\\Sudheer\\PowerShellScript-Own\\c#PowerShellScripts\\EX_QueuesBasedOnIdentity.ps1");
                string reportStream = sr.ReadToEnd();
                powershell.AddScript(reportStream);
                powershell.AddParameter("identity", "WIN-KURRR2QSDJ0\\Submission");
                try
                {
                    results = powershell.Invoke();
                   
                }
                catch (Exception ex)
                {


                    Console.WriteLine(ex.Message);
                }
                //powershell.Runspace.SessionStateProxy.SetVariable("server", "win-kurrr2qsdj0.jnittech.com");
                //powershell.Runspace.SessionStateProxy.SetVariable("mbx", "*MBX");

                

                if (powershell.Streams.Error.Count > 51)
                {
                    foreach (ErrorRecord er in powershell.Streams.Error)
                        Console.WriteLine(er.ErrorDetails);
                }
                else
                {
                    foreach (var ps in results)
                    {
                            Console.WriteLine(ps.Properties["RunspaceId"].Name.ToString() + ": " + ps.Properties["RunspaceId"].Value.ToString());
                            Console.WriteLine(ps.Properties["Identity"].Name.ToString() + ": " + ps.Properties["Identity"].Value.ToString());
                            Console.WriteLine(ps.Properties["DeliveryType"].Name.ToString() + ": " + ps.Properties["DeliveryType"].Value.ToString());
                    }
                }
            }
            finally
            {
                // dispose the runspace and enable garbage collection
                runspace.Dispose();
                runspace = null;

                // Finally dispose the powershell and set all variables to null to free
                // up any resources.
                powershell.Dispose();
                powershell = null;
            }
        }

        private static SecureString String2SecureString(string password)
        {
            SecureString remotePassword = new SecureString();
            for (int i = 0; i < password.Length; i++)
                remotePassword.AppendChar(password[i]);

            return remotePassword;

        }
    }
}
