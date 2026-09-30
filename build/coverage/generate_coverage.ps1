dotnet test ../KUKULCAN.SharedKernel.JsonEngine.UnitTests `
    --settings coverage.runsettings `
    --collect:"XPlat Code Coverage"

dotnet test ../KUKULCAN.SharedKernel.JsonEngine.UnitTests `
    --settings coverage.runsettings `
    --collect:"XPlat Code Coverage"

reportgenerator `
    -reports:"../**/coverage.cobertura.xml" `
    -targetdir:"../coverage-report" `
    -reporttypes:Html
