
@ECHO OFF
IF "%~1"=="-h" GOTO endparse
IF "%~1"=="" GOTO endparse
IF "%~2"=="" GOTO endparse
IF "%~3"=="" GOTO endparse
IF "%~4"=="" GOTO endparse
IF "%~5"=="" GOTO endparse
IF "%~6"=="" GOTO endparse

"%~6" -v dbpath="%~1" -S %~2 -A -i %~3

"%~6" -S %~2 -A -i %~4

"%~6" -v dbpath="%~1" -S %~2 -A -i %~5


GOTO actiontaken

:endparse
echo Pass parameters. e.g.& echo. SetupSQL "MSSQLDataFilePath" ServerName "VitalSignsDBCreationPath" "VitalSignsDataPath" "VSSDBCreationPath" & echo. File paths should be enclosed in double quotes.
GOTO endoffile

:actiontaken
echo "File executed"

:endoffile