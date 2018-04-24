
#if defined(OS400) && (__OS400_TGTVRM__ >= 510)
#pragma datamodel(P128)
#endif

/*********************************************************************/
/*                                                                   */
/* IBM Confidential                                                  */
/*                                                                   */
/* OCO Source Materials                                              */
/*                                                                   */
/* (C) Copyright IBM Corp. 2006                                      */
/*                                                                   */
/* The source code for this program is not published or otherwise    */
/* divested of its trade secrets, irrespective of what has been      */
/* deposited with the U.S. Copyright Office.                         */
/*                                                                   */
/*********************************************************************/

#ifndef _MIMEDIR_SYMBOLS_DEFS
#define _MIMEDIR_SYMBOLS_DEFS

#ifdef __cplusplus
extern "C" {
#endif

typedef enum tagMimeWellKnownSymbolId {
	MIME_SYMBOL_UNKNOWN		= 0,
	MIME_SYMBOL_TEXT		= 1,      /*  TEXT ... NONE - must stay in this order for the */
	MIME_SYMBOL_MULTIPART	= 2,      /*  array of content-type handlers in the 'inbound' */
	MIME_SYMBOL_MESSAGE		= 3,      /*  converter                                       */
	MIME_SYMBOL_APPLICATION = 4,
	MIME_SYMBOL_IMAGE		= 5,
	MIME_SYMBOL_AUDIO		= 6,
	MIME_SYMBOL_VIDEO		= 7,
	MIME_SYMBOL_NONE		= 8,      /*  last 'inbound' content-type handler */

	MIME_SYMBOL_TO,
	MIME_SYMBOL_FROM,
	MIME_SYMBOL_COMMENTS,
	MIME_SYMBOL_DATE,
	MIME_SYMBOL_ENCRYPTED,
	MIME_SYMBOL_IN_REPLY_TO,
	MIME_SYMBOL_KEYWORDS,
	MIME_SYMBOL_PLAIN,
	MIME_SYMBOL_BCC,
	MIME_SYMBOL_CC,
	MIME_SYMBOL_OCTET_STREAM,
	MIME_SYMBOL_SUBJECT,
	MIME_SYMBOL_HTML,
	MIME_SYMBOL_SENDER,
	MIME_SYMBOL_REPLY_TO,
	MIME_SYMBOL_INLINE,
	MIME_SYMBOL_ATTACHMENT,
	MIME_SYMBOL_FILENAME,
	MIME_SYMBOL_ALTERNATIVE,
	MIME_SYMBOL_MIXED,
	MIME_SYMBOL_7BIT,
	MIME_SYMBOL_8BIT,
	MIME_SYMBOL_QUOTED_PRINTABLE,
	MIME_SYMBOL_BASE64,
	MIME_SYMBOL_BINARY,
	MIME_SYMBOL_VERSION,
	MIME_SYMBOL_CHARSET,
	MIME_SYMBOL_BOUNDARY,
	MIME_SYMBOL_NAME,
	MIME_SYMBOL_MESSAGE_ID,
	MIME_SYMBOL_CONTENT_TYPE,
	MIME_SYMBOL_CONTENT_TRANSFER_ENCODING,
	MIME_SYMBOL_CONTENT_DISPOSITION,
	MIME_SYMBOL_CONTENT_MD5,
	MIME_SYMBOL_CONTENT_LANGUAGE,
	MIME_SYMBOL_RECEIVED,
	MIME_SYMBOL_NEWSGROUPS,
	MIME_SYMBOL_PATH,
	MIME_SYMBOL_FOLLOWUP_TO,
	MIME_SYMBOL_EXPIRES,
	MIME_SYMBOL_REFERENCES,
	MIME_SYMBOL_CONTROL,
	MIME_SYMBOL_DISTRIBUTION,
	MIME_SYMBOL_ORGANIZATION,
	MIME_SYMBOL_SUMMARY,
	MIME_SYMBOL_APPROVED,
	MIME_SYMBOL_LINES,
	MIME_SYMBOL_XREF,
	MIME_SYMBOL_NNTP_POSTING_HOST,
	MIME_SYMBOL_XNEWSREADER,
	MIME_SYMBOL_XMAILER,
	MIME_SYMBOL_RFC822,
	MIME_SYMBOL_CONTENT_DESCRIPTION,
	MIME_SYMBOL_CONTENT_ID,
	MIME_SYMBOL_RELATED,
	MIME_SYMBOL_GIF,
	MIME_SYMBOL_JPEG,
	MIME_SYMBOL_PARTIAL,
	MIME_SYMBOL_EXTERNAL_BODY,
	MIME_SYMBOL_ENRICHED,
	MIME_SYMBOL_MPEG,
	MIME_SYMBOL_BASIC,
	MIME_SYMBOL_START,
	MIME_SYMBOL_PARALLEL,
	MIME_SYMBOL_URL,
	MIME_SYMBOL_ACCESS_TYPE,
	MIME_SYMBOL_EXPIRATION,
	MIME_SYMBOL_SITE,
	MIME_SYMBOL_SERVER,
	MIME_SYMBOL_DIRECTORY,
	MIME_SYMBOL_MODE,
	MIME_SYMBOL_PERMISSION,
	MIME_SYMBOL_SIZE,
	MIME_SYMBOL_CONTENT_BASE,
	MIME_SYMBOL_CONTENT_LOCATION,
	MIME_SYMBOL_X_X509_USER_CERT,
	MIME_SYMBOL_X_X509_CA_CERT,
	MIME_SYMBOL_SIGNED,
	MIME_SYMBOL_XPKCS7SIGNATURE,
	MIME_SYMBOL_PKCS7SIGNATURE,
	MIME_SYMBOL_XPKCS7MIME,
	MIME_SYMBOL_PKCS7MIME,
	MIME_SYMBOL_APPLEDOUBLE,
	MIME_SYMBOL_APPLEFILE,
	MIME_SYMBOL_METHOD,
	MIME_SYMBOL_PROFILE,
	MIME_SYMBOL_CALENDAR,
	MIME_SYMBOL_VCARD21,
	MIME_SYMBOL_X_MAC_TYPE,
	MIME_SYMBOL_X_MAC_CREATOR,
	MIME_SYMBOL_MAC_BINHEX40,
	MIME_SYMBOL_X_NOTES_ITEM,
	MIME_SYMBOL_X_UUENCODE,
	MIME_SYMBOL_X_UUE,
	MIME_SYMBOL_UUENCODE,
	MIME_SYMBOL_X_LOTUS_ENCAP,
	MIME_SYMBOL_X_LOTUS_ENCAP1,
	MIME_SYMBOL_X_LOTUS_ENCAP2,
	MIME_SYMBOL_X_MIMETRACK,
	MIME_SYMBOL_DIGEST,
	MIME_SYMBOL_32KADPCM,
	MIME_SYMBOL_VOICE_MESSAGE,
	MIME_SYMBOL_BMP,
	MIME_SYMBOL_DELIVERY_STATUS,
	MIME_SYMBOL_REPORT,
	MIME_SYMBOL_RFC822_HEADERS,
	MIME_SYMBOL_PROTOCOL,
	MIME_SYMBOL_CGM,
	MIME_SYMBOL_CSS,
	MIME_SYMBOL_POSTSCRIPT,
	MIME_SYMBOL_RICHTEXT,
	MIME_SYMBOL_TIFF,
	MIME_SYMBOL_TNEF,

	/*	When you add a symbol here, you must also add an entry to the MimeKeywords
		table in mime\mimdrsym.cpp. */

    MIME_SYMBOL_LAST    /* for bounds checking -- no equivalent in mime\mimdrsym.cpp */

} MimeWellKnownSymbolId;

typedef MimeWellKnownSymbolId		MIMESYMBOL;


#ifdef __cplusplus
}
#endif

#endif /* _MIMEDIR_SYMBOLS_DEFS */

#if defined(OS400) && (__OS400_TGTVRM__ >= 510)
#pragma datamodel(pop)
#endif

