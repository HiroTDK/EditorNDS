@Echo off&SetLocal EnableDelayedExpansion

Set magick="C:\Program Files\ImageMagick-7.0.8-Q16\magick.exe"

for %%f in (*.png) do (
    %magick% convert %%~nf.png -resize 16x16^ -background none -gravity center -extent 16x16 %%~nf.png
)