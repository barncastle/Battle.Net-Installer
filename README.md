# Battle.Net Installer

A command line tool for installing games via Blizzard's Battle.Net application. Windows only. See releases for a compiled binary.

#### Project Prerequisites
- [.Net Core 3.1](https://dotnet.microsoft.com/download/dotnet)
- [Battle.net](https://www.blizzard.com/en-us/apps/battle.net/desktop) must be installed, up to date and have been recently signed in to.

#### Arguments

| Argument | Description |
| ------- | :---- |
| --prod | TACT Product **(Required)** |
| --lang | Game/Asset language **(Required)** |
| --dir | Installation Directory **(Required)** |
| --uid | Agent UID (Required if different to the TACT product) |
| --repair | Repairs the installation opposed to installing/updating it |
| --help | Shows this table |

All products and Agent UIDs can be found [here](https://wowdev.wiki/TACT#Products) however only (green) Active products will work.  
Languages are listed [here](BNetInstaller/Constants/Locale.cs), availability will vary between products.

#### Usage

You will need to run this from either command line or PowerShell. To do this:

1. Navigate to the folder that `bnetinstaller.exe` is located.
2. Hold shift and right click on the folder.
3. Click "Open PowerShell window here" or "Open command window here".
4. Enter `.\bnetinstaller.exe` followed by the above arguments.

For example for StarCraft 2, which has a TACT Product of `s2` and an Agent UID of `s2(_locale)`):  

`.\bnetinstaller.exe --prod s2 --uid s2_enus --lang enus --dir "C:\Test"`

**Note:** *If run against an existing installation the installer will attempt to update the product.*