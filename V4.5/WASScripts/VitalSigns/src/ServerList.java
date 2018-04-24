import java.util.Date;
import java.util.Properties;
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

import java.util.List;
import java.util.ArrayList;

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
//import org.w3c.dom.NodeMap;
import org.w3c.dom.Node;
import org.w3c.dom.Element;
import org.w3c.dom.Attr;
import org.xml.sax.SAXException;

public class ServerList
{
	private AdminClient adminClient;
    private ObjectName nodeAgent;
    private long ntfyCount = 0;
	private String host,port,user,pwd,sasprops,sslprops,connStatus,dnt;
	private String op="";
	private int cFlag=0;
	private String cellName;
	
	
    public static void main(String[] args)
    {
     try 
	 {
	   ServerList ms = new ServerList();
	   
	   String hostName = args[0];
	   String portNo = args[1];
	   String connType = args[2];
	   String userName = args[3];
	   String passwd = args[4];
	   String realm = "defaultWIMFileBasedRealm";
	   String secured = "true";
	   //Create an AdminClient 
       ms.createAdminClient(hostName,portNo,connType,secured,userName,passwd,realm);
	   ms.getServersList();
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
		//System.out.println("Host,Port,User,pwd"+hostName+portNo+userName+passwd);
		//System.out.println("ConnType: "+connType);
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
		
		System.setOut(new PrintStream(new FileOutputStream("logs/ServersList_Connection.log", true)));
		
		adminClient = null;
		adminClient = AdminClientFactory.createAdminClient(connectProps);
		getCellName();
	   } catch (ConnectorException e) {
			System.out.append(dnt+" - Could not create an "+connType+" connector to connect to host "+hostName+" at port "+portNo+"\n"+e);
			e.printStackTrace(System.out);
			cFlag = 1;
			connStatus = "ERROR";
			getServersList();
			System.exit(-1);
	   }
	   connStatus = "CONNECTED";
	   System.out.println(dnt+" - Connection established");
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
	 
	 private void getServersList() throws Exception
	 {
		 Attr attr,nodeAttr,hostAttr;
		 List<String> srvrs;
		 File f = new File("VitalSigns/xml/AppServerList.xml");
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
		 // Set Cell Element in AppServerList.xml
		 Element rootElement = doc.createElement("cell");
		 cellsElement.appendChild(rootElement);
		 attr = doc.createAttribute("name");
         attr.setValue(cellName);
		 rootElement.setAttributeNode(attr);
		 Element nodesElement = doc.createElement("nodes");
		 rootElement.appendChild(nodesElement);
		 
		 com.ibm.websphere.management.configservice.ConfigServiceProxy configService = new com.ibm.websphere.management.configservice.ConfigServiceProxy(adminClient);
		 Session session = new Session();
		 String srvInfo;
		 ObjectName nodes = ConfigServiceHelper.createObjectName(null,"Node",null);
		 ObjectName[] nodeList = configService.queryConfigObjects(session,null,nodes,null);
		 if (nodeList.length <= 0) {
			 //String msg = " - Node not found in the configuration. Please correct the name attribute of the server element in servers.xml file.";
			 //System.out.println(msg);
		 }
		 String nodeDisplayName,srvDisplayName;
		 com.ibm.websphere.management.configservice.ConfigDataId nodeID,srvID;
		 System.out.println("NodeName/HostName/Servers");
		 for (int i = 0; i < nodeList.length; i++) {
			int flag=0;
			srvrs = new ArrayList<String>();
			nodeDisplayName = ConfigServiceHelper.getDisplayName(nodeList[i]);
			nodeID = ConfigServiceHelper.getConfigDataId(nodeList[i]);
			srvInfo = nodeDisplayName;
			ObjectName node = ConfigServiceHelper.createObjectName(nodeID,"Node",nodeDisplayName);
			Object hostName = configService.getAttribute(session,node,"hostName",false);
			srvInfo = srvInfo+"/"+hostName+"/Servers - ";
			ObjectName srvr = ConfigServiceHelper.createObjectName(null,"Server",null);
			ObjectName[] srvList = configService.queryConfigObjects(session,node,srvr,null);
		    if (srvList.length <= 0) {
				String msg = " - Servers not found in the configuration. Please correct the name attribute of the server element in servers.xml file.";
				System.out.println(msg);
		    }
			for (int j = 0; j < srvList.length; j++) {
				srvDisplayName = ConfigServiceHelper.getDisplayName(srvList[j]);
				srvID = ConfigServiceHelper.getConfigDataId(srvList[j]);
				ObjectName srvData = ConfigServiceHelper.createObjectName(srvID,"Server",srvDisplayName);
				Object srvType = configService.getAttribute(session,srvData,"serverType",false);
				if (srvType.equals("APPLICATION_SERVER") || srvType.equals("NODE_AGENT")) {
						//System.out.print(srvDisplayName+" | ");
						flag = 1;
						srvInfo = srvInfo+""+srvDisplayName+" | ";
						
						srvrs.add(srvDisplayName);
				}
				
			}
            if (flag==1) {
				//Append Node Element and name, host name attributes to XML
				Element nodeElement = doc.createElement("node");
				Element srvrsElement = doc.createElement("servers");
				nodesElement.appendChild(nodeElement);
				nodeElement.appendChild(srvrsElement);
				nodeAttr = doc.createAttribute("name");
				hostAttr = doc.createAttribute("hostName");
				nodeAttr.setValue(nodeDisplayName);
				hostAttr.setValue(hostName.toString());
				hostAttr.setValue(hostName.toString());
				nodeElement.setAttributeNode(nodeAttr);
				nodeElement.setAttributeNode(hostAttr);
				for (String s : srvrs) {
					//System.out.println("SERVER Name:"+s);
					Element srvrElement = doc.createElement("server");
					srvrElement.appendChild(doc.createTextNode(s));
					srvrsElement.appendChild(srvrElement);
					
				}
				System.out.println(srvInfo);
			}
		 }
		 configService.discard(session);
		} 
		TransformerFactory transformerFactory = TransformerFactory.newInstance();
		Transformer transformer = transformerFactory.newTransformer();
		DOMSource source = new DOMSource(doc);
		StreamResult result = new StreamResult(f);
		transformer.transform(source, result);
		
		 
	 }
}