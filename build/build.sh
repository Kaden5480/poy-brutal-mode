#!/usr/bin/env bash

set -xe

cd ../

VERSION="$(git describe --tags | tr -d  "v")"
BP_DIR="build/BrutalMode-$VERSION-BepInEx"
ML_DIR="build/BrutalMode-$VERSION-MelonLoader"


dotnet build -c Release-BepInEx
dotnet build -c Release-MelonLoader

mkdir -p "$BP_DIR"/{patchers,plugins}
mkdir -p "$ML_DIR"/{Mods,Plugins,UserLibs}

# BepInEx
cp bin/patcher/release-bepinex/net472/*.dll \
    "$BP_DIR/patchers/"
cp bin/plugin/release-bepinex/net472/*.dll \
    "$BP_DIR/plugins/"

# MelonLoader
cp bin/plugin/release-melonloader/net472/*.dll \
    "$ML_DIR/Mods/"
cp bin/patcher/release-melonloader/net472/*.dll \
    "$ML_DIR/Plugins/"
cp bin/patcher/release-melonloader/net472/libs/{Mono.Cecil,MonoMod.Utils}.dll \
    "$ML_DIR/UserLibs/"
chmod -x "$ML_DIR"/UserLibs/*.dll

# Zip everything
cd build/
zip -r "BrutalMode-$VERSION-BepInEx.zip" "BrutalMode-$VERSION-BepInEx"
zip -r "BrutalMode-$VERSION-MelonLoader.zip" "BrutalMode-$VERSION-MelonLoader"
