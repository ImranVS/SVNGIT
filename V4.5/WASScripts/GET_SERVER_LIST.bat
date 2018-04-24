@echo off
@REM set HOST_NAME=stsc.jnitinc.com
@REM set RMI_PORT=8702
@REM USAGE: GET_SERVER_LIST.bat <hostname> <port no.> <conn type> <was uid> <waspwd> <realm> <pathToClient>
set HOST_NAME=%1
set CONN_PORT=%2
set CONN_TYPE=%3
set WAS_USER=%4
set WAS_PWD=%5
set REALM=defaultWIMFileBasedRealm

set APP_CLIENT_HOME=%~dpn7
set VS_JAVA_HOME=%APP_CLIENT_HOME%\java
set VS_HOME=%~dp0VitalSigns
set VS_LIB_PATH=%VS_HOME%\lib
set VS_CLASSPATH=%VS_LIB_PATH%\com.ibm.ws.admin.client_7.0.0.jar;%VS_LIB_PATH%\com.ibm.ws.security.crypto.jar;%VS_LIB_PATH%\xalan.jar;%VS_LIB_PATH%\xercesImpl.jar;%VS_LIB_PATH%\xml-apis.jar;%VS_LIB_PATH%\serializer.jar;%VS_HOME%\classes\;.

@REM pause
IF %CONN_TYPE% EQU RMI (
CALL "%APP_CLIENT_HOME%/bin/retrieveSigners.bat" -autoAcceptBootstrapSigner -conntype %CONN_TYPE% -host %HOST_NAME% -port %CONN_PORT% -user %WAS_USER% -password %WAS_PWD%
CALL "%VS_JAVA_HOME%/bin/java" -cp "%VS_CLASSPATH%" -Dcom.ibm.CORBA.ConfigURL=file:"%VS_HOME%/properties/sas.client.props" -Dcom.ibm.SSL.ConfigURL=file:"%APP_CLIENT_HOME%\properties\ssl.client.props" ServerList %HOST_NAME% %CONN_PORT% %CONN_TYPE% %WAS_USER% %WAS_PWD% %REALM%
)

IF %CONN_TYPE% EQU SOAP (
CALL "%APP_CLIENT_HOME%/bin/retrieveSigners.bat" -autoAcceptBootstrapSigner -conntype %CONN_TYPE% -host %HOST_NAME% -port %CONN_PORT% -user %WAS_USER% -password %WAS_PWD%
CALL "%VS_JAVA_HOME%/bin/java" -cp "%VS_CLASSPATH%" -Dcom.ibm.SOAP.ConfigURL=file:"%VS_HOME%/properties/soap.client.props" -Dcom.ibm.SSL.ConfigURL=file:"%APP_CLIENT_HOME%\properties\ssl.client.props" ServerList %HOST_NAME% %CONN_PORT% %CONN_TYPE% %WAS_USER% %WAS_PWD% NULL
)

IF %CONN_TYPE% EQU "RMI" (
CALL "%APP_CLIENT_HOME%/bin/retrieveSigners.bat" -autoAcceptBootstrapSigner -conntype %CONN_TYPE% -host %HOST_NAME% -port %CONN_PORT% -user %WAS_USER% -password %WAS_PWD%
CALL "%VS_JAVA_HOME%/bin/java" -cp "%VS_CLASSPATH%" -Dcom.ibm.CORBA.ConfigURL=file:"%VS_HOME%/properties/sas.client.props" -Dcom.ibm.SSL.ConfigURL=file:"%APP_CLIENT_HOME%\properties\ssl.client.props" ServerList %HOST_NAME% %CONN_PORT% %CONN_TYPE% %WAS_USER% %WAS_PWD% %REALM%
)

IF %CONN_TYPE% EQU "SOAP" (
CALL "%APP_CLIENT_HOME%/bin/retrieveSigners.bat" -autoAcceptBootstrapSigner -conntype %CONN_TYPE% -host %HOST_NAME% -port %CONN_PORT% -user %WAS_USER% -password %WAS_PWD%
CALL "%VS_JAVA_HOME%/bin/java" -cp "%VS_CLASSPATH%" -Dcom.ibm.SOAP.ConfigURL=file:"%VS_HOME%/properties/soap.client.props" -Dcom.ibm.SSL.ConfigURL=file:"%APP_CLIENT_HOME%\properties\ssl.client.props" ServerList %HOST_NAME% %CONN_PORT% %CONN_TYPE% %WAS_USER% %WAS_PWD% NULL
)