using System;
using System.Collections.Generic;
using System.IO;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Dialogs;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace WindowsFormEnAvalonia;

public partial class MainWindow : Window
{
    private static List<Coche> lista = new List<Coche>(); //Lista principal

    static int contador = 0; //Variable para almacenar el numero de coches.
    static int actual = 0; //Variable que contiene la posicion del elemento actual.
    Boolean modifyMode = false; //Variable para comprobar si estamos en modificar o en añadir.
    public MainWindow()
    {
        InitializeComponent();
        
        leerArchivo();
        
        //Los add posteriores están comentados para que se ejecute el programa con los datos del archivo.
        //En caso de querer ejecutar con los add, quitarle los comentarios y comentar la función de arriba llamada leerArchivo().
        
        //aniadirElemento("1234DMF", "Nissan", true, (float)2500.99, 20, "C:\\Users\\David\\RiderProjects\\WindowsFormEnAvalonia\\WindowsFormEnAvalonia\\Imagenes\\nissan.jpg");
        //aniadirElemento("4567PPS", "Volvo", false, (float)4750.50, 15, "C:\\Users\\David\\RiderProjects\\WindowsFormEnAvalonia\\WindowsFormEnAvalonia\\Imagenes\\volvo.jpg");
        //aniadirElemento("5643DVZ", "Volskwagen", true, (float)4000.52, 17, "C:\\Users\\David\\RiderProjects\\WindowsFormEnAvalonia\\WindowsFormEnAvalonia\\Imagenes\\Volsk.jpg");
        //aniadirElemento("2472AND", "Ferrari", false, (float)200000, 4, "C:\\Users\\David\\RiderProjects\\WindowsFormEnAvalonia\\WindowsFormEnAvalonia\\Imagenes\\ferrari.jpg");
        
        mostrarElementos(actual);
        btnAnterior.IsEnabled = false;
        btnAceptar.IsEnabled = false;
        btnCancelar.IsEnabled = false;
        lblRutaFoto.IsVisible = false;
        txtRutaFoto.IsVisible = false;

        this.CanResize = false; //Para que el tamaño de la ventana del programa no pueda ser modificado.
        this.Closed += MainWindow_Closed; //Función a realizar cuando se cierre la ventana del programa.
    }
    
    private void MainWindow_Closed(object? sender, EventArgs e)
    {
        // Función que se ejecutará al cerrar la ventana principal
        escribirArchivo();
    }
    private void Cancelar_Click(object sender, RoutedEventArgs e)
    {
        mostrarElementos(actual);
        
        //Control de botones y etiquetas
        
        btnAceptar.IsEnabled = false;
        btnCancelar.IsEnabled = false;
        if (isFirst())
        {
            btnAnterior.IsEnabled = false;
        }
        else
        {
            btnAnterior.IsEnabled = true;
        }

        if (isLast())
        {
            btnSiguiente.IsEnabled = false;
        }
        else
        {
            btnSiguiente.IsEnabled = true;
        }

        btnAniadir.IsEnabled = true;
        btnModificar.IsEnabled = true;
        btnBorrar.IsEnabled = true;
        lblRutaFoto.IsVisible = false;
        txtRutaFoto.IsVisible = false;
        txtPrimeraLetra.IsEnabled = true;
        modifyMode = false;
    }
    
    private void Aniadir_Click(object sender, RoutedEventArgs e)
    {
        vaciarDatos();
        
        //Control de botones y etiquetas
        
        btnAceptar.IsEnabled = true;
        btnCancelar.IsEnabled = true;
        btnSiguiente.IsEnabled = false;
        btnAnterior.IsEnabled = false;
        btnAniadir.IsEnabled = false;
        btnModificar.IsEnabled = false;
        btnBorrar.IsEnabled = false;
        lblRutaFoto.IsVisible = true;
        txtRutaFoto.IsVisible = true;
        txtPrimeraLetra.IsEnabled = false;
        txtAniosEdad.IsEnabled = true;
        txtMarca.IsEnabled = true;
        txtPrecio.IsEnabled = true;
        txtMatricula.IsEnabled = true;
        txtParticular.IsEnabled = true;
        txtAniosEdad.IsEnabled = true;
    }
    
