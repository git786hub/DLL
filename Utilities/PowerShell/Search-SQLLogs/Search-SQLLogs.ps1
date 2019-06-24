# Import Find-Code function
. C:\Users\Josh.Mascorro.INTERNAL\Documents\Oncor\test_dgs_aegis\Utilities\Search-SQLLogs\Search-SQLLogs.Function.Find-Code.ps1

# Run all functions and script commands
Function Search-SQLLogs($InputDirectory, $SQLcode, $ExportPath) {
    # Set variable to log data and execute function
    $log = Find-Code -Directory $InputDirectory -Code $SQLcode
    $export = @()
    # Show Log 
    $export += $log.ShowHeader()
    # Show removed commits log
    $export += $log.LogObject1.Show()
    # Show skipped files log
    $export += $log.LogObject2.Show()
    # Show missed commits log
    $export += $log.LogObject3.Show()
    $export
    $export | Out-File -FilePath $ExportPath
}
