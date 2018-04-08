$version = $env:BUILD_VERSION + $env:APPCENTER_BUILD_ID;
if($env:APPCENTER_BRANCH -ne "master")
{
	$version = $version + "-alpha";
}
$path = ".\nuspec\Sannel.House.Configuration.nuspec"
[xml]$xml = Get-Content $path
$xml.package.metadata.version = $version
$xml.Save($path);

nuget pack $path