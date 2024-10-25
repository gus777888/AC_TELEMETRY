int led1 = 2;
int led2 = 3;
int led3 = 4;
int led4 = 5;
int led5 = 6;
int led6 = 7;




void setup() 

{
  Serial.begin(9600);         // Initialize serial communication at 9600 bps
  pinMode(led1, OUTPUT);        // Set pin 0 as an output (for the LED)
  pinMode(led2, OUTPUT);        // Set pin 1 as an output (for the LED)
  pinMode(led3, OUTPUT);        // Set pin 2 as an output (for the LED)
  pinMode(led4, OUTPUT);        // Set pin 3 as an output (for the LED)
  pinMode(led5, OUTPUT);        // Set pin 4 as an output (for the LED)
  pinMode(led6, OUTPUT);        // Set pin 5 as an output (for the LED)
}

void loop() 
{
  if (Serial.available() > 0) {
    int incomingData = Serial.parseInt();  // Read incoming data as an integer
    int rpms = incomingData;       // Example of handling RPM data

    // Control LED based on gear value
    if (rpms >=  3000) {
      digitalWrite(led1, HIGH);  // Turn on LED on pin 13 if gear is greater than 1
    } else {
      digitalWrite(led1, LOW);   // Turn off LED otherwise
    }
    
    if (rpms >=  4000) {
      digitalWrite(led2, HIGH);  // Turn on LED on pin 13 if gear is greater than 1
    } else {
      digitalWrite(led2, LOW);   // Turn off LED otherwise
    }

     if (rpms >=  5000) {
      digitalWrite(led3, HIGH);  // Turn on LED on pin 13 if gear is greater than 1
    } else {
      digitalWrite(led3, LOW);   // Turn off LED otherwise
    }

     if (rpms >=  6000) {
      digitalWrite(led4, HIGH);  // Turn on LED on pin 13 if gear is greater than 1
    } else {
      digitalWrite(led4, LOW);   // Turn off LED otherwise
    }
   
    if (rpms >=  7000) {
      digitalWrite(led5, HIGH);  // Turn on LED on pin 13 if gear is greater than 1
    } else {
      digitalWrite(led5, LOW);   // Turn off LED otherwise
    }
    
    if (rpms >=  8000) {
      digitalWrite(led6, HIGH);  // Turn on LED on pin 13 if gear is greater than 1
    } else {
      digitalWrite(led6, LOW);   // Turn off LED otherwise
    }
  }
}
