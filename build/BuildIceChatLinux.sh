# build IceChat with Mono with .NET 4
# openssl req -newkey rsa:2048 -nodes -keyout key.pem -x509 -days 365 -out certificate.pem
# openssl pkcs12 -inkey key.pem -in certificate.pem -export -out certificate.p12

rm -rf ./IceChatBuild

mkdir IceChatBuild

xbuild ../src/IceChat/IceChat9-VS2015.sln

mv ../src/IceChat/bin/Debug/* ./IceChatBuild

rm -f ./IceChatBuild/*.pdb

echo Completed! Files copied to the IceChatBuild folder
