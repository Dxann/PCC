$ErrorActionPreference = 'Stop'

$out = Join-Path (Get-Location) 'code-listing.txt'

try {
  $probe = [System.IO.File]::Open($out, [System.IO.FileMode]::Create, [System.IO.FileAccess]::Write, [System.IO.FileShare]::None)
  $probe.Close()
} catch {
  $stamp = Get-Date -Format 'yyyyMMdd-HHmmss'
  $out = Join-Path (Get-Location) ("code-listing-$stamp.txt")
  $probe = [System.IO.File]::Open($out, [System.IO.FileMode]::Create, [System.IO.FileAccess]::Write, [System.IO.FileShare]::None)
  $probe.Close()
}

function Add-File {
  param(
    [Parameter(Mandatory=$true)][string]$Path
  )

  Add-Content -Path $out -Value ('FILE: ' + $Path) -Encoding utf8
  Add-Content -Path $out -Value (Get-Content -LiteralPath $Path -Raw -Encoding utf8) -Encoding utf8
  Add-Content -Path $out -Value "`r`n`r`n" -Encoding utf8
}

$files = @()

$files += Get-ChildItem -Path '.\PCConfigurator.api' -Recurse -File -Include *.cs
$files += Get-ChildItem -Path '.\pcconfigurator-client\src' -Recurse -File -Include *.js,*.jsx,*.css

$files = $files | Where-Object {
  $_.FullName -notmatch '\\(bin|obj|node_modules|build|\.vs|\.git|Migrations|SeedData)\\' -and
  $_.FullName -notmatch '\\nginx\\' -and
  $_.Name -notmatch '^appsettings.*\.json$' -and
  $_.Name -ne 'launchSettings.json' -and
  $_.Name -notlike 'Dockerfile*' -and
  $_.Name -ne 'docker-compose.yml' -and
  $_.Name -ne 'dump.sql'
}

$files = $files | Sort-Object FullName

foreach ($f in $files) {
  Add-File -Path $f.FullName
}

Write-Host "Created: $out"
