
const int pinoAnalogico = A0;
const int pinoDigital1 = 12;
const int pinoDigital2 = 13;


void setup(){
  Serial.begin(9600);
  pinMode(pinoDigital1,INPUT);
  pinMode(pinoDigital2,INPUT);
}

void loop(){


if(Serial.available()){

String comando = Serial.readStringUntil('\n');
comando.trim();// remove espaços e quebras de linha

if(comando == "READ"){

delay(100);


int valorBruto = analogRead(pinoAnalogico);
int valorA0 = map (valorBruto , 0 , 1023 , 0 , 100);

int ValorD1 = digitalRead(pinoDigital1);
int ValorD2 = digitalRead(pinoDigital2);

Serial.print("A:");
Serial.print(valorA0);
Serial.print(",D1:");
Serial.print(!ValorD1);
Serial.print(",D2:");
Serial.print(!ValorD2);
Serial.println(",");


}
}
}
