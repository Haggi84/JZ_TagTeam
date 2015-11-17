if "%FORGEDIR_OPTAVER%" == "" set FORGEDIR_OPTAVER=%ProgramFiles%\forge
xcopy bin\Debug\*.dll "%FORGEDIR_OPTAVER%\bin\" /Y /R /I
xcopy bin\Debug\*.exe "%FORGEDIR_OPTAVER%\bin\" /Y /R /I
REM xcopy bin\*.pdb "%FORGEDIR_OPTAVER%\bin\" /Y /R /I
REM xcopy locale\*.mo "%FORGEDIR_OPTAVER%\locale\" /Y /R /I /S
REM xcopy bin\*.lib "%FORGEDIR_OPTAVER%\lib\" /Y /R /I
REM xcopy inc\*.h "%FORGEDIR_OPTAVER%\inc\BRepMID\inc\" /Y /R /I
REM xcopy inc\*.hxx "%FORGEDIR_OPTAVER%\inc\BRepMID\inc\" /Y /R /I
