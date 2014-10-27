Pull requests welcome.

Version like...0.0.1. Not ready for use yet.

#Uses Layers
These scripts use layers to exclude the collider that the raycast originates from.

For some reason in Unity2D, if the object the raycast originates from (i.e. transform.position) has a collider, that collider is returned in the resulting RaycastHit2D.

I tried using various ways to exclude this (for example, comparing the normal of the RaycastHit2D with the inverse of the direction of the Raycast) but it did not work for me.

To use layers

Choose a layer from the inspector for the objects you want to include as "obstacles":

![Use the dropdown](https://raw.githubusercontent.com/nicolechung/Unity2D-Steering/master/images/inspector-layers.png)

Find the number of the layer by going to:
```
Edit > Project Settings > Tags and Layers
```
![Look at the number](https://raw.githubusercontent.com/nicolechung/Unity2D-Steering/master/images/layer-8.png)


#Seeker


#Wander
