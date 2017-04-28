/*
  Button.cpp - Library for debouncing buttons.
  Created by Martin Kibsgaard, Feb. 2016.
  Released into the public domain.
*/

#include "Arduino.h"
#include "Button.h"

Button::Button(int pin, int mode)
{
  this->pin = pin;
  pinMode(pin, mode);
}

bool Button::state(){
  reading = digitalRead(pin);
  if(reading == currentState && counter > 0)
    counter--;
  else if(reading != currentState)
    counter++;

  if(counter > debounceCount)
  {
    counter = 0;
    currentState = reading;
  }
  
  return currentState;
}

