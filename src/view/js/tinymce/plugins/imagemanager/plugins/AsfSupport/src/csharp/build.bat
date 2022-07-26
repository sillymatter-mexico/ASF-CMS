@echo off
set path=%path%;%windir%\Microsoft.NET\Framework\v2.0.50727
csc /nologo /reference:D:\proyectos\einsNull\svn\cmsDemo\bin\cmsDemo.dll /target:library /out:..\..\bin\AsfSupportPlugin.dll /reference:D:\proyectos\einsNull\svn\cmsDemo\bin\MCManager.dll *.cs
pause
