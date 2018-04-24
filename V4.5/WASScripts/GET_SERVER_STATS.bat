@echo off
@REM USAGE: GET_SERVER_STATS.bat <hostname> <portno.> <conntype> <was uid> <was pwd> <realm> <pathToClient> <nodeName> <serverName> 
set HOST_NAME=%1
set CONN_PORT=%2
set CONN_TYPE=%3
set WAS_USER=%4
set WAS_PWD=%5
set REALM=defaultWIMFileBasedRealm
set APP_CLIENT_HOME=%~dpn7
set WAS_NODE=%8
set WAS_SERVER=%9
set VS_JAVA_HOME=%APP_CLIENT_HOME%\java
set VS_HOME=%~dp0VitalSigns
set VS_LIB_PATH=%VS_HOME%\lib
set VS_CLASSPATH=%VS_LIB_PATH%\com.ibm.ws.admin.client_7.0.0.jar;%VS_LIB_PATH%\com.ibm.ws.security.crypto.jar;%VS_LIB_PATH%\xalan.jar;%VS_LIB_PATH%\xercesImpl.jar;%VS_LIB_PATH%\xml-apis.jar;%VS_LIB_PATH%\serializer.jar;%VS_HOME%\classes\;.

@REM CALL "%APP_CLIENT_HOME%bin/retrieveSigners.bat" -autoAcceptBootstrapSigner -conntype %CONN_TYPE% -host %HOST_NAME% -port %RMI_PORT% -user %WAS_USER% -password %WAS_PWD%
@REM pause

IF %CONN_TYPE% EQU "RMI" (
CALL "%VS_JAVA_HOME%/bin/java" -cp "%VS_CLASSPATH%" -Dcom.ibm.CORBA.ConfigURL=file:"%VS_HOME%\properties\sas.client.props" -Dcom.ibm.SSL.ConfigURL=file:"%APP_CLIENT_HOME%\properties\ssl.client.props" ServerStats %HOST_NAME% %CONN_PORT% %CONN_TYPE% %WAS_USER% %WAS_PWD% %WAS_NODE% %WAS_SERVER% %REALM%
)

IF %CONN_TYPE% EQU "SOAP" (
CALL "%VS_JAVA_HOME%/bin/java" -cp "%VS_CLASSPATH%" -Dcom.ibm.SOAP.ConfigURL=file:"%VS_HOME%\properties\soap.client.props" -Dcom.ibm.SSL.ConfigURL=file:"%APP_CLIENT_HOME%\properties\ssl.client.props" ServerStats %HOST_NAME% %CONN_PORT% %CONN_TYPE% %WAS_USER% %WAS_PWD% %WAS_NODE% %WAS_SERVER% NULL
)

IF %CONN_TYPE% EQU RMI (
CALL "%VS_JAVA_HOME%/bin/java" -cp "%VS_CLASSPATH%" -Dcom.ibm.CORBA.ConfigURL=file:"%VS_HOME%\properties\sas.client.props" -Dcom.ibm.SSL.ConfigURL=file:"%APP_CLIENT_HOME%\properties\ssl.client.props" ServerStats %HOST_NAME% %CONN_PORT% %CONN_TYPE% %WAS_USER% %WAS_PWD% %WAS_NODE% %WAS_SERVER% %REALM%
)

IF %CONN_TYPE% EQU SOAP (
CALL "%VS_JAVA_HOME%/bin/java" -cp "%VS_CLASSPATH%" -Dcom.ibm.SOAP.ConfigURL=file:"%VS_HOME%\properties\soap.client.props" -Dcom.ibm.SSL.ConfigURL=file:"%APP_CLIENT_HOME%\properties\ssl.client.props" ServerStats %HOST_NAME% %CONN_PORT% %CONN_TYPE% %WAS_USER% %WAS_PWD% %WAS_NODE% %WAS_SERVER% NULL
)

