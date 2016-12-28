import java.util.Date;
import java.util.Properties;
import java.lang.String;
import java.util.Set;
import java.io.*;
import java.util.Calendar;
import javax.management.InstanceNotFoundException;
import javax.management.MalformedObjectNameException;
import javax.management.Notification;
import javax.management.NotificationListener;
import javax.management.ObjectName;
import javax.management.Attribute;
import javax.management.AttributeList;
import javax.management.j2ee.statistics.Stats;
import com.ibm.websphere.management.AdminClient;
import com.ibm.websphere.management.AdminClientFactory;
import com.ibm.websphere.management.exception.ConnectorException;
import com.ibm.websphere.management.configservice.*;
import com.ibm.websphere.management.Session;
import javax.management.j2ee.statistics.Statistic;
import javax.management.j2ee.statistics.CountStatistic;
import javax.management.j2ee.statistics.BoundedRangeStatistic;
import javax.management.j2ee.statistics.RangeStatistic;

import java.io.File;
import java.io.IOException;

import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.ParserConfigurationException;
import javax.xml.transform.Transformer;
import javax.xml.transform.TransformerFactory;
import javax.xml.transform.dom.DOMSource;
import javax.xml.transform.stream.StreamResult;
import javax.xml.xpath.XPath;
import javax.xml.xpath.XPathConstants;
import javax.xml.xpath.XPathExpressionException;
import javax.xml.xpath.XPathExpression;
import javax.xml.xpath.XPathFactory;

import org.w3c.dom.Document;
import org.w3c.dom.NodeList;
import org.w3c.dom.Node;
import org.w3c.dom.Element;
import org.w3c.dom.Attr;
import org.xml.sax.SAXException;

public class ServerStats
{
	private AdminClient adminClient;
    private ObjectName nodeAgent;
    private long ntfyCount = 0;
	private String host,port,user,pwd,sasprops,sslprops,connStatus,dnt;
	private String op="";
	private String cellName;
	private int cFlag=0;
	private long responseTimeInMs;
    
	public static void main(String[] args)
    {
     try
	 {
	   ServerStats ms = new ServerStats();
		String hostName = args[0];
	   String portNo = args[1];
	   String connType = args[2];
	   String userName = args[3];
	   String passwd = args[4];
	   String secured = "true";
	   String nodeName = args[5];
	   String srvrName = args[6];
	   String realm = args[7];

       ms.createAdminClient(hostName,portNo,connType,secured,userName,passwd,realm);
	   ms.getHeapStats(nodeName,srvrName);
	 } catch (Exception e) {
         e.printStackTrace();
     }
	}

