# PermissionsNodeGenerator
DISCLAIMER: Source generators are in pre-release and only work with .NET Standard/.NET Core at the moment. However, Terraria extensions and plugins work in .NET Framework. I do not recommend going through the trouble of enabling it on .NET Framework and other .NET platforms unless you really need it

PermissionsNodeGenerator is a source generator dedicated to generating strongly-typed permission classes

![GitHub license](https://img.shields.io/github/license/Arthri/PermissionsNodeGenerator?style=flat-square) ![GitHub release (latest SemVer)](https://img.shields.io/github/v/release/Arthri/PermissionsNodeGenerator?sort=semver&style=flat-square) ![GitHub release (latest SemVer including pre-releases)](https://img.shields.io/github/v/release/Arthri/PermissionsNodeGenerator?include_prereleases&sort=semver&style=flat-square)

## Installation
TODO

## Usage
Create a new file with the extension of `.ptxt`. The file name will be the permissions class's name

An example ptxt looks like this:
```
TShock
 Ignore
  SSC
Inventorer
 Delete
  Force
```

Assuming the file is named `Permissions.ptxt`, the permissions can be referenced like so:
- `GeneratedPermissions.Permissions.TShock.Ignore.SSC` -> `tshock.ignore.ssc`
- `GeneratedPermissions.Permissions.Inventorer.Delete.Force` -> `inventorer.delete.force`