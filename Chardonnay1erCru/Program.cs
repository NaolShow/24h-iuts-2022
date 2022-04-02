using System;

namespace Chardonnay1erCru {

    internal class Program {

        static void Main(string[] args) {

            // On initialise la connexion au serveur de jeu et on s'inscrit
            NetworkManager manager = new NetworkManager();
            manager.Join();

            while (true) {

                // On attends le début de notre tour de jeu
                manager.WaitForTurn();

                // PHASE 1
                // => On récupère la carte du milieu
                Console.WriteLine("Carte prise: " + manager.Pick.Get(1));

                // PHASE 2
                // => On récupère la carte du milieu
                Console.WriteLine("Carte prise: " + manager.Pick.Get(1));

                // PHASE 3
                // => On passe son tour ou on défausse
                manager.PasserTour();

            }

            Console.ReadLine();

        }

    }

}
