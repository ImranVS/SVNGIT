package com.rpr.sametime;

public class SametimeTest {

	/**
	 * @param args
	 */
	public static void main(String[] args) {
		// TODO Auto-generated method stub
			try {
				int retryInterval = 1;
				// Read the list of Sametime Servers to scan... 
				// Loop through those servers without any wait   
				if (args.length < 6){
					System.out.println("Usage = java SametimeTest  ");
					System.exit(1);
				}else {
					String stSrv1 = args[0];
					String stUsr1 = args[1];
					String stPwd1 = args[2];
					String stSrv2 = args[3];
					String stUsr2 = args[4];
					String stPwd2 = args[5];
					
					awarecheck awcheck = new awarecheck(stSrv1,stUsr1,stPwd1,stSrv2,stUsr2,stPwd2);
					// Everytime we need to set the SameTime Servers 1 & 2 
					// And the 2 Users for doing Ping Pong test. 
					awcheck.start();
					awcheck.join();
				}				
			} catch (Exception ex) {
				ex.printStackTrace();
			}
	}
}
