# poy-brutal-mode
![Code size](https://img.shields.io/github/languages/code-size/Kaden5480/poy-brutal-mode?color=5c85d6)
![Open issues](https://img.shields.io/github/issues/Kaden5480/poy-brutal-mode?color=d65c5c)
![License](https://img.shields.io/github/license/Kaden5480/poy-brutal-mode?color=a35cd6)

A challenging
[Peaks of Yore](https://store.steampowered.com/app/2236070/)
mod.

# Overview
- [Installing](#installing)
    - [BepInEx](#bepinex)
    - [Brutal mode](#brutal-mode)
- [Building from source](#building)
    - [Dotnet](#dotnet-build)
    - [Visual Studio](#visual-studio-build)
    - [Custom game locations](#custom-game-locations)

# Installing
## BepInEx
- Download the latest stable win_x64 version of BepInEx
[here](https://github.com/BepInEx/BepInEx/releases).
- Find the Peaks of Yore game directory, this is most easily done by going to the game in steam,
  pressing the settings for the game (⚙️), selecting "Manage", then "Browse local files".
- Extract the contents of BepInEx_win_x64_<version>.zip into your Peaks of Yore game directory.
- You should now have "BepInEx", ".doorstop_version", "changelog.txt", "doorstop_config.ini", and "winhttp.dll"
  in the same place as "Peaks of Yore_Data", "MonoBleedingEdge", "Peaks of Yore.exe", and "UnityPlayer.dll"
- Start the game so BepInEx can generate other necessary files for modding.
- Close the game.

## Brutal mode
- Download the latest release
[here](https://github.com/Kaden5480/poy-brutal-mode/releases).
- The compressed zip will contain a "patchers" and "plugins" directory.
- Copy the files in "patchers" to "BepInEx/patchers" in your game directory.
- Copy the files in "plugins" to "BepInEx/plugins" in your game directory.
- If installed correctly, you can now start the game and "enjoy" brutal mode.


# Building from source
Whichever approach you use for building from source, the resulting
patcher and plugin can be found in bin/

## Dotnet build
To build with dotnet, run the following command:
```sh
dotnet build
```

## Visual Studio build
To build with Visual Studio, open BrutalMode.sln and build by pressing ctrl + shift + b,
or by selecting Build -> Build Solution

## Custom game locations
> [!NOTE]
> In the current version of brutal mode, this isn't used, so can be safely ignored.

If you installed Peaks of Yore in a custom game location, you may require
an extra file to configure the build so it knows where to find the Peaks of Yore game
libraries.

The file must be in the root of this repository and must be called "GamePath.props".

Below gives an example where Peaks of Yore is installed on the F drive:
```xml
<Project>
  <PropertyGroup>
    <GamePath>F:\Games\Peaks of Yore</GamePath>
  </PropertyGroup>
</Project>
```
