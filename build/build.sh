#!/usr/bin/env bash

set -xe

cd ../

VERSION="$(git describe --abbrev=0 | tr -d  "v")"
BP_NAME="BrutalMode-$VERSION-BepInEx"
ML_NAME="BrutalMode-$VERSION-MelonLoader"
BP_DIR="build/$BP_NAME"
ML_DIR="build/$ML_NAME"


dotnet build -c Release-BepInEx
dotnet build -c Release-MelonLoader

mkdir -p "$BP_DIR"/{patchers,plugins}
mkdir -p "$ML_DIR"/{Mods,Plugins}

# BepInEx
cp bin/patcher/release-bepinex/net472/*.dll \
    "$BP_DIR/patchers/"
cp bin/plugin/release-bepinex/net472/*.dll \
    "$BP_DIR/plugins/"
cp build/README-BepInEx.txt "$BP_DIR/README.txt"

# MelonLoader
cp bin/plugin/release-melonloader/net472/*.dll \
    "$ML_DIR/Mods/"
cp bin/patcher/release-melonloader/net472/*.dll \
    "$ML_DIR/Plugins/"
cp build/README-MelonLoader.txt "$ML_DIR/README.txt"

# Zip everything
pushd "$BP_DIR"
zip -r ../"$BP_NAME.zip" .
popd

pushd "$ML_DIR"
zip -r ../"$ML_NAME.zip" .
popd

# Remove directories
rm -rf "$BP_DIR" "$ML_DIR"