	private void createAdminClient(String hostName,String portNo,String connType,String secured,String userName,String passwd,String realm) throws Exception
	{
      Calendar cal = Calendar.getInstance();
	  Date currentTime = cal.getTime();
	  dnt=currentTime.toString();
	  String connTypeStr=null;
	  try
	  {
		Properties connectProps = new Properties();
		connectProps.setProperty(AdminClient.CONNECTOR_HOST,hostName);
		connectProps.setProperty(AdminClient.CONNECTOR_PORT,portNo);
		/*connectProps.setProperty(AdminClient.CONNECTOR_TYPE, AdminClient.CONNECTOR_TYPE_RMI);
		connectProps.setProperty(AdminClient.CONNECTOR_AUTO_ACCEPT_SIGNER,"true");
		connectProps.setProperty(AdminClient.CONNECTOR_HOST,hostName);
		connectProps.setProperty(AdminClient.CONNECTOR_PORT,portNo);
		//System.setOut(new PrintStream(new FileOutputStream("Log_Files/Stats_Connection.log", true)));
		connectProps.setProperty(AdminClient.USERNAME,userName);
		connectProps.setProperty(AdminClient.PASSWORD,passwd);
		System.setProperty("com.ibm.CORBA.loginUserid",userName);
		System.setProperty("com.ibm.CORBA.loginPassword",passwd);
		System.setProperty("com.ibm.CORBA.loginRealm",realm);
		adminClient = null;
		adminClient = AdminClientFactory.createAdminClient(connectProps);
		getCellName(); */
		
		if (connType.equals("RMI") || connType.equals("rmi")) {
			connectProps.setProperty(AdminClient.CONNECTOR_TYPE,AdminClient.CONNECTOR_TYPE_RMI);
			if (secured.equals("true")) {
				connectProps.setProperty(AdminClient.CONNECTOR_SECURITY_ENABLED, "true");
				connectProps.setProperty(AdminClient.CONNECTOR_AUTO_ACCEPT_SIGNER,"true");
				//connectProps.setProperty(AdminClient.USERNAME,userName);
				//connectProps.setProperty(AdminClient.PASSWORD,passwd);
				System.setProperty("com.ibm.CORBA.loginUserid",userName);
				System.setProperty("com.ibm.CORBA.loginPassword",passwd);
				System.setProperty("com.ibm.CORBA.loginRealm",realm);
				//System.setProperty("com.ibm.CORBA.ConfigURL","file:properties//sas.client.props");
				//System.setProperty("com.ibm.SSL.ConfigURL","file:properties//ssl.client.props");
			} else {
				connectProps.setProperty(AdminClient.CONNECTOR_SECURITY_ENABLED, "false");
			}
		}
		if (connType.equals("SOAP") || connType.equals("soap")) {
			//connTypeStr = "AdminClient.CONNECTOR_TYPE_SOAP";
			connectProps.setProperty(AdminClient.CONNECTOR_TYPE,AdminClient.CONNECTOR_TYPE_SOAP);
			connectProps.setProperty(AdminClient.CONNECTOR_SECURITY_ENABLED, "true");
			connectProps.setProperty(AdminClient.CONNECTOR_AUTO_ACCEPT_SIGNER,"true");
			connectProps.setProperty(AdminClient.USERNAME,userName);
			connectProps.setProperty(AdminClient.PASSWORD,passwd);
		} else {
			connectProps.setProperty(AdminClient.CONNECTOR_SECURITY_ENABLED, "false");
		}
		//System.out.println("ConnType String: "+connTypeStr);
		//connectProps.setProperty(AdminClient.CONNECTOR_TYPE,connTypeStr);
		//connectProps.setProperty(AdminClient.CONNECTOR_AUTO_ACCEPT_SIGNER,"true");
		
		//System.setOut(new PrintStream(new FileOutputStream("logs/Stats_Connection.log", true)));
		
		adminClient = null;
		long startTime = System.nanoTime();
		adminClient = AdminClientFactory.createAdminClient(connectProps);
		long endTime = System.nanoTime();
		
		responseTimeInMs = ((long) (endTime - startTime) / 1000000);
		
		getCellName();
		
	   } catch (ConnectorException e) {
			System.out.append(dnt+" - Could not create an RMI connector to connect to host "+hostName+" at port "+portNo+"\n"+e);
			e.printStackTrace(System.out);
			cFlag = 1;
			connStatus = "ERROR";
			System.exit(-1);
	   }
	   connStatus = "CONNECTED";
	   System.out.println(dnt+" - Connection established");
    }

