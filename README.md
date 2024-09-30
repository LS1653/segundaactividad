# Ejercicio 1: comunicación entre computador y controlador

using UnityEngine;
using System.IO.Ports;
public class Serial : MonoBehaviour
{
private SerialPort _serialPort =new SerialPort();
private byte[] buffer =new byte[32];

void Start()
    {
        _serialPort.PortName = "COM4";
        _serialPort.BaudRate = 115200;
        _serialPort.DtrEnable =true;
        _serialPort.Open();
        Debug.Log("Open Serial Port");
    }

void Update()
    {

				if (Input.GetKeyDown(KeyCode.A))
        {
            byte[] data = {0x31};// or byte[] data = {'1'};            
						_serialPort.Write(data,0,1);
            Debug.Log("Send Data");
        }

				if (Input.GetKeyDown(KeyCode.B))
        {
						if (_serialPort.BytesToRead >= 16)
            {
                _serialPort.Read(buffer, 0, 20);
                Debug.Log("Receive Data");
                Debug.Log(System.Text.Encoding.ASCII.GetString(buffer));
            }
        }

    }
}

------------------------------------------------------------------------------------------

- ¿Por qué es importante considerar las propiedades *PortName* y *BaudRate*?

*PortName* : Es importante debido a que en esta se guarda el nombre del puerto serial en la cual se va a conectar la placa, sin embargo ya que esta variable la define la persona que haga el codigo, puede pasar que alguien conecte la placa a un puerto equibocado por el desconocimiento de que no en todas las compuertas se puede conectar la placa ya que no la detectaria.

*BaudRate* : Esta variable es la que le asigna la velocidad de envio de baudios (información) del computador a la placa y viceversa.

- ¿Qué relación tienen las propiedades anteriores con el controlador?

PortName y BaudRate están relacionados con la comunicación entre lao placa y la computadora. PortName establece el puerto de comunicación correcto, lo que asegura que se está comunicando con el dispositivo adecuado, mientras que BaudRate asegura que la velocidad de transferencia de datos entre el controlador y la computadora sea la correcta.

------------------------------------------------------------------------------------------------------
////////////////////////////////////////////////////////////////////////////////////////////////////

# Ejercicio 2: experimento

using UnityEngine;
using System.IO.Ports;
using TMPro;  // Importa el paquete TextMeshPro para manejar texto en Unity

public class Serial : MonoBehaviour
{
    // Declara un puerto serial para la comunicación
    private SerialPort _serialPort = new SerialPort();
    // Crea un buffer de 32 bytes para almacenar los datos leídos desde el puerto
    private byte[] buffer = new byte[32];

    // Referencia a un componente TextMeshProUGUI para mostrar texto en la interfaz de Unity
    public TextMeshProUGUI myText;

    // Variable estática para contar cuántas veces se ha actualizado el texto
    private static int counter = 0;

    // Función que se ejecuta al iniciar el script
    void Start()
    {
        // Establece el nombre del puerto serial a "COM4"
        _serialPort.PortName = "COM4";
        // Configura la velocidad de transmisión de datos a 115200 baudios
        _serialPort.BaudRate = 115200;
        // Habilita la señal Data Terminal Ready (DTR), importante para algunos dispositivos
        _serialPort.DtrEnable = true;
        // Abre la conexión del puerto serial
        _serialPort.Open();
        // Imprime un mensaje en la consola indicando que el puerto serial se ha abierto
        Debug.Log("Open Serial Port");
    }

    // Función que se llama en cada frame del juego
    void Update()
    {
        // Actualiza el texto en la pantalla con el valor del contador
        myText.text = counter.ToString();
        // Incrementa el contador en cada frame
        counter++;

        // Verifica si la tecla 'A' fue presionada
        if (Input.GetKeyDown(KeyCode.A))
        {
            // Crea un arreglo de bytes con el valor 0x31 (que representa el carácter '1')
            byte[] data = { 0x31 }; // o byte[] data = {'1'};
            // Envía un byte a través del puerto serial
            _serialPort.Write(data, 0, 1);
            // Lee hasta 20 bytes del puerto serial y los almacena en el buffer
            int numData = _serialPort.Read(buffer, 0, 20);
            // Convierte los datos del buffer en una cadena de texto ASCII y los imprime en la consola
            Debug.Log(System.Text.Encoding.ASCII.GetString(buffer));
            // Muestra cuántos bytes se recibieron e imprime el número en la consola
            Debug.Log("Bytes received: " + numData.ToString());
        }
    }
}

----------------------------------------------------------------------------------------

## Programa en unity con c#


using UnityEngine;
using System.IO.Ports;
using TMPro;  // Importa TextMeshPro para trabajar con texto en la UI de Unity

public class Serial : MonoBehaviour
{
    private SerialPort _serialPort = new SerialPort();
    private byte[] buffer = new byte[32];  // Buffer para almacenar los datos leídos desde el puerto serial

    public TextMeshProUGUI myText;  // Referencia al texto en la UI donde se mostrará la información

    private static int counter = 0;

    void Start()
    {
        _serialPort.PortName = "COM4";  // Configura el puerto serial correcto
        _serialPort.BaudRate = 115200;  // Configura la velocidad de transmisión a 115200 baudios
        _serialPort.DtrEnable = true;   // Habilita la señal DTR si es necesaria para el controlador
        _serialPort.Open();             // Abre el puerto serial
        Debug.Log("Open Serial Port");
    }

