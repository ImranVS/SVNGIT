
#if defined(OS400) && (__OS400_TGTVRM__ >= 510)
#pragma datamodel(P128)
#endif

/*********************************************************************/
/*                                                                   */
/* Licensed Materials - Property of IBM                              */
/*                                                                   */
/* L-GHUS-6ENK4N, L-GHUS-6ENJVF                                      */
/* (C) Copyright IBM Corp. 1995, 2005  All Rights Reserved           */
/*                                                                   */
/* US Government Users Restricted Rights - Use, duplication or       */
/* disclosure restricted by GSA ADP Schedule Contract with           */
/* IBM Corp.                                                         */
/*                                                                   */
/*********************************************************************/




#ifndef AGENTS_ERR_DEFS
#define AGENTS_ERR_DEFS

#define ERR_QUERY_BADDATATYPE	PKG_AGENTS+1
	errortext(ERR_QUERY_BADDATATYPE,	"Unable to load saved search; search is corrupt.")
#define ERR_QUERY_BADDATA		PKG_AGENTS+2
	errortext(ERR_QUERY_BADDATA,	"Unable to load saved search; data is corrupt.")
#define ERR_QUERY_UNKNOWNTERM	PKG_AGENTS+3
	errortext(ERR_QUERY_UNKNOWNTERM,	"This search cannot be recognized by this version of Lotus Notes.")
#define ERR_ACTION_UNKNOWNTERM	PKG_AGENTS+4
	errortext(ERR_ACTION_UNKNOWNTERM,	"This action cannot be recognized by this version of Lotus Notes.")
#define ERR_ACTION_BADDATA		PKG_AGENTS+5
	errortext(ERR_ACTION_BADDATA,	"Unable to load saved action; action is corrupt.")
#define ERR_ASSISTANT_BADDATA	PKG_AGENTS+6
	errortext(ERR_ASSISTANT_BADDATA,	"Unable to load agent; data is corrupt.")
#define STR_QUERY_KEYWORDS		PKG_AGENTS+7
	stringtext(STR_QUERY_KEYWORDS,		"AND OR ACCRUE CONTAINS FIELD NEAR NOT PARAGRAPH SENTENCE TOPIC TERMWEIGHT EXACTCASE VERITYQUERY")
#define STR_ACTION_REPLYTEXT	PKG_AGENTS+8
	stringtext(STR_ACTION_REPLYTEXT,	"Reply to sender")
#define STR_ACTION_MODIFYFIELD	PKG_AGENTS+9
	stringtext(STR_ACTION_MODIFYFIELD,	"Modify field '%s': Set to '%s'")
#define ERR_ACTION_NONAMETOREPLYTO	PKG_AGENTS+10
	errortext(ERR_ACTION_NONAMETOREPLYTO,	"The agent could not reply to the document because the author of the document is unknown.")
#define STR_ACTION_RE			PKG_AGENTS+11
	stringtext(STR_ACTION_RE,	"Re: ")
#define ERR_ACTION_NO_MATCH		PKG_AGENTS+12
	errortext(ERR_ACTION_NO_MATCH,	"Unable to send mail to %s, no match found in Name & Address Book(s)")
#define ERR_ACTION_AMBIG_MATCH	PKG_AGENTS+13
	errortext(ERR_ACTION_AMBIG_MATCH,	"Unable to send mail to %s, multiple matches found in Name & Address Book(s)")
#define STR_ACTION_REPLYFORWARD	PKG_AGENTS+14
	stringtext(STR_ACTION_REPLYFORWARD,	"In response to:")
#define STR_ACTION_FORMULA		PKG_AGENTS+15
	stringtext(STR_ACTION_FORMULA,	"Run @function formula")
#define STR_ACTION_LOTUSSCRIPT	PKG_AGENTS+16
	stringtext(STR_ACTION_LOTUSSCRIPT,	"Run LotusScript")
#define STR_ACTION_SENDMAIL	PKG_AGENTS+17
	stringtext(STR_ACTION_SENDMAIL,	"Send Mail")
#define ERR_QUERY_INVALIDSETTINGS	PKG_AGENTS+18
	errortext(ERR_QUERY_INVALIDSETTINGS,	"Saved search is invalid.")
#define ERR_ACTION_BADDATATYPE		PKG_AGENTS+19
	errortext(ERR_ACTION_BADDATATYPE,	"Unable to load saved action; action is corrupt.")
#define STR_ACTION_COPYDBLOCAL		PKG_AGENTS+20
	stringtext(STR_ACTION_COPYDBLOCAL,	"Copy document to %s")
#define STR_ACTION_COPYDBREMOTE		PKG_AGENTS+21
	stringtext(STR_ACTION_COPYDBREMOTE,	"Copy document to %s on %s")
#define STR_ACTION_MOVEDBLOCAL		PKG_AGENTS+22
	stringtext(STR_ACTION_MOVEDBLOCAL,	"Move document to %s")
#define STR_ACTION_MOVEDBREMOTE		PKG_AGENTS+23
	stringtext(STR_ACTION_MOVEDBREMOTE,	"Move document to %s on %s")
#define STR_ACTION_DELETE			PKG_AGENTS+24
	stringtext(STR_ACTION_DELETE,	"Delete document")