	private void getHeapStats(String nodeName,String serverName) throws Exception
    {
      String opName = "freeMemory";
      String signature[] = {"java.lang.String"};
      String params[] = {serverName};
	  String pid=" ",srvVersion=" ";
	  String srvInfo;
	  int freemem,heapsize,maxMem;
	  Statistic[] ststats=null,threadPoolStats=null;
	  try {
		 com.ibm.websphere.management.configservice.ConfigServiceProxy configService = new com.ibm.websphere.management.configservice.ConfigServiceProxy(adminClient);
		 Session session = new Session();
 		 ObjectName nodes = ConfigServiceHelper.createObjectName(null,"Node",null);
		 ObjectName[] nodeList = configService.queryConfigObjects(session,null,nodes,null);
		 if (nodeList.length <= 0) {
			 //System.out.println(msg);
		 }
		 String nodeDisplayName,srvDisplayName;
		 com.ibm.websphere.management.configservice.ConfigDataId nodeID,srvID;
		 for (int i = 0; i < nodeList.length; i++) {
			 int flag=0;
			 nodeDisplayName = ConfigServiceHelper.getDisplayName(nodeList[i]);
			 //System.out.append("NodeDisplayName: "+nodeDisplayName);
			 nodeID = ConfigServiceHelper.getConfigDataId(nodeList[i]);
			 srvInfo = nodeDisplayName;
			 if (nodeDisplayName.equals(nodeName)) {
				 ObjectName node = ConfigServiceHelper.createObjectName(nodeID,"Node",nodeDisplayName);
				 Object hostName = configService.getAttribute(session,node,"hostName",false);
				 host = hostName.toString();
			 }
		 }
		 configService.discard(session);
         	 	 String queryJVM = "WebSphere:type=JVM,node=" + nodeName + ",process=" + serverName + ",*";
		 String querySrv = "WebSphere:type=Server,node=" + nodeName + ",process=" + serverName + ",*";
		 String queryTP = "WebSphere:type=ThreadPool,name=WebContainer,node=" + nodeName + ",process=" + serverName + ",*";
		 //This will be used later for listing the applications --- String queryDO = "WebSphere:type=DeployedObject,j2eeType=J2EEDeployedObject,node="+ nodeName + ",process=" + serverName + ",*";
		 String msg;
		 Calendar cal = Calendar.getInstance();
		 Date currentTime = cal.getTime();
         	 	 ObjectName queryJ = new ObjectName(queryJVM);
		 ObjectName queryS = new ObjectName(querySrv);
		 ObjectName queryt = new ObjectName(queryTP);
		 //ObjectName queryD = new ObjectName(queryDO);
                	 Set j = adminClient.queryNames(queryJ, null);
		 Set s = adminClient.queryNames(queryS, null);
		 Set tp = adminClient.queryNames(queryt, null);
		 //Set dpO = adminClient.queryNames(queryD, null);
		 ObjectName srvr1,srvr2,tpool,dpObj;
		 if (!j.isEmpty() || !s.isEmpty()) {
				srvr1 = (ObjectName)j.iterator().next();
				//System.out.println("Server mbean found - "+ srvr1.toString());
				srvr2 = (ObjectName)s.iterator().next();
				pid = (String)adminClient.getAttribute(srvr2,"pid");
				srvVersion = (String)adminClient.getAttribute(srvr2,"platformVersion");
				String [] apps = (String [])adminClient.getAttribute(srvr2,"deployedObjects");
				Stats j2eeStats = (Stats)adminClient.getAttribute(srvr1,"stats");
				System.out.append(currentTime.toString()+" - "+nodeName+"/"+serverName+"/"+pid+"/"+srvVersion+"\n");
				//System.out.append("List of applications: \n");
				/*if (!dpO.isEmpty()) {
					dpObj = (ObjectName)dpO.iterator().next();
					String app = (String)adminClient.getAttribute(dpObj,"name");
					System.out.append(app);
				}*/
				/*for (String a : apps) {
					//System.out.append(a.toString());
					ObjectName depObj = new ObjectName(a);
					String app = (String)adminClient.getAttribute(depObj,"name");
					int ear = app.indexOf(".ear");
					if (ear > -1 ){
						System.out.append(app+"\n");
					}

				}*/
				ststats = j2eeStats.getStatistics();
				if (!tp.isEmpty()) {
					tpool = (ObjectName)tp.iterator().next();
					Stats tpStats = (Stats)adminClient.getAttribute(tpool,"stats");
					threadPoolStats = tpStats.getStatistics();
				}
				msg = " - PID:"+pid;
				op = op.concat(" "+msg);
         } else {
			 msg = " - Server "+serverName+" is not reachable. Please check the server logs for more information";
			 op = op.concat(" "+msg);
			 System.out.append(op+"\n");
         }
		 
		 Attr attr,nodeAttr,hostAttr,verAttr,srvrAttr,unitAttr,countAttr,brscurrAttr,brslbAttr,brsubAttr,rscurrAttr,statusAttr,pidAttr,responseTimeAttr;
		 File f = new File("VitalSigns/xml/AppServerStats.xml");
		 boolean boo = f.createNewFile();
		 DocumentBuilderFactory dbFactory = DocumentBuilderFactory.newInstance();
		 DocumentBuilder dBuilder = dbFactory.newDocumentBuilder();
		 Document doc = dBuilder.newDocument();
		 Element cellsElement = doc.createElement("cells");
		 doc.appendChild(cellsElement);
		 Element timeElement = doc.createElement("timestamp");
		 cellsElement.appendChild(timeElement);
		 timeElement.appendChild(doc.createTextNode(dnt));
		 Element connElement = doc.createElement("connection-status");
		 cellsElement.appendChild(connElement);
		 connElement.appendChild(doc.createTextNode(connStatus));
		 if (cFlag == 0) {
		 // Set Cell Element in AppServerStats.xml
		 System.out.println("Cell Name: "+cellName);
		 Element rootElement = doc.createElement("cell");
		 cellsElement.appendChild(rootElement);
		 attr = doc.createAttribute("name");
         attr.setValue(cellName);
		 rootElement.setAttributeNode(attr);
		 Element nodesElement = doc.createElement("nodes");
		 rootElement.appendChild(nodesElement);
		 Element nodeElement = doc.createElement("node");
		 nodeAttr = doc.createAttribute("name");
		 hostAttr = doc.createAttribute("hostName");
		 verAttr = doc.createAttribute("version");
		 nodeAttr.setValue(nodeName);
		 hostAttr.setValue(host);
		 verAttr.setValue(srvVersion);
		 nodeElement.setAttributeNode(nodeAttr);
		 nodeElement.setAttributeNode(hostAttr);
		 nodeElement.setAttributeNode(verAttr);
		 nodesElement.appendChild(nodeElement);
		 Element srvrsElement = doc.createElement("servers");
		 nodeElement.appendChild(srvrsElement);
		 Element srvrElement = doc.createElement("server");
		 srvrAttr = doc.createAttribute("name");
		 srvrAttr.setValue(serverName);
		 srvrElement.setAttributeNode(srvrAttr);
		 srvrsElement.appendChild(srvrElement);
		 Element statsElement = doc.createElement("stats");
		 srvrElement.appendChild(statsElement);
		 Element statusElement;
		 if (pid.equals(" ")) {
			 statusElement = doc.createElement("status");
			 statusAttr = doc.createAttribute("value");
			 statusAttr.setValue("Not Reachable");
			 statusElement.setAttributeNode(statusAttr);
			 statsElement.appendChild(statusElement);
		 } else {
			 statusElement = doc.createElement("status");
			 statusAttr = doc.createAttribute("value");
			 statusAttr.setValue("Running");
			 statusElement.setAttributeNode(statusAttr);
			 Element pidElement = doc.createElement("process");
			 pidAttr = doc.createAttribute("id");
			 pidAttr.setValue(pid);
			 pidElement.setAttributeNode(pidAttr);
			 Element responseTime = doc.createElement("ResponseTime");
			 responseTimeAttr = doc.createAttribute("Value");
			 responseTimeAttr.setValue(Long.toString(responseTimeInMs));
			 responseTime.setAttributeNode(responseTimeAttr);
			 statsElement.appendChild(statusElement);
			 statsElement.appendChild(pidElement);
			 statsElement.appendChild(responseTime);
			 for (Statistic stst : ststats) {
					System.out.append(" Stat: "+stst.getName());
					Element stNameElement = doc.createElement(stst.getName());
					System.out.append(" Unit: "+stst.getUnit());
					unitAttr = doc.createAttribute("unit");
					unitAttr.setValue(stst.getUnit());
					stNameElement.setAttributeNode(unitAttr);
					String [] statstr = stst.toString().split(",");
					for(String instr:statstr) {
						String[] keyvalue = instr.split("=");
						String key = keyvalue[0];
						String val = keyvalue[1];
						if(val.equals("CountStatistic")) {
							CountStatistic cs = (CountStatistic)stst;
							System.out.append(" Count: "+cs.getCount());
							System.out.append("\n");
							countAttr = doc.createAttribute("count");
					        countAttr.setValue(Long.toString(cs.getCount()));
							stNameElement.setAttributeNode(countAttr);
						}
						if(val.equals("BoundedRangeStatistic")) {
							BoundedRangeStatistic brs = (BoundedRangeStatistic)stst;
							System.out.append(" Current: "+brs.getCurrent());
							System.out.append(" Initial: "+brs.getLowerBound());
							System.out.append(" Maximum: "+brs.getUpperBound());
							System.out.append("\n");
							brslbAttr = doc.createAttribute("initial");
					        brslbAttr.setValue(Long.toString(brs.getLowerBound()));
							brsubAttr = doc.createAttribute("maximum");
					        brsubAttr.setValue(Long.toString(brs.getUpperBound()));
							brscurrAttr = doc.createAttribute("current");
					        brscurrAttr.setValue(Long.toString(brs.getCurrent()));
							stNameElement.setAttributeNode(brscurrAttr);
							stNameElement.setAttributeNode(brslbAttr);
							stNameElement.setAttributeNode(brsubAttr);
						}
						if(val.equals("RangeStatistic")) {
							RangeStatistic rs = (RangeStatistic)stst;
							System.out.append(" Current: "+rs.getCurrent());
							System.out.append("\n");
							rscurrAttr = doc.createAttribute("current");
					        rscurrAttr.setValue(Long.toString(rs.getCurrent()));
							stNameElement.setAttributeNode(rscurrAttr);
						}
					}
				    statsElement.appendChild(stNameElement);
				}
			for (Statistic stst : threadPoolStats) {
					System.out.append(" Stat: "+stst.getName());
					System.out.append(" Unit: "+stst.getUnit());
					Element stNameElement = doc.createElement(stst.getName());
					unitAttr = doc.createAttribute("unit");
					unitAttr.setValue(stst.getUnit());
					stNameElement.setAttributeNode(unitAttr);
					String [] statstr = stst.toString().split(",");
					for(String instr:statstr) {
						String[] keyvalue = instr.split("=");
						String key = keyvalue[0];
						String val = keyvalue[1];
						if(val.equals("CountStatistic")) {
							CountStatistic cs = (CountStatistic)stst;
							System.out.append(" Count: "+cs.getCount());
							System.out.append("\n");
							countAttr = doc.createAttribute("count");
					        countAttr.setValue(Long.toString(cs.getCount()));
							stNameElement.setAttributeNode(countAttr);
						}
						if(val.equals("BoundedRangeStatistic")) {
							BoundedRangeStatistic brs = (BoundedRangeStatistic)stst;
							System.out.append(" Current: "+brs.getCurrent());
							brscurrAttr = doc.createAttribute("current");
					        brscurrAttr.setValue(Long.toString(brs.getCurrent()));
							stNameElement.setAttributeNode(brscurrAttr);
							if (stst.getName().equals("PoolSize")) {
								System.out.append(" Minimum: "+brs.getLowerBound());
								System.out.append(" Maximum: "+brs.getUpperBound());
								brslbAttr = doc.createAttribute("minimum");
								brslbAttr.setValue(Long.toString(brs.getLowerBound()));
								brsubAttr = doc.createAttribute("maximum");
								brsubAttr.setValue(Long.toString(brs.getUpperBound()));
								stNameElement.setAttributeNode(brslbAttr);
								stNameElement.setAttributeNode(brsubAttr);
							}
							System.out.append("\n");
						}
						if(val.equals("RangeStatistic")) {
							RangeStatistic rs = (RangeStatistic)stst;
							System.out.append(" Current: "+rs.getCurrent());
							System.out.append("\n");
							rscurrAttr = doc.createAttribute("current");
					        rscurrAttr.setValue(Long.toString(rs.getCurrent()));
							stNameElement.setAttributeNode(rscurrAttr);
						}
					}
			statsElement.appendChild(stNameElement);
			}
		 }
		 TransformerFactory transformerFactory = TransformerFactory.newInstance();
		 Transformer transformer = transformerFactory.newTransformer();
		 DOMSource source = new DOMSource(doc);
		 StreamResult result = new StreamResult(f);
		 transformer.transform(source, result);

		transformer.setOutputProperty(javax.xml.transform.OutputKeys.OMIT_XML_DECLARATION, "yes");
		StringWriter writer = new StringWriter();
		transformer.transform(source, new StreamResult(writer));
		String xmlString = writer.getBuffer().toString().replaceAll("\n|\r", "");
		System.out.println("VitalSigns Output: " + xmlString + "\n");
		 } //End of IF
		 
      } catch (Exception e) {
         System.out.println(e);
         System.exit(-1);
      }
    }

	private void getCellName() throws Exception
	 {
		String query = "WebSphere:type=Server,*";
		ObjectName querySrvs = new ObjectName(query);
        Set srvrs = adminClient.queryNames(querySrvs, null);
		ObjectName srvr;
		 //System.out.println("--------------------------------------------------------");
         if (!srvrs.isEmpty()) {
			srvr = (ObjectName)srvrs.iterator().next();
			cellName = (String)adminClient.getAttribute(srvr,"cellName");

		 }
	 }


}