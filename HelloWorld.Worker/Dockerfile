# Use a Windows Server Core base image with .NET Framework support
FROM mcr.microsoft.com/dotnet/framework/runtime:4.8-windowsservercore-ltsc2022

# Set working directory inside the container
WORKDIR C:/worker/

# Copy all service files into the image
COPY ./bin/Release/ .

# Install the service using sc.exe
# Then start it using powershell Start-Service
ENTRYPOINT powershell -Command " \
  New-Service -Name 'HelloWorkerService' -BinaryPathName 'C:\\worker\\HelloWorld.Worker.exe' -DisplayName 'HelloWorkerService' -StartupType Manual; \
  Start-Service -Name 'HelloWorkerService'; \
  Start-Sleep -Seconds 10; \
  $proc = Get-Process 'HelloWorld.Worker' -ErrorAction SilentlyContinue; \
  if ($proc) { Wait-Process -Id $proc.Id } else { Write-Host 'Process not found. Exiting.' }"

