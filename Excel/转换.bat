@echo off
@del data\design.db
@echo convert excel to sqlite db
tools\excel2sqlite\excel2sqlite-windows -c config-excel.xml -j config-json.xml -f 0
@echo convert excel encode pretty
tools\excel2sqlite\excel2sqlite-windows -j config-json-pretty.xml -f 0
@echo encrypt json data
tools\zip_encrypt\zip_encrypt  zip_encrypt_tool.xml
::tools\StringReplace\StringReplace
@echo move file
copy data\design.json  ..\Assets\Resources\ExcelCfg\design.json
pause
