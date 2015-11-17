if "%FORGEDIR_OPTAVER%" == "" set FORGEDIR_OPTAVER=%ProgramFiles%\forge
xcopy bin\*.dll "%FORGEDIR_OPTAVER%\bin\" /Y /R /I
xcopy bin\*.pdb "%FORGEDIR_OPTAVER%\bin\" /Y /R /I
xcopy locale\*.mo "%FORGEDIR_OPTAVER%\locale\" /Y /R /I /S
xcopy bin\*.lib "%FORGEDIR_OPTAVER%\lib\" /Y /R /I
xcopy inc\*.h "%FORGEDIR_OPTAVER%\inc\BRepMID\inc\" /Y /R /I
xcopy inc\*.hxx "%FORGEDIR_OPTAVER%\inc\BRepMID\inc\" /Y /R /I
