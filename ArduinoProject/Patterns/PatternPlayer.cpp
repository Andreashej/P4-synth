/*
  PatternPlayer.cpp - Library for playing patterns.
  Created by Martin Kibsgaard, Feb. 2016.
  Released into the public domain.
*/

#include "Arduino.h"
#include "PatternPlayer.h"

//Setup the Pattern Player and set the pin mode for the selected pin.
PatternPlayer::PatternPlayer(int pin)
{
  this->pin = pin;
  pinMode(pin, OUTPUT);
}

//Play the specified pattern. It is optinal to enable fading (default off) and how many times it should repeat (default 0).
void PatternPlayer::playPattern(Pattern pattern, bool fade, int repeat) {
  //Save the chosen parameters
  this->pattern = pattern;
  this->repeat = repeat;
  this->fade = fade;
  
  //Reset variables
  patternStep = 0;
  reps = 0;

  //Set the first intensity and time until next step
  analogWrite(pin, pattern.intensities[patternStep]);
  patternNextStepTime = millis() + pattern.durations[patternStep];

  //Enable playback in the loop-function
  playing = true;
}


void PatternPlayer::loop() {
  if (!playing) //do nothing if no pattern is currently playing
    return;

  if (fade) //seperate code for dealing with fade/smooth transistions (more computational heavy)
    return fader();

  if (millis() < patternNextStepTime) //Time for next step?
    return;

  patternStep++; //Increase step number

  //Check if we have reached the end of the pattern and check if we should repeat, else stop playing.
  if (patternStep == pattern.length) {
    if (reps == repeat) {
      playing = false;
      return;
    } else {
      reps++;
      patternStep = 0;
    }
  }

  //Set intensity of current step and set time until next step
  analogWrite(pin, pattern.intensities[patternStep]);
  patternNextStepTime = millis() + pattern.durations[patternStep];
}



//Fader that linearly interpolates between each intensity
//CurrentIntensity = LastIntensity * a + NextIntensity * (1 - a)
//Where 'a' is a parameter between 0 and 1, which indicates how close we currently are to the next time step.
float a = 0;
float deltaTime = 0;
int intensity = 0;
void PatternPlayer::fader() {
  //Check if it is time to refresh the intensity
  if(millis() - lastUpdate < updateInterval)
    return;

  lastUpdate = millis();
  
  //Check if it is the last step (no interpolation - optimally, it should interpolate between reps also)
  if (patternStep < pattern.length - 1) {
    deltaTime = patternNextStepTime - millis();
    a = deltaTime / pattern.durations[patternStep];
    intensity = pattern.intensities[patternStep] * a + pattern.intensities[patternStep + 1] * (1.0 - a);
    analogWrite(pin, intensity);
  } else {
    analogWrite(pin, pattern.intensities[patternStep]);
  }

  if (millis() > patternNextStepTime) {
    patternStep++;
    if (patternStep == pattern.length) {
      if (reps == repeat) {
        playing = false;
      } else {
        reps++;
        patternStep = 0;
      }
    }
    patternNextStepTime = millis() + pattern.durations[patternStep];
  }
}


