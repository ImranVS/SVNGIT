package com.rpr.sametime;

import java.io.File;
import java.io.FileWriter;
import java.io.IOException;
import java.io.PrintWriter;
import java.text.SimpleDateFormat;
import java.util.Date;

public class Logger {

	static PrintWriter out;
	static{
		try{
		out = new PrintWriter(new FileWriter(new File("Log_Files\\SametimeLog.txt"),true),true);
		}catch (IOException ioe){
			ioe.printStackTrace();
		}
	}
    public static void log(String message) { 
    	SimpleDateFormat sdf = new SimpleDateFormat("MM/dd/yyyy HH:mm:ss");
        Date resultdate = new Date(System.currentTimeMillis());
        out.println(sdf.format(resultdate) + "  " + message);
    }

    public static void closeLog(){
    	if(out != null)
    		out.close();
    }
    
}