package com.rpr.sametime;

import java.io.*;
import java.text.SimpleDateFormat;
import java.util.*;

public class awarecheck extends Thread
{

	public awarecheck(String stServer1, String stUser1, String stPw1, String stServer2, String stUser2, String stPw2){
		this.STServer1 = stServer1;
		this.STUser1 = stUser1;
		this.STPw1 = stPw1;
		this.STServer2 = stServer2;
		this.STUser2 = stUser2;
		this.STPw2 = stPw2;
	}
	
    public awarecheck()
    {
        STServer1 = new String();
        STUser1 = new String();
        STPw1 = new String();
        STLookup1 = new String();
        STServer2 = new String();
        STUser2 = new String();
        STPw2 = new String();
        STLookup2 = new String();
        name_count = 0;
        user_array = new String[100];
        start_as_service = true;
        service_log = true;
    }

    public static void resetcomplete()
    {
        notification_complete = false;
    }

    public static void setcomplete()
    {
        notification_complete = true;
    }

    public static void reset_aware_complete()
    {
        awarecheck_complete = false;
    }

    public static void set_aware_complete()
    {
        awarecheck_complete = true;
    }

    public static boolean iscomplete()
    {
        return notification_complete;
    }

    public static boolean aware_iscomplete()
    {
        return awarecheck_complete;
    }

    public void run()
    {
        SimpleDateFormat simpledateformat = new SimpleDateFormat("MM/dd/yy HH:mm:ss");
        Date date = new Date();
        loop_time = simpledateformat.format(date);
        if(!service_log_failures)
            Logger.log((new StringBuilder()).append("Date/time of Check: ").append(loop_time).toString());
        // parseinifile();
        do
        {
            Thread.yield();
            reset_aware_complete();
            awarethreadbegin();
            File file = new File("stlogin_alert");
            file.delete();
        } while(!aware_iscomplete());
    }

    static boolean Find_External_User()
    {
        String s = new String("");
        try
        {
            ResourceBundle resourcebundle = ResourceBundle.getBundle(open_ini);
            s = resourcebundle.getString("External_User");
        }
        catch(Exception exception) { }
        return !s.equals("");
    }
    void awarethreadbegin()
    {
        slave_login_complete = false;
        master_login_complete = false;
        try
        {
            // for loop 
        	STaware staware = new STaware(STServer1, STUser1, STPw1, STUser2, true, debug, preferlogin);
            STaware staware1 = new STaware(STServer2, STUser2, STPw2, STUser1, false, debug, preferlogin);
            // for loop 
            
            System.out.println("<Sametime>");
            Thread thread = new Thread(staware);
            Thread thread1 = new Thread(staware1);
            thread.start();
            thread1.start();
  
            // Hard coding the 30 second wait interval for now. 
            String s = "30"; //FindInterval("Check");
            loop_interval = new Integer(s);
            // String s1 = new String("Check");
            int i = 1;
            do
            {
                if(i > loop_interval.intValue())
                    break;
                sleep(30000L);
                if(aware_iscomplete())
                    break;
                i++;
            } while(true);
            set_aware_complete();
            staware.teardown();
            staware1.teardown();
            thread.join();
            thread1.join();
            System.out.println("</Sametime>");
            Logger.closeLog();
            
        }
        catch(Exception exception)
        {
            Logger.log("exception from sendchats");
            exception.printStackTrace();
        }
    }

 
    static void Printnow(String s)
    {
        if(!debug);
        if((!service_log_failures || !s.startsWith("Connection was successful")) && (!service_log_failures || !s.startsWith("\nServer:")))
        {
            Date date = new Date();
            String s1 = "staware_";
            SimpleDateFormat simpledateformat = new SimpleDateFormat("MM/dd/yy HH:mm:ss");
            String s2 = simpledateformat.format(date);
        }
    }

    
    static String Find_ResolveWaitTime()
    {
    	return "20000"; 
    }

    static String Find_LoginWaitTime()
    {
    	return "20000";
    }

    static String Find_IM_Timeout()
    {
    	return "10000";
    }

    static String FindLoginWait_Interval()
    {
    	return "20000";
    }

    static String FindIM_Interval()
    {
    	return "10";
    }

    static String Findlogut_sleep()
    {
    	return "5";
    }

    String FindInterval(String s)
    {
        String s1 = new String("");
        try
        {
            ResourceBundle resourcebundle = ResourceBundle.getBundle(open_ini);
            s1 = resourcebundle.getString((new StringBuilder()).append(s).append("_Interval").toString());
        }
        catch(Exception exception) { }
        if(s1.equals(""))
            s1 = "90";
        return s1;
    }

    static boolean DisableIM()
    {
        File file = new File("configs/IMEasy.properties");
        return !file.exists();
    }

    static String FindDebug()
    {
        String s = new String("");
        try
        {
            ResourceBundle resourcebundle = ResourceBundle.getBundle(open_ini);
            s = resourcebundle.getString("Debug");
            s = s.trim();
        }
        catch(Exception exception) { }
        if(s.equals(""))
            s = "false";
        return s;
    }

    static void FindSTlogin()
    {
        String s = new String("");
        try
        {
            ResourceBundle resourcebundle = ResourceBundle.getBundle(open_ini);
            hc_stuser = resourcebundle.getString("Sametime_login_user").trim();
            hc_stpw = resourcebundle.getString("Sametime_login_pw").trim();
        }
        catch(Exception exception) { }
    }

    static PrintWriter outlogfile;
    static String TOOL_NAME = "RPR Wyatt Sametime IM";
    static String TOOL_VERSION = "Version 9.0; May 29, 2013";
    static String INIFILE = "configs/staware.properties";
    static String open_ini = new String("");
    String STServer1;
    String STUser1;
    String STPw1;
    String STLookup1;
    String STServer2;
    String STUser2;
    String STPw2;
    String STLookup2;
    Integer loop_interval;
    static String groups_to_search[] = new String[50];
    static int group_count = 0;
    static boolean sendEmail = false;
    static String Errors[] = new String[1000];
    static int error_index = 0;
    static boolean start_as_service = false;
    static String outputfile = "";
    static String inifile = "";
    static String hc_stuser = "";
    static String hc_stpw = "";
    int name_count;
    String user_array[];
    static boolean debug = false;
    static boolean service_log = false;
    static boolean service_log_failures = false;
    static boolean notification_complete = false;
    static boolean awarecheck_complete = false;
    static String loop_time;
    static boolean preferlogin = false;
    static boolean slave_login_complete = false;
    static boolean master_login_complete = false;

}
