@echo off
rem https://docs.microsoft.com/ja-jp/dotnet/core/tools/dotnet-publish
rem https://docs.microsoft.com/ja-jp/dotnet/core/deploying/single-file

dotnet publish -c Release -p:PublishSingleFile=true --self-contained -r win-x64
