// Library includes
#include "I2Cdev.h"
#include "MPU6050_6Axis_MotionApps20.h"
#if I2CDEV_IMPLEMENTATION == I2CDEV_ARDUINO_WIRE
    // If I2Cdev I2CDEV_ARDUINO_WIRE is used in I2Cdev.h
    #include "Wire.h"
#endif

MPU6050 mpu;

#define LED_PIN 13
bool blinkState = false;

// MPU control/status vars
bool dmpReady = false;  // set true if DMP init was successful
uint8_t mpuIntStatus;   // holds actual interrupt status byte from MPU
uint8_t devStatus;      // return status after each device operation (0 = success, !0 = error)
uint16_t packetSize;    // expected DMP packet size (default is 42 bytes)
uint16_t fifoCount;     // count of all bytes currently in FIFO
uint8_t fifoBuffer[64]; // FIFO storage buffer

// orientation/motion vars
Quaternion q;           // [w, x, y, z]         quaternion container
VectorInt16 aa;         // [x, y, z]            accel sensor measurements
VectorInt16 aaReal;     // [x, y, z]            gravity-free accel sensor measurements
VectorInt16 aaWorld;    // [x, y, z]            world-frame accel sensor measurements
VectorFloat gravity;    // [x, y, z]            gravity vector
float euler[3];         // [psi, theta, phi]    Euler angle container
float ypr[3];           // [yaw, pitch, roll]   yaw/pitch/roll container and gravity vector
int rotX = 0;
int rotY = 0;
int rotZ = 0;

// Output Frequency = 1000 
// serialOutputInterval = 1000 
// 10 = 100Hz
const unsigned int serialOutputInterval = 10; 
unsigned long serialLastOutput = 0;

const char StartFlag = '#';
const String Delimiter = "\t";

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

// indicates whether MPU interrupt pin has gone high
volatile bool mpuInterrupt = false;     
void dmpDataReady() {
    mpuInterrupt = true;
}

void setup() {
    // join I2C bus (I2Cdev library doesn't do this automatically)
    #if I2CDEV_IMPLEMENTATION == I2CDEV_ARDUINO_WIRE
        Wire.begin();
        TWBR = 24; // 400kHz I2C clock (200kHz if CPU is 8MHz)
    #elif I2CDEV_IMPLEMENTATION == I2CDEV_BUILTIN_FASTWIRE
        Fastwire::setup(400, true);
    #endif

    // initialize serial communication
    Serial.begin(38400);
    while (!Serial);

    // initialize device
    Serial.println(F("Initializing I2C devices..."));
    mpu.initialize();

    // verify connection
    Serial.println(F("Testing device connections..."));
    Serial.println(mpu.testConnection() ? F("MPU6050 connection successful") : F("MPU6050 connection failed"));

    // load and configure the DMP
    Serial.println(F("Initializing DMP..."));
    devStatus = mpu.dmpInitialize();

    // Gyroscope offsets
    mpu.setXAccelOffset(-2463);
    mpu.setYAccelOffset(-217);
    mpu.setZAccelOffset(1314);
    mpu.setXGyroOffset(72);
    mpu.setYGyroOffset(-54);
    mpu.setZGyroOffset(40);

    // make sure it worked (returns 0 if so)
    if (devStatus == 0) {
        // turn on the DMP, now that it's ready
        Serial.println(F("Enabling DMP..."));
        mpu.setDMPEnabled(true);

        // enable Arduino interrupt detection
        Serial.println(F("Enabling interrupt detection (Arduino external interrupt 0)..."));
        attachInterrupt(0, dmpDataReady, RISING);
        mpuIntStatus = mpu.getIntStatus();

        // set our DMP Ready flag so the main loop() function knows it's okay to use it
        Serial.println(F("DMP ready! Waiting for first interrupt..."));
        dmpReady = true;

        // get expected DMP packet size for later comparison
        packetSize = mpu.dmpGetFIFOPacketSize();
    } else {
        // ERROR!
        // 1 = initial memory load failed
        // 2 = DMP configuration updates failed
        // (if it's going to break, usually the code will be 1)
        Serial.print(F("DMP Initialization failed (code "));
        Serial.print(devStatus);
        Serial.println(F(")"));
    }

    // configure LED for output
    pinMode(LED_PIN, OUTPUT);

    // Analog Input for Flexion Sensor
    pinMode(A0, INPUT);

    //PWM and echo pins for ultrasonic
    pinMode(TTL_PIN, OUTPUT);
    pinMode(MEASURE_PIN, INPUT);
}

void loop() {
    // if programming failed, don't try to do anything
    if (!dmpReady) return;

    // wait for MPU interrupt or extra packet(s) available
    //while (!mpuInterrupt && fifoCount < packetSize) {
        // other program behavior stuff here
        // .
        // .
        // .
        // if you are really paranoid you can frequently test in between other
        // stuff to see if mpuInterrupt is true, and if so, "break;" from the
        // while() loop to immediately process the MPU data
        // .
        // .
        // .
    //}

    // reset interrupt flag and get INT_STATUS byte
    mpuInterrupt = false;
    mpuIntStatus = mpu.getIntStatus();

    // get current FIFO count
    fifoCount = mpu.getFIFOCount();

    // check for overflow (this should never happen unless our code is too inefficient)
    if ((mpuIntStatus & 0x10) || fifoCount == 1024) {
        // reset so we can continue cleanly
        mpu.resetFIFO();
        Serial.println(F("FIFO overflow!"));

    // otherwise, check for DMP data ready interrupt (this should happen frequently)
    } else if (mpuIntStatus & 0x02) {
        // wait for correct available data length, should be a VERY short wait
        while (fifoCount < packetSize) fifoCount = mpu.getFIFOCount();

        // read a packet from FIFO
        mpu.getFIFOBytes(fifoBuffer, packetSize);
        
        // track FIFO count here in case there is > 1 packet available
        // (this lets us immediately read more without waiting for an interrupt)
        fifoCount -= packetSize;

        // display Euler angles in degrees
        mpu.dmpGetQuaternion(&q, fifoBuffer);
        mpu.dmpGetGravity(&gravity, &q);
        mpu.dmpGetYawPitchRoll(ypr, &q, &gravity);
        rotX = (ypr[0] * 180/M_PI);
        rotY = (ypr[1] * 180/M_PI);
        rotZ = (ypr[2] * 180/M_PI);

        // Indicate activity with LED BLINK
        blinkState = !blinkState;
        digitalWrite(LED_PIN, blinkState);    
    }

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
    
    //Check if it is time to send data to Unity
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
  Serial.print(rotX);       //Write a value
  Serial.print(Delimiter);    //Write delimiter
  Serial.print(rotY);       //...
  Serial.print(Delimiter);
  Serial.print(rotZ);
  Serial.print(Delimiter);
  Serial.print(analogRead(A0));
  Serial.print(Delimiter);    //Delimiter used to split values
  Serial.print(freq);         //Write a value
  Serial.println();           // Write endflag '\n' to indicate end of package
}
