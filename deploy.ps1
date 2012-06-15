$ErrorActionPreference = 'Stop'

$hostname = "ec2-174-129-225-146.compute-1.amazonaws.com"
$username = "Administrator"
$password = ConvertTo-SecureString "gHg&nci87)i" -AsPlainText -Force
$credentials = New-Object System.Management.Automation.PSCredential ($username, $password)


Invoke-Command -Computer $hostname -Credential $credentials -ScriptBlock { hostname }	