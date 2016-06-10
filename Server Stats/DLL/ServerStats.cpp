// ServerStats.cpp : Defines the initialization routines for the DLL.
//

#include "stdafx.h"
#include "ServerStats.h"
#include "atlconv.h"
#include "exception"
#include <windows.h>			// for GetTickCount

/* Notes API include files */

#include <global.h>
#include <nsfdb.h>
#include <stats.h>
#include <srverr.h>
#include <ns.h>
#include <osmem.h>
#include <osmisc.h>
#include <kfm.h>				// required for SECKFMSwitchToIDFile

using namespace std;
const MAXLENGTH = 255;

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


//-------- Default VC DLL code

// CServerStatsApp
BEGIN_MESSAGE_MAP(CServerStatsApp, CWinApp)
END_MESSAGE_MAP()

// CServerStatsApp construction
CServerStatsApp::CServerStatsApp()
{
	// TODO: add construction code here,
	// Place all significant initialization in InitInstance
}


// The one and only CServerStatsApp object
CServerStatsApp theApp;

// CServerStatsApp initialization
BOOL CServerStatsApp::InitInstance()
{
	CWinApp::InitInstance();
	return TRUE;
}

//-------- Default Windows DLL code

/*----------
  InitNotes
  ----------
	1. Initialise Notes environment
	2. Switch to specified user id

Parameters:
	char *pNotesProgDir			Full path to the Notes program file directory
	char *pNotesINIFullPath		Full path to the Notes.ini file. MUST be preceeded by "=", eg "=c:\lotus\notes\notes.ini"
	char* pUserIdFilename		Full path to user id file eg c:\lotus\notes\data\JoBloggs.id"
	char * pPassword			Password in plain text

----------*/
BSTR InitNotes( char *pNotesProgDir, char *pNotesINIFullPath, char* pUserIdFilename, char * pPassword )
{
	char  szErrorString[MAXLENGTH];		// buffer for any errors.
	char *argv[2];						// array for args to pass to NotesInitExtended
	char username[MAXLENGTH];			// not actually used but buffer required by api function
	CString csResults;					// results string to return to caller

	try{
		argv[0] = pNotesProgDir;
		argv[1] = pNotesINIFullPath;

		if( STATUS nError = NotesInitExtended( 2, argv ) == NOERROR )		// If Init notes ok
		{
			if( nError = SECKFMSwitchToIDFile(									// switch to specified id file
				pUserIdFilename,
				pPassword,
				username,
				MAXLENGTH,
				fKFM_switchid_DontSetEnvVar,
				NULL)  == NOERROR ){

					csResults = "*SUCCESS*";											// if that worked then we are ok to proceed

				}else{															// otherwise return error from SECKFMSwitchToIDFile
					OSLoadString(NULL, ERR(nError),	szErrorString, MAXLENGTH-1);
					csResults.Format("*ERROR*SECKFMSwitchToIDFile failed with error:(%d) %s\n",nError, szErrorString);
				}
		} else {															// otherwise return error from NotesInitExtended
			OSLoadString(NULL, ERR(nError),	szErrorString, MAXLENGTH-1);
			csResults.Format("*ERROR*NotesInitExtended failed with error:(%d) %s\n",nError, szErrorString);
		}

	// Catch and process any random errors.
	} catch (STATUS nError){
		OSLoadString(NULL, ERR(nError),	szErrorString, MAXLENGTH-1);
		csResults.Format("*ERROR*Start_Notes failed with unexpected error:(%d) %s\n",nError, szErrorString);
	} catch (exception& e){
		csResults.Format("*ERROR*GetServerStat failed with unexpected error: %s\n", e.what());
	}

	return csResults.AllocSysString();			// return string to caller.

}


