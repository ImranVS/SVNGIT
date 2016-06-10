<%@ Page Title="VitalSigns Plus - DominoStatistic" Language="C#" MasterPageFile="~/Site1.Master"AutoEventWireup="true" CodeBehind="DominoStatistic.aspx.cs" Inherits="VSWebUI.Configurator.DominoStatistic" %>
<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>


<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script src="../js/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.alert-success').delay(10000).fadeOut("slow", function () {
            });
        });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table>
    <tr>
        <td>
            <div class="header" id="servernamelbldisp" runat="server">Custom Statistic</div>
        </td>
    </tr>
        <tr>
            <td>
                <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" 
                    style="background-color: #CCFFFF" Width="600px" 
                    HeaderText="Alert Setting" GroupBoxCaptionOffsetY="-24px" 
                    SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Theme="Glass">
                    <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                    <HeaderStyle Height="23px">
                        </HeaderStyle>
                    <PanelCollection>
<dx:PanelContent ID="PanelContent3" runat="server" SupportsDisabledAttribute="True">
    <table>
        <tr>
            <td>
                
                <dx:ASPxLabel ID="ASPxLabel1" runat="server" CssClass="lblsmallFont" 
                    Text="Domino Server: ">
                </dx:ASPxLabel>
                
            </td>
            <td colspan="3">
                <dx:ASPxComboBox ID="DServerComboBox" runat="server" AutoPostBack="True" 
                    OnSelectedIndexChanged="DServerComboBox_SelectedIndexChanged" Spacing="0" 
                    SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css">
                    <LoadingPanelImage Url="~/App_Themes/Office2010Blue/Editors/Loading.gif">
                    </LoadingPanelImage>
                    <LoadingPanelStyle ImageSpacing="5px">
                    </LoadingPanelStyle>
                    <ButtonStyle Width="13px">
                    </ButtonStyle>
                    <ValidationSettings>
                        <RequiredField IsRequired="True" />
                    </ValidationSettings>
                </dx:ASPxComboBox>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxLabel ID="ASPxLabel2" runat="server" 
                    Text="Trigger an alert if  this statistic" Wrap="False" 
                    CssClass="lblsmallFont">
                </dx:ASPxLabel>
            </td>
            <td colspan="3">
                <dx:ASPxComboBox ID="DStatComboBox" runat="server" Spacing="0" 
                    SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css" Width="300px">
                    <Items>
                        <dx:ListEditItem Text="Calendar.Total.All.Appts.Reservations" 
                            Value="Calendar.Total.All.Appts.Reservations" />
                        <dx:ListEditItem Text="Calendar.Total.All.Users.Resources" 
                            Value="Calendar.Total.All.Users.Resources" />
                        <dx:ListEditItem Text="Calendar.Total.Appts" Value="Calendar.Total.Appts" />
                        <dx:ListEditItem Text="Calendar.Total.Reservations" 
                            Value="Calendar.Total.Reservations" />
                        <dx:ListEditItem Text="Calendar.Total.Resources" 
                            Value="Calendar.Total.Resources" />
                        <dx:ListEditItem Text="Calendar.Total.Users" Value="Calendar.Total.Users" />
                        <dx:ListEditItem Text="Database.DAFailoverCount" 
                            Value="Database.DAFailoverCount" />
                        <dx:ListEditItem Text="Database.DARefreshServerInfoCount" 
                            Value="Database.DARefreshServerInfoCount" />
                        <dx:ListEditItem Text="Database.DAReloadCount " 
                            Value="Database.DAReloadCount " />
                        <dx:ListEditItem Text="Database.Database.BufferPool.Maximum.Megabytes" 
                            Value="Database.Database.BufferPool.Maximum.Megabytes" />
                        <dx:ListEditItem Text="Database.Database.BufferPool.MM.Reads" 
                            Value="Database.Database.BufferPool.MM.Reads" />
                        <dx:ListEditItem Text="Database.Database.BufferPool.MM.Writes " 
                            Value="Database.Database.BufferPool.MM.Writes " />
                        <dx:ListEditItem Text="Database.Database.BufferPool.Peak.Megabytes " 
                            Value="Database.Database.BufferPool.Peak.Megabytes " />
                        <dx:ListEditItem Text="Database.Database.BufferPool.PerCentReadsInBuffer" 
                            Value="Database.Database.BufferPool.PerCentReadsInBuffer" />
                        <dx:ListEditItem Text="Database.DbCache.CurrentEntries " 
                            Value="Database.DbCache.CurrentEntries " />
                        <dx:ListEditItem Text="Database.DbCache.HighWaterMark" 
                            Value="Database.DbCache.HighWaterMark" />
                        <dx:ListEditItem Text="Database.DbCache.Hits " Value="Database.DbCache.Hits " />
                        <dx:ListEditItem Text="Database.DbCache.InitialDbOpens" 
                            Value="Database.DbCache.InitialDbOpens" />
                        <dx:ListEditItem Text="Database.DbCache.Lookups " 
                            Value="Database.DbCache.Lookups " />
                        <dx:ListEditItem Text="Database.DbCache.MaxEntries " 
                            Value="Database.DbCache.MaxEntries " />
                        <dx:ListEditItem Text="Database.DbCache.OvercrowdingRejections " 
                            Value="Database.DbCache.OvercrowdingRejections " />
                        <dx:ListEditItem Text="Database.ExtMgrPool.Peak " 
                            Value="Database.ExtMgrPool.Peak " />
                        <dx:ListEditItem Text="Database.ExtMgrPool.Used" 
                            Value="Database.ExtMgrPool.Used" />
                        <dx:ListEditItem Text="Database.FreeHandleStack.FreeHandleStackHits" 
                            Value="Database.FreeHandleStack.FreeHandleStackHits" />
                        <dx:ListEditItem Text="Database.FreeHandleStack.HandleAllocations " 
                            Value="Database.FreeHandleStack.HandleAllocations " />
                        <dx:ListEditItem Text="Database.FreeHandleStack.MissRate" 
                            Value="Database.FreeHandleStack.MissRate" />
                        <dx:ListEditItem Text="Database.LDAP.NAMELookupBindFailures " 
                            Value="Database.LDAP.NAMELookupBindFailures " />
                        <dx:ListEditItem Text="Database.LDAP.NAMELookupBinds " 
                            Value="Database.LDAP.NAMELookupBinds " />
                        <dx:ListEditItem Text="Database.LDAP.NAMELookupBytesReceived " 
                            Value="Database.LDAP.NAMELookupBytesReceived " />
                        <dx:ListEditItem Text="Database.LDAP.NAMELookupEntries" 
                            Value="Database.LDAP.NAMELookupEntries" />
                        <dx:ListEditItem Text="Database.LDAP.NAMELookupFailures " 
                            Value="Database.LDAP.NAMELookupFailures " />
                        <dx:ListEditItem Text="Database.LDAP.NAMELookupTotal " 
                            Value="Database.LDAP.NAMELookupTotal " />
                        <dx:ListEditItem Text="Database.LDAP.NAMELookupTotalLookupTime" 
                            Value="Database.LDAP.NAMELookupTotalLookupTime" />
                        <dx:ListEditItem Text="Database.NAMELookupCacheCacheSize " 
                            Value="Database.NAMELookupCacheCacheSize " />
                        <dx:ListEditItem Text="Database.NAMELookupCacheHashSize " 
                            Value="Database.NAMELookupCacheHashSize " />
                        <dx:ListEditItem Text="Database.NAMELookupCacheHits" 
                            Value="Database.NAMELookupCacheHits" />
                        <dx:ListEditItem Text="Database.NAMELookupCacheLookups" 
                            Value="Database.NAMELookupCacheLookups" />
                        <dx:ListEditItem Text="Database.NAMELookupCacheMaxSize " 
                            Value="Database.NAMELookupCacheMaxSize " />
                        <dx:ListEditItem Text="Database.NAMELookupCacheMisses" 
                            Value="Database.NAMELookupCacheMisses" />
                        <dx:ListEditItem Text="Database.NAMELookupCacheNoHitHits " 
                            Value="Database.NAMELookupCacheNoHitHits " />
                        <dx:ListEditItem Text="Database.NAMELookupCachePool.Peak" 
                            Value="Database.NAMELookupCachePool.Peak" />
                        <dx:ListEditItem Text="Database.NAMELookupCachePool.Used" 
                            Value="Database.NAMELookupCachePool.Used" />
                        <dx:ListEditItem Text="Database.NAMELookupCacheResets" 
                            Value="Database.NAMELookupCacheResets" />
                        <dx:ListEditItem Text="Database.NAMELookupMisses" 
                            Value="Database.NAMELookupMisses" />
                        <dx:ListEditItem Text="Database.NAMELookupTotal" 
                            Value="Database.NAMELookupTotal" />
                        <dx:ListEditItem Text="Database.NAMELookupTotalLookupTime " 
                            Value="Database.NAMELookupTotalLookupTime " />
                        <dx:ListEditItem Text="Database.NIFPool.Peak " Value="Database.NIFPool.Peak " />
                        <dx:ListEditItem Text="Database.NIFPool.Used" Value="Database.NIFPool.Used" />
                        <dx:ListEditItem Text="Database.NSFPool.Peak" Value="Database.NSFPool.Peak" />
                        <dx:ListEditItem Text="Database.NSFPool.Used" Value="Database.NSFPool.Used" />
                        <dx:ListEditItem Text="Database.NSF.ClusterHashTable.EntriesWithSameIndex" 
                            Value="Database.NSF.ClusterHashTable.EntriesWithSameIndex" />
                        <dx:ListEditItem Text="Database.NSF.ClusterHashTable.FreedEntriesOnCleanup" 
                            Value="Database.NSF.ClusterHashTable.FreedEntriesOnCleanup" />
                        <dx:ListEditItem Text="Database.NSF.ClusterHashTable.HashedEntries" 
                            Value="Database.NSF.ClusterHashTable.HashedEntries" />
                        <dx:ListEditItem Text="Database.NSF.ClusterHashTable.HashIsFull" 
                            Value="Database.NSF.ClusterHashTable.HashIsFull" />
                        <dx:ListEditItem Text="Database.NSF.ClusterHashTable.MissedHashHits" 
                            Value="Database.NSF.ClusterHashTable.MissedHashHits" />
                        <dx:ListEditItem Text="Database.NSF.ClusterHashTable.SuccessfullHashHits" 
                            Value="Database.NSF.ClusterHashTable.SuccessfullHashHits" />
                        <dx:ListEditItem Text="Database.NSF.Replicate.NotesMergedBack" 
                            Value="Database.NSF.Replicate.NotesMergedBack" />
                        <dx:ListEditItem Text="Database.NSF.Replicate.NotesReceived" 
                            Value="Database.NSF.Replicate.NotesReceived" />
                        <dx:ListEditItem Text="Database.NSF.Replicate.NotesReopened" 
                            Value="Database.NSF.Replicate.NotesReopened" />
                        <dx:ListEditItem Text="Database.NSF.Replicate.NotesSent" 
                            Value="Database.NSF.Replicate.NotesSent" />
                        <dx:ListEditItem Text="Database.NSF.Replicate.UnreadMarks.ChunkLookups" 
                            Value="Database.NSF.Replicate.UnreadMarks.ChunkLookups" />
                        <dx:ListEditItem Text="Database.NSF.Replicate.UnreadMarks.ChunksScanned" 
                            Value="Database.NSF.Replicate.UnreadMarks.ChunksScanned" />
                        <dx:ListEditItem Text="Database.NSF.Replicate.UnreadMarks.ChunksSkimmed" 
                            Value="Database.NSF.Replicate.UnreadMarks.ChunksSkimmed" />
                        <dx:ListEditItem Text="Database.NSF.Replicate.UnreadMarks.DroppedFutureEntries" 
                            Value="Database.NSF.Replicate.UnreadMarks.DroppedFutureEntries" />
                        <dx:ListEditItem Text="Database.NSF.Replicate.UnreadMarks.DroppedOldEntries" 
                            Value="Database.NSF.Replicate.UnreadMarks.DroppedOldEntries" />
                        <dx:ListEditItem Text="Database.NSF.Replicate.UnreadMarks.FullReplications" 
                            Value="Database.NSF.Replicate.UnreadMarks.FullReplications" />
                        <dx:ListEditItem Text="Database.NSF.Replicate.UnreadMarks.LocalMaxChunks" 
                            Value="Database.NSF.Replicate.UnreadMarks.LocalMaxChunks" />
                        <dx:ListEditItem Text="Database.NSF.Replicate.UnreadMarks.LocalUnreadOperations" 
                            Value="Database.NSF.Replicate.UnreadMarks.LocalUnreadOperations" />
                        <dx:ListEditItem Text="Database.NSF.Replicate.UnreadMarks.MessagesReceived" 
                            Value="Database.NSF.Replicate.UnreadMarks.MessagesReceived" />
                        <dx:ListEditItem Text="Database.NSF.Replicate.UnreadMarks.MessagesSent" 
                            Value="Database.NSF.Replicate.UnreadMarks.MessagesSent" />
                        <dx:ListEditItem Text="Database.NSF.Replicate.UnreadMarks.OperationsReceived" 
                            Value="Database.NSF.Replicate.UnreadMarks.OperationsReceived" />
                        <dx:ListEditItem Text="Database.NSF.Replicate.UnreadMarks.OperationsSent" 
                            Value="Database.NSF.Replicate.UnreadMarks.OperationsSent" />
                        <dx:ListEditItem Text="Database.NSF.Replicate.UnreadMarks.UsersActive" 
                            Value="Database.NSF.Replicate.UnreadMarks.UsersActive" />
                        <dx:ListEditItem Text="Database.NSF.Replicate.UnreadMarks.UsersActiveMax" 
                            Value="Database.NSF.Replicate.UnreadMarks.UsersActiveMax" />
                        <dx:ListEditItem Text="Database.NSF.SignatureCache.Hits" 
                            Value="Database.NSF.SignatureCache.Hits" />
                        <dx:ListEditItem Text="Database.NSF.SignatureCache.Tries" 
                            Value="Database.NSF.SignatureCache.Tries" />
                        <dx:ListEditItem Text="Disk.C.Free" Value="Disk.C.Free" />
                        <dx:ListEditItem Text="Disk.D.Free" Value="Disk.D.Free" />
                        <dx:ListEditItem Text="Disk.E.Free" Value="Disk.E.Free" />
                        <dx:ListEditItem Text="Disk.F.Free" Value="Disk.F.Free" />
                        <dx:ListEditItem Text="Disk.G.Free" Value="Disk.G.Free" />
                        <dx:ListEditItem Text="Disk.I.Free" Value="Disk.I.Free" />
                        <dx:ListEditItem Text="IMAP.Sessions.Inbound.Accept.Queue" 
                            Value="IMAP.Sessions.Inbound.Accept.Queue" />
                        <dx:ListEditItem Text="IMAP.Sessions.Inbound.Active" 
                            Value="IMAP.Sessions.Inbound.Active" />
                        <dx:ListEditItem Text="IMAP.Sessions.Inbound.Active.SSL" 
                            Value="IMAP.Sessions.Inbound.Active.SSL" />
                        <dx:ListEditItem Text="IMAP.Sessions.Inbound.BytesReceived" 
                            Value="IMAP.Sessions.Inbound.BytesReceived" />
                        <dx:ListEditItem Text="IMAP.Sessions.Inbound.BytesSent" 
                            Value="IMAP.Sessions.Inbound.BytesSent" />
                        <dx:ListEditItem Text="IMAP.Sessions.Inbound.Peak" 
                            Value="IMAP.Sessions.Inbound.Peak" />
                        <dx:ListEditItem Text="IMAP.Sessions.Inbound.Peak.SSL" 
                            Value="IMAP.Sessions.Inbound.Peak.SSL" />
                        <dx:ListEditItem Text="IMAP.Sessions.Inbound.Total" 
                            Value="IMAP.Sessions.Inbound.Total" />
                        <dx:ListEditItem Text="IMAP.Sessions.Inbound.Total.SSL" 
                            Value="IMAP.Sessions.Inbound.Total.SSL" />
                        <dx:ListEditItem Text="IMAP.Sessions.Inbound.Total.SSL.Bad_Handshake" 
                            Value="IMAP.Sessions.Inbound.Total.SSL.Bad_Handshake" />
                        <dx:ListEditItem Text="IMAP.Sessions.Threads.Busy" 
                            Value="IMAP.Sessions.Threads.Busy" />
                        <dx:ListEditItem Text="IMAP.Sessions.Threads.Idle" 
                            Value="IMAP.Sessions.Threads.Idle" />
                        <dx:ListEditItem Text="IMAP.Sessions.Threads.InThreadPool" 
                            Value="IMAP.Sessions.Threads.InThreadPool" />
                        <dx:ListEditItem Text="IMAP.Sessions.Threads.Peak" 
                            Value="IMAP.Sessions.Threads.Peak" />
                        <dx:ListEditItem Text="IMAP.TotalMsgCached" Value="IMAP.TotalMsgCached" />
                        <dx:ListEditItem Text="IMAP.WorkerThreadPool.ConvertThreads.Total" 
                            Value="IMAP.WorkerThreadPool.ConvertThreads.Total" />
                        <dx:ListEditItem Text="IMAP.WorkerThreadPool.CopyThreads.Total" 
                            Value="IMAP.WorkerThreadPool.CopyThreads.Total" />
                        <dx:ListEditItem Text="IMAP.WorkerThreadPool.FetchThreads.Max" 
                            Value="IMAP.WorkerThreadPool.FetchThreads.Max" />
                        <dx:ListEditItem Text="IMAP.WorkerThreadPool.FetchThreads.Total" 
                            Value="IMAP.WorkerThreadPool.FetchThreads.Total" />
                        <dx:ListEditItem Text="IMAP.WorkerThreadPool.ResponseThreads.Max" 
                            Value="IMAP.WorkerThreadPool.ResponseThreads.Max" />
                        <dx:ListEditItem Text="IMAP.WorkerThreadPool.ResponseThreads.MaxPerFetch" 
                            Value="IMAP.WorkerThreadPool.ResponseThreads.MaxPerFetch" />
                        <dx:ListEditItem Text="IMAP.WorkerThreadPool.ResponseThreads.Total" 
                            Value="IMAP.WorkerThreadPool.ResponseThreads.Total" />
                        <dx:ListEditItem Text="IMAP.WorkerThreadPool.Threads.Idle" 
                            Value="IMAP.WorkerThreadPool.Threads.Idle" />
                        <dx:ListEditItem Text="IMAP.WorkerThreadPool.Threads.Max" 
                            Value="IMAP.WorkerThreadPool.Threads.Max" />
                        <dx:ListEditItem Text="IMAP.WorkerThreadPool.Threads.Total" 
                            Value="IMAP.WorkerThreadPool.Threads.Total" />
                        <dx:ListEditItem Text="Mail.CurrentByteDeliveryRate" 
                            Value="Mail.CurrentByteDeliveryRate" />
                        <dx:ListEditItem Text="Mail.CurrentByteTransferRate" 
                            Value="Mail.CurrentByteTransferRate" />
                        <dx:ListEditItem Text="Mail.CurrentMessageDeliveryRate" 
                            Value="Mail.CurrentMessageDeliveryRate" />
                        <dx:ListEditItem Text="Mail.CurrentMessageTransferRate" 
                            Value="Mail.CurrentMessageTransferRate" />
                        <dx:ListEditItem Text="Mail.DBCacheEntries" Value="Mail.DBCacheEntries" />
                        <dx:ListEditItem Text="Mail.DBCacheHits" Value="Mail.DBCacheHits" />
                        <dx:ListEditItem Text="Mail.DBCacheReads" Value="Mail.DBCacheReads" />
                        <dx:ListEditItem Text="Mail.DeliveryThreads.Max" 
                            Value="Mail.DeliveryThreads.Max" />
                        <dx:ListEditItem Text="Mail.DeliveryThreads.Total" 
                            Value="Mail.DeliveryThreads.Total" />
                        <dx:ListEditItem Text="MAIL.Hold" Value="MAIL.Hold" />
                        <dx:ListEditItem Text="Mail.TransferThreads.Concurrent.Highest" 
                            Value="Mail.TransferThreads.Concurrent.Highest" />
                        <dx:ListEditItem Text="Mail.TransferThreads.Concurrent.Max" 
                            Value="Mail.TransferThreads.Concurrent.Max" />
                        <dx:ListEditItem Text="Mail.TransferThreads.Max" 
                            Value="Mail.TransferThreads.Max" />
                        <dx:ListEditItem Text="Mail.TransferThreads.Total" 
                            Value="Mail.TransferThreads.Total" />
                        <dx:ListEditItem Text="MAIL.WaitingForDIR" Value="MAIL.WaitingForDIR" />
                        <dx:ListEditItem Text="MAIL.WaitingForDNS" Value="MAIL.WaitingForDNS" />
                        <dx:ListEditItem Text="MAIL.WaitingRecipients" Value="MAIL.WaitingRecipients" />
                        <dx:ListEditItem Text="Mem.Allocated" Value="Mem.Allocated" />
                        <dx:ListEditItem Text="Mem.Allocated.Process" Value="Mem.Allocated.Process" />
                        <dx:ListEditItem Text="Mem.Allocated.Shared" Value="Mem.Allocated.Shared" />
                        <dx:ListEditItem Text="Mem.Free" Value="Mem.Free" />
                        <dx:ListEditItem Text="Mem.PhysicalRAM" Value="Mem.PhysicalRAM" />
                        <dx:ListEditItem Text="Monitor.Last.ROUTER.Failure" 
                            Value="Monitor.Last.ROUTER.Failure" />
                        <dx:ListEditItem Text="Monitor.ROUTER.Failure" Value="Monitor.ROUTER.Failure" />
                        <dx:ListEditItem Text="Monitor.ROUTER.Fatal" Value="Monitor.ROUTER.Fatal" />
                        <dx:ListEditItem Text="NET.GroupCache.Hits" Value="NET.GroupCache.Hits" />
                        <dx:ListEditItem Text="NET.GroupCache.Misses" Value="NET.GroupCache.Misses" />
                        <dx:ListEditItem Text="NET.GroupCache.NumEntries" 
                            Value="NET.GroupCache.NumEntries" />
                        <dx:ListEditItem Text="NET.GroupCache.Size" Value="NET.GroupCache.Size" />
                        <dx:ListEditItem Text="NET.GroupCache.Used" Value="NET.GroupCache.Used" />
                        <dx:ListEditItem Text="POP3.Sessions.Inbound.Accept.Queue" 
                            Value="POP3.Sessions.Inbound.Accept.Queue" />
                        <dx:ListEditItem Text="POP3.Sessions.Inbound.Active" 
                            Value="POP3.Sessions.Inbound.Active" />
                        <dx:ListEditItem Text="POP3.Sessions.Inbound.Active.SSL" 
                            Value="POP3.Sessions.Inbound.Active.SSL" />
                        <dx:ListEditItem Text="POP3.Sessions.Inbound.BytesReceived" 
                            Value="POP3.Sessions.Inbound.BytesReceived" />
                        <dx:ListEditItem Text="POP3.Sessions.Inbound.BytesSent" 
                            Value="POP3.Sessions.Inbound.BytesSent" />
                        <dx:ListEditItem Text="POP3.Sessions.Inbound.Peak" 
                            Value="POP3.Sessions.Inbound.Peak" />
                        <dx:ListEditItem Text="POP3.Sessions.Inbound.Peak.SSL" 
                            Value="POP3.Sessions.Inbound.Peak.SSL" />
                        <dx:ListEditItem Text="POP3.Sessions.Inbound.Total" 
                            Value="POP3.Sessions.Inbound.Total" />
                        <dx:ListEditItem Text="POP3.Sessions.Inbound.Total.SSL" 
                            Value="POP3.Sessions.Inbound.Total.SSL" />
                        <dx:ListEditItem Text="POP3.Sessions.Inbound.Total.SSL.Bad_Handshake" 
                            Value="POP3.Sessions.Inbound.Total.SSL.Bad_Handshake" />
                        <dx:ListEditItem Text="POP3.Sessions.Threads.Busy" 
                            Value="POP3.Sessions.Threads.Busy" />
                        <dx:ListEditItem Text="POP3.Sessions.Threads.Idle" 
                            Value="POP3.Sessions.Threads.Idle" />
                        <dx:ListEditItem Text="POP3.Sessions.Threads.InThreadPool" 
                            Value="POP3.Sessions.Threads.InThreadPool" />
                        <dx:ListEditItem Text="POP3.Sessions.Threads.Peak" 
                            Value="POP3.Sessions.Threads.Peak" />
                        <dx:ListEditItem Text="Server.AvailabilityIndex" 
                            Value="Server.AvailabilityIndex" />
                        <dx:ListEditItem Text="Server.AvailabilityThreshold" 
                            Value="Server.AvailabilityThreshold" />
                        <dx:ListEditItem Text="Server.BusyTimeQuery.ReceivedCount" 
                            Value="Server.BusyTimeQuery.ReceivedCount" />
                        <dx:ListEditItem Text="Server.CPU.Count" Value="Server.CPU.Count" />
                        <dx:ListEditItem Text="Server.MailBoxes" Value="Server.MailBoxes" />
                        <dx:ListEditItem Text="Server.OpenRequest.MaxUsers" 
                            Value="Server.OpenRequest.MaxUsers" />
                        <dx:ListEditItem Text="Server.OpenRequest.Restricted" 
                            Value="Server.OpenRequest.Restricted" />
                        <dx:ListEditItem Text="Server.Ports" Value="Server.Ports" />
                        <dx:ListEditItem Text="Server.Sessions.Dropped" 
                            Value="Server.Sessions.Dropped" />
                        <dx:ListEditItem Text="Server.Trans.PerMinute" Value="Server.Trans.PerMinute" />
                        <dx:ListEditItem Text="Server.Trans.PerMinute.Peak" 
                            Value="Server.Trans.PerMinute.Peak" />
                        <dx:ListEditItem Text="Server.Trans.PerMinute.Peak.Time" 
                            Value="Server.Trans.PerMinute.Peak.Time" />
                        <dx:ListEditItem Text="Server.Trans.Total" Value="Server.Trans.Total" />
                        <dx:ListEditItem Text="Server.Users" Value="Server.Users" />
                        <dx:ListEditItem Text="Server.Users.Peak" Value="Server.Users.Peak" />
                        <dx:ListEditItem Text="Server.Version.Notes.BuildNumber" 
                            Value="Server.Version.Notes.BuildNumber" />
                        <dx:ListEditItem Text="Server.WorkThreads" Value="Server.WorkThreads" />
                        <dx:ListEditItem Text="SMTP.Sessions.Inbound.Accept.Queue" 
                            Value="SMTP.Sessions.Inbound.Accept.Queue" />
                        <dx:ListEditItem Text="SMTP.Sessions.Inbound.Active" 
                            Value="SMTP.Sessions.Inbound.Active" />
                        <dx:ListEditItem Text="SMTP.Sessions.Inbound.Active.SSL" 
                            Value="SMTP.Sessions.Inbound.Active.SSL" />
                        <dx:ListEditItem Text="SMTP.Sessions.Inbound.BytesReceived" 
                            Value="SMTP.Sessions.Inbound.BytesReceived" />
                        <dx:ListEditItem Text="SMTP.Sessions.Inbound.BytesSent" 
                            Value="SMTP.Sessions.Inbound.BytesSent" />
                        <dx:ListEditItem Text="SMTP.Sessions.Inbound.Peak" 
                            Value="SMTP.Sessions.Inbound.Peak" />
                        <dx:ListEditItem Text="SMTP.Sessions.Inbound.Peak.SSL" 
                            Value="SMTP.Sessions.Inbound.Peak.SSL" />
                        <dx:ListEditItem Text="SMTP.Sessions.Inbound.Total" 
                            Value="SMTP.Sessions.Inbound.Total" />
                        <dx:ListEditItem Text="SMTP.Sessions.Inbound.Total.SSL" 
                            Value="SMTP.Sessions.Inbound.Total.SSL" />
                        <dx:ListEditItem Text="SMTP.Sessions.Inbound.Total.SSL.Bad_Handshake" 
                            Value="SMTP.Sessions.Inbound.Total.SSL.Bad_Handshake" />
                        <dx:ListEditItem Text="SMTP.Sessions.Threads.Busy" 
                            Value="SMTP.Sessions.Threads.Busy" />
                        <dx:ListEditItem Text="SMTP.Sessions.Threads.Idle" 
                            Value="SMTP.Sessions.Threads.Idle" />
                        <dx:ListEditItem Text="SMTP.Sessions.Threads.InThreadPool" 
                            Value="SMTP.Sessions.Threads.InThreadPool" />
                        <dx:ListEditItem Text="SMTP.Sessions.Threads.Peak" 
                            Value="SMTP.Sessions.Threads.Peak" />
						<dx:ListEditItem Text="Traveler.Constrained.State" 
                            Value="Traveler.Constrained.State" />
						<dx:ListEditItem Text="Traveler.Push.Devices.Total" 
                            Value="Traveler.Push.Devices.Total" />
                            <dx:ListEditItem Text="Platform.LogicalDisk" 
                            Value="Platform.LogicalDisk" />
                    </Items>
                    <LoadingPanelImage Url="~/App_Themes/Office2010Blue/Editors/Loading.gif">
                    </LoadingPanelImage>
                    <LoadingPanelStyle ImageSpacing="5px">
                    </LoadingPanelStyle>
                    <ButtonStyle Width="13px">
                    </ButtonStyle>
                    <ValidationSettings>
                        <RequiredField IsRequired="True" />
                    </ValidationSettings>
                </dx:ASPxComboBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label2" runat="server" Text="...has a value that is" CssClass="lblsmallFontR"></asp:Label>
            </td>
            <td colspan="3">
                <dx:ASPxRadioButton ID="GreaterRadioButton" runat="server" Text="Greater Than" CssClass="lblsmallFont"
                    CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua"
                    SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" TextSpacing="2px" 
                    GroupName="comparision" Wrap="False">
                </dx:ASPxRadioButton>
                <dx:ASPxRadioButton ID="LessRadioButton" runat="server" CssClass="lblsmallFont" 
                    CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" 
                    GroupName="comparision" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" 
                    Text="Less Than" TextSpacing="2px" Wrap="False">
                </dx:ASPxRadioButton>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label3" runat="server" Text="...this value" CssClass="lblsmallFontR"></asp:Label>
            </td>
            <td>
                <dx:ASPxTextBox ID="ThresholdTextBox" runat="server" CssClass="lblsmallFont" 
                    Width="170px">
                    <MaskSettings Mask="&lt;0..99999&gt;" />
                    <ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" 
                        SetFocusOnError="True">
                        <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." 
                            ValidationExpression="^\d+$" />
                        <RequiredField IsRequired="True" />
                    </ValidationSettings>
                </dx:ASPxTextBox>
            </td>
            <td>
                <dx:ASPxTextBox ID="TimesInRowTextBox" runat="server" CssClass="txtsmall" 
                    Width="40px">
                    <MaskSettings Mask="&lt;0..99999&gt;" />
                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" CausesValidation="True" 
                        SetFocusOnError="True">
                        <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." 
                            ValidationExpression="^\d+$" />
                        <RequiredField IsRequired="True" />
                    </ValidationSettings>
                </dx:ASPxTextBox>
            </td>
            <td>
                <dx:ASPxLabel ID="ASPxLabel6" runat="server" CssClass="lblsmallFont" 
                    Height="16px" Text="times in a row" Width="77px" Wrap="False">
                </dx:ASPxLabel>
            </td>
        </tr>
    </table>
                        </dx:PanelContent>
