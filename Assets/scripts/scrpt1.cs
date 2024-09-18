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






