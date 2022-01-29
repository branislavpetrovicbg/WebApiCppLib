FROM mcr.microsoft.com/windows/servercore:ltsc2019
RUN powershell Invoke-webrequest http://nginx.org/download/nginx-1.20.2.zip -UseBasicParsing -outfile c:\\nginx.zip
RUN powershell Expand-Archive c:\\nginx.zip -Dest c:\\nginx
WORKDIR c:\\nginx\\nginx-1.20.2
ENTRYPOINT powershell .\\nginx.exe