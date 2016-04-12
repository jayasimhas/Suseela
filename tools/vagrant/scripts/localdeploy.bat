
c:\vagrant_root\nant init -D:env=local
"C:\Program Files (x86)\MSBuild\14.0\Bin\msbuild" c:\vagrant_root\informa.sln /property:Configuration=Debug-NoTds

robocopy "c:\vagrant_website" "c:\inetpub\wwwroot\Informa\Website" /E /XD "c:\vagrant_website\node_modules"

exit 0