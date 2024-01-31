dotnet publish .\src\CalculateAverage\CalculateAverage.csproj --nologo --runtime win-x64 --output .\outpub
Get-Date -Format HH:mm:sss.fff
&".\outpub\CalculateAverage.exe" ".\data\measurements-20.txt"
Get-Date -Format HH:mm:sss.fff