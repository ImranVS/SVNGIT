set VS_JAVA_HOME=%~dp1java
set VS_HOME=%~dp0VitalSigns
set VS_LIB_PATH=%VS_HOME%\lib
set VS_CLASSPATH=%VS_LIB_PATH%\com.ibm.ws.admin.client_7.0.0.jar;%VS_LIB_PATH%\xalan.jar;%VS_LIB_PATH%\xercesImpl.jar;%VS_LIB_PATH%\xml-apis.jar;%VS_LIB_PATH%\serializer.jar

"%VS_JAVA_HOME%/bin/javac" -cp "%VS_CLASSPATH%" -d "%VS_HOME%\classes" "%VS_HOME%\src\ServerList.java"
"%VS_JAVA_HOME%/bin/javac" -cp "%VS_CLASSPATH%" -d "%VS_HOME%\classes" "%VS_HOME%\src\ServerStats.java"