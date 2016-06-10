// ServerStats.h : main header file for the ServerStats DLL
//

#pragma once

#ifndef __AFXWIN_H__
	#error include 'stdafx.h' before including this file for PCH
#endif

#include "resource.h"		// main symbols


// CServerStatsApp
// See ServerStats.cpp for the implementation of this class
//

class CServerStatsApp : public CWinApp
{
public:
	CServerStatsApp();

// Overrides
public:
	virtual BOOL InitInstance();

	DECLARE_MESSAGE_MAP()
};