    void Update()
    {
        myText.text = counter.ToString();  // Actualiza el contador en la UI
        counter++;

        if (Input.GetKeyDown(KeyCode.A))  // Verifica si se presionó la tecla 'A'
        {
            byte[] data = { 0x31 };  // Envía el carácter '1' al controlador
            _serialPort.Write(data, 0, 1);
            int numData = _serialPort.Read(buffer, 0, 20);  // Lee hasta 20 bytes de datos desde el puerto serial
            Debug.Log(System.Text.Encoding.ASCII.GetString(buffer));  // Convierte el buffer a texto y lo imprime en consola
            Debug.Log("Bytes received: " + numData.ToString());  // Muestra cuántos bytes se recibieron
        }
    }
}

## Programa en la placa con c++

void setup()
{
    Serial.begin(115200);  // Configura la velocidad de comunicación serial a 115200 baudios
}

void loop()
{
    // Verifica si hay datos disponibles en el puerto serial
    if(Serial.available())
    {
        // Si el dato recibido es '1', envía el mensaje después de un retraso de 3 segundos
        if(Serial.read() == '1')
        {
            delay(3000);  // Espera 3 segundos
            Serial.print("Hello from Raspi");  // Envía el mensaje de vuelta por el puerto serial
        }
    }
}


Accion del programa
1)Presionas la tecla A.
2)Unity envía el byte '1' al controlador.
3)El controlador recibe el byte, espera 3 segundos, y envía el mensaje "Hello from Raspi" de vuelta  a Unity.
4)Unity recibe el mensaje y lo muestra en la consola o en la interfaz.

¿Observas que la aplicación se bloquea? Este comportamiento es inaceptable para una aplicación interactiva de tiempo real. ¿Cómo se podría corregir este comportamiento?
El bloqueo considero que ocurre porque serial.Read() es bloqueante y se llama sin verificar si hay datos disponibles osea el codigo se detiene esperando a que le llegue información al serial.read(). 

-------------------------------------------------------------------------------------------------------------------------


using UnityEngine;
using System.IO.Ports;
using TMPro;

public class Serial : MonoBehaviour
{
    // Crea una instancia de SerialPort para manejar la conexión serial
    private SerialPort _serialPort = new SerialPort();
    
    // Crea un buffer de 32 bytes para almacenar los datos recibidos
    private byte[] buffer = new byte[32];

    // Campo para mostrar texto en la interfaz gráfica (UI) usando TextMeshPro
    public TextMeshProUGUI myText;

    // Variable estática que se incrementará para mostrar un contador en pantalla
    private static int counter = 0;

    // Se ejecuta al iniciar el script
    void Start()
    {
        // Establece el puerto serial a usar (COM4 en este caso)
        _serialPort.PortName = "COM4";
        
        // Configura la tasa de baudios (velocidad de transmisión) en 115200
        _serialPort.BaudRate = 115200;
        
        // Habilita la señal DTR (Data Terminal Ready) que se utiliza en la comunicación serial
        _serialPort.DtrEnable = true;
        
        // Abre la conexión al puerto serial para comenzar a enviar y recibir datos
        _serialPort.Open();
        
        // Imprime en la consola de Unity que el puerto serial se ha abierto correctamente
        Debug.Log("Open Serial Port");
    }

    // Este método se llama una vez por cada frame del juego
    void Update()
    {
        // Actualiza el texto en la UI con el valor actual de 'counter'
        myText.text = counter.ToString();
        
        // Incrementa el valor del contador en cada frame
        counter++;

        // Si el usuario presiona la tecla "A"
        if (Input.GetKeyDown(KeyCode.A))
        {
            // Crea un arreglo de bytes con el valor 0x31 (equivalente al carácter '1' en ASCII)
            byte[] data = { 0x31 }; // o byte[] data = {'1'};
            
            // Envía un byte (el valor 0x31) a través del puerto serial
            _serialPort.Write(data, 0, 1);
        }

        // Si hay datos disponibles para leer en el puerto serial
        if (_serialPort.BytesToRead > 0)
        {
            // Lee hasta 20 bytes desde el puerto serial y los almacena en el buffer
            int numData = _serialPort.Read(buffer, 0, 20);
            
            // Convierte los bytes leídos a una cadena de texto ASCII y los imprime en la consola
            Debug.Log(System.Text.Encoding.ASCII.GetString(buffer));
            
            // Muestra en la consola la cantidad de bytes recibidos
            Debug.Log("Bytes received: " + numData.ToString());
        }
    }
}

¿Funciona?
Si, el codigo funciona y no se bloquea, sin embargo este mismo codigo siempre asume que se le enviaran todos los bytes que contienen al menaje a la hora de hacer la operación de lectura de dichos bytes.

¿Qué pasaría si al ejecutar _serialPort.Read(buffer, 0, 20) solo han llegado 10 de los 16 bytes del mensaje?
En ese caso, la función Read() leería solo los 10 bytes disponibles y guardaria esos bytes en el buffer. El problema es que el código se sigue ejecutándose, por lo que el codigo ignoria el hecho de que faltan 6 bytes.

¿Cómo puede hacer tu programa para saber que ya tiene el mensaje completo?
Las dos opciones que se me ocurren son, tener una variable que posea el numero de bytes que se necesecitan para imprimir el mensaje completo y pues hacer un verificador en el cual se mire  si el bufer posee o no la cantidad de bytes pedida, el mensaje no se imprimira, caso contrario si tiene la cantidad requerida.

Y la otra opción es hacer lo mismo que ya hace este programa que es mantener fija la cantidad de bytes que contienen al mensaje o mejor dicho el mensaje es el mismo siempre, lo cual le facilita al programa saber que solo tiene que llegar a la cantidad de bytes requerida para imprimir el mensaje.

¿Cómo se podría garantizar que antes de hacer la operación Read tenga los 16 bytes listos para ser leídos?
Por lo que pude investigar, para garantizar que antes de realizar una lectura se hayan recibido los 16 bytes, se puede llegar a verificar la cantidad de datos disponibles en el puerto serial con _serialPort.BytesToRead. Si hay menos de 16 bytes disponibles, el programa puede esperar y continuar acumulando bytes hasta que tenga los 16 completos.

