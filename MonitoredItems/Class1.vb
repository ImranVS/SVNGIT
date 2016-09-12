Imports System
Imports System.Collections
Imports System.Data.DataView
Imports System.ComponentModel
Imports VSFramework
Imports System.Configuration
'Monitored Items Class for use with VitalSigns
'Developed by Alan Forbes
'Copyright 2010, All Rights Reserved.


'The Base Class for monitored items, such as URL, network device, etc.

Public Class MonitoredDevice

#Region "Member Variables"
    Dim mBuild As Integer = 180
    Dim mName As String
    Dim mAlias As String
    Dim mScanInterval As Double 'A value in minutes
    Dim mOffHoursScanInterval As Double 'A value in minutes
    Dim mStatus As String = "Not yet scanned"
    Dim mIsUp As Boolean = True  'True if the device is up
    Dim mNotificationGroup As String 'Alerts go to this group, if a value is there

    Dim mCPU As Double = 0
    Dim mUpCount As Integer = 0
    Dim mDownCount As Integer = 0

    Dim mDownMinutes As Double = 0
    Dim mUpMinutes As Double = 0
    Dim mUpPercentCount As Double = 0
    Dim mUpPercentMinutes As Double = 0
    Dim mOnTargetUpCount As Integer = 0
    Dim mOnTargetDownCount As Integer = 0
    Dim mOnTargetPercent As Double = 0
    Dim mBusinessHoursOnTargetUpCount As Integer = 0
    Dim mBusinessHoursOnTargetDownCount As Integer = 0
    Dim mBusinessHoursOnTargetPercent As Double = 0
    Dim mBusinessHoursUpPercent As Double = 0

    Dim mAlertCondition As Boolean  'True if the monitored item is in a state of alert
    Dim mAlertID As Double 'incremented after each alert condition
    '   Dim mLastScan As DateTime = Now.Now.AddMinutes(-15)
    Dim mLastStatusUpdate As DateTime = Now   'Used for calculating up and down time
    Dim mLastScan As DateTime = Now
    Dim mNextScan As DateTime
    Dim mOffHours As Boolean  'True if the server is in off-hours mode
    Dim mResponseThreshold As Long
    Dim mResponseTime As Long
    Dim mPeakResponseTime As Long  'the longest it took for this device to respond
    Dim mPeakResponseDateTime As DateTime = Now

    Dim mIPAddress As String
    Dim mPort As Integer
    Dim mEnabled As Boolean
    Dim mCategory As String
    Dim mLocation As String
    Dim mLocationKey As Integer
    Dim mRetryInterval As Integer = 2
    Dim mResponseDetails As String
    Dim mAlert As String
    Dim mFailureCount As Integer  'Used to store how many times in a row the device has failed
    Dim mFailureThreshold As Integer ' Used to store how many times a device can fail before triggering an alert
    Dim mPreviousKeyValue As Double
    Dim mKey As Integer  'Stores the key or ID file in the Access table to uniquely identify the record

    Dim mIsBeingScanned As Boolean = False
    Dim mScanAgentLog As Boolean
	Dim mScanLog As Boolean
    Dim mInsufficientLicenses As Boolean

    Public Property ServerObjectID As String

    Enum Alert As Integer
        'This never got used.
        DeadMail = 0
        PendingMail = 1
        Memory = 2
        DiskSpace = 3
        ServerTask = 4
        NotResponding = 5
        Slow = 6
        HeldMail = 7
    End Enum

    Dim mStartTicks, mEndTicks As String

#End Region

    Public Property NotificationGroup As String
        Get
            Return mNotificationGroup
        End Get
        Set(ByVal value As String)
            mNotificationGroup = value
        End Set
    End Property


    Public ReadOnly Property BuildNumber() As Integer
        Get
            Return mBuild
        End Get
    End Property

#Region "Availability and On Target Percentages"

    Public ReadOnly Property UpCount() As Integer
        Get
            Return mUpCount
        End Get
    End Property
    Public ReadOnly Property DownCount() As Integer
        Get
            Return mDownCount
        End Get
    End Property
    Public ReadOnly Property UpPercentCount() As Double
        Get
            Return mUpPercentCount
        End Get
    End Property

    Public Sub IncrementUpCount()
        mUpCount += 1
        mFailureCount = 0 'the number of times the device has failed in a row
        mUpPercentCount = 1 - (mDownCount / (mUpCount + mDownCount))
        If mUpPercentCount < 0 Then
            mUpPercentCount = 0
        End If

        Try
            Dim ts As New TimeSpan
            Dim dtNow As DateTime = Date.Now
            ts = dtNow.Subtract(mLastStatusUpdate)
            IncrementUpMinutes(System.Math.Abs(ts.TotalMinutes))
            ts = Nothing
            dtNow = Nothing
        Catch ex As Exception

        End Try

        mLastStatusUpdate = Date.Now
    End Sub

    Public Sub IncrementDownCount()
        mDownCount += 1
        mFailureCount += 1 'the number of times the device has failed in a row
        mUpPercentCount = 1 - (mDownCount / (mUpCount + mDownCount))
        If mUpPercentCount < 0 Then
            mUpPercentCount = 0
        End If

        If mLastStatusUpdate <> Nothing Then
            Try
                Dim ts As New TimeSpan
                Dim dtNow As DateTime = Date.Now
                ts = dtNow.Subtract(mLastStatusUpdate)
                IncrementDownMinutes(System.Math.Abs(ts.TotalMinutes))
                ts = Nothing
                dtNow = Nothing
            Catch ex As Exception

            End Try

        End If
        mLastStatusUpdate = Date.Now
    End Sub


    Public ReadOnly Property DownMinutes() As Double
        Get
            If mDownMinutes > 58.25 Or mDownMinutes > 60 Then
                Return 60
            Else
                Return mDownMinutes
            End If

        End Get
    End Property
    Public ReadOnly Property UpMinutes() As Double
        Get
            Try
                Dim myMinutes As Double
                myMinutes = Now.Minute
                Dim myUptime As Double
                myUptime = myMinutes - mDownMinutes
                If myUptime < 0 Then myUptime = 0
                Return myUptime
            Catch ex As Exception
                Return 0

            End Try

        End Get
    End Property
    Public ReadOnly Property UpPercentMinutes() As Double
        Get
            Try
                If mUpPercentMinutes <> Double.NaN Then
                    Return mUpPercentMinutes
                Else
                    Return 0
                End If
            Catch ex As Exception
                Return 0
            End Try

        End Get
    End Property

    Private Sub IncrementDownMinutes(ByVal Minutes As Double)
        mDownMinutes += Minutes
        Try
            mUpPercentMinutes = 1 - (mDownMinutes / (mDownMinutes + mUpMinutes))
        Catch ex As Exception
            mUpPercentMinutes = 1
        End Try

    End Sub

    Private Sub IncrementUpMinutes(ByVal Minutes As Double)
        mUpMinutes += Minutes
        Try
            mUpPercentMinutes = 1 - (mDownMinutes / (mDownMinutes + mUpMinutes))
        Catch ex As Exception
            mUpPercentMinutes = 1
        End Try

    End Sub


    'OnTargetUp and Down counts are used to increment how many times the device responds within the target response time
    Public ReadOnly Property OnTargeUpCount() As Integer
        Get
            Return mOnTargetUpCount
        End Get
    End Property
    Public ReadOnly Property OnTargetDownCount() As Integer
        Get
            Return mOnTargetDownCount
        End Get
    End Property
    Public ReadOnly Property OnTargetPercent() As Double
        Get
            Return mOnTargetPercent
        End Get
    End Property
    Public Sub IncrementOnTargetCount()
        mOnTargetUpCount += 1
        '     mFailureCount = 0 'the number of times the device has failed in a row
        Try
            mOnTargetPercent = 1 - (mOnTargetDownCount / (mOnTargetUpCount + mOnTargetDownCount))
            If mOnTargetPercent < 0 Then
                mOnTargetPercent = 0
            End If
        Catch ex As Exception
        End Try
    End Sub
    Public Sub IncrementOffTargetCount()
        mOnTargetDownCount += 1

        mOnTargetPercent = 1 - (mOnTargetDownCount / (mOnTargetUpCount + mOnTargetDownCount))
        If mOnTargetPercent < 0 Then
            mOnTargetPercent = 0
        End If
    End Sub

    'Business Hours OnTarget is how many times it happened during business hours
    Public ReadOnly Property BusinessHoursOnTargeCount() As Integer
        Get
            Return mBusinessHoursOnTargetUpCount
        End Get
    End Property
    Public ReadOnly Property BusinessHoursOffTargetCount() As Integer
        Get
            Return mBusinessHoursOnTargetDownCount
        End Get
    End Property
    Public ReadOnly Property BusinessHoursOnTargetPercent() As Double
        Get
            Return mBusinessHoursOnTargetPercent
        End Get
    End Property
    Public Sub IncrementBusinessHoursOnTargetCount()
        mBusinessHoursOnTargetUpCount += 1
        ' mFailureCount = 0 'the number of times the device has failed in a row
        Try
            mBusinessHoursOnTargetPercent = 1 - (mBusinessHoursOnTargetDownCount / (mBusinessHoursOnTargetUpCount + mBusinessHoursOnTargetDownCount))
            If mBusinessHoursOnTargetPercent < 0 Then
                mBusinessHoursOnTargetPercent = 0
            End If
        Catch ex As Exception
        End Try
    End Sub
    Public Sub IncrementBusinessHoursOffTargetCount()
        mBusinessHoursOnTargetPercent = 1 - (mBusinessHoursOnTargetDownCount / (mBusinessHoursOnTargetUpCount + mBusinessHoursOnTargetDownCount))
        If mBusinessHoursOnTargetPercent < 0 Then
            mBusinessHoursOnTargetPercent = 0
        End If
    End Sub

    Public Sub ResetUpandDownCounts()
        mDownCount = 0
        mUpCount = 0
        mDownMinutes = 0
        mUpMinutes = 0
        mUpPercentCount = 1
        mUpPercentMinutes = 1

        mOnTargetDownCount = 0
        mOnTargetUpCount = 0
        mOnTargetPercent = 1

        mBusinessHoursOnTargetDownCount = 0
        mBusinessHoursOnTargetUpCount = 0
        mBusinessHoursOnTargetPercent = 1
        mLastStatusUpdate = Date.Now
    End Sub


#End Region

#Region "Scan Interval Properties"

    Public Property IsBeingScanned As Boolean

        Get
            Return mIsBeingScanned
        End Get
        Set(ByVal value As Boolean)
            mIsBeingScanned = value
        End Set
    End Property

    Public Property RetryInterval() As Integer
        Get
            Return mRetryInterval
        End Get
        Set(ByVal Value As Integer)
            If Value < 1 Then
                mRetryInterval = 1
            Else
                mRetryInterval = Value
            End If
        End Set
    End Property

    Public Property ScanInterval() As Integer
        Get
            Return mScanInterval
        End Get
        Set(ByVal interval As Integer)

            If interval > 0 And interval < 360 Then
                mScanInterval = interval
            Else
                mScanInterval = 8
            End If

        End Set
    End Property

    Public Property OffHoursScanInterval() As Integer
        Get
            Return mOffHoursScanInterval
        End Get
        Set(ByVal interval As Integer)
            If interval > 0 And interval < 120 Then
                mOffHoursScanInterval = interval
            Else
                mOffHoursScanInterval = 30
            End If

        End Set
    End Property

    Public Property LastScan() As DateTime
        Get
            Return mLastScan
        End Get

        Set(ByVal value As DateTime)
            mLastScan = value
        End Set
    End Property

    Public Property NextScan() As DateTime
        Get
            Dim MyTime As DateTime

            If mAlertCondition = True Then
                MyTime = mLastScan.AddMinutes(mRetryInterval)
            Else
                If mOffHours Then
                    MyTime = mLastScan.AddMinutes(mOffHoursScanInterval)
                Else
                    MyTime = mLastScan.AddMinutes(mScanInterval)
                End If
            End If

            mNextScan = MyTime
            Return mNextScan
        End Get
        Set(ByVal Value As DateTime)
            mNextScan = Value
        End Set
    End Property

#End Region

#Region "Performance Properties"

    Public ReadOnly Property PeakResponseTime() As Double
        Get
            Return mPeakResponseTime
        End Get

    End Property

    Public ReadOnly Property PeakResponseDateTime() As DateTime
        Get
            Return mPeakResponseDateTime
        End Get
    End Property

    Public Property FailureThreshold() As Integer
        Get
            Return mFailureThreshold
        End Get
        Set(ByVal Value As Integer)
            If Value > 0 Then
                mFailureThreshold = Value
            Else
                mFailureThreshold = 1
            End If
        End Set
    End Property

    Public Property ResponseTime() As Double
        Get
            Return mResponseTime
        End Get
        Set(ByVal Value As Double)
            mResponseTime = Value
            If Value > mPeakResponseTime Then
                mPeakResponseTime = Value
                mPeakResponseDateTime = Now
            End If
        End Set
    End Property

    Public Property ResponseDetails() As String
        Get
            Return mResponseDetails
        End Get
        Set(ByVal Value As String)
            mResponseDetails = Value
        End Set
    End Property
    Public Property ResponseThreshold() As Long
        Get
            Return mResponseThreshold
        End Get
        Set(ByVal Value As Long)
            If Value <= 0 Then
                mResponseThreshold = 1
            Else
                mResponseThreshold = Value
            End If
        End Set
    End Property
    Public ReadOnly Property FailureCount() As Integer
        Get
            Return mFailureCount
        End Get
    End Property

    Public Property AlertCondition() As Boolean
        Get
            Return mAlertCondition
        End Get
        Set(ByVal Value As Boolean)
            If Value = False Then
                mAlertCondition = False
                ResetStatus()
            Else
                mAlertCondition = True
            End If
        End Set
    End Property

    Public Property AlertType() As Alert
        Get
            Return mAlert
        End Get
        Set(ByVal Value As Alert)
            mAlert = Value
        End Set
    End Property

#End Region

#Region "Other Properties"

    Public Property SecondaryRole As String
    Public Property StatusCode As String
    Public Property ServerType As String

    Public Property Key() As Integer
        'Stores the key or ID file in the Access table to uniquely identify the record
        Get
            Return mKey
        End Get
        Set(ByVal Value As Integer)
            mKey = Value
        End Set
    End Property

    Public Property CPU_Utilization As Double
        Get
            Return mCPU

        End Get
        Set(ByVal Value As Double)
            mCPU = Value
        End Set
    End Property


    Public WriteOnly Property StartTicks() As Long
        Set(ByVal Value As Long)
            mStartTicks = Value
        End Set
    End Property

    Public Property PreviousKeyValue() As Double
        Get
            Return mPreviousKeyValue
        End Get
        Set(ByVal Value As Double)
            mPreviousKeyValue = Value
        End Set
    End Property   'Will hold the previous value of the thing measured in Threshold meter-- i.e., pending mail forDomino server, previous response time for others, etc.

    Public WriteOnly Property EndTicks() As Long
        Set(ByVal Value As Long)
            mEndTicks = Value
            Dim elapsed As TimeSpan
            elapsed = New TimeSpan(mEndTicks - mStartTicks)
            ResponseTime = elapsed.TotalMilliseconds
        End Set
    End Property

    Public Property IsUp() As Boolean
        Get
            Return mIsUp
        End Get
        Set(ByVal Value As Boolean)
            mIsUp = Value
        End Set
    End Property   'True if the device is responding or "up"

    Public Property Port() As Integer
        Get
            Return mPort
        End Get
        Set(ByVal Value As Integer)
            mPort = Value
        End Set
    End Property

    Public Property Location() As String
        Get
            Return mLocation
        End Get
        Set(ByVal Value As String)
            mLocation = Value
        End Set
    End Property

    Public Property LocationKey() As Integer
        Get
            Return mLocationKey
        End Get
        Set(ByVal Value As Integer)
            mLocationKey = Value
        End Set
    End Property

    Public Property Category() As String
        Get
            Return mCategory
        End Get
        Set(ByVal Value As String)
            mCategory = Value
        End Set
    End Property

    Public Property Enabled() As Boolean
        Get
            Return mEnabled
        End Get
        Set(ByVal Value As Boolean)
            mEnabled = Value
            If Value = False Then  'if disabled, reset statistics
                mUpCount = 0
                mDownCount = 0
                mUpPercentCount = 0
                mStatus = "Disabled"
                mResponseDetails = ""
            End If
        End Set
    End Property

    Public Property IPAddress As String
        Get
            Return mIPAddress
        End Get
        Set(ByVal Value As String)
            mIPAddress = Value
        End Set
    End Property

    Private Function ResetStatus()
        ' mFailureCount = 0
        mAlertCondition = False
        mAlertID += 1
        mStatus = "OK"
        '  mFailureCount = 0
    End Function

    Public Property Status() As String
        Set(ByVal Value As String)
            mStatus = Value
        End Set
        Get
            Return mStatus
        End Get

    End Property

    Public Property OffHours() As Boolean
        'True if currently operating in off hours mode
        Set(ByVal OffHours As Boolean)
            mOffHours = OffHours
        End Set
        Get
            Return mOffHours
        End Get
    End Property

    Public Property Name() As String
        Get
            Return mName
        End Get
        Set(ByVal MyName As String)
            mName = MyName
        End Set
    End Property

    Public Property Description() As String
        Get
            Return mAlias
        End Get
        Set(ByVal strAlias As String)
            mAlias = strAlias
        End Set
    End Property

    Public Property ScanAgentLog() As Boolean
        Get
            Return mScanAgentLog
        End Get
        Set(ByVal value As Boolean)
            mScanAgentLog = value
        End Set
    End Property

    Public Property ScanLog() As Boolean
        Get
            Return mScanLog
        End Get
        Set(ByVal value As Boolean)
            mScanLog = value
        End Set
    End Property
    Public isMasterRunning As Boolean

    Public Property InsufficentLicenses() As Boolean
        Get
            Return mInsufficientLicenses
        End Get
        Set(ByVal value As Boolean)

            'if we are trying to set as insufficient license, then check to see if any master is alive
            If value = True Then
                Dim IgnoreMaster As Boolean = False
                If ConfigurationManager.AppSettings("IgnoreMaster") IsNot Nothing Then
                    If ConfigurationManager.AppSettings("IgnoreMaster").ToString.ToLower() = "true" Then
                        IgnoreMaster = True
                    End If
                End If
                If IgnoreMaster Then
                    mInsufficientLicenses = False
                    isMasterRunning = True
                Else
                    Dim isNodesAlive As Boolean = checkIfNodeIsAlive()
                    If isNodesAlive Then
                        isMasterRunning = True
                    Else
                        isMasterRunning = False
                    End If
                    mInsufficientLicenses = isNodesAlive
                End If


            Else
                mInsufficientLicenses = value
                isMasterRunning = True
            End If

        End Set
    End Property
    Public Function checkIfNodeIsAlive() As Boolean

        Try
            Dim adapter As New VSAdaptor
            Dim sql As String = "Select Name from dbo.Nodes where Alive=1"
            Dim dt As New DataTable()
            Dim ds As New DataSet()
            adapter.FillDatasetAny("VitalSigns", "VitalSigns", sql, ds, "AliveNodes")
            dt = ds.Tables("AliveNodes")

            If dt.Rows.Count > 0 Then

                Return True
            End If
            Return False
        Catch ex As Exception

        End Try
        Return True
    End Function
#End Region

End Class



#Region "Derived Classes"
Public Class MicrosoftServer
	Inherits MonitoredDevice
	Public Property UserName As String
	Public Property Password As String
	Public Property ServerTypeId As Integer
	Public Property CPU_Threshold As Double
	Public Property Memory_Threshold As Double
	Public Property MemoryPercentUsed As Double
	Public Property DiskThreshold As Double
	Public Property OperatingSystem As String
	Public Property ServerDaysAlert As String
	Public Property UserCount As Long
	Public Property VersionNo As String
    Public Property ServerId As String
    Public Property IsPrereqsDone As Boolean
    Public Property FastScan As Boolean
    Public Property HourlyAlerts As System.Collections.Generic.List(Of HourlyAlert)

    Public Property TypeANDName As String
        Get
            If ServerType = "Office365" Then
                Return Name & "-" & Location
            Else
                Return Name & "-" & ServerType
            End If

        End Get
        Set(value As String)
            TypeANDName = value
        End Set
    End Property

    Public Property ListOfRequiredServices As New Collections.Generic.List(Of String)

    Public Class HourlyAlert
        Public Property AlertType As String
        Public Property AlertRaised As Boolean
        Public Property AlertDetails As String

    End Class
End Class
Public Class MailFlowTest
	Public Property SourceServer As String
	Public Property DestinationServer As String
	Public Property LatencyYellowThreshold As Integer
	Public Property LatencyRedThreshold As Integer

