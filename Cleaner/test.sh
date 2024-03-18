cp -r ./testdir ./testdir_copy

echo "Initial state:"
echo "testdir:" && ls testdir && echo "subdir:" && ls testdir/subdir
echo -e "\n"

echo "./cleaner.sh -d testdir -e txt"
./cleaner.sh -d testdir -e txt
diff -qr ./testdir ./testdir_copy
# echo "testdir:" && ls testdir && echo "subdir:" && ls testdir/subdir
echo -e "\n"

cp -r ./testdir_copy/* ./testdir

echo "./cleaner.sh -d testdir -e tar.gz"
./cleaner.sh -d testdir -e tar.gz
diff -qr ./testdir ./testdir_copy
# echo "testdir:" && ls testdir && echo "subdir:" && ls testdir/subdir
echo -e "\n"

cp -r ./testdir_copy/* ./testdir && rm -rf ./testdir_copy