#define STR_LOG_STARTAGENT			PKG_AGENTS+25
	stringtext(STR_LOG_STARTAGENT,	"Started running agent '%s' on %z")
#define STR_LOG_STOPAGENT			PKG_AGENTS+26
	stringtext(STR_LOG_STOPAGENT,	"Done running agent '%s' on %z")
#define STR_LOG_RUNON_ALL			PKG_AGENTS+27
	stringtext(STR_LOG_RUNON_ALL,	"Running on all documents in database: %ld total")
#define STR_LOG_RUNON_NEWMAIL		PKG_AGENTS+28
	stringtext(STR_LOG_RUNON_NEWMAIL,	"Running on new mail messages: %ld total")
#define STR_LOG_RUNON_MODIFIED		PKG_AGENTS+29
	stringtext(STR_LOG_RUNON_MODIFIED,	"Running on new or modified documents: %ld total")
#define STR_LOG_RUNON_NONE			PKG_AGENTS+30
	stringtext(STR_LOG_RUNON_NONE,	"Running with no context")
#define STR_LOG_FTRESULT			PKG_AGENTS+31
	stringtext(STR_LOG_FTRESULT,	"Found %ld document(s) that match search criteria")
#define STR_LOG_REPLY				PKG_AGENTS+32
	stringtext(STR_LOG_REPLY,		"Replied to %ld document(s)")
#define STR_LOG_FORMULA				PKG_AGENTS+33
	stringtext(STR_LOG_FORMULA,		"%ld document(s) were modified by formula")
#define STR_LOG_SENDMAIL			PKG_AGENTS+34
	stringtext(STR_LOG_SENDMAIL,	"Sent mail for %ld document(s)")
#define STR_LOG_DBCOPY				PKG_AGENTS+35
	stringtext(STR_LOG_DBCOPY,		"Copied %ld document(s)")
#define STR_LOG_DELETED				PKG_AGENTS+36
	stringtext(STR_LOG_DELETED,		"Deleted %ld document(s)")
#define STR_LOG_GENERALERROR		PKG_AGENTS+37
	stringtext(STR_LOG_GENERALERROR,	"ERROR: %e")
#define STR_LOG_MODIFYFIELD			PKG_AGENTS+38
	stringtext(STR_LOG_MODIFYFIELD,	"Modified field in %ld document(s)")
#define STR_ACTION_BYFORM			PKG_AGENTS+39
	stringtext(STR_ACTION_BYFORM,	"Modify fields by form")
#define STR_ACTION_MARKREAD			(PKG_AGENTS+40)
	stringtext(STR_ACTION_MARKREAD,	"Mark document read")
#define STR_ACTION_MARKUNREAD		(PKG_AGENTS+41)
	stringtext(STR_ACTION_MARKUNREAD,	"Mark document unread")
#define ERR_UNKNOWN_SEARCHTYPE		(PKG_AGENTS+42)
	errortext(ERR_UNKNOWN_SEARCHTYPE,	"Unknown trigger and search type; agent may be corrupt")
#define STR_QUERY_ETC				(PKG_AGENTS+43)
	stringtext(STR_QUERY_ETC,		"etc...")
#define STR_QUERY_INVALID			(PKG_AGENTS+44)
	stringtext(STR_QUERY_INVALID,	"(Invalid Query)")
#define ERR_INVALID_RUNNOW			(PKG_AGENTS+45)
	errortext(ERR_INVALID_RUNNOW,	"This agent cannot be run manually.  It will only be run when documents are pasted into the database.")
#define STR_LOG_MODIFYBYFORM		(PKG_AGENTS+46)
	stringtext(STR_LOG_MODIFYBYFORM,"Modified fields by form in %ld document(s)")
#define STR_ACTION_MOVETOFOLDER		(PKG_AGENTS+47)
	stringtext(STR_ACTION_MOVETOFOLDER,	"Move document to folder '%s'")
#define STR_ACTION_COPYTOFOLDER		(PKG_AGENTS+48)
	stringtext(STR_ACTION_COPYTOFOLDER,	"Copy document to folder '%s'")
#define STR_ACTION_REMOVEFROMFOLDER	(PKG_AGENTS+49)
	stringtext(STR_ACTION_REMOVEFROMFOLDER,	"Remove document from folder '%s'")
#define STR_ACTION_NEWSLETTER		(PKG_AGENTS+50)
	stringtext(STR_ACTION_NEWSLETTER,	"Send newsletter summary")
#define STR_ACTION_RUNAGENT			(PKG_AGENTS+51)
	stringtext(STR_ACTION_RUNAGENT,	"Run '%s' agent")
#define STR_ACTION_SENDDOCUMENT		(PKG_AGENTS+52)
	stringtext(STR_ACTION_SENDDOCUMENT,	"Send document")
#define STR_QUERY_BYFIELD			(PKG_AGENTS+53)
	stringtext(STR_QUERY_BYFIELD,	"field %s ")
#define STR_AGENT_PROG_RESTRICTIONS	(PKG_AGENTS+54)
	errortext(STR_AGENT_PROG_RESTRICTIONS,		"Programmability Restrictions")