End Class

Public Class ExchangeServer
	Inherits MicrosoftServer

    'Public Property IsActiveDirectoryServer as boolean
    'Public Property IsClientAccessServer As Boolean
    'Public Property IsHubTransportServer As Boolean
    'Public Property IsEdgeTransportServer As Boolean
    'Public Property IsMailBoxServer As Boolean
    'Public Property IsUnifiedMessagingServer As Boolean
    Public Property Role As String()
    Public Property VersionExchange As String
	'Public Property UserCount As Long
    Public Property Domain As String
	'Public Property UserName As String
    Public Property OWAUsers As Long
    Public Property RPCClientAccesUsers As Long
	'Public Property Password As String

	Public Property ThresholdSetting As ExchangeThresholdSettings
	'Public Property OperatingSystem As String
	Public Property DAGScan As Boolean
	'Public Property VersionNo As String
	'Public Property ServerDaysAlert As String
	Public Property DAGPrimaryIPAddress As String
	Public Property DAGPrimaryUserName As String
	Public Property DAGPrimaryPassword As String
	Public Property DAGPrimaryAuthenticationType As String
	Public Property DAGBackupIPAddress As String
	Public Property DAGBackupUserName As String
	Public Property DAGBackupPassword As String
	Public Property DAGBackupAuthenticationType As String
	Public Property DAGReplyQueueThreshold As Integer
    Public Property DAGCopyQueueThreshold As Integer

    Public Property DAGStatus As String

	' Dim mDiskPlatformStatistics As DominoDiskPlatformStatisticsCollection
	Dim mAvailabilityIndex, mAvailabilityThreshold As Integer
	Dim mExpansionFactor As Integer
	Dim mVersion, mOS As String
	Dim mCommonName As String
	Dim mMemory As String 'stores the status of memory
	Dim mMemoryPercentUsed As Integer  'the percent of memory usage that triggers an alert, i.e., 80
	Dim mTaskStatus As String 'stores the status of the server tasks



	'Mail Related
	Dim mMailFileCount As Integer 'the number of mail files on the server
	Dim mPendingMail, mDeadMail, mHeldMail, mHeldThreshold, mPendingThreshold, mDeadThreshold, mMailboxCount As Integer
	Dim mPeakPendingMail, mPeakDeadMail As Integer
	Dim mPeakPendingTime, mPeakDeadTime As DateTime
	Dim mPriorPending, mPriorDead As Integer
	Dim mLocation As String	'mail server, app server, etc.
	Dim mMailDirectory As String
	Dim mDeliveredMail, mRoutedMail, mTransferredMail, mSMTPMailProcessed, mMailTransferFailures As Long
	Dim mMailFileScanDate As DateTime

	'CAS Options
	Public Property CASSmtp As Boolean
	Public Property CASPop3 As Boolean
	Public Property CASImap As Boolean
	Public Property CASOARPC As Boolean
	Public Property CASOWA As Boolean
	Public Property CASActiveSync As Boolean
	Public Property CASEWS As Boolean
	Public Property CASECP As Boolean
	Public Property CASAutoDiscovery As Boolean
	Public Property CASOAB As Boolean
	Public Property ActiveSyncUserName As String
    Public Property ActiveSyncPassword As String
    Public Property TestId As Integer
    Public Property SMTPURLs As String
    Public Property SMTPCASUserName As String
    Public Property SMTPCASPassword As String
    Public Property POP3URLs As String
    Public Property POP3CASUserName As String
    Public Property POP3CASPassword As String
    Public Property IMAPURLs As String
    Public Property IMAPCASUserName As String
    Public Property IMAPCASPassword As String
    Public Property OutlookAnywhereURLs As String
    Public Property OutlookAnywhereCASUserName As String
    Public Property OutlookAnywhereCASPassword As String
    Public Property AutoDiscoveryURLs As String
    Public Property AutoDiscoveryCASUserName As String
    Public Property AutoDiscoveryCASPassword As String
    Public Property ActiveSyncURLs As String
    Public Property ActiveSyncCASUserName As String
    Public Property ActiveSyncCASPassword As String
    Public Property OWAURLs As String
    Public Property OWACASUserName As String
    Public Property OWACASPassword As String
    Public Property RPCURLs As String
    Public Property RPCCASUserName As String
    Public Property RPCCASPassword As String


	'   Public Property CPU_Threshold As Double
	'   Public Property Memory_Threshold As Double
	'   Public Property MemoryPercentUsed As Double
	'Public Property DiskThreshold As Double
	'Public Property ServerType As String 'Exchange or Lync 
	Public Property DeliveryThreshold As Integer 'mail probe
	Public Property MailProbeName As String
	Public Property MailProbeAddress As String
	Public Property MailProbeSourceServer As String
	Public Property MailProbeDestinationServer As String
	Public Property EnableLatencyTest As Boolean
	Public Property LatencyRedThreshold As Integer
	Public Property LatencyYellowThreshold As Integer

	Public Property AuthenticationType As String


#Region "Mail Properties"
    Public Property DeleteDeadThreshold() As Integer

    Public Property MailFileCount() As Integer
        Get
            Return mMailFileCount
        End Get
        Set(ByVal Value As Integer)
            mMailFileCount = Value
        End Set
    End Property

    Public Property MailFileScanDate() As DateTime
        Get
            Return mMailFileScanDate
        End Get
        Set(ByVal Value As DateTime)
            mMailFileScanDate = Value
        End Set
    End Property

    Public Property MailDirectory() As String
        Get
            Return mMailDirectory
        End Get
        Set(ByVal Value As String)
            mMailDirectory = Value
        End Set
    End Property

    Public Property MailboxCount() As Integer
        Get
            Return mMailboxCount
        End Get
        Set(ByVal Value As Integer)
            mMailboxCount = Value
        End Set
    End Property



    Public ReadOnly Property PriorPendingMail() As Integer
        'returns the value of pending mail in the prior cycle
        Get
            Return mPriorPending
        End Get
    End Property

    Public ReadOnly Property PriorDeadMail() As Integer
        'returns the value of dead mail in the prior cycle
        Get
            Return mPriorDead
        End Get
    End Property

    Public ReadOnly Property PeakPendingMail() As Integer
        Get
            Return mPeakPendingMail
        End Get
    End Property

    Public ReadOnly Property PeakPendingTime() As DateTime
        Get
            Return mPeakPendingTime
        End Get
    End Property

    Public Property PendingMail() As Integer
        'The current value of pending mail on the server
        Get
            Return mPendingMail
        End Get
        Set(ByVal Value As Integer)
            mPriorPending = mPendingMail
            mPendingMail = Value
            If Value > mPeakPendingMail Then
                mPeakPendingMail = Value
                mPeakPendingTime = Now
            End If
        End Set
    End Property

    Public ReadOnly Property PeakDeadMail() As Integer
        Get
            Return mPeakDeadMail
        End Get
    End Property

    Public ReadOnly Property PeakDeadTime() As DateTime
        Get
            Return mPeakDeadTime
        End Get
    End Property

    Public Property DeadMail() As Integer
        'the current value of dead mail on the server
        Get
            Return mDeadMail
        End Get
        Set(ByVal Value As Integer)
            mPriorDead = mDeadMail
            mDeadMail = Value
            If Value > mPeakDeadMail Then
                mPeakDeadMail = Value
                mPeakDeadTime = Now
            End If
        End Set
    End Property


    Public Property HeldMail() As Integer
        'the current value of dead mail on the server
        Get
            Return mHeldMail
        End Get
        Set(ByVal Value As Integer)

            mHeldMail = Value

        End Set
    End Property

    Public Property HeldThreshold() As Integer
        Get
            Return mHeldThreshold
        End Get
        Set(ByVal Value As Integer)
            If Value >= 0 Then
                mHeldThreshold = Value
            Else
                mHeldThreshold = 25
            End If
        End Set
    End Property



    Public Property PendingThreshold() As Integer
        'The value of pending mail that triggers an alert
        Get
            Return mPendingThreshold
        End Get
        Set(ByVal Value As Integer)
            If Value >= 0 Then
                mPendingThreshold = Value
            Else
                mPendingThreshold = 50  'default value
            End If

        End Set
    End Property

    Public Property DeadThreshold() As Integer
        Get
            Return mDeadThreshold
        End Get
        Set(ByVal Value As Integer)
            If Value >= 0 Then
                mDeadThreshold = Value
            Else
                mDeadThreshold = 25
            End If
        End Set
    End Property


#End Region
End Class
Public Class ExchangeThresholdSettings

    Public Property SubQThreshold As Integer
	Public Property PoisonQThreshold As Integer
	Public Property ShadowQThreshold As Integer
    Public Property UnReachableQThreshold As Integer
    Public Property TotalQThreshold As Integer

End Class
Public Class SharepointServer
	Inherits MicrosoftServer
	Public Property Role As String
    Public Property Farm As String
    Public Property IsSearchServer As Boolean
    Public Property ConflictingContentType As Boolean
    Public Property CustomizedFiles As Boolean
    Public Property MissingGalleries As Boolean
    Public Property MissingParentContentTypes As Boolean
    Public Property MissingSiteTemplates As Boolean
    Public Property UnsupportedLanguagePack As Boolean
    Public Property UnsupportedMUI As Boolean
End Class
Public Class Office365Server
	Inherits MicrosoftServer
	Public Property tenantName As String

	Public Property EnableMailFlow As Boolean
	Public Property EnableInboxTest As Boolean
	Public Property EnableOWATest As Boolean
    Public Property EnableSMTPTest As Boolean
    Public Property EnablePOPTest As Boolean
    Public Property EnableIMAPTest As Boolean
    Public Property EnableAutoDiscoveryTest As Boolean
    Public Property EnableMAPIConnectivityTest As Boolean
    Public Property EnableCreateTaskTest As Boolean
    Public Property EnableCreateFolderTest As Boolean
    Public Property EnableOneDriveUploadTest As Boolean
    Public Property EnableOneDriveDownloadTest As Boolean
    Public Property EnableOneDriveSearchTest As Boolean
    Public Property EnableCreateSiteTest As Boolean
    Public Property EnableCreateCalEntryTest As Boolean
    Public Property EnableResolveUserTest As Boolean
    Public Property DirSyncExportTest As Boolean
    Public Property DirSyncImportTest As Boolean

    Public Property ADFSMode As Boolean
    Public Property Mode As String
    Public Property DirSyncServerName As String
    Public Property DirSyncUID As String
    Public Property DirSyncPWD As String

    Public Property ADFSRedirectTest As Boolean
    Public Property URLTest As Boolean
    Public Property AuthenticationTest As Boolean

    Public Property MailFlowThreshold As Integer
    Public Property InboxThreshold As Integer
    Public Property ComposeEmailThreshold As Integer
    Public Property CreateTaskThreshold As Integer
    Public Property CreateFolderThreshold As Integer
    Public Property OneDriveUplaodThreshold As Integer
    Public Property OneDriveDownlaodThreshold As Integer
    Public Property OneDriveSearchThreshold As Integer
    Public Property CreateSiteThreshold As Integer
    Public Property CreateCalEntryThreshold As Integer
    Public Property ResolveUserThreshold As Integer
    Public Property DirSyncExportThreshold As Integer
    Public Property DirSyncImportThreshold As Integer

End Class
Public Class ActiveDirectoryServer
    Inherits MicrosoftServer
    Public Property ADLogonTest As String
    Public Property ADQueryTest As String
    Public Property ADPortTest As String
    Public Property ADAdvertisingTest As String
    Public Property ADFrsSysVolTest As String
    Public Property ADReplicationsTest As String
    Public Property ADServicesTest As String
    Public Property ADDNSTest As String
    Public Property ADFsmoCheckTest As String
End Class
Public Class Traveler_Backend
    Public Property TravelerServicePoolName As String
    Public Property ServerName As String
    Public Property DataStore As String
    Public Property UserName As String
    Public Property Password As String
    Public Property UsedByServers As String
    Public Property Port As Integer
    Public Property IntegratedSecurity As Boolean
    Public Property TestScanServer As String
    Public Property DatabaseName As String
End Class


Public Class DominoServer
    Inherits MonitoredDevice
    Dim mHierarchicalName As String
    Dim mTitle As String

    'Quickr server information
    Dim mQuickrServer As Boolean = False 'if this server is a Quickr server
    Dim mQuickrPlaceCount As Long 'the number of Quickr Places, if this is a Quickr server

    'Traveler server information
    Dim mTravelerServer As Boolean = False 'if this server runs Traveler
    Dim mTravelerUsers As Integer
    Dim mTraveler_Successful_DeviceSync_Count As Long  'The current value
    Dim mPrior_Traveler_Successful_DeviceSync_Count As Long ' the previous value (needed because we compare prior value to current value)
    Dim mPrior_Traveler_Successful_DeviceSync_Count_Updated_Time As DateTime  'the time the previous value was updated


    Dim mScanDBHealth As Boolean 'if the server should be scanned for database health
    Dim mBES_Server As Boolean   'if the server runs BES
    Dim mBESthreshold As Integer
    Dim mUserCount As Integer
    Dim mHTTPCurrentConnections As Integer
    Dim mRequireSSL As Boolean
    Dim mExternalAlaias As String
    Dim mScanTravelerServer As Boolean
    Dim mPeakUserCount As Integer
    Dim mPeakUserTime As DateTime
    Dim mDiskThreshold As Double  'Stores the value used to trigger a disk space alert, for example 10% will be stored as .1
    Dim mServersDayAlert As Integer = 0

    Dim mStatistics_All As String  'stores the results of 'show stat' but sometimes this gets truncated so the other ones are for that
    Dim mStatistics_Mail As String  'stores the results of 'show stat mail'
    Dim mStatistics_Server As String 'stores the results of 'show stat server'
    Dim mStatistics_Replica As String 'stores the results of 'show stat replica'
    Dim mStatistics_Disk As String 'stores the results of 'show stat disk'
    Dim mStatistics_Database As String 'stores the results of 'show stat disk'
    Dim mStatistics_Misc As String 'stores the results of a miscellaneous stat request
    Dim mStatistics_Memory As String  'stores the results of 'show stat Mem'
    Dim mStatistics_Platform As String  'stores the results of 'show stat Platform'
    Dim mStatistics_Traveler As String  'stores the results of 'show stat Traveler'
    Dim mStatistics_Http As String 'stores the results of 'show stat HTTP'
    Dim mStatistics_Domino As String 'stores the results of 'show stat Domino'

    Dim mSHOWTASKS As String 'the results of a SH TA command
    Dim mTaskStrings As String    'some Admins want to search the SH TA string for certain words--store them in in this variable
    Dim mRemoteConsoleAccess As Boolean
    Dim mTaskCollection As ServerTasksCollection
    Dim mTaskSettings As ServerTaskSettingCollection
    Dim mCustomStats As DominoStatisticsCollection
    Dim mDiskDrives As DominoDiskSpaceCollection
    Dim mTravelerStatusReasons As TravelerStatusReasonsCollection

    ' Dim mDiskPlatformStatistics As DominoDiskPlatformStatisticsCollection
    Dim mAvailabilityIndex, mAvailabilityThreshold As Integer
    '2/22/2016 NS added for VSPLUS-2641
    Dim mAvailabilityIndexThreshold As Integer
    Dim mExpansionFactor As Integer
    Dim mVersion, mOS As String
    Dim mCommonName As String
    Dim mMemory As String 'stores the status of memory
    Dim mMemoryPercentUsed As Integer  'the percent of memory usage that triggers an alert, i.e., 80
    Dim mTaskStatus As String 'stores the status of the server tasks

    'Mail Related
    Dim mCountMailFiles As Boolean 'whether to count the mail files on this server or not
    Dim mAdvancedMailScan As Boolean 'whether to check for OOO agent and other advanced properties
    Dim mDeleteDeadThreshold As Integer 'if not zero, delete dead mail if greater than this number
    Dim mMailFileCount As Integer 'the number of mail files on the server
    Dim mPendingMail, mDeadMail, mHeldMail, mHeldThreshold, mPendingThreshold, mDeadThreshold, mMailboxCount As Integer
    Dim mPeakPendingMail, mPeakDeadMail As Integer
    Dim mPeakPendingTime, mPeakDeadTime As DateTime
    Dim mPriorPending, mPriorDead As Integer
    Dim mLocation As String 'mail server, app server, etc.
    Dim mMailDirectory As String
    Dim mDeliveredMail, mRoutedMail, mTransferredMail, mSMTPMailProcessed, mMailTransferFailures As Long
    Dim mMailFileScanDate As DateTime
    Dim mCriticalMailboxIndexCount As Integer   ' The Mail.Box Performance Index, which should be under 2
    Dim mMailBoxPerformanceIndex As Double

    Dim mCheckMailThreshold As Integer 'VSPLUS-913,Mukund 22Oct14, Stop counting mail, if over threshold
    Dim mRestartRouter As Boolean 'VSPLUS-915, 11/14/2014, restart router
    'Cluster Related
    Dim mClusterMember As String  'is the server a member of a cluster; if yes, which one?
    Dim mReplicaClusterSecondsOnQueue As Integer
    Dim mReplicaClusterWorkQueueDepth As Integer
    Dim mReplicaClusterSecondsOnQueueAvg As Integer
    Dim mReplicaClusterWorkQueueDepthAvg As Integer
    Dim mReplicaClusterSecondsOnQueueMax As Integer
    Dim mReplicaClusterWorkQueueDepthMax As Integer

    Dim mClusterOpenRedirectsFailoverSuccessful, mPreviousClusterOpenRedirectsFailoverSuccessful As Long
    Dim mClusterOpenRedirectsFailoverUnsuccessful, mPreviousClusterOpenFailoverRedirectsUnuccessful As Long

    Dim mClusterOpenRedirectsLoadBalanceSuccessful, mPreviousClusterOpenRedirectsLoadBalanceSuccessful As Long
    Dim mClusterOpenRedirectsLoadBalanceUnsuccessful, mPreviousClusterOpenLoadBalanceRedirectsUnuccessful As Long

    'Used to track stats on the server
    Dim mPreviousDeliveredMail, mPreviousRoutedMail, mPreviousTransferredMail As Long
    Dim mSMTPMessagesPrevious, mMailFailuresPrevious As Long

    '6/18/2015 NS added for VSPLUS-1802
    Dim mEXJEnabled As Boolean
    Dim mEXJStartTime As String
    Dim mEXJDuration As Integer
    Dim mEXJLookBackDuration As Integer

    '10/9/2015 NS added for VSPLUS-2252
    Dim mElapsedTime As Long
    Dim mVersionArchitecture As String
    Dim mCPUCount As Integer

    Public Property LastPing As DateTime
    Public Property ScanCount As Integer
    Public Property CPU_Threshold As Double
    Public Property Memory_Threshold As Double
    Public Property ClusterRep_Threshold As Double
    Public Property LastLogDocScanned As String  'Used to remember which log file document you last scanned so you can start on the next one
    Public Property LastAgentLogDocScanned As String   'Same as above, except for the Agentlog.nsf database
    '7/15/2016 NS added for VSPLUS-3120
    Public Property LogLineCounter As Integer
    '8/12/2016 NS added for VSPLUS-3167
    Public Property LastDocCreatedDate As DateTime
    Public Property IsLogFileBeingScanned As Boolean
    Public Property EXJournal1_DocCount As Long
    Public Property EXJournal2_DocCount As Long
    Public Property EXJournal_DocCount As Long
    Public Property HTTP_Configured_Max_Sessions As Integer 'Configured as the maximum number of concurrent http threads allowed
    Public Property HTTP_Actual_Max_Sessions As Integer  'The maximum number used by the server
    Public Property PingCount As Integer
    Public Property Load_ClusterRep_Threshold As Double

    Public Property HTTP_UserName As String
    Public Property HTTP_Password As String


    Public Property Port_50125_Status As String
    Public Property ConsecutiveTelnetCount As Integer
    Public Property ConsecutiveTelnetCountThreshold As Integer
    Public Property scanASAP As Boolean = False

    Public Property scanServlet As Boolean
    '3/28/2016 NS added for VSPLUS-2669
    Public Property ServerTimeTZ As Integer = 1000
    Public Property ServerTimeDST As Boolean = False

    Public Property ServerDaysAlert As Integer
        Get
            Return mServersDayAlert
        End Get
        Set(ByVal value As Integer)
            mServersDayAlert = value
        End Set
    End Property

    'VSPLUS-913,Mukund 22Oct14, Stop counting mail, if over threshold
    Public Property MailChecking As Integer
        Get
            Return mCheckMailThreshold

        End Get
        Set(ByVal value As Integer)
            mCheckMailThreshold = value
        End Set
    End Property


    'Used by Database Health
    Public Property LastDBHealthScan As DateTime

    Public Property Title As String

        Get
            Return mTitle
        End Get
        Set(ByVal value As String)
            mTitle = value
        End Set
    End Property

    Public Property RestartRouter() As Boolean
        Get
            Return mRestartRouter
        End Get
        Set(ByVal value As Boolean)
            mRestartRouter = value
        End Set
    End Property

    Public Property HierarchicalName As String
        Get
            Return mHierarchicalName
        End Get
        Set(ByVal value As String)
            mHierarchicalName = value
        End Set
    End Property


    Public Property ScanDBHealth As Boolean
        Get
            Return mScanDBHealth
        End Get
        Set(ByVal value As Boolean)
            mScanDBHealth = value
        End Set
    End Property

    Public Property QuickrPlaceCount As Long
        Get
            Return mQuickrPlaceCount
        End Get
        Set(ByVal value As Long)
            mQuickrPlaceCount = value
        End Set
    End Property

    Public Property QuickrServer As Boolean
        Get
            Return mQuickrServer
        End Get
        Set(ByVal value As Boolean)
            mQuickrServer = value
        End Set
    End Property
    '6/18/2015 NS added for VSPLUS-1802
    Public Property EXJEnabled As Boolean
        Get
            Return mEXJEnabled
        End Get
        Set(ByVal Value As Boolean)
            mEXJEnabled = Value
        End Set
    End Property
    Public Property EXJStartTime As String
        Get
            Return mEXJStartTime
        End Get
        Set(ByVal value As String)
            mEXJStartTime = value
        End Set
    End Property
    Public Property EXJDuration As Integer
        Get
            Return mEXJDuration
        End Get
        Set(ByVal value As Integer)
            mEXJDuration = value
        End Set
    End Property
    Public Property EXJLookBackDuration As Integer
        Get
            Return mEXJLookBackDuration
        End Get
        Set(ByVal value As Integer)
            mEXJLookBackDuration = value
        End Set
    End Property
    '10/0/2015 NS added for VSPLUS-2252
    Public Property ElapsedTime As Long
        Get
            Return mElapsedTime
        End Get
        Set(ByVal value As Long)
            mElapsedTime = value
        End Set
    End Property
    Public Property VersionArchitecture As String
        Get
            Return mVersionArchitecture
        End Get
        Set(ByVal value As String)
            mVersionArchitecture = value
        End Set
    End Property
    Public Property CPUCount As Integer
        Get
            Return mCPUCount
        End Get
        Set(ByVal value As Integer)
            mCPUCount = value
        End Set
    End Property

