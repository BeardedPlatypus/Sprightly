<p align='center'><img align='center' src='https://raw.githubusercontent.com/BeardedPlatypus/Sprightly/c97e699823d44fe857f54f6561129bbfe8be9695/readme/sprightly_icon.svg' width='25%'></p>

*This is a work-in-progress and will be updated further soon*

# Sprightly - Organising Sprites

Sprightly is a small Fabulous for Xamarin.Forms application to define sprite
sheets, sprites, and sprite animations and export the metadata as a json file,
so it can be consumed in another application. It currently only targets the
WPF back-end.

# Motivation

I wanted to have a small application that let me define sprites and sprite 
animations on image file, and then let me export these definitions to a 
simple human-readable format. These exported definitions I could then use
to load in SDL2 applications. In order to ensure consistency between this 
application, and a SDL2 application, which might use them, Sprightly uses 
SDL2 to render any and all textures, sprites, and sprite animations. 

I chose hte current technology stack because I wanted to gain a better 
understanding of F#, Fabulous, and Xamarin Forms. As such, this application
is meant as a proof of concept / prototype and not a full-fledged production
ready application. In order to allow me to maximise my time learning, I opted
not to write any unit tests at this time, though they might be added in the 
future.

# Gallery

<p align='center'><img align='center' src='https://github.com/BeardedPlatypus/Sprightly/blob/master/readme/StartPage.png?raw=true' width='50%'></p>
Start page with a recent projects list and the option to open an existing 
project or create a new one.  

<p align='center'><img align='center' src='https://github.com/BeardedPlatypus/Sprightly/blob/master/readme/NewProjectPage.png?raw=true' width='50%'></p>
New project page to set up an initial sprightly project.

# Theme

The theme is strongly based upon (read: practically copied) from the 
[Material Design Rally Demo](https://material.io/design/material-studies/rally.html).
UX / UI Design is not one of my strong suits, and not a goal in and of itself
for this project, as such I opted for copying a well thought out design, 
instead of creating a worse UX / UI from scratch.