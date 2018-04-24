@ECHO OFF
IF "%~1"=="-h" GOTO endparse
IF "%~1"=="" GOTO endparse
IF "%~2"=="" GOTO endparse
IF "%~3"=="" GOTO endparse

GOTO actiontaken

:endparse
echo Passed parameters. e.g.& echo. SetupSQL "%~1" "%~2" "%~3" "%~4"
GOTO endoffile

:actiontaken
echo "Executing ................ %~4 .........................."
sqlcmd -S "%~1" -U "%~2" -P "%~3" -i "%~4"
echo "File executed"

:endoffile