Write-Output "IdentityServer------------->"
Set-Location "D:\projects\DotNet.Quick.AllInOne\sources\IdentityServer4\QuickIdentityServer"
dotnet ./bin/Debug/netcoreapp2.1/QuickIdentityServer.dll

Write-Output "API------------->"
Set-Location "D:\projects\DotNet.Quick.AllInOne\sources\IdentityServer4\QuicktIdentityServer.Api"
dotnet ./bin/Debug/netcoreapp2.1/QuicktIdentityServer.Api.dll

Write-Output "MvcClient------------->"
Set-Location "D:\projects\DotNet.Quick.AllInOne\sources\IdentityServer4\QuickIdentityServer.MvcClient"
dotnet ./bin/Debug/netcoreapp2.1/QuickIdentityServer.MvcClient.dll

Set-Location ../