#Region "Cluster Properties"

    Public Property ClusterMember() As String
        Get
            Return mClusterMember
        End Get
        Set(ByVal Value As String)
            mClusterMember = Value
        End Set
    End Property


#Region "Cluster Statistics"

#Region "Seconds on Queue"
    Public Property ReplicaClusterSecondsOnQueueAvg As Integer
        Get
            Return mReplicaClusterSecondsOnQueueAvg
        End Get
        Set(ByVal value As Integer)
            mReplicaClusterSecondsOnQueueAvg = value
        End Set
    End Property

    Public Property ReplicaClusterSecondsOnQueueMax As Integer
        Get
            Return mReplicaClusterSecondsOnQueueMax
        End Get
        Set(ByVal value As Integer)
            mReplicaClusterSecondsOnQueueMax = value
        End Set
    End Property

    Public Property ReplicaClusterSecondsOnQueue As Integer
        Get
            Return mReplicaClusterSecondsOnQueue
        End Get
        Set(ByVal value As Integer)
            mReplicaClusterSecondsOnQueue = value
        End Set
    End Property

#End Region

#Region "Work Queue Depth"

    Public Property ReplicaClusterWorkQueueDepth As Integer
        Get
            Return mReplicaClusterWorkQueueDepth
        End Get
        Set(ByVal value As Integer)
            mReplicaClusterWorkQueueDepth = value
        End Set
    End Property

    Public Property ReplicaClusterWorkQueueDepthAvg As Integer
        Get
            Return mReplicaClusterWorkQueueDepthAvg
        End Get
        Set(ByVal value As Integer)
            mReplicaClusterWorkQueueDepthAvg = value
        End Set
    End Property
    Public Property ReplicaClusterWorkQueueDepthMax As Integer
        Get
            Return mReplicaClusterWorkQueueDepthMax
        End Get
        Set(ByVal value As Integer)
            mReplicaClusterWorkQueueDepthMax = value
        End Set
    End Property
#End Region


    Public Property ClusterOpenRedirects_Previous_FailoverSuccessful() As Long
        Get
            Return mPreviousClusterOpenRedirectsFailoverSuccessful
        End Get
        Set(ByVal Value As Long)
            mPreviousClusterOpenRedirectsFailoverSuccessful = Value
        End Set
    End Property

    Public Property ClusterOpenRedirectsFailoverSuccessful() As Long
        Get
            Return mClusterOpenRedirectsFailoverSuccessful
        End Get
        Set(ByVal Value As Long)
            mClusterOpenRedirectsFailoverSuccessful = Value
        End Set
    End Property

    Public Property ClusterOpenRedirects_Previous_FailoverUnSuccessful() As Long
        Get
            Return mPreviousClusterOpenFailoverRedirectsUnuccessful
        End Get
        Set(ByVal Value As Long)
            mPreviousClusterOpenFailoverRedirectsUnuccessful = Value
        End Set
    End Property

    Public Property ClusterOpenRedirectsFailoverUnuccessful() As Long
        Get
            Return mClusterOpenRedirectsFailoverUnsuccessful
        End Get
        Set(ByVal Value As Long)
            mClusterOpenRedirectsFailoverUnsuccessful = Value
        End Set
    End Property

    'Load Balancing
    Public Property ClusterOpenRedirectsLoadBalanceSuccessful() As Long
        Get
            Return mClusterOpenRedirectsLoadBalanceSuccessful
        End Get
        Set(ByVal Value As Long)
            mClusterOpenRedirectsLoadBalanceSuccessful = Value
        End Set
    End Property

    Public Property ClusterOpenRedirects_Previous_LoadBalanceSuccessful() As Long
        Get
            Return mPreviousClusterOpenRedirectsLoadBalanceSuccessful
        End Get
        Set(ByVal Value As Long)
            mPreviousClusterOpenRedirectsLoadBalanceSuccessful = Value
        End Set
    End Property

    Public Property ClusterOpenRedirectsLoadBalanceUnSuccessful() As Long
        Get
            Return mClusterOpenRedirectsLoadBalanceUnsuccessful
        End Get
        Set(ByVal Value As Long)
            mClusterOpenRedirectsLoadBalanceUnsuccessful = Value
        End Set
    End Property

    Public Property ClusterOpenRedirects_Previous_LoadBalanceUnSuccessful() As Long
        Get
            Return mPreviousClusterOpenLoadBalanceRedirectsUnuccessful
        End Get
        Set(ByVal Value As Long)
            mPreviousClusterOpenLoadBalanceRedirectsUnuccessful = Value
        End Set
    End Property
#End Region


#End Region

#Region "Availability"
    Public Property AvailabilityIndex() As Integer
        Get
            Return mAvailabilityIndex
        End Get
        Set(ByVal Value As Integer)
            mAvailabilityIndex = Value
        End Set
    End Property

    Public Property AvailabilityThreshold() As Integer
        Get
            Return mAvailabilityThreshold
        End Get
        Set(ByVal Value As Integer)
            mAvailabilityThreshold = Value
        End Set
    End Property

    '2/22/2016 NS added for VSPLUS-2641
    Public Property AvailabilityIndexThreshold() As Integer
        Get
            Return mAvailabilityIndexThreshold
        End Get
        Set(ByVal Value As Integer)
            mAvailabilityIndexThreshold = Value
        End Set
    End Property

    Public Property ExpansionFactor() As Integer
        Get
            Return mExpansionFactor
        End Get
        Set(ByVal Value As Integer)
            mExpansionFactor = Value
        End Set
    End Property

#End Region

#Region "Domino.Command Values"
    Public Property priorDominoOpenDocument As Integer
    Public Property priorDominoDeleteDocument As Integer
    Public Property priorDominoCreateDocument As Integer
    Public Property priorDominoOpenDatabase As Integer
    Public Property priorDominoOpenView As Integer
    Public Property priorDominoTotal As Integer
#End Region

#Region "Mail Properties"

    Public Property MailBoxPerformanceIndex As Double

        Get
            Return mMailBoxPerformanceIndex
        End Get
        Set(ByVal value As Double)
            mMailBoxPerformanceIndex = value
        End Set
    End Property

    Public Property MailFileCount() As Integer
        Get
            Return mMailFileCount
        End Get
        Set(ByVal Value As Integer)
            mMailFileCount = Value
        End Set
    End Property

    Public Property MailFileScanDate() As DateTime
        Get
            Return mMailFileScanDate
        End Get
        Set(ByVal Value As DateTime)
            mMailFileScanDate = Value
        End Set
    End Property

    Public Property MailDirectory() As String
        Get
            Return mMailDirectory
        End Get
        Set(ByVal Value As String)
            mMailDirectory = Value
        End Set
    End Property

    Public Property MailboxCount() As Integer
        Get
            Return mMailboxCount
        End Get
        Set(ByVal Value As Integer)
            mMailboxCount = Value
        End Set
    End Property

    Public Property PreviousSMTPMessages() As Long
        Get
            Return mSMTPMessagesPrevious
        End Get
        Set(ByVal Value As Long)
            mSMTPMessagesPrevious = Value
        End Set
    End Property

    Public Property PreviousMailFailures() As Long
        Get
            Return mMailFailuresPrevious
        End Get
        Set(ByVal Value As Long)
            mMailFailuresPrevious = Value
        End Set
    End Property

    Public Property PreviousTransferredMail() As Long
        Get
            Return mPreviousTransferredMail
        End Get
        Set(ByVal Value As Long)
            mPreviousTransferredMail = Value
        End Set
    End Property

    Public Property PreviousRoutedMail() As Long
        Get
            Return mPreviousRoutedMail
        End Get
        Set(ByVal Value As Long)
            mPreviousRoutedMail = Value
        End Set
    End Property

    Public Property PreviousDeliveredMail() As Long
        Get
            Return mPreviousDeliveredMail
        End Get
        Set(ByVal Value As Long)
            mPreviousDeliveredMail = Value
        End Set
    End Property

    Public Property DeliveredMail() As Long
        Get
            Return mDeliveredMail
        End Get
        Set(ByVal Value As Long)
            If Value > 0 Then
                mDeliveredMail = Value
            Else
                mDeliveredMail = 0
            End If
        End Set
    End Property

    Public Property MailTransferFailures() As Long
        Get
            Return mMailTransferFailures
        End Get
        Set(ByVal Value As Long)
            If Value > 0 Then
                mMailTransferFailures = Value
            Else
                mMailTransferFailures = 0
            End If
        End Set
    End Property

    Public Property SMTPMailProcessed() As Long
        Get
            Return mSMTPMailProcessed
        End Get
        Set(ByVal Value As Long)
            If Value > 0 Then
                mSMTPMailProcessed = Value
            Else
                mSMTPMailProcessed = 0
            End If
        End Set
    End Property

    Public Property TransferredMail() As Long
        Get
            Return mTransferredMail
        End Get
        Set(ByVal Value As Long)
            If Value > 0 Then
                mTransferredMail = Value
            Else
                mTransferredMail = 0
            End If
        End Set
    End Property

    Public Property RoutedMail() As Long
        Get
            Return mRoutedMail
        End Get
        Set(ByVal Value As Long)
            If Value > 0 Then
                mRoutedMail = Value
            Else
                mRoutedMail = 0
            End If
        End Set
    End Property

    Public Sub ResetMailValues()
        mRoutedMail = 0
        mDeliveredMail = 0
        mTransferredMail = 0
        mMailTransferFailures = 0
        mSMTPMailProcessed = 0
    End Sub

    Public ReadOnly Property PriorPendingMail() As Integer
        'returns the value of pending mail in the prior cycle
        Get
            Return mPriorPending
        End Get
    End Property

    Public ReadOnly Property PriorDeadMail() As Integer
        'returns the value of dead mail in the prior cycle
        Get
            Return mPriorDead
        End Get
    End Property

    Public ReadOnly Property PeakPendingMail() As Integer
        Get
            Return mPeakPendingMail
        End Get
    End Property

    Public ReadOnly Property PeakPendingTime() As DateTime
        Get
            Return mPeakPendingTime
        End Get
    End Property

    Public Property PendingMail() As Integer
        'The current value of pending mail on the server
        Get
            Return mPendingMail
        End Get
        Set(ByVal Value As Integer)
            mPriorPending = mPendingMail
            mPendingMail = Value
            If Value > mPeakPendingMail Then
                mPeakPendingMail = Value
                mPeakPendingTime = Now
            End If
        End Set
    End Property

    Public ReadOnly Property PeakDeadMail() As Integer
        Get
            Return mPeakDeadMail
        End Get
    End Property

    Public ReadOnly Property PeakDeadTime() As DateTime
        Get
            Return mPeakDeadTime
        End Get
    End Property

    Public Property DeadMail() As Integer
        'the current value of dead mail on the server
        Get
            Return mDeadMail
        End Get
        Set(ByVal Value As Integer)
            mPriorDead = mDeadMail
            mDeadMail = Value
            If Value > mPeakDeadMail Then
                mPeakDeadMail = Value
                mPeakDeadTime = Now
            End If
        End Set
    End Property


    Public Property HeldMail() As Integer
        'the current value of dead mail on the server
        Get
            Return mHeldMail
        End Get
        Set(ByVal Value As Integer)

            mHeldMail = Value

        End Set
    End Property

    Public Property HeldThreshold() As Integer
        Get
            Return mHeldThreshold
        End Get
        Set(ByVal Value As Integer)
            If Value >= 0 Then
                mHeldThreshold = Value
            Else
                mHeldThreshold = 25
            End If
        End Set
    End Property



    Public Property PendingThreshold() As Integer
        'The value of pending mail that triggers an alert
        Get
            Return mPendingThreshold
        End Get
        Set(ByVal Value As Integer)
            If Value >= 0 Then
                mPendingThreshold = Value
            Else
                mPendingThreshold = 50  'default value
            End If

        End Set
    End Property

    Public Property DeadThreshold() As Integer
        Get
            Return mDeadThreshold
        End Get
        Set(ByVal Value As Integer)
            If Value >= 0 Then
                mDeadThreshold = Value
            Else
                mDeadThreshold = 25
            End If
        End Set
    End Property

    Public Property DeleteDeadThreshold() As Integer
        Get
            Return mDeleteDeadThreshold
        End Get
        Set(ByVal Value As Integer)
            If Value >= 0 Then
                mDeleteDeadThreshold = Value
            Else
                mDeleteDeadThreshold = 0
            End If
        End Set
    End Property


    Public Property CountMailFiles() As Boolean
        Get
            Return (mCountMailFiles)
        End Get
        Set(ByVal Value As Boolean)
            mCountMailFiles = Value
        End Set
    End Property

    Public Property AdvancedMailScan() As Boolean
        Get
            Return mAdvancedMailScan
        End Get
        Set(ByVal Value As Boolean)
            mAdvancedMailScan = Value
        End Set
    End Property

    Public Property CriticalMailboxIndexCount As Integer
        'used to keep track of how many times we figured they need another mail.box file
        Get
            Return mCriticalMailboxIndexCount
        End Get
        Set(ByVal value As Integer)
            mCriticalMailboxIndexCount = value
        End Set
    End Property

#End Region

#Region "Server Tasks"

    Public Property ShowTaskStringsToSearchFor() As String
        'some Admins want to search the SH TA string for certain words
        Get
            Return mTaskStrings
        End Get
        Set(ByVal Value As String)
            mTaskStrings = Value
        End Set
    End Property

    Public Property ShowTasks() As String
        'used to store the results of a show tasks command
        Get
            Return mSHOWTASKS
        End Get
        Set(ByVal Value As String)
            mSHOWTASKS = Value
        End Set
    End Property

    Public Property ServerTasks() As ServerTasksCollection
        Get
            Return mTaskCollection
        End Get

        Set(ByVal Value As ServerTasksCollection)
            mTaskCollection = Value
        End Set
    End Property

    Public Property ServerTaskSettings() As ServerTaskSettingCollection

        Get
            Return mTaskSettings
        End Get

        Set(ByVal Value As ServerTaskSettingCollection)
            mTaskSettings = Value
        End Set
    End Property

    Public Property TravelerStatusReasons() As TravelerStatusReasonsCollection

        Get
            Return mTravelerStatusReasons
        End Get

        Set(ByVal Value As TravelerStatusReasonsCollection)
            mTravelerStatusReasons = Value
        End Set
    End Property


#End Region

    Public Property MemoryPercentUsed As Integer

        Get
            Return mMemoryPercentUsed
        End Get
        Set(ByVal value As Integer)
            mMemoryPercentUsed = value
        End Set
    End Property

    Public Property Memory() As String
        Get
            Return mMemory
        End Get
        Set(ByVal Value As String)
            mMemory = Value
        End Set
    End Property

    Public Property DiskThreshold() As Double
        Get
            Return mDiskThreshold
        End Get
        Set(ByVal Value As Double)
            mDiskThreshold = Value
        End Set
    End Property

    Public Property TaskStatus() As String
        Get
            Return mTaskStatus
        End Get
        Set(ByVal Value As String)
            mTaskStatus = Value
        End Set
    End Property

    Public Property CommonName() As String
        Get
            Return mCommonName
        End Get
        Set(ByVal Value As String)
            mCommonName = Value
        End Set
    End Property

    Public Property OperatingSystem() As String
        Get
            Return mOS
        End Get
        Set(ByVal Value As String)
            mOS = Value
        End Set
    End Property

    Public Property VersionDomino() As String
        Get
            Return mVersion
        End Get
        Set(ByVal Value As String)
            mVersion = Value
        End Set
    End Property

    Public Property RemoteConsoleAccess() As Boolean
        Get
            Return mRemoteConsoleAccess
        End Get
        Set(ByVal Value As Boolean)
            mRemoteConsoleAccess = Value
        End Set
    End Property

#Region "Traveler"

    Public Property Traveler_DeviceCount As Long
    Public Property Traveler_UserCount As Long
    Public Property Traveler_Version As String
    Public Property Traveler_Overall_Health As String
    Public Property Traveler_Availability_Index As Long
    '10/8/2015 NS added for VSPLUS-2208
    Public Property Traveler_Memory_Java As Long
    Public Property Traveler_Memory_C As Long

    Public ReadOnly Property Traveler_Ideal_HTTP_Worker_Count As Long
        Get
            If Traveler_DeviceCount > 0 Then
                Return Traveler_DeviceCount * 1.2
            Else
                Return 0
            End If
        End Get
    End Property

    Public Property Traveler_Server_HA As Boolean

    Public Property Traveler_Server() As Boolean
        Get
            Return mTravelerServer
        End Get
        Set(ByVal Value As Boolean)
            mTravelerServer = Value
        End Set
    End Property
    Public Property TravelerHA_Pool_Name As String

    Public Property Traveler_User_Scanning As Boolean
    'Set to true if a thread has been assigned to scan traveler users for this server

    Public ReadOnly Property Traveler_Previous_Successful_DeviceSync_Count As Long
        Get
            Return mPrior_Traveler_Successful_DeviceSync_Count
        End Get
    End Property

    Public ReadOnly Property Traveler_Previous_Successful_DeviceSync_Count_Updated_Time As DateTime
        Get
            Return mPrior_Traveler_Successful_DeviceSync_Count_Updated_Time
        End Get
    End Property

    Public ReadOnly Property Traveler_Seconds_Since_Last_Sync As Double
        Get
            Dim ScanInterval As TimeSpan
            Dim tNow As New DateTime
            tNow = Now
            ScanInterval = tNow.Subtract(mPrior_Traveler_Successful_DeviceSync_Count_Updated_Time)
            Return ScanInterval.TotalSeconds
        End Get
    End Property

    Public Property Traveler_Successful_DeviceSync_Count As Long
        Get
            Return mTraveler_Successful_DeviceSync_Count
        End Get

        Set(ByVal value As Long)

            'if the server has reset since the last scan, the prior value will be higher.  We can ignore this condition
            If mPrior_Traveler_Successful_DeviceSync_Count > value Then
                mPrior_Traveler_Successful_DeviceSync_Count = value - 1
                mPrior_Traveler_Successful_DeviceSync_Count_Updated_Time = Now
            ElseIf mPrior_Traveler_Successful_DeviceSync_Count = 0 Then
                'if this is the first time VS has scanned this server, the prior value will be 0.  We'll assume all is well the first time.
                mPrior_Traveler_Successful_DeviceSync_Count = value - 1
                mPrior_Traveler_Successful_DeviceSync_Count_Updated_Time = Now
            ElseIf mTraveler_Successful_DeviceSync_Count < value Then
                mPrior_Traveler_Successful_DeviceSync_Count_Updated_Time = Now
                mPrior_Traveler_Successful_DeviceSync_Count = mTraveler_Successful_DeviceSync_Count
                mTraveler_Successful_DeviceSync_Count = value
            End If

            mTraveler_Successful_DeviceSync_Count = value
        End Set
    End Property


    Public Property Traveler_Status As String
    Public Property Traveler_Status_Prior As String
    Public Property Traveler_Details As String

