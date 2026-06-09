cd c:\git\ai-test\ParkingPlaces\.vs 2>$null
try {
    # Delete all .slnx and their subdirectories (Recursive removal of solution cache files)
    Remove-Item "ParkingPlaces.slnx" -Recurse -Force | Out-Null

} catch { }

# Verify deletion  
if (-NOT $PSScriptRoot\ParkingPlaces.slnx\exists) {
    Write-Host "Success"
} else { 
    # If we still have it, let's try to use a .tmp renaming trick + delete
    if ($PSScriptRoot\ParkingPlaces.slnx\FileContentIndex -PathType Container ) {
        Remove-Item "$PSScriptRoot\ParkingPlaces.slnx\*" -Recurse | Out-Null  
    } 
}
