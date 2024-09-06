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
