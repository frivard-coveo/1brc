dotnet publish .\src\CalculateAverage\CalculateAverage.csproj --nologo --runtime win-x64 --output .\outpub
&".\outpub\CalculateAverage.exe" ".\measurements.txt"
