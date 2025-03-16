Simple support tool for Ragnarock (VR) map generation. IT IS NOT SEPARETE map editor, only some utility tool for Edda program https://pkbeam.github.io/Edda/getting-started

In Edda to craete map you need to press '1','2','3','4' in the rythm of song. As simple human being i was able to only focus to one thing - keeping up with rythm of song i was mapping, so i had no capacity to "design" interesting combinations on the go. So I thought about
making some tool to "randomize" combinations I was creating.

This tool outputs some random number or two (in case of two drums being hit at once) when user i pressing key 'Z' - you just need to listen music and tap Z in tempo/rythm.
Map needs to be reviewed after such creation, but I find it usefull especially for quick generation of maps with difficulties around 1-4.


todo:
-'X' button implementation; sometimes you want some parts to be more repeatable and You can even hear it - thats why combiantion 'Z' 'X' 'Z' 'X' 'Z' 'X' should somehow repeat same/similar patern without changing/updating sequence

-store DrumSequences in *.txt file to let user change it


Quick instruction how to use:

-go and find interesting music for example on YouTube
-with some online youtube dowloader get it in *.mp3 format
-with some online mp3 to ogg converter convert this to *.ogg music file

-in Edda program; https://pkbeam.github.io/Edda/getting-started, create new folder for map u will be creating, select new map, ogg file . Then go to some online bpm (beats per minute) checker and check bpm for your music and set this number in Edda.
-Run my RagnaRanda program and click start button, swtich back to Edda- press start and just start clicking just key 'Z" to match tempo of your music.
-And thats it- when song come to an end - just verify your map and remove "ugly looking/imposible" combinations if there are any.
-Save&Export it

https://www.ragnarock-game.com/faq

"Once you have custom songs, you just need to place them in the correct folder so that they are imported by the game:

On PC, navigate to your your default 'Documents' folder. Create a folder named 'Ragnarock' there and another folder named 'CustomSongs' inside (dont forget the upper case letters!). Place your custom songs folders inside of it. To sum up, the full path will look like [pathToDocumentsFolder]\Documents\Ragnarock\CustomSongs\myawesomecustomsong. OR Custom songs placed in the installation folder of Ragnarock, under 'Ragnarock/Ragnarock' in a folder 'CustomSongs' are also imported.
On Meta Quest/Pico, connect your headset to your computer by cable. Make sure you enable USB transfer to your device by validating the popup in your headset. Open up the Meta Quest/Pico folder on your computer's file explorer and navigate to Internal shared storage > Android > data > com.wanadev.ragnarockquest > files > UE4Game > Ragnarock > Ragnarock > Saved. Create a folder named 'CustomSongs' there and place your custom songs folders inside.
On PSVR2: Not available"
