#.D:\Mukund\E\RPRWyatt\VSTrunk\PowerShell_POC\PSPOC1\bin\Debug\Scripts\EX_ResponseTime.ps1 -server win-kurrr2qsdj0.jnittech.com

Get-WmiObject -Class Win32_PingStatus -Filter Address= JNITTECH-EXCHG1 | Select-Object -Property Address,ResponseTime,StatusCode