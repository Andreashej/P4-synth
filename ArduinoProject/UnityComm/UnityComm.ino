int rawEDA = 0;
int rawIBI = 0;
int rawDistance = 0;
bool Connected = false;

const unsigned int serialOutputInterval = 10; // Output Frequency = 1000 / serialOutputInterval = 1000 / 10 = 100Hz
unsigned long serialLastOutput = 0;

const char StartFlag = '#';
const String Delimiter = "\t";

void setup() {
  Serial.begin(115200);

//Example input/output pin
  pinMode(13, OUTPUT);
  pinMode(12, OUTPUT);
  pinMode(11, OUTPUT);
  pinMode(9, OUTPUT);
  pinMode(8, OUTPUT);
  pinMode(4, OUTPUT);
  pinMode(3, OUTPUT);
  
  pinMode(A0, INPUT);
  pinMode(A1, INPUT);
  pinMode(A2, INPUT);
}

void loop() {

  ReadSensors(); //Have the Arduino read it's sensors etc.

  SerialInput(); //Check if Unity has send anything

  SerialOutput(); //Check if it is time to send data to Unity
}

void ReadSensors()
{
  //Just some bogus examples to get some data to send
  rawEDA = analogRead(A0);
  rawIBI = analogRead(A1); //This is NOT how you read IBI
  rawDistance = analogRead(A2); 
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
  Serial.print(rawEDA);       //Write a value
  Serial.print(Delimiter);    //Write delimiter
  Serial.print(rawIBI);       //...
  Serial.print(Delimiter);
  Serial.print(rawDistance);
  Serial.println();           // Write endflag '\n' to indicate end of package
}



