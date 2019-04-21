dotnet build

dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:Exclude="[*]*ErrorHandlingMiddleware%2c[*]*Startup%2c[*]*Program%2c[*]*AutoGeneratedProgram"

dotnet .\ReportCoverage\ReportGenerator.dll "-reports:.\*.Tests\coverage.opencover.xml;" "-targetdir:.\CodeCoverageHTML"

get-childitem -path ((Get-Item -Path ".\").FullName) -include coverage.opencover.xml -recurse | remove-item

Start-Process ".\CodeCoverageHTML\index.htm"