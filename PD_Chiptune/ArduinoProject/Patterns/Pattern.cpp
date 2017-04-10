/*
  Pattern.cpp - Library for playing patterns.
  Created by Martin Kibsgaard, Feb. 2016.
  Released into the public domain.
*/

#include "Arduino.h"
#include "Pattern.h"

Pattern::Pattern(int length, byte intensities[], unsigned int durations[])
{
  this->length = length;
  this->durations = durations;
  this->intensities = intensities;
  
}

//Pattern::Pattern(){};