ej: Verificar que hay al menos 16 bytes disponibles antes de leer
if (_serialPort.BytesToRead >= 16)
{
    int numData = _serialPort.Read(buffer, 0, 16);
    // Procesar el mensaje de 16 bytes
}

intento de implementación en el codigo

using UnityEngine;
using System.IO.Ports;
using TMPro;

public class Serial : MonoBehaviour
{
    private SerialPort _serialPort = new SerialPort();
    private byte[] buffer = new byte[32]; // Buffer para almacenar datos recibidos
    private const int messageLength = 16; // Longitud del mensaje esperado

    public TextMeshProUGUI myText;

    private static int counter = 0;

    void Start()
    {
        _serialPort.PortName = "COM4"; // Nombre del puerto serial
        _serialPort.BaudRate = 115200; // Velocidad de comunicación
        _serialPort.DtrEnable = true;  // Habilita DTR (Data Terminal Ready)
        _serialPort.Open(); // Abre el puerto serial
        Debug.Log("Open Serial Port");
    }

    void Update()
    {
        myText.text = counter.ToString(); // Actualiza el texto en pantalla con el valor de 'counter'
        counter++; // Incrementa el contador en cada frame

        if (Input.GetKeyDown(KeyCode.A)) // Si se presiona la tecla 'A'
        {
            byte[] data = { 0x31 }; // Envía el byte 0x31 (equivalente al carácter '1')
            _serialPort.Write(data, 0, 1); // Envía un byte al puerto serial
        }

        // Verifica si hay datos disponibles para leer y si la cantidad de datos es suficiente
        if (_serialPort.BytesToRead >= messageLength)
        {
            // Lee solo si ya hay suficientes bytes disponibles
            int numData = _serialPort.Read(buffer, 0, messageLength); // Lee 16 bytes del puerto serial
            string receivedMessage = System.Text.Encoding.ASCII.GetString(buffer, 0, numData); // Convierte los bytes leídos en un string
            Debug.Log("Mensaje recibido: " + receivedMessage); // Muestra el mensaje recibido
            Debug.Log("Bytes recibidos: " + numData.ToString()); // Muestra cuántos bytes se recibieron
        }
    }
}


¿Cómo haces para saber que el mensaje enviado está completo o faltan bytes por recibir cuando los mensajes tienen tamaños diferentes?
Por lo que pude investigar una forma de saber si el mensaje esta completo o no es poniendole un delimitador "/n" al final del mismo.
El código puede acumular bytes hasta que encuentre el delimitador y procesar el mensaje cuando lo haya recibido por completo.


using UnityEngine;
using System.IO.Ports;
using TMPro;
using System.Text;

public class SerialExample : MonoBehaviour
{
    private SerialPort _serialPort = new SerialPort();
    private StringBuilder messageBuffer = new StringBuilder(); // Buffer para acumular los bytes leídos
    public TextMeshProUGUI myText;
    private static int counter = 0;

    void Start()
    {
        _serialPort.PortName = "COM4";   // Configura el puerto serie
        _serialPort.BaudRate = 115200;   // Velocidad de transmisión
        _serialPort.DtrEnable = true;    // Habilita el DTR
        _serialPort.Open();              // Abre el puerto serie
        if (_serialPort.IsOpen)          // Verificar si el puerto se abre correctamente
        {
            Debug.Log("Puerto serie abierto correctamente.");
        }
        else
        {
            Debug.Log("Error al abrir el puerto serie.");
        }
    }

    void Update()
    {
        myText.text = counter.ToString(); // Actualiza el contador en pantalla
        counter++;

        // Enviar datos al presionar la tecla 'A'
        if (Input.GetKeyDown(KeyCode.A))  // Si se presiona la tecla 'A'
        {
            byte[] data = { 0x31 };       // Envía el byte '1' al puerto serial
            _serialPort.Write(data, 0, 1);
            Debug.Log("Byte enviado: 1");  // Depuración: Verificar que el byte '1' fue enviado
        }

        // Leer datos del puerto serial
        if (_serialPort.BytesToRead > 0)  // Si hay datos disponibles para leer
        {
            string receivedData = _serialPort.ReadExisting();  // Lee todos los datos disponibles como string
            messageBuffer.Append(receivedData);  // Acumula los datos en el buffer

            // Verifica si el buffer contiene el delimitador '\n' (fin del mensaje)
            if (messageBuffer.ToString().Contains("\n"))
            {
                // Procesa cada mensaje que termina con el delimitador
                string[] messages = messageBuffer.ToString().Split('\n');  // Divide el buffer en mensajes

                // Procesa todos los mensajes completos recibidos
                for (int i = 0; i < messages.Length - 1; i++)
                {
                    Debug.Log("Mensaje recibido: " + messages[i]);  // Depuración: Verificar que los mensajes se recibieron correctamente
                }

                // Borra el buffer hasta el último mensaje incompleto (si existe)
                messageBuffer.Clear();
                if (!string.IsNullOrEmpty(messages[messages.Length - 1]))
                {
                    messageBuffer.Append(messages[messages.Length - 1]);  // Guarda el mensaje incompleto
                }
            }
        }
    }
}

Modificación en el codigo del pi pico

void setup() {
    Serial.begin(115200);  // Configura la velocidad de comunicación serial a 115200 baudios
}

