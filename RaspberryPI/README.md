##This is the section for python script used to relay sensor data to the Hololens 2##

**Setting up PI:**

sudo apt-get update

sudo apt install python3-pip

**Wiring Diagram:**

https://learn.adafruit.com/adafruit-ultimate-gps-on-the-raspberry-pi/using-uart-instead-of-usb

**Install GPS daemon (gpsd)**

sudo apt-get install gpsd gpsd-clients

**Disable default gpsd**

sudo systemctl stop gpsd.socket

sudo systemctl disable gpsd.socket

***Run these before gps data can be received on UART***

sudo killall gpsd

sudo gpsd /dev/serial0 -F /var/run/gpsd.sock

***If using usb version:***

sudo gpsd /dev/ttyUSB0 -F /var/run/gpsd.sock

**Command to use script:**

python3 gps_server_tcp.py

git archive --remote=ssh://github.com/rmhowe425/NASA-SUITS-2021.git HEAD README.md

