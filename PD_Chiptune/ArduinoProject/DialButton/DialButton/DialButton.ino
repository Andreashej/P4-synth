int potPin = A0; 
int buttonPin = 13;

int pMin = 0; 
int pMax = 1023;
int val = 0;

int buttonState = 0;
int selector = 0;
int count = 0;
int reverse = 0;

const unsigned int serialOutputInterval = 10; // Output Frequency = 1000 / serialOutputInterval = 1000 / 10 = 100Hz
unsigned long serialLastOutput = 0;

const char StartFlag = '#';
const String Delimiter = "\t";

void setup() {
  Serial.begin(115200);
  pinMode(buttonPin, INPUT);  
}

void loop() {

  ReadSensors(); //Have the Arduino read it's sensors etc.
  SerialOutput(); //Check if it is time to send data to Unity
}

void ReadSensors()
{
  //Just some bogus examples to get some data to send
  val = analogRead(potPin);
  val = map(val, pMin, pMax, 1, 800);
  buttonState = digitalRead(buttonPin);

  if (selector == 0){
    reverse = 0;
    } else if (selector == 7){
      reverse = 7;
    }
  
  if (buttonState == HIGH) {
    count++;
  } else if (buttonState == LOW) {
    if (count > 0) {
      if(reverse == 0){
      selector += 1;
      count = 0;
      } else if (reverse == 7){
        selector -= 1;
        count = 0;
      }
    }
  }
  
}

const int inputCount = 7; //This must match the amount of bytes you send from Unity!
byte inputBuffer[inputCount];
void SerialInput()
{  
  if(Serial.available() > 0){ //check if there is some data from Unity
     Serial.readBytes(inputBuffer, inputCount); //read the data
     //Use the data for something
     digitalWrite(13, inputBuffer[0]);
     digitalWrite(12, inputBuffer[1]);
     digitalWrite(11, inputBuffer[2]);
     digitalWrite(9, inputBuffer[3]);
     digitalWrite(8, inputBuffer[4]);
     digitalWrite(4, inputBuffer[5]);
     digitalWrite(3, inputBuffer[6]);

     //You could for example use the data for playing patterns
     //e.g. first value indicates which player and second value indicates which pattern to play
     }

     //Currently no checks for desync (no start/end flags or package size checks)
     //This should be implemented to make the imp. more robust
}


void SerialOutput() {
  //Time to output new data?
  if(millis() - serialLastOutput < serialOutputInterval)
    return;
  serialLastOutput = millis();

  //Write data package to Unity
  Serial.write(StartFlag);    //Flag to indicate start of data package
  Serial.print(millis());     //Write the current "time"
  Serial.print(Delimiter);    //Delimiter used to split values
  Serial.print(val);       //Write a value
  Serial.print(Delimiter);    //Write delimiter
  Serial.print(selector);       //...
  Serial.println();           // Write endflag '\n' to indicate end of package
}



