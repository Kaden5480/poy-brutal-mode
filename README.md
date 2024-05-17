# poy-brutal-mode
![Code size](https://img.shields.io/github/languages/code-size/Kaden5480/poy-brutal-mode?color=5c85d6)
![Open issues](https://img.shields.io/github/issues/Kaden5480/poy-brutal-mode?color=d65c5c)
![License](https://img.shields.io/github/license/Kaden5480/poy-brutal-mode?color=a35cd6)

A challenging
[Peaks of Yore](https://store.steampowered.com/app/2236070/)
mod.

# Overview
- [Features](#features)
- [Installing](#installing)
    - [BepInEx](#bepinex)
    - [MelonLoader (Windows)](#melonloader-windows)
    - [MelonLoader (Linux)](#melonloader-linux)
- [Building from source](#building)
    - [Dotnet](#dotnet-build)
    - [Visual Studio](#visual-studio-build)
    - [Custom game locations](#custom-game-locations)

# Features
This mod introduces some tweaks to the vanilla game mechanics which
can make the gameplay significantly more challenging.

The following tweaks are currently applied:
- Gravity is 50% stronger
- Crimp stamina drains 2x faster
- Crumbling holds disappear 3x faster
- Pitch hold stamina drains 4x faster
- Ice pick stamina drains 4x faster
- Ice picks have 10% more force to be more usable with the increased gravity
- You slide down slopers significantly faster

# Installing
## BepInEx
### Installing BepInEx
- Download the latest stable win_x64 version of BepInEx
[here](https://github.com/BepInEx/BepInEx/releases).
- Find the Peaks of Yore game directory, this is most easily done by going to the game in steam,
  pressing the settings for the game (⚙️), selecting "Manage", then "Browse local files".
- Extract the contents of `BepInEx_win_x64_<version>.zip` into your Peaks of Yore game directory.
- You should now have files/directories such as `BepInEx` and `winhttp.dll`
  in the same place as `Peaks of Yore.exe` and `UnityPlayer.dll`.
- Start the game so BepInEx can generate other necessary files for modding.
- Close the game.

### Installing brutal mode
- Download the latest BepInEx release
[here](https://github.com/Kaden5480/poy-brutal-mode/releases).
- The compressed zip will contain a `patchers` and `plugins` directory.
- Copy the files in `patchers` to `BepInEx/patchers` in your game directory.
- Copy the files in `plugins` to `BepInEx/plugins` in your game directory.
- If installed correctly, you can now start the game and "enjoy" brutal mode.

## MelonLoader (Windows)
### Prerequisites
- Install Microsoft Visual C++ 2015-2022 Redistributable from
[this link](https://aka.ms/vs/17/release/vc_redist.x64.exe)
or by running `winget install Microsoft.VCRedist.2015+.x64` in cmd/powershell/terminal.
- Install Microsoft .NET Desktop Runtime 6 from
[this link](https://download.visualstudio.microsoft.com/download/pr/d0849e66-227d-40f7-8f7b-c3f7dfe51f43/37f8a04ab7ff94db7f20d3c598dc4d74/windowsdesktop-runtime-6.0.29-win-x64.exe)
or by running `winget install Microsoft.DotNet.DesktopRuntime.6` in cmd/powershell/terminal.

### MelonLoader
- Download the latest nightly MelonLoader build
[here](https://nightly.link/LavaGang/MelonLoader/workflows/build/alpha-development/MelonLoader.Windows.x64.CI.Release.zip).
- Find the Peaks of Yore game directory, this is most easily done by going to the game in steam,
  pressing the settings for the game (⚙️), selecting "Manage", then "Browse local files".
- Extract the contents of the downloaded zip file into your game directory.
- Run Peaks of Yore and then quit the game.
- If MelonLoader was installed correctly, you should notice new directories
  in your game directory (such as Mods).

### Brutal mode
- Download the latest release
[here](https://github.com/Kaden5480/poy-brutal-mode/releases).
- The compressed zip file will contain a `Mods` and `Plugins` directory.
- Copy the files from `Mods` to `Mods` in your game directory.
- Copy the files from `Plugins` to `Plugins` in your game directory.
- Copy the files from `UserLibs` to `UserLibs` in your game directory.
- If installed correctly, you can now start the game and "enjoy" brutal mode.

## MelonLoader (Linux)
### Prerequisites
- Install [protontricks](https://pkgs.org/download/protontricks).

### Prefix configuration
- Open protontricks.
- Select "Peaks of Yore".
- Select "Select the default wineprefix" and press "OK".
- Select "Run winecfg" and press "OK".
- Change "Windows Version" to "Windows 10" and press "Apply".
- Switch to the "Libraries" tab.
- Where it says "New override for library:", choose "version", press "Add", then press "OK".

### Installing prefix components
- Open protontricks.
- Select "Peaks of Yore".
- Select "Select the default wineprefix" and press "OK".
- Select "Install Windows DLL or component" and press "OK".
- Select the packages "dotnetdesktop5" and "vcrun2019" and press "OK".
- You may get errors that say checksums didn't match, you can ignore these. When
  you are asked to "Continue anyway", choose "Yes".

### MelonLoader
- Download the latest nightly MelonLoader build
[here](https://nightly.link/LavaGang/MelonLoader/workflows/build/alpha-development/MelonLoader.Windows.x64.CI.Release.zip).
- Find the Peaks of Yore game directory, this is most easily done by going to the game in steam,
  pressing the settings for the game (⚙️), selecting "Manage", then "Browse local files".
- Extract the contents of the downloaded zip file into your game directory.
- Run Peaks of Yore and then quit the game.
- If MelonLoader was installed correctly, you should notice new directories
  in your game directory (such as Mods).

### Brutal mode
- Download the latest release
[here](https://github.com/Kaden5480/poy-brutal-mode/releases).
- The compressed zip file will contain a `Mods` and `Plugins` directory.
- Copy the files from `Mods` to `Mods` in your game directory.
- Copy the files from `Plugins` to `Plugins` in your game directory.
- Copy the files from `UserLibs` to `UserLibs` in your game directory.
- If installed correctly, you can now start the game and "enjoy" brutal mode.

# Building from source
Whichever approach you use for building from source, the resulting
patcher and plugin can be found in `bin/`.

The following configurations are supported:
- Debug-BepInEx
- Release-BepInEx
- Debug-MelonLoader
- Release-MelonLoader

## Dotnet build
To build with dotnet, run the following command, replacing
<configuration> with the desired value:
```sh
dotnet build -c <configuration>
```

## Visual Studio build
To build with Visual Studio, open BrutalMode.sln and build by pressing ctrl + shift + b,
or by selecting Build -> Build Solution.

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
