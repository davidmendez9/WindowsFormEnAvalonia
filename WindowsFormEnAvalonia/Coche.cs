using System;

using Avalonia.Media;

namespace WindowsFormEnAvalonia;

public class Coche
{
    public string matricula { get; set; }
    public char primeraLetraMatricula { get; set; } //Contiene la primera letra de la matricula
    public string marca { get; set; }
    public Boolean particular { get; set; } //Indica si el coche es de un particular o no
    public float precio { get; set; }
    public int aniosEdad { get; set; } //Indica cuantos años tiene el coche desde que se matriculó por primera vez
    public IImage fotoCoche { get; set; } //Imagen del coche
    public string rutaFoto { get; set; }

//Constructor de la clase Coche
    public Coche(string matricula, string marca, bool particular, float precio, int aniosEdad, IImage fotoCoche, string rutaFoto)
    {
        this.matricula = matricula;
        this.primeraLetraMatricula = matricula[4];
        this.marca = marca;
        this.particular = particular;
        this.precio = precio;
        this.aniosEdad = aniosEdad;
        this.fotoCoche = fotoCoche;
        this.rutaFoto = rutaFoto;
    }

}