void loop() {
    // Verifica si hay datos disponibles en el puerto serial
    if (Serial.available()) {
        char received = Serial.read();  // Lee el byte recibido
        Serial.print("Recibido: ");
        Serial.println(received);  // Depuración: Mostrar el byte recibido en el monitor serie

        // Si el dato recibido es '1', envía el mensaje después de un retraso de 3 segundos
        if (received == '1') {
            delay(3000);  // Espera 3 segundos
            Serial.print("Hola desde Raspi\n");  // Envía el mensaje de vuelta por el puerto serial
        }
    }
}

--------------------------------------------------------------------------
/////////////////////////////////////////////////////////////////////////////
# Ejercicio 3:

Nótese que en los dos ejercicios anteriores, el PC primero le pregunta al controlador por datos (le manda un 1). ¿Y si el PC no pregunta?

Realiza el siguiente experimento. Programa ambos códigos y analiza su funcionamiento.
## Codigo en c++:

void task()
{
		enum class TaskStates
		{
				INIT,
				WAIT_INIT,
				SEND_EVENT
		};
		static TaskStates taskState = TaskStates::INIT;
		static uint32_t previous = 0;
		static u_int32_t counter = 0;

		switch (taskState)
		{
				case TaskStates::INIT:
				{
						Serial.begin(115200);
						taskState = TaskStates::WAIT_INIT;
						break;
				}
				case TaskStates::WAIT_INIT:
				{
						if (Serial.available() > 0)
						{
								if (Serial.read() == '1')
								{
										previous = 0; // Force to send the first value immediately
										taskState = TaskStates::SEND_EVENT;
								}
						}
						break;
				}
				case TaskStates::SEND_EVENT:
				{
						uint32_t current = millis();
						if ((current - previous) > 2000)
						{
								previous = current;
								Serial.print(counter);
								counter++;
						}
						if (Serial.available() > 0)
						{
							  if (Serial.read() == '2')
							  {
								    taskState = TaskStates::WAIT_INIT;
							  }
						}
						break;
				}
				default:
				{
						break;
				}
		}
}

void setup()
{
		task();
}

void loop()
{
		task();
}



## Codigo en c#



using UnityEngine;
using System.IO.Ports;
using TMPro;

enum TaskState
{
    INIT,
    WAIT_START,
    WAIT_EVENTS
}

public class Serial : MonoBehaviour
{
		private static TaskState taskState = TaskState.INIT;
		private SerialPort _serialPort;
		private byte[] buffer;
		public TextMeshProUGUI myText;
		private int counter = 0;

		void Start()
    {
        _serialPort =new SerialPort();
        _serialPort.PortName = "COM4";
        _serialPort.BaudRate = 115200;
        _serialPort.DtrEnable =true;
        _serialPort.Open();
        Debug.Log("Open Serial Port");
        buffer =new byte[128];
    }

void Update()
    {
        myText.text = counter.ToString();
        counter++;

				switch (taskState)
        {
						case TaskState.INIT:
		            taskState = TaskState.WAIT_START;
                Debug.Log("WAIT START");
								break;
						case TaskState.WAIT_START:
								if (Input.GetKeyDown(KeyCode.A))
                {
                    byte[] data = {0x31};// start
                    _serialPort.Write(data,0,1);
                    Debug.Log("WAIT EVENTS");
                    taskState = TaskState.WAIT_EVENTS;
                }
								break;
						case TaskState.WAIT_EVENTS:
								if (Input.GetKeyDown(KeyCode.B))
                {
                    byte[] data = {0x32};// stop
                    _serialPort.Write(data,0,1);
                    Debug.Log("WAIT START");
                    taskState = TaskState.WAIT_START;
                }
								if (_serialPort.BytesToRead > 0)
                {
                    int numData = _serialPort.Read(buffer, 0, 128);
                    Debug.Log(System.Text.Encoding.ASCII.GetString(buffer));
                }
								break;
						default:
                Debug.Log("State Error");
								break;
        }
    }
}


¿Recuerdas las preguntas presentadas en el experimento anterior? ¿Aquí nos pasa lo mismo?
No, no paso lo mismo, osea no se traba ni se detiene en ningun momento, esto debido a que bloqueo ocurria porque serial.Read() es bloqueante y se llamaba sin verificar si habian datos disponibles, osea el codigo se detenia esperando a que le llegara información al serial.read(), pero en este codigo como en la corrección que hice en el punto anterior, el codigo verifica mediante la función Serial.available(), si hay datos disponibles, de no ser asi no activa el serial.Read().

Analicemos el asunto. Cuando preguntas _serialPort.BytesToRead > 0 lo que puedes asegurar es que al MENOS tienes un byte del mensaje, pero no puedes saber si tienes todos los bytes que lo componen. Una idea para resolver esto, sería hacer que todos los mensajes tengan el mismo tamaño. De esta manera solo tendrías que preguntar _serialPort.BytesToRead > SIZE, donde SIZE sería el tamaño fijo; sin embargo, esto le resta flexibilidad al protocolo de comunicación. Nota que esto mismo ocurre en el caso del programa del controlador con Serial.available() > 0. ¿Cómo podrías solucionar este problema?

Mi soulación a este problema tambien lo puse en el ejercicio anterior, en donde lo que hago para saber cuando el mensaje ya se puede enviar ya que ya tengo toda la información es mediante un delimitador "/n" al final del mensaje.
El código puede acumular bytes hasta que encuentre el delimitador y procesar el mensaje cuando lo haya recibido por completo.


using UnityEngine;
using System.IO.Ports;
using TMPro;
using System.Text;

public class Serial : MonoBehaviour
{
    private SerialPort _serialPort;
    private StringBuilder messageBuffer = new StringBuilder();  // Buffer para acumular los bytes leídos
    public TextMeshProUGUI myText;
    private int counter = 0;

    void Start()
    {
        _serialPort = new SerialPort();
        _serialPort.PortName = "COM3";
        _serialPort.BaudRate = 115200;
        _serialPort.DtrEnable = true;
        _serialPort.Open();
        Debug.Log("Puerto serial abierto");
    }

