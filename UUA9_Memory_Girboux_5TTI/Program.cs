using System;
using System.Collections.Generic;
using System.Text;

namespace UUA9_Memory_Girboux_5TTI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(210, 60);

            int nbPaire;
            string Color = "";
            bool gagner = false;

            int continuer = 1;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("(\\__/)\r\n( '-')\r\n/>:");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("<3");
            Console.WriteLine("     Binevenue dans le jeu de carte Memory !");
            Console.ReadLine();
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("(\\__/)\r\n( °.°)\r\n/>:  Combien de paires voulez-vous ?");
            nbPaire = int.Parse(Console.ReadLine());

            while (nbPaire > 20)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("(\\__/)\r\n( >_<)\r\n/>:  Veillez choisir un nombre inférieur à 20 !");
                nbPaire = int.Parse(Console.ReadLine());
            }

            do
            {
                GenerationMatrice2(nbPaire, out int[,] matriceCarte, out int[,] matriceNombre, out int[,] matriceSelect);

                int PointeurLigne = 0;
                int PointeurColonne = 0;

                Console.Clear();
                Console.WriteLine(ConcateneMatrice(matriceCarte, Color, PointeurLigne, PointeurColonne));

                while (!ToutesTrouvees(matriceSelect))
                {
                    int nbRetournees = 0;

                    while (nbRetournees < 2)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\nUtilisez les flèches afin de sélectionner une carte");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Appuyez sur Entrée pour valider...");

                        selectCard(matriceCarte, matriceSelect, ref PointeurLigne, ref PointeurColonne, out Color);

                        if (matriceSelect[PointeurLigne, PointeurColonne] == 1)
                            continue;

                        matriceSelect[PointeurLigne, PointeurColonne] = 2;

                        Console.Clear();
                        Console.WriteLine(RetournerCarte(matriceCarte, matriceSelect, PointeurLigne, PointeurColonne));

                        nbRetournees++;
                    }

                    System.Threading.Thread.Sleep(700);

                    if (CheckPair(matriceCarte, matriceSelect))
                    {
                        for (int i = 0; i < matriceSelect.GetLength(0); i++)
                            for (int j = 0; j < matriceSelect.GetLength(1); j++)
                                if (matriceSelect[i, j] == 2)
                                    matriceSelect[i, j] = 1;
                    }
                    else
                    {
                        for (int i = 0; i < matriceSelect.GetLength(0); i++)
                            for (int j = 0; j < matriceSelect.GetLength(1); j++)
                                if (matriceSelect[i, j] == 2)
                                    matriceSelect[i, j] = 0;
                    }

                    Console.Clear();
                    Console.WriteLine(ConcateneMatrice(matriceCarte, Color, PointeurLigne, PointeurColonne));
                }

                gagner = true;

            } while (gagner == true);
        }

        static string ConcateneMatrice(int[,] Matrice, string Color, int PointeurLigne, int PointeurColonne)
        {
            StringBuilder sb = new StringBuilder();

            string Reset = "\u001b[0m";
            Color = "\u001b[37m";

            var fruits = new Dictionary<int, (string[] Dessin, string Couleur)>
            {
                { 0,  (new[] { " |---| ", " |???| ", " |---| " }, Color) }
            };

            int nbColonnes = Matrice.GetLength(1);
            int nbLignes = Matrice.GetLength(0);

            string ligneSep = Reset + new string('-', nbColonnes * 8 + 1);

            for (int iligne = 0; iligne < nbLignes; iligne++)
            {
                sb.AppendLine(ligneSep);

                for (int iEtage = 0; iEtage < 3; iEtage++)
                {
                    sb.Append(Reset + "|");
                    for (int icolonne = 0; icolonne < nbColonnes; icolonne++)
                    {
                        if (iligne == PointeurLigne && icolonne == PointeurColonne)
                            Color = "\u001b[91m";
                        else
                            Color = "\u001b[37m";

                        sb.Append(Color + fruits[0].Dessin[iEtage].PadRight(7) + Reset + "|");
                    }
                    sb.Append("\n");
                }
            }
            sb.AppendLine(ligneSep);

            return sb.ToString();
        }

        static void GenerationMatrice2(int nbPaire, out int[,] matriceCarte, out int[,] matriceNombre, out int[,] matriceSelect)
        {
            int totalCartes = nbPaire * 2;

            int lignes = (int)Math.Sqrt(totalCartes);
            while (totalCartes % lignes != 0)
            {
                lignes--;
            }
            int colonnes = totalCartes / lignes;

            matriceCarte = new int[lignes, colonnes];
            matriceSelect = new int[lignes, colonnes];

            matriceNombre = new int[nbPaire, 2];
            Random alea = new Random();

            for (int iNombre = 0; iNombre < nbPaire; iNombre++)
            {
                matriceNombre[iNombre, 0] = iNombre + 1;
                matriceNombre[iNombre, 1] = iNombre + 1;
            }

            for (int iLigne = 0; iLigne < matriceCarte.GetLength(0); iLigne++)
            {
                for (int iColonne = 0; iColonne < matriceCarte.GetLength(1); iColonne++)
                {
                    matriceCarte[iLigne, iColonne] = 0;
                    matriceSelect[iLigne, iColonne] = 0;
                }
            }

            for (int iLigne = 0; iLigne < matriceNombre.GetLength(0); iLigne++)
            {
                for (int iColonne = 0; iColonne < matriceNombre.GetLength(1); iColonne++)
                {
                    int nbr1;
                    int nbr2;
                    do
                    {
                        nbr1 = alea.Next(0, matriceCarte.GetLength(0));
                        nbr2 = alea.Next(0, matriceCarte.GetLength(1));

                    } while (matriceCarte[nbr1, nbr2] != 0);

                    matriceCarte[nbr1, nbr2] = matriceNombre[iLigne, iColonne];
                }
            }
        }

        static void selectCard(int[,] matriceCarte, int[,] matriceSelect, ref int PointeurLigne, ref int PointeurColonne, out string Color)
        {
            bool continuer = true;
            Color = "";

            while (continuer)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (PointeurLigne != 0)
                        {
                            PointeurLigne--;
                            Console.Clear();
                            Console.WriteLine(ConcateneMatrice(matriceCarte, Color, PointeurLigne, PointeurColonne));
                        }
                        break;

                    case ConsoleKey.DownArrow:
                        if (PointeurLigne != matriceCarte.GetLength(0) - 1)
                        {
                            PointeurLigne++;
                            Console.Clear();
                            Console.WriteLine(ConcateneMatrice(matriceCarte, Color, PointeurLigne, PointeurColonne));
                        }
                        break;

                    case ConsoleKey.LeftArrow:
                        if (PointeurColonne != 0)
                        {
                            PointeurColonne--;
                            Console.Clear();
                            Console.WriteLine(ConcateneMatrice(matriceCarte, Color, PointeurLigne, PointeurColonne));
                        }
                        break;

                    case ConsoleKey.RightArrow:
                        if (PointeurColonne != matriceCarte.GetLength(1) - 1)
                        {
                            PointeurColonne++;
                            Console.Clear();
                            Console.WriteLine(ConcateneMatrice(matriceCarte, Color, PointeurLigne, PointeurColonne));
                        }
                        break;

                    case ConsoleKey.Enter:
                        if (matriceSelect[PointeurLigne, PointeurColonne] != 1)
                            continuer = false;
                        break;
                }
            }
        }

        static string RetournerCarte(int[,] Matrice, int[,] matriceSelect, int PointeurLigne, int PointeurColonne)
        {
            StringBuilder sb = new StringBuilder();

            string Reset = "\u001b[0m";
            string Rouge = "\u001b[91m";
            string Vert = "\u001b[92m";
            string Jaune = "\u001b[93m";
            string Bleu = "\u001b[94m";
            string Rose = "\u001b[95m";
            string Cyan = "\u001b[96m";
            string Gris = "\u001b[37m";

            var fruits = new Dictionary<int, (string[] Dessin, string Couleur)>
            {
                { 0,  (new[] { " |---| ", " |???| ", " |---| " }, Gris) },
                { 1,  (new[] { "  _(_  ", " (   ) ", "  `-'  " }, Rouge) },
                { 2,  (new[] { "  _ _  ", " (_(_ )", "  (_ ) " }, Rose)  },
                { 3,  (new[] { "  ___  ", " /   \\ ", " `---' " }, Jaune) },
                { 4,  (new[] { "   |   ", "  / \\  ", "  \\_/  " }, Jaune) },
                { 5,  (new[] { "  \\|/  ", "  -O-  ", "  /|\\  " }, Cyan)  },
                { 6,  (new[] { "   ^   ", "  / \\  ", " /___\\ " }, Vert)  },
                { 7,  (new[] { "  _ _  ", " ( v ) ", "  \\ /  " }, Rouge) },
                { 8,  (new[] { "   /   ", "  |    ", "   \\   " }, Jaune) },
                { 9,  (new[] { "  o o  ", "  | |  ", "  J L  " }, Rouge) },
                { 10, (new[] { "  ---  ", " |   | ", "  ---  " }, Vert)  },
                { 11, (new[] { "  /\\   ", " /  \\  ", " \\__/  " }, Rouge) },
                { 12, (new[] { "  { }  ", "  { }  ", "  { }  " }, Jaune) },
                { 13, (new[] { "  _|_  ", " |   | ", " \\___/ " }, Bleu)  },
                { 14, (new[] { "  . .  ", " (   ) ", "  '-'  " }, Rouge) },
                { 15, (new[] { "  _~_  ", " (   ) ", "  \\_/  " }, Rose)  },
                { 16, (new[] { "  [ ]  ", "  [ ]  ", "  [ ]  " }, Vert)  },
                { 17, (new[] { "  * * ", "  *X* ", "  * * " }, Rose)  },
                { 18, (new[] { "  /=\\  ", " ( = ) ", "  \\=/  " }, Vert)  },
                { 19, (new[] { "  ( )  ", " ( @ ) ", "  ( )  " }, Gris)  },
                { 20, (new[] { "  _|_  ", " /_|_\\ ", " (___) " }, Rouge) }
            };

            int nbColonnes = Matrice.GetLength(1);
            int nbLignes = Matrice.GetLength(0);

            string ligneSep = Reset + new string('-', nbColonnes * 8 + 1);

            for (int iligne = 0; iligne < nbLignes; iligne++)
            {
                sb.AppendLine(ligneSep);

                for (int iEtage = 0; iEtage < 3; iEtage++)
                {
                    sb.Append(Reset + "|");
                    for (int icolonne = 0; icolonne < nbColonnes; icolonne++)
                    {
                        int id = Matrice[iligne, icolonne];

                        if (matriceSelect[iligne, icolonne] > 0)
                        {
                            var item = fruits[id];
                            sb.Append(item.Couleur + item.Dessin[iEtage].PadRight(7) + Reset + "|");
                        }
                        else
                        {
                            var item = fruits[0];
                            sb.Append(item.Couleur + item.Dessin[iEtage].PadRight(7) + Reset + "|");
                        }
                    }
                    sb.Append("\n");
                }
            }
            sb.AppendLine(ligneSep);

            return sb.ToString();
        }

        static bool CheckPair(int[,] matriceCarte, int[,] matriceSelect)
        {
            List<int> valeurs = new List<int>();

            for (int i = 0; i < matriceCarte.GetLength(0); i++)
                for (int j = 0; j < matriceCarte.GetLength(1); j++)
                    if (matriceSelect[i, j] == 2)
                        valeurs.Add(matriceCarte[i, j]);

            return valeurs.Count == 2 && valeurs[0] == valeurs[1];
        }

        static bool ToutesTrouvees(int[,] matriceSelect)
        {
            foreach (int v in matriceSelect)
                if (v != 1) return false;
            return true;
        }
    }
}
