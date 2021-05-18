/*
  This example code will walk you through how to set the interrupts on the
  SparkFun VEML6030 Ambient Light Sensor. You can set both high and low
  thresholds as well as how many values must be below or above the threshold
  before an interrupt occurs. This example does not require the interrupt pin on
  the product to be connected a pin on your micro-controller. 
  
  SparkFun Electronics 
  Author: Elias Santistevan
  Date: July 2019

	License: This code is public domain but if you use this and we meet someday, get me a beer! 

	Feel like supporting our work? Buy a board from Sparkfun!
	https://www.sparkfun.com/products/15436

*/

#include <Wire.h>
#include "SparkFun_VEML6030_Ambient_Light_Sensor.h"

// Close the address jumper on the product for addres 0x10.
#define AL_ADDR_1 0x10
#define AL_ADDR_2 0x48

SparkFun_Ambient_Light light_1(AL_ADDR_1);
SparkFun_Ambient_Light light_2(AL_ADDR_2);

// Possible values: .125, .25, 1, 2
// Both .125 and .25 should be used in most cases except darker rooms.
// A gain of 2 should only be used if the sensor will be covered by a dark
// glass.
float gain = .125;

// Possible integration times in milliseconds: 800, 400, 200, 100, 50, 25
// Higher times give higher resolutions and should be used in darker light. 
int time1 = 100;
long luxVal_1 = 0;
long luxVal_2 = 0; 

/* Relay Number: Relay 1 = Pin 4-> Qwiic Board: 4 [Redboard Artemis ATP 22}
 *  Relay 2 = Pin 7-> Qwiic Board: 7 [Redboard Artemis ATP 28]
 *  Relay 3 = Pin 8-> Qwiic Board: 8 [Redboard Artemis ATP A32 -> Digital 32]
 *  Relay 4 = Pin 12-> Qwiic Board: 12 [Redboard Artemis ATP MISO -> Digital 6]
 */
int RelayControl1_1 = 4;
int RelayControl1_2 = 7; 


// Interrupt settings. 
long lowThresh = 200; 
long highThresh = 1000; 
int numbValues = 1;

// Interrupt variable
int interrupt; 

// Message packet
String message;
String comma = ",";

void setup(){

  Wire.begin();
  Serial.begin(9600);
  pinMode(RelayControl1_1, OUTPUT);
  pinMode(RelayControl1_2, OUTPUT);

  light_1.begin();
  light_2.begin();
  //if(light.begin())
    //Serial.println("Ready to sense some light!"); 
  //else
    //Serial.println("Could not communicate with the sensor!");

  // Again the gain and integration times determine the resolution of the lux
  // value, and give different ranges of possible light readings. Check out
  // hoookup guide for more info. The gain/integration time also affects 
  // interrupt threshold settings so ALWAYS set gain and time first. 
  light_1.setGain(gain);
  light_1.setIntegTime(time1);

  light_2.setGain(gain);
  light_2.setIntegTime(time1);


  //Serial.println("Reading settings..."); 
  //Serial.print("Gain: ");
  float gainVal = light_1.readGain();
  //Serial.print(gainVal, 3); 
  //Serial.print(" Integration Time: ");
  int timeVal = light_1.readIntegTime();
  //Serial.println(timeVal);

  // Set both low and high thresholds, they take values in lux.
  light_1.setIntLowThresh(lowThresh);
  light_1.setIntHighThresh(highThresh);
  light_2.setIntLowThresh(lowThresh);
  light_2.setIntHighThresh(highThresh);
  
  // Let's check that they were set correctly. 
  // There are some rounding issues inherently in the IC's design so your lux
  // may be one off. 
  //Serial.print("Aux Light ON At: ");
  long lowVal = light_1.readLowThresh();
  //Serial.print(lowVal);
  //Serial.println("Aux Light OFF At:  ");
  long highVal = light_1.readHighThresh();
  //Serial.println(highVal);

  // This setting modifies the number of times a value has to fall below or
  // above the threshold before the interrupt fires! Values include 1, 2, 4 and
  // 8. 
  light_1.setProtect(numbValues);
  light_2.setProtect(numbValues);
  //Serial.print("Number of values that must fall below/above threshold before interupt occurrs: ");
  int protectVal = light_1.readProtect();
  //Serial.println(protectVal);

  // Now we enable the interrupt, now that he thresholds are set. 
  light_1.enableInt();
  //Serial.print("Is interrupt enabled: ");
  int intVal = light_1.readIntSetting(); 
  //if (intVal == 1)
    //Serial.println("Yes"); 
  //else 
    //Serial.println("No");


  //Serial.println("-------------------------------------------------");
  
  // Give some time to read our settings on startup.
  delay(3000);
}

void loop(){

  luxVal_1 = light_1.readLight();
  luxVal_2 = light_2.readLight();
  //Serial.print("Ambient Light Reading: ");
  //Serial.print(luxVal_1);
  //Serial.print(",");
  //Serial.println(luxVal_2);
  message = luxVal_1 + comma + luxVal_2;
  Serial.println(message);  

  
  //if (Serial.readString())
  
  
  interrupt = light_1.readInterrupt();
  if (interrupt == 1)
    {
    //Serial.println("Auxilary Light Off");
    digitalWrite(RelayControl1_1,LOW);// NO1 and COM1 Disconnected (LED off)
    digitalWrite(RelayControl1_2,LOW);}// NO1 and COM1 Disconnected (LED off)
    //delay(1000); // wait 1000 milliseconds (1 second)
  else if (interrupt == 2)
    {
    //Serial.println("Auxilarly Light ON");
    digitalWrite(RelayControl1_1,HIGH);// NO1 and COM1 Connected (LED on)
    digitalWrite(RelayControl1_2,HIGH);}// NO1 and COM1 Connected (LED on)
    //delay(1000); // wait 1000 milliseconds (1 second)
    
  delay(500);
}