#End Region

#Region "Statistics"

    Public Property Statistics_All() As String
        'used to store a collection of stats ("show stat")
        Get
            Return mStatistics_All
        End Get
        Set(ByVal Value As String)
            mStatistics_All = Value
        End Set
    End Property

    Public Property Statistics_Mail() As String
        'used to store a collection of stats ("show stat mail")
        Get
            Return mStatistics_Mail
        End Get
        Set(ByVal Value As String)
            mStatistics_Mail = Value
        End Set
    End Property

    Public Property Statistics_Traveler As String
        'used to store a collection of stats ("show stat traveler")
        Get
            Return mStatistics_Traveler
        End Get
        Set(ByVal Value As String)
            mStatistics_Traveler = Value
        End Set
    End Property

    'mStatistics_Domino
    Public Property Statistics_Domino As String
        'used to store a collection of stats ("show stat http")
        Get
            Return mStatistics_Domino
        End Get
        Set(ByVal Value As String)
            mStatistics_Domino = Value
        End Set
    End Property

    Public Property Statistics_HTTP As String
        'used to store a collection of stats ("show stat http")
        Get
            Return mStatistics_Http
        End Get
        Set(ByVal Value As String)
            mStatistics_Http = Value
        End Set
    End Property

    Public Property Statistics_Misc() As String
        'used to store a collection of stats ("show stat")
        Get
            Return mStatistics_Misc
        End Get
        Set(ByVal Value As String)
            mStatistics_Misc = Value
        End Set
    End Property

    Public Property Statistics_Platform() As String
        'used to store a collection of stats ("show stat")
        Get
            Return mStatistics_Platform
        End Get
        Set(ByVal Value As String)
            mStatistics_Platform = Value
        End Set
    End Property


    Public Property Statistics_Database() As String
        'used to store a collection of stats ("show stat")
        Get
            Return mStatistics_Database
        End Get
        Set(ByVal Value As String)
            mStatistics_Database = Value
        End Set
    End Property

    Public Property Statistics_Server() As String
        'used to store a collection of stats ("show stat server")
        Get
            Return mStatistics_Server
        End Get
        Set(ByVal Value As String)
            mStatistics_Server = Value
        End Set
    End Property

    Public Property Statistics_Replica() As String
        'used to store a collection of stats ("show stat replica")
        Get
            Return mStatistics_Replica
        End Get
        Set(ByVal Value As String)
            mStatistics_Replica = Value
        End Set
    End Property

    Public Property Statistics_Disk() As String
        'used to store a collection of stats ("show stat disk")
        Get
            Return mStatistics_Disk
        End Get
        Set(ByVal Value As String)
            mStatistics_Disk = Value
        End Set
    End Property

    Public Property Statistics_Memory() As String
        'used to store a collection of stats ("show stat memory")
        Get
            Return mStatistics_Memory
        End Get
        Set(ByVal Value As String)
            mStatistics_Memory = Value
        End Set
    End Property

#End Region


    Public Property BES_Server() As Boolean
        Get
            Return mBES_Server
        End Get
        Set(ByVal Value As Boolean)
            mBES_Server = Value
        End Set
    End Property

    Public Property BES_Threshold() As Integer
        Get
            Return mBESthreshold
        End Get
        Set(ByVal Value As Integer)
            mBESthreshold = Value
        End Set
    End Property

    'Public Property DiskPlatformStatistics As DominoDiskPlatformStatisticsCollection
    '    Get
    '        Return mDiskPlatformStatistics
    '    End Get
    '    Set(ByVal value As DominoDiskPlatformStatisticsCollection)
    '        mDiskPlatformStatistics = value
    '    End Set
    'End Property


    Public Property DiskDrives As DominoDiskSpaceCollection

        Get
            Return mDiskDrives
        End Get
        Set(ByVal value As DominoDiskSpaceCollection)
            mDiskDrives = value
        End Set
    End Property

    Public Property CustomStatisticsSettings() As DominoStatisticsCollection
        Get
            Return mCustomStats
        End Get
        Set(ByVal Value As DominoStatisticsCollection)
            mCustomStats = Value
        End Set
    End Property


    Public ReadOnly Property PeakUserCount() As Integer
        Get
            Return mPeakUserCount
        End Get
    End Property

    Public ReadOnly Property PeakUserTime() As DateTime
        Get
            Return mPeakUserTime
        End Get
    End Property

    Public Property HttpCurrentConnections() As Integer
        Get
            Return mHTTPCurrentConnections
        End Get

        Set(ByVal value As Integer)
            mHTTPCurrentConnections = value
        End Set
    End Property
    Public Property RequireSSL() As Boolean
        Get
            Return mRequireSSL
        End Get

        Set(ByVal value As Boolean)
            mRequireSSL = value
        End Set
    End Property

    Public Property ExternalAlias() As String
        Get
            Return mExternalAlaias
        End Get

        Set(ByVal value As String)
            mExternalAlaias = value
        End Set
    End Property
    Public Property ScanTravelerServer() As Boolean
        Get
            Return mScanTravelerServer
        End Get

        Set(ByVal value As Boolean)
            mScanTravelerServer = value
        End Set
    End Property
    Public Property UserCount() As Integer
        Get
            Return mUserCount
        End Get
        Set(ByVal Value As Integer)
            If Value >= 0 Then
                mUserCount = Value
                If Value > mPeakUserCount Then
                    mPeakUserCount = Value
                    mPeakUserTime = Now
                End If
            Else
                mUserCount = 0
            End If
        End Set
    End Property

    Public Sub New(ByVal ServerName As String, ByVal Description As String)
        mPendingMail = 0
        mDeadMail = 0
        Me.Name = ServerName
        Me.Description = Description
    End Sub






End Class

Public Class SametimeServer
    Inherits MonitoredDevice

    Dim mSSL As Boolean = False
    Dim mNWayChat, mChat, mPlaces, mUsers As Integer
    Dim mNWayChatThreshold, mChatThreshold, mPlacesThreshold, mUsersThreshold As Integer
    Dim mStatistics As New SametimeStatisticsCollection
    Dim mEnumeratedProcesses As New SametimeRunningProcessCollection
    Dim mMonitoredProcesses As New SametimeMonitoredProcessCollection


    Public Property SSL As Boolean

        Get
            Return mSSL
        End Get
        Set(ByVal value As Boolean)
            mSSL = value
        End Set
    End Property

    Public Property MonitoredProcesses As SametimeMonitoredProcessCollection

        Get
            Return mMonitoredProcesses
        End Get
        Set(ByVal value As SametimeMonitoredProcessCollection)
            mMonitoredProcesses = value
        End Set
    End Property

    Public Property RunningProcesses As SametimeRunningProcessCollection

        Get
            Return mEnumeratedProcesses
        End Get
        Set(ByVal value As SametimeRunningProcessCollection)
            mEnumeratedProcesses = value
        End Set
    End Property

    Public Property StatisticsCollection As SametimeStatisticsCollection

        Get
            Return mStatistics
        End Get
        Set(ByVal value As SametimeStatisticsCollection)
            mStatistics = value
        End Set
    End Property

#Region "Thresholds"
    Public Property Chat_Sessions_Threshold() As Integer
        Get
            Return mChatThreshold
        End Get
        Set(ByVal Value As Integer)
            mChatThreshold = Value
        End Set
    End Property

    Public Property nWay_Chat_Sessions_Threshold() As Integer
        Get
            Return mNWayChatThreshold
        End Get
        Set(ByVal Value As Integer)
            mNWayChatThreshold = Value
        End Set
    End Property

    Public Property Places_Threshold() As Integer
        Get
            Return mPlacesThreshold
        End Get
        Set(ByVal Value As Integer)
            mPlacesThreshold = Value
        End Set
    End Property

    Public Property Users_Threshold() As Integer
        Get
            Return mUsersThreshold
        End Get
        Set(ByVal Value As Integer)
            mUsersThreshold = Value
        End Set
    End Property
#End Region

    Public Property Time_Login As Integer
    Public Property Time_Resolve As Integer


#Region "Tests"
    Public Property Test_Login As String
    Public Property Test_Resolve As String
    Public Property Test_StatusChange As String
    Public Property Test_IM As String
    Public Property Test_Awareness As String


#End Region

#Region "Values"
    Public Property Chat_Sessions() As Integer
        Get
            Return mChat
        End Get
        Set(ByVal Value As Integer)
            mChat = Value
        End Set
    End Property

    Public Property nWay_Chat_Sessions() As Integer
        Get
            Return mNWayChat
        End Get
        Set(ByVal Value As Integer)
            mNWayChat = Value
        End Set
    End Property

    Public Property Places() As Integer
        Get
            Return mPlaces
        End Get
        Set(ByVal Value As Integer)
            mPlaces = Value
        End Set
    End Property

    Public Property Users() As Integer
        Get
            Return mUsers
        End Get
        Set(ByVal Value As Integer)
            mUsers = Value
        End Set
    End Property

#End Region
    Public Property Platform As String

    Public Property WsScanMeetingServer As Boolean
    Public Property WsMeetingHost As String
    Public Property WsMeetingRequireSSL As Boolean
    Public Property WsMeetingPort As String

    Public Property WsScanMediaServer As Boolean
    Public Property WsMediaHost As String
    Public Property WsMediaRequireSSL As Boolean
    Public Property WsMediaPort As String
    Public Property UserId1 As String
    Public Property Password1 As String
    Public Property UserId2 As String
    Public Property Password2 As String
    Public Property CollectExtendedChat As Boolean
    Public Property ExtendedChatPort As String
    Public Property TestChatSimulation As Boolean

    Public Property DBHostName As String
    Public Property DBPort As String
    Public Property DBUserName As String
    Public Property DBPassword As String



End Class

Public Class DominoMailCluster
    Inherits MonitoredDevice
    Dim mServerAName, mServerBName, mServerCName As String
    Dim mServerADirectory, mServerBDirectory, mServerCDirectory As String
    Dim mServerAExcludeList, mServerBExcludeList, mServerCExcludeList As String
    Dim mFirstAlertThreshold, mSecondAlertThreshold As Double
    Dim mMissing_Replica_Alert As Boolean
    Dim mNSFCollection As DominoMailClusterDatabaseCollection
    Dim mTotalErrors As Long
    Dim mClusterID As String


#Region "Exclude Lists"

    'the exclude list is a comma separated list of filenames that should not be included in 
    'the analysis of the cluster, even if they otherwise match the directory 

    Public Property Server_A_ExcludeList() As String
        Get
            Return mServerAExcludeList
        End Get
        Set(ByVal Value As String)
            mServerAExcludeList = Value
        End Set
    End Property

    Public Property Server_B_ExcludeList() As String
        Get
            Return mServerBExcludeList
        End Get
        Set(ByVal Value As String)
            mServerBExcludeList = Value
        End Set
    End Property

    Public Property Server_C_ExcludeList() As String
        Get
            Return mServerCExcludeList
        End Get
        Set(ByVal Value As String)
            mServerCExcludeList = Value
        End Set
    End Property

#End Region

#Region "Server Names"

    Public Property Server_A_Name() As String
        Get
            Return mServerAName
        End Get
        Set(ByVal Value As String)
            mServerAName = Value
        End Set
    End Property

    Public Property Server_B_Name() As String
        Get
            Return mServerBName
        End Get
        Set(ByVal Value As String)
            mServerBName = Value
        End Set
    End Property

    Public Property Server_C_Name() As String
        Get
            Return mServerCName
        End Get
        Set(ByVal Value As String)
            mServerCName = Value
        End Set
    End Property


    Public Property ClusterID() As String
        Get
            Return mClusterID

        End Get
        Set(ByVal value As String)
            mClusterID = value
        End Set
    End Property

#End Region

#Region "Server Directories"
    'The directory is used as a match string; if 'mail' is specified then
    'the database \mail\tjones.nsf will be a match

    Public Property Server_A_Directory() As String
        Get
            Return mServerADirectory
        End Get
        Set(ByVal Value As String)
            mServerADirectory = Value
        End Set
    End Property


    Public Property Server_B_Directory() As String
        Get
            Return mServerBDirectory
        End Get
        Set(ByVal Value As String)
            mServerBDirectory = Value
        End Set
    End Property


    Public Property Server_C_Directory() As String
        Get
            Return mServerCDirectory
        End Get
        Set(ByVal Value As String)
            mServerCDirectory = Value
        End Set
    End Property
#End Region

#Region "Alerts"

    Public Property FirstAlertThreshold() As Double
        Get
            Return mFirstAlertThreshold
        End Get
        Set(ByVal Value As Double)
            mFirstAlertThreshold = Value
        End Set
    End Property

    Public Property SecondAlertThreshold() As Double
        Get
            Return mSecondAlertThreshold
        End Get
        Set(ByVal Value As Double)
            mSecondAlertThreshold = Value
        End Set
    End Property

    Public Property Missing_Replica_Alert() As Boolean
        Get
            Return mMissing_Replica_Alert
        End Get
        Set(ByVal Value As Boolean)
            mMissing_Replica_Alert = Value
        End Set
    End Property

#End Region

    Public Property DatabaseCollection() As DominoMailClusterDatabaseCollection
        Get
            Return mNSFCollection
        End Get

        Set(ByVal Value As DominoMailClusterDatabaseCollection)
            mNSFCollection = Value
        End Set
    End Property

    Public ReadOnly Property TotalDatabases() As Long
        Get
            Return DatabaseCollection.Count
        End Get
    End Property

    Public Property TotalDatabasesInError() As Long
        Get
            Return mTotalErrors
        End Get
        Set(ByVal Value As Long)
            mTotalErrors = Value
        End Set
    End Property

End Class

Public Class BlackBerryUser
    Inherits MonitoredDevice
    Dim mServer As String
    Dim mLastErrorTime As String
    Dim mLastErrorString As String
    Dim mWirelessNetwork As String
    Dim mInCradle As Boolean
    Dim mDisabled_In_BES As Boolean
    Dim mPendingMessages As Integer
    Dim mPendingThreshold, mExpiredThreshold As Integer
    Dim mExpiredMessages As Integer
    Dim mTotalMessages, mPreviousTotalMessages As Long
    Dim mFullyConfigured As Boolean
    Dim mLastInteraction As String
    Dim mInteractionMinutesThreshold As Integer
    Dim mPIN As String
    Dim mOID As Integer
    Dim mIsMonitored As Boolean = False

    Public Property IsMonitored() As Boolean
        Get
            Return mIsMonitored
        End Get
        Set(ByVal Value As Boolean)
            mIsMonitored = Value
        End Set
    End Property

    Public Property OID() As Integer
        Get
            Return mOID
        End Get
        Set(ByVal Value As Integer)
            mOID = Value
        End Set
    End Property

    Public Sub New(ByVal UserName As String)
        Me.Name = UserName
        '   Me.Server = Server
    End Sub

    Public Property PIN() As String
        Get
            Return mPIN
        End Get
        Set(ByVal Value As String)
            mPIN = Value
        End Set
    End Property

    Public Property Disabled_In_BES() As Boolean
        Get
            Return mDisabled_In_BES
        End Get
        Set(ByVal Value As Boolean)
            mOID = mDisabled_In_BES
        End Set
    End Property

    Public Property LastInteractionTime() As String
        Get
            Return mLastInteraction
        End Get
        Set(ByVal Value As String)
            mLastInteraction = Value
        End Set
    End Property

    Public Property LastInteractionTime_in_Minutes_Threshold() As Integer
        Get
            Return mInteractionMinutesThreshold
        End Get
        Set(ByVal Value As Integer)
            mInteractionMinutesThreshold = Value
        End Set
    End Property

    Public Property Fully_Configured() As Boolean
        Get
            Return mFullyConfigured
        End Get
        Set(ByVal Value As Boolean)
            mFullyConfigured = Value
        End Set
    End Property

    Public Property TotalMessagesProcessed() As Long
        Get
            Return mTotalMessages
        End Get
        Set(ByVal Value As Long)
            mTotalMessages = Value
        End Set
    End Property

    Public Property Previous_TotalMessagesProcessed() As Long
        Get
            Return mPreviousTotalMessages
        End Get
        Set(ByVal Value As Long)
            mPreviousTotalMessages = Value
        End Set
    End Property

    Public Property Expired_Messages() As Integer
        Get
            Return mExpiredMessages
        End Get
        Set(ByVal Value As Integer)
            mExpiredMessages = Value
        End Set
    End Property

    Public Property Pending_Messages() As Integer
        Get
            Return mPendingMessages
        End Get
        Set(ByVal Value As Integer)
            mPendingMessages = Value
        End Set
    End Property

    Public Property Expired_Messages_Theshold() As Integer
        Get
            Return mExpiredThreshold
        End Get
        Set(ByVal Value As Integer)
            mExpiredThreshold = Value
        End Set
    End Property

    Public Property Pending_Messages_Threshold() As Integer
        Get
            Return mPendingThreshold
        End Get
        Set(ByVal Value As Integer)
            mPendingThreshold = Value
        End Set
    End Property

    Public Property InCradle() As Boolean
        Get
            Return mInCradle
        End Get
        Set(ByVal Value As Boolean)
            mInCradle = False
        End Set
    End Property

    Public Property Wireless_Network() As String
        Get
            Return mWirelessNetwork
        End Get
        Set(ByVal Value As String)
            mWirelessNetwork = Value
        End Set
    End Property

    Public Property LastErrorString() As String
        Get
            Return mLastErrorString
        End Get
        Set(ByVal Value As String)
            mLastErrorString = Value
        End Set
    End Property

    Public Property LastErrorTime() As String
        Get
            Return mLastErrorTime
        End Get
        Set(ByVal Value As String)
            mLastErrorTime = Value
        End Set
    End Property

    Public Property Server() As String
        Get
            Return mServer
        End Get
        Set(ByVal Value As String)
            mServer = Value
        End Set
    End Property

End Class

Public Class BlackBerryServer
    Inherits MonitoredDevice
    Dim mSNMPCommunity, mAlertIP, mRouterIP, mAttachmentIP, mServicesList, mBEServerName As String
    Dim mBESVersion As String
    Dim mMessaging, mController, mDispatcher, mSynchronization, mPolicy, mMDS40, _
    mAttachment, mAlert, mRouter, mConnectedStated, MDSService41, MDSConnection41 As Boolean
    Dim mPendingMessages As Double
    Dim mSNMPFailureCount As Integer
    Dim mPreviousMsgsSent, mPreviousMsgsRecvd, mPreviousMsgsFiltered, mPreviousMsgsExpired As Integer
    Dim mMsgsSent, mMsgsRecvd, mMsgsFiltered, mMsgsExpired, mMsgsSentPerMinAvg As Integer
    Dim mMonitoredUsers, mAllUsers As MonitoredItems.BlackBerryUsersCollection
    Dim mTimeZone As Integer = 0
    Dim mDateFormat As Boolean = True
    Dim mTotalUsers, mUsersInError As Integer
    Dim mLicensesUsed As Integer
    Dim mServerPendingMailTheshold, mServerExpiredMailThreshold As Double
    Dim mUserPendingMailThreshold, mUserExpiredMailThreshold, mUserInteractionMinutesThreshold As Integer
    Public Property HAOption As String
    Public Property HAPartner As String


#Region "Generic User Tracking thresholds"
    Public Property UserInteractionMinutesThreshold() As Integer
        Get
            Return mUserInteractionMinutesThreshold
        End Get
        Set(ByVal Value As Integer)
            mUserInteractionMinutesThreshold = Value
        End Set
    End Property

    Public Property UserExpiredMailThreshold() As Integer
        Get
            Return mUserExpiredMailThreshold
        End Get
        Set(ByVal Value As Integer)
            mUserExpiredMailThreshold = Value
        End Set
    End Property

    Public Property UserPendingMailThreshold() As Integer
        'stores the value of pending mail threshold for users not specifically tracked (set by All Others)
        Get
            Return mUserPendingMailThreshold
        End Get
        Set(ByVal Value As Integer)
            mUserPendingMailThreshold = Value
        End Set
    End Property

    Public Property TotalUsers() As Integer
        Get
            Return mTotalUsers
        End Get
        Set(ByVal Value As Integer)
            mTotalUsers = Value
        End Set
    End Property

    Public Property UserInError() As Integer
        Get
            Return mUsersInError
        End Get
        Set(ByVal Value As Integer)
            mUsersInError = Value
        End Set
    End Property