    private void Aceptar_Click(object sender, RoutedEventArgs e)
    {
        if (todoVacio())
        {
            var customDialog = new CustomDialog("Por favor, rellene los campos vacíos");
            customDialog.ShowDialog(this);
        }
        else if (!todoVacio())
        {
            if (!modifyMode) //Comprobamos que no estemos modificando
            {
                if (txtRutaFoto.Text == "")
                {
                    aniadirElemento(txtMatricula.Text, txtMarca.Text, Boolean.Parse(txtParticular.Text), float.Parse(txtPrecio.Text), int.Parse(txtAniosEdad.Text), "C:\\Users\\David\\RiderProjects\\WindowsFormEnAvalonia\\WindowsFormEnAvalonia\\Imagenes\\pordefecto.png");
                }
                else
                {
                    aniadirElemento(txtMatricula.Text, txtMarca.Text, Boolean.Parse(txtParticular.Text), float.Parse(txtPrecio.Text), int.Parse(txtAniosEdad.Text), txtRutaFoto.Text);
                }

            }
            else //Si estamos modificando, sobreescribimos los valores del elemento por los nuevos
            {
                lista[actual].matricula = txtMatricula.Text;
                lista[actual].particular = Boolean.Parse(txtParticular.Text);
                lista[actual].marca = txtMarca.Text;
                lista[actual].precio = float.Parse(txtPrecio.Text);
                lista[actual].aniosEdad = int.Parse(txtAniosEdad.Text);
                if (txtRutaFoto.Text == "")
                {
                    Bitmap imagen =
                        new Bitmap("C:\\Users\\David\\RiderProjects\\WindowsFormEnAvalonia\\WindowsFormEnAvalonia\\Imagenes\\pordefecto.png");
                    lista[actual].fotoCoche = imagen;
                }
                else
                {
                    Bitmap imagen = new Bitmap(txtRutaFoto.Text);
                    lista[actual].fotoCoche = imagen;
                }
            }
        }
        

        mostrarElementos(actual);
        btnAceptar.IsEnabled = false;
        btnCancelar.IsEnabled = false;

//Comprobaciones de los botones

        if (isFirst())
        {
            btnAnterior.IsEnabled = false;
        }
        else
        {
            btnAnterior.IsEnabled = true;
        }
        if (isLast())
        {
            btnSiguiente.IsEnabled = false;
        }
        else
        {
            btnSiguiente.IsEnabled = true;
        }
        btnAniadir.IsEnabled = true;
        btnModificar.IsEnabled = true;
        btnBorrar.IsEnabled = true;
        lblRutaFoto.IsVisible = false;
        txtRutaFoto.IsVisible = false;
        txtPrimeraLetra.IsEnabled = true;
        modifyMode = false;
    }
    
    private void Borrar_Click(object sender, RoutedEventArgs e)
    {
        if (isFirst())
        {
            btnAnterior.IsEnabled = false;
        }
        else
        {
            btnAnterior.IsEnabled = true;
        }
        if (isLast())
        {
            btnSiguiente.IsEnabled = false;
        }
        else
        {
            btnSiguiente.IsEnabled = true;
        }

        borrarElemento();
        
    }
    
    private void Anterior_Click(object sender, RoutedEventArgs e)
    {
        retroceder();
    }
    
    private void Siguiente_Click(object sender, RoutedEventArgs e)
    {
        avanzar();
    }
    
    private void Modificar_Click(object sender, RoutedEventArgs e)
    {
        modifyMode = true; //Activamos el boolean para indicar que vamos a modificar
        modificar();
    }
    
