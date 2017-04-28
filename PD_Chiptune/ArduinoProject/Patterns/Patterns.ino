#include "PatternPlayer.h"

//Create PatternPlayers (one for each output pin you need)
//PatternPlayer name(pin);

PatternPlayer myLED0(10);
PatternPlayer myLED1(11);
PatternPlayer myLED2(13);

//Define patterns like this:
//Pattern patternName(
//                    Pattern length/steps,
//                    Array of Intensities (0-255),
//                    Array of Durations (0-65535)
//                    );
//Length of the arrays must match the defined pattern length

//Example of pattern that blinks twice and then waits for 800 ms
const Pattern twoBlinks(
                    4,
                    (byte[])          {255,   0, 255,   0},
                    (unsigned int[])  {200, 200, 200, 800}
                    );

//Example of pattern that blinks three times, with decaying intensity and then waits for 800 ms
Pattern threeBlinks(
                    6,
                    (byte[])          {255,   0, 140,   0,  70,   0},
                    (unsigned int[])  {200, 200, 200, 200, 200, 800}
                    );

//Example of pattern used for fading (e.g. if it should fade-in, the first value should be zero)
Pattern threeBlinksFade(
                    7,
                    (byte[])          {  0, 255,   0, 255,   0, 255,   0},
                    (unsigned int[])  {400, 400, 400, 400, 400, 400, 800}
                    );


//It is possible to save the patterns in a list if needed (e.g. to make it easier to specify which pattern to play directly from Unity)
Pattern PatternList[] = {twoBlinks, twoBlinks, threeBlinks, threeBlinksFade};
const int PatternListSize = sizeof(PatternList)/sizeof(Pattern);

//Also possibleto create player lists
PatternPlayer PlayerList[] = {myLED0, myLED1, myLED2};

void setup() {
  //As an example input, I use a button on pin 7, that is pulled to ground when pressed
  pinMode(7, INPUT_PULLUP);
  Serial.begin(115200);
  Serial.println(PatternListSize);
}

int index = 0;
void loop() {
    //Always call loop on the pattern players. They will return immediatly if no pattern is playing, or if it is still waiting to do the next step.
    myLED0.loop();
    myLED1.loop();
    myLED2.loop();

    //Example of how to play a pattern (in this case when a button on pin 7 is pressed)
    if(!digitalRead(7))
      myLED0.playPattern(threeBlinksFade, true); //the 'true' indicates that the pattern should fade between intensities (requires a PWM pin)
      
    
    //patternPlayer.playPattern(Pattern pattern, bool fade, int repeat). fade and repeat are optinal. repeat specifices how many times the pattern should repeat after the first 

    //it is also possible to only activate a pattern if none is currently playing (not fading + repeating twice):
    if(!digitalRead(7) && !myLED1.playing)
      myLED1.playPattern(twoBlinks, false, 2); 


    //You can also use one (or more) pattern lists to easily play many different patterns (e.g. from Unity by just sending an interger/index over serial)
    if(!digitalRead(7) && !myLED2.playing)
      myLED2.playPattern(PatternList[index++], false); 
    
    if(index == PatternListSize)
      index = 0;
}