#End Region

    Public Property LicensesUsed As Integer

        Get
            Return mLicensesUsed
        End Get
        Set(ByVal value As Integer)
            mLicensesUsed = value
        End Set
    End Property

    Public Property BES_Expired_Messages_Theshold() As Double
        Get
            Return mServerExpiredMailThreshold
        End Get
        Set(ByVal Value As Double)
            If Value >= 0 Then
                mServerExpiredMailThreshold = Value
            Else
                mServerExpiredMailThreshold = 0
            End If
        End Set
    End Property


    Public Property BES_Pending_Messages_Threshold() As Double
        Get
            Return mServerPendingMailTheshold
        End Get

        Set(ByVal Value As Double)
            If Value >= 0 Then
                mServerPendingMailTheshold = Value
            Else
                mServerPendingMailTheshold = 0
            End If
        End Set
    End Property

    Public Property TimeZoneAdjustment() As Integer
        Get
            Return mTimeZone
        End Get
        Set(ByVal Value As Integer)
            Try
                mTimeZone = Value
            Catch ex As Exception
                mTimeZone = 0
            End Try
        End Set
    End Property
    Public Property USDateFormat() As Boolean
        Get
            'if true, then 10/5/06 is October 5, 2006.  If False, then it is May 10
            Return mDateFormat
        End Get
        Set(ByVal Value As Boolean)
            mDateFormat = Value
        End Set
    End Property

    Public Property AllUsers() As MonitoredItems.BlackBerryUsersCollection
        Get
            Return mAllUsers
        End Get
        Set(ByVal Value As MonitoredItems.BlackBerryUsersCollection)
            mAllUsers = Value
        End Set
    End Property

    Public Property MonitoredUsers() As MonitoredItems.BlackBerryUsersCollection
        Get
            Return mMonitoredUsers
        End Get
        Set(ByVal Value As MonitoredItems.BlackBerryUsersCollection)
            mMonitoredUsers = Value
        End Set
    End Property

    Public Property BES_Total_Mgs_Sent_PerMinute() As Integer
        Get
            Return mMsgsSentPerMinAvg
        End Get
        Set(ByVal Value As Integer)
            mMsgsSentPerMinAvg = Value
        End Set
    End Property

    Public Property BES_Total_Messages_Xpired() As Integer
        Get
            Return mMsgsExpired
        End Get
        Set(ByVal Value As Integer)
            mMsgsExpired = Value
        End Set
    End Property

    Public Property BES_Total_Messages_Filtered() As Integer
        Get
            Return mMsgsFiltered
        End Get
        Set(ByVal Value As Integer)
            mMsgsFiltered = Value
        End Set
    End Property

    Public Property BES_Total_Messages_Rcvd() As Integer
        Get
            Return mMsgsRecvd
        End Get
        Set(ByVal Value As Integer)
            mMsgsRecvd = Value
        End Set
    End Property

    Public Property BES_Total_Messages_Sent() As Integer
        Get
            Return mMsgsSent
        End Get
        Set(ByVal Value As Integer)
            mMsgsSent = Value
        End Set
    End Property

    Public Property BES_Version() As String
        Get
            Return (mBESVersion)
        End Get
        Set(ByVal Value As String)
            mBESVersion = Value
        End Set
    End Property

    Public Property BES_ServerName() As String
        Get
            Return mBEServerName
        End Get
        Set(ByVal Value As String)
            mBEServerName = Value
        End Set
    End Property
    Public Property PreviousMessagesExpired() As Integer
        Get
            Return mPreviousMsgsExpired
        End Get
        Set(ByVal Value As Integer)
            mPreviousMsgsExpired = Value
        End Set
    End Property
    Public Property PreviousMessagesFiltered() As Integer
        Get
            Return mPreviousMsgsFiltered
        End Get
        Set(ByVal Value As Integer)
            mPreviousMsgsFiltered = Value
        End Set
    End Property

    Public Property PreviousMessagesReceived() As Integer
        Get
            Return mPreviousMsgsRecvd
        End Get
        Set(ByVal Value As Integer)
            mPreviousMsgsRecvd = Value
        End Set
    End Property
    Public Property PreviousMessagesSent() As Integer
        Get
            Return mPreviousMsgsSent
        End Get
        Set(ByVal Value As Integer)
            mPreviousMsgsSent = Value
        End Set
    End Property

    Public Property Windows_Services() As String
        Get
            Return mServicesList
        End Get
        Set(ByVal Value As String)
            mServicesList = Value
        End Set
    End Property

    Public Property SNMP_Failure_Count() As Integer
        Get
            Return mSNMPFailureCount
        End Get
        Set(ByVal Value As Integer)
            mSNMPFailureCount = Value
        End Set
    End Property

    Public Property Connected_To_SRP() As Boolean
        Get
            Return mConnectedStated
        End Get
        Set(ByVal Value As Boolean)
            mConnectedStated = Value
        End Set
    End Property

    Public Property PendingMessages() As Double
        Get
            Return mPendingMessages
        End Get
        Set(ByVal Value As Double)
            mPendingMessages = Value
        End Set
    End Property

    Public Property Attachment_Service_Address() As String
        Get
            Return mAttachmentIP
        End Get
        Set(ByVal Value As String)
            mAttachmentIP = Value
        End Set
    End Property

    Public Property Router_Service_Address() As String
        Get
            Return mRouterIP
        End Get
        Set(ByVal Value As String)
            mRouterIP = Value
        End Set
    End Property

    Public Property Alert_Service_Address() As String
        Get
            Return mAlertIP
        End Get
        Set(ByVal Value As String)
            mAlertIP = Value
        End Set
    End Property

    Public Property SNMP_Community() As String
        Get
            Return mSNMPCommunity
        End Get
        Set(ByVal Value As String)
            mSNMPCommunity = Value
        End Set
    End Property


    Public Property RouterService() As Boolean
        Get
            Return mRouter
        End Get
        Set(ByVal Value As Boolean)
            mRouter = Value
        End Set
    End Property

    Public Property AlertService() As Boolean
        Get
            Return mAlert
        End Get
        Set(ByVal Value As Boolean)
            mAlert = Value
        End Set
    End Property
    Public Property AttachmentService() As Boolean
        Get
            Return mAttachment
        End Get
        Set(ByVal Value As Boolean)
            mAttachment = Value
        End Set
    End Property
    'Available on 4.1.x and higher
    Public Property MDS_Services_BES41() As Boolean
        Get
            Return MDSService41
        End Get
        Set(ByVal Value As Boolean)
            MDSService41 = Value
        End Set
    End Property

    Public Property MDS_Connection_Service_BES41() As Boolean
        Get
            Return MDSConnection41
        End Get
        Set(ByVal Value As Boolean)
            MDSConnection41 = Value
        End Set
    End Property
    'Available on 4.0 only
    Public Property MobileDataService_BES40() As Boolean
        Get
            Return mMDS40
        End Get
        Set(ByVal Value As Boolean)
            mMDS40 = Value
        End Set
    End Property
    Public Property PolicyService() As Boolean
        Get
            Return mPolicy
        End Get
        Set(ByVal Value As Boolean)
            mPolicy = Value
        End Set
    End Property

    Public Property MessagingService() As Boolean
        Get
            Return mMessaging
        End Get
        Set(ByVal Value As Boolean)
            mMessaging = Value
        End Set
    End Property
    Public Property ControllerService() As Boolean
        Get
            Return mController
        End Get
        Set(ByVal Value As Boolean)
            mController = Value
        End Set
    End Property

    Public Property DispatcherService() As Boolean
        Get
            Return (mDispatcher)
        End Get
        Set(ByVal Value As Boolean)
            mDispatcher = Value
        End Set
    End Property

    Public Property SynchronizationService() As Boolean
        Get
            Return mSynchronization
        End Get
        Set(ByVal Value As Boolean)
            mSynchronization = Value
        End Set
    End Property
End Class

Public Class DNS_Server
    Inherits MonitoredDevice

    Enum DNSQueryEnum
        Address_Records = 1
        Name_Server_Records = 2
        Mail_Destination_Records = 3
        Mail_Forwarder_Records = 4
        Canonical_Name_Records = 5
        Start_Of_Authority_Records = 6
        MailBox_Records = 7
        Mail_Group_Records = 8
        MailBox_Rename_Records = 9
        NULL_Records = 10
        Well_Known_Services_Records = 11
        Pointer_Records = 12
        Host_Information_Records = 13
        Mail_Information_Records = 14
        Mail_Exchange_Records = 15
        Text_Records = 16
        Responsible_Person_Records = 17
        Service_Records = 18
        X25_Records = 19
        ISDN_Records = 20
        Route_Through_Records = 21
        AAAA_Record = 28
        SRV_Record = 33
    End Enum
    Dim mDNSQueryType As DNSQueryEnum
    Dim mDesiredResponsesString As String
    Dim mHostName As String
    Dim mARecord, mNSRecord As String


    Public Property HostNameToQuery() As String
        Get
            Return mHostName
        End Get
        Set(ByVal Value As String)
            mHostName = Value
        End Set
    End Property

    Public Property QueryType() As DNSQueryEnum
        Get
            Return mDNSQueryType
        End Get
        Set(ByVal Value As DNSQueryEnum)
            mDNSQueryType = Value
        End Set
    End Property

    Public Property AllowableResponses() As String
        Get
            Return mDesiredResponsesString
        End Get
        Set(ByVal Value As String)
            mDesiredResponsesString = Value
        End Set
    End Property

    Public Property A_Record_Responses() As String
        Get
            Return mARecord
        End Get
        Set(ByVal Value As String)
            mARecord = Value
        End Set
    End Property

    Public Property NS_Record_Responses() As String
        Get
            Return mNSRecord
        End Get
        Set(ByVal Value As String)
            mNSRecord = Value
        End Set
    End Property


End Class

Public Class NotesDatabase
    Inherits MonitoredDevice
    Dim mTriggers As String
    Dim mDocumentCount, mDocCountTrigger As Double
    Dim mDBSize, mDBSizeTrigger As Long
    Dim mServer, mFileName As String
    Dim mAboveBelow, mReplicationDestination As String
    Dim mReplicationCollection As DominoCollection
    Dim mInitiateReplication As Boolean
    Dim mDocumentID As String 'Used to store the doc ID of the document used for testing replication
    Dim mScanning As Boolean 'Used to track if the database is being scanned in a thread or not

#Region "Replication Monitoring"

    Public Property InitiateReplication() As Boolean
        Get
            Return mInitiateReplication
        End Get
        Set(ByVal Value As Boolean)
            mInitiateReplication = Value
        End Set
    End Property

    Public Property ReplicationServers() As DominoCollection
        'A collection of servers with which the Notes database should replicate
        Get
            Return mReplicationCollection
        End Get

        Set(ByVal Value As DominoCollection)
            mReplicationCollection = Value
        End Set
    End Property

    Public Property ReplicationDestination() As String
        Get
            Return mReplicationDestination
        End Get
        Set(ByVal Value As String)
            mReplicationDestination = Value
        End Set
    End Property

    Public Property DocumentID() As String
        Get
            Return mDocumentID
        End Get
        Set(ByVal Value As String)
            mDocumentID = Value
        End Set
    End Property
#End Region

    Public Property TriggerType() As String
        Get
            Return mTriggers
        End Get
        Set(ByVal Value As String)
            mTriggers = Value
        End Set
    End Property

    Public Property DocumentCount() As Double
        Get
            Return mDocumentCount
        End Get
        Set(ByVal Value As Double)
            mDocumentCount = Value
        End Set
    End Property

    Public Property DocumentCountTrigger() As Double
        Get
            Return mDocCountTrigger
        End Get
        Set(ByVal Value As Double)
            mDocCountTrigger = Value
        End Set
    End Property

    Public Property AboveBelow() As String
        Get
            Return mAboveBelow
        End Get
        Set(ByVal Value As String)
            mAboveBelow = Value
        End Set
    End Property

    Public Property DatabaseSize() As Long
        Get
            Return mDBSize
        End Get
        Set(ByVal Value As Long)
            mDBSize = Value
        End Set
    End Property

    Public Property DatabaseSizeTrigger() As Long
        Get
            Return mDBSizeTrigger
        End Get
        Set(ByVal Value As Long)
            mDBSizeTrigger = Value
        End Set
    End Property

    Public Property ServerName() As String
        Get
            Return mServer
        End Get
        Set(ByVal Value As String)
            mServer = Value
        End Set
    End Property

    Public Property FileName() As String
        Get
            Return mFileName
        End Get
        Set(ByVal Value As String)
            mFileName = Value
        End Set
    End Property

    Public Property Scanning As Boolean

        Get
            Return mScanning
        End Get
        Set(ByVal value As Boolean)
            mScanning = value
        End Set
    End Property
End Class

Public Class BlackBerryQueue
    Inherits DominoServer
    Dim mPendingMail, mPendingThreshold As Integer


    Public Sub New(ByVal ServerName As String)
        MyBase.New(ServerName, "BlackBerry Enterprise Server Pending Message Count")
    End Sub

End Class

Public Class NetworkDevice
    Inherits MonitoredDevice
    Public Property ID As Integer
    Public Property UserName As String
    Public Property Password As String
    Public Property NetworkType As String
    Public Property PowerSupplyStatus As String
    Public Property BoardTemp As String
    Public Property FanStatus As String
    Public Property OS As String
    Public Property Model As String
    Public Property Memory As String
    Public Property CPU As String
    Public Property SerialID As String
    Public Property StartTime As String
    Public Property UpTime As String
    Public Property LastRebootReason As String

    Public Sub New(ByVal Name As String)
        Me.Name = Name
    End Sub
End Class

Public Class MailService
    Inherits MonitoredDevice
    ' Private mPort As Integer
    Dim mServerName As String

    Enum MailTypeEnum
        IMAP = 1
        SMTP = 2
        POP3 = 3
        Other = 4
    End Enum

    Dim mMailType As MailTypeEnum

    Public Property ServerName As String
        Get
            Return mServerName
        End Get
        Set(ByVal value As String)
            mServerName = value
        End Set
    End Property


    Public Property MailType() As MailTypeEnum
        Get
            Select Case mMailType
                Case MailTypeEnum.IMAP
                    Return MailTypeEnum.IMAP
                Case MailTypeEnum.POP3
                    Return MailTypeEnum.POP3
                Case MailTypeEnum.SMTP
                    Return MailTypeEnum.SMTP
                Case MailTypeEnum.Other
                    Return MailTypeEnum.Other
                Case Else
                    Return Nothing
            End Select
        End Get
        Set(ByVal Value As MailTypeEnum)
            mMailType = Value
            Select Case mMailType
                Case MailTypeEnum.IMAP
                    Port = 143
                Case MailTypeEnum.POP3
                    Port = 110
                Case MailTypeEnum.SMTP
                    Port = 25
            End Select
        End Set
    End Property

End Class

Public Class BlackBerryMailProbe
    Inherits MonitoredDevice
    Dim mEmailAddress As String
    Dim mDeliveryThreshold As Integer
    Dim mTargetServer As String
    Dim mTargetDatabase As String
    Dim mSourceServer As String
    Dim mNotesMailAddress As String
    Dim mConfirmationServer As String
    Dim mConfirmationDatabase As String

    'Where the delivery confirmation message goes
    Public Property ConfirmationServer() As String
        Get
            Return mConfirmationServer
        End Get
        Set(ByVal Value As String)
            mConfirmationServer = Value
        End Set
    End Property

    Public Property ConfirmationDatabase() As String
        Get
            Return mConfirmationDatabase
        End Get
        Set(ByVal Value As String)
            mConfirmationDatabase = Value
        End Set
    End Property


    Public Property eMailAddress() As String
        Get
            Return mEmailAddress
        End Get
        Set(ByVal Value As String)
            mEmailAddress = Value
        End Set
    End Property

    Public Property NotesMailAddress() As String
        Get
            Return mNotesMailAddress
        End Get
        Set(ByVal Value As String)
            mNotesMailAddress = Value
        End Set
    End Property

    ' Where the initial email is sent to
    Public Property TargetServer() As String
        Get
            Return mTargetServer
        End Get
        Set(ByVal Value As String)
            mTargetServer = Value
        End Set
    End Property

    Public Property TargetDatabase() As String
        Get
            Return mTargetDatabase
        End Get
        Set(ByVal Value As String)
            mTargetDatabase = Value
        End Set
    End Property

    Public Property DeliveryThreshold() As Integer
        Get
            Return mDeliveryThreshold
        End Get
        Set(ByVal Value As Integer)
            mDeliveryThreshold = Value
        End Set
    End Property

    Public Property SourceServer() As String
        Get
            Return mSourceServer
        End Get
        Set(ByVal Value As String)
            mSourceServer = Value
        End Set
    End Property

    Public Sub New(ByVal Name As String)
        Me.Name = Name
    End Sub

End Class

Public Class DominoMailProbe
    Inherits BlackBerryMailProbe
    Public Property ReplyTo
    Dim mFilename As String
    Public Sub New(ByVal eMailAddress As String)
        MyBase.New(eMailAddress)
    End Sub
    Public Property FileName() As String
        Get
            Return mFilename
        End Get
        Set(ByVal Value As String)
            mFilename = Value
        End Set
    End Property

End Class

Public Class URL
    Inherits MonitoredDevice
    Dim mURL As String
    Dim mSearchString As String
    Public UserName As String
    Public Password As String
    Public HTML As String
    Public mAlertStringFound As String

    Public Property URL() As String
        Get
            Return (mURL)
        End Get
        Set(ByVal Value As String)
            mURL = Value
        End Set
    End Property

    Public Property SearchString() As String
        Get
            Return mSearchString
        End Get
        Set(ByVal Value As String)
            mSearchString = Value
        End Set
    End Property

    Public Property AlertStringFound() As String
        Get
            Return mAlertStringFound
        End Get
        Set(ByVal Value As String)
            mAlertStringFound = Value
        End Set
    End Property
End Class

'10/6/2014 NS added for VSPLUS-1002
Public Class Cloud
    Inherits MonitoredDevice
    Dim mCloudURL As String
    Dim mSearchString As String
    Public UserName As String
    Public Password As String
    Public HTML As String

    Public Property CloudURL() As String
        Get
            Return (mCloudURL)
        End Get
        Set(ByVal Value As String)
            mCloudURL = Value
        End Set
    End Property

    Public Property SearchString() As String
        Get
            Return mSearchString
        End Get
        Set(ByVal Value As String)
            mSearchString = Value
        End Set
    End Property

End Class

Public Class ServersQueue
    Inherits MonitoredDevice

End Class

Public Class WebSphere
    Inherits MonitoredDevice

    Public ID As Integer
    Public NodeID As String
    Public CellID As String
    Public HostName As String
    Public ServerName As String
    Public NodeName As String
    Public CellName As String
    Public CellHostName As String
    Public ConnectionType As String
    Public GlobalSecurity As Boolean
    Public SametimeID As Integer
    Public Realm As String
    Public ServerTypeID As Integer
    Public UserName As String
    Public Password As String
    Public CPU_Threshold As Integer
    Public Memory_Threshold As Integer
    Public ServerDaysAlert As Integer
    Public AverageThreadPoolThreshold As Double
    Public ActiveThreadCountThreshold As Integer
    Public CurrentHeapThreshold As Integer
    Public HeapSizeinitial As Integer
    Public HeapSizemaximum As Integer
    Public MaxHeapThreshold As Integer
    Public UpTimeThreshold As Double
    Public HungThreadCountThreshold As Integer
    Public DumpGeneratedThreshold As Integer
    Public AverageThreadPool As Double
    Public ActiveThreadCount As Integer
    Public CurrentHeap As Integer
    Public MaxHeap As Integer
    Public UpTime As Double
    Public HungThreadCount As Integer
    Public DumpGenerated As Integer
    Public Memory_Utilization As Double
    Public Memory_Free As Double
    Public Memory_Used As Double

    Public DeclaredThreadHungCount As Double
    Public DeclaredThreadHungCountThreshold As Double
    Public ClearedThreadHangCount As Double
    Public ClearedThreadHangCountThreshold As Double
    Public ConcurrentHungThreadCount As Double
    Public ConcurrentHungThreadCountThreshold As Double

    Public ProcessId As Integer


End Class

Public Class IBMConnect
    Inherits WebSphere

    Public DeviceType As String = "IBM Connections"
    Public DeviceTypeID As Int32 = 27

    Public TestCreateActivity As Boolean = True
    Public TestCreateBlog As Boolean = True
    Public TestCreateBookmarks As Boolean = True
    Public TestCreateCommunities As Boolean = True
    Public TestCreateFiles As Boolean = True
    Public TestCreateForums As Boolean = True
    Public TestSearchProfiles As Boolean = True
    Public TestCreateWikis As Boolean = True

    Public CreateActivityThreshold As Int32
    Public CreateBlogThreshold As Int32
    Public CreateBookmarkThreshold As Int32
    Public CreateCommunitiesThreshold As Int32
    Public CreateFilesThreshold As Int32
    Public CreateForumsThreshold As Int32
    Public SearchProfilesThreshold As Int32
    Public CreateWikisThreshold As Int32

    Public DBUserName As String
    Public DBPassword As String
    Public DBPort As String
    Public DBHostName As String


