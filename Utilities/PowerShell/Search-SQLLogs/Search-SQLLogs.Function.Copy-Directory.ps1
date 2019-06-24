# Function for returing a copy of the source directory and renaming to "Copy of <source directory>"
Function Copy-Directory($Directory) {
    #
    $destination = Get-Item $Directory
    $destinationName = $destination.Name
    $destination = $Directory.Replace("`\$destinationName","`\Copy of $destinationName")
    Copy-Item $Directory -Destination $destination -Recurse
    return $destination
}