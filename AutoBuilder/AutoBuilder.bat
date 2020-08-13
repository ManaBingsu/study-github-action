@Echo off
setlocal EnableDelayedExpansion
title Auto builder ver 0.0.1
set BUILDERCONFIG=./BuilderConfig/BuilderConfig.txt
set PLATFORMCONFIG=./BuilderConfig/PlatformConfig.txt
echo Read configuration from "%BUILDERCONFIG%"
echo.
rem #Set editor and project path

for /f "tokens=1,2 delims=$$" %%a in (%BUILDERCONFIG%) do (
	if "%%a"=="PROJECTPATH" (
		set PROJECTPATH=%%b
	)
	if "%%a"=="EDITORPATH" (
		set EDITORPATH=%%b
	)
	if "%%a"=="GAMEVERSION" (
		set GAMEVERSION=%%b
	)
)
echo PROJECTPATH	: %PROJECTPATH%
echo EDITORPATH	: %EDITORPATH%

echo.
echo Read configuration from "%PLATFORMCONFIG%"
echo.
echo [---BUILD LIST---]
for /f "tokens=1,2 delims=$$" %%a in (%PLATFORMCONFIG%) do (
	if "%%a"=="COMPRESSION" (
		echo COMPRESSION	: %%b
	)
	if "%%a"=="PLATFORM" (
		echo PLATFORM	: %%b
		echo.
	)
)
for /f "tokens=1,2 delims=$$" %%a in (%PLATFORMCONFIG%) do (
	if "%%a"=="COMPRESSION" (
		set COMPRESSION=%%b
	)
	if "%%a"=="PLATFORM" (
		set PLATFORM=%%b
		call "./PathConfig/PlatformBuilder.bat"
	)
)
pause