End Class


#End Region

#Region "Collections"

Public Class MonitoredDevicesCollection
    Inherits System.Collections.CollectionBase

    Public Sub Add(ByVal objItemToAdd As MonitoredDevice)
        Me.List.Add(objItemToAdd)
    End Sub

    Public ReadOnly Property Item(ByVal iIndex As Integer) As MonitoredDevice
        Get
            Return Me.List(iIndex)
        End Get
    End Property

    Public Function SearchByName(ByVal Name As String) As MonitoredDevice
        Dim iIndex As Integer
        Dim MyCommand As MonitoredItems.MonitoredDevice
        For iIndex = 0 To Me.List.Count - 1
            MyCommand = Me.List(iIndex)
            If MyCommand.Name.ToUpper = Name.ToUpper Then
                Return Me.List(iIndex)
                Exit Function
            End If
        Next
        Return Nothing
    End Function

    Public Function Delete(ByVal Name As String) As Boolean
        Dim iIndex As Integer
        Dim MyCommand As MonitoredItems.MonitoredDevice
        For iIndex = 0 To Me.List.Count - 1
            MyCommand = Me.List(iIndex)
            If MyCommand.Name = Name Then
                Try
                    Me.List.RemoveAt(iIndex)
                    Return True
                Catch ex As Exception
                    Return False
                End Try
                Exit Function
            End If
        Next
        Return Nothing
    End Function

End Class

Public Class ExchangeServersCollection
    Inherits MicrosoftServersCollection

    Public Overloads Sub Add(ByVal objItemToAdd As ExchangeServer)
        Me.List.Add(objItemToAdd)
    End Sub

    Public Overloads ReadOnly Property Item(ByVal iIndex As Integer) As ExchangeServer
        Get
            Return Me.List(iIndex)
        End Get
    End Property

    Public Overloads Function SearchByName(ByVal Name As String) As ExchangeServer
        Dim iIndex As Integer
        Dim MyCommand As MonitoredItems.ExchangeServer
        For iIndex = 0 To Me.List.Count - 1
            MyCommand = Me.List(iIndex)
            If MyCommand.Name.ToUpper = Name.ToUpper Then
                Return Me.List(iIndex)
                Exit Function
            End If
        Next
        Return Nothing
    End Function

    Public Overloads Function SearchByIPAddress(ByVal Name As String) As ExchangeServer
        Dim iIndex As Integer
        Dim MyCommand As MonitoredItems.ExchangeServer
        For iIndex = 0 To Me.List.Count - 1
            MyCommand = Me.List(iIndex)
            If MyCommand.IPAddress = Name Then
                Return Me.List(iIndex)
                Exit Function
            End If
        Next
        Return Nothing
    End Function

    Public Overloads Function Delete(ByVal Name As String) As Boolean
        Dim iIndex As Integer
        Dim MyCommand As MonitoredItems.ExchangeServer
        For iIndex = 0 To Me.List.Count - 1
            MyCommand = Me.List(iIndex)
            If MyCommand.Name = Name Then
                Try
                    Me.List.RemoveAt(iIndex)
                    Return True
                Catch ex As Exception
                    Return False
                End Try
                Exit Function
            End If
        Next
        Return Nothing
    End Function
End Class

Public Class MicrosoftServersCollection
    Inherits MonitoredDevicesCollection

    Public Overloads Sub Add(ByVal objItemToAdd As MicrosoftServer)
        Me.List.Add(objItemToAdd)
    End Sub

    Public Overloads ReadOnly Property Item(ByVal iIndex As Integer) As MicrosoftServer
        Get
            Return Me.List(iIndex)
        End Get
    End Property

    Public Overloads Function SearchByName(ByVal Name As String) As MicrosoftServer
        Dim iIndex As Integer
        Dim MyCommand As MonitoredItems.MicrosoftServer
        For iIndex = 0 To Me.List.Count - 1
            MyCommand = Me.List(iIndex)
            If MyCommand.Name.ToUpper = Name.ToUpper Then
                Return Me.List(iIndex)
                Exit Function
            End If
        Next
        Return Nothing
    End Function

    Public Overloads Function SearchByIPAddress(ByVal Name As String) As MicrosoftServer
        Dim iIndex As Integer
        Dim MyCommand As MonitoredItems.MicrosoftServer
        For iIndex = 0 To Me.List.Count - 1
            MyCommand = Me.List(iIndex)
            If MyCommand.IPAddress = Name Then
                Return Me.List(iIndex)
                Exit Function
            End If
        Next
        Return Nothing
    End Function

    Public Overloads Function Delete(ByVal Name As String) As Boolean
        Dim iIndex As Integer
        Dim MyCommand As MonitoredItems.MicrosoftServer
        For iIndex = 0 To Me.List.Count - 1
            MyCommand = Me.List(iIndex)
            If MyCommand.Name = Name Then
                Try
                    Me.List.RemoveAt(iIndex)
                    Return True
                Catch ex As Exception
                    Return False
                End Try
                Exit Function
            End If
        Next
        Return Nothing
    End Function
End Class

Public Class ActiveDirectoryServersCollection
    Inherits MicrosoftServersCollection

    Public Overloads Sub Add(ByVal objItemToAdd As ActiveDirectoryServer)
        Me.List.Add(objItemToAdd)
    End Sub

    Public Overloads ReadOnly Property Item(ByVal iIndex As Integer) As ActiveDirectoryServer
        Get
            Return Me.List(iIndex)
        End Get
    End Property

    Public Overloads Function SearchByName(ByVal Name As String) As ActiveDirectoryServer
        Dim iIndex As Integer
        Dim MyCommand As MonitoredItems.ActiveDirectoryServer
        For iIndex = 0 To Me.List.Count - 1
            MyCommand = Me.List(iIndex)
            If MyCommand.Name.ToUpper = Name.ToUpper Then
                Return Me.List(iIndex)
                Exit Function
            End If
        Next
        Return Nothing
    End Function

    Public Overloads Function SearchByIPAddress(ByVal Name As String) As ActiveDirectoryServer
        Dim iIndex As Integer
        Dim MyCommand As MonitoredItems.ActiveDirectoryServer
        For iIndex = 0 To Me.List.Count - 1
            MyCommand = Me.List(iIndex)
            If MyCommand.IPAddress = Name Then
                Return Me.List(iIndex)
                Exit Function
            End If
        Next
        Return Nothing
    End Function

    Public Overloads Function Delete(ByVal Name As String) As Boolean
        Dim iIndex As Integer
        Dim MyCommand As MonitoredItems.ActiveDirectoryServer
        For iIndex = 0 To Me.List.Count - 1
            MyCommand = Me.List(iIndex)
            If MyCommand.Name = Name Then
                Try
                    Me.List.RemoveAt(iIndex)
                    Return True
                Catch ex As Exception
                    Return False
                End Try
                Exit Function
            End If
        Next
        Return Nothing
    End Function
End Class
Public Class SharepointServersCollection
    Inherits MicrosoftServersCollection

    Public Overloads Sub Add(ByVal objItemToAdd As SharepointServer)
        Me.List.Add(objItemToAdd)
    End Sub

    Public Overloads ReadOnly Property Item(ByVal iIndex As Integer) As SharepointServer
        Get
            Return Me.List(iIndex)
        End Get
    End Property

    Public Overloads Function SearchByName(ByVal Name As String) As SharepointServer
        Dim iIndex As Integer
        Dim MyCommand As MonitoredItems.SharepointServer
        For iIndex = 0 To Me.List.Count - 1
            MyCommand = Me.List(iIndex)
            If MyCommand.Name.ToUpper = Name.ToUpper Then
                Return Me.List(iIndex)
                Exit Function
            End If
        Next
        Return Nothing
    End Function

    Public Overloads Function SearchByIPAddress(ByVal Name As String) As SharepointServer
        Dim iIndex As Integer
        Dim MyCommand As MonitoredItems.SharepointServer
        For iIndex = 0 To Me.List.Count - 1
            MyCommand = Me.List(iIndex)
            If MyCommand.IPAddress = Name Then
                Return Me.List(iIndex)
                Exit Function
            End If
        Next
        Return Nothing
    End Function

    Public Overloads Function Delete(ByVal Name As String) As Boolean
        Dim iIndex As Integer
        Dim MyCommand As MonitoredItems.SharepointServer
        For iIndex = 0 To Me.List.Count - 1
            MyCommand = Me.List(iIndex)
            If MyCommand.Name = Name Then
                Try
                    Me.List.RemoveAt(iIndex)
                    Return True
                Catch ex As Exception
                    Return False
                End Try
                Exit Function
            End If
        Next
        Return Nothing
    End Function
End Class
Public Class Office365ServersCollection
    Inherits MicrosoftServersCollection

    Public Overloads Sub Add(ByVal objItemToAdd As Office365Server)
        Me.List.Add(objItemToAdd)
    End Sub

    Public Overloads ReadOnly Property Item(ByVal iIndex As Integer) As Office365Server
        Get
            Return Me.List(iIndex)
        End Get
    End Property

    Public Overloads Function SearchByName(ByVal Name As String) As Office365Server
        Dim iIndex As Integer
        Dim MyCommand As MonitoredItems.Office365Server
        For iIndex = 0 To Me.List.Count - 1
            MyCommand = Me.List(iIndex)
            If MyCommand.Name.ToUpper = Name.ToUpper Then
                Return Me.List(iIndex)
                Exit Function
            End If
        Next
        Return Nothing
    End Function

    Public Overloads Function SearchByIPAddress(ByVal Name As String) As Office365Server
        Dim iIndex As Integer
        Dim MyCommand As MonitoredItems.Office365Server
        For iIndex = 0 To Me.List.Count - 1
            MyCommand = Me.List(iIndex)
            If MyCommand.IPAddress = Name Then
                Return Me.List(iIndex)
                Exit Function
            End If
        Next
        Return Nothing
    End Function

    Public Overloads Function Delete(ByVal Name As String) As Boolean
        Dim iIndex As Integer
        Dim MyCommand As MonitoredItems.Office365Server
        For iIndex = 0 To Me.List.Count - 1
            MyCommand = Me.List(iIndex)
            If MyCommand.Name = Name Then
                Try
                    Me.List.RemoveAt(iIndex)
                    Return True
                Catch ex As Exception
                    Return False
                End Try
                Exit Function
            End If
        Next
        Return Nothing
    End Function
End Class

#Region "Domino Collections"

'Public Class DominoDiskPlatformStatisticsCollection
'    Inherits System.Collections.CollectionBase

'    Public Sub Add(ByVal objItemToAdd As DominoDiskPlatformStatistic)
'        Me.List.Add(objItemToAdd)
'    End Sub

'    Public ReadOnly Property Item(ByVal iIndex As Integer) As DominoDiskPlatformStatistic
'        Get
'            Return Me.List(iIndex)
'        End Get
'    End Property

'    Public Function Search(ByVal DriveName As String) As DominoDiskPlatformStatistic
'        Dim iIndex As Integer
'        Dim MyDrive As MonitoredItems.DominoDiskPlatformStatistic
'        For iIndex = 0 To Me.List.Count - 1
'            MyDrive = Me.List(iIndex)
'            If MyDrive.DiskName = DriveName Then
'                Return Me.List(iIndex)
'                Exit Function
'            End If
'        Next
'        Return Nothing
'    End Function

'End Class


Public Class DominoDiskSpaceCollection
    Inherits System.Collections.CollectionBase

    Public Overloads Sub Add(ByVal objItemToAdd As DominoDiskSpace)
        If Not (InStr(objItemToAdd.DiskName, "patrol") > 0) And Not (InStr(objItemToAdd.DiskName, "domlog") > 0) Then
            Me.List.Add(objItemToAdd)
        End If

    End Sub

    Public Overloads ReadOnly Property Item(ByVal iIndex As Integer) As DominoDiskSpace
        Get
            Return Me.List(iIndex)
        End Get
    End Property

    Public Overloads Function Search(ByVal DriveName As String) As DominoDiskSpace
        Dim iIndex As Integer
        Dim MyDrive As MonitoredItems.DominoDiskSpace
        For iIndex = 0 To Me.List.Count - 1
            MyDrive = Me.List(iIndex)
            If MyDrive.DiskName = DriveName Then
                Return Me.List(iIndex)
                Exit Function
            End If
        Next
        Return Nothing
    End Function


End Class

Public Class DominoMailClusterDatabaseCollection
    Inherits System.collections.CollectionBase
    ' A collection of NSF files that are in a mail cluster
    Public Sub Add(ByVal objItemToAdd As DominoMailClusterDatabase)
        Me.List.Add(objItemToAdd)
    End Sub

    Public ReadOnly Property Item(ByVal iIndex As Integer) As DominoMailClusterDatabase
        Get
            Return Me.List(iIndex)
        End Get
    End Property

    Public Function Search(ByVal ReplicaID As String) As DominoMailClusterDatabase
        Dim iIndex As Integer
        Dim myNSF As MonitoredItems.DominoMailClusterDatabase
        For iIndex = 0 To Me.List.Count - 1
            myNSF = Me.List(iIndex)
            If myNSF.ReplicaID = ReplicaID Then
                Return Me.List(iIndex)
                Exit Function
            End If
        Next
        Return Nothing
    End Function

End Class

Public Class DominoStatisticsCollection
    Inherits System.collections.CollectionBase

    Public Sub Add(ByVal objItemToAdd As DominoCustomStatistic)
        Me.List.Add(objItemToAdd)
    End Sub

    Public ReadOnly Property Item(ByVal iIndex As Integer) As DominoCustomStatistic
        Get
            Return Me.List(iIndex)
        End Get
    End Property

    Public Function Search(ByVal StatisticName As String) As DominoCustomStatistic
        Dim iIndex As Integer
        Dim MyStat As MonitoredItems.DominoCustomStatistic
        For iIndex = 0 To Me.List.Count - 1
            MyStat = Me.List(iIndex)
            If MyStat.Statistic = StatisticName Then
                Return Me.List(iIndex)
                Exit Function
            End If
        Next
        Return Nothing
    End Function


End Class


Public Class TravelerStatusReasonsCollection
    Inherits System.collections.CollectionBase

    Public Sub Add(ByVal objItemToAdd As TravelerStatusReason)
        Me.List.Add(objItemToAdd)
    End Sub

    Public ReadOnly Property Item(ByVal iIndex As Integer) As DominoCustomStatistic
        Get
            Return Me.List(iIndex)
        End Get
    End Property


End Class


Public Class ServerTaskSettingCollection
    'this is the collection of tasks that are supposed to be running on the server
    Inherits System.collections.CollectionBase

    Public Sub Add(ByVal objItemToAdd As ServerTaskSetting)
        Me.List.Add(objItemToAdd)
    End Sub

    Public ReadOnly Property Item(ByVal iIndex As Integer) As ServerTaskSetting
        Get
            Return Me.List(iIndex)
        End Get
    End Property

    Public Function Search(ByVal Name As String) As ServerTaskSetting
        Dim iIndex As Integer
        Dim MyCommand As MonitoredItems.ServerTaskSetting
        For iIndex = 0 To Me.List.Count - 1
            MyCommand = Me.List(iIndex)
            If Trim(MyCommand.Name) = Trim(Name) Then
                Return Me.List(iIndex)
                Exit Function
            End If
            If UCase(MyCommand.ConsoleString) = UCase(Name) Then
                Return Me.List(iIndex)
                Exit Function
            End If
        Next
        Return Nothing
    End Function

End Class

Public Class ServerTasksCollection
    'This is the collection of tasks that are actually running on the server
    Inherits System.collections.CollectionBase

    Public Sub Add(ByVal objItemToAdd As ServerTask)
        Me.List.Add(objItemToAdd)
    End Sub

    Public ReadOnly Property Item(ByVal iIndex As Integer) As ServerTask
        Get
            Return Me.List(iIndex)
        End Get
    End Property
    '8/16/2016 NS modified for VSPLUS-2380
    Public Function Search(ByVal Name As String, Optional ByVal exactSearch As Boolean = True) As ServerTask
        Dim iIndex As Integer
        Dim MyCommand As MonitoredItems.ServerTask
        For iIndex = 0 To Me.List.Count - 1
            MyCommand = Me.List(iIndex)
            If exactSearch Then
                If MyCommand.Name = Name Then
                    Return Me.List(iIndex)
                    Exit Function
                End If
            Else
                If InStr(MyCommand.Name.ToLower, Name.ToLower) > 0 Then
                    Return Me.List(iIndex)
                    Exit Function
                End If
            End If
        Next
        Return Nothing
    End Function

End Class

Public Class DominoConsoleCommandCollection
    Inherits System.Collections.CollectionBase

    Public Sub Add(ByVal objItemToAdd As ScheduledServerCommand)
        Me.List.Add(objItemToAdd)
    End Sub

    Public ReadOnly Property Item(ByVal iIndex As Integer) As ScheduledServerCommand
        Get
            Return Me.List(iIndex)
        End Get
    End Property

    Public Function Search(ByVal Name As String) As ScheduledServerCommand
        Dim iIndex As Integer
        Dim MyCommand As MonitoredItems.ScheduledServerCommand
        For iIndex = 0 To Me.List.Count - 1
            MyCommand = Me.List(iIndex)
            If MyCommand.Name = Name Then
                Return Me.List(iIndex)
                Exit Function
            End If
        Next
        Return Nothing
    End Function

    Public Function Search(ByVal UniqueKey As Long) As ScheduledServerCommand
        Dim iIndex As Integer
        Dim MyCommand As MonitoredItems.ScheduledServerCommand
        For iIndex = 0 To Me.List.Count - 1
            MyCommand = Me.List(iIndex)
            If MyCommand.UniqueKey = UniqueKey Then
                Return Me.List(iIndex)
                Exit Function
            End If
        Next
        Return Nothing
    End Function

End Class

Public Class NotesDatabaseCollection
    Inherits MonitoredDevicesCollection

    Public Overloads Sub Add(ByVal objItemToAdd As NotesDatabase)
        Me.List.Add(objItemToAdd)
    End Sub

    Public Overloads Function Delete(ByVal Name As String) As Boolean
        Dim iIndex As Integer
        Dim MyCommand As MonitoredItems.NotesDatabase
        For iIndex = 0 To Me.List.Count - 1
            MyCommand = Me.List(iIndex)
            If MyCommand.Name = Name Then
                Try
                    Me.List.RemoveAt(iIndex)
                    Return True
                Catch ex As Exception
                    Return False
                End Try
                Exit Function
            End If
        Next
        Return Nothing
    End Function


    Public Overloads ReadOnly Property Item(ByVal iIndex As Integer) As NotesDatabase
        Get
            Return Me.List(iIndex)
        End Get
    End Property

    Public Overloads Function Search(ByVal Name As String) As NotesDatabase
        Dim iIndex As Integer
        Dim MyNotesDatabase As MonitoredItems.NotesDatabase
        For iIndex = 0 To Me.List.Count - 1
            MyNotesDatabase = Me.List(iIndex)
            If MyNotesDatabase.Name = Name Then
                Return Me.List(iIndex)
                Exit Function
            End If
        Next
        Return Nothing
    End Function
End Class

Public Class DominoCollection
    Inherits MonitoredDevicesCollection

    Public Overloads Sub Add(ByVal objItemToAdd As DominoServer)
        Me.List.Add(objItemToAdd)
    End Sub

    Public Overloads ReadOnly Property Item(ByVal iIndex As Integer) As DominoServer
        Get
            Return Me.List(iIndex)
        End Get
    End Property

    'Public Overloads Function SearchByKey(ByVal Key As String) As DominoServer
    '    Dim iIndex As Integer
    '    Dim DominoServer As MonitoredItems.DominoServer
    '    For iIndex = 0 To Me.List.Count - 1
    '        DominoServer = Me.List(iIndex)
    '        If DominoServer.Key = Key Then
    '            Return Me.List(iIndex)
    '            Exit Function
    '        End If
    '    Next
    '    Return Nothing
    'End Function

    Public Overloads Function Search(ByVal Name As String) As DominoServer
        Dim iIndex As Integer
        Dim DominoServer As MonitoredItems.DominoServer
        For iIndex = 0 To Me.List.Count - 1
            DominoServer = Me.List(iIndex)
            If DominoServer.Name = Name Then
                Return Me.List(iIndex)
                Exit Function
            End If

            'If InStr(Name, DominoServer.Name) > 0 Then
            '    Return Me.List(iIndex)
            '    Exit Function
            'End If

        Next
        Return Nothing
    End Function

    Public Overloads Function Delete(ByVal Name As String) As Boolean
        Dim iIndex As Integer
        Dim MyCommand As MonitoredItems.DominoServer
        For iIndex = 0 To Me.List.Count - 1
            MyCommand = Me.List(iIndex)
            If MyCommand.Name = Name Then
                Try
                    Me.List.RemoveAt(iIndex)
                    Return True
                Catch ex As Exception
                    Return False
                End Try
                Exit Function
            End If
        Next
        Return Nothing
    End Function

