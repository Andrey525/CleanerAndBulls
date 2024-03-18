#!/bin/bash

DIR=""
FILE_EXTENSION=""

function print_help
{
  echo -e "\nThe Cleaner utility allows you to delete all files with a given extension in a given directory.\n"
  echo "Usage: cleaner.sh -d <dir_name> -e <file_extension>"
  echo " Example:"
  echo -e " cleaner -d /home/username -e txt\n"
}

function parse_cmd_args
{
  while [ "$1" ]
  do
    case "$1" in
      -d)
        DIR="$2"
        shift ;;
      -e)
        FILE_EXTENSION="$2"
        shift ;;
      *)
        print_help
        exit -1;;
    esac
    shift
  done

  if [ ! "$DIR" ] || [ ! "$FILE_EXTENSION" ];
  then
    print_help
    exit -1
  fi
}

function delete_files
{
  if [ ! -d "$DIR" ];
  then
    echo "Directory $DIR doesn't exist!"
    exit -1
  fi

  find $DIR/ -maxdepth 1 -name *.$FILE_EXTENSION -delete 2> /dev/null

  if [ $? -eq 0 ];
  then
    echo "Successfully cleaned!"
  else
    echo "Some errors occured, while cleaning!"
  fi
}


parse_cmd_args "$@"
delete_files
exit 0