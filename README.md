# TouchPlusCMDR
Touch+ Camera Application 

Update: 3/17/2015
Got a disparity map working via OpenCV! Getting closer to a working virtual mouse. Needs some form of PID error correction
in the hand model class, and some code to read the disparity map. Otherwise, some functions to move and click the mouse 
which shouldn't be too difficult.

---------------

Since Ractiv has left us high and dry, this is my personal attempt to get the device they shipped us all to do something
near what it was designed for. WORK IN PROGRESS. Please feel free to tinker and suggest anything, I really have no formal
knowledge in Computer Vision technologies so this is a learning experience.

Example screenshot: http://www.shadowmite.com/img/hulled2.png

This project is in C# 
Credit due: Ported over the ideas of how to unlock the camera from: https://github.com/umarniz/TouchPlusLib/tree/master/src

Some ongoing discussion where everyone should be able to register and comment is at: http://gharbi.me/ractiv/index.php

My running theory is to get the fingers mapped, and try to coorelate the fingers from left to right between the two video 
feeds. If they are about the same length, we match them and calculate z-depth.