End Class


Public Class Traveler_Backend_Collection
    Inherits System.Collections.CollectionBase

    Public Sub Add(ByVal objItemToAdd As Traveler_Backend)
        Me.List.Add(objItemToAdd)
    End Sub

    Public ReadOnly Property Item(ByVal iIndex As Integer) As Traveler_Backend
        Get
            Return Me.List(iIndex)
        End Get
    End Property


End Class

Public Class DominoMailProbeCollection
    Inherits MonitoredDevicesCollection

    Public Overloads Function Delete(ByVal Name As String) As Boolean
        Dim iIndex As Integer
        Dim MyCommand As MonitoredItems.DominoMailProbe
        For iIndex = 0 To Me.List.Count - 1
            MyCommand = Me.List(iIndex)
            If MyCommand.Name = Name Then
                Try
                    Me.List.RemoveAt(iIndex)
                    Return True
                Catch ex As Exception
                    Return False
                End Try
                Exit Function
            End If
        Next
        Return Nothing
    End Function


    Public Overloads Sub Add(ByVal objItemToAdd As DominoMailProbe)
        Me.List.Add(objItemToAdd)
    End Sub

    Public Overloads ReadOnly Property Item(ByVal iIndex As Integer) As DominoMailProbe
        Get
            Return Me.List(iIndex)
        End Get
    End Property

    Public Overloads Function Search(ByVal DeviceName As String) As DominoMailProbe
        Dim iIndex As Integer
        Dim NotesMailProbe As MonitoredItems.DominoMailProbe
        For iIndex = 0 To Me.List.Count - 1
            NotesMailProbe = Me.List(iIndex)
            '  Debug.Write("Checking " & BBProbe.Name)
            If NotesMailProbe.Name = DeviceName Then
                Return Me.List(iIndex)
                Exit Function
            End If
        Next
        Return Nothing
    End Function

End Class

Public Class DominoMailClusterCollection
    Inherits MonitoredDevicesCollection

    Public Overloads Function Delete(ByVal Name As String) As Boolean
        Dim iIndex As Integer
        Dim MyCommand As MonitoredItems.DominoMailCluster
        For iIndex = 0 To Me.List.Count - 1
            MyCommand = Me.List(iIndex)
            If MyCommand.Name = Name Then
                Try
                    Me.List.RemoveAt(iIndex)
                    Return True
                Catch ex As Exception
                    Return False
                End Try
                Exit Function
            End If
        Next
        Return Nothing
    End Function

    Public Overloads Sub Add(ByVal objItemToAdd As DominoMailCluster)
        Me.List.Add(objItemToAdd)
    End Sub

    Public Overloads ReadOnly Property Item(ByVal iIndex As Integer) As DominoMailCluster
        Get
            Return Me.List(iIndex)
        End Get
    End Property

    Public Overloads Function Search(ByVal Name As String) As DominoMailCluster
        Dim iIndex As Integer
        Dim DominoServer As MonitoredItems.DominoMailCluster
        For iIndex = 0 To Me.List.Count - 1
            DominoServer = Me.List(iIndex)
            If DominoServer.Name = Name Then
                Return Me.List(iIndex)
                Exit Function
            End If

            '9/25/2015 NS modified for VSPLUS-2126
            'If InStr(Name, DominoServer.Name) > 0 Then
            '	Return Me.List(iIndex)
            '	Exit Function
            'End If
        Next
        Return Nothing
    End Function
End Class

#End Region

Public Class SametimeServersCollection
    Inherits MonitoredDevicesCollection

    Public Overloads Sub Add(ByVal objItemToAdd As SametimeServer)
        Me.List.Add(objItemToAdd)
    End Sub

    Public Overloads ReadOnly Property Item(ByVal iIndex As Integer) As SametimeServer
        Get
            Return Me.List(iIndex)
        End Get
    End Property

    Public Overloads Function SearchByName(ByVal Name As String) As SametimeServer
        Dim iIndex As Integer
        Dim MyCommand As MonitoredItems.SametimeServer
        For iIndex = 0 To Me.List.Count - 1
            MyCommand = Me.List(iIndex)
            If MyCommand.Name = Name Then
                Return Me.List(iIndex)
                Exit Function
            End If
        Next
        Return Nothing
    End Function

    Public Overloads Function SearchByIPAddress(ByVal Name As String) As SametimeServer
        Dim iIndex As Integer
        Dim MyCommand As MonitoredItems.SametimeServer
        For iIndex = 0 To Me.List.Count - 1
            MyCommand = Me.List(iIndex)
            If MyCommand.IPAddress = Name Then
                Return Me.List(iIndex)
                Exit Function
            End If
        Next
        Return Nothing
    End Function

    Public Overloads Function Delete(ByVal Name As String) As Boolean
        Dim iIndex As Integer
        Dim MyCommand As MonitoredItems.SametimeServer
        For iIndex = 0 To Me.List.Count - 1
            MyCommand = Me.List(iIndex)
            If MyCommand.Name = Name Then
                Try
                    Me.List.RemoveAt(iIndex)
                    Return True
                Catch ex As Exception
                    Return False
                End Try
                Exit Function
            End If
        Next
        Return Nothing
    End Function
End Class

Public Class SametimeMonitoredProcessCollection
    Inherits System.Collections.CollectionBase

    Public Sub Add(ByVal objItemToAdd As SametimeMonitoredProcess)
        Me.List.Add(objItemToAdd)
    End Sub

    Public ReadOnly Property Item(ByVal iIndex As Integer) As SametimeMonitoredProcess
        Get
            Return Me.List(iIndex)
        End Get
    End Property

    Public Function Search(ByVal Name As String) As SametimeMonitoredProcess
        Dim iIndex As Integer
        Dim myProcess As MonitoredItems.SametimeMonitoredProcess
        For iIndex = 0 To Me.List.Count - 1
            myProcess = Me.List(iIndex)
            If myProcess.Name.ToUpper = Name.ToUpper Then
                Return Me.List(iIndex)
                Exit Function
            End If
        Next
        Return Nothing
    End Function


End Class

Public Class SametimeRunningProcessCollection
    Inherits System.Collections.CollectionBase

    Public Sub Add(ByVal objItemToAdd As SametimeRunningProcess)
        Me.List.Add(objItemToAdd)
    End Sub

    Public ReadOnly Property Item(ByVal iIndex As Integer) As SametimeRunningProcess
        Get
            Return Me.List(iIndex)
        End Get
    End Property

    Public Function Search(ByVal Name As String) As SametimeRunningProcess
        Dim iIndex As Integer
        Dim myProcess As MonitoredItems.SametimeRunningProcess
        For iIndex = 0 To Me.List.Count - 1
            myProcess = Me.List(iIndex)
            If myProcess.Name.ToUpper = Name.ToUpper Then
                Return Me.List(iIndex)
                Exit Function
            End If
        Next
        Return Nothing
    End Function


End Class

Public Class SametimeStatisticsCollection
    Inherits System.Collections.CollectionBase

    Public Sub Add(ByVal objItemToAdd As SametimeStatistic)
        Me.List.Add(objItemToAdd)
    End Sub

    Public ReadOnly Property Item(ByVal iIndex As Integer) As SametimeStatistic
        Get
            Return Me.List(iIndex)
        End Get
    End Property

    Public Function Search(ByVal StatisticName As String) As SametimeStatistic
        Dim iIndex As Integer
        Dim MyStatistic As MonitoredItems.SametimeStatistic
        For iIndex = 0 To Me.List.Count - 1
            MyStatistic = Me.List(iIndex)
            If MyStatistic.Name = StatisticName Then
                Return Me.List(iIndex)
                Exit Function
            End If
        Next
        Return Nothing
    End Function


End Class

Public Class DNS_ServersCollection
    Inherits MonitoredDevicesCollection

    Public Overloads Sub Add(ByVal objItemToAdd As DNS_Server)
        Me.List.Add(objItemToAdd)
    End Sub

    Public Overloads ReadOnly Property Item(ByVal iIndex As Integer) As DNS_Server
        Get
            Return Me.List(iIndex)
        End Get
    End Property

    Public Overloads Function SearchByName(ByVal Name As String) As DNS_Server
        Dim iIndex As Integer
        Dim MyCommand As MonitoredItems.DNS_Server
        For iIndex = 0 To Me.List.Count - 1
            MyCommand = Me.List(iIndex)
            If MyCommand.Name = Name Then
                Return Me.List(iIndex)
                Exit Function
            End If
        Next
        Return Nothing
    End Function

    Public Overloads Function SearchByDomain(ByVal Name As String) As DNS_Server
        Dim iIndex As Integer
        Dim MyCommand As MonitoredItems.DNS_Server
        For iIndex = 0 To Me.List.Count - 1
            MyCommand = Me.List(iIndex)
            If MyCommand.HostNameToQuery = Name Then
                Return Me.List(iIndex)
                Exit Function
            End If
        Next
        Return Nothing
    End Function

    Public Overloads Function Delete(ByVal Name As String) As Boolean
        Dim iIndex As Integer
        Dim MyCommand As MonitoredItems.DNS_Server
        For iIndex = 0 To Me.List.Count - 1
            MyCommand = Me.List(iIndex)
            If MyCommand.Name = Name Then
                Try
                    Me.List.RemoveAt(iIndex)
                    Return True
                Catch ex As Exception
                    Return False
                End Try
                Exit Function
            End If
        Next
        Return Nothing
    End Function
End Class

#Region "BlackBerry"

Public Class BlackBerryUsersCollection
    Inherits MonitoredDevicesCollection

    Public Overloads Sub Add(ByVal objItemToAdd As BlackBerryUser)
        Me.List.Add(objItemToAdd)
    End Sub

    Public Overloads ReadOnly Property Item(ByVal iIndex As Integer) As BlackBerryUser
        Get
            Return Me.List(iIndex)
        End Get
    End Property

    Public Overloads Function Search(ByVal Name As String) As BlackBerryUser
        Dim iIndex As Integer
        Dim BES_User As MonitoredItems.BlackBerryUser
        Try
            For iIndex = 0 To Me.List.Count - 1
                BES_User = Me.List(iIndex)
                If BES_User.Name.ToUpper = Name.ToUpper Then
                    Return Me.List(iIndex)
                    Exit Function
                End If
            Next
        Catch ex As Exception

        End Try

        Try
            Dim myName As String = ""
            For iIndex = 0 To Me.List.Count - 1
                BES_User = Me.List(iIndex)
                myName = BES_User.Name.ToUpper
                If myName.StartsWith(Name) Then
                    Return Me.List(iIndex)
                    Exit Function
                End If
                If InStr(BES_User.Name.ToUpper, Name.ToUpper) > 0 Then
                    Return Me.List(iIndex)
                    Exit Function
                End If
            Next
        Catch ex As Exception

        End Try


        Return Nothing
    End Function

    Public Overloads Function Delete(ByVal Name As String) As Boolean
        Dim iIndex As Integer
        Dim MyUser As MonitoredItems.BlackBerryUser
        For iIndex = 0 To Me.List.Count - 1
            MyUser = Me.List(iIndex)
            If MyUser.Name = Name Then
                Try
                    Me.List.RemoveAt(iIndex)
                    Return True
                Catch ex As Exception
                    Return False
                End Try
                Exit Function
            End If
        Next
        Return Nothing
    End Function




End Class

Public Class BlackBerryServerCollection
    Inherits MonitoredDevicesCollection

    Public Overloads Sub Add(ByVal objItemToAdd As BlackBerryServer)
        Me.List.Add(objItemToAdd)
    End Sub

    Public Overloads ReadOnly Property Item(ByVal iIndex As Integer) As BlackBerryServer
        Get
            Return Me.List(iIndex)
        End Get
    End Property

    Public Overloads Function Search(ByVal Name As String) As BlackBerryServer
        Dim iIndex As Integer
        Dim MyCommand As MonitoredItems.BlackBerryServer
        For iIndex = 0 To Me.List.Count - 1
            MyCommand = Me.List(iIndex)
            If MyCommand.Name = Name Then
                Return Me.List(iIndex)
                Exit Function
            End If
        Next
        Return Nothing
    End Function

    Public Overloads Function Delete(ByVal Name As String) As Boolean
        Dim iIndex As Integer
        Dim MyCommand As MonitoredItems.BlackBerryServer
        For iIndex = 0 To Me.List.Count - 1
            MyCommand = Me.List(iIndex)
            If MyCommand.Name = Name Then
                Try
                    Me.List.RemoveAt(iIndex)
                    Return True
                Catch ex As Exception
                    Return False
                End Try
                Exit Function
            End If
        Next
        Return Nothing
    End Function

End Class

Public Class BlackBerryQueueCollection
    Inherits MonitoredDevicesCollection

    Public Overloads Sub Add(ByVal objItemToAdd As BlackBerryQueue)
        Me.List.Add(objItemToAdd)
    End Sub

    Public Overloads ReadOnly Property Item(ByVal iIndex As Integer) As BlackBerryQueue
        Get
            Return Me.List(iIndex)
        End Get
    End Property

    Public Overloads Function Search(ByVal Name As String) As BlackBerryQueue
        Dim iIndex As Integer
        Dim BlackBerryQueue As MonitoredItems.BlackBerryQueue
        For iIndex = 0 To Me.List.Count - 1
            BlackBerryQueue = Me.List(iIndex)
            If BlackBerryQueue.Name = Name Then
                Return Me.List(iIndex)
                Exit Function
            End If
        Next
        Return Nothing
    End Function

End Class

Public Class BlackBerryMailProbeCollection
    Inherits MonitoredDevicesCollection

    Public Overloads Sub Add(ByVal objItemToAdd As BlackBerryMailProbe)
        Me.List.Add(objItemToAdd)
    End Sub

    Public Overloads ReadOnly Property Item(ByVal iIndex As Integer) As BlackBerryMailProbe
        Get
            Return Me.List(iIndex)
        End Get
    End Property

    Public Overloads Function Search(ByVal DeviceName As String) As BlackBerryMailProbe
        Dim iIndex As Integer
        Dim BBProbe As MonitoredItems.BlackBerryMailProbe
        For iIndex = 0 To Me.List.Count - 1
            BBProbe = Me.List(iIndex)
            '  Debug.Write("Checking " & BBProbe.Name)
            If BBProbe.Name = DeviceName Then
                Return Me.List(iIndex)
                Exit Function
            End If
        Next
        Return Nothing
    End Function

    Public Overloads Function Delete(ByVal Name As String) As Boolean
        Dim iIndex As Integer
        Dim MyCommand As MonitoredItems.BlackBerryMailProbe
        For iIndex = 0 To Me.List.Count - 1
            MyCommand = Me.List(iIndex)
            If MyCommand.Name = Name Then
                Try
                    Me.List.RemoveAt(iIndex)
                    Return True
                Catch ex As Exception
                    Return False
                End Try
                Exit Function
            End If
        Next
        Return Nothing
    End Function

End Class

#End Region

Public Class NetworkDeviceCollection
    Inherits MonitoredDevicesCollection

    Public Overloads Sub Add(ByVal objItemToAdd As NetworkDevice)
        Me.List.Add(objItemToAdd)
    End Sub

    Public Overloads Function Search(ByVal Name As String) As NetworkDevice
        Dim iIndex As Integer
        Dim NetworkDevice As MonitoredItems.NetworkDevice
        For iIndex = 0 To Me.List.Count - 1
            NetworkDevice = Me.List(iIndex)
            If NetworkDevice.Name = Name Then
                Return Me.List(iIndex)
                Exit Function
            End If
        Next
        Return Nothing
    End Function

    Public Overloads Function Delete(ByVal Name As String) As Boolean
        Dim iIndex As Integer
        Dim MyCommand As MonitoredItems.NetworkDevice
        For iIndex = 0 To Me.List.Count - 1
            MyCommand = Me.List(iIndex)
            If MyCommand.Name = Name Then
                Try
                    Me.List.RemoveAt(iIndex)
                    Return True
                Catch ex As Exception
                    Return False
                End Try
                Exit Function
            End If
        Next
        Return Nothing
    End Function

    Public Overloads ReadOnly Property Item(ByVal iIndex As Integer) As NetworkDevice
        Get
            Return Me.List(iIndex)
        End Get
    End Property
End Class

Public Class URLCollection
    Inherits MonitoredDevicesCollection

    Public Overloads Function Search(ByVal Name As String) As URL
        Dim iIndex As Integer
        Dim URL As MonitoredItems.URL
        For iIndex = 0 To Me.List.Count - 1
            URL = Me.List(iIndex)
            If URL.Name = Name Then
                Return Me.List(iIndex)
                Exit Function
            End If
        Next
        Return Nothing
    End Function

    Public Overloads Function SearchByURL(ByVal URL As String) As URL
        Dim iIndex As Integer
        Dim myURL As MonitoredItems.URL
        For iIndex = 0 To Me.List.Count - 1
            myURL = Me.List(iIndex)
            If myURL.URL = URL Then
                Return Me.List(iIndex)
                Exit Function
            End If
        Next
        Return Nothing
    End Function

    Public Overloads Sub Add(ByVal objItemToAdd As URL)
        Me.List.Add(objItemToAdd)
    End Sub

    Public Overloads Function Delete(ByVal Name As String) As Boolean
        Dim iIndex As Integer
        Dim MyCommand As MonitoredItems.URL
        For iIndex = 0 To Me.List.Count - 1
            MyCommand = Me.List(iIndex)
            If MyCommand.Name = Name Then
                Try
                    Me.List.RemoveAt(iIndex)
                    Return True
                Catch ex As Exception
                    Return False
                End Try
                Exit Function
            End If
        Next
        Return Nothing
    End Function


    Public Overloads ReadOnly Property Item(ByVal iIndex As Integer) As URL
        Get
            Return Me.List(iIndex)
        End Get
    End Property
End Class

Public Class MailServiceCollection
    Inherits MonitoredDevicesCollection

    Public Overloads Sub Add(ByVal objItemToAdd As MailService)
        Me.List.Add(objItemToAdd)
    End Sub

    Public Overloads Function Search(ByVal Name As String) As MailService
        Dim iIndex As Integer
        Dim MailService As MonitoredItems.MailService
        For iIndex = 0 To Me.List.Count - 1
            MailService = Me.List(iIndex)
            If MailService.Name = Name Then
                Return Me.List(iIndex)
                Exit Function
            End If
        Next
        Return Nothing
    End Function

    Public Overloads Function Delete(ByVal Name As String) As Boolean
        Dim iIndex As Integer
        Dim MyCommand As MonitoredItems.MailService
        For iIndex = 0 To Me.List.Count - 1
            MyCommand = Me.List(iIndex)
            If MyCommand.Name = Name Then
                Try
                    Me.List.RemoveAt(iIndex)
                    Return True
                Catch ex As Exception
                    Return False
                End Try
                Exit Function
            End If
        Next
        Return Nothing
    End Function


    Public Overloads ReadOnly Property Item(ByVal iIndex As Integer) As MailService
        Get
            Return Me.List(iIndex)
        End Get
    End Property
End Class

'10/6/2014 NS added for VSPLUS-1002
Public Class CloudCollection
    Inherits MonitoredDevicesCollection

    Public Overloads Function Search(ByVal Name As String) As Cloud
        Dim iIndex As Integer
        Dim Cloud As MonitoredItems.Cloud
        For iIndex = 0 To Me.List.Count - 1
            Cloud = Me.List(iIndex)
            If Cloud.Name = Name Then
                Return Me.List(iIndex)
                Exit Function
            End If
        Next
        Return Nothing
    End Function

    Public Overloads Function SearchByURL(ByVal URL As String) As Cloud
        Dim iIndex As Integer
        Dim myCloud As MonitoredItems.Cloud
        For iIndex = 0 To Me.List.Count - 1
            myCloud = Me.List(iIndex)
            If myCloud.CloudURL = URL Then
                Return Me.List(iIndex)
                Exit Function
            End If
        Next
        Return Nothing
    End Function

    Public Overloads Sub Add(ByVal objItemToAdd As Cloud)
        Me.List.Add(objItemToAdd)
    End Sub

    Public Overloads Function Delete(ByVal Name As String) As Boolean
        Dim iIndex As Integer
        Dim MyCommand As MonitoredItems.Cloud
        For iIndex = 0 To Me.List.Count - 1
            MyCommand = Me.List(iIndex)
            If MyCommand.Name = Name Then
                Try
                    Me.List.RemoveAt(iIndex)
                    Return True
                Catch ex As Exception
                    Return False
                End Try
                Exit Function
            End If
        Next
        Return Nothing
    End Function


    Public Overloads ReadOnly Property Item(ByVal iIndex As Integer) As Cloud
        Get
            Return Me.List(iIndex)
        End Get
    End Property
