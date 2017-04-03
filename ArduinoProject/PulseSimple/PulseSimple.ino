int PulsePin = A0;         // Pulse Sensor purple wire connected to analog pin 0
int IBI = 600;             // int that holds the time interval between beats! Must be seeded! 
int IBISign = 1;           // used to change the sign of the IBI, to indicate a new value (in case two succeeding values are the same)
boolean Pulse = false;     // "True" when a heartbeat is detected. (can be used for for blinking an LED everytime a heartbeat is detected) 

void setup() {
  Serial.begin(115200);
  pinMode(13,OUTPUT);
}


int lastIBISign = 1; //Store the last state of the IBI sign
void loop() {
  checkPulseSensor(); //call this funtion as often as possible

  //Only print a value when a new pulse is detected
  if(IBISign != lastIBISign){
    Serial.println(IBI * IBISign);
    lastIBISign = IBISign;
  }

  //You can also use the pulse bool to e.g. light an LED
  digitalWrite(13,Pulse);
}
