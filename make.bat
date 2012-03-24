@echo off
cls

@%~dp0tools\Pencil\Pencil.exe %~dp0Build.cs %*

if not %ERRORLEVEL% ==0 goto error

rem Success, paint it green.
	color 2F
	goto done

:error
rem Fail!, paint it red.
	color 4F
	
:done
	color