 
dim http

On Error Resume Next


Set http = CreateObject("Microsoft.XmlHttp")
rem You must edit the path below to reflect the true path of the local environment
http.open "GET", "http://localhost/SchedReports/SchedReport.aspx", FALSE
http.send
http=Nothing 