/*
  PatternPlayer.h - Library for playing patterns.
  Created by Martin Kibsgaard, Feb. 2016.
  Released into the public domain.
*/

#ifndef PatternPlayer_h
#define PatternPlayer_h

#include "Arduino.h"
#include "Pattern.h"

class PatternPlayer {
  public:
    PatternPlayer(int pin);
    void playPattern(Pattern pattern, bool fade = false, int repeat = 0);
    void loop();
    bool playing = false;
    void fader();
  private:
    Pattern pattern;
    int pin;
    unsigned long patternNextStepTime;
    int patternStep;
    int reps;
    int repeat;
    bool play = false;
    
    
    bool fade = false;
    int updateInterval = 3;
    unsigned long lastUpdate = 0UL;
        
};

#endif

