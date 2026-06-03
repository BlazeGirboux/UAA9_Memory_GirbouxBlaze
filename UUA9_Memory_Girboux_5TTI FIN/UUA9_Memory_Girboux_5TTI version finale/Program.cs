using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace UUA9_Memory_Girboux_5TTI
{
    internal class Program
    {

        // Fonction pour dessiner une box
        static string[] MakeBoxLines(string title, string[] lines)
        {
            int width = 38; // largeur interne
            string top = "┌" + new string('─', width) + "┐";
            string bottom = "└" + new string('─', width) + "┘";

            List<string> result = new List<string>();
            result.Add(top);

            int pad = (width - title.Length) / 2;
            string titleLine = "│" + new string(' ', pad) + title + new string(' ', width - pad - title.Length) + "│";
            result.Add(titleLine);

            result.Add("├" + new string('─', width) + "┤");

            foreach (var line in lines)
            {
                string content = line.Length > width - 1 ? line.Substring(0, width - 1) : line;
                result.Add("│ " + content.PadRight(width - 1) + "│");
            }

            result.Add(bottom);
            return result.ToArray();
        }

        static void PrintBoxesRow(params string[][] boxes)
        {
            int maxHeight = boxes.Max(b => b.Length);

            for (int i = 0; i < maxHeight; i++)
            {
                foreach (var box in boxes)
                {
                    string line = i < box.Length ? box[i] : new string(' ', box[0].Length);
                    Console.Write(line + "   "); // espace entre les box
                }
                Console.WriteLine();
            }
        }


        static void Main(string[] args)
        {
            string fichierStats = "stats.txt";

            // === 26 STATS ===
            double tempsCumule = 0;
            int nbParties = 0;
            int nbVictoires = 0;
            int nbDefaites = 0;
            int nbNuls = 0;
            int nbPairesTrouvees = 0;
            int nbCartesRetournees = 0;
            int meilleurScoreJ1 = 0;
            int meilleurScoreAdv = 0;
            int scoreTotalJ1 = 0;
            int scoreTotalAdv = 0;
            double partiePlusLongue = 0;
            double partiePlusCourte = double.MaxValue;
            int vicAmi = 0, defAmi = 0;
            int vicFacile = 0, defFacile = 0;
            int vicMoyen = 0, defMoyen = 0;
            int vicHard = 0, defHard = 0;
            int toursTotal = 0;
            int debutJoueur = 0;
            int debutRobot = 0;
            int pairesJoueur = 0;
            int pairesRobot = 0;
            int cartesJoueur = 0;
            int cartesRobot = 0;

            // LECTURE DU FICHIER
            if (File.Exists(fichierStats))
            {
                string contenu = File.ReadAllText(fichierStats);
                string[] data = contenu.Split(';');

                // RESET PROPRE SI PAS 26 VALEURS
                if (data.Length == 26)
                {
                    double.TryParse(data[0], out tempsCumule);
                    int.TryParse(data[1], out nbParties);
                    int.TryParse(data[2], out nbVictoires);
                    int.TryParse(data[3], out nbDefaites);
                    int.TryParse(data[4], out nbNuls);
                    int.TryParse(data[5], out nbPairesTrouvees);
                    int.TryParse(data[6], out nbCartesRetournees);
                    int.TryParse(data[7], out meilleurScoreJ1);
                    int.TryParse(data[8], out meilleurScoreAdv);
                    int.TryParse(data[9], out scoreTotalJ1);
                    int.TryParse(data[10], out scoreTotalAdv);
                    double.TryParse(data[11], out partiePlusLongue);
                    double.TryParse(data[12], out partiePlusCourte);
                    int.TryParse(data[13], out vicAmi);
                    int.TryParse(data[14], out defAmi);
                    int.TryParse(data[15], out vicFacile);
                    int.TryParse(data[16], out defFacile);
                    int.TryParse(data[17], out vicMoyen);
                    int.TryParse(data[18], out defMoyen);
                    int.TryParse(data[19], out vicHard);
                    int.TryParse(data[20], out defHard);
                    int.TryParse(data[21], out toursTotal);
                    int.TryParse(data[22], out debutJoueur);
                    int.TryParse(data[23], out debutRobot);
                    int.TryParse(data[24], out pairesJoueur);
                    int.TryParse(data[25], out pairesRobot);
                }
            }

            Stopwatch session = Stopwatch.StartNew();

            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            bool rejouer = true;
            double nouveauTotal;
            int nbPaire;

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("");
            Console.WriteLine(@" __  __                                          ");
            System.Threading.Thread.Sleep(500);
            Console.WriteLine(@"|  \/  | ___ _ __ ___   ___  _ __ _   _          ");
            System.Threading.Thread.Sleep(500);
            Console.WriteLine(@"| |\/| |/ _ \ '_ ` _ \ / _ \| '__| | | |         ");
            System.Threading.Thread.Sleep(500);
            Console.WriteLine(@"| |  | |  __/ | | | | | (_) | |  | |_| |         ");
            System.Threading.Thread.Sleep(500);
            Console.WriteLine(@"|_|  |_|\___|_| |_| |_|\___/|_|   \__, |         ");
            System.Threading.Thread.Sleep(500);
            Console.WriteLine(@"                                  |___/          ");
            System.Threading.Thread.Sleep(1000);
            Console.Write("(\\__/)\r\n( '-')\r\n/>:");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("<3");
            Console.WriteLine("      Bienvenue dans le jeu de carte Memory !");
            Console.ReadLine();

            while (rejouer)
            {
                int scoreJoueur1 = 0;
                int scoreJoueur2 = 0;
                int tourActuel = 1;
                int choixMode = 1;

                bool choixValide = false;
                while (!choixValide)
                {
                    Console.Clear();

                    // Titre
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("══════════════════════════════════════");
                    Console.WriteLine("        CHOISISSEZ VOTRE ADVERSAIRE");
                    Console.WriteLine("══════════════════════════════════════\n");

                    // Options
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(" 1. "); Console.ForegroundColor = ConsoleColor.Cyan; Console.WriteLine("Un ami");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(" 2. "); Console.ForegroundColor = ConsoleColor.Green; Console.WriteLine("Robot Facile");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(" 3. "); Console.ForegroundColor = ConsoleColor.Yellow; Console.WriteLine("Robot Moyen");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(" 4. "); Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("Robot Hard");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(" 5. "); Console.ForegroundColor = ConsoleColor.Gray; Console.WriteLine("Statistiques");

                    // Reset
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("\n══════════════════════════════════════");
                    Console.Write("Votre choix : ");


                    if (!int.TryParse(Console.ReadLine(), out choixMode))
                        continue;

                    if (choixMode == 5)
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("\n=========== STATISTIQUES COMPLETES ===========\n");
                        Console.ForegroundColor = ConsoleColor.White;

                        // TEMPS
                        var boxTemps = MakeBoxLines(
                            "Temps",
                            new[]
                            {
                            $"Temps total : {TimeSpan.FromSeconds(tempsCumule):hh\\:mm\\:ss}",
                            $"Plus longue : {partiePlusLongue:F1}s",
                            $"Plus courte : {(partiePlusCourte == double.MaxValue ? 0 : partiePlusCourte):F1}s",
                            $"Temps moyen : {(nbParties > 0 ? tempsCumule / nbParties : 0):F1}s"
                            }
                        );

                        // PARTIES
                        var boxParties = MakeBoxLines(
                            "Parties",
                            new[]
                            {
                            $"Jouées : {nbParties}",
                            $"Tours total : {toursTotal}",
                            $"Début joueur : {debutJoueur}",
                            $"Début robot : {debutRobot}"
                            }
                        );

                        // SCORES
                        var boxScores = MakeBoxLines(
                            "Scores",
                            new[]
                            {
                            $"Victoires : {nbVictoires}",
                            $"Défaites : {nbDefaites}",
                            $"Nuls : {nbNuls}",
                            $"Winrate : {(nbParties > 0 ? (nbVictoires * 100.0 / nbParties) : 0):F1}%"
                            }
                        );

                        // SCORES DÉTAILLÉS
                        var boxScoresDet = MakeBoxLines(
                            "Scores détaillés",
                            new[]
                            {
                            $"Meilleur J1 : {meilleurScoreJ1}",
                            $"Meilleur Adv : {meilleurScoreAdv}",
                            $"Total J1 : {scoreTotalJ1}",
                            $"Total Adv : {scoreTotalAdv}",
                            $"Moyenne J1 : {(nbParties > 0 ? (double)scoreTotalJ1 / nbParties : 0):F2}",
                            $"Moyenne Adv : {(nbParties > 0 ? (double)scoreTotalAdv / nbParties : 0):F2}"
                            }
                        );

                        // PAIRES & CARTES
                        var boxPaires = MakeBoxLines(
                            "Paires & Cartes",
                            new[]
                            {
                            $"Paires J1 : {pairesJoueur}",
                            $"Paires Adv : {pairesRobot}",
                            $"Total paires : {nbPairesTrouvees}",
                            $"Cartes J1 : {cartesJoueur}",
                            $"Cartes Adv : {cartesRobot}",
                            $"Total cartes : {nbCartesRetournees}"
                            }
                        );

                        // PAR ADVERSAIRE
                        var boxAdv = MakeBoxLines(
                            "Par adversaire",
                            new[]
                            {
                                $"Ami : {vicAmi} / {defAmi}",
                                $"Facile : {vicFacile} / {defFacile}",
                                $"Moyen : {vicMoyen} / {defMoyen}",
                                $"Hard : {vicHard} / {defHard}"
                            }
                        );

                        // AFFICHAGE : 3 lignes de 2 box
                        PrintBoxesRow(boxTemps, boxParties);
                        Console.WriteLine();
                        PrintBoxesRow(boxScores, boxScoresDet);
                        Console.WriteLine();
                        PrintBoxesRow(boxPaires, boxAdv);

                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("Appuyez sur Entrée pour revenir.");
                        Console.ReadLine();

                        continue;
                    }


                    else
                    {
                        choixValide = true;
                    }

                    // Choix du nombre de paires
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("(\\__/)\r\n( °.°)\r\n/>:   Combien de paires voulez-vous ?");

                    while (!int.TryParse(Console.ReadLine(), out nbPaire) || nbPaire > 20 || nbPaire < 1)
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("(\\__/)\r\n( >_<)\r\n/>:   Veuillez choisir un nombre entre 1 et 20 !");
                    }

                    // Génération du plateau
                    GenerationMatrice2(nbPaire, out int[,] matriceCarte, out int[,] matriceNombre, out int[,] matriceEtat);

                    int[,] memoireRobot = new int[matriceCarte.GetLength(0), matriceCarte.GetLength(1)];
                    for (int iL = 0; iL < memoireRobot.GetLength(0); iL++)
                        for (int iC = 0; iC < memoireRobot.GetLength(1); iC++)
                            memoireRobot[iL, iC] = -1;

                    int PointeurLigne = 0;
                    int PointeurColonne = 0;

                    // Stat partie
                    Stopwatch partieTimer = Stopwatch.StartNew();
                    int toursPartie = 0;
                    int cartesJ1Partie = 0;
                    int cartesAdvPartie = 0;
                    int pairesJ1Partie = 0;
                    int pairesAdvPartie = 0;

                    if (tourActuel == 1) debutJoueur++;
                    else debutRobot++;

                    // Boucle principale
                    while (!ToutesTrouvees(matriceEtat))
                    {
                        int nbRetournees = 0;

                        while (nbRetournees < 2)
                        {
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine("SCORE J1 : " + scoreJoueur1 + " | ADVERSAIRE : " + scoreJoueur2);
                            Console.WriteLine("Tour du joueur n°" + tourActuel);

                            Console.WriteLine(RetournerCarte(matriceCarte, matriceEtat, PointeurLigne, PointeurColonne));

                            if (tourActuel == 2 && choixMode > 1)
                            {
                                System.Threading.Thread.Sleep(1000);
                                RobotJoue(matriceCarte, matriceEtat, memoireRobot, choixMode, nbRetournees, ref PointeurLigne, ref PointeurColonne);
                                cartesAdvPartie++;
                            }
                            else
                            {
                                ChoisirCarte(matriceCarte, matriceEtat, ref PointeurLigne, ref PointeurColonne);
                                cartesJ1Partie++;
                            }

                            matriceEtat[PointeurLigne, PointeurColonne] = 2;
                            memoireRobot[PointeurLigne, PointeurColonne] = matriceCarte[PointeurLigne, PointeurColonne];
                            nbRetournees++;

                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine("SCORE J1 : " + scoreJoueur1 + " | ADVERSAIRE : " + scoreJoueur2);
                            Console.WriteLine("Tour du joueur n°" + tourActuel);
                            Console.WriteLine(RetournerCarte(matriceCarte, matriceEtat, PointeurLigne, PointeurColonne));
                        }

                        System.Threading.Thread.Sleep(1000);

                        if (VerifierPaire(matriceCarte, matriceEtat))
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("BRAVO ! Une paire !");
                            if (tourActuel == 1)
                            {
                                scoreJoueur1++;
                                pairesJ1Partie++;
                            }
                            else
                            {
                                scoreJoueur2++;
                                pairesAdvPartie++;
                            }
                            System.Threading.Thread.Sleep(800);
                            ChangerEtatCartes(matriceEtat, 2, 1);
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("DOMMAGE... Ce n'est pas une paire.");
                            System.Threading.Thread.Sleep(800);
                            ChangerEtatCartes(matriceEtat, 2, 0);
                            tourActuel = (tourActuel == 1 ? 2 : 1);
                        }

                        toursPartie++;
                    }

                    // Fin de partie
                    partieTimer.Stop();
                    double duree = partieTimer.Elapsed.TotalSeconds;

                    nbParties++;
                    tempsCumule += duree;
                    toursTotal += toursPartie;
                    nbCartesRetournees += cartesJ1Partie + cartesAdvPartie;
                    cartesJoueur += cartesJ1Partie;
                    cartesRobot += cartesAdvPartie;
                    pairesJoueur += pairesJ1Partie;
                    pairesRobot += pairesAdvPartie;
                    nbPairesTrouvees += pairesJ1Partie;

                    if (duree > partiePlusLongue) partiePlusLongue = duree;
                    if (duree < partiePlusCourte) partiePlusCourte = duree;

                    scoreTotalJ1 += scoreJoueur1;
                    scoreTotalAdv += scoreJoueur2;

                    if (scoreJoueur1 > meilleurScoreJ1) meilleurScoreJ1 = scoreJoueur1;
                    if (scoreJoueur2 > meilleurScoreAdv) meilleurScoreAdv = scoreJoueur2;

                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("\n======================================");

                    if (scoreJoueur1 > scoreJoueur2)
                    {
                        Console.WriteLine("VICTOIRE DU JOUEUR 1 !");
                        nbVictoires++;

                        if (choixMode == 1) vicAmi++;
                        if (choixMode == 2) vicFacile++;
                        if (choixMode == 3) vicMoyen++;
                        if (choixMode == 4) vicHard++;
                    }
                    else if (scoreJoueur2 > scoreJoueur1)
                    {
                        Console.WriteLine("VICTOIRE DE L'ADVERSAIRE !");
                        nbDefaites++;

                        if (choixMode == 1) defAmi++;
                        if (choixMode == 2) defFacile++;
                        if (choixMode == 3) defMoyen++;
                        if (choixMode == 4) defHard++;
                    }
                    else
                    {
                        Console.WriteLine("MATCH NUL !");
                        nbNuls++;
                    }

                    Console.WriteLine("======================================");

                    // === SAUVEGARDE DES 26 STATS ===
                    File.WriteAllText(fichierStats,
                        $"{tempsCumule};" +
                        $"{nbParties};" +
                        $"{nbVictoires};" +
                        $"{nbDefaites};" +
                        $"{nbNuls};" +
                        $"{nbPairesTrouvees};" +
                        $"{nbCartesRetournees};" +
                        $"{meilleurScoreJ1};" +
                        $"{meilleurScoreAdv};" +
                        $"{scoreTotalJ1};" +
                        $"{scoreTotalAdv};" +
                        $"{partiePlusLongue};" +
                        $"{partiePlusCourte};" +
                        $"{vicAmi};" +
                        $"{defAmi};" +
                        $"{vicFacile};" +
                        $"{defFacile};" +
                        $"{vicMoyen};" +
                        $"{defMoyen};" +
                        $"{vicHard};" +
                        $"{defHard};" +
                        $"{toursTotal};" +
                        $"{debutJoueur};" +
                        $"{debutRobot};" +
                        $"{pairesJoueur};" +
                        $"{pairesRobot}"
                    );

                    // Rejouer ?
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Voulez-vous recommencer ? (o/n)");
                    string choix = Console.ReadLine().ToLower();
                    if (choix == "o") rejouer = true;
                    else rejouer = false;
                }

                // Fin de session
                session.Stop();
                nouveauTotal = tempsCumule + session.Elapsed.TotalSeconds;

                // Sauvegarde finale
                File.WriteAllText(fichierStats,
                    $"{nouveauTotal};" +
                    $"{nbParties};" +
                    $"{nbVictoires};" +
                    $"{nbDefaites};" +
                    $"{nbNuls};" +
                    $"{nbPairesTrouvees};" +
                    $"{nbCartesRetournees};" +
                    $"{meilleurScoreJ1};" +
                    $"{meilleurScoreAdv};" +
                    $"{scoreTotalJ1};" +
                    $"{scoreTotalAdv};" +
                    $"{partiePlusLongue};" +
                    $"{partiePlusCourte};" +
                    $"{vicAmi};" +
                    $"{defAmi};" +
                    $"{vicFacile};" +
                    $"{defFacile};" +
                    $"{vicMoyen};" +
                    $"{defMoyen};" +
                    $"{vicHard};" +
                    $"{defHard};" +
                    $"{toursTotal};" +
                    $"{debutJoueur};" +
                    $"{debutRobot};" +
                    $"{pairesJoueur};" +
                    $"{pairesRobot}"
                );
            }




            static void RobotJoue(int[,] mCarte, int[,] mEtat, int[,] mMem, int mode, int nbCartes, ref int rLigne, ref int rCol)
            {
                Random alea = new Random();
                int maxL = mCarte.GetLength(0);
                int maxC = mCarte.GetLength(1);

                // Mode Moyen = une chance sur deux de rater
                if (mode == 3 && alea.Next(0, 2) == 0)
                {
                    do { rLigne = alea.Next(0, maxL); rCol = alea.Next(0, maxC); } while (mEtat[rLigne, rCol] != 0);
                    return;
                }

                if (mode >= 3)
                {
                    if (nbCartes == 0) // robot cherche s'il connaît une paire
                    {
                        for (int l1 = 0; l1 < maxL; l1++)
                            for (int c1 = 0; c1 < maxC; c1++)
                                if (mMem[l1, c1] != -1 && mEtat[l1, c1] == 0)
                                {
                                    for (int l2 = 0; l2 < maxL; l2++)
                                        for (int c2 = 0; c2 < maxC; c2++)
                                            if ((l1 != l2 || c1 != c2) && mMem[l2, c2] == mMem[l1, c1] && mEtat[l2, c2] == 0)
                                            {
                                                rLigne = l1; rCol = c1; return;
                                            }
                                }
                    }
                    else //cherche la paire de la carte déjà retournée
                    {
                        int valVisible = -1;
                        for (int l = 0; l < maxL; l++)
                            for (int c = 0; c < maxC; c++)
                                if (mEtat[l, c] == 2) valVisible = mCarte[l, c];

                        for (int l = 0; l < maxL; l++)
                            for (int c = 0; c < maxC; c++)
                                if (mMem[l, c] == valVisible && mEtat[l, c] == 0)
                                {
                                    rLigne = l; rCol = c; return;
                                }
                    }
                }

                // Si rien trouvé ou mode facile : hasard
                do { rLigne = alea.Next(0, maxL); rCol = alea.Next(0, maxC); } while (mEtat[rLigne, rCol] != 0);
            }

            static void ChoisirCarte(int[,] matriceCarte, int[,] matriceEtat, ref int PointeurLigne, ref int PointeurColonne)
            {
                bool selectionEnCours = true;
                while (selectionEnCours)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.UpArrow:
                            if (PointeurLigne > 0) PointeurLigne--;
                            break;
                        case ConsoleKey.DownArrow:
                            if (PointeurLigne < matriceCarte.GetLength(0) - 1) PointeurLigne++;
                            break;
                        case ConsoleKey.LeftArrow:
                            if (PointeurColonne > 0) PointeurColonne--;
                            break;
                        case ConsoleKey.RightArrow:
                            if (PointeurColonne < matriceCarte.GetLength(1) - 1) PointeurColonne++;
                            break;
                        case ConsoleKey.Enter:
                            if (matriceEtat[PointeurLigne, PointeurColonne] == 0)
                                selectionEnCours = false;
                            break;
                    }
                    Console.Clear();
                    Console.WriteLine(RetournerCarte(matriceCarte, matriceEtat, PointeurLigne, PointeurColonne));
                }
            }

            static void ChangerEtatCartes(int[,] matriceEtat, int ancienEtat, int nouvelEtat)
            {
                for (int iLigne = 0; iLigne < matriceEtat.GetLength(0); iLigne++)
                    for (int iColonne = 0; iColonne < matriceEtat.GetLength(1); iColonne++)
                        if (matriceEtat[iLigne, iColonne] == ancienEtat)
                            matriceEtat[iLigne, iColonne] = nouvelEtat;
            }

            static string RetournerCarte(int[,] Matrice, int[,] matriceEtat, int PointeurLigne, int PointeurColonne)
            {
                StringBuilder sb = new StringBuilder();
                string Reset = "\u001b[0m"; string Rouge = "\u001b[91m"; string Vert = "\u001b[92m";
                string Jaune = "\u001b[93m"; string Bleu = "\u001b[94m"; string Rose = "\u001b[95m";
                string Cyan = "\u001b[96m"; string Gris = "\u001b[37m";

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

                for (int iLigne = 0; iLigne < nbLignes; iLigne++)
                {
                    sb.AppendLine(ligneSep);
                    for (int iEtage = 0; iEtage < 3; iEtage++)
                    {
                        sb.Append(Reset + "|");
                        for (int iColonne = 0; iColonne < nbColonnes; iColonne++)
                        {
                            string SelectCouleur = (iLigne == PointeurLigne && iColonne == PointeurColonne) ? "\u001b[41m" : "";
                            if (matriceEtat[iLigne, iColonne] > 0)
                            {
                                var item = fruits[Matrice[iLigne, iColonne]];
                                sb.Append(SelectCouleur + item.Couleur + item.Dessin[iEtage].PadRight(7) + Reset + "|");
                            }
                            else
                            {
                                var item = fruits[0];
                                sb.Append(SelectCouleur + item.Couleur + item.Dessin[iEtage].PadRight(7) + Reset + "|");
                            }
                        }
                        sb.Append("\n");
                    }
                }
                sb.AppendLine(ligneSep);
                return sb.ToString();
            }

            static void GenerationMatrice2(int nbPaire, out int[,] matriceCarte, out int[,] matriceNombre, out int[,] matriceEtat)
            {
                int totalCartes = nbPaire * 2;
                int lignes = (int)Math.Sqrt(totalCartes);
                while (totalCartes % lignes != 0) { lignes--; }
                int colonnes = totalCartes / lignes;

                matriceCarte = new int[lignes, colonnes];
                matriceEtat = new int[lignes, colonnes];
                matriceNombre = new int[nbPaire, 2];
                Random alea = new Random();

                for (int i = 0; i < nbPaire; i++) { matriceNombre[i, 0] = i + 1; matriceNombre[i, 1] = i + 1; }

                for (int i = 0; i < nbPaire; i++)
                    for (int j = 0; j < 2; j++)
                    {
                        int rL, rC;
                        do { rL = alea.Next(0, lignes); rC = alea.Next(0, colonnes); } while (matriceCarte[rL, rC] != 0);
                        matriceCarte[rL, rC] = matriceNombre[i, j];
                    }
            }

            static bool VerifierPaire(int[,] matriceCarte, int[,] matriceEtat)
            {
                List<int> valeurs = new List<int>();
                for (int iLigne = 0; iLigne < matriceCarte.GetLength(0); iLigne++)
                    for (int iColonne = 0; iColonne < matriceCarte.GetLength(1); iColonne++)
                        if (matriceEtat[iLigne, iColonne] == 2)
                            valeurs.Add(matriceCarte[iLigne, iColonne]);
                return valeurs.Count == 2 && valeurs[0] == valeurs[1];
            }

            static bool ToutesTrouvees(int[,] matriceEtat)
            {
                foreach (int v in matriceEtat) if (v == 0) return false;
                return true;
            }
        }
    }
}