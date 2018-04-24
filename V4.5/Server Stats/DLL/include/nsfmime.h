
#if defined(OS400) && (__OS400_TGTVRM__ >= 510)
#pragma datamodel(P128)
#endif

/*********************************************************************/
/*                                                                   */
/* IBM Confidential                                                  */
/*                                                                   */
/* OCO Source Materials                                              */
/*                                                                   */
/* (C) Copyright IBM Corp. 1999, 2002                                */
/*                                                                   */
/* The source code for this program is not published or otherwise    */
/* divested of its trade secrets, irrespective of what has been      */
/* deposited with the U.S. Copyright Office.                         */
/*                                                                   */
/*********************************************************************/

#ifndef _NSFMIME_DEFS_
#define _NSFMIME_DEFS_

#include "nsf.h"

#ifdef __cplusplus 
extern "C" {
#endif


STATUS LNPUBLIC NSFMimePartDelete(NOTEHANDLE hNote, char *pchItemName, WORD wItemNameLen);


STATUS LNPUBLIC NSFMimePartAppend(NOTEHANDLE hNote, char *pchItemName, WORD wItemNameLen,
					WORD wPartType, DWORD dwFlags, char *pchPart, WORD wPartLen);


STATUS LNPUBLIC NSFMimePartGetInfo(NOTEHANDLE hNote, char *pchItemName, WORD wItemNameLen,
					WORD *pwPartType, DWORD *pdwFlags, WORD *pwReserved, WORD *pwPartLen,
					BLOCKID *pbhItem);

STATUS LNPUBLIC NSFMimePartGetInfoNext(NOTEHANDLE hNote, 
					BLOCKID bhItem, char *pchItemName, WORD wItemNameLen,
					WORD *pwPartType, DWORD *pdwFlags, WORD *pwReserved, WORD *pwPartLen,
					BLOCKID *pbhItemNext);

STATUS LNPUBLIC NSFMimePartGetInfoByBLOCKID(BLOCKID bhItem,
					WORD *pwPartType, DWORD *pdwFlags, WORD *pwReserved, 
					WORD *pwPartOffset, WORD *pwPartLen,
					WORD *pwBoundaryOffset, WORD *pwBoundaryLen,
					WORD *pwHeadersOffset, WORD *pwHeadersLen,
					WORD *pwBodyOffset, WORD *pwBodyLen);

STATUS LNPUBLIC NSFMimePartGetPart(BLOCKID bhItem, HANDLE *phPart);

BOOL LNPUBLIC NSFIsFileItemMimePart(NOTEHANDLE hNote, BLOCKID bhFileItem);
BOOL LNPUBLIC NSFIsMimePartInFile(NOTEHANDLE hNote, BLOCKID bhMimeItem, char *pszFileName, WORD wMaxFileNameLen);

STATUS LNPUBLIC NSFMimePartCreateStream(NOTEHANDLE hNote, char *pchItemName, 
					WORD wItemNameLen, WORD wPartType, DWORD dwFlags, HANDLE *phCtx);

STATUS LNPUBLIC NSFMimePartAppendStream(HANDLE hCtx, char *pchData, WORD wDataLen);
STATUS LNPUBLIC NSFMimePartAppendFileToStream(HANDLE hCtx, char *pszFilename);
STATUS LNPUBLIC NSFMimePartAppendObjectToStream(HANDLE hCtx, char *pszObject);
STATUS LNPUBLIC NSFMimePartCloseStream(HANDLE hCtx, BOOL bUpdate);

#ifdef __cplusplus 
}
#endif

#endif

#if defined(OS400) && (__OS400_TGTVRM__ >= 510)
#pragma datamodel(pop)
#endif

