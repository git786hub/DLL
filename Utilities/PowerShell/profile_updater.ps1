#clear screen
clear

# Profile Updater

# This script updates the user's powershell profile to include the functions in their local powershell sessions.

#updating the profile

function Update-Profile($ScriptFullName) {
    if (Test-Path -Path $profile -PathType Leaf) {
        $profile_old = Get-Content -Path $profile
        $new_module_path = $ScriptFullName
        $new_module = Get-Content -Path $new_module_path
        $lines = @()
        foreach ($line in $new_module) {
            $lines += $line
        }
        Set-Content -Path $profile -Value $new_module
    }
    else {
        New-Item -Path $profile
        $profile_old = Get-Content -Path $profile
        $new_module_path = $ScriptFullName
        $new_module = Get-Content -Path $new_module_path
        $lines = @()
        foreach ($line in $new_module) {
            $lines += $line
        }
        Set-Content -Path $profile -Value $new_module
    }
}
function Update-DotSource($ScriptPath, $updatedDSList, $numLines) {
    $scriptContent = Get-Content -Path $ScriptPath
    $lines = @()
    $lineNo = 1
    
    foreach ($line in $scriptContent) {
        $lines += $line
    }
    foreach ($no in 0..$numLines) {
        $lines[$no] = $updatedDSList[$no]
    }

    #$lines[$numLines] = $lines[$numLines] + "`n"

    Set-Content -Path $ScriptPath -Value $lines
}
function Dynamic-Path($AppendPath) {
    $outputPath = $PSScriptRoot + $AppendPath
    return $outputPath
}

try 
{
    # Dynamic Dot Source filepaths

    $DS_path_LogContainer = Dynamic-Path -AppendPath "\Search-SQLLogs\Search-SQLLogs.Class.LogContainer.ps1"
    $DS_path_CopyDirectory = Dynamic-Path -AppendPath "\Search-SQLLogs\Search-SQLLogs.Function.Copy-Directory.ps1"
    $DS_path_SkipBool = Dynamic-Path -AppendPath "\Search-SQLLogs\Search-SQLLogs.Function.Get-SkipBool.ps1"

    # Dynamic script paths

    $script_path_SearchSQLLogs = Dynamic-Path -AppendPath "\Search-SQLLogs\Search-SQLLogs.ps1"
    $script_path_FindCode = Dynamic-Path -AppendPath "\Search-SQLLogs\Search-SQLLogs.Function.Find-Code.ps1"

    Update-Profile -ScriptFullName $script_path_SearchSQLLogs

    $script_path = $script_path_FindCode
    $num_lines = 5
    $find_code_DS = @()
    $find_code_DS = '# Import LogContainer function',". $DS_path_LogContainer", "# Import Copy-Directory function",". $DS_path_CopyDirectory","# Import Get-SkipBool function",". $DS_path_SkipBool"

    Update-DotSource -ScriptPath $script_path -numLines $num_lines -updatedDSList $find_code_DS

    $script_path = $script_path_SearchSQLLogs
    $num_lines = 1
    $find_code_DS = @()
    $find_code_DS = "# Import Find-Code function",". $script_path_FindCode"

    Update-DotSource -ScriptPath $script_path -numLines $num_lines -updatedDSList $find_code_DS
    
    # Success Message
    Write-Host "`nProfile update complete.`n`nDot-source updates complete."

}
catch {
    Write-Host "`nProfile update failed."
}

# Exit Message
Read-Host "`nPress enter to exit..."
exit