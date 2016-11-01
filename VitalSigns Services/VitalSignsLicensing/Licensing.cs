﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSNext.Mongo.Repository;
using VSNext.Mongo.Entities;
using MongoDB.Driver;
using System.Configuration;
namespace VitalSignsLicensing
{
    /// <summary>
    /// designed: Dhanraj Seri
    /// Date: 08/30/2016
    /// description: License and HA. 
    /// </summary>
    public class Licensing
    {
        //string cs = "mongodb://192.168.1.10:27017/vitalsigns_reference";
        string cs = ConfigurationManager.ConnectionStrings["VitalSignsMongo"].ToString();
        //List<ServerType> deviceTypeList;
        //List<Nodes> nodesListAlive;
        /// <summary>
        /// reassigns the nodes to all server based on the license and active nodes
        /// first sets all the servers to node "-1" (no node), then checks to see if any nodes are active and if the licenses are adequately available, then does the node assignemnt to the server
        /// </summary>
        private void assignNodesToServers()
        {
            try
            {

                VSNext.Mongo.Repository.Repository<ServerType> repoDeviceLic = new VSNext.Mongo.Repository.Repository<ServerType>(cs);
                List<ServerType> deviceTypeList = repoDeviceLic.All().ToList();

                VSNext.Mongo.Repository.Repository<Nodes> repoLiveNodes = new VSNext.Mongo.Repository.Repository<Nodes>(cs);
                List<Nodes> nodesListAlive = repoLiveNodes.Find(i => i.IsAlive == true).ToList();

                VSNext.Mongo.Repository.Repository<License> repoLic = new VSNext.Mongo.Repository.Repository<License>(cs);
                List<License> licenseList = repoLic.Find(i => i.LicenseKey != "").ToList();

                bool isHAMode = false;
                int validUnits = checkLicenseValidity(licenseList);
                if (validUnits > 0)
                    isHAMode = getLicenseType(licenseList);

                revokeNodeFromAllServers();
                string msg = "Insufficient Licenses";
                AlertLibrary.Alertdll myAlert = new AlertLibrary.Alertdll();

                if (validUnits == 0)
                {
                    //raise system message about insufficient license
                    myAlert.QueueSysMessage(msg);
                    return;
                }
                else
                    myAlert.ResetSysMessage(msg);
                    
                VSNext.Mongo.Repository.Repository<Server> repo = new VSNext.Mongo.Repository.Repository<Server>(cs);
                FilterDefinition<VSNext.Mongo.Entities.Server> filterdef = repo.Filter.Where(i => i.IsEnabled == true);
                ProjectionDefinition<VSNext.Mongo.Entities.Server> projectDef = repo.Project.Include(i => i.CurrentNode).Include(i => i.DeviceType).Include(i => i.LicenseCost).Include(i => i.DeviceName);
                List<Server> serverList = repo.Find(filterdef, projectDef).ToList();
                   
                //first set all servers node to -1 and cost to 0
                serverList= serverList.Select(c => {c.CurrentNode = "-1"; c.CostPerUser =0; return c;}).ToList();

                if (nodesListAlive.Count > 0)
                {

                    VSNext.Mongo.Repository.Repository<Server> repo2 = new VSNext.Mongo.Repository.Repository<Server>(cs);
                    //loop thru each server and assign node
                    foreach (Server s in serverList)
                    {
                        FilterDefinition<VSNext.Mongo.Entities.Server> filterdef1 = repo2.Filter.Where(i => i.DeviceName == s.DeviceName);
                        UpdateDefinition<VSNext.Mongo.Entities.Server> updatedef1 = default(UpdateDefinition<VSNext.Mongo.Entities.Server>);
                        string n = "-1";
                        double licenseCost = 0;
                        if (isFreeLicenseAvailable(s.DeviceType, validUnits, serverList, deviceTypeList))
                        {
                            FilterDefinition<VSNext.Mongo.Entities.Server> filterdefType = repo.Filter.Where(i => i.IsEnabled == true && i.DeviceType==s.DeviceType);
                            ProjectionDefinition<VSNext.Mongo.Entities.Server> projectDefType = repo.Project.Include(i => i.CurrentNode).Include(i => i.DeviceType).Include(i => i.LicenseCost).Include(i => i.DeviceName);
                            List<Server> serverListType = repo.Find(filterdefType, projectDefType).ToList();

                            n = getFreeNode(s, nodesListAlive, serverList, serverListType);
                            licenseCost = getLicenseCost(s.DeviceType, deviceTypeList);
                            if (n == "")
                            {
                                n = "-1";
                                licenseCost = 0;
                            }
                        }
                        updatedef1 = repo2.Updater
                              .Set(i => i.LicenseCost, licenseCost)
                              .Set(i => i.CurrentNode, n);
                        repo2.Update(filterdef1, updatedef1);
                        s.CurrentNode = n;
                        s.LicenseCost = licenseCost;
                    }
                }
                else
                    revokeNodeFromAllServers();

            }
            catch(Exception ex )
            {
                string s = ex.Message.ToString();
            }

        }
        private void revokeNodeFromAllServers()
        {
            try
            {
                VSNext.Mongo.Repository.Repository<Server> repo = new VSNext.Mongo.Repository.Repository<Server>(cs);
                FilterDefinition<VSNext.Mongo.Entities.Server> filterdef = repo.Filter.Where(i => i.IsEnabled == true);
                UpdateDefinition<VSNext.Mongo.Entities.Server> updatedef = default(UpdateDefinition<VSNext.Mongo.Entities.Server>);
                updatedef = repo.Updater
                    .Set(i => i.CurrentNode, "-1")
                    .Set(i => i.LicenseCost, 0);
                repo.Update(filterdef, updatedef);
            }
            catch
            {

            }
           
        }
        /// <summary>
        /// gets the license cost for this type of device
        /// </summary>
        /// <param name="deviceType"></param>
        /// <returns></returns>
        private double getLicenseCost(string deviceType,List<ServerType> deviceTypeList)
        {
            double tempCost = 0;
            try
            {
                tempCost = deviceTypeList.Find(i => i.Name == deviceType).UnitCost;
            }
            catch
            {
                tempCost = 0;
            }
           
            return tempCost;
        }
        /// <summary>
        /// This sub must be called from UI, when a server is enabled/disabled or added/deleted. Should also be called when a License information is updated and when the cost for a device is added/updated/deleted
        /// Triggers the node assignment for all the servers. Also raises a system message when no nodes are alive.
        /// </summary>
        public void refreshServerCollectionWrapper()
        {
            //basically to report dead nodes to system messages
            checkNodeHealth();
            assignNodesToServers();
        }
        private string getFreeNode(Server s1, List<Nodes> nodesListAlive, List<Server> serverListAll, List<Server> serverTypeListAll)
        {
            string returnNode = "";
            try
            {
                double loadFactor = 0;
                bool isLoadBalanced = true;
                if (nodesListAlive.Count == 0)
                    return "-1";
                //VSNext.Mongo.Repository.Repository<Nodes> repo = new VSNext.Mongo.Repository.Repository<Nodes>(cs);
                //List<Nodes> nodesList = repo.Find(i => i.IsAlive == true).ToList();
                //loop thru each server and assign node
                if (nodesListAlive.Count == 1)
                {
                    //just return the one node
                    returnNode = nodesListAlive[0].Name;
                    return returnNode;
                }
                else
                {
                    //compute the load factor. It has to be equal to 100%
                    foreach (Nodes s in nodesListAlive)
                    {
                        if (s.LoadFactor != null)
                            loadFactor += s.LoadFactor;
                    }
                    //if the user has made an error of not properly distributing the load, we will divide equally
                    if (loadFactor != 100)
                    {
                        isLoadBalanced = false;
                        loadFactor = 100 / nodesListAlive.Count;

                    }
                }


                bool isPreferredNodeAlive = false;

                //check if preferred node is alive
                if (s1.AssignedNode != "")
                {
                    foreach (Nodes s in nodesListAlive)
                    {
                        if (s.Name == s1.AssignedNode)
                            isPreferredNodeAlive = true;
                    }
                }
                VSNext.Mongo.Repository.Repository<Server> repoServers = new VSNext.Mongo.Repository.Repository<Server>(cs);
                double useLoadFactor = 100;
                //get the appropriate node
                foreach (Nodes s in nodesListAlive)
                {
                    //List<Server> serverListNode = repoServers.Find(i => i.IsEnabled == true && i.CurrentNode == s.Name).ToList();
                    FilterDefinition<VSNext.Mongo.Entities.Server> filterdef = repoServers.Filter.Where(i => i.IsEnabled == true && i.CurrentNode == s.Name && i.DeviceType ==s1.DeviceType );
                    ProjectionDefinition<VSNext.Mongo.Entities.Server> projectDef = repoServers.Project.Include(i => i.CurrentNode);
                    List<Server> serverListNode = repoServers.Find(filterdef, projectDef).ToList();

                    //List<Server> serverListAll = repoServers.Find(i => i.IsEnabled == true).ToList();
                    //if load on the node is less or the node is set as preferred node
                    if (serverListNode.Count == 0)
                    {
                        returnNode = s.Name;
                        break;
                    }
                    if (isLoadBalanced)
                        useLoadFactor = s.LoadFactor;
                    else
                        useLoadFactor = loadFactor;

                    double nodeLoad = Convert.ToDouble(serverListNode.Count) / Convert.ToDouble(serverTypeListAll.Count);
                    nodeLoad = nodeLoad * 100;

                    if ((nodeLoad < useLoadFactor) || (s1.AssignedNode != "" && isPreferredNodeAlive))
                    {
                        //code to set the preferred node
                        if (isPreferredNodeAlive && s.Name == s1.AssignedNode)
                        {
                            returnNode = s.Name;
                            break;
                        }
                        returnNode = s.Name;
                        break;

                    }
                }
            }
            catch
            {
            }

            return returnNode;
        }
        private bool isFreeLicenseAvailable(string deviceType, int units, List<Server> serverList, List<ServerType> deviceTypeList)
        {
            bool licAvailable = false;
            try
            {
                double tempCost = 0;
                double totalCost = 0;
                try
                {
                    tempCost = deviceTypeList.Single(c => c.Name == deviceType).UnitCost;
                }
                catch
                {
                    tempCost = 0;
                }

                try
                {
                    totalCost = serverList.Sum(s => s.LicenseCost);
                }
                catch
                {
                }

                
                //    totalCost += s.LicenseCost;
                double remainingUnits = units - totalCost;
                if (remainingUnits >= tempCost)
                    licAvailable= true;
                else
                    licAvailable= false;
            }
            catch
            {

            }
            return licAvailable;
            
        }
        private int checkLicenseValidity(List<License> licenseList)
        {
            int units = 0;
            units=licenseList.Where(c => c.ExpirationDate > DateTime.Now && c.units > 0).Sum(c => c.units);
            try
            {
                foreach (License s in licenseList)
                {
                    if (s.ExpirationDate > DateTime.Now && s.units > 0)
                        units = s.units;
                }
            }
            catch
            {
            }
           
            return units;
        }
        private bool getLicenseType(List<License> licenseList)
        {
            bool isHAMode = false;
            try
            {
            if ( licenseList.Where(c => c.InstallType == "HA").Count() > 0)
                        isHAMode = true;
            }
                
            catch
            {

            }
           
            return isHAMode;
        }
        /// <summary>
        /// create a node in the colection if it's not already defined
        /// </summary>
        /// <param name="nodeName"></param>
        /// <param name="hostName"></param>
        private void createNode(string nodeName,string hostName)
        {
            try
            {
                //update the last ping time in the appropriate node
                VSNext.Mongo.Repository.Repository<Nodes> repoNodes = new VSNext.Mongo.Repository.Repository<Nodes>(cs);
                List<Nodes> nodesList = repoNodes.Find(i => i.Name == nodeName).ToList();
                List<Nodes> allLiveNodes = repoNodes.Find(i => i.IsAlive == true).ToList();

                if (nodesList.Count == 0)
                {
                    Nodes n = new Nodes();
                    //if none of the nodes are alive mark this as primary
                    if (allLiveNodes.Count == 0)
                        n.IsPrimary = true;
                    n.Pulse = DateTime.UtcNow;
                    n.Name = nodeName;
                    n.HostName = hostName;
                    repoNodes.Insert(n);
                }
            }
            catch
            {
            }
           
        }
        /// <summary>
        /// Master service should call this sub every minute. 
        /// This sub updated the pulse for the supplied node. Also computes if the node was up or down and triggers the fresh node assignment to servers if true.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="hostName"></param>
        public void doMasterPing(string node,string hostName)
        {
            //createLicense();
            createNode(node, hostName);
            try
            {
                //createDeviceTypeLicense();
                //update the last ping time in the appropriate node
                VSNext.Mongo.Repository.Repository<Nodes> repoNodes = new VSNext.Mongo.Repository.Repository<Nodes>(cs);
                List<Nodes> nodesList = repoNodes.Find(i => i.Name == node).ToList();
                bool triggerServerRefresh = false;
                foreach (Nodes s in nodesList)
                {
                    if (!s.IsAlive)
                        triggerServerRefresh = true;


                    //update the nodetime
                    FilterDefinition<VSNext.Mongo.Entities.Nodes> filterdef = repoNodes.Filter.Where(i => i.Name == node);
                    UpdateDefinition<VSNext.Mongo.Entities.Nodes> updatedef = default(UpdateDefinition<VSNext.Mongo.Entities.Nodes>);
                    updatedef = repoNodes.Updater
                        .Set(i => i.IsAlive, true)
                        .Set(i => i.Pulse, DateTime.UtcNow);
                    repoNodes.Update(filterdef, updatedef);
                }

                //node up or down
                if (checkNodeHealth() || triggerServerRefresh)
                    assignNodesToServers();
            }
            catch
            {
            }
           
        }
        //check if all the nodes are pinging and set the alive status accordingly
        private bool  checkNodeHealth()
        {
            bool triggerServerRefresh = false;

            try
            {
                //update the last ping time in the appropriate node
                VSNext.Mongo.Repository.Repository<Nodes> repoNodes = new VSNext.Mongo.Repository.Repository<Nodes>(cs);
                List<Nodes> nodesListAll = repoNodes.All().ToList();
                foreach (Nodes s in nodesListAll)
                {
                    if ((s.Pulse < DateTime.UtcNow.AddMinutes(-5) || s.Pulse == null) && s.IsAlive)
                    {
                        //update the nodetime
                        FilterDefinition<VSNext.Mongo.Entities.Nodes> filterdef = repoNodes.Filter.Where(i => i.Name == s.Name);
                        UpdateDefinition<VSNext.Mongo.Entities.Nodes> updatedef = default(UpdateDefinition<VSNext.Mongo.Entities.Nodes>);
                        updatedef = repoNodes.Updater
                            .Set(i => i.IsAlive, false);
                        repoNodes.Update(filterdef, updatedef);
                        //node went down
                        triggerServerRefresh = true;

                    }
                }
                //appropriately set the primary node. If the configured primary goes down, set the other active node as primary
                //if the node is configured to be primary and it's down, we need to set another node as primary
                List<Nodes> nodesListPrimary = repoNodes.Find(i => i.IsAlive == false && i.IsConfiguredPrimary ==true && i.IsPrimary==true ).ToList();
                if (nodesListPrimary.Count > 0)
                {
                    List<Nodes> nodesListAlive2 = repoNodes.Find(i => i.IsAlive == true && i.IsPrimary == false ).ToList();
                    foreach (Nodes s in nodesListAlive2)
                    {
                            FilterDefinition<VSNext.Mongo.Entities.Nodes> filterdef = repoNodes.Filter.Where(i => i.Name == s.Name);
                            UpdateDefinition<VSNext.Mongo.Entities.Nodes> updatedef = default(UpdateDefinition<VSNext.Mongo.Entities.Nodes>);
                            updatedef = repoNodes.Updater
                                .Set(i => i.IsPrimary, true );
                            repoNodes.Update(filterdef, updatedef);
                            break;
                    }
                }

                //set the configured primary node as primary if it was not
                List<Nodes> nodesListPrimary2 = repoNodes.Find(i => i.IsAlive == true  && i.IsConfiguredPrimary == true && i.IsPrimary == false).ToList();
                foreach (Nodes s in nodesListPrimary2)
                {
                    FilterDefinition<VSNext.Mongo.Entities.Nodes> filterdef = repoNodes.Filter.Where(i => i.Name == s.Name);
                    UpdateDefinition<VSNext.Mongo.Entities.Nodes> updatedef = default(UpdateDefinition<VSNext.Mongo.Entities.Nodes>);
                    updatedef = repoNodes.Updater
                        .Set(i => i.IsPrimary, true);
                    repoNodes.Update(filterdef, updatedef);
                    break;
                }
                //set rest other not primary
                List<Nodes> nodesListPrimary3 = repoNodes.Find(i => i.IsAlive == true && i.IsConfiguredPrimary == false  && i.IsPrimary == true ).ToList();
                foreach (Nodes s in nodesListPrimary3)
                {
                    FilterDefinition<VSNext.Mongo.Entities.Nodes> filterdef = repoNodes.Filter.Where(i => i.Name == s.Name);
                    UpdateDefinition<VSNext.Mongo.Entities.Nodes> updatedef = default(UpdateDefinition<VSNext.Mongo.Entities.Nodes>);
                    updatedef = repoNodes.Updater
                        .Set(i => i.IsPrimary, false );
                    repoNodes.Update(filterdef, updatedef);
                    break;
                }
                List<Nodes> nodesListPrimary4 = repoNodes.Find(i => i.IsAlive == false  && i.IsPrimary == true).ToList();
                foreach (Nodes s in nodesListPrimary4)
                {
                    FilterDefinition<VSNext.Mongo.Entities.Nodes> filterdef = repoNodes.Filter.Where(i => i.Name == s.Name);
                    UpdateDefinition<VSNext.Mongo.Entities.Nodes> updatedef = default(UpdateDefinition<VSNext.Mongo.Entities.Nodes>);
                    updatedef = repoNodes.Updater
                        .Set(i => i.IsPrimary, false);
                    repoNodes.Update(filterdef, updatedef);
                    break;
                }

                //code to trigger system message that master is not running
                List<Nodes> nodesListAlive = repoNodes.Find(i => i.IsAlive == true).ToList();
                 string msg = "Master Service is not running";
                AlertLibrary.Alertdll myAlert = new AlertLibrary.Alertdll();
                if (nodesListAlive.Count == 0)
                    myAlert.QueueSysMessage(msg);
                else
                {
                    if (nodesListAlive.Count == 1)
                    {
                        foreach (Nodes s in nodesListAlive)
                        {
                            FilterDefinition<VSNext.Mongo.Entities.Nodes> filterdef = repoNodes.Filter.Where(i => i.Name == s.Name);
                            UpdateDefinition<VSNext.Mongo.Entities.Nodes> updatedef = default(UpdateDefinition<VSNext.Mongo.Entities.Nodes>);
                            updatedef = repoNodes.Updater
                                .Set(i => i.IsPrimary, true);
                            repoNodes.Update(filterdef, updatedef);
                        }
                    }
                }
                    myAlert.ResetSysMessage(msg);
            }
            catch
            {
            }

           
            return triggerServerRefresh;
        }
        #region utilities
        private void createLicense()
        {
            //update the last ping time in the appropriate node
            VSNext.Mongo.Repository.Repository<License> repoLicense = new VSNext.Mongo.Repository.Repository<License>(cs);

            FilterDefinition<VSNext.Mongo.Entities.License> filterdef = repoLicense.Filter.Where(i => i.CompanyName == "RPR Wyatt");
            UpdateDefinition<VSNext.Mongo.Entities.License> updatedef = default(UpdateDefinition<VSNext.Mongo.Entities.License>);
            updatedef = repoLicense.Updater
                .Set(i => i.InstallType, "SA")
                .Set(i => i.LicenseKey, "XXXXXXXXX")
                .Set(i => i.units, 40)
                .Set(i => i.ExpirationDate, Convert.ToDateTime("10/10/2042"))
            .Set(i => i.LicenseType, "Subscription");

            repoLicense.Upsert(filterdef, updatedef);

        }
        public License getLicenseInfo(string key)
        {
            VSNext.Mongo.Repository.Repository<License> repoLicense = new VSNext.Mongo.Repository.Repository<License>(cs);
            License l = new License();
            List<License> nodesList = repoLicense.Find(i => i.LicenseKey == key).ToList();
            foreach (License s in nodesList)
            {
                l.InstallType = s.InstallType;
                l.ExpirationDate = s.ExpirationDate;
                l.units = s.units;
            }
            return l;
           
        }
        private void createDeviceTypeLicense()
        {
            //update the last ping time in the appropriate node
            VSNext.Mongo.Repository.Repository<ServerType> repoDeviceTypeLicense = new VSNext.Mongo.Repository.Repository<ServerType>(cs);
            List<ServerType> deviceTypeList = repoDeviceTypeLicense.Find(i => i.Name != "").ToList();
            foreach (ServerType s in deviceTypeList)
            {
                FilterDefinition<VSNext.Mongo.Entities.ServerType> filterdef = repoDeviceTypeLicense.Filter.Where(i => i.Name != s.Name);
                UpdateDefinition<VSNext.Mongo.Entities.ServerType> updatedef = default(UpdateDefinition<VSNext.Mongo.Entities.ServerType>);
                updatedef = repoDeviceTypeLicense.Updater
                    .Set(i => i.UnitCost, 1);

                repoDeviceTypeLicense.Upsert(filterdef, updatedef);
            }

        }
        //check the toal server costs
        //private bool canAssignServerToNode_2(string deviceType, int units)
        //{
        //    bool canAssignServer = false;
        //    VSNext.Mongo.Repository.Repository<DeviceTypeLicense> repoDeviceLic = new VSNext.Mongo.Repository.Repository<DeviceTypeLicense>(cs);
        //    List<DeviceTypeLicense> deviceTypeList = repoDeviceLic.Find(i => i.DeviceType != "").ToList();
        //    double tempCost = 0;
        //    double totalCost = 0;
        //    foreach (DeviceTypeLicense s in deviceTypeList)
        //        if (s.DeviceType == deviceType)
        //            tempCost = s.UnitCost;
        //    VSNext.Mongo.Repository.Repository<Server> repo = new VSNext.Mongo.Repository.Repository<Server>(cs);
        //    List<Server> serverList = repo.Find(i => i.IsEnabled == true).ToList();
        //    //loop thru each server and assign node

        //    foreach (Server s in serverList)
        //        totalCost += s.LicenseCost;
        //    double remainingUnits = units - totalCost;
        //    if (remainingUnits >= tempCost)
        //        return true;
        //    else
        //        return false;
        //}
        #endregion
    }
}
