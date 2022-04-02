using System;

namespace Chardonnay1erCru {

    internal class Program {

        static void Main(string[] args) {

            // On initialize l'intramuros et la connexion au serveur de jeu
            //InNetworkManager inManager = new InNetworkManager();
            NetworkManager manager = new NetworkManager();

            manager.Join();
            manager.WaitForTurn();
            manager.Sommet();

            foreach (Card bouteille in manager.WinesSommet) Console.WriteLine(bouteille);

            Console.ReadLine();

        }

    }

}