#define STR_QUERY_MISC1				(PKG_AGENTS+55)
	stringtext(STR_QUERY_MISC1,		"date created |date modified |contains any of |contains |does not contain ")
#define IDX_QUERY_BYDATECREATED		1
#define IDX_QUERY_BYDATEMODIFIED	2
#define IDX_QUERY_CONTAINSANY		3
#define IDX_QUERY_CONTAINSALL		4
#define IDX_QUERY_DOESNOTCONTAIN	5

#define STR_LOG_STARTTESTING		(PKG_AGENTS+56)
	stringtext(STR_LOG_STARTTESTING,"The following will occur when this agent is run:")
#define STR_LOG_TESTINGPREFIX		(PKG_AGENTS+57)
	stringtext(STR_LOG_TESTINGPREFIX,	"Testing:  ")
#define ERR_ASSIST_NO_USER			(PKG_AGENTS+58)
	errortext(ERR_ASSIST_NO_USER,	"(must have a user name to open db with access control)")
#define ERR_LOG_NODELETEACCESS		(PKG_AGENTS+59)
	errortext(ERR_LOG_NODELETEACCESS,	"%a is not authorized to delete document %lx")
#define ERR_LOG_NOMODIFYACCESS		(PKG_AGENTS+60)
	errortext(ERR_LOG_NOMODIFYACCESS,	"%a is not authorized to modify document %lx")
#define ERR_BAD_LS_AGENT            (PKG_AGENTS+61)
    errortext(ERR_BAD_LS_AGENT,     "Unable to edit LotusScript agent; action list is corrupt")
#define STR_QUERY_BYAUTHOR			(PKG_AGENTS+62)
	stringtext(STR_QUERY_BYAUTHOR,	"Author ")
#define STR_QUERY_BYFORM			(PKG_AGENTS+63)
	stringtext(STR_QUERY_BYFORM,	"matches example form")
#define STR_LOG_COPYTOFOLDER		(PKG_AGENTS+64)
	stringtext(STR_LOG_COPYTOFOLDER,	"Placed %ld document(s) in folder")
#define STR_LOG_REMOVEFROMFOLDER	(PKG_AGENTS+65)
	stringtext(STR_LOG_REMOVEFROMFOLDER,	"Removed %ld document(s) from folder")
#define STR_LOG_FOLDERNOTFOUND		(PKG_AGENTS+66)
	stringtext(STR_LOG_FOLDERNOTFOUND,	"Folder '%s' could not be found")
#define STR_LOG_AGENTNOTFOUND		(PKG_AGENTS+67)
	stringtext(STR_LOG_AGENTNOTFOUND,	"Agent '%s' could not be found")
#define ERR_ASSIST_NORUNOBJECT		(PKG_AGENTS+68)
	errortext(ERR_ASSIST_NORUNOBJECT,	"Could not find agent run data object")
#define STR_LOG_SENDDOCUMENT		(PKG_AGENTS+69)
	stringtext(STR_LOG_SENDDOCUMENT,	"Sent %ld document(s)")
#define ERR_ASSIST_NOSAMPLENOTE		(PKG_AGENTS+70)
	errortext(ERR_ASSIST_NOSAMPLENOTE,	"Could not execute formula when composing mail: no document found")
#define STR_LOG_BELOWGATHERLIMIT	(PKG_AGENTS+71)
	stringtext(STR_LOG_BELOWGATHERLIMIT,	"Less than %ld document(s) found; not enough to send mail")
#define STR_LOG_VIEWNOTFOUND		(PKG_AGENTS+72)
	stringtext(STR_LOG_VIEWNOTFOUND,	"View '%s' could not be found")
#define ERR_ASSIST_TOOMANYCOLUMNS	(PKG_AGENTS+73)
	stringtext(ERR_ASSIST_TOOMANYCOLUMNS,	"View has too many columns")
#define ERR_ASSIST_BADVIEWFORMAT	(PKG_AGENTS+74)
	stringtext(ERR_ASSIST_BADVIEWFORMAT,	"Bad view format")
#define STR_LOG_SENDNEWSLETTER		(PKG_AGENTS+75)
	stringtext(STR_LOG_SENDNEWSLETTER,	"Sent %ld newsletter(s)")
#define STR_LOG_MAILERROR		(PKG_AGENTS+76)
	stringtext(STR_LOG_MAILERROR,	"Unable to send mail to '%s'")
#define STR_LOG_MARKREAD			(PKG_AGENTS+77)
	stringtext(STR_LOG_MARKREAD,	"Marked %ld document(s) read")
#define STR_LOG_MARKUNREAD			(PKG_AGENTS+78)
	stringtext(STR_LOG_MARKUNREAD,	"Marked %ld document(s) unread")
#define STR_LOG_RUNAGENT			(PKG_AGENTS+79)
	stringtext(STR_LOG_RUNAGENT,	"Ran agent on %ld document(s)")
#define STR_LOG_LOTUSSCRIPT			(PKG_AGENTS+80)
	stringtext(STR_LOG_LOTUSSCRIPT,	"Ran LotusScript code")
#define STR_LOG_FORMNOTFOUND		(PKG_AGENTS+81)
	stringtext(STR_LOG_FORMNOTFOUND,	"Unable to find form for document")
