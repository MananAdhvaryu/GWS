# GWS (Graphics and Windowing System)
GWS is a C# 2D graphics library created as part of the infrastructure for M&M Info-tech Ltd's Control Technologies. As such it is actively developed and documented by M&M Info-tech Ltd.

![Polygon rotation gif](https://media.giphy.com/media/QvwCkz2JDoQqt0WfGp/giphy.gif)

This live screen capture created by changing image properties in the demo by hand.

<h1>Feature List:</h1>

<ul>
<li>C# code.
<li>Test environment.
<li>Extensively Modular Design
<li>More gradient choices.
<li>More filling options (Flood fill & Poly Fill)
<li>More stroking options
<li>More stroking display options
<li>Built-in double/triple buffering (Pro)
<li>~50% faster than existing libraries in the market (Pro)
<li>Font rotation.
<li>Individual shape rotation.
</ul>

## Index

[Installation](#Installation)

[The MnM Story](#The-MnM-Story)

[High Level Functionality](#High-level-Functionality)

[Key Concepts](#Key-Concepts)

[Code Examples](#Code-Examples)


## Installation
<ul>
<li>Import the projects from the repo to your code.
<li>Add references to them in your desired project.
<li>Have fun!
</ul>

## The MnM Story

Have you ever noticed that your software compile takes up a lot of space and does not perform the way you hoped? We did. 
So we started changing it by replacing the 'good enough' infrastructure with better infrastructure. 
M&M's goal is a faster, smaller infrastructure that scales better than the 'good enough' components we have now. 

## High level Functionality
<b>GWS supports draw and fill for:</b>

<ul>
Arbitrary Polygons,
 Triangle,
 Square,
 Rectangle,
 Rounded Rectangle,
 Rhombus,
 Trapezium,
 Circle,
 Ellipse,
 Pie,
 Arcs and
 Lines.
</ul>

With optional orientation by translation and rotation.




<b>It supports filling using a palette of colours and at orientations like:</b>

* Horizontal
* Forward/Backward diagonal
* Vertical
* Central: Horizontal, Vertical, Diagonal
* Switched direction.

You can choose the shape to fill as well as the stroke.

Stroke can be arranged in relation to the shape boundary as:

* Inside
* Central
* Outside


## Key Concepts

<b>Axis</b>

The folowing parameters appear a lot in the functions:
start, end, axis and horizontal
These define an axis where 'horizontal' determines if the axis is horizontal (True) or vertical (False). The value 'axis'' is the position of the axis on the axis perpendicular to it. 'start' is the beginning of the range of used values which extends to 'end'.
Axis defines a reference lines for placing rectangles.

![Explanation of axis](https://i.imgur.com/M6jLYi1.png)

<b>Rectangle</b>

Drawing occurs in rectanglular spaces represented by memory which can be buffer memory or screen memory or something else.

<b>Path</b>

A Path is a linked list structure used to keep track of rectangles and their relationship to each other. If you redraw the image then you go through the path in sequence drawing each rectangle stored/parameterised. 

## Code Examples

You can use the provided demo to draw any shape in the library as per your parameters to see its functionality. You can also look at the code and understand from it how it was implemented.
