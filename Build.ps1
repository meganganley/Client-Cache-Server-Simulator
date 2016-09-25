function Clean-Solution ($thingToClean, $configuration) {
    set-alias msb "C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe"
    &  msb $thingToClean /t:Clean /p:Configuration=$configuration /m /nologo /p:VisualStudioVersion="14.0" /p:Platform="Any CPU"
}

function Build-Solution ($thingToBuild, $configuration) {
    set-alias msb "C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe"
    & msb /nologo $thingToBuild /p:Configuration=$configuration /m /p:VisualStudioVersion="14.0" /p:Platform="Any CPU"
}

$baseDir = Resolve-Path .
$configuration = "Release"
$solutions = @("Cache","Client","Server")

foreach ($solutionName in $solutions) {
    $solutionDir = Join-Path -Path $baseDir -ChildPath "$solutionName" 
    Clean-Solution "$solutionDir\$solutionName.sln" $configuration
    Build-Solution "$solutionDir\$solutionName.sln" $configuration
    $execFile = ""
    if($solutionName -eq "Cache"){
        $execFile = Join-Path -Path $solutionDir -ChildPath "Cache.GUI\bin\$configuration\Cache.GUI.exe"
        
    }elseif($solutionName -eq "Client"){
        $execFile = Join-Path -Path $solutionDir -ChildPath "Client.GUI\bin\$configuration\Client.GUI.exe"
    }
    else{
        $execFile = Join-Path -Path $solutionDir -ChildPath "Server.ConsoleApp\bin\$configuration\Server.ConsoleApp.exe"
    }
    & $execFile
}