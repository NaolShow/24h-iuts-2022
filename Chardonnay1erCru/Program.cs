using System;

namespace Chardonnay1erCru {

    internal class Program {

        static void Main(string[] args) {

            // On initialize l'intramuros et la connexion au serveur de jeu
            //InNetworkManager inManager = new InNetworkManager();
            NetworkManager manager = new NetworkManager();

            manager.Join();

            while (true) {

                Console.WriteLine("Attente de mon tour...");
                manager.WaitForTurn();
                Console.WriteLine("Début du tour");

                Console.WriteLine("Carte prise: " + manager.Pick.Get(0));
                Console.WriteLine("Carte prise: " + manager.Pick.Get(1));

                manager.PasserTour();
                Console.ReadLine();

            }

            Console.ReadLine();

        }

    }

}