    void Update()
    {
        myText.text = counter.ToString();  // Mostrar el contador en pantalla
        counter++;

        // Enviar datos con las teclas
        if (Input.GetKeyDown(KeyCode.A))
        {
            byte[] data = { 0x31 }; // Enviar el byte '1'
            _serialPort.Write(data, 0, 1);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            byte[] data = { 0x32 }; // Enviar el byte '2'
            _serialPort.Write(data, 0, 1);
        }

        // Leer datos desde el puerto serial
        if (_serialPort.BytesToRead > 0)
        {
            string receivedData = _serialPort.ReadExisting();  // Leer los datos disponibles
            messageBuffer.Append(receivedData);  // Acumular los datos en el buffer

            // Verificar si el buffer contiene el delimitador '\n'
            if (messageBuffer.ToString().Contains("\n"))
            {
                // Dividir el buffer en mensajes utilizando '\n' como delimitador
                string[] messages = messageBuffer.ToString().Split('\n');

                // Procesar todos los mensajes completos recibidos
                for (int i = 0; i < messages.Length - 1; i++)
                {
                    Debug.Log("Mensaje recibido: " + messages[i]);  // Mostrar el mensaje
                }

                // Limpiar el buffer hasta el último mensaje incompleto (si lo hay)
                messageBuffer.Clear();
                if (!string.IsNullOrEmpty(messages[messages.Length - 1]))
                {
                    messageBuffer.Append(messages[messages.Length - 1]);  // Guardar el mensaje incompleto
                }
            }

            // Control de tamaño del buffer para evitar crecimiento indefinido
            if (messageBuffer.Length > 1024)
            {
                Debug.LogWarning("Buffer demasiado grande, reiniciando...");
                messageBuffer.Clear();  // Limpiar buffer si es muy grande
            }
        }
    }
}



-------------------------------------------------------------------------------
//////////////////////////////////////////////////////////////////////////////7
# Ejercicio 4:

Ahora vas a analizar cómo puedes resolver el problema anterior. Puntualmente, analiza el siguiente programa del controlador:

Codigo en c++:


String btnState(uint8_t btnState)
{
		if(btnState == HIGH)
		{
				return "OFF";
		}
		else
				return "ON";
}

void task()
{
		enum class TaskStates
		{
		    INIT,
		    WAIT_COMMANDS
	  };
		static TaskStates taskState = TaskStates::INIT;
		constexpr uint8_t led = 25;
		constexpr uint8_t button1Pin = 12;
		constexpr uint8_t button2Pin = 13;
		constexpr uint8_t button3Pin = 32;
		constexpr uint8_t button4Pin = 33;

		switch (taskState)
	  {
				case TaskStates::INIT:
				{
						Serial.begin(115200);
						pinMode(led, OUTPUT);
						digitalWrite(led, LOW);
						pinMode(button1Pin, INPUT_PULLUP);
						pinMode(button2Pin, INPUT_PULLUP);
						pinMode(button3Pin, INPUT_PULLUP);
						pinMode(button4Pin, INPUT_PULLUP);
						taskState = TaskStates::WAIT_COMMANDS;
						break;
				}
				case TaskStates::WAIT_COMMANDS:
				{
						if (Serial.available() > 0)
						{
								String command = Serial.readStringUntil('\n');
								if (command == "ledON")
								{
										digitalWrite(led, HIGH);
								}
								else if (command == "ledOFF")
								{
										digitalWrite(led, LOW);
								}
								else if (command == "readBUTTONS")
							  {
										Serial.print("btn1: ");
						        Serial.print(btnState(digitalRead(button1Pin)).c_str());
						        Serial.print(" btn2: ");
						        Serial.print(btnState(digitalRead(button2Pin)).c_str());
						        Serial.print(" btn3: ");
						        Serial.print(btnState(digitalRead(button3Pin)).c_str());
						        Serial.print(" btn4: ");
						        Serial.print(btnState(digitalRead(button4Pin)).c_str());
						        Serial.print('\n');
					      }
						}
						break;
				}
				default:
				{
						break;
			  }
		}
}

void setup()
{
  task();
}

void loop()
{
  task();
}



Codigo en c#:


using UnityEngine;
using System.IO.Ports;
using TMPro;

enum TaskState
{
    INIT,
    WAIT_COMMANDS
}

public classSerial : MonoBehaviour
{
		private static TaskState taskState = TaskState.INIT;
		private SerialPort _serialPort;
		private byte[] buffer;
		public TextMeshProUGUI myText;
		private int counter = 0;

		void Start()
    {
				_serialPort =new SerialPort();
        _serialPort.PortName = "COM3";
        _serialPort.BaudRate = 115200;
        _serialPort.DtrEnable =true;
        _serialPort.NewLine = "\n";
        _serialPort.Open();
        Debug.Log("Open Serial Port");
        buffer =new byte[128];
    }

		void Update()
    {
        myText.text = counter.ToString();
        counter++;

				switch (taskState)
        {
						case TaskState.INIT:
		            taskState = TaskState.WAIT_COMMANDS;
                Debug.Log("WAIT COMMANDS");
								break;
						case TaskState.WAIT_COMMANDS:
								if (Input.GetKeyDown(KeyCode.A))
                {
		                _serialPort.Write("ledON\n");
                    Debug.Log("Send ledON");
                }
								if (Input.GetKeyDown(KeyCode.S))
                {
                    _serialPort.Write("ledOFF\n");
                    Debug.Log("Send ledOFF");
                }
								if (Input.GetKeyDown(KeyCode.R))
                {
                    _serialPort.Write("readBUTTONS\n");
                    Debug.Log("Send readBUTTONS");
                }
								if (_serialPort.BytesToRead > 0)
                {
                    string response = _serialPort.ReadLine();
                    Debug.Log(response);
                }
								break;
						default:
                Debug.Log("State Error");
								break;
        }
    }
}

