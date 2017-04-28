/*
  Pattern.h - Library for playing patterns.
  Created by Martin Kibsgaard, Feb. 2016.
  Released into the public domain.
*/

#ifndef Pattern_h
#define Pattern_h

#include "Arduino.h"

class Pattern {
  public:
    Pattern(int length, byte intensities[], unsigned int durations[]);
    Pattern(){};
    int length;
    unsigned int *durations;
    byte *intensities;    
    
};

#endif
