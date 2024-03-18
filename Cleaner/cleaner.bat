@echo off

set DIR=
set FILE_EXTENSION=

GOTO parse_cmd_args

:print_help_and_exit
    echo:
    echo The Cleaner utility allows you to delete all files with a given extension in a given directory.
    echo:
    echo Usage: cleaner.bat -d ^<dir_name^> -e ^<file_extension^>
    echo  Example:
    echo  cleaner.bat -d D:\dir_name -e txt
    echo:
exit /b

:parse_cmd_args
    if "%~1"=="" (
        GOTO endparse
    ) else if "%~1"=="-d" (
        set DIR=%~2
        shift
    ) else if "%~1"=="-e" (
        set FILE_EXTENSION=%~2
        shift
    ) else (
        GOTO print_help_and_exit
    )
    shift
    GOTO parse_cmd_args
:endparse

if [%DIR%] == [] (
    GOTO print_help_and_exit
)

if [%FILE_EXTENSION%] == [] (
    GOTO print_help_and_exit
)

if not exist %DIR%\ (
    echo Directory '%DIR%' doesn't exist!
    exit /b
)

del %DIR%\*.%FILE_EXTENSION% 2> nul

if %errorlevel% == 0 (
    echo Successfully cleaned!
) else (
    echo Some errors occured, while cleaning!
)

exit /b