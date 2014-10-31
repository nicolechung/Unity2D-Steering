Pull requests welcome.

Version like...0.0.1. Not ready for use yet.

#Uses Layers
These scripts use layers to exclude the collider that the raycast originates from.

For some reason in Unity2D, if the object the raycast originates from (i.e. transform.position) has a collider, that collider is returned in the resulting RaycastHit2D.

I tried using various ways to exclude this (for example, comparing the *normal* of the RaycastHit2D with the _inverse of the direction_ of the Raycast) but it did not work for me.

To use layers

Choose a layer from the inspector for the objects you want to include as "obstacles":

![Use the dropdown](https://raw.githubusercontent.com/nicolechung/Unity2D-Steering/master/images/inspector-layers.png)

Find the number of the layer by going to:
```
Edit > Project Settings > Tags and Layers
```
![Look at the number](https://raw.githubusercontent.com/nicolechung/Unity2D-Steering/master/images/layer-8.png)


#Seeker
I have multiple colliders on my Seeker (Box2D and Circle);

Create your Seeker game object.

Add RCSeeker.cs script to the game object.

#Flee
In progress

Create the game object that needs to flee.

Add RCFlee.cs script to the game object.

Set the layer number in the LAYER_MASK inside the script. 

For example, if all of your obstacles are on layer 8:

```c#
private static int LAYER_MASK = 8; // make sure your player isn't on this list!
```

#Wander

Create your Wanderer game object.

Add RCWanderer.cs script to the game object.

Set the layer number in the LAYER_MASK inside the script. 

For example, if all of your obstacles are on layer 8:

```c#
private static int LAYER_MASK = 8; // make sure your player isn't on this list!
```

## To do list for wanderer
Build "Lock to Camera view" so that the player sticks to the camera viewport

#Tweakables

##collisionDistance
This is the distance the object "looks ahead" for obstacles

##Target
This is the target (Game object) that the game object is either seeking or fleeing.

##DEBUG, DEBUG_DRAW, DEBUG_ERROR
You can turn these on or off.

DEBUG_DRAW shows the lines (rays) of the raycast.

DEBUG_ERROR will function as breakpoints if you select "Error Pause" in the Unity player...since Monodevelop has such a broken Unity debugger feature (for Macs at least)
