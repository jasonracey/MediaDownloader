#!/bin/bash

# deploy app
echo "Deploying App..."
rm -Rf /Applications/Media\ Downloader.app
cp -Rf ./MediaDownloaderUI/bin/Release/Media\ Downloader.app /Applications/Media\ Downloader.app

echo "Done!"
