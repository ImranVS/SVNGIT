package com.rpr.sametime;

import com.lotus.sametime.awareness.*;
import com.lotus.sametime.community.*;
import com.lotus.sametime.core.comparch.DuplicateObjectException;
import com.lotus.sametime.core.comparch.STSession;
import com.lotus.sametime.core.constants.EncLevel;
import com.lotus.sametime.core.types.*;
import com.lotus.sametime.im.*;
import com.lotus.sametime.lookup.*;
import java.io.PrintStream;
import java.sql.Timestamp;
import java.text.SimpleDateFormat;
import java.util.Date;

public class STaware extends Thread
    implements Runnable, LoginListener, ImServiceListener, ImListener, ResolveListener, AwarenessServiceListener, StatusListener, GroupContentListener
{

    public STaware(String s, String s1, String s2, String s3, boolean flag, boolean flag1, boolean flag2)
    {
        running = true;
        STUSERS = null;
        st_server = "";
        st_user = "";
        st_pw = "";
        MonitorName = new String();
        error_index = 0;
        users_to_notify = new String[100];
        splitter = ",";
        user_names = new String[100];
        name_count = 0;
        im_sent = 0;
        master = false;
        thread_complete = false;
        awareness_listening = false;
        master_IM_count = 0;
        slave_IM_count = 0;
        user_added_to_watch_list = false;
        IM_tag_started = false;
        preferloginid = false;
        IM_timeout = 0L;
        Login_threshold = 0L;
        Login_timeout = 0L;
        Resolve_threshold = 0L;
        ignore_first_status_check = true;
        debug = flag1;
        st_server = new String(s);
        st_user = new String(s1);
        st_pw = new String(s2);
        MonitorName = s3;
        master = flag;
        thread_complete = false;
        preferloginid = flag2;
        String s4 = awarecheck.Find_IM_Timeout();
        Integer integer = new Integer(s4);
        IM_timeout = integer.longValue();
        String s5 = awarecheck.Find_LoginWaitTime();
        Integer integer1 = new Integer(s5);
        Login_threshold = integer1.longValue();
        String s6 = awarecheck.Find_ResolveWaitTime();
        Integer integer2 = new Integer(s6);
        Resolve_threshold = integer2.longValue();
    }

    static void sysOut(String s, boolean flag)
    {
        //if(flag)
    	SimpleDateFormat sdf = new SimpleDateFormat("MM/dd/yyyy HH:mm:ss");
        Date resultdate = new Date(System.currentTimeMillis());
        System.out.println(sdf.format(resultdate) + "  "  + s);
    }

    public void run()
    {
        try
        {
            Logger.log("Notification thread is now starting");
            if(stsession != null)
            {
                stsession.stop();
                stsession.unloadSession();
            }
            if(master)
                stsession = new STSession("AwareSession_master");
            else
                stsession = new STSession("AwareSession_slave");
            stsession.loadSemanticComponents();
            stsession.start();
            stLogin(st_server, st_user, st_pw);
            Logger.log("Session created");
         
            
            while(!thread_complete) 
                try
                {
                    sleep(2000L);
                }
                catch(Exception exception) { }
            if(master_IM_count > 0)
            {
                IM_ReceiveDate = new Date();
                long l = IM_ReceiveDate.getTime() - IM_SendDate.getTime();
                if(IM_timeout * 2L <= l)
                {
                    Logger.log("Last IM sent during this run may not have been received. Run terminated and wait time was exceeding IM_Timeout setting multiplied by 2");
                }
            }
            if(master)
            {
                Logger.log("Master thread complete; Stats:\n");
                if(master_IM_count < 1)
                {
                    Logger.log((new StringBuilder()).append("No IMs were sent by ").append(st_user).append(", check in log to see if awareness was set to online or if IM was sent but not received by user: ").append(MonitorName).toString());
                }
                Logger.log((new StringBuilder()).append("\tTotal IM's sent: ").append(master_IM_count).append("\n").toString());
                Logger.log((new StringBuilder()).append("In Master, Recycling session in minutes: ").append(awarecheck.Findlogut_sleep()).toString());
            } else
            {
                Logger.log("Slave thread complete; Stats:\n");
                if(slave_IM_count < 1)
                {
                    Logger.log((new StringBuilder()).append("No IMs were received by ").append(st_user).append(", check in log to see if awareness was set to online or if IM was sent but not received").toString());
                }
                Logger.log((new StringBuilder()).append("\tTotal IM's sent: ").append(slave_IM_count).append("\n").toString());
                Logger.log((new StringBuilder()).append("In slave, Recycling session in minutes: ").append(awarecheck.Findlogut_sleep()).toString());
            }
            try
            {
                String s = awarecheck.Findlogut_sleep();
                Integer integer = new Integer(s);
                //sleep(integer.intValue() * 60 * 1000);
            }
            catch(Exception exception1) { }
            Logger.log("login thread is now complete");
        }
        catch(DuplicateObjectException duplicateobjectexception)
        {
            try
            {
                if(stsession != null)
                {
                    stsession.stop();
                    stsession.unloadSession();
                }
                stsession = new STSession("AwareSession3");
                stsession.loadSemanticComponents();
                stsession.start();
                Logger.log("Session created after duplicate session");
                stLogin(st_server, st_user, st_pw);
                while(!thread_complete) 
                    try
                    {
                        sleep(2000L);
                    }
                    catch(Exception exception2) { }
                if(master_IM_count > 0)
                {
                    IM_ReceiveDate = new Date();
                    long l1 = IM_ReceiveDate.getTime() - IM_SendDate.getTime();
                    if(IM_timeout * 2L <= l1)
                    {
                        Logger.log("Last IM sent during this run may not have been received. Run terminated and wait time was exceeding IM_Timeout setting multiplied by 2");
                    }
                }
                if(master)
                {
                    Logger.log("Master thread complete; Stats:\n");
                    if(master_IM_count < 1)
                    {
                        Logger.log((new StringBuilder()).append("No IMs were sent by ").append(st_user).append(", check in log to see if awareness was set to online or if IM was sent but not received by user: ").append(MonitorName).toString());
                    }
                    Logger.log((new StringBuilder()).append("\tTotal IM's sent: ").append(master_IM_count).append("\n").toString());
                    Logger.log((new StringBuilder()).append("In Master, Recycling session in minutes: ").append(awarecheck.Findlogut_sleep()).toString());
                } else
                {
                    Logger.log("Slave thread complete; Stats:\n");
                    if(slave_IM_count < 1)
                    {
                        Logger.log((new StringBuilder()).append("No IMs were received by ").append(st_user).append(", check in log to see if awareness was set to online or if IM was sent but not received").toString());
                    }
                    Logger.log((new StringBuilder()).append("\tTotal IM's sent: ").append(slave_IM_count).append("\n").toString());
                    Logger.log((new StringBuilder()).append("In slave, Recycling session in minutes: ").append(awarecheck.Findlogut_sleep()).toString());
                }
                try
                {
                    String s1 = awarecheck.Findlogut_sleep();
                    Integer integer1 = new Integer(s1);
                    //sleep(integer1.intValue() * 60 * 1000);
                }
                catch(Exception exception3) { }
                Logger.log("login thread is now complete");
            }
            catch(Exception exception4)
            {
                Logger.log("We failed getting the sametime session");
            }
        }
    }

    public void teardown()
    {
        Logger.log("Destroy called");
        if(master && !awarecheck.master_login_complete)
        {
            Logger.log((new StringBuilder()).append("Login failure for ").append(st_server).append(". Unable to complete login").toString());
        }
        if(!master && !awarecheck.slave_login_complete)
        {
            Logger.log((new StringBuilder()).append("Login failure for ").append(st_server).append(". Unable to complete login").toString());
        }
        if(debug)
            Logger.log("teardown process underway");
        if(commService != null && commService.isLoggedIn())
        {
            if(master)
                Logger.log("Master doing the logout");
            else
                Logger.log("slave doing the logout");
            try
            {
                commService.logout();
                sleep(5000L);
            }
            catch(Exception exception)
            {
                Logger.log((new StringBuilder()).append("Could not logout: ").append(exception.toString()).toString());
            }
        }
        try
        {
            if(stsession != null)
            {
                stsession.stop();
                stsession.unloadSession();
                Logger.log("Sametime Session unloaded");
            }
        }
        catch(Exception exception1)
        {
            Logger.log((new StringBuilder()).append("Could not logout:Could not unload session object ").append(exception1.toString()).toString());
        }
        thread_complete = true;
    }

    public void stLogin(String s, String s1, String s2)
    {
        commService = (CommunityService)stsession.getCompApi("com.lotus.sametime.community.STBase");
        commService.addLoginListener(this);
        LoginTime = new Date();
        commService.setLoginType((short)4661);
        commService.loginByPassword(s, s1, s2);
        Logger.log((new StringBuilder()).append(st_user).append(" Attempting login to ").append(s).toString());
    }

    
    public void loggedIn(LoginEvent loginevent)
    {
        if(master)
        {
            awarecheck.master_login_complete = true;
            master_le = loginevent;
        } else
        {
            awarecheck.slave_login_complete = true;
            slave_le = loginevent;
        }
        
        LoginCompleteTime = new Date();
        long l = LoginCompleteTime.getTime() - LoginTime.getTime();

        writeStatus("Login", "OK", "User " + st_user + " Logged In to server: " + loginevent.getHost() + "Elapsed Time = " + l);

        try
        {
            try
            {
            	loggedinCount++;
            	for (int i=0; i <= 30; i++){
            		Thread.sleep(1000L);
        			// Both users have successfully logged in... so breaking the sleep now. 
            		if (loggedinCount >= 2)
            			break;
            	}
            }
            catch(InterruptedException interruptedexception) { }
            Logger.log("Register to listen for incoming messages");
            imService = (InstantMessagingService)stsession.getCompApi("com.lotus.sametime.im.ImComp");
            imService.registerImType(1);
            imService.addImServiceListener(this);

            Logger.log("Get a handle to the Lookup Service and add a resolve listener");
            lookupService = (LookupService)stsession.getCompApi("com.lotus.sametime.lookup.LookupComp");
            groupcontent_getter = lookupService.createGroupContentGetter();
            groupcontent_getter.addGroupContentListener(this);
            resolver = lookupService.createResolver(false, false, true, true);
            // -- JT -- Adding Resolver to either resolve a Group or a User. 
            resolver.addResolveListener(this);

            
            awarenessService = (AwarenessService)stsession.getCompApi("com.lotus.sametime.awareness.AwarenessComp");
            awarenessService.addAwarenessServiceListener(this);
            watchList = awarenessService.createWatchList();
            // -- JT -- Adding a Status change Listener..... 
            
            
            watchList.addStatusListener(this);
            Logger.log((new StringBuilder()).append("Name of partner is ").append(MonitorName).toString());
            Group_ResolveTime = new Date();
            
            ResolveTime = new Date();
            ResolveTime = new Date();
            if(!awarecheck.Find_External_User())
            {
            	
                Logger.log((new StringBuilder()).append("Calling resolve for local user: ").append(MonitorName).toString());
                // -- JT -- Resolve just the external user. 
                resolver.resolve(MonitorName);
            } else
            {
                resolved_external_user(MonitorName);
            }
        }
        catch(Exception exception)
        {
            Logger.log((new StringBuilder()).append("Exception caught - ").append(exception.getMessage()).toString());
        }
    }

    public void groupContentQueried(GroupContentEvent groupcontentevent)
    {
        STUSERS = groupcontentevent.getGroupContent();
        if(STUSERS.length == 0)
            Logger.log("ERROR: group content returned zero members");
        Logger.log("Group resolved and successfully queried");
    }

    public void queryGroupContentFailed(GroupContentEvent groupcontentevent)
    {
        Logger.log("ERROR: Group could not be resolved");
        resolver.resolve(MonitorName);
    }

    public void serviceAvailable(AwarenessServiceEvent awarenessserviceevent)
    {
        watchList = awarenessService.createWatchList();
        watchList.addStatusListener(this);
    }

    public void userStatusChanged(StatusEvent statusevent)
    {
        if(master)
            Logger.log("master: we are checking a user status change");
        else
            Logger.log("Slave: we are checking a user status change");
        if(!awarecheck.aware_iscomplete())
        {
            STWatchedUser astwatcheduser[] = (STWatchedUser[])statusevent.getWatchedUsers();
            for(int i = 0; i < astwatcheduser.length; i++)
            {
                String s = astwatcheduser[i].getStatus().getStatusDescription();
                STUserStatus stuserstatus = astwatcheduser[i].getStatus();
                // Should we check for Status change Event ? 
                
                Logger.log((new StringBuilder()).append("User status change for ").append(astwatcheduser[i].getDisplayName()).toString());
                Logger.log((new StringBuilder()).append("status changed was ").append(stuserstatus.getStatusType()).toString());
                Logger.log((new StringBuilder()).append("description changed was ").append(s).toString());
                
                writeStatus("StatusChange", "OK", "User status changed for " + astwatcheduser[i].getDisplayName() + " to " + stuserstatus.getStatusType());

                imService = (InstantMessagingService)stsession.getCompApi("com.lotus.sametime.im.ImComp");
                if(master && !IM_tag_started && astwatcheduser[i].getStatus().getStatusType() == 32)
                {
                    IM_tag_started = true;
                    int j = 1;
                    EncLevel enclevel = EncLevel.ENC_LEVEL_NONE;
                    if(preferloginid)
                        Logger.log("Opening IM with preferloginid");
                    Im im = imService.createIm(astwatcheduser[i], enclevel, j, preferloginid);
                    im.addImListener(this);
                    Logger.log("master now sending IM to slave");
                    im.open();
                    try
                    {
                        Thread.sleep(5000L);
                    }
                    catch(InterruptedException interruptedexception) { }
                    if(awarecheck.aware_iscomplete())
                    {
                        Logger.log("Master now closing IM to slave");
                        im.close(0);
                    }
                }
                if(astwatcheduser[i].getStatus().getStatusType() != 0 || !IM_tag_started)
                    continue;
                Logger.log((new StringBuilder()).append("User not active :status was ").append(astwatcheduser[i].getStatus().getStatusType()).toString());
                Logger.log((new StringBuilder()).append(astwatcheduser[i].getDisplayName()).append(" is offline").toString());
                Logger.log("Remote user and status can't change for some reason");
            }

        }
    }

    public void serviceUnavailable(AwarenessServiceEvent awarenessserviceevent)
    {
    }

    public void loggedOut(LoginEvent loginevent)
    {
        Logger.log((new StringBuilder()).append(st_user).append(" Logged out from server ").append(loginevent.getHost()).toString());
        try
        {
            commService.removeLoginListener(this);
            resolver.removeResolveListener(this);
            awarenessService.removeAwarenessServiceListener(this);
        }
        catch(Exception exception)
        {
        	//writeStatus("Login", "Failure", " Login failure for user: " + st_user);
            //System.out.println((new StringBuilder()).append("Failed connecting to Sametime host: ").append(loginevent.getHost()).toString());
        }
        if(!awarecheck.aware_iscomplete())
        {
        	writeStatus("Login", "Failure", " Login failure for user: " + st_user);
        	// System.out.println("<Status>Failure</Status>");
        	// Write an insert statement that the login failure has happened.
            Logger.log((new StringBuilder()).append("Login failure for user: ").append(st_user).append(" on host: ").append(loginevent.getHost()).append(". Unable to complete login due to reason: ").append(loginevent.getReason()).toString());
            awarecheck.set_aware_complete();
        }
    }

    public void imReceived(ImEvent imevent)
    {
        imevent.getIm().addImListener(this);

//        writeStatus("IM", "OK", "IM received from " + imevent.getIm().getPartner().getName()
//    			+ "Text Received was " + imevent.getText());

        if(master)
        {
            Logger.log((new StringBuilder()).append("Master received IM from ").append(imevent.getIm().getPartner().getName()).toString());
            Logger.log((new StringBuilder()).append("Text received was ").append(imevent.getText()).toString());
            //awarecheck.set_aware_complete();
        } else
        {
            Logger.log((new StringBuilder()).append("Slave received IM received from ").append(imevent.getIm().getPartner().getName()).toString());
            Logger.log((new StringBuilder()).append("Text received was ").append(imevent.getText()).toString());
            //awarecheck.set_aware_complete();
        }
    }

    public void dataReceived(ImEvent imevent)
    {
    	writeStatus("IM", "OK", "IM received from " + imevent.getIm().getPartner().getName()
    			+ "Text Received was " + imevent.getText());
        if(master)
        {
            Logger.log((new StringBuilder()).append("Master received Data from ").append(imevent.getIm().getPartner().getName()).toString());
        } else
        {
            Logger.log((new StringBuilder()).append("Slave received Data received from ").append(imevent.getIm().getPartner().getName()).toString());
            imevent.getIm().sendText(false, "Sending IM");
        }
    }

    public void imClosed(ImEvent imevent)
    {
        if(awarecheck.aware_iscomplete())
            return;
        if(master)
            Logger.log((new StringBuilder()).append("In Master; IM Closed from ").append(imevent.getIm().getPartner().getName()).toString());
        else
            Logger.log((new StringBuilder()).append("In Slave; IM Closed from ").append(imevent.getIm().getPartner().getName()).toString());
        imevent.getIm().removeImListener(this);
        if(!awarecheck.aware_iscomplete())
        {
            Logger.log((new StringBuilder()).append("IM's no longer able to reach ").append(imevent.getIm().getPartner().getName()).toString());
            awarecheck.set_aware_complete();
        }
    }

    public void imOpened(ImEvent imevent)
    {
        Logger.log((new StringBuilder()).append("IM Opened to ").append(imevent.getIm().getPartner().getName()).toString());
        try
        {
            Logger.log("Sleep for 10 seconds before an IM is sent");
        	Thread.sleep(10000L);
        }
        catch(InterruptedException interruptedexception) { }
        Logger.log("First IM sent to User #2");
        IM_SendDate = new Date();
        imevent.getIm().sendText(false, "Sending IM ");
        master_IM_count++;
    }

    public void openImFailed(ImEvent imevent)
    {
        if(debug)
            Logger.log((new StringBuilder()).append("IM Open Failed ").append(imevent.toString()).toString());
        writeStatus("IM", "Failed", "IM Open Failed ");
        //sysOut((new StringBuilder()).append("IM Open Failed ").append(imevent.toString()).toString(), debug);
        Logger.log((new StringBuilder()).append("failed to deliver IM to: ").append(imevent.getIm().getPartner().getName()).toString());
        Logger.log((new StringBuilder()).append("failed to deliver Reason: ").append(imevent.getReason()).toString());
        Logger.log((new StringBuilder()).append("failed to deliver Reason text : ").append(imevent.getText()).toString());
        awarecheck.set_aware_complete();
    }

    public void textReceived(ImEvent imevent)
    {
        awareness_listening = true;
        if(awarecheck.aware_iscomplete())
            return;
        IM_tag_started = true;
        IM_ReceiveDate = new Date();
        long l = IM_ReceiveDate.getTime() - IM_SendDate.getTime();
        
        writeStatus("IM", "OK", "Text received from " + imevent.getIm().getPartner().getName() 
        		+ " Text was " + imevent.getText());
        Logger.log((new StringBuilder()).append("Elasped time to receive the txt was ").append(l).toString());
        if(IM_timeout <= l)
        {
            Logger.log((new StringBuilder()).append("User: ").append(st_user).append(" reporting: Message from user: ").append(imevent.getIm().getPartner().getName()).append(" took (milliseconds)").append(l).append(".  Threshold was defined at : ").append(IM_timeout).append(". IM was delivered from : ").append(MonitorName).toString());
            Logger.log("issuing the close from text received");
            imevent.getIm().close(0);
            awarecheck.set_aware_complete();
        }
        if(master)
        {
            master_IM_count++;
            Logger.log((new StringBuilder()).append("Master Text received from ").append(imevent.getIm().getPartner().getName()).toString());
            Logger.log((new StringBuilder()).append("Text was ").append(imevent.getText()).toString());
            if(!imevent.getText().startsWith("Sending IM"))
            {
                Logger.log((new StringBuilder()).append("User: ").append(st_user).append(" reporting: Message from partner: ").append(imevent.getIm().getPartner().getName()).append(" not delivered correctly, return was ").append(imevent.getText()).toString());
                awarecheck.set_aware_complete();
            }
            try
            {
                String s = awarecheck.FindIM_Interval();
                Integer integer = new Integer(s);
                for(int i = 1; i <= integer.intValue(); i++)
                    sleep(1000L);

            }
            catch(InterruptedException interruptedexception) { }
            Logger.log("IM sent to User #2");
            IM_SendDate = new Date();
            imevent.getIm().sendText(false, (new StringBuilder()).append("Sending IM with sequence # ").append(master_IM_count).toString());
            if(awarecheck.aware_iscomplete())
            {
                Logger.log("issuing the close from text received");
                imevent.getIm().close(0);
            }
        } else
        {
            slave_IM_count++;
            Logger.log((new StringBuilder()).append("Slave Text received from ").append(imevent.getIm().getPartner().getName()).toString());
            Logger.log((new StringBuilder()).append("Text was ").append(imevent.getText()).toString());
            if(imevent.getText().startsWith("Sending IM with sequence # 2"))
            {
            	//System.out.println("All tests passed successfully");
            	writeStatus("Awareness","OK", "All tests Passed Successfully");
                Logger.log((new StringBuilder()).append("User: ").append(st_user).append(" reporting: Message from partner: ").append(imevent.getIm().getPartner().getName()).append(" not delivered correctly, return was ").append(imevent.getText()).toString());
                awarecheck.set_aware_complete();
            }
            try
            {
                String s1 = awarecheck.FindIM_Interval();
                Integer integer1 = new Integer(s1);
                for(int j = 1; j <= integer1.intValue(); j++)
                    sleep(1000L);

            }
            catch(InterruptedException interruptedexception1) { }
            Logger.log("IM sent to User #1");
            IM_SendDate = new Date();
            imevent.getIm().sendText(false, imevent.getText());
        }
        if(awarecheck.aware_iscomplete())
        {
            Logger.log("issuing the close from text received");
            imevent.getIm().close(0);
        }
    }

    public static void writeStatus(String task, String status,String reason){
    	System.out.println("<AttributeSet>");
    	System.out.println("	<Name>" + task + "</Name>");
    	System.out.println("	<Reason>" + reason + "</Reason>");
    	System.out.println("	<Status>" + status + "</Status>");
    	System.out.println("</AttributeSet>");
    }
    public void resolveConflict(ResolveEvent resolveevent)
    {
        if(debug)
            Logger.log("Notification not sent to user");
        Logger.log("resolve conflict");
        String as[] = resolveevent.getFailedNames();
        for(int i = 0; i < as.length; i++)
            Logger.log((new StringBuilder()).append("Notification thread could not resolve user: ").append(as[i]).toString());

        awarecheck.set_aware_complete();
    }

    public void resolved_external_user(String s)
    {
        Logger.log("User is external to community");
        imService = (InstantMessagingService)stsession.getCompApi("com.lotus.sametime.im.ImComp");
        STId stid = new STId(s, "");
        STUser stuser = new STUser(stid, s, "");
        stuser.setExternalUser();
        Logger.log("External user, no reason to resolve name");
        watchList.addItem(stuser);
        if(master)
            Logger.log("Master 60 sleeping in resolved_external_user method to allow both logins to complete");
        else
            Logger.log("Slave 60 sleeping in resolved_external_user method to allow both logins to complete");
        try
        {
            Thread.sleep(60000L);
        }
        catch(InterruptedException interruptedexception) { }
        try
        {
            if(master)
            {
                Logger.log("Master Changing status to active");
//                STUserStatus stuserstatus = new STUserStatus(STUserStatus.ST_USER_STATUS_AWAY, 0, "The Awareness Bot is online!");
//                master_le.getLogin().changeMyStatus(stuserstatus);
                
                STUserStatus stuserstatus = new STUserStatus(STUserStatus.ST_USER_STATUS_ACTIVE, 0, "The Awareness Bot is online!");
                master_le.getLogin().changeMyStatus(stuserstatus);

                Logger.log((new StringBuilder()).append("status now set to ").append(master_le.getLogin().getMyStatus().getStatusType()).toString());
            } else
            {
                Logger.log("Slave status to active");
                STUserStatus stuserstatus1 = new STUserStatus((short)32, 0, "The Awareness Bot is online!");
                slave_le.getLogin().changeMyStatus(stuserstatus1);
                Logger.log((new StringBuilder()).append("status now set to ").append(slave_le.getLogin().getMyStatus().getStatusType()).toString());
            }
            Thread.sleep(10000L);
        }
        catch(Exception exception)
        {
            Logger.log((new StringBuilder()).append("Exception caught - ").append(exception.getMessage()).toString());
        }
    }

    public void resolved(ResolveEvent resolveevent)
    {
        STUser stuser = null;
        if(resolveevent.getResolved() instanceof STGroup)
        {
            Group_ResolveCompleteTime = new Date();
            Logger.log("Group name was used");
            STGroup stgroup = (STGroup)resolveevent.getResolved();
            String s = stgroup.getName();
            long l1 = Group_ResolveCompleteTime.getTime() - Group_ResolveTime.getTime();
            Logger.log((new StringBuilder()).append("Elasped time to resolve name was ").append(l1).append(" Name was  ").append(stgroup).toString());
            if(Resolve_threshold < l1)
            {
                Logger.log((new StringBuilder()).append("Warning: Elasped time to resolve name: ").append(stgroup).append(" was ").append(l1).toString());
            }
        } else
        {
            ResolveCompleteTime = new Date();
            stuser = (STUser)resolveevent.getResolved();
            watchList.addItem(stuser);
            String s1 = stuser.getName();
        }
        if(resolveevent.getResolved() instanceof STGroup)
        {
            groupcontent_getter.queryGroupContent((STGroup)resolveevent.getResolved());
            return;
        }
        Logger.log("We resolved the name");
        long l = ResolveCompleteTime.getTime() - ResolveTime.getTime();
        Logger.log((new StringBuilder()).append("Elasped time to resolve name was ").append(l).append(" Name was  ").append(stuser).toString());
        writeStatus("Resolve","OK", "Elasped time to resolve name was " + l + " milliseconds");
        //  System.out.println((new StringBuilder()).append("Elasped time to resolve name was ").append(l).append(" Name was  ").append(stuser).toString());
        if(Resolve_threshold < l)
        {
            Logger.log((new StringBuilder()).append("Warning: Elasped time to resolve name: ").append(stuser).append(" was ").append(l).toString());
        }
        if(master)
            Logger.log("Master sleeping 30 seconds in LoggedIn method to allow both logins to complete");
        else
            Logger.log("Slave sleeping 30 seconds in LoggedIn method to allow both logins to complete");
//   	try
        {
//            for(int i = 0; i < 30; i++)
//                sleep(1000L);

        }
       // catch(InterruptedException interruptedexception) { }
        try
        {
            if(master)
            {
                Logger.log("Master Changing status to active in Resolved ");
                //STUserStatus stuserstatus = new STUserStatus(STUserStatus.ST_USER_STATUS_AWAY, 0, "The Awareness Bot is online!");
                //master_le.getLogin().changeMyStatus(stuserstatus);
                
                STUserStatus stuserstatus = new STUserStatus(STUserStatus.ST_USER_STATUS_ACTIVE, 0, "The Awareness Bot is online!");
                master_le.getLogin().changeMyStatus(stuserstatus);

                
                Logger.log((new StringBuilder()).append("status now set to ").append(master_le.getLogin().getMyStatus().getStatusType()).toString());
            } else
            {
                Logger.log("Slave Changing status to active in Resolved " );
                STUserStatus stuserstatus1 = new STUserStatus(STUserStatus.ST_USER_STATUS_ACTIVE, 0, "The Awareness Bot is online!");
                slave_le.getLogin().changeMyStatus(stuserstatus1);
                Logger.log((new StringBuilder()).append("status now set to ").append(slave_le.getLogin().getMyStatus().getStatusType()).toString());
            }
        }
        catch(Exception exception)
        {
            Logger.log((new StringBuilder()).append("Exception caught - ").append(exception.getMessage()).toString());
        }
    }

    public void resolveFailed(ResolveEvent resolveevent)
    {
        String as[] = resolveevent.getFailedNames();
        for(int i = 0; i < as.length; i++)
        {
            Logger.log((new StringBuilder()).append("Notification thread could not resolve user: ").append(as[i]).toString());
            //sysOut((new StringBuilder()).append("Notification thread could not resolve user: ").append(as[i]).toString(),false);
            writeStatus("Resolve", "Failed", " Couldnot resolve name ");
        }

        awarecheck.set_aware_complete();
    }

    public void groupCleared(StatusEvent statusevent)
    {
    }

    protected Thread engine;
    protected static boolean debug = true;
    protected boolean running;
    protected static final String STDPING = "Do as I command";
    protected static final String STDRESPONSE = "Sending IM";
    protected static final String STATUSMSG = "The Awareness Bot is online!";
    protected STSession stsession;
    protected Login login;
    protected CommunityService commService;
    protected InstantMessagingService imService;
    protected LookupService lookupService;
    protected Resolver resolver;
    protected AwarenessService awarenessService;
    protected WatchList watchList;
    STObject STUSERS[];
    String st_server;
    String st_user;
    String st_pw;
    String MonitorName;
    int error_index;
    String outputfile;
    String users_to_notify[];
    String splitter;
    String user_names[];
    int name_count;
    int im_sent;
    boolean master;
    boolean thread_complete;
    boolean awareness_listening;
    int master_IM_count;
    int slave_IM_count;
    boolean user_added_to_watch_list;
    boolean IM_tag_started;
    boolean preferloginid;
    protected GroupContentGetter groupcontent_getter;
    static Date IM_SendDate;
    static Date IM_ReceiveDate;
    static int loggedinCount;
    long IM_timeout;
    long Login_threshold;
    long Login_timeout;
    Date LoginTime;
    Date LoginCompleteTime;
    Date ResolveTime;
    Date ResolveCompleteTime;
    Date Group_ResolveTime;
    Date Group_ResolveCompleteTime;
    long Resolve_threshold;
    boolean ignore_first_status_check;
    LoginEvent master_le;
    LoginEvent slave_le;

}
