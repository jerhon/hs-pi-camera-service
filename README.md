# Overview

This runs a .NET application which takes pictures via the Raspberry Pi camera and serves them via a WebApi.

IMPORTANT: This DOES NOT have any authentication on it, so do not put it on a public network.

## Prerequisites

* Camera enabled via rasp-config 
  * `sudo raspi-config`
  * Should be the option under interfaces
* Setup .NET Core 5 on the raspberry pi for the "pi" linux user
* Build and transfer the build output to the raspberry pi
* Optionally, install as a service.  (Check the paths in camera.service and modify as appropriate)
  * Here's a good tutorial on that: https://learn.sparkfun.com/tutorials/how-to-run-a-raspberry-pi-program-on-startup#method-3-systemd
  * I included a sample .service file that can go in /etc/systemd/system/multi-user.target.wants/ on Raspbian assuming everything is installed in the same paths
  * I suggest try to run it by itself first to make sure everything is set up right, then later run it as a service.  Make sure to check with journalctl to ensure the service starts up right.
  
## Obtaining the Image

Visit http://pi-ip-address:5000/camera to see the latest snapshot from the camera.

The interval at which it takes pictures is defined via the application settings.
Take a look in there if you wish to modify.

Camera
* Interval - The interval between picture snaps.
* Timeout - Amount of time before a picture is considered timed out.
* Width - The width of the captured image. 0 = Maximum Width
* Height - The height of the captured image. 0 = Maximum Height

## TODO

* Setup is a bit manual, should make it easier to install.
* Would be nice to run in a docker container, but the Pi library I'm using to perform the camera capture is calling raspberry pi camera commandline app to get the captures. 