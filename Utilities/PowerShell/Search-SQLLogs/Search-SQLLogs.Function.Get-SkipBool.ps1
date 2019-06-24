Function Get-SkipBool($File) {
    $fileName = $File.Name
    $fileDirectoryName = $File.DirectoryName
    $bool = $fileDirectoryName.Contains('Master-Script') -OR $fileName -LIKE "*README*" -OR $fileDirectoryName.Contains("Package") -OR $fileName.Contains('PKG') -OR $fileDirectoryName.Contains('.git') -OR $fileName.Contains('Streetlight')
    return $bool
}