#define STR_LOG_AGENTRUNNING		(PKG_AGENTS+82)
	errortext(STR_LOG_AGENTRUNNING,	"Unable to run agent '%s'. This agent is already running")
#define ERR_ACCESS_OLDVERSION		(PKG_AGENTS+83)
	errortext(ERR_ACCESS_OLDVERSION,	"The server is running a previous version of Lotus Notes that does not support private agents")
#define STR_LOG_FORMULAERROR		(PKG_AGENTS+84)
	stringtext(STR_LOG_FORMULAERROR,	"Formula error: %e")
#define STR_QUERY_BYFOLDER			(PKG_AGENTS+85)
	stringtext(STR_QUERY_BYFOLDER,	"In folder '%s'")
#define ERR_CANNOT_QUERY_RICHTEXT	(PKG_AGENTS+86)
	errortext(ERR_CANNOT_QUERY_RICHTEXT,	"Cannot create a formula which references a rich text field")
#define ERR_DOCUMENT_NOT_SAVED		(PKG_AGENTS+87)
	errortext(ERR_DOCUMENT_NOT_SAVED,	"You must save the document before this action can be performed")
#define STR_QUERY_USESFORM			(PKG_AGENTS+88)
	stringtext(STR_QUERY_USESFORM,	"uses '%s' form")
#define STR_ACTION_MODIFYFIELDAPPEND (PKG_AGENTS+89)
	stringtext(STR_ACTION_MODIFYFIELDAPPEND,"Modify field '%s': Append '%s'")
#define ERR_MODIFYFIELD_APPEND		(PKG_AGENTS+90)
	errortext(ERR_MODIFYFIELD_APPEND,	"Append is only valid for text fields.")
#define ERR_ASSISTANT_TIMEOUT			(PKG_AGENTS+91)
	errortext(ERR_ASSISTANT_TIMEOUT,		"Execution time limit exceeded by Agent '%s' in database '%p'. Agent signer '%a'.")
#define ERR_AGENTS_NODOCUMENT			(PKG_AGENTS+92)
	errortext(ERR_AGENTS_NODOCUMENT,		"No document has been selected.")
#define STR_QUERY_TOPIC				(PKG_AGENTS+93)
	stringtext(STR_QUERY_TOPIC,		"Verity Topic Query")
#define STR_LOG_RUNON_PASTED			PKG_AGENTS+94
	stringtext(STR_LOG_RUNON_PASTED,	"Running on documents pasted into database: %ld total")
#define STR_LOG_RUNON_SELECTED			PKG_AGENTS+95
	stringtext(STR_LOG_RUNON_SELECTED,	"Running on selected documents: %ld total")
#define STR_LOG_RUNON_VIEW				PKG_AGENTS+96
	stringtext(STR_LOG_RUNON_VIEW,		"Running on all documents in view: %ld total")
#define STR_LOG_RUNON_UNREAD			PKG_AGENTS+97
	stringtext(STR_LOG_RUNON_UNREAD,	"Running on unread documents in view: %ld total")
#define STR_LOG_AGENT_PRINT_MSG            PKG_AGENTS+98
	stringtext(STR_LOG_AGENT_PRINT_MSG,    "Agent printing: %s")
#define STR_LOG_AGENT_ERROR_MSG            PKG_AGENTS+99
	stringtext(STR_LOG_AGENT_ERROR_MSG,    "Agent %s error: %s")
#define STR_LOG_AGENT_MSGBOX_MSG           PKG_AGENTS+100
	stringtext(STR_LOG_AGENT_MSGBOX_MSG,   "Agent message: %s")
#define STR_AGENTS_V3COMMENT			PKG_AGENTS+101
	stringtext(STR_AGENTS_V3COMMENT,	"This is an agent created in Release 4. Please do not run it or modify it using Release 3.")
#define ERR_ACTION_TEXTNOTSUPPORTED		PKG_AGENTS+102
	errortext(ERR_ACTION_TEXTNOTSUPPORTED,	"Plain text cannot be entered into a simple actions field.")
#define ERR_AGENT_AGENTRUNNING			(PKG_AGENTS+103)
	stringtext(ERR_AGENT_AGENTRUNNING,	"Unable to run agent because the agent is already running")
#define ERR_AGENT_MAILERROR				(PKG_AGENTS+104)
	stringtext(ERR_AGENT_MAILERROR,		"Unable to send mail")
#define ERR_AGENT_NOUICOMMANDS			(PKG_AGENTS+105)
	errortext(ERR_AGENT_NOUICOMMANDS,	"@Command and other UI functions are not allowed with this search type; please select 'None' as your runtime target.")
#define ERR_QUERY_FOLDERNOTFOUND		(PKG_AGENTS+106)
	errortext(ERR_QUERY_FOLDERNOTFOUND,	"Unable to find folder or view")
#define STR_AGENT_ATALL					(PKG_AGENTS+107)
	stringtext(STR_AGENT_ATALL,			";SELECT @All")
#define STR_AGENT_ATALL2	   			(PKG_AGENTS+108)
	stringtext(STR_AGENT_ATALL2,		"SELECT @All")
