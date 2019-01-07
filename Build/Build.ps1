Set-PSDebug -Trace 1

$dockerVolumesPath = (Get-ChildItem Env:DOCKER_VOLUMES_PATH).Value
$srcPath = Join-Path $dockerVolumesPath 'src'
cp -r . $srcPath

docker run --rm -v "$($srcPath):/var/src" microsoft/dotnet:2.1.500-sdk-stretch dotnet publish /var/src -c Release -f netcoreapp2.1 -o /var/src/published 2>&1
if($LASTEXITCODE){
    Exit $LASTEXITCODE
}

mv "$srcPath/published" .