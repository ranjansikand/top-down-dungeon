# top-down-dungeon

This is the source code for a game I was working on.

As a mage, you can traverse a small dungeon and avoid traps to encounter the boss in the final room. There is a small puzzle to solve along the way, and hints scattered
throughout the level indicate the horrors that trangressed there prior to your arrival.

https://www.youtube.com/watch?v=HnT8rb_Tdro&t=2s

The game was planned to be a rougelite, and you would take up the journey as a new hero with new powers and a new model. This repository contains code for 5 total characters.
It was a great learning opportunity, and I have taken many things from this project into my next.

Some issues include the enemy AI, which was overly simplistic, and the character controller, which uses Unity's basic input system and a simple control method. The AI lacked a state system, so often became cumbersome to navigate, particularly for the two implemented bosses. In addition, the inventory
is a bit cumbersome and difficult to navigate.

My current project rectifies these issues with an interactive inventory with an adaptive UI and a hierarchal state machine to control character movement. I also utilized Unity's new Input System, which allows support for a variety of input devices. I've tested with KB+M as well as a DualShock 5 controller.
