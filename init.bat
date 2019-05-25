@echo off
cd %~dp0..\
set dir=%cd%\

set /p projname=Enter project name: 
echo Project name is %projname%
echo Project directory will be %dir%%projname%\
set /p confirm=Confirm (y) 
if not %confirm%==y set /p a="The batchfile will now exit."
if not %confirm%==y exit

echo.
cls

set gitignore=%dir%.gitignore

echo Checking for file %gitignore%...
if exist %gitignore% set /p confirm=gitignore found, overwrite (y)? 
if %confirm%==y echo Deleting gitignore...
if %confirm%==y del %gitignore%
if not exist %gitignore% echo Copying gitignore...
if not exist %gitignore% copy %dir%StandardUnity\.gitignore %gitignore%

set gitattr=%dir%.gitattributes
echo.

echo Checking for file %gitattr%...
if exist %gitattr% set /p confirm=gitattributes found, overwrite (y)? 
if %confirm%==y echo Deleting gitattributes...
if %confirm%==y del %gitattr%
if not exist %gitattr% echo Copying gitattributes...
if not exist %gitattr% copy %dir%StandardUnity\.gitattributes %gitattr%

echo.
cls

set targetdir=%dir%%projname%\Assets\StandardUnity\

echo StandardUnity will be installed in %targetdir%
mkdir %targetdir%
set /p a=Press enter...
echo.
echo %targetdir%StandardUnity
echo %dir%StandardUnity\
mklink /d %targetdir%StandardUnity %dir%StandardUnity\
echo The symlink was created
set /p a=Press enter...

cls
echo.
set /p a=The batchfile has finished, please press enter