</PanelCollection>
                </dx:ASPxRoundPanel>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" Width="600px" 
                    HeaderText="Current Value" GroupBoxCaptionOffsetY="-24px" Theme="Glass">
                    <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                    <HeaderStyle Height="23px">
                       </HeaderStyle>
<PanelCollection>
<dx:PanelContent ID="PanelContent4" runat="server" SupportsDisabledAttribute="True">
    <table class="style1">
        <tr>
            <td colspan="2">
                <asp:Label ID="Label1" runat="server" CssClass="lblsmallFont" 
                    
                    Text="Note: To find the&lt;b&gt; current&lt;/b&gt; value of this statistic, click the button below to query the server."></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxButton ID="CurrValButton" runat="server" Text="Current Value" 
                    CssClass="sysButton"
                    OnClick="CurrValButton_Click" Wrap="False">
                </dx:ASPxButton>
            </td>
            <td width="100%">
                <dx:ASPxTextBox ID="CurrentValTextBox" runat="server" Width="100%" 
                    CssClass="txtsmall">
                </dx:ASPxTextBox>
            </td>
        </tr>
    </table>
    </dx:PanelContent>
</PanelCollection>
                </dx:ASPxRoundPanel>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxRoundPanel ID="ASPxRoundPanel3" runat="server" Width="600px" 
                    HeaderText="Action" GroupBoxCaptionOffsetY="-24px" Theme="Glass">
                    <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                    <HeaderStyle Height="23px">
                        </HeaderStyle>
