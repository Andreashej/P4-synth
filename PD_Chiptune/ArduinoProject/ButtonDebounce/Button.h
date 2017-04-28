/*
  Button.h - Library for debouncing buttons.
  Created by Martin Kibsgaard, Feb. 2016.
  Released into the public domain.
*/

#ifndef Button_h
#define Button_h

#include "Arduino.h"

class Button {
  public:
    Button(int pin, int mode);
    bool state();
  private:
    int pin;
    bool currentState = false;
    bool reading = false;
    int counter = 0;
    int debounceCount = 10;
    
};

#endif

