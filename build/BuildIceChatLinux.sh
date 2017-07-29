# build IceChat with Mono with .NET 4

rm -rf ./IceChatBuild

mkdir IceChatBuild

xbuild ../src/IceChat/IceChat9-45-VS2015.sln

mv ../src/IceChat/bin/Debug/* ./IceChatBuild

rm -f ./IceChatBuild/*.pdb

echo Completed! Files copied to the IceChatBuild folder
