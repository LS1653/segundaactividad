Ejercicio 1: comunicación entre computador y controlador

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

Ejercicio 2: experimento

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

public class Serial : MonoBehaviour
{
    private SerialPort _serialPort = new SerialPort();
    private StringBuilder messageBuffer = new StringBuilder(); // Buffer para acumular los bytes leídos

    public TextMeshProUGUI myText;

    private static int counter = 0;

    void Start()
    {
        _serialPort.PortName = "COM4"; // Configura el puerto serial
        _serialPort.BaudRate = 115200; // Velocidad de transmisión
        _serialPort.DtrEnable = true;  // Habilita el DTR
        _serialPort.Open(); // Abre el puerto serial
        Debug.Log("Open Serial Port");
    }

    void Update()
    {
        myText.text = counter.ToString(); // Actualiza el contador en pantalla
        counter++;

        if (Input.GetKeyDown(KeyCode.A)) // Si se presiona la tecla 'A'
        {
            byte[] data = { 0x31 }; // Envía el byte '1' al puerto serial
            _serialPort.Write(data, 0, 1);
        }

        if (_serialPort.BytesToRead > 0) // Si hay datos disponibles para leer
        {
            string receivedData = _serialPort.ReadExisting(); // Lee todos los datos disponibles como string
            messageBuffer.Append(receivedData); // Acumula los datos en el buffer

            // Verifica si el buffer contiene el delimitador '\n' (fin del mensaje)
            if (messageBuffer.ToString().Contains("\n"))
            {
                // Procesa cada mensaje que termina con el delimitador
                string[] messages = messageBuffer.ToString().Split('\n'); // Divide el buffer en mensajes

                // Procesa todos los mensajes completos recibidos
                for (int i = 0; i < messages.Length - 1; i++)
                {
                    Debug.Log("Mensaje recibido: " + messages[i]);
                }

                // Borra el buffer hasta el último mensaje incompleto (si existe)
                messageBuffer.Clear();
                if (!string.IsNullOrEmpty(messages[messages.Length - 1]))
                {
                    messageBuffer.Append(messages[messages.Length - 1]); // Guarda el mensaje incompleto
                }
            }
        }
    }
}

