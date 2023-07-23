using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GA_SalesmanProblem
{
    public class Comisvoiajor
    {
        public double FMut;//frecventa mutatiilor
        public double FCros;//frecventa crosingoverului
        public int Populatie = 100;//marime populatiei
        public int Norase = 10;//num orase
        public int[,] Pop = new int[1000, 100]; //populatie genomiala
        public double[] PopFitness = new double[1000]; //cromozomi
        public double[,] OrDist = new double[100, 100];//distante intre orase
        public int[,] OrCoordonate = new int[100, 2];//coordonatele x,y

        public Comisvoiajor(int nor, int pop, double fmut, double fcros)//constructor cu generarea oraselor
        {
            Norase = nor; Populatie = pop; FMut = fmut; FCros = fcros;
            genOrase();
            genPopulatie();
        }

        public Comisvoiajor(Comisvoiajor cv ,int pop, double fmut, double fcros)//constructor pentru copiere 
        {
            Norase = cv.Norase;
            Populatie = pop; FMut = fmut; FCros = fcros;
           
            //copiem coordonatele oraselor
            for (int i = 0; i < Norase; i++)
            {
                OrCoordonate[i, 0] = cv.OrCoordonate[i, 0];
                OrCoordonate[i, 1] = cv.OrCoordonate[i, 1];
            }

            //coppiem distantele intre orase
            for (int i = 0; i < Norase - 1; i++)
            {
                for (int j = i + 1; j < Norase; j++)
                    OrDist[i, j] = OrDist[j, i] = cv.OrDist[i, j];
                OrDist[i, i] = 0;
            }
            //generarea populatiei
            genPopulatie(); 
        }

        private void genOrase()
        {
            Random r = new Random((int)DateTime.Now.Ticks);
            //generam coordonatele oraselor
            for (int i = 0; i < Norase; i++)
            {
                OrCoordonate[i, 0] = r.Next(300);
                OrCoordonate[i, 1] = r.Next(300);
            }

            //calculam distantele intre orase
            for (int i = 0; i < Norase - 1; i++)
            {
                for (int j = i + 1; j < Norase; j++)
                    OrDist[i, j] = OrDist[j, i] = Math.Sqrt(Math.Pow(OrCoordonate[i, 0] - OrCoordonate[j, 0], 2) + Math.Pow(OrCoordonate[i, 1] - OrCoordonate[j, 1], 2));
                OrDist[i, i] = 0;
            }
        }

        private void genPopulatie()
        {
            Random r = new Random((int)DateTime.Now.Ticks);
            int x, marker;
            //generarea populatiei
            for (int i = 0; i < Populatie; i++)
            {
                for (int j = 0; j < Norase; j++)
                {
                    do
                    {
                        marker = 1;
                        x = r.Next(Norase);
                        for (int k = 0; k < j; k++)
                            if (x == Pop[i, k]) marker = 0;
                    }
                    while (marker == 0);
                    Pop[i, j] = x;
                }

            }
            //calcularea fitnesului
            for (int u = 0; u < Populatie; u++)
                PopFitness[u] = perform(u);
        }
        
        //mutatie cromozomului
        public void mutatie()
        {
            int n, m, p, x, marker, j;
            Random r = new Random((int)DateTime.Now.Ticks);
            int nummut = Convert.ToInt32(FMut / 100 * Populatie);
            for (int i = 0; i < nummut; i++)
            {
                do
                {
                    marker = 1;
                    n = r.Next(Populatie);
                    m = r.Next(Norase);
                    p = r.Next(Norase);
                    x = Pop[n, m]; Pop[n, m] = Pop[n, p]; Pop[n, p] = x;
                    for (j = 1; j < Norase; j++)
                        if (OrDist[j - 1, j] == 0)
                        {
                            marker = 0;
                            x = Pop[n, m]; Pop[n, m] = Pop[n, p]; Pop[n, p] = x;
                        }

                }
                while (marker == 0);
                PopFitness[n] = perform(n);
            }
        }

        //efectuarea crosingoverului
        public void crosingover()
        {
            int m, n, k, j, p, marker;
            Random r = new Random((int)DateTime.Now.Ticks);
            int var = Convert.ToInt32(FCros * Populatie / 100);
            for (int i = Populatie - var; i < Populatie - var / 2; i++)
            {
                do
                {
                salt:
                    marker = 1;
                    m = r.Next(var);
                    n = r.Next(var);
                    k = -1;
                    do
                    {
                        k++;
                    }
                    while ((Pop[m, k] != Pop[n, k]) && (k < Norase - 1));
                    if (k == Norase - 1) marker = 0;
                    else
                    {   
                        for (j = 0; j <= k; j++)
                            Pop[i, j] = Pop[m, j];
                        for (j = k + 1; j < Norase; j++)
                            Pop[i, j] = Pop[n, j];
                        for (j = 0; j < Norase - 1; j++)
                            for (p = j + 1; p < Norase ; p++)
                                if (Pop[i, j] == Pop[i, p]) goto salt;

                        for (j = 0; j <= k; j++)
                            Pop[i + var / 2, j] = Pop[n, j];
                        for (j = k + 1; j < Norase; j++)
                            Pop[i + var / 2, j] = Pop[m, j];
                        for (j = 0; j < Norase - 1; j++)
                            for (p = j + 1; p < Norase ; p++)
                                if (Pop[i + var / 2, j] == Pop[i + var / 2, p]) goto salt;
                        
                    }
                }
                while (marker == 0);
                PopFitness[i] = perform(i);
                PopFitness[i + var / 2] = perform(i + var / 2);
            }
        }

        //schimb de cromozomi
        public void schimb(int i, int j)
        {
            int x;
            for (int k = 0; k < Norase; k++)
            {
                x = Pop[i, k];
                Pop[i, k] = Pop[j, k];
                Pop[j, k] = x;
            }
        }

        //functia cu fitnes
        double perform(int i)
        {
            int j;
            double s = 0;
            for (j = 1; j < Norase; j++)
                s += OrDist[Pop[i, j - 1], Pop[i, j]];
            s += OrDist[Pop[i, 0], Pop[i, Norase - 1]];
            return s;
        }

        //ordonare crescatoare a populatiei
        public void ordonare() 
        {
            int i, j, k;
            i = 0;
            while (i < Populatie - 1)
            {
                j = i + 1;
                k = i;
                while (j < Populatie)
                {
                    if (PopFitness [j] < PopFitness[k])
                        k = j;
                    j++;
                }
                if (i != k)
                    schimb(i, k);
                i++;
            }
        }

    }

}