<PanelCollection>
<dx:PanelContent ID="PanelContent5" runat="server" SupportsDisabledAttribute="True">
    <div class="info"><b>Optional:</b> If you provide a server command below, it will be sent to the server if the threshold is met. If you do not provide a console command, an alert will be sent instead.
    </div>
    <table>
        <tr>
            <td>
                <dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="Console Command:" 
                    CssClass="lblsmallFont" Wrap="False">
                </dx:ASPxLabel>
            </td>
            <td>
                <dx:ASPxTextBox ID="ConsolecmdTextBox" runat="server" Width="170px" 
                    CssClass="txtsmall">
                </dx:ASPxTextBox>
            </td>
            <td>
                <dx:ASPxLabel ID="ASPxLabel8" runat="server" 
                    Text="for example: Tell HTTP Restart" CssClass="lblsmallFont">
                </dx:ASPxLabel>
            </td>
        </tr>
    </table>
    </dx:PanelContent>
</PanelCollection>
                </dx:ASPxRoundPanel>
            </td>
        </tr>
         <tr>
                <td>
                    <div id="errorDiv" class="alert alert-danger" runat="server" style="display: none">Error.
                    </div>
                    <table>
                        <tr>
                            <td>
                                <dx:ASPxButton ID="FormOkButton" runat="server" Text="OK" CssClass="sysButton"
                                    OnClick="OKButton_Click">
                                </dx:ASPxButton>
                            </td>
                            <td>
                                <dx:ASPxButton ID="FormCancelButton" runat="server" Text="Cancel" CssClass="sysButton"
                                    onclick="CancelButton_Click" 
                                    CausesValidation="False">
                                </dx:ASPxButton>
                                
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
    </table>
     <dx:ASPxPopupControl ID="ErrorMessagePopupControl" runat="server" 
                                    AllowDragging="True" ClientInstanceName="pcErrorMessage" 
                                    CloseAction="CloseButton" CssFilePath="~/App_Themes/Glass/{0}/styles.css" 
                                    CssPostfix="Glass" EnableAnimation="False" EnableViewState="False" 
                                    HeaderText="Validation Failure" Height="150px" Modal="True" 
                                    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" 
                                    SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="300px">
                                    <LoadingPanelImage Url="~/App_Themes/Glass/Web/Loading.gif">
                                    </LoadingPanelImage>
                                    <HeaderStyle>
                                    <Paddings PaddingLeft="10px" PaddingRight="6px" PaddingTop="1px" />
                                    </HeaderStyle>
                                    <ContentCollection>
                                        <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">
                                            <dx:ASPxPanel ID="Panel1" runat="server" DefaultButton="btOK">
                                                <PanelCollection>
                                                    <dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">
                                                        <div style="min-height: 70px;">
                                                            <dx:ASPxLabel ID="ErrorMessageLabel" runat="server" CssClass="lblsmallFont" 
                                                                Text="Username:">
                                                            </dx:ASPxLabel>
                                                        </div>
                                                        <div>
                                                            <table cellpadding="0" cellspacing="0" width="100%">
                                                                <tr>
                                                                    <td align="center">
                                                                        <dx:ASPxButton ID="ValidationOkButton" runat="server" AutoPostBack="False" 
                                                                            CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" 
                                                                            CssPostfix="Office2010Blue" 
                                                                            SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css" Text="OK" 
                                                                            Width="80px">
                                                                            <ClientSideEvents Click="function(s, e) { pcErrorMessage.Hide(); }" />
                                                                        </dx:ASPxButton>
                                                                        <dx:ASPxButton ID="ValidationUpdatedButton" runat="server" AutoPostBack="False" 
                                                                            CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" 
                                                                            CssPostfix="Office2010Blue" OnClick="ValidationUpdatedButton_Click" 
                                                                            SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css" Text="OK" 
                                                                            Visible="False" Width="80px">
                                                                            <ClientSideEvents Click="function(s, e) { pcErrorMessage.Hide(); }" />
                                                                        </dx:ASPxButton>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </dx:PanelContent>
                                                </PanelCollection>
                                            </dx:ASPxPanel>
                                        </dx:PopupControlContentControl>
                                    </ContentCollection>
                                </dx:ASPxPopupControl>
    <dx:ASPxRoundPanel ID="CustomRoundPanel" runat="server" 
        CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
        GroupBoxCaptionOffsetY="-24px" 
        SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="800px" 
        HeaderText="Custom Statistics" Visible="False">
        <HeaderStyle Height="23px" >
        <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
        </HeaderStyle>
        <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
        <PanelCollection>
<dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
    
   
      </dx:PanelContent>
                                        </PanelCollection>
                                    </dx:ASPxRoundPanel>
</asp:Content>
