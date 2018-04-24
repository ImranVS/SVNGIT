#D:\Mukund\E\RPRWyatt\VSTrunk\PowerShell_POC\PSPOC1\bin\Debug\Scripts\EX_QueuesBasedOnIdentity.ps1 -identity 'WIN-KURRR2QSDJ0\Submission'
#MessageCount - Queue


#Server\QueueJetID (Int64), \QueueJetID, Server\*, Server\NextHopDomain, \NextHopDomain, Server\Poison, Server\Submission, Server\Unreachable

param(
    [Parameter(ParameterSetName='identity')] [string]$identity
)

Get-Queue -Identity $identity -ErrorAction Stop | FL