## Codigo c# con comentarios

using UnityEngine; // Importa la librería principal de Unity para usar clases y funciones relacionadas con el juego.
using System.IO.Ports; // Importa la librería para manejar comunicaciones a través de puertos serie.
using TMPro; // Importa TextMeshPro para manejo avanzado de texto en Unity.
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters; // Importa elementos de Unity Editor, probablemente innecesario aquí.
using System.Threading.Tasks; // Importa funcionalidades para tareas y programación asíncrona.
using System; // Importa clases básicas del sistema, como tipos de datos y funciones.

enum TaskState // Define un enumerador llamado TaskState para manejar estados de la tarea.
{
    INIT, // Estado inicial.
    WAIT_COMMANDS // Estado donde espera comandos del usuario.
}

public class Serial : MonoBehaviour // Define una clase llamada 'Serial' que hereda de 'MonoBehaviour', lo que la hace un componente de Unity.
{
    private static TaskState taskState = TaskState.INIT; // Declara una variable de estado estática e inicializa en 'INIT'.
    private SerialPort _serialPort; // Declara un objeto para manejar el puerto serie.
    private byte[] buffer; // Declara un buffer para almacenar datos leídos del puerto serie.
    public TextMeshProUGUI myText; // Referencia a un objeto de UI TextMeshProUGUI para mostrar texto en pantalla.
    private int counter = 0; // Declara un contador entero inicializado en 0.

    void Start() // Método Start que se ejecuta al iniciar el script.
    {
        _serialPort = new SerialPort(); // Crea una nueva instancia del puerto serie.
        _serialPort.PortName = "COM4"; // Configura el nombre del puerto (COM4 en este caso).
        _serialPort.BaudRate = 115200; // Configura la tasa de baudios a 115200.
        _serialPort.DtrEnable = true; // Activa el DTR (Data Terminal Ready) del puerto serie.
        _serialPort.NewLine = "\n"; // Configura el carácter de nueva línea.
        _serialPort.Open(); // Abre la conexión con el puerto serie.
        Debug.Log("Open Serial Port"); // Imprime un mensaje en la consola de Unity indicando que el puerto serie se ha abierto.
        buffer = new byte[128]; // Inicializa el buffer con un tamaño de 128 bytes.
    }

    void Update() // Método Update que se ejecuta una vez por fotograma.
    {
        myText.text = counter.ToString(); // Actualiza el texto de 'myText' para mostrar el valor del contador.
        counter++; // Incrementa el contador en 1.

        switch (taskState) // Switch para manejar el estado de la tarea.
        {
            case TaskState.INIT: // Caso cuando el estado es INIT.
                taskState = TaskState.WAIT_COMMANDS; // Cambia el estado a WAIT_COMMANDS.
                Debug.Log("WAIT COMMANDS"); // Imprime un mensaje indicando que está esperando comandos.
                break;

            case TaskState.WAIT_COMMANDS: // Caso cuando el estado es WAIT_COMMANDS.
                if (Input.GetKeyDown(KeyCode.A)) // Si la tecla 'A' es presionada:
                {
                    _serialPort.Write("ledON\n"); // Envía el comando "ledON" al puerto serie.
                    Debug.Log("Send ledON"); // Imprime en consola que se envió "ledON".
                }
                if (Input.GetKeyDown(KeyCode.S)) // Si la tecla 'S' es presionada:
                {
                    _serialPort.Write("ledOFF\n"); // Envía el comando "ledOFF" al puerto serie.
                    Debug.Log("Send ledOFF"); // Imprime en consola que se envió "ledOFF".
                }
                if (Input.GetKeyDown(KeyCode.R)) // Si la tecla 'R' es presionada:
                {
                    _serialPort.Write("readBUTTONS\n"); // Envía el comando "readBUTTONS" al puerto serie.
                    Debug.Log("Send readBUTTONS"); // Imprime en consola que se envió "readBUTTONS".
                }
                if (_serialPort.BytesToRead > 0) // Si hay bytes disponibles para leer del puerto serie:
                {
                    string response = _serialPort.ReadLine(); // Lee una línea del puerto serie.
                    Debug.Log(response); // Imprime la respuesta recibida en la consola.
                }
                break;

            default: // Caso por defecto si el estado no coincide con ninguno de los anteriores.
                Debug.Log("State Error"); // Imprime un mensaje de error en consola indicando que hay un estado no manejado.
                break;
        }
    }
}

## codigo c++ con comentarios 

String btnState(uint8_t btnState) // Función que recibe el estado de un botón y retorna "ON" o "OFF" como string.
{
    if(btnState == HIGH) // Si el estado del botón es HIGH (botón no presionado en configuración INPUT_PULLUP).
    {
        return "OFF"; // Retorna "OFF" indicando que el botón está apagado.
    }
    else // Si el estado del botón no es HIGH (botón presionado).
        return "ON"; // Retorna "ON" indicando que el botón está encendido.
}

