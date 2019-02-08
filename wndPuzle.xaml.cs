using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace puzle_esdeveniments
{

    /// <summary>
    /// Lógica de interacción para wndPuzle.xaml
    /// </summary>
    /// 
    public struct Coordenades
    {
        public int fila;
        public int columna;
        public bool ordenat;
    }
    public partial class wndPuzle : Window
    {
        Random rand = new Random();
        private TextBlock blanc;
        List<TextBlock> textList;
        int moviments;
        TextBlock[,] matriu;
        int totalFiles;
        int totalColumnes;
        Coordenades cordActual;
        int ordenats;

        


        public wndPuzle(int files, int columnes)
        {
            InitializeComponent();

            ordenats = 0;
            moviments = 0;
            ugdGraella.Rows = files;
            ugdGraella.Columns = columnes;
            totalFiles = files;
            totalColumnes = columnes;
            List<int>llista=GenerarLlista(rand,files*columnes-1);


            CrearPuzle(llista,files,columnes);

            EmplenarMatriu(files, columnes);

            if (ordenats == totalColumnes * totalFiles - 1)
                JocGuanyat();
           
            
        }

        private void CrearPuzle(List<int> llista, int files, int columnes)
        {
            textList = new List<TextBlock>();
            
            int contador = 1;
            int fila = 0, columna = 0;
            Coordenades coord;
             TextBlock txb;
             
            foreach (int n in llista)
            {
                txb = CrearTextBlock(n.ToString(), contador);
                contador++;
                coord.columna = columna;
                coord.fila = fila;
                coord.ordenat = false;

                txb.Tag = coord;

                columna++;
                if (columna >= columnes)
                {
                    columna = 0;
                    fila++;

                }

                textList.Add(txb);

                
            }

            blanc = CrearTextBlock("", contador);
            
            coord.columna = columna;
            coord.fila = fila;
            coord.ordenat = false;
            blanc.Tag = coord;

            

            textList.Add(blanc);
            foreach(TextBlock t in textList)
            {
                ugdGraella.Children.Add(t);
            }

           

           


        }

        private void EmplenarMatriu(int files, int columnes)
        {
            int i=0, j=0, k;
            matriu = new TextBlock[files,columnes];
            k = 0;
            for (i = 0; i < matriu.GetLength(0); i++)
            {
                for (j = 0; j < matriu.GetLength(1); j++)
                {
                    matriu[i, j] = textList[k];
                    CanviarColors(i,j);
                    k++;
                }
            }
            matriu[i - 1, j - 1].Background = new SolidColorBrush(Colors.White);
        }

        private TextBlock CrearTextBlock(string num,int contador)
        {
            

            TextBlock txb = new TextBlock();
            txb.Text = num;
            
            txb.Margin = new Thickness(5);
            txb.MinWidth = 30;
            txb.MinHeight = 30;
            txb.TextAlignment = TextAlignment.Center;
            txb.Padding = new Thickness(10);
            txb.Name = "pice" + contador.ToString();

            

            return txb;
        }

        private List<int> GenerarLlista( Random r, int nelem)
        {
            List<int> llista = new List<int>();
            List<int> desordenada = new List<int>();
            int pos1, i;
           

            for (i = 0; i < nelem; i++)
                llista.Add(i + 1);

            while(llista.Count>0)
            {
                pos1 = r.Next(llista.Count);
                desordenada.Add(llista[pos1]);
                llista.Remove(llista[pos1]);
              
            }

            desordenada = DesordresCorrectes(desordenada);

            return desordenada;

        }

        private List<int> DesordresCorrectes(List<int> desordenada)
        {
            int numDesordres = 0;
            int i, j;
            int aux;
            for (i = 0; i < desordenada.Count; i++)
            {
                for (j = i + 1; j < desordenada.Count; j++)
                {
                    if (desordenada[i] > desordenada[j])
                        numDesordres++;
                    
                }

            }
           
            if (numDesordres % 2 != 0)
            {
                aux = desordenada[desordenada.Count - 1];
                desordenada[desordenada.Count - 1] = desordenada[desordenada.Count - 2];
                desordenada[desordenada.Count - 2] = aux;
            }


            return desordenada;
        }

       
    

        private void Moviment(object sender, MouseButtonEventArgs e)
        {
            TextBlock actiu;
            
            Coordenades cordBlanc;
            
           

            if (e.Source is TextBlock)
            {
                actiu = (TextBlock)e.Source;

                cordActual = (Coordenades)actiu.Tag;
                cordBlanc = (Coordenades)blanc.Tag;

                if (cordBlanc.columna == cordActual.columna&&cordBlanc.fila!=cordActual.fila)
                {
                    if (cordBlanc.fila > cordActual.fila)
                        MoureFilesAvall(cordActual.fila, cordActual.columna, cordBlanc.fila);
                    else if (cordBlanc.fila < cordActual.fila)
                        MoureFilesAmunt(cordBlanc.fila, cordBlanc.columna, cordActual.fila);
                }
                else if (cordBlanc.fila == cordActual.fila && cordActual.columna != cordBlanc.columna)
                {
                    if (cordBlanc.columna > cordActual.columna)
                        MoureColumnesDreta(cordActual.columna, cordActual.fila, cordBlanc.columna);
                    else if (cordBlanc.columna < cordActual.columna)
                        MoureColumnesEsquerra(cordBlanc.columna, cordBlanc.fila, cordActual.columna);
                }

                if (ordenats==totalColumnes*totalFiles-1) 
                    JocGuanyat();

            }
  
        }

        private void MoureFilesAmunt(int inici, int col, int final)
        {
            int i;
            string aux;

            aux = matriu[inici+1, col].Text;
            for (i = inici; i<final; i++)
            {


                matriu[i, col].Text = matriu[i + 1, col].Text;
                CanviarColors(i,col);
                moviments++;
            }
            matriu[inici, col].Text = aux;
            matriu[final, col].Text = "";
            txbMoviments.Text = moviments.ToString();
            blanc = matriu[i, col];
            blanc.Background = new SolidColorBrush(Colors.White);


        }
        private void MoureFilesAvall(int inici, int col, int final)
        {
            int i;
            string aux;

            aux = matriu[final-1, col].Text;
            for(i=final;i>inici;i--)
            {
               

                matriu[i, col].Text = matriu[i-1, col].Text;
                CanviarColors(i, col);
                moviments++;
            }
            matriu[final, col].Text = aux;
            matriu[inici, col].Text = "";
            txbMoviments.Text = moviments.ToString();
            blanc = matriu[i, col];
            blanc.Background = new SolidColorBrush(Colors.White);

            
        }

        private void MoureColumnesDreta(int inici, int fil, int final)
        {
            int i;
            string aux;

            aux = matriu[fil, final-1].Text;
            for (i = final; i > inici; i--)
            {


                matriu[fil, i].Text = matriu[fil, i-1].Text;
                CanviarColors(fil, i);
                moviments++;
            }
            matriu[fil, final].Text = aux;
            matriu[fil,inici].Text = "";
            txbMoviments.Text = moviments.ToString();
            blanc = matriu[fil, i];
            blanc.Background = new SolidColorBrush(Colors.White);
        }

        private void MoureColumnesEsquerra(int inici, int fil, int final)
        {
            int i;
            string aux;

            aux = matriu[fil, inici+1].Text;
            for (i = inici; i < final; i++)
            {


                matriu[fil, i].Text = matriu[fil, i+1].Text;
                CanviarColors(fil, i);
                moviments++;
            }
            matriu[fil, inici].Text = aux;
            matriu[fil, final].Text = "";
            txbMoviments.Text = moviments.ToString();
            blanc = matriu[fil, i];
            blanc.Background = new SolidColorBrush(Colors.White);
        }


        private void CanviarColors(int fil, int col)
        {
            Coordenades nova;

            int posicio = fil * totalColumnes + col + 1;
            if (matriu[fil, col].Text == posicio.ToString())
            {
                if (!((Coordenades)matriu[fil, col].Tag).ordenat)
                    ordenats++;
                nova.ordenat = true;
                nova.columna = col;
                nova.fila = fil;

                matriu[fil, col].Tag = nova;
                matriu[fil, col].Background = new SolidColorBrush(Colors.LightGreen);
                
            }
            else
            {
                if (((Coordenades)matriu[fil, col].Tag).ordenat)
                    ordenats--;
                nova.ordenat = false;
                nova.columna = col;
                nova.fila = fil;

                matriu[fil, col].Tag = nova;
                matriu[fil, col].Background = new SolidColorBrush(Colors.Tomato);
                
            }


        }


        private void JocGuanyat()
        {
            ugdGraella.MouseUp -= Moviment;
            wndPuz.KeyDown -= MovTecles;
            MessageBox.Show("Has guanyat!!!\nNum de Moviments: " + moviments);
        }

        private void MovTecles(object sender, KeyEventArgs e)
        {


            Coordenades cordBlanc;


            cordBlanc = (Coordenades)blanc.Tag;



            if (e.Key == Key.Right)
            {
                cordActual = (Coordenades)matriu[cordBlanc.fila, cordBlanc.columna ].Tag;
                cordActual.columna -= 1;
                if (cordActual.columna >= 0&& cordActual.columna!=cordBlanc.columna)
                    MoureColumnesDreta(cordActual.columna, cordActual.fila, cordBlanc.columna);
            }
            else if (e.Key == Key.Up)
            {
                cordActual = (Coordenades)matriu[cordBlanc.fila, cordBlanc.columna].Tag;
                cordActual.fila += 1;
                if (cordActual.fila < totalFiles)
                    MoureFilesAmunt(cordBlanc.fila, cordBlanc.columna, cordActual.fila);
            }
            else if (e.Key == Key.Down)
            {
                cordActual = (Coordenades)matriu[cordBlanc.fila, cordBlanc.columna].Tag;
                cordActual.fila -= 1;
                if (cordActual.fila >= 0)
                    MoureFilesAvall(cordActual.fila, cordActual.columna, cordBlanc.fila);
            }
            else if (e.Key == Key.Left)
            {
                cordActual = (Coordenades)matriu[cordBlanc.fila, cordBlanc.columna].Tag;
                cordActual.columna += 1;
                if (cordActual.columna < totalColumnes)
                    MoureColumnesEsquerra(cordBlanc.columna, cordBlanc.fila, cordActual.columna);

            }

            if (ordenats == totalColumnes * totalFiles - 1)
                JocGuanyat();

        }

        private void wndPuz_Loaded(object sender, RoutedEventArgs e)
        {

        }



   
    }
}
