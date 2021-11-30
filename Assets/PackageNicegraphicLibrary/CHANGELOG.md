# Changelog

## 1.0.3

### Bugfix

* Fixed issue if unity projects were build with this package. Reason: all C# files to implement the changes cause by custom properties 
used UnityEngine.Editor namespace and those files were in assembly not only run in editor. Moved those files into own assembly only run in editor.

## 1.0.2

### Bugfix

* Changed Directory Separators from '/' to Path.DirectorySeparators for cross over os compatibility.

## 1.0.1

### Bugfix

* Fixed compiler error for CS0106 - modifier "public"is not valid for this item in members of instances

