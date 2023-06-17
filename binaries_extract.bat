@echo off 

setlocal EnableDelayedExpansion


set "targetPaths[0]=%1python\"
set "targetPaths[1]=%1models\easyocr\"
set "targetPaths[2]=%1models\tessdata\"
set "targetPaths[3]=%1models\prediction\"

set "inputBinariesPaths[0]=%~dp0etc\python\Python_38.zip"
set "inputBinariesPaths[1]=%~dp0etc\models\easyocr.zip"
set "inputBinariesPaths[2]=%~dp0etc\models\tesseract.zip"
set "inputBinariesPaths[3]=%~dp0etc\models\prediction.zip"

for %%i in (0,1,2,3) do (
	if NOT exist !targetPaths[%%i]! (
		powershell Expand-Archive !inputBinariesPaths[%%i]! -DestinationPath !targetPaths[%%i]!
	)
)