End Class

Public Class WebSphereCollection
    Inherits MonitoredDevicesCollection

    Public Overloads Sub Add(ByVal objItemToAdd As WebSphere)
        Me.List.Add(objItemToAdd)
    End Sub

    Public Overloads ReadOnly Property Item(ByVal iIndex As Integer) As WebSphere
        Get
            Return Me.List(iIndex)
        End Get
    End Property

    Public Overloads Function SearchByName(ByVal Name As String) As WebSphere
        Dim iIndex As Integer
        Dim MyCommand As MonitoredItems.WebSphere
        For iIndex = 0 To Me.List.Count - 1
            MyCommand = Me.List(iIndex)
            If MyCommand.Name.ToUpper = Name.ToUpper Then
                Return Me.List(iIndex)
                Exit Function
            End If
        Next
        Return Nothing
    End Function

    Public Overloads Function SearchByIPAddress(ByVal Name As String) As WebSphere
        Dim iIndex As Integer
        Dim MyCommand As MonitoredItems.WebSphere
        For iIndex = 0 To Me.List.Count - 1
            MyCommand = Me.List(iIndex)
            If MyCommand.IPAddress = Name Then
                Return Me.List(iIndex)
                Exit Function
            End If
        Next
        Return Nothing
    End Function

    Public Overloads Function Delete(ByVal Name As String) As Boolean
        Dim iIndex As Integer
        Dim MyCommand As MonitoredItems.WebSphere
        For iIndex = 0 To Me.List.Count - 1
            MyCommand = Me.List(iIndex)
            If MyCommand.Name = Name Then
                Try
                    Me.List.RemoveAt(iIndex)
                    Return True
                Catch ex As Exception
                    Return False
                End Try
                Exit Function
            End If
        Next
        Return Nothing
    End Function
End Class

Public Class IBMConnectCollection
    Inherits MonitoredDevicesCollection

    Public Overloads Sub Add(ByVal objItemToAdd As IBMConnect)
        Me.List.Add(objItemToAdd)
    End Sub

    Public Overloads ReadOnly Property Item(ByVal iIndex As Integer) As IBMConnect
        Get
            Return Me.List(iIndex)
        End Get
    End Property

    Public Overloads Function SearchByName(ByVal Name As String) As IBMConnect
        Dim iIndex As Integer
        Dim MyCommand As MonitoredItems.IBMConnect
        For iIndex = 0 To Me.List.Count - 1
            MyCommand = Me.List(iIndex)
            If MyCommand.Name.ToUpper = Name.ToUpper Then
                Return Me.List(iIndex)
                Exit Function
            End If
        Next
        Return Nothing
    End Function

    Public Overloads Function SearchByIPAddress(ByVal Name As String) As IBMConnect
        Dim iIndex As Integer
        Dim MyCommand As MonitoredItems.IBMConnect
        For iIndex = 0 To Me.List.Count - 1
            MyCommand = Me.List(iIndex)
            If MyCommand.IPAddress = Name Then
                Return Me.List(iIndex)
                Exit Function
            End If
        Next
        Return Nothing
    End Function

    Public Overloads Function Delete(ByVal Name As String) As Boolean
        Dim iIndex As Integer
        Dim MyCommand As MonitoredItems.IBMConnect
        For iIndex = 0 To Me.List.Count - 1
            MyCommand = Me.List(iIndex)
            If MyCommand.Name = Name Then
                Try
                    Me.List.RemoveAt(iIndex)
                    Return True
                Catch ex As Exception
                    Return False
                End Try
                Exit Function
            End If
        Next
        Return Nothing
    End Function
End Class


#End Region

#Region "Other Classes"

#Region "Domino Mail Cluster"

Public Class DominoMailClusterDatabase
    'An instance of a database that is a defined mail cluster
    Dim mReplicaID, mFileName, mdbTitle As String
    Dim mServer_A_Doc_Count, mServer_B_Doc_Count, mServer_C_Doc_Count As Long
    Dim mServer_A_Size, mServer_B_Size, mServer_C_Size As Long
    Dim mAlertState1, mAlertState2, mAlertNoReplicas As Boolean
    Dim mServer_A_Comment, mServer_B_Comment, mServer_C_Comment As String

    Public Property Server_A_Comment As String
        Get
            Return mServer_A_Comment
        End Get
        Set(ByVal Value As String)
            mServer_A_Comment = Value
        End Set
    End Property

    Public Property Server_B_Comment As String
        Get
            Return mServer_B_Comment
        End Get
        Set(ByVal Value As String)
            mServer_B_Comment = Value
        End Set
    End Property


    Public Property Server_C_Comment As String
        Get
            Return mServer_C_Comment
        End Get
        Set(ByVal Value As String)
            mServer_C_Comment = Value
        End Set
    End Property

    Public Property Alert_State_One() As Boolean
        'true if the database has sent an alert for passing first threshold
        Get
            Return mAlertState1
        End Get
        Set(ByVal Value As Boolean)
            mAlertState1 = Value
        End Set
    End Property

    Public Property Alert_State_Two() As Boolean
        'true if the database has sent an alert for passing first threshold
        Get
            Return mAlertState2
        End Get
        Set(ByVal Value As Boolean)
            mAlertState2 = Value
        End Set
    End Property

    Public Property Alert_No_Replicas() As Boolean
        Get
            Return mAlertNoReplicas
        End Get
        Set(ByVal Value As Boolean)
            mAlertNoReplicas = Value
        End Set
    End Property

    Public Property ReplicaID() As String
        Get
            Return mReplicaID
        End Get
        Set(ByVal Value As String)
            mReplicaID = Value
        End Set
    End Property

    Public Property Database_Title() As String
        Get
            Return mdbTitle
        End Get
        Set(ByVal Value As String)
            mdbTitle = Value
        End Set
    End Property

    Public Property Database_FileName() As String
        Get
            Return mFileName
        End Get
        Set(ByVal Value As String)
            mFileName = Value
        End Set
    End Property

#Region "Database size and doc count"

    Public Property Server_A_Size() As Long
        Get
            Return mServer_A_Size
        End Get
        Set(ByVal Value As Long)
            mServer_A_Size = Value
        End Set
    End Property

    Public Property Server_B_Size() As Long
        Get
            Return mServer_B_Size
        End Get
        Set(ByVal Value As Long)
            mServer_B_Size = Value
        End Set
    End Property

    Public Property Server_C_Size() As Long
        Get
            Return mServer_C_Size
        End Get
        Set(ByVal Value As Long)
            mServer_C_Size = Value
        End Set
    End Property

    Public Property Server_A_Doc_Count As Long
        Get
            Return mServer_A_Doc_Count
        End Get
        Set(ByVal Value As Long)
            mServer_A_Doc_Count = Value
        End Set
    End Property
    Public Property Server_B_Doc_Count As Long
        Get
            Return mServer_B_Doc_Count
        End Get
        Set(ByVal Value As Long)
            mServer_B_Doc_Count = Value
        End Set
    End Property
    Public Property Server_C_Doc_Count As Long
        Get
            Return mServer_C_Doc_Count
        End Get
        Set(ByVal Value As Long)
            mServer_C_Doc_Count = Value
        End Set
    End Property

#End Region

End Class

#End Region

#Region "Scheduled Server Command"

Public Class ScheduledServerCommand
    Dim mServerName As String
    Dim mConsoleCommand As String
    Dim mSunday As Boolean
    Dim mMonday As Boolean
    Dim mTuesday As Boolean
    Dim mWednesday As Boolean
    Dim mThursday As Boolean
    Dim mFriday As Boolean
    Dim mSaturday As Boolean
    Dim mTimeOfDay As DateTime
    Dim mEnabled As Boolean
    Dim mUniqueKey As Long
    Dim mName As String
    Dim mLastSent As DateTime

    Public Property DominoServerName() As String
        Get
            Return mServerName
        End Get
        Set(ByVal Value As String)
            mServerName = Value
        End Set
    End Property

    Public Property LastSent() As DateTime
        Get
            Return mLastSent
        End Get
        Set(ByVal Value As DateTime)
            mLastSent = Value
        End Set
    End Property

    Public Property Name() As String
        Get
            Return mName
        End Get
        Set(ByVal Value As String)
            mName = Value
        End Set
    End Property

    Public Property ConsoleCommand() As String
        Get
            Return mConsoleCommand
        End Get
        Set(ByVal Value As String)
            mConsoleCommand = Value
        End Set
    End Property

    Public Property UniqueKey() As Long
        Get
            Return mUniqueKey
        End Get
        Set(ByVal Value As Long)
            mUniqueKey = Value
        End Set
    End Property

    Public Property Enabled() As Boolean
        Get
            Return mEnabled
        End Get
        Set(ByVal Value As Boolean)
            mEnabled = Value
        End Set
    End Property

    Public Property TimeofDay() As DateTime
        Get
            Return mTimeOfDay.ToShortTimeString
        End Get
        Set(ByVal Value As DateTime)
            Try
                mTimeOfDay = CType(Value, DateTime)
            Catch ex As Exception
                Throw New Exception("Error setting Scheduled Console Command time value -- Please provide a valid date/time")
            End Try

        End Set
    End Property

    Public Property Monday() As Boolean
        Get
            Return mMonday
        End Get
        Set(ByVal Value As Boolean)
            mMonday = Value
        End Set
    End Property
    Public Property Tuesday() As Boolean
        Get
            Return mTuesday
        End Get
        Set(ByVal Value As Boolean)
            mTuesday = Value
        End Set
    End Property

    Public Property Wednesday() As Boolean
        Get
            Return mWednesday
        End Get
        Set(ByVal Value As Boolean)
            mWednesday = Value
        End Set
    End Property

    Public Property Thursday() As Boolean
        Get
            Return mThursday
        End Get
        Set(ByVal Value As Boolean)
            mThursday = Value
        End Set
    End Property

    Public Property Friday() As Boolean
        Get
            Return mFriday
        End Get
        Set(ByVal Value As Boolean)
            mFriday = Value
        End Set
    End Property

    Public Property Saturday() As Boolean
        Get
            Return mSaturday
        End Get
        Set(ByVal Value As Boolean)
            mSaturday = Value
        End Set
    End Property
    Public Property Sunday() As Boolean
        Get
            Return mSunday
        End Get
        Set(ByVal Value As Boolean)
            mSunday = Value
        End Set
    End Property

End Class

#End Region

#Region "Server Task Stuff"

Public Class TravelerStatusReason
    Dim mStatusReason As String
    Public Property StatusDetails() As String
        Get
            Return mStatusReason
        End Get
        Set(ByVal Value As String)
            mStatusReason = Value
        End Set
    End Property
End Class

Public Class ServerTaskSetting
    'An individual server task that is supposed to be running on the server
    Dim mName As String
    Dim mConsoleString As String
    Dim mEnabled, mLoadIfMissing, mRestartOffHours, mRestart, mExit, mFreezeDetect, mDisallow As Boolean
    Dim mLoadCommand As String
    Dim mFailureCount As Integer
    Dim mFailureThreshold As Integer
    Dim mMaxRunTime As Integer  'number of minutes it is allowed to run before declared hung
    Dim mStatus As String
    Public Property Status() As String
        Get
            Return mStatus
        End Get
        Set(ByVal Value As String)
            mStatus = Value
        End Set
    End Property

    Public Property FailureThreshold() As Integer
        Get
            Return mFailureThreshold
        End Get
        Set(ByVal Value As Integer)
            mFailureThreshold = Value
        End Set
    End Property

    Public Property FreezeDetection() As Boolean
        Get
            Return mFreezeDetect
        End Get
        Set(ByVal Value As Boolean)
            mFreezeDetect = Value
        End Set
    End Property

    Public Property MaxRunTime() As Integer
        Get
            Return mMaxRunTime
        End Get
        Set(ByVal Value As Integer)
            mMaxRunTime = Value
        End Set
    End Property

    Public Property FailureCount() As Integer
        Get
            Return mFailureCount
        End Get
        Set(ByVal Value As Integer)
            mFailureCount = Value
        End Set
    End Property

    Public Property LoadCommand() As String
        Get
            Return mLoadCommand
        End Get
        Set(ByVal Value As String)
            mLoadCommand = Value
        End Set
    End Property

    Public Property DisallowTask() As Boolean
        Get
            Return mDisallow
        End Get
        Set(ByVal Value As Boolean)
            mDisallow = Value
        End Set
    End Property

    Public Property LoadIfMissing() As Boolean
        Get
            Return mLoadIfMissing
        End Get
        Set(ByVal Value As Boolean)
            mLoadIfMissing = Value
        End Set
    End Property

    Public Property RestartServerIfMissingASAP() As Boolean
        Get
            Return mRestart
        End Get
        Set(ByVal Value As Boolean)
            mRestart = Value
        End Set
    End Property


    Public Property RestartServerIfMissingOFFHOURS() As Boolean
        Get
            Return mRestartOffHours
        End Get
        Set(ByVal Value As Boolean)
            mRestartOffHours = Value
        End Set
    End Property

    Public Property ConsoleString() As String
        Get
            Return mConsoleString
        End Get
        Set(ByVal Value As String)
            mConsoleString = Value
        End Set
    End Property

    Public Property Name() As String
        Get
            Return mName
        End Get
        Set(ByVal Value As String)
            mName = Value
        End Set
    End Property

    Public Property Enabled() As Boolean
        Get
            Return mEnabled
        End Get
        Set(ByVal Value As Boolean)
            mEnabled = Value
        End Set
    End Property

End Class

Public Class ServerTask
    'An individual server task that is actually running on the server
    Dim mName As String
    Dim mStatus As String
    Dim mSecondaryStatus As String
    Dim mUpdate As DateTime
    Public StatusSummary As String
    Public IsMonitored As String
    Dim mTaskName As String

    Public Property TaskName As String
        Get
            Return mTaskName
        End Get
        Set(ByVal Value As String)
            mTaskName = Value
        End Set
    End Property

    Public Property Name As String
        Get
            Return mName
        End Get
        Set(ByVal Value As String)
            mName = Value
        End Set
    End Property

    Public Property Status As String
        Get
            Return mStatus
        End Get
        Set(ByVal Value As String)
            mStatus = Value
        End Set
    End Property

    Public Property SecondaryStatus As String
        Get
            Return mSecondaryStatus
        End Get
        Set(ByVal Value As String)
            mSecondaryStatus = Value
        End Set
    End Property

    Public Property LastUpdated() As DateTime
        Get
            Return mUpdate
        End Get
        Set(ByVal Value As DateTime)
            mUpdate = Value
        End Set
    End Property
End Class
#End Region

Public Class SametimeMonitoredProcess
    Public Property Name As String
End Class
Public Class SametimeRunningProcess
    Public Property Name As String
End Class

Public Class SametimeStatistic
    Public Property Name As String
    Public Property Value As String
End Class

'Public Class DominoDiskPlatformStatistic
'    Dim mDiskName As String
'    Dim mDiskAverageQueueLength As Long
'    Dim mDiskPercentUtilization As Double
'    Dim mLastUpdated As Date

'    Public Property DiskName As String

'        Get
'            Return mDiskName
'        End Get
'        Set(ByVal value As String)
'            mDiskName = value
'        End Set
'    End Property

'    Public Property DiskAverageQueueLength As Long
'        Get
'            Return mDiskAverageQueueLength
'        End Get
'        Set(ByVal value As Long)
'            mDiskAverageQueueLength = value
'        End Set
'    End Property

'    Public Property DiskPercentUtilization As Double
'        Get
'            Return mDiskPercentUtilization
'        End Get
'        Set(ByVal value As Double)
'            mDiskPercentUtilization = value
'        End Set
'    End Property


'    Public Property LastUpdated As Date

'        Get
'            Return mLastUpdated
'        End Get
'        Set(ByVal value As Date)
'            mLastUpdated = value
'        End Set
'    End Property
'End Class


Public Class DominoDiskSpace
    Dim mDiskName As String
    Dim mDiskSize As Double
    Dim mDiskFree As Double
    Dim mPercentFree As Double
    Dim mDiskAverageQueueLength As Double
    Dim mDiskPercentUtilization As Double
    Dim mLastUpdated As Date
    Dim mInfoType As String

    Public DiskSizeInGB As Double
    Public DiskFreeInGB As Double

    Public Property ThresholdType As String
        'should have the value of "Percent" or "GB"
        Get
            Return mInfoType
        End Get
        Set(ByVal value As String)
            mInfoType = value
        End Set
    End Property

    Public Property DiskName As String

        Get
            Return mDiskName
        End Get
        Set(ByVal value As String)
            mDiskName = value
        End Set
    End Property

    Public Property DiskSize As Double
        Get
            Return mDiskSize
        End Get
        Set(ByVal value As Double)
            mDiskSize = value
        End Set
    End Property

    Public Property DiskFree As Double
        Get
            Return mDiskFree
        End Get
        Set(ByVal value As Double)
            mDiskFree = value
        End Set
    End Property

    Public Property PercentFree As Double
        Get
            Return (mPercentFree)
        End Get
        Set(ByVal value As Double)
            mPercentFree = value
        End Set
    End Property

    Public Property LastUpdated As Date

        Get
            Return mLastUpdated
        End Get
        Set(ByVal value As Date)
            mLastUpdated = value
        End Set
    End Property


    Public Property DiskAverageQueueLength As Double
        Get
            Return mDiskAverageQueueLength
        End Get
        Set(ByVal value As Double)
            mDiskAverageQueueLength = value
        End Set
    End Property

    Public Property DiskPercentUtilization As Double
        Get
            Return mDiskPercentUtilization
        End Get
        Set(ByVal value As Double)
            mDiskPercentUtilization = value
        End Set
    End Property

End Class

Public Class DominoCustomStatistic
    Dim mStatName As String   'i.e.,  Server.Users
    Dim mStatValue As Double ' The current value of the statistic
    Dim mThreshold As Double ' The value of the stat that triggers an alert
    Public mRepeat As Integer 'the number of successive times the threshold must be met
    Dim mComparison As String  'will be 'Greater Than' or 'Less Than'
    Dim mRepeatActual As Integer = 0 'the number of successive times the threshold HAS be met
    Dim mConsoleCommand As String




    Public Property Value() As Double
        Get
            Return mStatValue
        End Get

        Set(ByVal Value As Double)
            If Value <> -999 Then
                ' -999 is used as a reset/junk value to make sure the stat is current.  Ignore this value
            mStatValue = Value
                Select Case mComparison
                    Case "Greater Than"
                        If mStatValue >= mThreshold + 1 Then
                        IncrementCounter()
                    Else
                        ResetCounter()
                    End If
                    Case "Less than"
                    If mStatValue <= mThreshold Then
                        IncrementCounter()
                    Else
                        ResetCounter()
                    End If
            End Select
            End If
           
        End Set
    End Property

    Public Function AlertCondition() As Boolean
        If mRepeatActual >= mRepeat Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub ResetCounter()
        mRepeatActual = 0
    End Sub

    Private Sub IncrementCounter()
        mRepeatActual += 1
    End Sub

    Public Property Statistic() As String
        Get
            Return mStatName
        End Get
        Set(ByVal Value As String)
            mStatName = Value
        End Set
    End Property

    Public Property ThresholdValue() As Double
        Get
            Return mThreshold
        End Get
        Set(ByVal Value As Double)
            mThreshold = Value
        End Set
    End Property

    Public Property RepeatThreshold() As Integer
        Get
            Return mRepeat
        End Get
        Set(ByVal Value As Integer)
            mRepeat = Value
        End Set
    End Property

    Public Property ComparisonOperator As String
        Get
            Return mComparison
        End Get
        Set(ByVal Value As String)
            If Value = "Greater Than" Or Value = "Less Than" Or Value = "Less than" Then
                mComparison = Value
            Else
                mComparison = "Greater Than"
            End If
        End Set
    End Property

    Public Property ConsoleCommand() As String
        Get
            Return mConsoleCommand
        End Get
        Set(ByVal Value As String)
            mConsoleCommand = Value
        End Set
    End Property
    '8/30/2016 NS added for VPLUS-3176
    Public Property ConsecutiveRepeat() As Integer
        Get
            Return mRepeatActual
        End Get
        Set(ByVal Value As Integer)
            mRepeatActual = Value
        End Set
    End Property

End Class

#End Region

