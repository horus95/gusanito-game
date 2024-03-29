﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
/*
| 38 | (Arriba) |
| 40 | (Abajo) |
| 37 | (Izqierda) |
| 39 | (Derecha) |
 Nv 1 = 5
 Nv 2 = 10
 Nv 3 = 15
 Nv 4 = 20
 */

namespace Gusanito_V1._0
{
    public partial class Form1 : Form
    {
        int control,enter=0,control_mov=0, time=250,tiempo_original=250,puntos=0,limite_nv=20,min_niv=5;
        Point [,]Mapa_pixele;
        List<PictureBox> Gusanito;
        PictureBox bajon;
        MapPoints pixel = new MapPoints();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Crear_Mapa();
            this.initialBuild();
            timer.Interval =250;
            timer.Enabled = true;
            timer.Start();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyValue)
            {
                case 38:
                    if (control_mov != 4)
                    {
                        control = 38;
                        control_mov = 2;
                    }
                    break;
                case 40:
                    if (control_mov != 2)
                    {
                        control = 40;
                        control_mov = 4;
                    }
                    break;
                case 37:
                    if (control_mov != 3)
                    {
                        control = 37;
                        control_mov = 1;
                    }
                    break;
                case 39:
                    if (control_mov != 1)
                    {
                        control = 39;
                        control_mov = 3;
                    }
                    break;
                case 13:
                    control = 13;
                    if (enter == 0)
                    {
                        enter = 1;
                        Pause();
                        control_mov = 0;
                    }
                    else
                        enter = 0;
                    break;
            }
        }

        private void Crear_Mapa()
        {
            Mapa_pixele = new Point[16,16];
            int x=0,y=0;
            for(int i=0; i <= 15 ; i++)
            {
                for (int j = 0; j <= 15; j++)
                    Mapa_pixele[i,j] = new Point(x,y);
                x += 20;
                y += 20;
            }       
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Mover_Gusanito();     
        }

        public void MoveWorm(string image, int X, int Y)
        {
            Gusanito[0].Image = Image.FromFile(Application.StartupPath.ToString() + "\\imagenes\\" + image);
            Point ultimo = Gusanito[Gusanito.Count - 1].Location;
            acomodar_piesas();
            Gusanito[0].Location = new Point(X, Y);
            if (bajon.Location == new Point(X, Y))
            {
                agregar_nodo(ultimo);
                comida_ramdon();
                if (puntos >= min_niv && min_niv <= limite_nv)
                {
                    time -= 60;
                    timer.Interval = time;
                    min_niv += 5;
                }
            }
            if (validar_crash())
                Perdiste();
        }

        private void Mover_Gusanito()
        {
            int X,Y;
            X = Gusanito[0].Location.X;
            Y = Gusanito[0].Location.Y;
            switch (control)
            {
                case 38:
                    Y -= 20;
                    if (Y >= 0)
                        MoveWorm("arriba.jpg", X, Y);
                    else
                        Perdiste();
                    break;
                case 40:
                    Y += 20;
                    if (Y <= 300)
                        MoveWorm("abajo.jpg", X, Y);
                    else
                        Perdiste();
                    break;
                case 37:
                    X -= 20;
                    if (X >= 0)
                        MoveWorm("izquierda.jpg", X, Y);
                    else
                        Perdiste();
                    break;
                case 39:
                    X += 20;
                    if (X <= 300)
                        MoveWorm("derecha.jpg", X, Y);
                    else
                        Perdiste();
                    break;
            }
        }

       public void Pause()
       {
           if (enter == 1)
           {
               timer.Dispose();
               MessageBox.Show("PAUSA", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
               control = 0;
               timer.Start();
           }
           else
               timer.Start();
       }

        private void Panel_Paint(object sender, PaintEventArgs e)
        {

        }

        public void Perdiste()
        {
            timer.Dispose();
            MessageBox.Show("Perdiste...", "",MessageBoxButtons.OK,MessageBoxIcon.Error);
            Gusanito[0].Location = Mapa_pixele[7, 7];
            enter = -1;
            control = 0;
            destruir();
            puntos = 0;
            min_niv = 5;
            time = tiempo_original;
            control_mov = 0;
            timer.Interval = tiempo_original;
            timer.Start();
        }
    
        public void agregar_nodo(Point x)
        {
            puntos++;
            PictureBox nuevo_nodo = new PictureBox();
            nuevo_nodo.Size = new Size(20,20);
            nuevo_nodo.Name = "Nodo_cuerpo" + Gusanito.Count;
            nuevo_nodo.SizeMode = PictureBoxSizeMode.StretchImage;
            nuevo_nodo.Image = Image.FromFile(Application.StartupPath.ToString() + "\\imagenes\\cuerpo.jpg");
            nuevo_nodo.Location = x;
            Panel.Controls.Add(nuevo_nodo);
            Gusanito.Add(nuevo_nodo);
            pointsLabel.Text = puntos.ToString();
        }

        public void acomodar_piesas()
        {
            List<PictureBox> copia = Gusanito;
            int y = Gusanito.Count - 2;
            for(int i = Gusanito.Count - 1; i >0 ;i--)
            {
                Gusanito[i].Location = copia[y].Location;
                y--;
            }
        }

        public void comida_ramdon()
        {
            Random randon;
            randon = new Random();
            int x, y,X,Y;
            X = bajon.Location.X;
            Y = bajon.Location.Y;
            x=X;
            y=Y;
            while(x==X && y==Y)
            {
                x = randon.Next(0,15);
                y = randon.Next(0, 15);
            }
            bajon.Location = Mapa_pixele[x,y];
        }

        public void destruir()
        {
            Panel.Controls.Clear();
            this.initialBuild();
        }

        public void initialBuild()
        {
            Gusanito = new List<PictureBox>();
            var headImage = Image.FromFile(Application.StartupPath.ToString() + "\\imagenes\\izquierda.jpg"); ;
            PictureBox Head = new PictureBox
            {
                Size = new Size(20, 20),
                Visible = true,
                Name = "Head",
                Image = headImage,
                SizeMode = PictureBoxSizeMode.StretchImage,
                BorderStyle = BorderStyle.None,
                Location = Mapa_pixele[7, 7]
            };
            pixel.set(7, 7);
            bajon = new PictureBox
            {
                Name = "Comidad",
                Size = new Size(20, 20),
                SizeMode = PictureBoxSizeMode.StretchImage,
                BorderStyle = BorderStyle.None,
                Image = Image.FromFile(Application.StartupPath.ToString() + "\\imagenes\\comida.jpg")
            };
            comida_ramdon();
            Panel.Controls.Add(bajon);
            Panel.Controls.Add(Head);
            Gusanito.Add(Head);
            pointsLabel.Text = puntos.ToString();
        }

        public bool validar_crash()
        {
            for (int i = 1; i < Gusanito.Count - 1; i++)
                if (Gusanito[0].Location == Gusanito[i].Location)
                    return true;
            return false;

        }
    }
}