void task() // Función que implementa la lógica principal de la máquina de estados.
{
    enum class TaskStates // Define una enumeración para los estados de la tarea.
    {
        INIT, // Estado inicial.
        WAIT_COMMANDS // Estado donde espera recibir comandos.
    };
    static TaskStates taskState = TaskStates::INIT; // Variable estática que mantiene el estado actual, inicializado en INIT.
    constexpr uint8_t led = 25; // Define un pin constante para el LED.
    constexpr uint8_t button1Pin = 12; // Define un pin constante para el botón 1.
    constexpr uint8_t button2Pin = 13; // Define un pin constante para el botón 2.
    constexpr uint8_t button3Pin = 32; // Define un pin constante para el botón 3.
    constexpr uint8_t button4Pin = 33; // Define un pin constante para el botón 4.

    switch (taskState) // Switch para manejar los diferentes estados.
    {
        case TaskStates::INIT: // Caso para el estado INIT.
        {
            Serial.begin(115200); // Inicia la comunicación serie a 115200 baudios.
            pinMode(led, OUTPUT); // Configura el pin del LED como salida.
            digitalWrite(led, LOW); // Apaga el LED inicialmente.
            pinMode(button1Pin, INPUT_PULLUP); // Configura el botón 1 con resistencia interna de pull-up.
            pinMode(button2Pin, INPUT_PULLUP); // Configura el botón 2 con resistencia interna de pull-up.
            pinMode(button3Pin, INPUT_PULLUP); // Configura el botón 3 con resistencia interna de pull-up.
            pinMode(button4Pin, INPUT_PULLUP); // Configura el botón 4 con resistencia interna de pull-up.
            taskState = TaskStates::WAIT_COMMANDS; // Cambia el estado a WAIT_COMMANDS.
            break; // Sale del switch.
        }
        case TaskStates::WAIT_COMMANDS: // Caso para el estado WAIT_COMMANDS.
        {
            if (Serial.available() > 0) // Si hay datos disponibles en el puerto serie.
            {
                String command = Serial.readStringUntil('\n'); // Lee una línea completa del puerto serie.
                if (command == "ledON") // Si el comando es "ledON".
                {
                    digitalWrite(led, HIGH); // Enciende el LED.
                }
                else if (command == "ledOFF") // Si el comando es "ledOFF".
                {
                    digitalWrite(led, LOW); // Apaga el LED.
                }
                else if (command == "readBUTTONS") // Si el comando es "readBUTTONS".
                {
                    Serial.print("btn1: "); // Imprime "btn1: ".
                    Serial.print(btnState(digitalRead(button1Pin)).c_str()); // Imprime el estado del botón 1.
                    Serial.print(" btn2: "); // Imprime " btn2: ".
                    Serial.print(btnState(digitalRead(button2Pin)).c_str()); // Imprime el estado del botón 2.
                    Serial.print(" btn3: "); // Imprime " btn3: ".
                    Serial.print(btnState(digitalRead(button3Pin)).c_str()); // Imprime el estado del botón 3.
                    Serial.print(" btn4: "); // Imprime " btn4: ".
                    Serial.print(btnState(digitalRead(button4Pin)).c_str()); // Imprime el estado del botón 4.
                    Serial.print('\n'); // Imprime una nueva línea.
                }
            }
            break; // Sale del switch.
        }
        default: // Caso por defecto (no debería ocurrir en esta lógica).
        {
            break; // No hace nada, solo sale del switch.
        }
    }
}

void setup() // Función de configuración de Arduino, llamada una vez al inicio.
{
  task(); // Llama a la función task para iniciar la máquina de estados.
}

void loop() // Función principal de Arduino, se ejecuta en un bucle infinito.
{
  task(); // Llama a la función task repetidamente para mantener la máquina de estados activa.
}


# Ejercicio 5:

Repaso.

# Ejercicio 6 Reto bonificación:

## Codigo c#

using UnityEngine;
using System.IO.Ports;
using TMPro;
using System.Text;
using System.Collections;  // Necesario para usar IEnumerator y Corrutinas

public class SerialExample : MonoBehaviour
{
    // Declara un objeto SerialPort para la comunicación serie.
    private SerialPort _serialPort = new SerialPort();

    // Buffer para acumular los datos leídos del puerto serial.
    private StringBuilder messageBuffer = new StringBuilder();

    // Campo para mostrar el contador en la UI (TextMeshPro).
    public TextMeshProUGUI myText;

    // Campo para mostrar los mensajes en la UI (TextMeshPro).
    public TextMeshProUGUI logText;

    // Referencia al objeto cubo 3D en la escena.
    public GameObject myCube;

    // Renderer del cubo para poder cambiar su color.
    private Renderer cubeRenderer;

    // Variable para contar el tiempo o eventos, comienza en 0.
    private static int counter = 0;

    // Método Start: se ejecuta al inicio del programa.
    void Start()
    {
        // Obtiene el componente Renderer del cubo.
        cubeRenderer = myCube.GetComponent<Renderer>();

        // Configura el puerto serie para conectarse al COM4.
        _serialPort.PortName = "COM4";

        // Configura la velocidad de transmisión en baudios.
        _serialPort.BaudRate = 115200;

        // Habilita DTR (Data Terminal Ready).
        _serialPort.DtrEnable = true;

        // Abre el puerto serie.
        _serialPort.Open();

        // Verifica si el puerto se abrió correctamente.
        if (_serialPort.IsOpen)
        {
            Debug.Log("Puerto serie abierto correctamente.");
            logText.text = "Puerto serie abierto correctamente.";  // Muestra mensaje en la UI.
        }
        else
        {
            Debug.Log("Error al abrir el puerto serie.");
            logText.text = "Error al abrir el puerto serie.";  // Muestra error en la UI.
        }
    }

