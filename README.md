# AudioManager
*An audio management interface for the Unity game engine.*

Audio in Unity can be hard -- but it doesn't have to be! Over the years, I've had to implement audio 
in Unity to get my sound effects and music all sounding just right. Over that time, I've learned a thing
or two and developed my own system for managing audio. Now, I present that system to you. 

This system builds on the work of others. A huge inspiration for the structure of this code comes from
the wonderful Brackeys. I tinkered with a few systems for managing audio, and his was the best. I've
modified and built on the original structure he laid out to create this tool. Find his original Youtube 
video on a similar audio management tool to this one at this link here: https://www.youtube.com/watch?v=6OT43pvUyfY 

What you will find in this repo:

`Sound.cs`: A simple class to help us represent sounds in Unity. Used by the `AudioManager` class.

`AudioManager.cs`: This is the component class which you will drag into your Unity scene. Takes in
                     many mixers and audio files through the Unity inspector, and can then be called
                     upon to play, pause, and stop those sounds at any time.
                     
`SetVolume.cs`: Auxilliary script that adds some functionality with regards to adjusting sounds in
                  the AudioManager.

**Requirements**
* Unity engine on your computer
* Basic understanding of Unity mixers/scripting
* That's it!

**How to Use this Component**
* Download repo
* Copy files into your Scripts directory in any Unity project
* Create an `AudioManager` instance on an empty `GameObject`
* Increase size of `sounds` array, and drag and drop your audio clips in (any Unity-supported file format will work!)
* Name each clip, and assign any other optional values you desire
* From any script, call `instance.Play(soundname)` where instance is your `AudioManager` instance and `soundname` is the string you named a particular `Sound`
* You should hear your sound in Unity!

That should be enough to get you started. Check out the source code to explore the other functionalities
of this system -- I've gone to lengths to make them readable and self-documenting as much as possible.
You can reach me with any bugs, questions, or concerns at maxwell.mitchell@tufts.edu

Feel free to fork this repo and make it your own!

