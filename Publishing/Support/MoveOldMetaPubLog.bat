@REM rename previous publish log if it exists
IF exist %temp%\mpublish%Config%.log (
  ren %temp%\mpublish%Config%.log prev_mpublish%Config%_arch_on_%LongDateTime%.log
)