    // Método Update: se ejecuta en cada frame.
    void Update()
    {
        // Actualiza el contador en la UI.
        myText.text = counter.ToString();
        counter++;  // Incrementa el contador.

        // Verifica si hay datos en el puerto serial.
        if (_serialPort.BytesToRead > 0)
        {
            // Lee los datos recibidos del puerto serial.
            string receivedData = _serialPort.ReadExisting();

            // Añade los datos recibidos al buffer.
            messageBuffer.Append(receivedData);

            // Si hay una nueva línea en los datos recibidos.
            if (messageBuffer.ToString().Contains("\n"))
            {
                // Divide el mensaje por cada línea recibida.
                string[] messages = messageBuffer.ToString().Split('\n');

                // Procesa cada mensaje recibido.
                for (int i = 0; i < messages.Length - 1; i++)
                {
                    Debug.Log("Mensaje recibido: " + messages[i]);
                    logText.text = "Mensaje recibido: " + messages[i];  // Muestra el mensaje en la UI.
                    ProcessMessage(messages[i]);  // Procesa el mensaje recibido.
                }

                // Limpia el buffer de mensajes.
                messageBuffer.Clear();

                // Si hay un mensaje incompleto, lo guarda para completarlo en la siguiente lectura.
                if (!string.IsNullOrEmpty(messages[messages.Length - 1]))
                {
                    messageBuffer.Append(messages[messages.Length - 1]);
                }
            }
        }
    }

    // Método para encender el LED (llamado por un botón en Unity).
    public void TurnOnLED()
    {
        // Verifica si el puerto serial está abierto.
        if (_serialPort.IsOpen)
        {
            _serialPort.Write("ON\n");  // Envía el comando "ON" por el puerto serial.
            Debug.Log("Comando para encender el LED enviado.");
            cubeRenderer.material.color = Color.yellow;  // Cambia el color del cubo a amarillo.
            logText.text = "Comando para encender el LED enviado.";  // Actualiza la UI.
        }
        else
        {
            Debug.Log("El puerto serial no está abierto.");
            logText.text = "El puerto serial no está abierto.";  // Muestra error en la UI.
        }
    }

    // Método para apagar el LED (llamado por un botón en Unity).
    public void TurnOffLED()
    {
        // Verifica si el puerto serial está abierto.
        if (_serialPort.IsOpen)
        {
            _serialPort.Write("OFF\n");  // Envía el comando "OFF" por el puerto serial.
            Debug.Log("Comando para apagar el LED enviado.");
            cubeRenderer.material.color = Color.black;  // Cambia el color del cubo a negro.
            logText.text = "Comando para apagar el LED enviado.";  // Actualiza la UI.
        }
        else
        {
            Debug.Log("El puerto serial no está abierto.");
            logText.text = "El puerto serial no está abierto.";  // Muestra error en la UI.
        }
    }

    // Método para leer el estado del controlador (llamado por un botón en Unity).
    public void ReadControllerState()
    {
        // Verifica si el puerto serial está abierto.
        if (_serialPort.IsOpen)
        {
            _serialPort.Write("r\n");  // Envía el comando "r" para leer el estado del controlador.
            Debug.Log("Comando para leer el estado enviado.");
            StartCoroutine(ChangeCubeToMulticolor());  // Inicia la animación multicolor del cubo.
            logText.text = "Comando para leer el estado enviado.";  // Actualiza la UI.
        }
        else
        {
            Debug.Log("El puerto serial no está abierto.");
            logText.text = "El puerto serial no está abierto.";  // Muestra error en la UI.
        }
    }

    // Método para procesar el mensaje recibido del controlador.
    private void ProcessMessage(string message)
    {
        // Divide el mensaje recibido por comas.
        string[] parts = message.Split(',');

        // Si el mensaje contiene dos partes (contador y estado del LED).
        if (parts.Length == 2)
        {
            string contador = parts[0];       // Primera parte del mensaje es el contador.
            string estadoLED = parts[1];      // Segunda parte del mensaje es el estado del LED.

            // Actualiza la UI con el contador y el estado del LED.
            myText.text = "Contador: " + contador + " | LED: " + estadoLED;
        }
        else
        {
            Debug.LogWarning("Mensaje recibido en formato incorrecto: " + message);
        }
    }

    // Corrutina para cambiar el cubo a colores multicolor.
    private IEnumerator ChangeCubeToMulticolor()
    {
        float duration = 2f;  // Duración de la animación.
        float elapsedTime = 0f;

        // Cambia el color del cubo de manera aleatoria durante el tiempo especificado.
        while (elapsedTime < duration)
        {
            cubeRenderer.material.color = new Color(Random.value, Random.value, Random.value);  // Color aleatorio.
            elapsedTime += Time.deltaTime;  // Incrementa el tiempo transcurrido.
            yield return null;  // Espera hasta el siguiente frame.
        }
    }
}

## Codigo c++

void setup() {
    pinMode(LED_BUILTIN, OUTPUT);  // Configura el pin del LED interno como salida
    Serial.begin(115200);          // Configura la velocidad de comunicación serial a 115200 baudios
}

void loop() {
    static bool ledState = false;  // Estado inicial del LED (apagado)

    // Verifica si hay datos disponibles en el puerto serial
    if (Serial.available()) {
        String received = Serial.readStringUntil('\n');  // Lee el mensaje completo hasta '\n'

        // Si el dato recibido es 'r', envía el mensaje en el formato correcto
        if (received == "r") {  // Comando 'read' recibido
            int contador = millis() / 1000;  // Simulación de un contador
            String estadoLED = digitalRead(LED_BUILTIN) == HIGH ? "ON" : "OFF";  // Lee el estado del LED interno
            Serial.print(contador);
            Serial.print(",");
            Serial.println(estadoLED);  // Envía el estado en formato contador,estadoLED
        }
        else if (received == "ON") {
            digitalWrite(LED_BUILTIN, HIGH);  // Enciende el LED
            Serial.println("LED encendido");  // Mensaje de confirmación
        }
        else if (received == "OFF") {
            digitalWrite(LED_BUILTIN, LOW);  // Apaga el LED
            Serial.println("LED apagado");  // Mensaje de confirmación
        }
    }
}

//fin
