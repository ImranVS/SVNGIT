
#if defined(OS400) && (__OS400_TGTVRM__ >= 510)
#pragma datamodel(P128)
#endif

/*********************************************************************/
/*                                                                   */
/* Licensed Materials - Property of IBM                              */
/*                                                                   */
/* L-GHUS-6ENK4N, L-GHUS-6ENJVF                                      */
/* (C) Copyright IBM Corp. 2006  All Rights Reserved                 */
/*                                                                   */
/* US Government Users Restricted Rights - Use, duplication or       */
/* disclosure restricted by GSA ADP Schedule Contract with           */
/* IBM Corp.                                                         */
/*                                                                   */
/*********************************************************************/



/* BSAFE (Security Package) interface definitions */

#ifndef	BSAFE_DEFS
#define	BSAFE_DEFS

#ifdef __cplusplus
extern "C" {
#endif

STATUS LNPUBLIC SECAttachToID (void);		/* Don't let parent process 	*/
											/* control our ID file.			*/
STATUS LNPUBLIC SECReattachToRootID (void);	/* Resync ID file with our		*/
											/* root process but remain		*/
											/* attached to the new	file.	*/

#define	fSECToken_EnableRenewal		0x0001

typedef struct
	{
	MEMHANDLE	mhName;
	MEMHANDLE	mhDomainList;
	WORD		wNumDomains;
	BOOL		bSecureOnly;
	MEMHANDLE	mhData;
	}
	SSO_TOKEN;

STATUS LNPUBLIC SECTokenGenerate(	
	char		*ServerName,	/* Reserved as NULL (ignored) */
	char		*OrgName,
	char		*ConfigName,	
	char		*UserName,
	TIMEDATE	*Creation,
	TIMEDATE	*Expiration,
	MEMHANDLE	*retmhToken,
	DWORD		dwReserved,
	void		*vpReserved
	);

STATUS LNPUBLIC SECTokenValidate(
	char		*ServerName,	/* Reserved as NULL (ignored) */
	char		*OrgName,
	char		*ConfigName,	
	char		*TokenData,
	char		*retUsername,
	TIMEDATE	*retCreation,
	TIMEDATE	*retExpiration,
	DWORD		dwReserved,
	void		*vpReserved
	);

void LNPUBLIC SECTokenFree(
	MEMHANDLE	*mhToken
	);

STATUS LNPUBLIC SECGetSSONameMappingConfig	(
	char		*ServerName, 	/* Reserved as NULL (ignored) */
	char		*OrgName,
	char		*ConfigName,
	BOOL		*retbNameMapping,
	DWORD		dwReserved,
	void		*vpReserved
	);

#ifdef __cplusplus
}
#endif

#endif /* BSAFE_DEFS */

#if defined(OS400) && (__OS400_TGTVRM__ >= 510)
#pragma datamodel(pop)
#endif

