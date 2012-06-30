$ErrorActionPreference = 'Stop'

$hostname = "ec2-174-129-225-146.compute-1.amazonaws.com"

Invoke-Command -Computer $hostname -ScriptBlock { hostname }	