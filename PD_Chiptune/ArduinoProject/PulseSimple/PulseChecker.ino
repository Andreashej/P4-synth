int Peak = 512;                   // used to find peak in pulse wave, seeded
int Trough = 512;                 // used to find trough in pulse wave, seeded
int Threshold = 525;              // used to find instant moment of heart beat, seeded
int Amplitude = 100;              // used to hold amplitude of pulse waveform, seeded
int Signal;                       // holds the incoming raw data
unsigned long lastBeatTime = 0;   // Store last millis a pulse was detected

void checkPulseSensor(){
  Signal = analogRead(PulsePin);                  // Read the Pulse Sensor
  int N = millis() - lastBeatTime;                // Calculate time since we last had a beat

  if (Signal < Threshold && Pulse == true) {      // When the values are going below the threshold, the beat is over
    Pulse = false;                                // reset the Pulse flag so we are ready for another pulse
    Threshold = (Peak + Trough) / 2;              // Set threshold to the average of min and max values read.
    Peak = Threshold;                             // reset for next pulse
    Trough = Threshold;                           // reset for next pulse
    return;
  }
  
  //  Find the trough and the peak (aka. min and max) of the pulse wave (they are used to adjust threshold)
  if (Signal > Peak) {
    Peak = Signal;                           // keep track of highest point in pulse wave   
  }                                          
  if (Signal < Trough) {                     
    Trough = Signal;                         // keep track of lowest point in pulse wave
  }

  // Avoid false positives by waiting 3/5 of last IBI (for example we want to avoid the second, smaller pulse, aka. dicrotic pulse)
  if (N < (IBI / 5) * 3)                  
    return;
    
  // Look for a heart beat
  if (N > 250) {                                      // avoid high frequency noise
    if ( (Signal > Threshold) && !Pulse ) {           // Signal surges up in value every time there is a pulse
      Pulse = true;                                   // set the Pulse flag when we think there is a pulse
      IBISign *= -1;                                  // Change the sign value, to indicate a new IBI is available (in case two consecutive IBIs are the same) 
      IBI = N;                                        // Set inter-beat interval
      lastBeatTime = millis();                        // keep track of time for next pulse
    }
  }

  if (N > 2500) {                          // if 2.5 seconds go by without a beat, reset values and wait for a clear beat
    Threshold = 512;                       // set threshhold to default value
    Peak = 512;                            // set P default
    Trough = 512;                          // set T default
    lastBeatTime = millis();               // bring the lastBeatTime up to date (would probably be better to just leave it be, and filter any large values (>2500) from the data)
  }
}