/*-------------
  GetServerStat
  -------------
	1. Ping Domino server to test connection
	2. Get specified statistics from Domino server

Parameters:
	char* pServerName			Full name of the Domino server eg "Server1/Marketing/Somecompany"
	char* pFacility				Name of the statistics facility eg "Server"
	char* pStat					Name of the statistic within the facility to return eg "Name"
								Leaving the this blank will cause all stats to be returned for the facility
----------*/
BSTR GetServerStat(char* pServerName, char* pFacility, char* pStat){

	// Query statistics from the Domino server using server name

	HANDLE hStatBuffer = NULL;
	DWORD dwStatBufferLen;
	char  szErrorString[MAXLENGTH];
	CString csResults;

	try{
		// ping the server to see if it is up
		// NSFGetServerStats is not always happy when we try to get stats from a server that it can't connect to
		STATUS nError = NSPingServer(pServerName, (DWORD *)NULL, (HANDLE *)NULL);
		if (nError || ERR(nError) == ERR_SERVER_UNAVAILABLE) {
			OSLoadString(NULL, ERR(nError),	szErrorString, MAXLENGTH-1);

			csResults.Format("*ERROR*NSPingServer failed on server:%s with rc:%d, Message:%s\n", pServerName,nError, szErrorString);

			// ** code can exit here ***
			return csResults.AllocSysString();				// return BSTR string with error to caller
			//**
		}

		// get the statistics from the Domino server
		if (nError = NSFGetServerStats(pServerName, pFacility, pStat, &hStatBuffer, &dwStatBufferLen) == NOERROR)
		{
			//'Lock memory to get pointer to buffer
			char* ptrBuffer = (char *) OSLockObject(hStatBuffer);

			// Copy buffer contents into CString for return to caller
			csResults = CString(ptrBuffer, dwStatBufferLen);

			return csResults.AllocSysString();
		}else{
			OSLoadString(NULL, ERR(nError),	szErrorString, MAXLENGTH-1);
			csResults.Format("*ERROR*NSFGetServerStats failed on server:%s with rc:%d, Message:%s\n", pServerName, nError, szErrorString);
		}

		//// free the memory that was allocated
		if (hStatBuffer) {
			OSUnlock (hStatBuffer);
			OSMemFree (hStatBuffer);
			hStatBuffer = NULL;
		}

	// Catch and process any random errors.
	} catch (exception& e){
		csResults.Format("*ERROR*GetServerStat failed with unexpected error: %s\n", e.what());
	} catch (STATUS nError){
		OSLoadString(NULL, ERR(nError),	szErrorString, MAXLENGTH-1);
		csResults.Format("*ERROR*GetServerStat failed with unexpected Notes error:(%d) %s\n",nError, szErrorString);
	}

	return csResults.AllocSysString();				// return BSTR string to caller.
}

/*----------------
  ResponseTime
  ----------------
	1. Get latency statistics from Domino server

	- Some Notes forums mention this call returning strange numbers in ServerToClient value
	- This may have to do with server timeouts.

Parameters:
	char* pServerName			Full name of the Domino server eg "Server1/Marketing/Somecompany"

----------*/
BSTR ResponseTime(char* pServerName){
	char  szErrorString[MAXLENGTH];			//  buffer to hold an errors
	CString csResults;						// result string to return to caller

	try{
		long int before = GetTickCount();		// get start time

		STATUS nError = NSPingServer(pServerName, (DWORD *)NULL, (HANDLE *)NULL);
		if (nError == NOERROR) {
			long int after = GetTickCount();
			csResults.Format("%ld", (after-before) );
		}else{
			OSLoadString(NULL, ERR(nError),	szErrorString, MAXLENGTH-1);
			csResults.Format("*ERROR*Failed getting response time (NSPingServer) from server:%s with rc:%d, Message:%s\n", pServerName,nError, szErrorString);
		}

	// Catch and process any random errors.
	} catch (exception& e){
		csResults.Format("*ERROR*GetServerStat failed with unexpected error: %s\n", e.what());
	} catch (STATUS nError){
		OSLoadString(NULL, ERR(nError),	szErrorString, MAXLENGTH-1);
		csResults.Format("*ERROR*GetServerStat failed with unexpected Notes error:(%d) %s\n",nError, szErrorString);
	}

	return csResults.AllocSysString();					// return BSTR string to caller.

}
