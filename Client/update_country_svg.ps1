# Directories
$sourceDirectory = "./node_modules/flag-icons/flags/1x1"
$targetDirectory = "./src/assets/country_flags"

# Delete all SVGs in the target directory
Get-ChildItem -Path $targetDirectory -Filter "*.svg" | Remove-Item -Force

# Copy all SVGs from the source directory to the target directory
Copy-Item -Path "$sourceDirectory/*.svg" -Destination $targetDirectory

# Navigate to the target directory
Set-Location -Path $targetDirectory

# Minify all SVGs in the target directory
Get-ChildItem -Filter "*.svg" | ForEach-Object {
    Write-Output "Processing $($_.Name)..."
    svgo $_.FullName --multipass
}

Write-Output "All SVGs in $targetDirectory have been minified!"