#define STR_AGENT_NEWSLETTER_ORPHAN		(PKG_AGENTS+109)
	stringtext(STR_AGENT_NEWSLETTER_ORPHAN,	"(This document was not found in the view specified by the agent)")
#define STR_LOG_NONEWSLETTERGATHER		(PKG_AGENTS+110)
	stringtext(STR_LOG_NONEWSLETTERGATHER,	"No newsletter was sent because less than %d documents were found")
#define STR_AGENT_SENTMAIL				(PKG_AGENTS+111)
	stringtext(STR_AGENT_SENTMAIL,		"Mail submitted for delivery.  (1 Person/Group)")
#define STR_AGENT_SENTMAIL2				(PKG_AGENTS+112)
	stringtext(STR_AGENT_SENTMAIL2,		"Mail submitted for delivery.  (%ld People/Groups)")
#define STR_AGENT_RUN_CORRUPT			(PKG_AGENTS+113)
	stringtext(STR_AGENT_RUN_CORRUPT,	"Unable to run this agent; Agent is corrupt - please edit and resave agent")
#define STR_LOG_MOVETOFOLDER			(PKG_AGENTS+114)
	stringtext(STR_LOG_MOVETOFOLDER,	"Moved %ld document(s) to folder")
#define ERR_MAIL_NO_MATCH				(PKG_AGENTS+115)
	errortext(ERR_MAIL_NO_MATCH,		"Unable to send mail; no match found in Name & Address Book(s)")
#define ERR_MAIL_AMBIGUOUS_MATCH		(PKG_AGENTS+116)
	errortext(ERR_MAIL_AMBIGUOUS_MATCH,	"Unable to send mail; multiple matches found in Name & Address Book(s)")
#define ERR_AGENT_RUNCTX_EXTENDED	(PKG_AGENTS+117)
	errortext(ERR_AGENT_RUNCTX_EXTENDED,	"Agent run context must be of extended type to use this call")
#define ERR_AGENT_NOMULT_AGENTRUN	(PKG_AGENTS+118)
	errortext(ERR_AGENT_NOMULT_AGENTRUN,	"Run context cannot be used with more than one agent at the same time")
#define ERR_AGENT_UNKNOWN_REDIR		(PKG_AGENTS+119)
	errortext(ERR_AGENT_UNKNOWN_REDIR,		"Unknown redirection type")
#define ERR_AGENT_UI_TRIGGER		(PKG_AGENTS+120)
	errortext(ERR_AGENT_UI_TRIGGER,			"Unsupported trigger and search in the background or embedded agent")
#define ERR_AMGR_RUN_ACCESS_ERROR           (PKG_AGENTS+121)
	errortext(ERR_AMGR_RUN_ACCESS_ERROR,    "Error validating user's agent execution access")
#define ERR_AGENT_LIBRARY_LOWER_RIGHTS      (PKG_AGENTS+122)
	errortext(ERR_AGENT_LIBRARY_LOWER_RIGHTS,    "Warning: in agent '%s' in database '%p' signed by '%a' calling script library '%s'. The rights of the agent have been lowered to the rights of the script library signer '%a'. %s")
#define ERR_AGENT_SCIPTLIB_FINISHEDUSE      (PKG_AGENTS+123)
	errortext(ERR_AGENT_SCIPTLIB_FINISHEDUSE,    "Security Error in Agent '%s' in database '%p' signed by '%a' calling script library '%s' signed by '%a'. The signer of the script library loaded via 'Execute' cannot have lower rights than the agent signer. %s")
#define STR_ACTION_JAVAAGENT			(PKG_AGENTS+124)
	stringtext(STR_ACTION_JAVAAGENT,	"Run Java Agent")
#define STR_LOG_JAVAAGENT			(PKG_AGENTS+125)
	stringtext(STR_LOG_JAVAAGENT,	"Ran Java Agent Class")
#define ERR_AMGR_DBOPEN_NOTLOCAL		(PKG_AGENT1+14)
	errortext(ERR_AMGR_DBOPEN_NOTLOCAL	,"Cannot open databases on machines other than the server running your program")
#define ERR_AMGR_CONSOLE_CANCEL			(PKG_AGENTS+126)
	stringtext(ERR_AMGR_CONSOLE_CANCEL	,"Agent '%s' in database '%p' has been canceled from server console.")
#define ERR_AMGR_CONSOLE_CANCEL2			(PKG_AGENTS+127)
	stringtext(ERR_AMGR_CONSOLE_CANCEL2	,"Agent has been canceled from server console.")

/* NOTE: Since the AGENTS PKG codes are shared with ASSISTSANT2, 
		we are limited to strings 0 - 127 */ 

/* extended codes to AGENTS2 PKG limited to strings PKG_AGENTS2+0 through PKG_AGENTS2+31 */
#define STR_LOG_SYNCH_NO_EMBED			(PKG_AGENTS2+0)
	errortext(STR_LOG_SYNCH_NO_EMBED,	"'Before mail arrives' agents cannot run other agents")

#define STR_WRONG_SERVER				(PKG_AGENTS2+1) /* inserted into ERR_AMGR_WRONG_SERVER (second param)*/
	stringtext(STR_WRONG_SERVER,	"'%a' not '%a'")
