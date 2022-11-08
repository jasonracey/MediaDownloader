#!/bin/bash

echo "Removing App..."
rm -Rf /Applications/Media\ Downloader.app

echo "Building..."
rm -Rf ./MediaDownloaderUI/bin/Release/*
msbuild MediaDownloader.sln /property:Configuration=Release

echo "Deploying App..."
cp -Rf ./MediaDownloaderUI/bin/Release/Media\ Downloader.app /Applications/Media\ Downloader.app

echo "Done. See output for results."
