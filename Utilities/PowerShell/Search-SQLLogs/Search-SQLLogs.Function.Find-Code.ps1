# Import LogContainer function
. C:\Users\Josh.Mascorro.INTERNAL\Documents\Oncor\test_dgs_aegis\Utilities\Search-SQLLogs\Search-SQLLogs.Class.LogContainer.ps1
# Import Copy-Directory function
. C:\Users\Josh.Mascorro.INTERNAL\Documents\Oncor\test_dgs_aegis\Utilities\Search-SQLLogs\Search-SQLLogs.Function.Copy-Directory.ps1
# Import Get-SkipBool function
. C:\Users\Josh.Mascorro.INTERNAL\Documents\Oncor\test_dgs_aegis\Utilities\Search-SQLLogs\Search-SQLLogs.Function.Get-SkipBool.ps1

Function Find-Code() {
    Param(
            [string] $Directory, 
    
            [string] $Code, 
            
            [switch] $ExecuteOnCopy
         )
    if($ExecuteOnCopy){ $destination = Copy-Directory -Directory $Directory }
    else { $destination = $Directory } 

    # Execute function to copy the source directory
    # Collect all file objects in the copy directory
    $files = Get-ChildItem $destination -Recurse -File
    # Initialize array of skipped files and array of missed commits
    $errorsFoundFiles = @()
    $warningsFoundFiles = @()
    $sqlCodeFoundFiles = @()
    # Initialize variable for counting skipped files and missed commits
    $errorsFoundFilesCount = 0
    $warningsFoundFilesCount = 0
    $sqlCodeFoundCount = 0

    # Run through each file to remove them from the search (skip)
    Foreach ($file in $files) {
        Write-Host "Searching $file.Name ...."
        $found = $false
        $error = $false
        $warning = $false
        # Execute function to create boolean and skip these folders/files when true
        $boolSkip = Get-SkipBool -File $file
        # Set file name variable
        $fileName = $file.Name
        $fileFullName = $file.FullName
        # Skip files where boolSkip is true, collect name and count on skipped files
        $sqlCodeFoundLineNos = @()
        $errorFoundLineNos = @()
        $warningFoundLineNos = @()
        If ($boolSkip) {
            #Excludes 
            If ($fileFullName.Contains(".git")) {
                continue
            }
            Else {
                $skippedFilesCount += 1
                $skippedFiles += $fileName
                continue
            }       
        }
        #Initialize array
        # Search these  files 
        Elseif ($fileName -match '.sql') {
            # Set script to contents of file (use force switch to override permission)
            $script = $file | Get-Content -Force
            # Set script to itselt after removing Code
            $lineNo = 0
            Foreach ($line in $script) {
                $lineNo += 1
                if ($line -match "$Code") {
                    $sqlCodeFoundLineNos += $lineNo
                    $sqlCodeFoundCount += 1
                    $found = $true
                }
            }
            if ($found -eq $true) {
                $lineNoString = foreach ($ln in $sqlCodeFoundLineNos) {"`n`t`t`t`t`t`t`t`tLine $ln"}
                $sqlCodeFoundFiles += "$fileName`n`t`t`t`t`t`t`t`t`t$lineNoString`n"
            }
        }
        #Initialize array
        
        Elseif ($fileName -match ".log") {
            $log = $file | Get-Content -Force
            $lineNo = 0
            Foreach ($line in $log) {
                $lineNo += 1
                if ($line -match "error") {
                    $errorFoundLineNos += $lineNo
                    $errorsFoundFilesCount += 1
                    $found = $true
                    $error = $true
                }
                Elseif ($line -match "warning") {
                    $warningFoundLineNos += $lineNo
                    $warningsFoundFilesCount += 1
                    $found = $true
                    $warning = $true
                }
            }
            if ($found -eq $true -and $error -eq $true) {
                $lineNoString = foreach ($ln in $errorFoundLineNos) {"`n`t`t`t`t`t`t`t`tLine $ln"}
                $errorsFoundFiles += "$fileName`n`t`t`t`t`t`t`t`t`t$lineNoString`n"
            }
            elseif ($found -eq $true -and $warning -eq $true) {
                $lineNoString = foreach ($ln in $warningFoundLineNos) {"`n`t`t`t`t`t`t`t`tLine $ln"}
                $warningsFoundFiles += "$fileName`n`t`t`t`t`t`t`t`t`t$lineNoString`n"
            }
        }
    }

     # Initialize instance of Log class
    $sqlcodeFoundLog = [Log]::new()
    $sqlcodeFoundLog.Name = "Matches for '$Code'`n`t------------------------"
    $sqlcodeFoundLog.DataList = $sqlCodeFoundFiles
    $sqlcodeFoundLog.Count = $sqlCodeFoundCount
    # Initialize instance of Log class
    $errorsFoundLog = [Log]::new()
    $errorsFoundLog.Name = "Errors`n`t------"
    $errorsFoundLog.DataList = $errorsFoundFiles
    $errorsFoundLog.Count = $errorsFoundFilesCount
    # Initialize instance of Log class
    $warningsFoundLog = [Log]::new()  
    $warningsFoundLog.Name = "Warnings`n`t---------"
    $warningsFoundLog.DataList = $warningsFoundFiles
    $warningsFoundLog.Count = $warningsFoundFilesCount
    # Initialize instance of Log class
    $logCodeRemoved = [Log]::new()
    $logCodeRemoved.Name = "Log Code Found"
    $logCodeRemoved.DataList = $logCodeFound
    $logCodeRemoved.Count = $LogcodeRemovedCount

    #Initialize instance of Log Container class
    $LogContainer = [LogContainer]::new()
    $LogContainer.LogObject1 = $sqlcodeFoundLog
    $LogContainer.LogObject2 = $errorsFoundLog
    $LogContainer.LogObject3 = $warningsFoundLog

    # Return log Object
    return $LogContainer
}
