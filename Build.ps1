$moduleName = 'PoShLog.Sinks.Http'

New-Item "$PSScriptRoot\output\$moduleName" -ItemType Directory -Force | Out-Null
Get-ChildItem "$PSScriptRoot\output\$moduleName\bin\*" | Remove-Item -Recurse -Force

Copy-Item "$PSScriptRoot\src\Metadata\*" "$PSScriptRoot\output\$moduleName" -Recurse -Force
dotnet build "$PSScriptRoot\src" -o "$PSScriptRoot\output\$moduleName\bin" -c Release

Get-ChildItem "$PSScriptRoot\output\$moduleName\bin\*" -Include '*.pdb', '*.json' | Remove-Item -Force