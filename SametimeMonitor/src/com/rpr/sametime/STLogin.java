package com.rpr.sametime;

import java.util.Date;

import com.lotus.sametime.community.CommunityService;
import com.lotus.sametime.community.LoginEvent;
import com.lotus.sametime.community.LoginListener;
import com.lotus.sametime.core.comparch.DuplicateObjectException;
import com.lotus.sametime.core.comparch.STSession;

public class STLogin implements LoginListener {

	protected CommunityService commService;
	Date LoginTime;
	String st_server;
	String st_user;
	String st_pw;
	Date LoginCompleteTime;
	STSession stsession;

	public STLogin(String st_pw, String st_server, String st_user, STSession stsession) {
		super();
		this.st_pw = st_pw;
		this.st_server = st_server;
		this.st_user = st_user;
		this.stsession = stsession;
	}

	static void sysOut(String s, boolean flag) {
		// if(flag)
		System.out.println(s);
	}

	public void stLogin() {
		try {
			if (stsession != null) {
				stsession.stop();
				stsession.unloadSession();
			} else
				stsession = new STSession("SameTime Login" + st_user);

			stsession.loadSemanticComponents();
			stsession.start();

			commService = (CommunityService) stsession.getCompApi("com.lotus.sametime.community.STBase");
			commService.addLoginListener(this);
			LoginTime = new Date();
			commService.setLoginType((short) 4661);
			commService.loginByPassword(st_server, st_user, st_pw);
			sysOut((new StringBuilder()).append(st_user).append(" Attempting login to ").append(st_server).toString(), true);
			
		} catch (DuplicateObjectException duplicateObjectException) {
			duplicateObjectException.printStackTrace();
		}
	}

	@Override
	public void loggedIn(LoginEvent loginevent) {

		sysOut((new StringBuilder()).append("user: ").append(st_user).append(
				" Logged In to server: ").append(loginevent.getHost())
				.toString(), true);
		LoginCompleteTime = new Date();
		long l = LoginCompleteTime.getTime() - LoginTime.getTime();
		
		
		// Write an insert statement that the server login in is successful and
		// is Up and Running and time took for Login is xyz milliseconds.
		commService.logout();
	}

	@Override
	public void loggedOut(LoginEvent loginevent) {
	    sysOut((new StringBuilder()).append(st_user).append(" Logged out from server ").append(loginevent.getHost() + loginevent.getReason()).toString(), true);
	    
        try
        {
            commService.removeLoginListener(this);
            
			if (stsession != null) {
				stsession.stop();
				stsession.unloadSession();
			} 
        }
        catch(Exception exception)
        {
            System.out.println((new StringBuilder()).append("Failed connecting to Sametime host: ").append(loginevent.getHost()).toString());
        }
	}

}