#define ERR_AMGR_WRONG_SERVER			(PKG_AGENTS2+2)
	errortext(ERR_AMGR_WRONG_SERVER,	"AMgr: Agent '%s' will not run. It is intended to run on %s")
/* Available			(PKG_AGENTS2+3) */
#define ERR_AMGR_NOPUBKEY   (PKG_AGENTS2+4)
	errortext(ERR_AMGR_NOPUBKEY, "Document set for JIT encryption and no public key available.")
#define ERR_AMGR_NOMIMESENT   (PKG_AGENTS2+5)
	errortext(ERR_AMGR_NOMIMESENT, "Document set for MIME format and an error occurred during sending or conversion.")
#define STR_LOG_SYNCH_INVALIDOP			(PKG_AGENTS2+6)
	errortext(STR_LOG_SYNCH_INVALIDOP,	"Invalid operation on folder '%s' in 'Before mail delivery' agent. Invalid operation(s) ignored.")
#define ERR_AGENT_CONSOLE   (PKG_AGENTS2+7)
	errortext(ERR_AGENT_CONSOLE, "Error processing your request for %s ")
#define STR_CONSOLE_LOTUSSCRIPT			(PKG_AGENTS2+8)
	stringtext(STR_CONSOLE_LOTUSSCRIPT,	"LotusScript")
#define STR_CONSOLE_JAVA				(PKG_AGENTS2+9)
	stringtext(STR_CONSOLE_JAVA,		"Java")
#define STR_CONSOLE_FORMULA				(PKG_AGENTS2+10)
	stringtext(STR_CONSOLE_FORMULA,		"Formula") 
#define STR_CONSOLE_SIMPLE				(PKG_AGENTS2+11)
	stringtext(STR_CONSOLE_SIMPLE,		"Simple")
#define STR_CONSOLE_INVOKER				(PKG_AGENTS2+12)
	stringtext(STR_CONSOLE_INVOKER,		"Invoker")
#define STR_CONSOLE_SIGNER				(PKG_AGENTS2+13)
	stringtext(STR_CONSOLE_SIGNER,		"Signer")
#define STR_CONSOLE_SHARED				(PKG_AGENTS2+14)
	stringtext(STR_CONSOLE_SHARED,		"Shared")
#define STR_CONSOLE_PRIVATE				(PKG_AGENTS2+15)
	stringtext(STR_CONSOLE_PRIVATE,		"Private")
#define STR_CONSOLE_SHAREDAGENTS			(PKG_AGENTS2+16)
	stringtext(STR_CONSOLE_SHAREDAGENTS,	"Shared agents:")
#define STR_CONSOLE_PRIVATEAGENTS			(PKG_AGENTS2+17)
	stringtext(STR_CONSOLE_PRIVATEAGENTS,	"Private agents:")
#define STR_CONSOLE_AGENT			(PKG_AGENTS2+18)
	stringtext(STR_CONSOLE_AGENT,	"Agent")
#define STR_CONSOLE_SIGNER2			(PKG_AGENTS2+19)
	stringtext(STR_CONSOLE_SIGNER2,	"Signer:\t\t")
#define STR_CONSOLE_RUNAS			(PKG_AGENTS2+20)
	stringtext(STR_CONSOLE_RUNAS,	"Run As:\t\t")
#define STR_CONSOLE_TYPE			(PKG_AGENTS2+21)
	stringtext(STR_CONSOLE_TYPE,	"Type: \t\t")
#define STR_CONSOLE_WEB				(PKG_AGENTS2+22)
	stringtext(STR_CONSOLE_WEB,		"Web")
#define STR_CONSOLE_NOTES			(PKG_AGENTS2+23)
	stringtext(STR_CONSOLE_NOTES,	"Notes")
#define STR_CONSOLE_NONE			(PKG_AGENTS2+24)
	stringtext(STR_CONSOLE_NONE,	"None")
#define STR_CONSOLE_HIDEFROM				(PKG_AGENTS2+25)
	stringtext(STR_CONSOLE_HIDEFROM,		"Hide From:\t")
#define STR_CONSOLE_EFFECTIVEUSER			(PKG_AGENTS2+26)
	stringtext(STR_CONSOLE_EFFECTIVEUSER,	"Effective User:\t")
#define STR_CONSOLE_ONBEHALFUSER			(PKG_AGENTS2+27)
	stringtext(STR_CONSOLE_ONBEHALFUSER,	"On Behalf User:\t")
#define STR_CONSOLE_RESTRICTED				(PKG_AGENTS2+28)
	stringtext(STR_CONSOLE_RESTRICTED,		"Restricted")
#define STR_CONSOLE_UNRESTRICTED			(PKG_AGENTS2+29)
	stringtext(STR_CONSOLE_UNRESTRICTED,	"Unrestricted")
#define STR_CONSOLE_NOACCESS				(PKG_AGENTS2+30)
	stringtext(STR_CONSOLE_NOACCESS,		"No Access")
#define STR_CONSOLE_SCRIPTLIBRARY			(PKG_AGENTS2+31)
	stringtext(STR_CONSOLE_SCRIPTLIBRARY,	"Script Library")

