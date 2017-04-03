#include "Button.h"

//Create a button object. Set which pin and mode (INPUT or INPUT_PULLUP)
Button button1(2,INPUT_PULLUP);

void setup() {
  // put your setup code here, to run once:
  pinMode(13, OUTPUT);
  Serial.begin(115200);
}

//Variables used for an example (not needed if you just want to know the state)
bool lastState = false;
unsigned int count = 0;
bool lastStateDebounced = false;
unsigned int countDebounced = 0;

void loop() {
  //Read the button state and use it for something
  bool state = button1.state();
  digitalWrite(13, state);

  

    
  //Below is an example showing the difference between debouncing and not
  //countDebounced should only increment by 1 when you press/release the button
  //While count might jump several values (some buttons bounce more than others)
    if(digitalRead(2) != lastState){
      
      lastState = !lastState;
      
      count++;
      Serial.print("Count: ");
      Serial.println(count);
    }
  
    if(button1.state() != lastStateDebounced){
      
      lastStateDebounced = !lastStateDebounced;
      
      countDebounced++;
      Serial.print("Debounced Count: ");
      Serial.println(countDebounced);
    }
}

