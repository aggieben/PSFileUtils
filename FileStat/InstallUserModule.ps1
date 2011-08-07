param($Name, $SourcePath)

$ModulePath = $env:PSModulePath.Split(';')[0]
Write-Host "$SourcePath -> $ModulePath\$Name"

if (!(test-path -path "$ModulePath\$Name")) {
	mkdir "$ModulePath\$Name"
}
copy "$SourcePath" "$ModulePath\$Name"