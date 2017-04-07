

int TTL_PIN = 11;
int MEASURE_PIN = 3;
unsigned long pause = 60000;
volatile unsigned long trigger;
unsigned long currentMillis;
unsigned long previousMillis;
const long interval = 1000;
volatile double distance;
float freq;
int dutycycle;

bool Connected = false;

const unsigned int serialOutputInterval = 10; // Output Frequency = 1000 / serialOutputInterval = 1000 / 10 = 100Hz
unsigned long serialLastOutput = 0;

const char StartFlag = '#';
const String Delimiter = "\t";

float c[] =   {16.35,   32.70,  65.41,  130.81,   261.63,   523.25,   1046.50,  2093.00,  4186.01};
float cis[] = {17.32,   34.65,  69.30,  138.59,   277.18,   554.37,   1108.73,  2217.46,  4434.92};
float d[] =   {18.35,   36.71,  73.42,  146.83,   293.66,   587.33,   1174.66,  2349.32,  4698.63};
float dis[] = {19.45,   38.89,  77.78,  155.56,   311.13,   622.25,   1244.51,  2489.02,  4978.03};
float e[] =   {20.60,   41.20,  82.41,  164.81,   329.63,   659.25,   1318.51,  2637.02,  5274.04};
float f[] =   {21.83,   43.65,  87.31,  174.61,   349.23,   698.46,   1396.91,  2793.83,  5587.65};
float fis[] = {23.12,   46.25,  92.50,  185.00,   369.99,   739.99,   1479.98,  2959.96,  5919.19};
float g[] =   {24.50,   49.00,  98.00,  196.00,   392.00,   783.99,   1567.98,  3135.96,  6271.93};
float gis[] = {25.96,   51.91,  103.83, 207.65,   415.30,   830.61,   1661.22,  3322.44,  6644.88};
float a[] =   {27.50,   55.00,  110.00, 220.00,   440.00,   880.00,   1760.00,  3520.00,  7040.00};
float ais[] = {29.14,   58.27,  116.54, 233.08,   466.16,   932.33,   1864.66,  3729.31,  7458.62};
float b[] =   {30.87,   61.74,  123.47, 246.94,   493.88,   987.77,   1975.53,  3951.07,  7902.13};

void setup() {
  // put your setup code here, to run once:
  pinMode(TTL_PIN, OUTPUT);
  pinMode(MEASURE_PIN, INPUT);
  Serial.begin(115200);
}

void loop() {
  
  digitalWrite(TTL_PIN, HIGH);
  delayMicroseconds(10);
  trigger = micros();
  digitalWrite(TTL_PIN, LOW);
  delayMicroseconds(pause-10);
  
  attachInterrupt(digitalPinToInterrupt(MEASURE_PIN), triggered, HIGH);

  

  if (distance < 60) {
    freq = c[4];
  } 
  if ( distance > 60 ) {
    freq = d[4];
  } 
  if ( distance > 63 ) {
    freq = e[4];
  } 
  if ( distance > 66 ) {
    freq = f[4];
  } 
  if ( distance > 69 ) {
    freq = g[4];
  } 
  if ( distance > 72 ) {
    freq = a[4];
  } 
  if ( distance > 75 ) {
    freq = b[4];
  } 
  if ( distance > 78 ) {
    freq = c[5];
  }
  SerialOutput();
}

void triggered() {
   unsigned long echo = micros()-trigger;
   distance = echo / 58;
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
  Serial.print(freq);         //Write a value
  Serial.println();           // Write endflag '\n' to indicate end of package
}

