#.\EX_Server_Load1.ps1 -Server win-kurrr2qsdj0.jnittech.com
param([string]$server, [int]$sampleinterval, [int]$maxsamples)    

#The following scriptlet will return the average CPU % over a period of "" seconds with "" samples averaged:
(get-counter -ComputerName $server -Counter "\Processor(_Total)\% Processor Time" -SampleInterval $sampleinterval -MaxSamples $maxsamples |
    select -ExpandProperty countersamples | select -ExpandProperty cookedvalue | Measure-Object -Average).average

