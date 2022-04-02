using System;
using System.Collections.Generic;

namespace Chardonnay1erCru {

    internal class Program {

        static void Main(string[] args) {

            try {

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

                    // Si on a une bouteille vide on produit
                    bool producted = false;
                    if (manager.Deck.HaveEmptyBottle) {

                        foreach (Card card in manager.Deck.Cards) {

                            if (card.IsGrape && card.Type == CardType.Aligote) {

                                Console.WriteLine("ON POSE DU " + card);
                                manager.Poser(card);
                                producted = true;
                                break;
                            }

                        }

                    }

                    if (!producted) Console.WriteLine("Carte prise: " + manager.Pick.Get(1));

                    // PHASE 3
                    // => On passe son tour ou on défausse

                    List<int> toDefauss = new List<int>();
                    for (int i = 0; i < manager.Deck.Cards.Count; i++) {
                        break;
                        if (manager.Deck.Cards[i].IsGrape && manager.Deck.Cards[i].Type == CardType.Aligote) {

                            toDefauss.Add(i);
                            if (toDefauss.Count >= 2) break;

                        }
                    }

                    if (toDefauss.Count == 0) manager.PasserTour();
                    else if (toDefauss.Count == 1) manager.Defausser(toDefauss[0]);
                    else if (toDefauss.Count == 2) manager.Defausser(toDefauss[0], toDefauss[1]);

                    Console.ReadLine();

                }

                Console.ReadLine();

            } catch (Exception ex) {

                Console.WriteLine(ex.StackTrace);
                Console.ReadLine();

            }



        }

    }

}
