#Following Commented Script configures Lync Enabled users for Persistant Chat Testing:
#New-CsHealthMonitoringConfiguration -Identity lync.jnittech.com -FirstTestUserSipUri “sip:joe@jnittech.com” -SecondTestUserSipUri “sip:tarun@jnittech.com”
#The Nest Script measures the connectivity between those 2 uesrs and measures the response time between chats
Test-CSim -TargetFqdn LYNC.jnittech.com