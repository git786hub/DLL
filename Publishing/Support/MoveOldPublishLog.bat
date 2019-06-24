REM rename previous publish log if it exists
IF exist %temp%\publish.log (
  ren %temp%\publish.log "prev_publish_archived_on_%LongDateTime%.log"
) 