/* extended codes to AGENTS3 PKG limited to strings PKG_AGENTS3+0 through PKG_AGENTS3+47 */
#define STR_CONSOLE_SCRIPTLIBRARIES			(PKG_AGENTS3+0)
	stringtext(STR_CONSOLE_SCRIPTLIBRARIES,	"Script Libraries")
#define STR_CONSOLE_TOTALSHARED				(PKG_AGENTS3+1)
	stringtext(STR_CONSOLE_TOTALSHARED,		"Total number of shared agents: %d")
#define STR_CONSOLE_TOTALSRIPLIB			(PKG_AGENTS3+2)
	stringtext(STR_CONSOLE_TOTALSRIPLIB,	"Total number of script libraries: %d")
#define STR_CONSOLE_TOTALPRIVATE			(PKG_AGENTS3+3)
	stringtext(STR_CONSOLE_TOTALPRIVATE,	"Total number of private agents: %d")
#define STR_CONSOLE_RESTRICTIONS			(PKG_AGENTS3+4)
	stringtext(STR_CONSOLE_RESTRICTIONS,	"Restrictions:\t")
#define STR_CONSOLE_NOTESWEB				(PKG_AGENTS3+5)
	stringtext(STR_CONSOLE_NOTESWEB,		"Notes and Web")
#define ERR_CONSOLE_FAIL_OWNER				(PKG_AGENTS3+6)
	stringtext(ERR_CONSOLE_FAIL_OWNER,		"Encountered error obtaining agent owner for agent '%s'")
/* PKG_AGENTS3+7 - available */
#define ERR_AGENT_SAVE_NOPRIVATE			(PKG_AGENTS3+8)
	errortext(ERR_AGENT_SAVE_NOPRIVATE,	"Private agents cannot be saved by server-based agents")
#define ERR_AGENT_SAVE_NOMATCH				(PKG_AGENTS3+9)
	errortext(ERR_AGENT_SAVE_NOMATCH,		"Effective users of the saved agent and the saving agent must match")
#define ERR_AGENT_SAVE_DIFFONBEHALF			(PKG_AGENTS3+10)
	errortext(ERR_AGENT_SAVE_DIFFONBEHALF,	"The agent being saved contains a conflicting 'On behalf' value")
#define ERR_AGENT_NO_RUNASWEB				(PKG_AGENTS3+11)
	errortext(ERR_AGENT_NO_RUNASWEB,		"An agent invoked via RunOnServer method does not support 'run as we web user' flag")
#define ERR_AGENT_WRONGVERSION				(PKG_AGENTS3+12)
	errortext(ERR_AGENT_WRONGVERSION,		"This agent contains an illegally added 'On behalf' attribute.  To make the agent valid, please remove it.")
#define ERR_AGENT_WRONGVERSION_GENERIC		(PKG_AGENTS3+13)
	errortext(ERR_AGENT_WRONGVERSION_GENERIC,"This version of Lotus Notes does not support agents of this version")
#define ERR_AGENT_INVALID_INVOKER			(PKG_AGENTS3+14)
	errortext(ERR_AGENT_INVALID_INVOKER,	"Invalid invoker category is used.  This category is reserved for Notes core.")	
#define ERR_AGENT_INVALID_WEBUSER			(PKG_AGENTS3+15)
	errortext(ERR_AGENT_INVALID_WEBUSER,	"Saving agent and saved agent have incompatible settings of 'run as web user' flag. ")
#define STR_AGENT_SIMPLE_FORMULA			(PKG_AGENTS3+16)
	errortext(STR_AGENT_SIMPLE_FORMULA,		"Run Simple and Formula agents")
#define ERR_AGENT_RESTRICTED_WEBUSER2			(PKG_AGENTS3+17)
	errortext(ERR_AGENT_RESTRICTED_WEBUSER2,	"Users without rights to sign 'On Behalf' agents cannot sign agents that run as web user unless web user is agent signer.")
#define ERR_AGENT_RESTRICTED_ONBEHALF			(PKG_AGENTS3+18)
	errortext(ERR_AGENT_RESTRICTED_ONBEHALF,	"Users without rights to sign 'On Behalf' agents can only run agents on their own behalf.")

#define STR_AGENT_NOFULLTEXTINDEX			(PKG_AGENTS3+19)
	errortext(STR_AGENT_NOFULLTEXTINDEX,	"Full text operations on database '%p' which is not full text indexed.  This is extremely inefficient.")	
#define ERR_AGENT_CORRUPTE_SIGN				(PKG_AGENTS3+20)
	errortext(ERR_AGENT_CORRUPTE_SIGN,		"Agent '%s' has been corrupted.  Significant fields have been excluded from the signature.")
#define ERR_AGENT_SCRIPTLIBRARY				(PKG_AGENTS3+21)
	errortext(ERR_AGENT_SCRIPTLIBRARY,		"Error in Agent '%s' in database '%p' signed by '%a' calling script library '%s'. Script library signer '%a' does not have proper rights. %s")
#define ERR_AGENT_BADWEDUSER_CONSOLE		(PKG_AGENTS3+22)
	errortext(ERR_AGENT_BADWEDUSER_CONSOLE,		"Agent '%s' contains invalidly modified 'Run as web user' flag. Examine and resave the agent.")
