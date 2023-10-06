# Directories
$sourceDirectory = "./node_modules/flag-icons/flags/1x1"
$targetDirectory = "./src/assets/country_flags"

# Delete all SVGs in the target directory
Get-ChildItem -Path $targetDirectory -Filter "*.svg" | Remove-Item -Force

# Copy all SVGs from the source directory to the target directory
Copy-Item -Path "$sourceDirectory/*.svg" -Destination $targetDirectory

# Navigate to the target directory
Set-Location -Path $targetDirectory

# Minify all SVGs in the target directory and make them circles
Get-ChildItem -Filter "*.svg" | ForEach-Object {
    Write-Output "Processing $($_.Name)..."

    # Load the SVG content
    [xml]$svgContent = Get-Content $_.FullName

    # Add a mask to the SVG
    $maskElement = $svgContent.CreateElement("mask", "http://www.w3.org/2000/svg")
    $maskElement.SetAttribute("id", "circleMask")

    $circleElement = $svgContent.CreateElement("circle", "http://www.w3.org/2000/svg")
    $circleElement.SetAttribute("cx", "256") # Center X of the circle
    $circleElement.SetAttribute("cy", "256") # Center Y of the circle
    $circleElement.SetAttribute("r", "256")  # Radius of the circle
    $circleElement.SetAttribute("fill", "white")

    $maskElement.AppendChild($circleElement)
    $svgContent.DocumentElement.AppendChild($maskElement)

    # Apply the mask to the SVG content (all child elements of the root SVG element)
    foreach ($child in $svgContent.DocumentElement.ChildNodes) {
        if ($child.NodeType -eq "Element" -and $child.Name -ne "mask") {
            $child.SetAttribute("mask", "url(#circleMask)")
        }
    }

    # Save the modified SVG
    $svgContent.OuterXml | Set-Content $_.FullName

    # Minify the SVG
    svgo $_.FullName --multipass
}

Write-Output "All SVGs in $targetDirectory have been minified and turned into circles!"