    public void mostrarElementos(int actual)
    {
        //Comprobamos si el numero introducido no supera el tamaño de la lista o
        //si en la lista hay elementos.
        if (lista.Count > 0 && actual >= 0 && actual < lista.Count)
        {
            //Intoducimos los valores en sus respectivos campos.
            txtMatricula.Text = "" + lista[actual].matricula;
            txtMarca.Text = "" + lista[actual].marca;
            txtAniosEdad.Text = "" + lista[actual].aniosEdad;
            txtPrecio.Text = "" + lista[actual].precio;
            txtPrimeraLetra.Text = "" + lista[actual].primeraLetraMatricula;
            txtParticular.Text = "" + lista[actual].particular;
            pbFotoCoche.Source = lista[actual].fotoCoche;
            txtRutaFoto.Text = "" + lista[actual].rutaFoto;
        }
        else
        {
            //Mostramos mensaje de error
            var customDialog = new CustomDialog("Error");
            customDialog.ShowDialog(this);
        }
    }
    
    public void leerArchivo()
    {
        string nombreArchivo = "C:\\Users\\David\\RiderProjects\\WindowsFormEnAvalonia\\WindowsFormEnAvalonia\\databank.data"; //Nombre del fichero con los datos

        try
        {
            if (File.Exists(nombreArchivo))
            {
                // Limpia la lista antes de leer nuevos datos
                lista.Clear();

                using (StreamReader reader = new StreamReader(nombreArchivo))//Abrimos archivo
                {
                    while (!reader.EndOfStream)//Recorremos hasta el final del fichero
                    {
                        string line = reader.ReadLine(); //Guardamos una linea
                        string[] datos = line.Split('-'); //Dividimos la linea a partir de '-' y guardamos cada parte en una posicion del vector
                    
                        // Comprobamos que tenemos los datos requeridos para crear un nuevo registro de coche
                        if (datos.Length == 6)
                        {
                            //Guardamos los datos en cada variable
                            string matricula = datos[0];
                            string marca = datos[1];
                            bool particular = bool.Parse(datos[2]);
                            float precio = float.Parse(datos[3]);
                            int anios = int.Parse(datos[4]);
                            string nombreFoto = datos[5];

                            //Añadimos los datos recogidos a la lista
                            aniadirElemento(matricula, marca, particular, precio, anios, nombreFoto);
                        }
                    }
                }

                // Establecer actual en 0 después de cargar los datos
                actual = 0;

                // Mostrar el primer elemento después de cargar los datos
                mostrarElementos(actual);
            }
        }
        catch (Exception ex) //Si hay algun problema de lectura con el archivo, indicamos por pantalla el fallo.
        {
            var customDialog = new CustomDialog("Error al leer el archivo");
            customDialog.ShowDialog(this);
        }
    }
    
    public void escribirArchivo()
    {
        // Nombre del archivo
        string nombreArchivo = "C:\\Users\\David\\RiderProjects\\WindowsFormEnAvalonia\\WindowsFormEnAvalonia\\databank.data";

        try
        {
            using (StreamWriter writer = new StreamWriter(nombreArchivo))
            {
                foreach (Coche coche in lista) //Recorremos por elementos la lista.
                {
                    //Lo escribimos todo siguiendo el formato siguiente:
                    // Formato de cada línea: matricula-marca-particular-precio-anios-nombreFoto
                    string linea = $"{coche.matricula}-{coche.marca}-{coche.particular}-{coche.precio}-{coche.aniosEdad}-{coche.rutaFoto}";
                    writer.WriteLine(linea);
                }
            }
        }
        catch (Exception ex)//Mostramos un error, en el caso de que lo haya, al intentar escribir en el fichero.
        {
            var customDialog = new CustomDialog("Error al escribir el archivo");
            customDialog.ShowDialog(this);
        }
    }
    
    public void aniadirElemento(string matricula, string marca, Boolean particular, float precio, int anios, String rutaFoto)
    {
        // Cargar la imagen desde la ruta
        Bitmap imagenBitmap = new Bitmap(rutaFoto);

        // Añadir un nuevo elemento a la lista
        lista.Add(new Coche(matricula, marca, particular, precio, anios, imagenBitmap, rutaFoto));

        // Incrementar el contador
        contador++;
    }
    
