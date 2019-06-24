
class Log {

    [String]
    $Name

    [Array]
    $DataList

    [String]
    $DataString

    [Int]
    $Count

    [string]Show()
    #Show($exportList)
    {
        $logName = $this.Name
        $logCount = $this.Count
        $logData = $this.DataList
        $logData = $logData -join "`n`t`t`t`t`t`t`t"
        Return "`n`t$logName`n`n`tTotal Found:`t$logCount`n`t`t`t`t`t`t`t$logData"
        
    }
}

class LogContainer {
    
    [Log]
    $LogObject1

    [Log]
    $LogObject2

    [Log]
    $LogObject3

    [Log]
    $LogObject4

    [Log]
    $LogObject5

    [string]ShowHeader() 
    {
        Return "`nLOG RESULTS:`n`n"
    }
}