#define ERR_AGENT_BADWEDUSER				(PKG_AGENTS3+23)
	errortext(ERR_AGENT_BADWEDUSER,			"Agent contains invalidly modified 'Run as web user' flag. Examine and resave the agent.")
#define ERR_CANNOT_QUERY_SITEMAP			(PKG_AGENTS3+24)
	errortext(ERR_CANNOT_QUERY_SITEMAP,		"Cannot create a formula which references an Outline")
#define ERR_BACKGROUNDTHREAD_UICOMMAND_CONFLICT		(PKG_AGENTS3+25)
	errortext(ERR_BACKGROUNDTHREAD_UICOMMAND_CONFLICT, "A runtime error will occur if this agent requires user interaction. Interactive agents cannot be run in a background client thread. Do you wish to save?")
#define STR_AGENT_SUPRESS_SIMPLESECURITY	(PKG_AGENTS3+26)
	errortext(STR_AGENT_SUPRESS_SIMPLESECURITY,	"Warning: INI variable is used to suppress expansion of personal agent restrictions list ")
#define ERR_AGENT_RESTRICTED_FULLADMIN		(PKG_AGENTS3+27)
	errortext(ERR_AGENT_RESTRICTED_FULLADMIN,	"Agent '%s': User ('%a') does not have rights to run agents in 'Full Administrator' mode ")				
#define ERR_AGENT_SCRIPTSIG						(PKG_AGENTS3+28)
	errortext(ERR_AGENT_SCRIPTSIG,				"Error in Agent '%s' in database '%p' signed by '%a' calling script library '%s'. Script library signature is corrupted.")
#define ERR_AGENT_DDM_LONG_AMGR					(PKG_AGENTS3+29)
	errortext(ERR_AGENT_DDM_LONG_AMGR,			"%ld minute(s) have elapsed since start of agent '%s' in database '%p'. Threshold level %ld minute(s). Agent Owner: '%a'. ")
#define ERR_AGENT_DDM_BEHINDSCHEDULE			(PKG_AGENTS3+30)
	errortext(ERR_AGENT_DDM_BEHINDSCHEDULE,		"Start of execution for agent '%s' in database '%p' is behind schedule by %ld minutes(s). Threshold level %ld minutes(s). Agent Owner: '%a'.")
#define ERR_AGENT_DDM_MEMORYHOG					(PKG_AGENTS3+31)
	errortext(ERR_AGENT_DDM_MEMORYHOG,			"%s memory usage by agent '%s' in database '%p'. Threshold level %s. Agent Owner: '%a'.")
#define ERR_AGENT_DDM_CPUHOG					(PKG_AGENTS3+32)
	errortext(ERR_AGENT_DDM_CPUHOG,				"%ld seconds CPU usage by agent '%s' in database '%p'. Threshold level %ld seconds. Agent Owner: '%a'.")
#define ERR_AGENT_DDM_NOACCESS					(PKG_AGENTS3+33)
	errortext(ERR_AGENT_DDM_NOACCESS,			"Error validating execution rights for agent '%s' in database '%p'. Agent signer '%a', effective user '%a'. %s")
#define STR_WRONG_FIELD							(PKG_AGENTS3+34)
	errortext(STR_WRONG_FIELD,					"Examine '%s' field in the Server Record.")  
#define STR_AGENT_ACCESS_SERVER					(PKG_AGENTS3+35)
	errortext(STR_AGENT_ACCESS_SERVER,			"Agent signer, '%a', does not have access to this server.")
#define MSG_AMGR_RUN_TIMEOUT 					(PKG_AGENTS3+36)
	errortext(MSG_AMGR_RUN_TIMEOUT,				"Agent execution time limit exceeded.")
#define MSG_AGENT_DESIGNUPDATE_DISABLE 			(PKG_AGENTS3+37)
	errortext(MSG_AGENT_DESIGNUPDATE_DISABLE,	"Agent '%s' in '%p' disabled during Design Update from template '%s'. Agent signer '%a'.")
/* even though the text is the same as ERR_AGENT_DDM_LONG_AMGR, solutions in events4.nsf are different */
#define ERR_AGENT_DDM_LONG_HTTP					(PKG_AGENTS3+38)
	errortext(ERR_AGENT_DDM_LONG_HTTP,			"%ld minute(s) have elapsed since start of agent '%s' in database '%p'. Threshold level %ld minute(s). Agent Owner: '%a'.")
#define STR_AGENT_SIGN_SCRIPTLIB				(PKG_AGENTS3+39)
	errortext(STR_AGENT_SIGN_SCRIPTLIB,			"Sign Script Libraries")
#define STR_AGENT_DDM_CLEAR						(PKG_AGENTS3+40)
	errortext(STR_AGENT_DDM_CLEAR,				"Daily Clearing Event Issued")
#define ERR_AGENT_DDM_OUTOFMEMORY				(PKG_AGENTS3+41)
	errortext(ERR_AGENT_DDM_OUTOFMEMORY,		"Out of memory, agent probe cannot run")


/* extended codes to AGENTS3 PKG limited to strings PKG_AGENTS3+0 through PKG_AGENTS3+47 */
#endif




#if defined(OS400) && (__OS400_TGTVRM__ >= 510)
#pragma datamodel(pop)
#endif