    private Boolean isLast() //Método para comprobar si nos encontramos en el último elemento de la lista.
    {
        if (actual == lista.Count - 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private Boolean isFirst() //Método para comprobar si nos encontramos en el primer elemento de la lista.
    {
        if (actual == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    private void avanzar() //Método para avanzar al siguiente elemento
    {
        if (actual < lista.Count - 1) //Importante comprobar que actual no sea mayor que el tamaño de la lista
        {
            actual++; //Sumando 1 a actual, avanzamos al siguiente indice
        
            //Comprobaciones de botones
            btnAnterior.IsEnabled = true;
            if (isLast())
            {
                btnSiguiente.IsEnabled = false;
            }
            mostrarElementos(actual);
        }
    }
    
    private void retroceder() //Método para retroceder al anterior elemento
    {
        if (actual > 0) //Comprobamos que actual sea un número válido para el índice de la lista
        {
            actual--; //Restando 1 a actual, volvemos al anterior indice

            //Comprobaciones de botones
            btnSiguiente.IsEnabled = true;
            if (isFirst())
            {
                btnAnterior.IsEnabled = false;
            }
            mostrarElementos(actual);
        }
    }
    
    public void vaciarDatos() //Vaciamos todos los campos con este método
    {
        txtMatricula.Clear();
        txtMarca.Clear();
        txtParticular.Clear();
        txtPrecio.Clear();
        txtPrimeraLetra.Clear();
        txtAniosEdad.Clear();
        txtRutaFoto.Clear();

        Bitmap imagen =new Bitmap("C:\\Users\\David\\RiderProjects\\WindowsFormEnAvalonia\\WindowsFormEnAvalonia\\Imagenes\\pordefecto.png");
        pbFotoCoche.Source = imagen;

    }
    
    public void modificar() //Método para modificar el estado de los botones cuando vayamos a modificar un elemento
    {
        btnAceptar.IsEnabled = true;
        btnCancelar.IsEnabled = true;
        btnSiguiente.IsEnabled = false;
        btnAnterior.IsEnabled = false;
        btnAniadir.IsEnabled = false;
        btnModificar.IsEnabled = false;
        btnBorrar.IsEnabled = false;
        lblRutaFoto.IsVisible = true;
        txtRutaFoto.IsVisible = true;
        txtPrimeraLetra.IsEnabled = false;
    }
    
    public void borrarElemento() //Método para borrar un elemento
    {
        if (contador > 2) //Comprobamos que haya algún elemento en la lista
        {
            lista.RemoveAt(actual);
            actual = 0; //Siempre volvemos a mostrar el elemento inicial para evitar fallos
            contador--;
            mostrarElementos(actual);
            btnAnterior.IsEnabled = false;
            btnSiguiente.IsEnabled = true;
        }
        else if (contador == 2)
        {
            lista.RemoveAt(actual);
            actual = 0; //Siempre volvemos a mostrar el elemento inicial para evitar fallos
            contador--;
            mostrarElementos(actual);
            btnAnterior.IsEnabled = false;
            btnSiguiente.IsEnabled = false;
        }
        else if (contador == 1) //Si queda uno, lo bottamos y dejamos los campos deshabilitados y vacíos. Sólo se podría añadir
        {
            lista.RemoveAt(actual);
            actual = 0;
            contador = 0;

            vaciarDatos();
            btnAceptar.IsEnabled = false;
            btnCancelar.IsEnabled = false;
            btnSiguiente.IsEnabled = false;
            btnAnterior.IsEnabled = false;
            btnAniadir.IsEnabled = true;
            btnModificar.IsEnabled = false;
            btnBorrar.IsEnabled = false;

            txtAniosEdad.IsEnabled = false;
            txtMarca.IsEnabled = false;
            txtPrecio.IsEnabled = false;
            txtPrimeraLetra.IsEnabled = false;
            txtMatricula.IsEnabled = false;
            txtParticular.IsEnabled = false;
            txtAniosEdad.IsEnabled = false;
        }

    }

    public Boolean todoVacio()
    {
        if (txtMarca.Text == "" || txtPrecio.Text == "" || txtMatricula.Text == "" || txtParticular.Text == "" || txtAniosEdad.Text == "")
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}