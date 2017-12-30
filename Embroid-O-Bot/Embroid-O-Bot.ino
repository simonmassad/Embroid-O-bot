#include "Stitch.h"

//const int stepPin = 2; 
//const int dirPin = 4; 
//const int stepPin = 6; 
//const int dirPin = 8; 


const int xStepPin = 2; 
const int xDirPin = 4; 
const int yStepPin = 6; 
const int yDirPin = 8; 
const int zStepPin = 10; 
const int zDirPin = 12; 

const int yStepsPerUnit = 8;// ->  (gearFactor)*(microstepping)/(10 unitsPerMM)
const int xStepsPerUnit = 8;// ->  (gearFactor)*(microstepping)/(10 unitsPerMM)
const int bedMotorDelay = 400;

void setup() {
  // Sets the two pins as Outputs
  pinMode(xStepPin,OUTPUT); 
  pinMode(xDirPin,OUTPUT);

  pinMode(yStepPin,OUTPUT); 
  pinMode(yDirPin,OUTPUT);

   pinMode(zStepPin,OUTPUT); 
  pinMode(zDirPin,OUTPUT);


  Serial.begin(250000);  
}

int i = 0;

void loop() {


        if (Serial.available() > 3) {
                // read the incoming byte:

                Stitch s;

                s.xPath = Serial.read();
                s.xPath = s.xPath - 128;
                s.yPath = Serial.read();
                s.yPath = s.yPath - 128;
                s.jumpStitch = Serial.read();
                s.colourChange = Serial.read();


                if (s.xPath<0){
                  digitalWrite(xDirPin,LOW);
                }
                else{
                  digitalWrite(xDirPin,HIGH);
                }

                if (s.yPath<0){
                  digitalWrite(yDirPin,LOW);
                }
                else{
                  digitalWrite(yDirPin,HIGH);
                }

                int xTarget = abs(s.xPath);
                int yTarget = abs(s.yPath);

                bool xTargetReached = false;
                bool yTargetReached = false;

                int xInc = 0;
                int yInc = 0;


                
                while (xTargetReached == false || yTargetReached == false)
                {

                    if (xTargetReached == false){

                        digitalWrite(xStepPin,HIGH); 
                        delayMicroseconds(bedMotorDelay); 
                        digitalWrite(xStepPin,LOW); 
                        delayMicroseconds(bedMotorDelay); 

                        xInc++;

                        if (xInc > xTarget * xStepsPerUnit){
                          xTargetReached = true;
                        }
                      
                    }

                    if (yTargetReached == false){

                        digitalWrite(yStepPin,HIGH); 
                        delayMicroseconds(bedMotorDelay); 
                        digitalWrite(yStepPin,LOW); 
                        delayMicroseconds(bedMotorDelay); 

                        yInc++;

                        if (yInc > yTarget * yStepsPerUnit){
                          yTargetReached = true;
                        }
                      
                    }

                  
                }
                
                if (s.jumpStitch == 0)
                {
                digitalWrite(zDirPin,LOW); // Enables the motor to move in a particular direction
                // Makes 200 pulses for making one full cycle rotation
                for(int x = 0; x < 200; x++) {
                  digitalWrite(zStepPin,HIGH); 
                  delayMicroseconds(900); 
                  digitalWrite(zStepPin,LOW); 
                  delayMicroseconds(900); 
                }
                }
       
               
                Serial.println( String(i++) + "   "  +     String(s.xPath) + ":" + String(s.yPath) + ":" + String(s.jumpStitch) + ":" + String(s.colourChange));
        }
        else
        {
         
        }

  /*
  digitalWrite(dirPin,LOW); // Enables the motor to move in a particular direction
  // Makes 200 pulses for making one full cycle rotation
  for(int x = 0; x < 200; x++) {
    digitalWrite(stepPin,HIGH); 
    delayMicroseconds(900); 
    digitalWrite(stepPin,LOW); 
    delayMicroseconds(1000); 
  }
  delay(0); // One second delay
*/
}

char* string2char(String command){
    if(command.length()!=0){
        char *p = const_cast<char*>(command.c_str());
        return p;
    }
}


